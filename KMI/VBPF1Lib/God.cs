namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Windows.Forms;

    [Serializable]
    public class God : ActiveObject
    {
        public int currentPeriod = -1;
        private Random InflationRandomizer = new Random(0x100014);

        public God()
        {
            A.Instance.Subscribe(this, Simulator.TimePeriod.Week);
            A.Instance.Subscribe(this, Simulator.TimePeriod.Day);
            A.Instance.Subscribe(this, Simulator.TimePeriod.Step);
            A.Instance.Subscribe(this, Simulator.TimePeriod.Year);
        }

        public void fixIR() {
            InflationRandomizer = new Random(0x100014);
        }

        public void DoInflation() {
            double percent = this.NextInflation(1.01, 1.05);
            double num3 = percent * 100.0;
            PlayerMessage.Broadcast(A.Resources.GetString("Some prices and rents may have risen due to inflation by " + num3.ToString() + "%!"), "", NotificationColor.Yellow);
            foreach (PurchasableItem item in A.State.AllPurchasables) {
                item.Price = this.Inflate(item.Price, percent);
            }
            float num2 = this.Inflate(A.State.ElectricityCost, percent);
            A.State.ElectricityCost = this.Inflate(A.State.ElectricityCost, percent);
            num2 = this.Inflate(A.State.InternetCost, percent);
            A.State.InternetCost = (num2 <= 55f) ? num2 : 55f;
            foreach (Offering offering in A.State.City.GetOfferings())
            {
                if (offering is DwellingOffer)
                {
                    DwellingOffer offer = (DwellingOffer) offering;
                    if ((!offer.Taken || offer.Condo) || (((Dwelling) offer.Building).MonthsLeftOnLease <= 0))
                    {
                        offering.Building.Rent = (int) this.Inflate((float) offering.Building.Rent, percent);
                    }
                }
                if (offering is Course)
                {
                    ((Course) offering).Cost = this.Inflate(((Course) offering).Cost, percent);
                }
            }
        }

        public float Inflate(float amount)
        {
            return (float) Math.Round((double) (amount * (1f + A.Settings.InflationRate)), 2);
        }

        public float Inflate(float amount, double percent)
        {
            return (float) Math.Round((double) (amount * percent), 2);
        }

        protected void ManageLevels()
        {
            AppEntity entity = (AppEntity) A.State.Entity[A.MainForm.CurrentEntityID];
            if (entity != null)
            {
                float num = entity.CriticalResourceBalance();
                if ((A.Settings.Level == 1) && (num > 25000f))
                {
                    entity.Player.SendModalMessage(A.Resources.GetString("Congratulations! You have reached Level 2. In this level, some of the jobs in the city will add 401K retirement plans and health benefits! You will also be able to use online banking to simplify your life, even better food no longer expires. You now have one additional health factor to consider. You must maintain an active social life by hosting and attending gatherings with your friends! Good luck."), A.Resources.GetString("Congratulations!"), MessageBoxIcon.Exclamation);
                    A.Settings.Level = 2;
                    A.MainForm.EnableDisable();
                }
                else if ((A.Settings.Level == 2) && (num > 100000f))
                {
                    entity.Player.SendModalMessage(A.Resources.GetString("Congratulations! You have reached Level 3, the final level. In this level your goal is to continue to build your wealth. You can now purchase condominiums (real estate). You can live in these or just purchase them just for investments."), A.Resources.GetString("Congratulations!"), MessageBoxIcon.Exclamation);
                    A.Settings.Level = 3;
                    A.MainForm.EnableDisable();
                }
            }
        }

        public override void NewDay()
        {
            AppEntity current;
            AppSimState sT = A.State;
            sT.DayCount++;
            if ((A.State.Day == 0x1c) && A.Settings.PayBillsEnabledForOwner)
            {
                PlayerMessage.Broadcast(A.Resources.GetString("It's near the end of the month. Keep an eye out for new bills on your desk."), "", NotificationColor.Yellow);
            }
            SortedList list = new SortedList(A.State.OneTimeEvents);
            foreach (OneTimeEvent event2 in list.Values)
            {
                if (A.State.Now > event2.OneTimeDay)
                {
                    A.State.OneTimeEvents.Remove(event2.Key);
                    IEnumerator enumerator = A.State.Entity.Values.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        current = (AppEntity) enumerator.Current;
                        current.OneTimeEvents.Remove(event2.Key);
                    }
                }
            }
            foreach (OneTimeEvent event3 in A.State.OneTimeEvents.Values)
            {
                if (A.State.Now.AddDays(1.0) > event3.OneTimeDay)
                {
                    current = (AppEntity) A.State.Entity[event3.HostID];
                    if (current != null)
                    {
                        float num = 600f;
                        float num2 = 0f;
                        foreach (PurchasableItem item in current.PartyFood)
                        {
                            num2 += item.Price;
                        }
                        if (current.Has("DDR"))
                        {
                            num2 += 200f;
                        }
                        if (current.Has("Stereo"))
                        {
                            num2 += (current.ImageIndexOf("Stereo") + 1) * 0x42;
                        }
                        event3.AddAIAttendees((int) (Math.Min((float) 1f, (float) (num2 / num)) * 40f));
                    }
                }
            }
            if (A.Settings.AIParties && (A.State.Random.Next(7) == 0))
            {
                A.State.AddAIOneTimeEvent();
            }
            foreach (AppEntity entity2 in A.State.Entity.Values)
            {
                foreach (Surprise surprise in entity2.Surprises)
                {
                    surprise.CheckFireSurprise(entity2);
                }
            }
            foreach (AppEntity entity3 in A.State.Entity.Values)
            {
                if (A.Settings.BreakInDate.ToShortDateString() == A.State.Now.ToShortDateString())
                {
                    entity3.Surprises[3].FireSurprise(entity3);
                }
            }
            if (A.Settings.LevelManagementOn)
            {
                this.ManageLevels();
            }
            if (((A.Settings.Level > 1) && (A.State.Day == 1)) && ((A.State.Month % 12) == 8))
            {
                A.State.City.AddHealthInsurance(0.1f);
            }
            if (((A.Settings.Level > 1) && (A.State.Day == 1)) && ((A.State.Month % 12) == 4))
            {
                A.State.City.Add401Ks(0.1f);
            }
            if ((A.Settings.LevelManagementOn && (A.State.Day == 1)) && ((A.State.Month % 12) == 2))
            {
                A.State.City.RaiseSomeWages(0.075f);
            }
        }

        public override bool NewStep()
        {
            int period = A.State.Period;
            if (period != this.currentPeriod)
            {
                foreach (AppEntity entity in A.State.Entity.Values)
                {
                    entity.NewPeriod();
                }
            }
            this.currentPeriod = period;
            return false;
        }

        public override void NewWeek()
        {
            if (A.Settings.Sales && A.Settings.ShopForGoodsEnabledForOwner)
            {
                ArrayList buildings = A.State.City.GetBuildings();
                foreach (AppBuilding building in buildings)
                {
                    if (building.BuildingType.Index == 12)
                    {
                        building.SaleDiscounts = new float[building.SaleDiscounts.Length];
                        int maxValue = 8;
                        if (A.State.Reserved["SaleEvery"] != null)
                        {
                            maxValue = (int) A.State.Reserved["SaleEvery"];
                        }
                        if (A.State.Random.Next(maxValue) == 0)
                        {
                            bool flag5 = false;
                            for (int i = 0; i < building.SaleDiscounts.Length; i++)
                            {
                                if (A.State.Random.NextDouble() < 0.25)
                                {
                                    building.SaleDiscounts[i] = (float) ((A.State.Random.NextDouble() * 0.4) + 0.1);
                                    flag5 = true;
                                }
                            }
                            if (flag5)
                            {
                                object[] args = new object[] { building.OwnerName };
                                PlayerMessage.Broadcast(A.Resources.GetString("{0} is having a sale this week!", args), "", NotificationColor.Green);
                            }
                        }
                    }
                }
            }
        }

        public override void NewYear()
        {
            if ((A.State.CurrentWeek > 1) && (A.Settings.InflationRate > 0f))
            {
                this.DoInflation();
            }
        }

        public double NextInflation(double minp, double maxp)
        {
            int minValue = (int) (minp * 100.0);
            int maxValue = (int) (maxp * 100.0);
            double num3 = this.InflationRandomizer.Next(minValue, maxValue);
            return (num3 / 100.0);
        }
    }
}

