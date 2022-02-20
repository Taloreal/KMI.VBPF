using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    [Serializable]
    public class AppEntity : Entity {

        public SortedList Bills;
        public SortedList BankAccounts;
        public SortedList AcademicTaskHistory;
        public int BusTokens;
        public Car Car;
        public float Cash;
        public SortedList Checks;
        public SortedList ClosedBankAccounts;
        public SortedList CreditCardAccounts;
        public SortedList ClosedInstallmentLoans;
        public SortedList ClosedCreditCardAccounts;
        private DailyRoutine dailyRoutineWD;
        private DailyRoutine dailyRoutineWE;
        public long DDRLockedBy;
        public Dwelling Dwelling;
        public bool ElectricityOff;
        public Hashtable F1099s;
        public Hashtable FiredFrom;
        public List<FoodEntry> Food;
        public Hashtable FW2s;
        public float[][] HealthFactors;
        public InsurancePolicy HealthInsurance;
        public bool InsuranceOff;
        public bool Internet;
        public SortedList InvestmentAccounts;
        public DateTime LastDanced;
        public Hashtable LTCapitalGains;
        public Hashtable MerchantAccounts;
        protected int modeOfTransportation;
        public SortedList Mortgages;
        public SortedList OneTimeEvents;
        public ArrayList OneTimeEventTaskQueue;
        public Hashtable PartyAttendance;
        public ArrayList PartyFood;
        public ArrayList Payees;
        public VBPFPerson Person;
        public ArrayList PurchasedItems;
        public Hashtable Quit;
        public InsurancePolicy RentersInsurance;
        public SortedList RetirementAccounts;
        private static ScoreAdapter scoreAdapter;
        public bool Sick;
        public Hashtable STCapitalGains;
        public SortedList StudentLoans;
        public Surprise[] Surprises;
        public ArrayList TaxReturns;
        public DateTime timeLastAte;
        public float[] TodaysHealth;
        public SortedList WorkTaskHistory;

        public AppEntity(Player player, string name) : base(player, name)
        {
            this.MerchantAccounts = new Hashtable();
            this.Food = new List<FoodEntry>();
            this.timeLastAte = DateTime.MinValue;
            this.ElectricityOff = false;
            this.Internet = false;
            this.InsuranceOff = false;
            this.HealthFactors = new float[AppConstants.HealthFactorNames.Length][];
            this.TodaysHealth = new float[AppConstants.HealthFactorNames.Length];
            this.BankAccounts = new SortedList();
            this.CreditCardAccounts = new SortedList();
            this.StudentLoans = new SortedList();
            this.ClosedBankAccounts = new SortedList();
            this.ClosedCreditCardAccounts = new SortedList();
            this.ClosedInstallmentLoans = new SortedList();
            this.RentersInsurance = new InsurancePolicy(-1f, false, 0f);
            this.HealthInsurance = new InsurancePolicy(-1f);
            this.Mortgages = new SortedList();
            this.Payees = new ArrayList();
            this.TaxReturns = new ArrayList();
            this.dailyRoutineWD = new DailyRoutine();
            this.dailyRoutineWE = new DailyRoutine();
            this.Car = null;
            this.AcademicTaskHistory = new SortedList();
            this.modeOfTransportation = -1;
            this.PurchasedItems = new ArrayList();
            this.Sick = false;
            this.Checks = new SortedList();
            this.Bills = new SortedList();
            this.WorkTaskHistory = new SortedList();
            this.FW2s = new Hashtable();
            this.F1099s = new Hashtable();
            this.FiredFrom = new Hashtable();
            this.Quit = new Hashtable();
            this.STCapitalGains = new Hashtable();
            this.LTCapitalGains = new Hashtable();
            this.InvestmentAccounts = new SortedList();
            this.RetirementAccounts = new SortedList();
            this.BusTokens = 0;
            this.OneTimeEvents = new SortedList();
            this.OneTimeEventTaskQueue = new ArrayList();
            this.PartyAttendance = new Hashtable();
            this.PartyFood = new ArrayList();
            this.DDRLockedBy = -1L;
            this.Init();
        }

        public void AddBill(Bill bill)
        {
            long key = -1L;
            foreach (Bill bill2 in this.Bills.Values)
            {
                if (bill2.Account.ID == bill.Account.ID)
                {
                    key = bill2.ID;
                }
            }
            if (key > -1L)
            {
                this.Bills.Remove(key);
            }
            this.Bills.Add(bill.ID, bill);
            if (!this.Payees.Contains(bill.Account))
            {
                this.Payees.Add(bill.Account);
            }
        }

        public void AddCheck(KMI.VBPF1Lib.Check c)
        {
            this.Checks.Add(c.ID, c);
            object[] args = new object[] { c.Payor };
            base.Player.SendMessage(A.Resources.GetString("A check has arrived from {0}. It is on your desk. Click it to cash it or deposit it.", args), "", NotificationColor.Green);
        }

        public void AddFood(int meals) {
            this.Food.Add(new FoodEntry(meals, A.State.Now));
        }

        public bool AlreadyBilledToday(string name)
        {
            foreach (Bill bill in this.Bills.Values)
            {
                if ((bill.From == name) && (bill.Date == A.State.Now))
                {
                    return true;
                }
            }
            return false;
        }

        public void ApplyPaymentToAccount(long accountNumber, float amount)
        {
            BankAccount accountByAccountNumber = this.GetAccountByAccountNumber(accountNumber);
            if (accountByAccountNumber != null)
            {
                accountByAccountNumber.Transactions.Add(new Transaction(amount, Transaction.TranType.Debit, A.Resources.GetString("Payment-Thank You!")));
                BankAccount.Status status = accountByAccountNumber.PastDueStatusPassive(A.State.Year, A.State.Month);
                if (((accountByAccountNumber == this.MerchantAccounts["NRG Electric"]) && (status == BankAccount.Status.Current)) && this.ElectricityOff)
                {
                    base.Player.SendMessage("Your electricity has been turned back on!", "", NotificationColor.Green);
                    this.ElectricityOff = false;
                }
                if ((((accountByAccountNumber == this.MerchantAccounts["Internet Connect"]) && (status == BankAccount.Status.Current)) && !this.Internet) && base.Reserved.ContainsKey("WantsInternet"))
                {
                    base.Player.SendMessage("Your internet access has been turned back on!", "", NotificationColor.Green);
                    this.Internet = true;
                }
                if (((accountByAccountNumber == this.MerchantAccounts["S&W Insurance"]) && (status == BankAccount.Status.Current)) && this.InsuranceOff)
                {
                    base.Player.SendMessage("Your insurance coverage has been reinstated!", "", NotificationColor.Green);
                    this.InsuranceOff = false;
                }
                if ((accountByAccountNumber is InstallmentLoan) && (accountByAccountNumber.EndingBalance() <= 0.1))
                {
                    object[] args = new object[] { accountByAccountNumber.BankName };
                    base.Player.SendMessage(A.Resources.GetString("Congratulations! You have paid off your loan from {0}!", args), "", NotificationColor.Green);
                    this.CloseAccount(accountByAccountNumber);
                }
                if ((accountByAccountNumber is CreditCardAccount) && (((status == BankAccount.Status.NewlyCancelled) || (status == BankAccount.Status.Cancelled)) && (accountByAccountNumber.EndingBalance() <= 0f)))
                {
                    this.CreditCardAccounts.Remove(accountByAccountNumber);
                    this.CloseAccount(accountByAccountNumber);
                }
            }
        }

        public void AuditAndRefundOrBill() {
            TaxReturn return2 = null;
            AccountantsReport report;
            int year = A.State.Now.Year - 1;
            foreach (TaxReturn return3 in this.TaxReturns) {
                if (return3.Year == year) { return2 = return3; }
            }
            if (return2 is AccountantsReport) {
                report = (AccountantsReport) return2;
            }
            else {
                report = new AccountantsReport(year);
                report.PrepareTaxes(year, this, false);
            }
            BankAccount account = (BankAccount) this.MerchantAccounts["IRS"];
            object[] args = new object[] { year };
            account.Transactions.Add(new Transaction((float) report.Tax, Transaction.TranType.Credit, A.Resources.GetString("Tax due for {0}", args)));
            object[] objArray2 = new object[] { year };
            account.Transactions.Add(new Transaction((float) report.FedWT, Transaction.TranType.Debit, A.Resources.GetString("Tax withheld for {0}", objArray2)));
            int num2 = (int) account.EndingBalance();
            int num3 = 0;
            string s = "";
            if (return2 == null) {
                num3 += 1000;
                s = s + "failing to file a tax return, ";
            }
            else if (return2 is F1040EZ) {
                float num4;
                float num5 = return2.Values[6];
                if (return2.Values[7] > 0) {
                    num4 = -return2.Values[7];
                }
                else { num4 = return2.Values[8]; }
                if ((num4 < num2) || (num5 < report.Tax)) {
                    num3 += 100;
                    s = s + "Form 1040EZ under-reporting tax due, ";
                }
            }
            if ((num2 + num3) > 0) {
                object[] objArray3 = new object[] { Utilities.FC((float) num2, 0, A.Instance.CurrencyConversion), Utilities.FC((float) num3, 0, A.Instance.CurrencyConversion), Utilities.FormatCommaSeries(s) };
                base.Player.SendMessage(A.Resources.GetString("Uh, oh! Your tax return was audited. You owe {0} in taxes + {1} in penalities for {2}.", objArray3), A.Resources.GetString("Internal Revenue Service"), NotificationColor.Yellow);
                this.AddBill(new Bill("IRS", A.Resources.GetString("Penalties"), (float) num3, account));
            }
            if (((num2 + num3) < 0) && (return2 != null))
            {
                object[] objArray4 = new object[] { Utilities.FC((float) -num2, 0, A.Instance.CurrencyConversion) };
                string message = A.Resources.GetString("Your tax refund check has arrived!! Your refund was {0}.", objArray4);
                if (num3 > 0)
                {
                    object[] objArray5 = new object[] { Utilities.FC((float) num3, 0, A.Instance.CurrencyConversion), Utilities.FormatCommaSeries(s) };
                    message = message + A.Resources.GetString("However, you were charged {0} in penalties for {1}.", objArray5);
                }
                base.Player.SendMessage(message, A.Resources.GetString("Internal Revenue Service"), NotificationColor.Green);
                Check check2 = new Check(-1L) {
                    Amount = -(num2 + num3),
                    Payee = base.Name,
                    Date = A.State.Now,
                    Payor = "Internal Revenue Service",
                    Number = (int) A.State.GetNextID()
                };
                object[] objArray6 = new object[] { year };
                check2.Memo = A.Resources.GetString("Tax refund {0}", objArray6);
                check2.Signature = A.Resources.GetString("Uncle Sam");
                Check c = check2;
                this.AddCheck(c);
                if (num3 > 0) {
                    account.Transactions.Add(new Transaction((float) num3, Transaction.TranType.Credit, A.Resources.GetString("Penalties")));
                }
                object[] objArray7 = new object[] { year };
                account.Transactions.Add(new Transaction(c.Amount, Transaction.TranType.Credit, A.Resources.GetString("Tax refund for {0}", objArray7)));
            }
        }

        public void BillDunCancel() {
            GameOverMessage message;
            int year = A.State.Year;
            int month = A.State.Month;
            BankAccount account = (BankAccount) this.MerchantAccounts["City Property Mgt"];
            BankAccount.Status status = account.PastDueStatus(year, month);
            if (account.DaysPastDue(A.State.Now) > 120) {
                message = new GameOverMessage(base.Player.PlayerName, "You have been evicted for not paying your rent bills. It's game over. You can try again though!");
                this.RetireFromApp(message);
                return;
            }
            if ((((status == BankAccount.Status.PastDue) || (status == BankAccount.Status.Deliquent)) || (status == BankAccount.Status.NewlyCancelled)) || (status == BankAccount.Status.Cancelled)) {
                base.Player.SendMessage("You haven't been paying your rent or past lease obligations on time. You may be evicted. If you are evicted, it's sim over for you!", "", NotificationColor.Yellow);
            }
            if (!((DwellingOffer)this.Dwelling.Offerings[0]).Condo) {
                this.AddBill(new Bill(A.Resources.GetString("City Property Mgt"), "Monthly Lease", (float)this.Dwelling.Rent, (BankAccount)this.MerchantAccounts["City Property Mgt"]));
            }
            if (this.Dwelling.MonthsLeftOnLease > 0) {
                this.Dwelling.MonthsLeftOnLease--;
            }
            if (!this.ElectricityOff) {
                this.AddBill(new Bill("NRG Electric", "Kilowatts of Electricity", A.State.ElectricityCost, (BankAccount)this.MerchantAccounts["NRG Electric"]));
            }
            switch (((BankAccount)this.MerchantAccounts["NRG Electric"]).PastDueStatus(year, month)) {
                case BankAccount.Status.Deliquent:
                    base.Player.SendMessage("You have unpaid electricity  bills! If you don't pay them, your credit rating will decline and your electricity may be shut off!", "", NotificationColor.Yellow);
                    break;

                case BankAccount.Status.NewlyCancelled:
                    this.ElectricityOff = true;
                    base.Player.SendMessage("Your electricity has been turned off, because you didn't pay your bills! You cannot watch TV, access the Internet, and your food will spoil.", "", NotificationColor.Red);
                    break;
            }
            if (this.Internet) {
                this.AddBill(new Bill("Internet Connect", "Hi-speed Internet Access", A.State.InternetCost, (BankAccount)this.MerchantAccounts["Internet Connect"]));
            }
            status = ((BankAccount) this.MerchantAccounts["Internet Connect"]).PastDueStatus(year, month);
            if (status == BankAccount.Status.NewlyCancelled) {
                this.Internet = false;
                base.Player.SendMessage("Your internet access has been turned off, because you didn't pay your bills!", "", NotificationColor.Red);
            }
            else if (status == BankAccount.Status.Deliquent) {
                base.Player.SendMessage("You have unpaid internet access bills! If you don't pay them, your credit rating will decline and your internet access will be shut off!", "", NotificationColor.Yellow);
            }
            status = ((BankAccount) this.MerchantAccounts["Taranti Auto Lease"]).PastDueStatus(year, month);
            if ((status == BankAccount.Status.NewlyCancelled) && (this.Car != null)) {
                base.Player.SendMessage("You have failed to make your car payments. Your car has been repossessed.", "", NotificationColor.Red);
                this.RepossessCar();
            }
            else if ((status == BankAccount.Status.Deliquent) && (this.Car != null)) {
                base.Player.SendMessage("Your car payments are behind. Make your account current or your car will be repossessed.", "", NotificationColor.Yellow);
            }
            if ((this.Car != null) && (this.Car.Loan != null)) {
                switch (this.Car.Loan.PastDueStatus(year, month)) {
                    case BankAccount.Status.NewlyCancelled:
                        base.Player.SendMessage("You have failed to make your car payments. Your car has been repossessed.", "", NotificationColor.Red);
                        this.RepossessCar();
                        break;

                    case BankAccount.Status.Deliquent:
                        base.Player.SendMessage("Your car payments are behind. Make your account current or your car will be repossessed.", "", NotificationColor.Yellow);
                        break;
                }
            }
            if (this.Car != null) {
                if (this.Car.LeaseCost > 0f) {
                    this.AddBill(new Bill("Taranti Auto Lease", "Automobile Lease", this.Car.LeaseCost, (BankAccount)this.MerchantAccounts["Taranti Auto Lease"]));
                }
                if (this.Car.Loan != null) {
                    this.AddBill(new Bill("Taranti Auto Loan", "", this.Car.Loan.Payment + this.Car.Loan.PastDueAmount(A.State.Now), this.Car.Loan));
                }
            }
            foreach (CreditCardAccount account2 in this.CreditCardAccounts.Values) {
                switch (account2.PastDueStatus(year, month)) {
                    case BankAccount.Status.Deliquent: {
                            object[] args = new object[] { account2.BankName };
                            base.Player.SendMessage(A.Resources.GetString("Your credit card from {0} may soon be frozen due to lack of payments.", args), "", NotificationColor.Yellow);
                            break;
                        }
                    case BankAccount.Status.NewlyCancelled: {
                            object[] objArray1 = new object[] { account2.BankName };
                            base.Player.SendMessage(A.Resources.GetString("Your credit card from {0} has been frozen due to lack of payments.", objArray1), "", NotificationColor.Red);
                            break;
                        }
                }
                this.AddBill(new Bill(account2.BankName, "", account2.MinimumPayment(year, month), account2));
            }
            foreach (InstallmentLoan loan in this.StudentLoans.Values) {
                if (loan.PastDueStatus(year, month) == BankAccount.Status.NewlyCancelled) {
                    base.Player.SendMessage(A.Resources.GetString("Your student loan is now in default."), "", NotificationColor.Red);
                }
                if (A.State.Now > loan.BeginBilling) {
                    this.AddBill(new Bill("Quest Student Loans", "", loan.Payment + loan.PastDueAmount(A.State.Now), loan));
                }
            }
            foreach (BankAccount account3 in this.MerchantAccounts.Values) {
                if ((account3.EndingBalance() > 0f) && !this.AlreadyBilledToday(account3.BankName)) {
                    this.AddBill(new Bill(account3.BankName, "", 0f, account3));
                }
            }
            if (!this.InsuranceOff) {
                if (this.Car != null) {
                    this.AddBill(new Bill("S&W Insurance", "Car Insurance", this.Car.Insurance.MonthlyPremium, (BankAccount)this.MerchantAccounts["S&W Insurance"]));
                }
                if (this.HealthInsurance.Copay > -1f) {
                    this.AddBill(new Bill("S&W Insurance", "Health Insurance", this.HealthInsurance.MonthlyPremium, (BankAccount)this.MerchantAccounts["S&W Insurance"]));
                }
                if (this.RentersInsurance.Deductible > -1f) {
                    this.AddBill(new Bill("S&W Insurance", "Renter's Insurance", this.RentersInsurance.MonthlyPremium, (BankAccount)this.MerchantAccounts["S&W Insurance"]));
                }
                float amount = 0f;
                foreach (AppBuilding building in A.State.City.GetBuildings()) {
                    if ((building.Owner == this) && ((DwellingOffer)building.Offerings[0]).Condo) {
                        amount += ((KMI.VBPF1Lib.Dwelling)building).Insurance.MonthlyPremium;
                    }
                }
                if (amount > 0f) {
                    this.AddBill(new Bill("S&W Insurance", "Homeowner's Insurance", amount, (BankAccount)this.MerchantAccounts["S&W Insurance"]));
                }
            }
            status = ((BankAccount) this.MerchantAccounts["S&W Insurance"]).PastDueStatus(year, month);
            if (status == BankAccount.Status.NewlyCancelled) {
                this.InsuranceOff = true;
                base.Player.SendMessage("Your insurance coverage has been suspended, because you didn't pay your bills! Losses will not be covered.", "", NotificationColor.Red);
            }
            else if (status == BankAccount.Status.Deliquent) {
                base.Player.SendMessage("You have unpaid insurance  bills! If you don't pay them, your credit rating will decline and your insurance coverage may be suspended!", "", NotificationColor.Yellow);
            }
            foreach (Mortgage mortgage in this.Mortgages.Values) {
                switch (mortgage.PastDueStatus(year, month)) {
                    case BankAccount.Status.PastDue:
                    case BankAccount.Status.Deliquent:
                        base.Player.SendMessage("You haven't been paying your mortgage obligations on time. You may be evicted or face foreclosure. If you are evicted or foreclosed upon, it's sim over for you!", "", NotificationColor.Yellow);
                        break;

                    case BankAccount.Status.NewlyCancelled:
                        message = new GameOverMessage(base.Player.PlayerName, "You have been evicted or foreclosed upon for not paying your mortgage. It's game over. You can try again though!");
                        this.RetireFromApp(message);
                        return;
                }
                this.AddBill(new Bill("Century Mortgage", "", mortgage.Payment + mortgage.PastDueAmount(A.State.Now), mortgage));
            }
        }

        public void CheckValiditySynchTravel(int modeOfTransportation, DailyRoutine routine, Task changedTask, bool addTask) {
            ArrayList list = new ArrayList(routine.Tasks.Values);
            foreach (Task task in list) {
                if (task is TravelTask) {
                    routine.Tasks.Remove(task.StartPeriod);
                }
            }
            if (changedTask != null) {
                routine.CheckHoursConflict(changedTask);
                if (addTask) {
                    routine.Tasks.Add(changedTask.StartPeriod, changedTask);
                }
                else {
                    int key = -1;
                    foreach (int num2 in routine.Tasks.Keys) {
                        if (routine.Tasks[num2] == changedTask) {
                            key = num2;
                        }
                    }
                    routine.Tasks.Remove(key);
                    routine.Tasks.Add(changedTask.StartPeriod, changedTask);
                }
            }
            if ((routine.Tasks.Count > 1) && (modeOfTransportation != -1)) {
                ArrayList list2 = new ArrayList();
                foreach (Task task2 in routine.Tasks.Values) {
                    Task task3 = routine.PriorTask(task2);
                    AppBuilding from = task3.Building;
                    if (task2.Building != from) {
                        TravelTask task4 = TravelTask.CreateTravelTask(modeOfTransportation);
                        float num3 = ((float) (task4.EstTimeInSteps(from, task2.Building) * 0x4e20)) / 1800000f;
                        int num4 = (int) Math.Ceiling((double) num3);
                        task4.StartPeriod = task3.EndPeriod;
                        task4.EndPeriod = (task4.StartPeriod + num4) % 0x30;
                        task4.From = from;
                        task4.To = task2.Building;
                        task4.Building = task4.From;
                        task4.Owner = this.Person;
                        list2.Add(task4);
                    }
                }
                foreach (TravelTask task5 in list2) {
                    routine.Tasks.Add(task5.StartPeriod, task5);
                }
            }
            this.OneTimeEventTaskQueue.Clear();
            foreach (OneTimeEvent event2 in this.OneTimeEvents.Values) {
                TravelTask task6 = TravelTask.CreateTravelTask(modeOfTransportation);
                TravelTask task7 = TravelTask.CreateTravelTask(modeOfTransportation);
                OneTimeEvent event3 = (OneTimeEvent) Utilities.DeepCopyBySerialization(event2);
                event3.ID = A.State.GetNextID();
                task6.Owner = this.Person;
                task7.Owner = this.Person;
                event3.Owner = this.Person;
                event3.Building = event2.Building;
                task6.Building = this.Dwelling;
                task6.From = this.Dwelling;
                task6.To = event2.Building;
                task7.Building = event2.Building;
                task7.From = event2.Building;
                task7.To = this.Dwelling;
                DateTime time = new DateTime(event2.OneTimeDay.Year, event2.OneTimeDay.Month, event2.OneTimeDay.Day).AddHours((double) (((float) event2.StartPeriod) / 2f));
                task6.OneTimeDay = time.AddMinutes((double) (-task6.EstTimeInSteps(task6.From, task6.To) * (((float) A.State.SimulatedTimePerStep) / 60000f)));
                task6.StartPeriod = task6.OneTimeDay.Hour * 2;
                if (event2.StartPeriod < event2.EndPeriod) {
                    task7.OneTimeDay = event2.OneTimeDay;
                }
                else { task7.OneTimeDay = event2.OneTimeDay.AddDays(1.0); }
                task7.StartPeriod = event2.EndPeriod;
                Task[] c = new Task[] { task6, event3, task7 };
                this.OneTimeEventTaskQueue.AddRange(c);
            }
        }

        public void CloseAccount(BankAccount a)
        {
            foreach (BankAccount account in this.BankAccounts.Values)
            {
                if (account is CheckingAccount)
                {
                    foreach (RecurringPayment payment in ((CheckingAccount) account).RecurringPayments)
                    {
                        if (payment.PayeeAccountNumber == a.AccountNumber)
                        {
                            ((CheckingAccount) account).RecurringPayments.Remove(payment);
                            this.Payees.Remove(a);
                            break;
                        }
                    }
                }
            }
            a.DateClosed = A.State.Now;
            if (a is CreditCardAccount)
            {
                this.ClosedCreditCardAccounts.Add(a.AccountNumber, this.CreditCardAccounts[a.AccountNumber]);
                this.CreditCardAccounts.Remove(a.AccountNumber);
            }
            else if (a is InstallmentLoan)
            {
                foreach (BankAccount account2 in this.BankAccounts.Values)
                {
                    if (account2 is CheckingAccount)
                    {
                        foreach (RecurringPayment payment2 in ((CheckingAccount) account2).RecurringPayments)
                        {
                            if (payment2.PayeeAccountNumber == a.AccountNumber)
                            {
                                ((CheckingAccount) account2).RecurringPayments.Remove(payment2);
                                break;
                            }
                        }
                    }
                }
                if (((this.Car != null) && (this.Car.Loan != null)) && (this.Car.Loan.AccountNumber == a.AccountNumber))
                {
                    this.ClosedInstallmentLoans.Add(a.AccountNumber, this.Car.Loan);
                    this.Car.Loan = null;
                }
                else if (this.Mortgages.ContainsKey(a.AccountNumber))
                {
                    this.ClosedInstallmentLoans.Add(a.AccountNumber, this.Mortgages[a.AccountNumber]);
                    this.Mortgages.Remove(a.AccountNumber);
                }
                else
                {
                    this.ClosedInstallmentLoans.Add(a.AccountNumber, this.StudentLoans[a.AccountNumber]);
                    this.StudentLoans.Remove(a.AccountNumber);
                }
            }
            else if (a != null)
            {
                this.ClosedBankAccounts.Add(a.AccountNumber, this.BankAccounts[a.AccountNumber]);
                this.BankAccounts.Remove(a.AccountNumber);
                this.Cash += a.EndingBalance();
                foreach (Task task in this.GetAllTasks())
                {
                    if ((task is WorkTask) && (((WorkTask) task).DirectDepositAccount == a))
                    {
                        ((WorkTask) task).DirectDepositAccount = null;
                    }
                }
            }
        }

        public float ComputeRealEstateValue()
        {
            float num = 0f;
            foreach (AppBuilding building in A.State.City.GetBuildings())
            {
                if ((building.Owner == this) && ((DwellingOffer) building.Offerings[0]).Condo)
                {
                    num += building.Rent * A.State.RealEstateIndex;
                }
            }
            return num;
        }

        public int CreditScore()
        {
            return this.CreditScore(new ArrayList());
        }

        public int CreditScore(ArrayList reasons)
        {
            float num = 550f;
            foreach (BankAccount account in this.MerchantAccounts.Values)
            {
                if (account.DaysPastDue(A.State.Now) > 60)
                {
                    num -= 50f;
                }
            }
            ArrayList list = new ArrayList(A.Adapter.GetInstallmentLoans(base.ID).Values);
            list.AddRange(this.ClosedInstallmentLoans.Values);
            float num2 = 0f;
            foreach (InstallmentLoan loan in list)
            {
                if (loan.DateClosed > A.State.Now.AddMonths(-36))
                {
                    num2 -= 20 * loan.MissedPayments();
                    num2 += 5 * loan.OnTimePayments();
                }
            }
            num += Math.Min(150f, num2);
            ArrayList list2 = new ArrayList(this.CreditCardAccounts.Values);
            list2.AddRange(this.ClosedCreditCardAccounts.Values);
            float num3 = 0f;
            foreach (CreditCardAccount account2 in list2)
            {
                if (account2.DateClosed > A.State.Now.AddMonths(-36))
                {
                    num3 -= 20 * account2.MissedPayments();
                    num3 += 5 * account2.OnTimePayments();
                }
            }
            num += Math.Min(150f, num3);
            return (int) num;
        }

        public float CreditScoreNormalized()
        {
            return Utilities.Clamp(((float) (this.CreditScore(new ArrayList()) - 400)) / 450f);
        }

        public override float CriticalResourceBalance()
        {
            float cash = this.Cash;
            foreach (BankAccount account in this.BankAccounts.Values)
            {
                cash += account.EndingBalance();
            }
            float num2 = 0f;
            foreach (InvestmentAccount account2 in this.InvestmentAccounts.Values)
            {
                num2 += account2.Value;
            }
            foreach (InvestmentAccount account3 in this.RetirementAccounts.Values)
            {
                num2 += account3.Value;
            }
            float num3 = this.ComputeRealEstateValue();
            if ((this.Car != null) && (this.Car.LeaseCost == 0f))
            {
                num3 += this.Car.ComputeResalePrice(A.State.Now);
            }
            float num4 = 0f;
            foreach (CreditCardAccount account4 in this.CreditCardAccounts.Values)
            {
                num4 += account4.EndingBalance();
            }
            if (A.Settings.IncludeOtherLiabilities)
            {
                foreach (BankAccount account5 in this.MerchantAccounts.Values)
                {
                    if (account5.EndingBalance() > 0f)
                    {
                        num4 += account5.EndingBalance();
                    }
                }
            }
            float num5 = 0f;
            SortedList installmentLoans = this.GetInstallmentLoans();
            foreach (InstallmentLoan loan in installmentLoans.Values)
            {
                num5 += loan.EndingBalance();
            }
            return (((cash + num2) + num3) - (num4 + num5));
        }

        public float FICAPaidThisYear()
        {
            float num = 0f;
            foreach (WorkTask task in this.WorkTaskHistory.Values)
            {
                foreach (PayStub stub in task.PayStubs)
                {
                    if (stub.Year() == A.State.Now.Year)
                    {
                        num += stub.GetValue("Soc Sec");
                    }
                }
            }
            return num;
        }

        protected Task FindActualTravelTask(TravelTask originalTask)
        {
            bool flag = false;
            if (originalTask is TravelByCar)
            {
                KMI.VBPF1Lib.Car car = this.Car;
                ((TravelByCar) originalTask).Car = car;
                if (((car == null) || (car.Gas <= 0f)) || car.Broken)
                {
                    flag = true;
                }
                if ((car != null) && (car.Gas <= 0f))
                {
                    base.Player.SendPeriodicMessage("Your car is out of gas. You had to walk. Buy some gas at the gas station.", "", NotificationColor.Yellow, 3f);
                }
                if ((car != null) && car.Broken)
                {
                    base.Player.SendPeriodicMessage("Your car has broken down. You had to walk. Get your car fixed at the gas station.", "", NotificationColor.Yellow, 3f);
                }
            }
            TravelByFoot foot = new TravelByFoot();
            if (originalTask is TravelByBus)
            {
                if (this.BusTokens <= 0)
                {
                    base.Player.SendPeriodicMessage("You are out of bus tokens. You had to walk. Buy some tokens at any bus stop.", "", NotificationColor.Yellow, 3f);
                    flag = true;
                }
                else if (foot.EstTimeInSteps(originalTask.From, originalTask.To) <= originalTask.EstTimeInSteps(originalTask.From, originalTask.To))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                foot.From = originalTask.From;
                foot.To = originalTask.To;
                foot.Building = originalTask.Building;
                foot.Owner = originalTask.Owner;
                return foot;
            }
            return originalTask;
        }

        public BankAccount GetAccountByAccountNumber(long accountNumber)
        {
            foreach (BankAccount account2 in this.MerchantAccounts.Values)
            {
                if (account2.AccountNumber == accountNumber)
                {
                    return account2;
                }
            }
            BankAccount account = (BankAccount) this.CreditCardAccounts[accountNumber];
            if (account != null)
            {
                return account;
            }
            if (((this.Car != null) && (this.Car.Loan != null)) && (this.Car.Loan.AccountNumber == accountNumber))
            {
                return this.Car.Loan;
            }
            foreach (BankAccount account4 in this.StudentLoans.Values)
            {
                if (account4.AccountNumber == accountNumber)
                {
                    return account4;
                }
            }
            foreach (BankAccount account5 in this.Mortgages.Values)
            {
                if (account5.AccountNumber == accountNumber)
                {
                    return account5;
                }
            }
            return null;
        }

        public ArrayList GetAllTasks()
        {
            ArrayList list = new ArrayList(this.dailyRoutineWD.Tasks.Values);
            list.AddRange(this.dailyRoutineWE.Tasks.Values);
            list.AddRange(this.OneTimeEventTaskQueue);
            return list;
        }

        public InsurancePolicy GetBestHealthInsurance()
        {
            InsurancePolicy healthInsurance = this.HealthInsurance;
            foreach (Task task in this.GetAllTasks())
            {
                if (task is WorkTask)
                {
                    WorkTask task2 = (WorkTask) task;
                    if (((task2.HealthInsurance != null) && (task2.HealthInsurance.Copay > -1f)) && ((task2.HealthInsurance.Copay < healthInsurance.Copay) || (healthInsurance.Copay == -1f)))
                    {
                        healthInsurance = task2.HealthInsurance;
                    }
                }
            }
            return healthInsurance;
        }

        public Task GetCurrentTaskForOneTimeEvent()
        {
            Task task = null;
            foreach (Task task2 in this.OneTimeEventTaskQueue)
            {
                if ((((task2.DayLastStarted != A.State.Day) && (task2.OneTimeDay.Day == A.State.Day)) && (task2.OneTimeDay.Month == A.State.Month)) && (task2.OneTimeDay.Year == A.State.Year))
                {
                    if (task2.StartPeriod < task2.EndPeriod)
                    {
                        if ((task2.StartPeriod <= A.State.Period) && (A.State.Period < task2.EndPeriod))
                        {
                            task = task2;
                        }
                    }
                    else if ((task2.StartPeriod <= A.State.Period) || (A.State.Period < task2.EndPeriod))
                    {
                        task = task2;
                    }
                }
            }
            if (task != null)
            {
                task.DayLastStarted = A.State.Day;
            }
            return task;
        }

        public DailyRoutine GetDailyRoutine()
        {
            return this.GetDailyRoutine(A.State.Weekend);
        }

        public DailyRoutine GetDailyRoutine(bool weekend)
        {
            if (weekend)
            {
                return this.dailyRoutineWE;
            }
            return this.dailyRoutineWD;
        }

        public SortedList GetInstallmentLoans()
        {
            SortedList list = (SortedList) this.StudentLoans.Clone();
            foreach (InstallmentLoan loan in this.Mortgages.Values)
            {
                list.Add(loan.AccountNumber, loan);
            }
            if ((this.Car != null) && (this.Car.Loan != null))
            {
                list.Add(this.Car.Loan.AccountNumber, this.Car.Loan);
            }
            return list;
        }

        public InvestmentAccount GetInvestmentAccount(string fundName, bool retirement)
        {
            SortedList investmentAccounts = this.InvestmentAccounts;
            if (retirement)
            {
                investmentAccounts = this.RetirementAccounts;
            }
            InvestmentAccount account = null;
            foreach (InvestmentAccount account2 in investmentAccounts.Values)
            {
                if (account2.Fund.Name == fundName)
                {
                    account = account2;
                }
            }
            if (account == null)
            {
                account = new InvestmentAccount(A.State.GetFund(fundName)) {
                    Retirement = retirement,
                    BankName = A.Resources.GetString("Fiduciary Investments")
                };
                investmentAccounts.Add(account.AccountNumber, account);
            }
            return account;
        }

        public Mortgage GetMortgage(AppBuilding building)
        {
            foreach (Mortgage mortgage in this.Mortgages.Values)
            {
                if (mortgage.Building == building)
                {
                    return mortgage;
                }
            }
            return null;
        }

        public Task GetTaskByID(long taskID)
        {
            foreach (Task task in this.GetAllTasks())
            {
                if (task.ID == taskID)
                {
                    return task;
                }
            }
            return null;
        }

        public bool Has(string itemName)
        {
            foreach (PurchasableItem item in this.PurchasedItems)
            {
                if (item.Name.StartsWith(itemName))
                {
                    return true;
                }
            }
            return false;
        }

        public float HealthFactorAvg(int index)
        {
            float num = 0f;
            foreach (float num3 in this.HealthFactors[index])
            {
                num += num3;
            }
            return (num / ((float) this.HealthFactors[index].Length));
        }

        public int ImageIndexOf(string itemName)
        {
            foreach (PurchasableItem item in this.PurchasedItems)
            {
                if (item.Name.StartsWith(itemName))
                {
                    return int.Parse(item.Name.Substring(itemName.Length));
                }
            }
            return -1;
        }

        public void Init()
        {
            this.Cash = A.Settings.InitialCash;
            for (int i = 0; i < this.HealthFactors.Length; i++)
            {
                this.HealthFactors[i] = new float[AppConstants.HealthFactoryHistoryDays[i]];
                for (int j = 0; j < this.HealthFactors[i].Length; j++)
                {
                    this.HealthFactors[i][j] = A.Settings.InitialHealth;
                }
            }
            this.LastDanced = A.State.Now;
            this.MerchantAccounts.Add("NRG Electric", new BankAccount("NRG Electric", base.Name));
            this.MerchantAccounts.Add("City Property Mgt", new BankAccount("City Property Mgt", base.Name));
            this.MerchantAccounts.Add("Internet Connect", new BankAccount("Internet Connect", base.Name));
            this.MerchantAccounts.Add("Taranti Auto Lease", new BankAccount("Taranti Auto Lease", base.Name));
            this.MerchantAccounts.Add("Quest Student Loans", new BankAccount("Quest Student Loans", base.Name));
            this.MerchantAccounts.Add("S&W Insurance", new BankAccount("S&W Insurance", base.Name));
            this.MerchantAccounts.Add("Vincent Medical", new BankAccount("Vincent Medical", base.Name));
            this.MerchantAccounts.Add("IRS", new BankAccount("IRS", base.Name));
            this.MerchantAccounts.Add("Century Mortgage", new BankAccount("Century Mortgage", base.Name));
            this.Surprises = (Surprise[]) TableReader.Read(base.GetType().Assembly, typeof(Surprise), "KMI.VBPF1Lib.Data.Surprises.txt");
            this.SetUpReserved();
        }

        public override void NewDay() {
            GameOverMessage message;
            if (((A.State.DayOfWeek == DayOfWeek.Saturday) || (A.State.DayOfWeek == DayOfWeek.Monday)) && (this.Person.Task != null))
            {
                this.Person.Task.CleanUp();
                this.Person.Task = null;
            }
            if (A.Settings.EndOnLowCreditScore && (this.CreditScore() < 400))
            {
                object[] args = new object[] { 400 };
                message = new GameOverMessage(base.Player.PlayerName, A.Resources.GetString("Your credit score has fallen below {0}. Unfortunately, that's sim over for you! Try again and keep an eye on your credit score!", args));
                this.RetireFromApp(message);
            }
            else if (A.Settings.Sickness && (this.HealthFactorAvg(0) == 0f))
            {
                message = new GameOverMessage(base.Player.PlayerName, A.Resources.GetString("Your nutrition was so bad that you passed away. Unfortunately, that's sim over for you! Try again and remember to eat well."));
                this.RetireFromApp(message);
            }
            else
            {
                ArrayList list;
                int num;
                TimeSpan span;
                if (A.Settings.Sickness && (this.HealthFactorAvg(0) <= 0.15f))
                {
                    base.Player.SendPeriodicMessage(A.Resources.GetString("Your nutrition is very low. Buy food and eat before you pass away!"), "", NotificationColor.Red, 2f);
                }
                if ((A.State.Day == 1) && (A.State.Month == 1))
                {
                    this.PrepareTaxForms();
                }
                if (A.State.Day == 28)
                {
                    list = new ArrayList(this.BankAccounts.Values);
                    foreach (BankAccount account in list)
                    {
                        account.EndMonth();
                        if (account.EndingBalance() < 0f)
                        {
                            object[] objArray2 = new object[] { account.AccountTypeFriendlyName.ToLower(), account.BankName };
                            base.Player.SendMessage(A.Resources.GetString("The balance in your {0} account at {1} has fallen below zero due to fees or other charges. The account has been closed.", objArray2), "", NotificationColor.Red);
                            this.CloseAccount(account);
                        }
                    }
                    foreach (BankAccount account2 in this.CreditCardAccounts.Values)
                    {
                        account2.EndMonth();
                    }
                    foreach (InstallmentLoan loan in A.Adapter.GetInstallmentLoans(base.ID).Values)
                    {
                        loan.EndMonth();
                    }
                    foreach (InvestmentAccount account3 in this.InvestmentAccounts.Values)
                    {
                        account3.EndMonth();
                    }
                    foreach (InvestmentAccount account4 in this.RetirementAccounts.Values)
                    {
                        account4.EndMonth();
                    }
                }
                if ((A.State.Day == 28) && (this.Dwelling != null))
                {
                    this.BillDunCancel();
                }
                if (A.State.DayOfWeek == DayOfWeek.Saturday)
                {
                    foreach (Task task in this.GetAllTasks())
                    {
                        if (task is WorkTask)
                        {
                            KMI.VBPF1Lib.Check check;
                            WorkTask task2 = (WorkTask) task;
                            float grossPay = task2.HourlyWage * task2.HoursThisWeek;
                            string payDescription = A.Resources.GetString("Wages");
                            if (task2 is WorkDrugRep)
                            {
                                payDescription = "Commis-" + Environment.NewLine + "sions";
                                grossPay *= (float) ((1.0 + (A.State.Random.NextDouble() * 1.3)) - 0.625);
                            }
                            if ((task2.BonusPotential > 0f) && ((A.State.CurrentWeek % 13) == 12))
                            {
                                payDescription = payDescription + Environment.NewLine + " + Bonus";
                                float amount = (float) (((grossPay * 13f) * A.State.Random.NextDouble()) * task2.BonusPotential);
                                if (amount > 0f)
                                {
                                    object[] objArray3 = new object[] { task2.Building.OwnerName, Utilities.FC(amount, A.Instance.CurrencyConversion) };
                                    base.Player.SendMessage(A.Resources.GetString("Congratulations! {0} is doing well, and you got a quarterly bonus of {1}.", objArray3), "", NotificationColor.Green);
                                }
                                grossPay += amount;
                            }
                            PayStub stub = new PayStub(task.Building.OwnerName, base.Name, payDescription, A.State.Now, task2.HoursThisWeek, grossPay, (WorkTask) task, this.FICAPaidThisYear());
                            task2.HoursThisWeek = 0f;
                            task2.PayStubs.Add(stub);
                            if (task2.DirectDepositAccount != null)
                            {
                                float netPay = stub.NetPay;
                                if (!A.Settings.AutofillCheckRegister)
                                {
                                    netPay = (float) Math.Round((double) netPay, 2);
                                }
                                task2.DirectDepositAccount.Transactions.Add(new Transaction(netPay, Transaction.TranType.Credit, "Wages Direct Deposit"));
                            }
                            else
                            {
                                check = new KMI.VBPF1Lib.Check(-1L) {
                                    Amount = stub.NetPay,
                                    Payee = base.Name,
                                    Date = A.State.Now,
                                    Payor = task2.Building.OwnerName,
                                    Number = (int) A.State.GetNextID(),
                                    Memo = A.Resources.GetString("Weekly pay"),
                                    Signature = A.Resources.GetString("John Q. Controller")
                                };
                                this.AddCheck(check);
                            }
                            if (task2 is WorkTravellingSalesman)
                            {
                                check = new KMI.VBPF1Lib.Check(-1L) {
                                    Amount = ((WorkTravellingSalesman) task2).Mileage * 0.005f,
                                    Payee = base.Name,
                                    Date = A.State.Now,
                                    Payor = task2.Building.OwnerName,
                                    Number = (int) A.State.GetNextID(),
                                    Memo = A.Resources.GetString("Mileage Reimb."),
                                    Signature = A.Resources.GetString("John Q. Controller")
                                };
                                this.AddCheck(check);
                                ((WorkTravellingSalesman) task2).Mileage = 0;
                            }
                            float num3 = stub.GetValue("401K");
                            if (task2.R401KMatch > -1f)
                            {
                                num3 += grossPay * Math.Min(task2.R401KPercentWitheld, task2.R401KMatch);
                            }
                            num = 0;
                            foreach (Fund fund in A.State.MutualFunds)
                            {
                                if (task2.R401KAllocations[num] > 0f)
                                {
                                    this.GetInvestmentAccount(fund.Name, true).Transactions.Add(new Transaction((num3 * task2.R401KAllocations[num]) / fund.Price, Transaction.TranType.Credit, "Share Purchase"));
                                }
                                num++;
                            }
                            span = (TimeSpan) (A.State.Now - task2.StartDate);
                            if (((span.Days / 7) % 0x34) == 0x33)
                            {
                                task2.HourlyWage *= 1.02f;
                                task2.HourlyWage = (float) Math.Round((double) task2.HourlyWage, 2);
                                object[] objArray4 = new object[] { task2.Name().ToLower(), task2.Building.OwnerName, Utilities.FP(0.02f) };
                                base.Player.SendMessage(A.Resources.GetString("Congratulations. It's the anniversary of your hiring as a {0} for {1}. You've been given a {2} raise!", objArray4), "", NotificationColor.Green);
                            }
                        }
                    }
                }
                foreach (BankAccount account5 in this.BankAccounts.Values)
                {
                    if (account5 is CheckingAccount)
                    {
                        list = new ArrayList(((CheckingAccount) account5).ChecksInTheMail);
                        foreach (KMI.VBPF1Lib.Check check2 in list)
                        {
                            span = (TimeSpan) (A.State.Now - check2.Date);
                            int num6 = span.Days - 2;
                            if (A.State.Random.NextDouble() < (((float) num6) / 12f))
                            {
                                ((CheckingAccount) account5).ChecksInTheMail.Remove(check2);
                                if (check2.Amount <= account5.EndingBalance())
                                {
                                    object[] objArray5 = new object[] { check2.Number, check2.Payee };
                                    string description = A.Resources.GetString("Chk#{0}: {1}", objArray5);
                                    if (description.Length > 0x16)
                                    {
                                        description = description.Substring(0, 0x16);
                                    }
                                    account5.Transactions.Add(new Transaction(check2.Amount, Transaction.TranType.Debit, description));
                                    this.ApplyPaymentToAccount(check2.ApplyToAccount.AccountNumber, check2.Amount);
                                }
                                else
                                {
                                    object[] objArray6 = new object[] { check2.Number, check2.Payee, Utilities.FC(check2.Amount, A.Instance.CurrencyConversion), Utilities.FC(35f, A.Instance.CurrencyConversion) };
                                    base.Player.SendMessage(A.Resources.GetString("Your check # {0} to {1} for {2} has bounced! You will be charged a fee of {3}.", objArray6), "", NotificationColor.Yellow);
                                    account5.Transactions.Add(new Transaction(35f, Transaction.TranType.Debit, A.Resources.GetString("Overdraft Fee")));
                                }
                            }
                        }
                    }
                }
                for (num = 0; num < A.Settings.HealthFactorsToConsider; num++)
                {
                    this.HealthFactors[num][A.State.DayCount % AppConstants.HealthFactoryHistoryDays[num]] = 0f;
                    for (int i = 0; (this.TodaysHealth[num] > 0f) && (i < AppConstants.HealthFactorApplyForwardDays[num]); i++)
                    {
                        int index = (i + A.State.DayCount) % AppConstants.HealthFactoryHistoryDays[num];
                        this.HealthFactors[num][index] = Math.Min((float) 1f, (float) (this.HealthFactors[num][index] + (this.TodaysHealth[num] / AppConstants.HealthFactorNeededPerDay[num])));
                        this.TodaysHealth[num] -= AppConstants.HealthFactorNeededPerDay[num];
                    }
                    this.TodaysHealth[num] = 0f;
                }
                if (A.Settings.HealthFactorsToConsider < 5)
                {
                    this.LastDanced = A.State.Now;
                }
                span = (TimeSpan) (A.State.Now - this.LastDanced);
                this.HealthFactors[4][0] = Utilities.Clamp(1f - (((float) span.Days) / 100f));
                if (this.ElectricityOff)
                {
                    this.Food.Clear();
                }
                if (this.Dwelling != null)
                {
                    float health = this.Health;
                    if (!A.Settings.Sickness)
                    {
                        this.Sick = false;
                    }
                    else if (health < 0.1)
                    {
                        this.Sick = A.State.Random.Next(3) == 0;
                    }
                    else if (health < 0.66)
                    {
                        this.Sick = A.State.Random.NextDouble() < (health * 0.1);
                    }
                    else
                    {
                        this.Sick = false;
                    }
                    if (this.Sick)
                    {
                        object[] objArray7 = new object[] { this.WorstHealthFactor().ToLower() };
                        base.Player.SendMessage(A.Resources.GetString("Because of some unhealthy habits, particularly your lack of {0}, you have become sick and cannot leave home for the next 24 hours.", objArray7), "", NotificationColor.Yellow);
                        foreach (Task task3 in this.GetDailyRoutine().Tasks.Values)
                        {
                            if ((task3 is WorkTask) || (task3 is AttendClass))
                            {
                                task3.DatesAbsent.Add(A.State.Now);
                            }
                        }
                    }
                }
                foreach (BankAccount account6 in this.BankAccounts.Values)
                {
                    if (account6 is CheckingAccount)
                    {
                        list = (ArrayList) ((CheckingAccount) account6).RecurringPayments.Clone();
                        foreach (RecurringPayment payment in list)
                        {
                            if (payment.Day == A.State.Day)
                            {
                                if (account6.EndingBalance() < payment.Amount)
                                {
                                    base.Player.SendMessage(A.Resources.GetString("Some scheduled recurring payments were rejected. You do not have enough money in the account."), account6.BankName, NotificationColor.Yellow);
                                    break;
                                }
                                string payeeName = payment.PayeeName;
                                if (payeeName.IndexOf("Acct#") > -1)
                                {
                                    payeeName = payeeName.Substring(0, payeeName.IndexOf("Acct#") - 1);
                                }
                                account6.Transactions.Add(new Transaction(payment.Amount, Transaction.TranType.Debit, payeeName));
                                this.ApplyPaymentToAccount(payment.PayeeAccountNumber, payment.Amount);
                                this.RemoveBillIfExactMatch(payment.PayeeAccountNumber, payment.Amount);
                            }
                        }
                    }
                }
                ManageExpiringFood();
                if (((this.Car != null) && (this.Car.LeaseCost > 0f)) && ((span = (TimeSpan) (A.State.Now - this.Car.Purchased)).Days > 730))
                {
                    base.Player.SendModalMessage(A.Resources.GetString("Your car lease has ended. If you want to keep driving, back to Tommy Taranti's to get a new lease."), A.Resources.GetString("Car Lease Ended"), MessageBoxIcon.None);
                    A.State.City.Cars.Remove(this.Car);
                    this.Car = null;
                }
                DateTime time = A.State.Now.AddHours(-1.0);
                bool weekend = (time.DayOfWeek == DayOfWeek.Saturday) || (time.DayOfWeek == DayOfWeek.Sunday);
                foreach (Task task4 in this.GetDailyRoutine(weekend).Tasks.Values)
                {
                    if ((task4 is WorkTask) || (task4 is AttendClass))
                    {
                        DateTime time3 = new DateTime(time.Year, time.Month, time.Day, task4.StartPeriod / 2, (task4.StartPeriod % 2) * 30, 0);
                        if ((task4.ArrivedToday < DateTime.MaxValue) && (task4.ArrivedToday > time3.AddMinutes(5.0)))
                        {
                            TimeSpan span3 = (TimeSpan) (task4.ArrivedToday - time3);
                            string str4 = A.Resources.GetString("work");
                            if (task4 is AttendClass)
                            {
                                str4 = A.Resources.GetString("class");
                            }
                            object[] objArray8 = new object[] { span3.Minutes, str4, time.DayOfWeek.ToString() };
                            base.Player.SendMessage(A.Resources.GetString("You were {0} minutes late to {1} on {2}. Check your travel schedule. You need to allow more time to travel or use faster transportation.", objArray8), "", NotificationColor.Yellow);
                            task4.DatesLate.Add(time);
                        }
                    }
                }
                foreach (Task task5 in this.GetDailyRoutine().Tasks.Values)
                {
                    task5.ArrivedToday = DateTime.MaxValue;
                }
                ArrayList allTasks = this.GetAllTasks();
                foreach (Task task6 in allTasks)
                {
                    if ((task6 is WorkTask) || (task6 is AttendClass))
                    {
                        string str5 = task6.BadAttendance();
                        if (str5 != null)
                        {
                            A.Adapter.DeleteTask(base.ID, task6.ID, true, false);
                            base.Player.SendMessage(str5, "", NotificationColor.Red);
                        }
                    }
                }
                if (((A.Settings.TaxesEnabledForOwner && (A.State.Now.Year > A.Settings.StartDate.Year)) && (A.State.Now.Month == 5)) && (A.State.Now.Day == 1))
                {
                    this.AuditAndRefundOrBill();
                }
                if (A.Settings.DislocatedShoulderDate.ToShortDateString() == A.State.Now.ToShortDateString())
                {
                    base.Player.SendMessage(A.Resources.GetString("Oh, no! What bad luck! You have suffered from a dislocated shoulder. Fortunately, you were treated successfully at the hospital. A bill from the hospital will be sent to you shortly!"), "", NotificationColor.Red);
                    BankAccount account7 = (BankAccount) this.MerchantAccounts["Vincent Medical"];
                    this.AddBill(new Bill("Vincent Medical", A.Resources.GetString("Treatment for shoulder"), 613f, account7));
                }
            }
        }

        public void ManageExpiringFood() {
            if (Food.Count == 0) { return; }
            if (!Food[0].Expired(A.State.Now)) { return; }
            if (A.Settings.Level > 1) { return; }
            Food.ForEach(f => { if (f.Expired(A.State.Now)) { f.Expire(); } });
            this.Food.RemoveAll(f => f.Food == 0);
            base.Player.SendMessage(A.Resources.
                GetString("Some of your food has expired!"), "", 
                NotificationColor.Yellow);
        }

        public override void NewHour()
        {
        }

        public void NewPeriod() {
            if (this.Person.Task == null) { return; }
            this.Person.Task.Process(this);
        }

        public override bool NewStep()
        {
            if (this.Person.Task == null)
            {
                this.Person.Task = this.GetCurrentTaskForOneTimeEvent();
                if (this.Person.Task == null)
                {
                    this.Person.Task = this.GetDailyRoutine().GetCurrentTask();
                }
                if (this.Person.Task != null)
                {
                    if (this.Sick && ((((this.Person.Task is WorkTask) || (this.Person.Task is TravelTask)) || (this.Person.Task is AttendClass)) || (this.Person.Task is Sleep)))
                    {
                        BeSick sick1 = new BeSick {
                            StartPeriod = this.Person.Task.StartPeriod,
                            EndPeriod = this.Person.Task.EndPeriod,
                            Building = this.Dwelling,
                            Owner = this.Person
                        };
                        Task task = sick1;
                        this.Person.Task = task;
                    }
                    AppBuilding building = A.State.City.FindInsideBuilding(this);
                    if (building != this.Person.Task.Building)
                    {
                        if (building != null)
                        {
                            building.Persons.Remove(this.Person);
                        }
                        this.Person.Task.Building.Persons.Add(this.Person);
                        MapV2 map = this.Person.Task.Building.Map;
                        if (map != null)
                        {
                            this.Person.Location = (PointF) map.getNode("EntryPoint").Location;
                        }
                    }
                    this.Person.Task.ArrivedToday = A.State.Now;
                    if (this.Person.Task is TravelTask)
                    {
                        this.Person.Task = this.FindActualTravelTask((TravelTask) this.Person.Task);
                    }
                }
            }
            if (this.Person.Task == null)
            {
                this.Person.Pose = "StandSW";
            }
            else if (this.Person.Task.Do())
            {
                this.Person.Task.CleanUp();
                this.Person.Task = null;
            }
            return false;
        }

        public override void NewWeek()
        {
            if (A.Settings.EndOnLowCreditScore && (this.CreditScore() < 500))
            {
                object[] args = new object[] { 500, 400 };
                base.Player.SendMessage(A.Resources.GetString("Your credit score is below {0}. If it falls below {1}, it's sim over for you! Try to improve your credit score.", args), "", NotificationColor.Red);
            }
            if (base.Player.PlayerType == PlayerType.Human)
            {
                if (scoreAdapter == null)
                {
                    scoreAdapter = ScoreAdapter.JoinScoring();
                }
                if (scoreAdapter != null)
                {
                    scoreAdapter.SendScore(base.Name, this.CriticalResourceBalance());
                }
            }
            base.Journal.AddNumericData("Net Worth", this.CriticalResourceBalance());
        }

        public override void NewYear()
        {
        }

        public float PartyHostingScore()
        {
            int num = 0;
            foreach (DateTime time in this.PartyAttendance.Keys)
            {
                if (time > A.State.Now.AddDays(-120.0))
                {
                    num += (int) this.PartyAttendance[time];
                }
            }
            return (((float) num) / 120f);
        }

        public void PrepareTaxForms()
        {
            string str;
            int year = A.State.Year - 1;
            DateTime date = new DateTime(year, 12, 12, 0x17, 0x3b, 0x3b);
            ArrayList list = new ArrayList();
            foreach (WorkTask task in this.WorkTaskHistory.Values)
            {
                foreach (PayStub stub in task.PayStubs)
                {
                    if ((stub.WeekEnding.Year == year) && !list.Contains(task))
                    {
                        list.Add(task);
                    }
                }
            }
            foreach (WorkTask task2 in list)
            {
                FW2 fw = null;
                str = year.ToString() + "-" + task2.Building.ID;
                if (this.FW2s.ContainsKey(str))
                {
                    fw = (FW2) this.FW2s[str];
                }
                else
                {
                    fw = new FW2(year, task2.Building.OwnerName, base.Name, this.Person.ID);
                    this.FW2s.Add(str, fw);
                }
                fw.Wages += task2.GetValueYTD("Gross Pay", date) - task2.GetValueYTD("401K", date);
                fw.RetirementPlan = task2.GetValueYTD("401K", date) > 0f;
                fw.SSWages += task2.GetValueYTD("Soc Sec", date) / 0.062f;
                fw.MedicareWages += task2.GetValueYTD("Gross Pay", date);
                fw.FedWT += task2.GetValueYTD("Fed WT", date);
                fw.StateWT += task2.GetValueYTD("State WT", date);
            }
            ArrayList list2 = new ArrayList(this.BankAccounts.Values);
            list2.AddRange(this.ClosedBankAccounts.Values);
            foreach (BankAccount account in list2)
            {
                if (account is SavingsAccount)
                {
                    F1099Int num2 = null;
                    str = year.ToString() + "-" + account.ID;
                    if (this.F1099s.ContainsKey(str))
                    {
                        num2 = (F1099Int) this.F1099s[str];
                    }
                    else
                    {
                        num2 = new F1099Int(year, account.BankName, base.Name, this.Person.ID);
                        this.F1099s.Add(str, num2);
                    }
                    num2.Interest += ((SavingsAccount) account).Interest(year);
                }
            }
            if (A.Settings.TaxesEnabledForOwner)
            {
                base.Player.SendMessage("Your tax information for last year has arrived. You now have the information needed to prepare your tax return.", "", NotificationColor.Green);
            }
        }

        public void RemoveBillIfExactMatch(long payeeAccountNumber, float amount)
        {
            foreach (Bill bill in this.Bills.Values)
            {
                if (((bill.Account != null) && (bill.Account.AccountNumber == payeeAccountNumber)) && (bill.Amount <= amount))
                {
                    this.Bills.Remove(bill.ID);
                    break;
                }
            }
        }

        public void RemoveTask(Task t)
        {
            t.EndDate = A.State.Now;
            if (t is WorkTask)
            {
                Offering offering = A.State.City.FindOfferingForTask(t);
                if (offering != null)
                {
                    offering.Taken = false;
                }
            }
            this.GetDailyRoutine(t.Weekend).Tasks.Remove(t.StartPeriod);
        }

        public void RepossessCar()
        {
            if (this.Car.Loan != null)
            {
                this.Car.Loan.Transactions.Add(new Transaction(this.Car.ComputeResalePrice(A.State.Now), Transaction.TranType.Debit, "Value of repossessed car"));
                this.CloseAccount(this.Car.Loan);
            }
            A.State.City.Cars.Remove(this.Car);
            this.Car = null;
        }

        public void RetireFromApp(GameOverMessage gomsg)
        {
            if (A.State.Multiplayer)
            {
                foreach (AppBuilding building in A.State.City.GetBuildings())
                {
                    if (building.Persons.Contains(this.Person))
                    {
                        building.Persons.Remove(this.Person);
                    }
                    if (building.Owner == this)
                    {
                        building.Owner = null;
                    }
                }
                DwellingOffer offer = (DwellingOffer) this.Dwelling.Offerings[0];
                offer.Taken = false;
                ArrayList allTasks = this.GetAllTasks();
                foreach (Task task in allTasks)
                {
                    this.RemoveTask(task);
                }
                object[] args = new object[] { base.Name };
                PlayerMessage.Broadcast(A.Resources.GetString("The player, {0}, has died or been foreclosed upon and is out of the sim.", args), "", NotificationColor.Green);
            }
            base.Retire(gomsg);
        }

        public void SetDailyRoutine(bool weekend, DailyRoutine routine)
        {
            if (weekend)
            {
                this.dailyRoutineWE = routine;
            }
            else
            {
                this.dailyRoutineWD = routine;
            }
        }

        public void SetModeOfTransportation(int index)
        {
            DailyRoutine routine = this.dailyRoutineWD.MakeCopy();
            this.CheckValiditySynchTravel(index, routine, null, false);
            this.dailyRoutineWD = routine;
            DailyRoutine routine2 = this.dailyRoutineWE.MakeCopy();
            this.CheckValiditySynchTravel(index, routine2, null, false);
            this.dailyRoutineWE = routine2;
            this.modeOfTransportation = index;
        }

        public void SetUpReserved()
        {
            base.reserved = new Hashtable();
        }

        public string WorstHealthFactor()
        {
            float maxValue = float.MaxValue;
            int index = -1;
            for (int i = 0; i < A.Settings.HealthFactorsToConsider; i++)
            {
                if (this.HealthFactorAvg(i) < maxValue)
                {
                    index = i;
                    maxValue = this.HealthFactorAvg(i);
                }
            }
            return AppConstants.HealthFactorNames[index];
        }

        public float YearsExperience(string taskName)
        {
            float num = 0f;
            foreach (WorkTask task in this.WorkTaskHistory.Values)
            {
                if ((task.Name() == taskName) || (taskName == "worker of any kind"))
                {
                    TimeSpan span;
                    if (task.EndDate == DateTime.MinValue)
                    {
                        span = (TimeSpan) (A.State.Now - task.StartDate);
                        num += ((float) span.Days) / 365f;
                    }
                    else
                    {
                        span = (TimeSpan) (task.EndDate - task.StartDate);
                        num += ((float) span.Days) / 365f;
                    }
                }
            }
            return num;
        }

        public float DebtService
        {
            get
            {
                float num = 0f;
                foreach (InstallmentLoan loan in A.Adapter.GetInstallmentLoans(base.ID).Values)
                {
                    num += loan.Payment;
                }
                return (num * 12f);
            }
        }

        public float GrossIncomeLast12Months
        {
            get
            {
                float num = 0f;
                foreach (WorkTask task in this.WorkTaskHistory.Values)
                {
                    foreach (PayStub stub in task.PayStubs)
                    {
                        TimeSpan span = (TimeSpan) (A.State.Now - stub.WeekEnding);
                        if (span.Days < 0x16d)
                        {
                            num += (float) stub.PayValues["Gross Pay"];
                        }
                    }
                }
                return num;
            }
        }

        public float Health
        {
            get
            {
                int num;
                float num2 = 0f;
                for (num = 0; num < A.Settings.HealthFactorsToConsider; num++)
                {
                    num2 += this.HealthFactorAvg(num);
                }
                num2 /= (float) A.Settings.HealthFactorsToConsider;
                float maxValue = float.MaxValue;
                for (num = 0; num < A.Settings.HealthFactorsToConsider; num++)
                {
                    maxValue = Math.Min(maxValue, this.HealthFactorAvg(num));
                }
                if (maxValue < 0.1)
                {
                    return maxValue;
                }
                return num2;
            }
        }

        public int ModeOfTransportation
        {
            get
            {
                return this.modeOfTransportation;
            }
        }
    }
}

