namespace KMI.Sim
{
    using KMI.Sim.Academics;
    using System;
    using System.Collections;

    [Serializable]
    public class SimState
    {
        protected int currentWeek;
        protected Hashtable entity;
        internal SortedList eventQueue;
        public int FrameCounter;
        protected Guid guid;
        protected bool multiplayer;
        protected Hashtable multiplayerTeamPasswords;
        private bool newDay;
        private bool newHour;
        private bool newMonth;
        private bool newWeek;
        private bool newYear;
        protected long nextID;
        protected DateTime now;
        internal ArrayList[] perTimePeriodCollection;
        internal ArrayList[] perTimePeriodCollectionUpdated;
        protected Hashtable player;
        public int PowerLevel;
        public int PowerPoints;
        protected System.Random random;
        protected Hashtable reserved;
        protected Hashtable retiredEntity;
        protected bool roleBasedMultiplayer;
        protected DateTime runToDate;
        internal long SavedEntityID;
        internal string SavedViewName;
        protected KMI.Sim.SimSettings simSettings;
        private int simulatedTimePerStep;
        protected int speedIndex;
        protected SimSpeed[] speeds;

        public SimState()
        {
        }

        public SimState(KMI.Sim.SimSettings simSettings, bool multiplayer)
        {
            int num;
            this.multiplayer = false;
            this.roleBasedMultiplayer = false;
            this.simulatedTimePerStep = 0xea60;
            this.now = new DateTime();
            this.entity = new Hashtable();
            this.retiredEntity = new Hashtable();
            this.player = new Hashtable();
            this.currentWeek = 0;
            this.FrameCounter = 0;
            this.guid = Guid.NewGuid();
            this.perTimePeriodCollection = new ArrayList[5];
            this.perTimePeriodCollectionUpdated = new ArrayList[5];
            this.SavedViewName = "";
            this.SavedEntityID = -1L;
            this.eventQueue = new SortedList(new EventQueueComparer());
            this.nextID = 0L;
            this.multiplayerTeamPasswords = new Hashtable();
            this.PowerLevel = 0;
            this.PowerPoints = 0;
            this.runToDate = DateTime.MaxValue;
            if (simSettings.RandomSeed == -1)
            {
                this.random = new System.Random();
            }
            else
            {
                this.random = new System.Random(simSettings.RandomSeed);
            }
            this.simSettings = simSettings;
            this.multiplayer = multiplayer;
            for (num = 0; num < this.perTimePeriodCollection.Length; num++)
            {
                this.perTimePeriodCollection[num] = new ArrayList();
            }
            for (num = 0; num < this.perTimePeriodCollectionUpdated.Length; num++)
            {
                this.perTimePeriodCollectionUpdated[num] = new ArrayList();
            }
        }

        protected virtual void CreateSpeeds()
        {
            this.speeds = new SimSpeed[] {
                new SimSpeed(300, 1),
                new SimSpeed(125, 1),
                new SimSpeed(0, 1),
                new SimSpeed(0, 11),
                new SimSpeed(0, 101) };
        }

        public void DumpActiveObjectList()
        {
            Simulator.TimePeriod[] periodArray = new Simulator.TimePeriod[5];
            periodArray[1] = Simulator.TimePeriod.Hour;
            periodArray[2] = Simulator.TimePeriod.Day;
            periodArray[3] = Simulator.TimePeriod.Week;
            periodArray[4] = Simulator.TimePeriod.Year;
            Simulator.TimePeriod[] periodArray2 = periodArray;
            foreach (Simulator.TimePeriod period in periodArray2)
            {
                ArrayList list = this.perTimePeriodCollection[(int) period];
                foreach (ActiveObject obj2 in list)
                {
                    Console.WriteLine(obj2.ToString() + " " + period.ToString());
                }
            }
            foreach (ActiveObject obj3 in this.eventQueue.Values)
            {
                Console.WriteLine(obj3.ToString() + " sleeping");
            }
        }

