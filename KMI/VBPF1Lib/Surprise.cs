using System;
using System.Collections;

using KMI.Sim;

namespace KMI.VBPF1Lib {

    [Serializable]
    public class Surprise {


        public static bool Enabled = true;

        private static int CanNext = 31;


        private int _Level;
        public int Level { get { return _Level; } set { _Level = value; } }

        private float _MinSpacing;
        public float MinSpacing { get { return _MinSpacing; } set { _MinSpacing = value; } }

        private float _NetWorth;
        public float NetWorth { get { return _NetWorth; } set { _NetWorth = value; } }

        private float _LikelihoodPerDay;
        public float LikelihoodPerDay { get { return _LikelihoodPerDay; } set { _LikelihoodPerDay = value; } }

        private string _KeyName;
        public string KeyName { get { return _KeyName; } set { _KeyName = value; } }

        protected DateTime LastFired = DateTime.MinValue;


        public void CheckFireSurprise(AppEntity e) {
            if (CanNext > 0 || !Enabled) { CanNext--; return; }
            if (CorrectLvl(A.Settings.Level, Level)) {
                bool indexed = A.Settings.Surprises.IndexOf(KeyName) > -1;
                bool wealthyEnough = e.CriticalResourceBalance() > NetWorth;
                if (indexed && wealthyEnough) {
                    TimeSpan span = A.State.Now - LastFired;
                    bool spaced = span.Days > MinSpacing;
                    bool fired = A.State.Random.NextDouble() < LikelihoodPerDay;
                    if (spaced && fired) {
                        CanNext = 31;
                        FireSurprise(e);
                    }
                }
            }
        }

        private bool CorrectLvl(int current, int required) {
            return current >= required;
        }

        public void FireSurprise(AppEntity e) {
            switch (KeyName) {
                case "Health":          FireHealthCrisis(e);    return;
                case "Car Accident":    FireCarAccident(e);     return;
                case "Car Breakdown":   FireCarBreakdown(e);    return;
                case "Robbery":         FireRobbery(e);         return;
                case "Layoff":          FireLayoff(e);          return;
            }
            throw new Exception("Bad key name in surprises.");
        }

        protected void FireHealthCrisis(AppEntity entity) {
            InsurancePolicy bestHealthInsurance;
            int index = A.State.Random.Next(4);
            string[] situations = { "staph infection", "dislocated shoulder", "torn ligament", "ruptured appendix" };
            string str2 = situations[index].Split(new char[] { ' ' })[1];
            float amount = new float[] { 313f, 617f, 1326f, 4276f }[index];
            entity.Player.SendMessage("Oh, no! What bad luck! You have suffered from a " + situations[index] + ". " +
                "Fortunately, you were treated successfully at the hospital. " +
                "A bill from the hospital will be sent to you shortly!", "", NotificationColor.Red);
            BankAccount account = (BankAccount)entity.MerchantAccounts["Vincent Medical"];
            entity.AddBill(new Bill("Vincent Medical", "Treatment for " + str2, amount, account));
            bestHealthInsurance = entity.GetBestHealthInsurance();
            if ((bestHealthInsurance == entity.HealthInsurance) && entity.InsuranceOff) {
                entity.Player.SendMessage(
                    "The insurance company did not pay out because your policy was suspended due to lack of payment.", 
                    "", NotificationColor.Red);
            }
            else if (bestHealthInsurance.Copay > -1f) {
                account.Transactions.Add(new Transaction(amount - bestHealthInsurance.Copay, 
                    Transaction.TranType.Debit, "Insurance payment"));
            }
            LastFired = A.State.Now;
        }

        protected void FireCarAccident(AppEntity entity) {
            KMI.VBPF1Lib.Check check;
            bool hasCar = entity.Car != null;
            if (hasCar && !entity.Car.Broken) {
                entity.Player.SendMessage("Uh, oh! You've been in a car accident and your transmission is ruined. " +
                    "You can't drive it until you get it repaired. If you have insurance, you will receive a check " +
                    "for the repair cost less your deductible.", "", NotificationColor.Red);
                entity.Car.Broken = true;
                if (entity.Car.Insurance.Deductible > -1f) {
                    if (entity.InsuranceOff) {
                        entity.Player.SendMessage("The insurance company did not pay out because your " +
                            "policy was suspended due to lack of payment.", "", NotificationColor.Red);
                    }
                    else {
                        check = new KMI.VBPF1Lib.Check(-1L) {
                            Amount = ((PurchasableItem)A.State.PurchasableAutoSupplies[4]).Price - 
                                entity.Car.Insurance.Deductible,
                            Payee = entity.Name,
                            Date = A.State.Now,
                            Payor = "S&&W Insurance",
                            Number = (int)A.State.GetNextID(),
                            Memo = A.Resources.GetString("Car repairs"),
                            Signature = A.Resources.GetString("Samuel S. Steiner")
                        };
                        entity.AddCheck(check);
                    }
                }
                LastFired = A.State.Now;
            }
        }

