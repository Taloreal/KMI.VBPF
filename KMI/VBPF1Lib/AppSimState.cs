namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;

    [Serializable]
    public class AppSimState : SimState
    {
        public ArrayList AllPurchasables;
        protected float amp;
        public AppCity City;
        public int DayCount;
        public float ElectricityCost;
        public KMI.VBPF1Lib.God God;
        public float InternetCost;
        public ArrayList MutualFunds;
        public SortedList OneTimeEvents;
        protected float period;
        protected float phase;
        public ArrayList PurchasableAutoSupplies;
        public ArrayList PurchasableBusTokens;
        public ArrayList PurchasableCars;
        public ArrayList PurchasableFood;
        public ArrayList PurchasableItems;
        public Surprise[] Surprises;
        public object[] ViewerOptions1;

        public AppSimState(SimSettings simSettings, bool multiplayer) : base(simSettings, multiplayer)
        {
            this.ElectricityCost = 47.5f;
            this.InternetCost = 45f;
            this.MutualFunds = new ArrayList();
            this.OneTimeEvents = new SortedList();
            this.DayCount = 0;
        }

        public void AddAIOneTimeEvent()
        {
            Dance dance = new Dance();
            dance.OneTimeDay = new DateTime(base.Now.Year, base.Now.Month, base.Now.Day).AddDays(21.0);
            dance.StartPeriod = 0x22;
            dance.EndPeriod = 0x2c;
            while (dance.Building == null)
            {
                AppBuilding randomBuilding = (AppBuilding) A.State.City.GetRandomBuilding(1);
                if ((randomBuilding != null) && (randomBuilding.Owner == null))
                {
                    dance.Building = randomBuilding;
                }
            }
            dance.AddAIAttendees(10);
            this.OneTimeEvents.Add(dance.Key, dance);
        }

        public Fund GetFund(string name)
        {
            foreach (Fund fund in this.MutualFunds)
            {
                if (fund.Name == name)
                {
                    return fund;
                }
            }
            return null;
        }

        public Task GetOneTimeEventByID(long id)
        {
            foreach (Task task in this.OneTimeEvents.Values)
            {
                if (task.ID == id)
                {
                    return task;
                }
            }
            return null;
        }

        public void GetPurchasables()
        {
            this.PurchasableItems = new ArrayList((PurchasableItem[]) TableReader.Read(base.GetType().Assembly, typeof(PurchasableItem), "KMI.VBPF1Lib.Data.PurchasableItems.txt"));
            this.PurchasableCars = new ArrayList((PurchasableItem[]) TableReader.Read(base.GetType().Assembly, typeof(PurchasableItem), "KMI.VBPF1Lib.Data.PurchasableCars.txt"));
            this.PurchasableFood = new ArrayList((PurchasableItem[]) TableReader.Read(base.GetType().Assembly, typeof(PurchasableItem), "KMI.VBPF1Lib.Data.PurchasableFood.txt"));
            this.PurchasableAutoSupplies = new ArrayList((PurchasableItem[]) TableReader.Read(base.GetType().Assembly, typeof(PurchasableItem), "KMI.VBPF1Lib.Data.PurchasableAutoSupplies.txt"));
            this.PurchasableBusTokens = new ArrayList((PurchasableItem[]) TableReader.Read(base.GetType().Assembly, typeof(PurchasableItem), "KMI.VBPF1Lib.Data.PurchasableBusTokens.txt"));
            this.AllPurchasables = new ArrayList();
            this.AllPurchasables.AddRange(this.PurchasableItems);
            this.AllPurchasables.AddRange(this.PurchasableCars);
            this.AllPurchasables.AddRange(this.PurchasableFood);
            this.AllPurchasables.AddRange(this.PurchasableAutoSupplies);
            this.AllPurchasables.AddRange(this.PurchasableBusTokens);
        }

        public override void Init()
        {
            base.Init();
            if (A.State.Multiplayer)
            {
                frmMultiplayerPlayers players = new frmMultiplayerPlayers();
                players.ShowDialog();
                A.Settings.ExpectedMultiplayerPlayers = players.NumPlayers;
            }
            this.phase = (float) ((A.State.Random.NextDouble() * 2.0) * 3.1415926535897931);
            this.period = A.State.Random.Next(360, 0x438);
            this.amp = (float) ((A.State.Random.NextDouble() * 0.02) + 0.02);
            string[] strArray = new string[] { "Select", "Vanyard", "Finality", "Oppenhizer", "FMS", "Sun", "Unity", "Watch Tower", "Highpoint", "Everlast", "Mercury", "Titan", "Oak", "SBU" };
            for (int i = 0; i < 14; i++)
            {
                this.MutualFunds.Add(new StockFund(strArray[i] + " US Stock Fund", 2f, ((8f + (((float) A.State.Random.NextDouble()) * 2f)) / 100f) / 365f));
                this.MutualFunds.Add(new StockFund(strArray[i] + " Intl Stock Fund", 3f, ((9f + (((float) A.State.Random.NextDouble()) * 2f)) / 100f) / 365f));
                this.MutualFunds.Add(new BondFund(strArray[i] + " Bond Fund", ((3f + (((float) A.State.Random.NextDouble()) * 1f)) / 100f) / 365f));
                this.MutualFunds.Add(new MoneyMarketFund(strArray[i] + " Money Market Fund", -((float) A.State.Random.NextDouble()) * 0.001f));
            }
            base.SimulatedTimePerStep = 0x4e20;
            this.GetPurchasables();
            this.UpdateCarAndGasData();
            this.City = new AppCity();
            this.God = new KMI.VBPF1Lib.God();
            if (A.State.Multiplayer)
            {
                A.Settings.Level = 2;
                A.Settings.Level = 3;
                A.Settings.LevelManagementOn = false;
            }
            else
            {
                A.Settings.Level = 1;
            }
        }

        public float PrimeRate()
        {
            return this.PrimeRate(A.State.Now);
        }

        public float PrimeRate(DateTime date)
        {
            TimeSpan span = (TimeSpan) (date - new DateTime(0x7d5, 1, 1));
            float days = span.Days;
            return (float) Math.Round((double) (0.05 + (this.amp * ((float) Math.Sin((double) ((this.phase + days) / this.period))))), 3);
        }

        public void UpdateCarAndGasData()
        {
            if (((PurchasableItem) this.PurchasableAutoSupplies[0]).Price < 30f)
            {
                foreach (PurchasableItem item in this.PurchasableAutoSupplies)
                {
                    if (item.Name.StartsWith("Gas"))
                    {
                        item.Price *= 1.5f;
                    }
                }
                int num = 0;
                foreach (PurchasableItem item2 in this.PurchasableCars)
                {
                    object[] args = new object[] { AppConstants.MPGs[num++] };
                    item2.Description = item2.Description + A.Resources.GetString(" MPG: {0}.", args);
                    item2.Description = item2.Description.Replace(" 100,000 mile bumper to bumper warranty.", "");
                }
            }
        }

        public int Period
        {
            get
            {
                int num = base.Hour * 2;
                if (base.Now.Minute >= 30)
                {
                    num++;
                }
                return num;
            }
        }

        public float RealEstateIndex
        {
            get
            {
                int num = (A.State.DayCount / 0x17) * 0x17;
                DateTime date = new DateTime(base.Year, base.Month, 0x11);
                return (1.5f * (200f + ((((num * 14f) / 365f) * this.PrimeRate(A.Settings.StartDate)) / this.PrimeRate(date))));
            }
        }

        public bool Weekend
        {
            get
            {
                return ((A.State.DayOfWeek == DayOfWeek.Saturday) || (A.State.DayOfWeek == DayOfWeek.Sunday));
            }
        }
    }
}

