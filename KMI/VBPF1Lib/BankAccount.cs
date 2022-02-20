namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class BankAccount : Offering
    {
        public string BankName;
        protected static Font bankNameFont = new Font("Times New Roman", 16f, FontStyle.Italic);
        protected static Brush brush = new SolidBrush(Color.Black);
        protected int col1Width;
        protected int colWidth;
        public DateTime DateClosed;
        protected static Font font = new Font("Arial", 8f);
        protected static Font fontS = new Font("Arial", 7f);
        protected float interestRate;
        protected float maintenanceFee;
        protected int margin;
        protected float minimumBalance;
        public string OwnerName;
        protected static Pen pen = new Pen(brush, 1f);
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        protected Status status;
        public ArrayList Transactions;

        static BankAccount()
        {
            sfr.Alignment = StringAlignment.Far;
            sfc.Alignment = StringAlignment.Center;
        }

        public BankAccount()
        {
            this.Transactions = new ArrayList();
            this.DateClosed = DateTime.MaxValue;
            this.margin = 5;
            this.col1Width = 120;
            this.colWidth = 60;
            this.status = Status.Current;
            this.GenerateNewAccountNumber();
        }

        public BankAccount(string bankName, string ownerName)
        {
            this.Transactions = new ArrayList();
            this.DateClosed = DateTime.MaxValue;
            this.margin = 5;
            this.col1Width = 120;
            this.colWidth = 60;
            this.status = Status.Current;
            this.GenerateNewAccountNumber();
            this.BankName = bankName;
            this.OwnerName = ownerName;
        }

        public float AverageDailyBalance(int year, int month)
        {
            ArrayList list = this.TransactionsForMonth(year, month);
            float num = this.BeginningBalance(year, month);
            float num2 = 0f;
            int day = 1;
            if ((this.DateOpened.Year == year) && (this.DateOpened.Month == month))
            {
                day = this.DateOpened.Day;
            }
            int num4 = 0;
            for (int i = day; i <= DateTime.DaysInMonth(year, month); i++)
            {
                while ((list.Count > 0) && (((Transaction) list[0]).Date.Day == i))
                {
                    Transaction transaction = (Transaction) list[0];
                    if (transaction.Type == Transaction.TranType.Debit)
                    {
                        num -= transaction.Amount;
                    }
                    else
                    {
                        num += transaction.Amount;
                    }
                    num = (float) Math.Round((double) num, 2);
                    list.RemoveAt(0);
                }
                num2 += num;
                num2 = (float) Math.Round((double) num2, 2);
                num4++;
            }
            return (float) Math.Round((double) (num2 / ((float) num4)), 2);
        }

        public float BalanceThru(Transaction last)
        {
            float num = 0f;
            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num -= transaction.Amount;
                }
                else
                {
                    num += transaction.Amount;
                }
                num = (float) Math.Round((double) num, 2);
                if (transaction == last)
                {
                    return num;
                }
            }
            return num;
        }

        public float BeginningBalance(int year, int month)
        {
            if (month == 1)
            {
                month = 12;
                year--;
            }
            else
            {
                month--;
            }
            return this.EndingBalance(year, month);
        }

        public float Credits(int year, int month)
        {
            float num = 0f;
            ArrayList list = this.TransactionsForMonth(year, month);
            foreach (Transaction transaction in list)
            {
                if (transaction.Type == Transaction.TranType.Credit)
                {
                    num += transaction.Amount;
                    num = (float) Math.Round((double) num, 2);
                }
            }
            return num;
        }

        public string CRStatus(DateTime now)
        {
            string str = "O";
            if (this is InstallmentLoan)
            {
                str = "I";
            }
            if (this is CreditCardAccount)
            {
                str = "R";
            }
            int num = this.DaysPastDue(now);
            if (num > 0)
            {
                num = (num / 30) + 2;
            }
            else
            {
                num = 1;
            }
            return (str + num);
        }

        public float CurrentCharges(DateTime now)
        {
            float num = 0f;
            foreach (Transaction transaction in this.Transactions)
            {
                if ((transaction.Type == Transaction.TranType.Credit) && ((now - transaction.Date).Days <= 30))
                {
                    num += transaction.Amount;
                    num = (float) Math.Round((double) num, 2);
                }
            }
            return num;
        }

        public virtual int DaysPastDue(DateTime now)
        {
            float num = 0f;
            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num += transaction.Amount;
                    num = (float) Math.Round((double) num, 2);
                }
            }
            foreach (Transaction transaction2 in this.Transactions)
            {
                if (transaction2.Type == Transaction.TranType.Credit)
                {
                    num -= transaction2.Amount;
                    num = (float) Math.Round((double) num, 2);
                    if (num < -0.01)
                    {
                        TimeSpan span = (TimeSpan) (now - transaction2.Date);
                        int num2 = span.Days - 30;
                        return Math.Max(0, num2);
                    }
                }
            }
            return 0;
        }

        public float Debits(int year, int month)
        {
            float num = 0f;
            ArrayList list = this.TransactionsForMonth(year, month);
            foreach (Transaction transaction in list)
            {
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num += transaction.Amount;
                    num = (float) Math.Round((double) num, 2);
                }
            }
            return num;
        }

        public float Debits(int year, int month, int day)
        {
            float num = 0f;
            ArrayList list = this.TransactionsForMonth(year, month);
            foreach (Transaction transaction in list)
            {
                if ((transaction.Type == Transaction.TranType.Debit) && (transaction.Date.Day <= day))
                {
                    num += transaction.Amount;
                    num = (float) Math.Round((double) num, 2);
                }
            }
            return num;
        }

        public float EndingBalance()
        {
            return this.EndingBalance(0x270f, 1);
        }

        public float EndingBalance(DateTime date)
        {
            float num = 0f;
            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction.Date > date)
                {
                    return num;
                }
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num -= transaction.Amount;
                }
                else
                {
                    num += transaction.Amount;
                }
                num = (float) Math.Round((double) num, 2);
            }
            return num;
        }

        public float EndingBalance(int year, int month)
        {
            float num = 0f;
            foreach (Transaction transaction in this.Transactions)
            {
                if (transaction.Date < LastDayOfCycle(year, month))
                {
                    if (transaction.Type == Transaction.TranType.Debit)
                    {
                        num -= transaction.Amount;
                    }
                    else
                    {
                        num += transaction.Amount;
                    }
                    num = (float) Math.Round((double) num, 2);
                }
            }
            return num;
        }

        public virtual void EndMonth()
        {
            int year = A.State.Year;
            int month = A.State.Month;
            float num3 = this.AverageDailyBalance(year, month);
            float amount = (this.interestRate / 12f) * num3;
            if ((this.DateOpened.Year == year) && (this.DateOpened.Month == month))
            {
                amount *= ((float) (DateTime.DaysInMonth(year, month) - this.DateOpened.Day)) / ((float) DateTime.DaysInMonth(year, month));
            }
            if (this.interestRate > 0f)
            {
                this.Transactions.Add(new Transaction(amount, Transaction.TranType.Credit, A.Resources.GetString("Interest earned"), A.State.Now.AddDays(-1.0)));
            }
            if (num3 < this.minimumBalance)
            {
                this.Transactions.Add(new Transaction(this.maintenanceFee, Transaction.TranType.Debit, A.Resources.GetString("Monthly account fee"), A.State.Now.AddDays(1.0)));
            }
        }

        public static DateTime FirstDayOfCycle(int year, int month)
        {
            PreviousMonth(ref year, ref month);
            return new DateTime(year, month, 0x1c, 0, 0, 0);
        }

        public void GenerateNewAccountNumber()
        {
            base.ID = 0x3e8L + A.State.GetNextID();
        }

        public static DateTime LastDayOfCycle(int year, int month)
        {
            return new DateTime(year, month, 0x1c, 0, 0, 0);
        }

        public int MonthsOpen(DateTime now)
        {
            int year = this.DateOpened.Year;
            int month = this.DateOpened.Month;
            int day = 0x1c;
            int num4 = 0;
            while (new DateTime(year, month, day) <= now)
            {
                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
                num4++;
            }
            return num4;
        }

        public virtual float PastDueAmount(DateTime now)
        {
            return (this.EndingBalance(now) - this.CurrentCharges(now));
        }

        public Status PastDueStatus(int y, int m)
        {
            Status status = this.PastDueStatusPassive(y, m);
            if (((status == Status.Cancelled) && (this.status != Status.Cancelled)) && (this.status != Status.NewlyCancelled))
            {
                this.status = Status.NewlyCancelled;
            }
            else
            {
                this.status = status;
            }
            return this.status;
        }

        public virtual Status PastDueStatusPassive(int y, int m)
        {
            int num = this.DaysPastDue(new DateTime(y, m, 0x1c));
            if (num >= 60)
            {
                return Status.Cancelled;
            }
            if (num >= 30)
            {
                return Status.Deliquent;
            }
            if (num > 0)
            {
                return Status.PastDue;
            }
            return Status.Current;
        }

        public static void PreviousMonth(ref int year, ref int month)
        {
            month--;
            if (month == 0)
            {
                month = 12;
                year--;
            }
        }

        public virtual void PrintPage(int page, Graphics g, int year, int month, int Pages, int TransactionsPerPage)
        {
            int num;
            int margin = this.margin;
            int num3 = 200;
            ArrayList list = this.TransactionsForMonth(year, month);
            g.DrawImageUnscaled(A.Resources.GetImage("Logo" + this.BankName), 4, 4);
            object[] args = new object[] { page + 1, Pages };
            g.DrawString(A.Resources.GetString("Page {0} of {1}", args), font, brush, 320f, (float) margin, sfr);
            margin += 30;
            g.DrawString(this.OwnerName, font, brush, (float) num3, (float) margin);
            margin += 12;
            object[] objArray2 = new object[] { this.AccountNumber };
            g.DrawString(A.Resources.GetString("Account Number: {0}", objArray2), font, brush, (float) num3, (float) margin);
            margin += 12;
            object[] objArray = new object[] { new DateTime(year, month, 0x1c).ToShortDateString() };
            g.DrawString(A.Resources.GetString("Statement Date: {0}", objArray), font, brush, (float) num3, (float) margin);
            margin += 0x19;
            g.DrawLine(pen, 0, margin, 400, margin);
            margin += 2;
            g.DrawLine(pen, 0, margin, 400, margin);
            string[] strArray = new string[] { A.Resources.GetString("Beginning Balance"), A.Resources.GetString("Credits (+)"), A.Resources.GetString("Debits (-)"), A.Resources.GetString("Ending Balance") };
            string[] strArray2 = new string[] { this.BeginningBalance(year, month).ToString("C2"), this.Credits(year, month).ToString("C2"), this.Debits(year, month).ToString("C2"), this.EndingBalance(year, month).ToString("C2") };
            for (num = 0; num < 4; num++)
            {
                g.DrawString(strArray[num], fontS, brush, 90f * (num + 0.5f), (float) (margin + this.margin), sfc);
                g.DrawLine(pen, num * 90, margin, num * 90, margin + 40);
                g.DrawString(strArray2[num], font, brush, 90f * (num + 0.5f), (float) ((margin + 20) + this.margin), sfc);
            }
            margin += 20;
            g.DrawLine(pen, 0, margin, 400, margin);
            margin += 20;
            g.DrawLine(pen, 0, margin, 400, margin);
            margin += 2;
            g.DrawLine(pen, 0, margin, 400, margin);
            for (num = 0; num < 4; num++)
            {
                int num5 = 0;
                if (num == 3)
                {
                    num5 = -this.margin;
                }
                g.DrawLine(pen, (this.col1Width + (num * this.colWidth)) + num5, margin, (this.col1Width + (num * this.colWidth)) + num5, 600);
            }
            margin += this.margin;
            string[] strArray3 = new string[] { A.Resources.GetString("Description"), A.Resources.GetString("Debits"), A.Resources.GetString("Credits"), A.Resources.GetString("Date"), A.Resources.GetString("Balance") };
            g.DrawString(strArray3[0], fontS, brush, (float) (this.col1Width / 2), (float) margin, sfc);
            for (num = 1; num < 5; num++)
            {
                g.DrawString(strArray3[num], fontS, brush, this.col1Width + ((num - 0.5f) * this.colWidth), (float) margin, sfc);
            }
            margin += 20;
            g.DrawLine(pen, 0, margin, 400, margin);
            margin += this.margin;
            for (int i = page * TransactionsPerPage; i < Math.Min((page + 1) * TransactionsPerPage, list.Count); i++)
            {
                Transaction last = (Transaction) list[i];
                g.DrawString(last.Description, font, brush, (float) this.margin, (float) margin);
                if (last.Type == Transaction.TranType.Debit)
                {
                    g.DrawString(last.Amount.ToString("N2"), font, brush, (float) ((this.col1Width + this.colWidth) - this.margin), (float) margin, sfr);
                }
                else
                {
                    g.DrawString(last.Amount.ToString("N2"), font, brush, (float) ((this.col1Width + (2 * this.colWidth)) - this.margin), (float) margin, sfr);
                }
                g.DrawString(last.Date.ToString("M/d"), font, brush, (float) ((this.col1Width + (3 * this.colWidth)) - (2 * this.margin)), (float) margin, sfr);
                g.DrawString(this.BalanceThru(last).ToString("N2"), font, brush, (float) ((this.col1Width + (4 * this.colWidth)) - this.margin), (float) margin, sfr);
                margin += 11;
            }
        }

        public void StampStatus(Graphics g, int year, int month)
        {
            Font font = new Font("Arial", 10f, FontStyle.Bold);
            Font font2 = new Font("Arial", 24f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.FromArgb(0x80, 0xff, 0, 0));
            switch (this.PastDueStatusPassive(year, month))
            {
                case Status.PastDue:
                    g.DrawString(A.Resources.GetString("Past Due"), font2, brush, g.ClipBounds.Width / 2f, g.ClipBounds.Height / 2f, sfc);
                    break;

                case Status.Deliquent:
                    g.DrawString(A.Resources.GetString("Deliquent"), font2, brush, g.ClipBounds.Width / 2f, g.ClipBounds.Height / 2f, sfc);
                    g.DrawString(A.Resources.GetString("Pay immediately."), font, brush, g.ClipBounds.Width / 2f, (g.ClipBounds.Height / 2f) + 60f, sfc);
                    break;

                case Status.Cancelled:
                    g.DrawString(A.Resources.GetString("In Collection"), font2, brush, g.ClipBounds.Width / 2f, g.ClipBounds.Height / 2f, sfc);
                    break;
            }
        }

        public override string ThingName()
        {
            return A.Resources.GetString("Products");
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { this.AccountTypeFriendlyName, " ", this.BankName, " #", this.AccountNumber };
            return string.Concat(objArray1);
        }

        public ArrayList TransactionsForMonth(int year, int month)
        {
            ArrayList list = new ArrayList();
            foreach (Transaction transaction in this.Transactions)
            {
                if ((transaction.Date >= FirstDayOfCycle(year, month)) && (transaction.Date < LastDayOfCycle(year, month)))
                {
                    list.Add(transaction);
                }
            }
            return list;
        }

        public void ZeroOut()
        {
        }

        public long AccountNumber
        {
            get
            {
                return base.ID;
            }
        }

        public virtual string AccountTypeFriendlyName
        {
            get
            {
                return A.Resources.GetString("Bank");
            }
        }

        public string BankURL
        {
            get
            {
                return ("www." + this.BankName.Replace(" ", "") + ".com");
            }
        }

        public DateTime DateOpened
        {
            get
            {
                return ((Transaction) this.Transactions[0]).Date;
            }
        }

        public float InterestRate
        {
            get
            {
                return this.interestRate;
            }
            set
            {
                this.interestRate = value;
            }
        }

        public enum Status
        {
            Current,
            PastDue,
            Deliquent,
            NewlyCancelled,
            Cancelled
        }
    }
}