        protected void FireCarBreakdown(AppEntity entity) {
            if (entity.Car != null) {
                entity.Player.SendPeriodicMessage("Getting a lube for your car every four months or so " +
                    "will help prevent costly breakdowns.", "", NotificationColor.Yellow, 365f);
                if (entity.Car.Broken) { return; }
                if (entity.Car.LeaseCost != 0f) { return; }
                if (A.State.Random.NextDouble() >= entity.Car.LikelihoodOfBreakDown()) { return; }
                entity.Player.SendMessage("Oh no! Your car has broken down because you failed to maintain it " +
                    "properly or it's getting old. You can't use it until you get it repaired at the auto garage.", 
                    "", NotificationColor.Red);
                entity.Car.Broken = true;
                LastFired = A.State.Now;
            }
        }

        protected void FireRobbery(AppEntity entity) {
            float cash = entity.Cash;
            foreach (PurchasableItem item in entity.PurchasedItems) {
                cash += item.Price;
            }
            if (cash > 0f) {
                entity.Player.SendMessage("Bad news! Your residence was broken into and everything was taken. " +
                    "If you have insurance, you will receive a check for the cost of items stolen (up to your " + 
                    "coverage limit) less your deductible.", "", NotificationColor.Red);
                entity.PurchasedItems.Clear();
                entity.Cash = 0f;
                InsurancePolicy insurance = ((DwellingOffer)entity.Dwelling.Offerings[0]).Condo ? 
                    entity.Dwelling.Insurance : entity.RentersInsurance;
                if ((insurance.Deductible > -1f) && (insurance.Limit > 0f)) {
                    if (entity.InsuranceOff) {
                        entity.Player.SendMessage("The insurance company did not pay out because your policy " +
                            "was suspended due to lack of payment.", "", NotificationColor.Red);
                    }
                    else {
                        Check check = new KMI.VBPF1Lib.Check(-1L) {
                            Amount = Math.Max((float)(Math.Min(cash, insurance.Limit) - insurance.Deductible), (float)0f),
                            Payee = entity.Name,
                            Date = A.State.Now,
                            Payor = "S&W Insurance",
                            Number = (int)A.State.GetNextID(),
                            Memo = A.Resources.GetString("Loss from Theft"),
                            Signature = A.Resources.GetString("Samuel S. Steiner")
                        };
                        if (check.Amount > 0f) { entity.AddCheck(check); }
                    }
                }
                LastFired = A.State.Now;
            }
        }

        protected void FireLayoff(AppEntity entity) {
            foreach (Task task in entity.GetAllTasks()) {
                if ((task is WorkTask) && !LastOfItsKind((WorkTask)task)) {
                    string msg = "Tough luck. Your employer, " + task.Building.OwnerName + ", has eliminated your " +
                        "position as a " + ((WorkTask)task).Name() + ". You have been laid off!";
                    entity.Player.SendMessage(msg, "", NotificationColor.Red);
                    A.Adapter.DeleteTask(entity.ID, task.ID);
                    A.State.City.FindOfferingForTask(task).Taken = true;
                    LastFired = A.State.Now;
                    break;
                }
            }
        }

        protected bool LastOfItsKind(WorkTask t) {
            Offering offering = A.State.City.FindOfferingForTask(t);
            ArrayList offerings = A.State.City.GetOfferings(offering.GetType());
            foreach (Offering offering2 in offerings) {
                if (!((((offering == offering2) || (offering2.PrototypeTask.StartPeriod != offering.PrototypeTask.StartPeriod)) || (offering2.PrototypeTask.GetType() != offering.PrototypeTask.GetType())) || offering2.Taken)) {
                    return false;
                }
            }
            return true;
        }
    }
}