        public void DumpActiveObjectList(Type type)
        {
            Simulator.TimePeriod[] periodArray = new Simulator.TimePeriod[5];
            periodArray[1] = Simulator.TimePeriod.Hour;
            periodArray[2] = Simulator.TimePeriod.Day;
            periodArray[3] = Simulator.TimePeriod.Week;
            periodArray[4] = Simulator.TimePeriod.Year;
            Simulator.TimePeriod[] periodArray2 = periodArray;
            foreach (Simulator.TimePeriod period in periodArray2)
            {
                ArrayList list = this.perTimePeriodCollection[(int) period];
                foreach (ActiveObject obj2 in list)
                {
                    if (obj2.GetType() == type)
                    {
                        Console.WriteLine(obj2.ToString() + " " + period.ToString());
                    }
                }
            }
            foreach (ActiveObject obj3 in this.eventQueue.Values)
            {
                if (obj3.GetType() == type)
                {
                    Console.WriteLine(obj3.ToString() + " sleeping");
                }
            }
        }

        public int EntityCount(KMI.Sim.Player player)
        {
            int num = 0;
            foreach (KMI.Sim.Entity entity in this.Entity.Values)
            {
                if (entity.Player == player)
                {
                    num++;
                }
            }
            return num;
        }

        public Hashtable EntityNameTable()
        {
            Hashtable hashtable = new Hashtable();
            foreach (KMI.Sim.Entity entity in S.State.Entity.Values)
            {
                hashtable.Add(entity.ID, entity.Name);
            }
            return hashtable;
        }

        public AcademicGod GetAcademicGod()
        {
            foreach (ArrayList list in this.perTimePeriodCollection)
            {
                foreach (ActiveObject obj2 in list)
                {
                    if (obj2 is AcademicGod)
                    {
                        return (AcademicGod) obj2;
                    }
                }
            }
            return null;
        }

        public KMI.Sim.Entity GetEntityByName(string name)
        {
            foreach (KMI.Sim.Entity entity in this.Entity.Values)
            {
                if (entity.Name == name)
                {
                    return entity;
                }
            }
            return null;
        }

        public string GetMultiplayerTeamPassword(string teamName)
        {
            return (string) this.multiplayerTeamPasswords[teamName.ToUpper()];
        }

        public long GetNextID()
        {
            return (this.nextID += 1L);
        }

        public KMI.Sim.Entity[] GetOtherEntities(string entityName)
        {
            ArrayList list = new ArrayList(this.Entity.Values);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (((KMI.Sim.Entity) list[i]).Name == entityName)
                {
                    list.RemoveAt(i);
                }
            }
            return (KMI.Sim.Entity[]) list.ToArray(typeof(KMI.Sim.Entity));
        }

