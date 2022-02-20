namespace KMI.Sim
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    [Serializable]
    public class Entity : ActiveObject
    {
        protected KMI.Sim.ComplaintBuffer complaintBuffer;
        protected GeneralLedger gl;
        protected long iD;
        protected KMI.Sim.Journal journal;
        protected string name;
        protected KMI.Sim.Player player;
        protected Hashtable reserved;

        public Entity(KMI.Sim.Player player, string name)
        {
            Simulator instance = Simulator.Instance;
            this.player = player;
            this.name = name;
            this.ID = S.State.GetNextID();
            this.journal = new KMI.Sim.Journal(name, KMI.Sim.Journal.JournalNumericDataSeriesNames, (float) KMI.Sim.Journal.JournalDaysPerPeriod);
            this.complaintBuffer = new KMI.Sim.ComplaintBuffer(this);
            foreach (string str in this.journal.NumericDataSeriesNames)
            {
                for (int i = 0; i < S.State.CurrentWeek; i++)
                {
                    this.journal.AddNumericData(str, 0f);
                }
            }
        }

        public virtual float CriticalResourceBalance()
        {
            return this.GL.AccountBalance("Cash");
        }

        public override void Retire()
        {
            base.Retire();
            S.State.Entity.Remove(this.ID);
            S.State.RetiredEntity.Add(this.ID, this);
            if (this.GL.AccountBalance("Cash") <= 0f)
            {
                if (this.AI)
                {
                    object[] objArray1 = new object[] { S.Instance.EntityName.ToLower(), this.Name };
                    PlayerMessage.Broadcast(S.Resources.GetString("The {0} named {1} has gone out of business!!", objArray1), "", NotificationColor.Green);
                }
                else
                {
                    object[] objArray2 = new object[] { S.Instance.EntityName.ToLower(), this.Name };
                    PlayerMessage.BroadcastAllBut(this.Player.PlayerName, S.Resources.GetString("The {0} named {1} has gone out of business!!", objArray2), "", NotificationColor.Green);
                    if (S.State.EntityCount(this.Player) > 0)
                    {
                        object[] objArray3 = new object[] { S.Instance.EntityName.ToLower(), this.Name };
                        this.Player.SendModalMessage(S.Resources.GetString("Your {0} named {1} has run out of cash and is now out of business. Keep a close eye on your existing businesses! The net worth of the {0} (positive or negative) will be transferred to your remaining {0}(s).", objArray3), S.Resources.GetString("Out of Business"), MessageBoxIcon.Exclamation);
                        this.TransferNetWorthUponRetirement();
                    }
                    else
                    {
                        object[] objArray4 = new object[] { S.Instance.EntityName.ToLower() };
                        this.Player.SendModalMessage(new GameOverMessage(this.Player.PlayerName, S.Resources.GetString("Your {0} has run out of cash and is now out of business! That's all part of learning. Give it another try!", objArray4)));
                    }
                }
            }
            else if (S.State.EntityCount(this.Player) == 0)
            {
                object[] objArray5 = new object[] { S.Instance.EntityName.ToLower() };
                this.Player.SendModalMessage(new GameOverMessage(this.Player.PlayerName, S.Resources.GetString("You have closed your only {0}. This simulation is over. Give it another try!", objArray5)));
            }
            else
            {
                object[] objArray6 = new object[] { S.Instance.EntityName.ToLower(), this.Name };
                this.Player.SendModalMessage(S.Resources.GetString("The net worth of the {0} (positive or negative) will be transferred to your remaining {0}(s).", objArray6), S.Resources.GetString("Business Closed"), MessageBoxIcon.Exclamation);
                this.TransferNetWorthUponRetirement();
            }
            string[] args = new string[] { S.Instance.EntityName.ToLower(), this.Name.ToLower(), S.State.Now.ToLongDateString() };
            this.Journal.AddEntry(S.Resources.GetString("Closed {0} named {1} on {2}.", args));
        }

        public void Retire(ModalMessage message)
        {
            base.Retire();
            S.State.Entity.Remove(this.ID);
            S.State.RetiredEntity.Add(this.ID, this);
            this.Player.SendModalMessage(message);
        }

        protected virtual void TransferNetWorthUponRetirement()
        {
            Entity[] playersEntities = S.State.GetPlayersEntities(this.Player.PlayerName);
            float num = this.GL.AccountBalance("Cash") - this.GL.AccountBalance("Liabilities", S.State.CurrentWeek - 1);
            float amount = num / ((float) Math.Max(1, playersEntities.Length));
            foreach (Entity entity in playersEntities)
            {
                entity.GL.Post("Cash", amount, "Paid-in Capital");
            }
        }

        public bool AI
        {
            get
            {
                return (this.Player.PlayerType == PlayerType.AI);
            }
        }

        public KMI.Sim.ComplaintBuffer ComplaintBuffer
        {
            get
            {
                return this.complaintBuffer;
            }
        }

        public GeneralLedger GL
        {
            get
            {
                return this.gl;
            }
            set
            {
                this.gl = value;
            }
        }

        public long ID
        {
            get
            {
                return this.iD;
            }
            set
            {
                this.iD = value;
            }
        }

        public KMI.Sim.Journal Journal
        {
            get
            {
                return this.journal;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public KMI.Sim.Player Player
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

        public bool Retired
        {
            get
            {
                return S.State.RetiredEntity.ContainsValue(this);
            }
        }
    }
}

