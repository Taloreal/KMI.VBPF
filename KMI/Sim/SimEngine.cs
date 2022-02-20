namespace KMI.Sim
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class SimEngine
    {
        private int count = 0;
        protected Thread mainThread;
        private object pauseLock = new object();
        protected bool running;
        protected Simulator simulator = Simulator.Instance;
        protected int skip;
        protected int stepPeriod;
        public bool stopEngine = false;

        public void Draw()
        {
            S.MainForm.UpdateView();
        }

        private void MainLoop()
        {
            while (!this.stopEngine)
            {
                lock (this.pauseLock)
                {
                    if (!this.running)
                    {
                        Monitor.Wait(this.pauseLock);
                    }
                }
                S.MainForm.PlayMacroAction();
                int tickCount = Environment.TickCount;
                lock (S.Adapter)
                {
                    if (!S.Instance.Client)
                    {
                        this.ProcessTick();
                    }
                }
                int millisecondsTimeout = (tickCount + this.stepPeriod) - Environment.TickCount;
                if (millisecondsTimeout > 0)
                {
                    Thread.Sleep(millisecondsTimeout);
                }
                if ((this.skip <= 0) || ((this.count++ % this.skip) == 0))
                {
                    try { S.MainForm.Invoke(new NoArgDelegate(this.Draw)); }
                    catch { stopEngine = true; }
                }
            }
            this.mainThread = null;
        }

        public void PauseThread()
        {
            this.running = false;
        }

        public void ProcessTick()
        {
            try
            {
                if ((S.State.Now >= S.Settings.StopDate) && (S.Settings.StopDate != DateTime.MinValue))
                {
                    Player player = (Player) S.State.Player[S.Instance.ThisPlayerName];
                    if (S.Instance.Demo)
                    {
                        S.MainForm.DirtySimState = false;
                        object[] args = new object[] { S.Instance.DemoDuration };
                        string message = S.Resources.GetString("Simulations in this demo version are limited to {0} simulated days. In the classroom version, simulations can run indefinitely.", args);
                        this.running = false;
                        player.SendModalMessage(new GameOverMessage(S.Instance.ThisPlayerName, message));
                    }
                    else if (S.Settings.StudentOrg > 0)
                    {
                        this.running = false;
                        player.SendModalMessage(new StopDateReachedMessage());
                    }
                }
                else if (S.State.Now >= S.State.RunToDate)
                {
                    this.running = false;
                    S.State.RunToDate = DateTime.MaxValue;
                    ((Player) S.State.Player[S.Instance.ThisPlayerName]).SendModalMessage(new RunToDateReachedMessage());
                }
                else
                {
                    SimState simState = Simulator.Instance.SimState;
                    simState.Step();
                    simState.UpdateEventQueue();
                    foreach (Simulator.TimePeriod period in Simulator.FiringOrder)
                    {
                        if (period == Simulator.TimePeriod.Step)
                        {
                            simState.NewTimePeriod(period);
                        }
                        else if ((period == Simulator.TimePeriod.Hour) && simState.NewHour)
                        {
                            simState.NewTimePeriod(period);
                        }
                        else if ((period == Simulator.TimePeriod.Day) && simState.NewDay)
                        {
                            simState.NewTimePeriod(period);
                        }
                        else if ((period == Simulator.TimePeriod.Week) && simState.NewWeek)
                        {
                            simState.NewTimePeriod(period);
                            //GC.Collect();
                        }
                        else if ((period == Simulator.TimePeriod.Year) && simState.NewYear)
                        {
                            simState.NewTimePeriod(period);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void ResumeThread()
        {
            object pauseLock = this.pauseLock;
            lock (pauseLock)
            {
                if (this.mainThread == null)
                {
                    this.mainThread = new Thread(new ThreadStart(this.MainLoop));
                    this.mainThread.IsBackground = true;
                    this.mainThread.Priority = ThreadPriority.Lowest;
                    this.stopEngine = false;
                    this.running = true;
                    this.mainThread.Start();
                }
                else
                {
                    this.running = true;
                    Monitor.Pulse(this.pauseLock);
                }
            }
        }

        public bool Running
        {
            get
            {
                return this.running;
            }
        }

        public int Skip
        {
            set
            {
                this.skip = value;
            }
        }

        public int StepPeriod
        {
            set
            {
                this.stepPeriod = value;
            }
        }

        public delegate void NoArgDelegate();
    }
}