        public KMI.Sim.Entity[] GetOtherPlayersEntities(string playerName)
        {
            ArrayList list = new ArrayList(this.Entity.Values);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (((KMI.Sim.Entity) list[i]).Player.PlayerName == playerName)
                {
                    list.RemoveAt(i);
                }
            }
            return (KMI.Sim.Entity[]) list.ToArray(typeof(KMI.Sim.Entity));
        }

        public KMI.Sim.Entity[] GetPlayersEntities(string playerName)
        {
            ArrayList list = new ArrayList();
            foreach (KMI.Sim.Entity entity in this.Entity.Values)
            {
                if (entity.Player.PlayerName.Equals(playerName))
                {
                    list.Add(entity);
                }
            }
            return (KMI.Sim.Entity[]) list.ToArray(typeof(KMI.Sim.Entity));
        }

        public KMI.Sim.Entity[] GetPlayersRetiredEntities(string playerName)
        {
            ArrayList list = new ArrayList();
            foreach (KMI.Sim.Entity entity in this.RetiredEntity.Values)
            {
                if (entity.Player.PlayerName.Equals(playerName))
                {
                    list.Add(entity);
                }
            }
            return (KMI.Sim.Entity[]) list.ToArray(typeof(KMI.Sim.Entity));
        }

        public virtual void Init()
        {
            this.now = this.simSettings.StartDate;
            this.Player.Add("", S.Instance.SimFactory.CreatePlayer("", PlayerType.Human));
            this.CreateSpeeds();
            this.SpeedIndex = 0;
        }

        public bool InSleepQueue(ActiveObject activeObject)
        {
            return this.eventQueue.Contains(activeObject);
        }

        public SortedList InspectSleepQueue()
        {
            return this.eventQueue;
        }

        internal void NewTimePeriod(Simulator.TimePeriod timePeriod)
        {
            int index = (int) timePeriod;
            while (this.perTimePeriodCollection[index].Count > 0)
            {
                ActiveObject obj2 = (ActiveObject) this.perTimePeriodCollection[(int) timePeriod][0];
                this.perTimePeriodCollectionUpdated[index].Add(obj2);
                this.perTimePeriodCollection[index].RemoveAt(0);
                switch (timePeriod)
                {
                    case Simulator.TimePeriod.Step:
                        if (obj2.NewStep())
                        {
                            this.UnSubscribeTimedEvent(obj2, Simulator.TimePeriod.Step);
                            this.eventQueue.Add(obj2, obj2);
                        }
                        break;

                    case Simulator.TimePeriod.Hour:
                        obj2.NewHour();
                        break;

                    case Simulator.TimePeriod.Day:
                        obj2.NewDay();
                        break;

                    case Simulator.TimePeriod.Week:
                        obj2.NewWeek();
                        break;

                    case Simulator.TimePeriod.Year:
                        obj2.NewYear();
                        break;

                    default:
                        throw new Exception("Unexpected time period.");
                }
            }
            this.perTimePeriodCollection[index] = (ArrayList) this.perTimePeriodCollectionUpdated[(int) timePeriod].Clone();
            this.perTimePeriodCollectionUpdated[index].Clear();
        }

        public float Season()
        {
            if (this.SimSettings.Seasonality)
            {
                return (float) Math.Sin((double) ((this.Now.Subtract(new DateTime(0x7d4, 1, 0x1f)).Days * 0.01721421f) + -1.570796f));
            }
            return 0f;
        }

        protected virtual void SetSpeed(SimSpeed speed)
        {
            S.Instance.SetSimEngineSpeed(speed);
        }

        public void Step()
        {
            int hour = this.Hour;
            int day = this.Day;
            System.DayOfWeek dayOfWeek = this.DayOfWeek;
            int month = this.Month;
            int year = this.Year;
            this.now = this.now.AddMilliseconds((double) this.simulatedTimePerStep);
            this.newHour = this.now.Hour != hour;
            this.newDay = this.now.Day != day;
            this.newWeek = (this.now.DayOfWeek == S.Instance.FirstDayOfWeek) && (dayOfWeek != S.Instance.FirstDayOfWeek);
            this.newMonth = this.now.Month != month;
            if (this.newWeek)
            {
                this.newYear = (this.currentWeek % 0x34) == 0;
                this.currentWeek++;
            }
            else
            {
                this.newYear = false;
            }
            if (this.FrameCounter == 0x7fffffff)
            {
                this.FrameCounter = 0;
            }
            else
            {
                this.FrameCounter++;
            }
        }

        internal void SubscribeTimedEvent(ActiveObject activeObject)
        {
            foreach (ArrayList list in this.perTimePeriodCollection)
            {
                if (!list.Contains(activeObject))
                {
                    list.Add(activeObject);
                }
            }
        }

        internal void SubscribeTimedEvent(ActiveObject activeObject, Simulator.TimePeriod timePeriod)
        {
            if (!this.perTimePeriodCollection[(int) timePeriod].Contains(activeObject))
            {
                this.perTimePeriodCollection[(int) timePeriod].Add(activeObject);
            }
        }

        internal void UnSubscribeTimedEvent(ActiveObject activeObject)
        {
            for (int i = 0; i < this.perTimePeriodCollection.Length; i++)
            {
                this.UnSubscribeTimedEvent(activeObject, (Simulator.TimePeriod) i);
            }
        }

        internal void UnSubscribeTimedEvent(ActiveObject activeObject, Simulator.TimePeriod timePeriod)
        {
            this.perTimePeriodCollection[(int) timePeriod].Remove(activeObject);
            this.perTimePeriodCollectionUpdated[(int) timePeriod].Remove(activeObject);
            if (timePeriod == Simulator.TimePeriod.Step)
            {
                int index = this.eventQueue.IndexOfValue(activeObject);
                if (index > -1)
                {
                    this.eventQueue.RemoveAt(index);
                }
            }
        }

        internal void UpdateEventQueue()
        {
            int count = this.eventQueue.Count;
            DateTime now = Simulator.Instance.SimState.Now;
            for (int i = 0; i < count; i++)
            {
                ActiveObject byIndex = (ActiveObject) this.eventQueue.GetByIndex(0);
                if (byIndex.WakeupTime > now)
                {
                    break;
                }
                this.eventQueue.RemoveAt(0);
                this.perTimePeriodCollection[0].Add(byIndex);
            }
        }

        public bool ValidateMultiplayerTeamPassword(string teamName, string password)
        {
            if (!this.multiplayerTeamPasswords.ContainsKey(teamName.ToUpper()))
            {
                this.multiplayerTeamPasswords.Add(teamName.ToUpper(), password);
                return true;
            }
            return (password.ToUpper() == ((string) this.multiplayerTeamPasswords[teamName.ToUpper()]).ToUpper());
        }

        public int AIPlayerCount
        {
            get
            {
                int num = 0;
                foreach (KMI.Sim.Entity entity in this.Entity.Values)
                {
                    if (entity.Player.PlayerType == PlayerType.Human)
                    {
                        num++;
                    }
                }
                return num;
            }
        }

        public int CurrentWeek
        {
            get
            {
                return this.currentWeek;
            }
            set
            {
                this.currentWeek = value;
            }
        }

        public int Day
        {
            get
            {
                return this.now.Day;
            }
        }

        public System.DayOfWeek DayOfWeek
        {
            get
            {
                return this.now.DayOfWeek;
            }
        }

        public Hashtable Entity
        {
            get
            {
                return this.entity;
            }
            set
            {
                this.entity = value;
            }
        }

        public Guid GUID
        {
            get
            {
                return this.guid;
            }
        }

        public int Hour
        {
            get
            {
                return this.now.Hour;
            }
        }

        public int Month
        {
            get
            {
                return this.now.Month;
            }
        }

        public bool Multiplayer
        {
            get
            {
                return this.multiplayer;
            }
            set
            {
                this.multiplayer = value;
            }
        }

        public bool NewDay
        {
            get
            {
                return this.newDay;
            }
        }

        public bool NewHour
        {
            get
            {
                return this.newHour;
            }
        }

        public bool NewMonth
        {
            get
            {
                return this.newMonth;
            }
        }

        public bool NewWeek
        {
            get
            {
                return this.newWeek;
            }
        }

        public bool NewYear
        {
            get
            {
                return this.newYear;
            }
        }

        public DateTime Now
        {
            get
            {
                return this.now;
            }
        }

        public Hashtable Player
        {
            get
            {
                return this.player;
            }
            set
            {
                this.player = value;
            }
        }

        public System.Random Random
        {
            get
            {
                return this.random;
            }
        }

        public Hashtable Reserved
        {
            get
            {
                if (this.reserved == null)
                {
                    this.reserved = new Hashtable();
                }
                return this.reserved;
            }
        }

        public Hashtable RetiredEntity
        {
            get
            {
                return this.retiredEntity;
            }
            set
            {
                this.retiredEntity = value;
            }
        }

        public bool RoleBasedMultiplayer
        {
            get
            {
                return this.roleBasedMultiplayer;
            }
            set
            {
                this.roleBasedMultiplayer = value;
            }
        }

        public DateTime RunToDate
        {
            get
            {
                return this.runToDate;
            }
            set
            {
                this.runToDate = value;
            }
        }

        public KMI.Sim.SimSettings SimSettings
        {
            get
            {
                return this.simSettings;
            }
        }

        public int SimulatedTimePerStep
        {
            get
            {
                return this.simulatedTimePerStep;
            }
            set
            {
                this.simulatedTimePerStep = value;
            }
        }

        public int SpeedIndex
        {
            get
            {
                return this.speedIndex;
            }
            set
            {
                this.speedIndex = value;
                this.SetSpeed(this.speeds[this.speedIndex]);
            }
        }

        public SimSpeed[] Speeds
        {
            get
            {
                return this.speeds;
            }
        }

        public int Year
        {
            get
            {
                return this.now.Year;
            }
        }

        [Serializable]
        internal class EventQueueComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (((ActiveObject) x).WakeupTime > ((ActiveObject) y).WakeupTime)
                {
                    return 1;
                }
                return -1;
            }
        }
    }
}

