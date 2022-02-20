namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class CreditCardAccount : BankAccount
    {
        protected float creditLimit;
        protected float latePaymentFee;
        public ArrayList PaidMin = new ArrayList();

        public CreditCardAccount(float creditLimit, float interestRate, float latePaymentFee)
        {
            base.Transactions.Add(new Transaction(0f, Transaction.TranType.Credit, "Opening Balance"));
            this.creditLimit = creditLimit;
            base.interestRate = interestRate;
            this.latePaymentFee = latePaymentFee;
        }

        public override int DaysPastDue(DateTime now)
        {
            int num = this.PaidMin.Count - 1;
            int num2 = 0;
            while ((num >= 0) && (((int) this.PaidMin[num]) == -1))
            {
                num2++;
                num--;
            }
            if (num2 == 0)
            {
                return 0;
            }
            return (1 + ((num2 - 1) * 30));
        }

        public override string Description()
        {
            return A.Resources.GetString("Credit card at low APR!. High credit limit. Click for your custom offer!.");
        }

        public override void EndMonth()
        {
            float num = base.AverageDailyBalance(A.State.Year, A.State.Month);
            float num2 = base.Debits(A.State.Year, A.State.Month);
            int month = A.State.Month - 1;
            int year = A.State.Year;
            if (month == 0)
            {
                month = 12;
                year--;
            }
            float num5 = base.EndingBalance(year, month);
            if (num2 < num5)
            {
                base.Transactions.Add(new Transaction((num * base.interestRate) / 12f, Transaction.TranType.Credit, "Finance Charges", A.State.Now.AddDays(-1.0)));
            }
            float num6 = this.MinimumPayment(year, month);
            if (num6 == 0f)
            {
                this.PaidMin.Add(0);
            }
            else if (num2 < (num6 - 0.05f))
            {
                base.Transactions.Add(new Transaction(this.latePaymentFee, Transaction.TranType.Credit, A.Resources.GetString("Late Fee"), A.State.Now.AddDays(-1.0)));
                this.PaidMin.Add(-1);
            }
            else
            {
                this.PaidMin.Add(1);
            }
        }

        public float FinanceCharges(int year, int month)
        {
            ArrayList list = base.TransactionsForMonth(year, month);
            float num = 0f;
            foreach (Transaction transaction in list)
            {
                if (transaction.Description == A.Resources.GetString("Finance Charges"))
                {
                    num += transaction.Amount;
                }
            }
            return num;
        }

        public float MinimumPayment(int year, int month)
        {
            float num = 0.01f * base.EndingBalance(year, month);
            if (num < 0.01)
            {
                return 0f;
            }
            return num;
        }

        public int MissedPayments()
        {
            int num = 0;
            int num2 = 0;
            while ((((this.PaidMin.Count - num) - 1) >= 0) && (num < 0x24))
            {
                if (((int) this.PaidMin[(this.PaidMin.Count - num) - 1]) == -1)
                {
                    num2++;
                }
                num++;
            }
            return num2;
        }

        public int OnTimePayments()
        {
            int num = 0;
            int num2 = 0;
            while ((((this.PaidMin.Count - num) - 1) >= 0) && (num < 0x24))
            {
                if (((int) this.PaidMin[(this.PaidMin.Count - num) - 1]) == 1)
                {
                    num2++;
                }
                num++;
            }
            return num2;
        }

        public float Payments(int year, int month)
        {
            ArrayList list = base.TransactionsForMonth(year, month);
            float num = 0f;
            foreach (Transaction transaction in list)
            {
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num += transaction.Amount;
                }
            }
            return num;
        }

        public override void PrintPage(int page, Graphics g, int year, int month, int Pages, int TransactionsPerPage)
        {
            int margin = base.margin;
            int num2 = 200;
            ArrayList list = base.TransactionsForMonth(year, month);
            g.DrawImageUnscaled(A.Resources.GetImage("Logo" + base.BankName), 4, 4);
            object[] args = new object[] { page + 1, Pages };
            g.DrawString(A.Resources.GetString("Page {0} of {1}", args), BankAccount.font, BankAccount.brush, 320f, (float) margin, BankAccount.sfr);
            margin += 0x19;
            g.DrawString(base.OwnerName, BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 10;
            object[] objArray2 = new object[] { base.AccountNumber };
            g.DrawString(A.Resources.GetString("Account Number: {0}", objArray2), BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 10;
            object[] objArray = new object[] { new DateTime(year, month, 0x1c).ToShortDateString() };
            g.DrawString(A.Resources.GetString("Statement Date: {0}", objArray), BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 10;
            object[] objArray4 = new object[] { Utilities.FC(this.CreditLimit, 2, A.Instance.CurrencyConversion) };
            g.DrawString(A.Resources.GetString("Credit Limit: {0}", objArray4), BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 10;
            object[] objArray5 = new object[] { Utilities.FC(this.CreditLimit - base.EndingBalance(year, month), 2, A.Instance.CurrencyConversion) };
            g.DrawString(A.Resources.GetString("Available Credit: {0}", objArray5), BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 10;
            object[] objArray6 = new object[] { Utilities.FC(this.MinimumPayment(year, month), 2, A.Instance.CurrencyConversion) };
            g.DrawString(A.Resources.GetString("Minimum Payment: {0}", objArray6), BankAccount.fontS, BankAccount.brush, (float) num2, (float) margin);
            margin += 0x19;
            StringFormat format = new StringFormat(BankAccount.sfc) {
                LineAlignment = StringAlignment.Far
            };
            Font font = new Font("Times New Roman", 9f, FontStyle.Bold);
            g.DrawString(A.Resources.GetString("CARDHOLDER SUMMARY"), font, BankAccount.brush, (float) (base.margin + 0xa6), (float) margin, BankAccount.sfc);
            margin += 15;
            g.DrawRectangle(BankAccount.pen, base.margin, margin, 0x145, 0x2d);
            g.DrawLine(BankAccount.pen, 0x37 + base.margin, margin, 0x37 + base.margin, margin + 0x2d);
            g.DrawLine(BankAccount.pen, 270 + base.margin, margin, 270 + base.margin, margin + 0x2d);
            g.DrawLine(BankAccount.pen, base.margin, margin + 30, 0x145 + base.margin, margin + 30);
            string[] strArray = new string[] { A.Resources.GetString("Previous Balance"), A.Resources.GetString("- Payments"), A.Resources.GetString("- Credits"), A.Resources.GetString("+Purchases Fees"), A.Resources.GetString("+ Finance Charges"), A.Resources.GetString("= New Balance") };
            string[] strArray2 = new string[6];
            strArray2[0] = base.BeginningBalance(year, month).ToString("C2");
            strArray2[1] = this.Payments(year, month).ToString("C2");
            strArray2[2] = (base.Debits(year, month) - this.Payments(year, month)).ToString("C2");
            strArray2[3] = (base.Credits(year, month) - this.FinanceCharges(year, month)).ToString("C2");
            strArray2[4] = this.FinanceCharges(year, month).ToString("C2");
            strArray2[5] = base.EndingBalance(year, month).ToString("C2");
            for (int i = 0; i < 6; i++)
            {
                g.DrawString(strArray[i], BankAccount.fontS, BankAccount.brush, new Rectangle(base.margin + (0x37 * i), margin - 15, 0x37, 0x2d), format);
                g.DrawString(strArray2[i], BankAccount.fontS, BankAccount.brush, new Rectangle(base.margin + (0x37 * i), margin + 0x21, 0x37, 15), BankAccount.sfc);
            }
            margin += 60;
            g.DrawString(A.Resources.GetString("CARDHOLDER ACTIVITY"), font, BankAccount.brush, (float) (base.margin + 0xa6), (float) margin, BankAccount.sfc);
            margin += 15;
            g.DrawRectangle(BankAccount.pen, base.margin, margin, 0x145, 0x106);
            g.DrawLine(BankAccount.pen, base.margin, margin + 30, 0x145 + base.margin, margin + 30);
            StringFormat format2 = new StringFormat {
                LineAlignment = StringAlignment.Far
            };
            strArray = new string[] { A.Resources.GetString("Posting Date"), A.Resources.GetString("Transaction"), A.Resources.GetString("Amount") };
            g.DrawString(strArray[0], BankAccount.fontS, BankAccount.brush, new Rectangle(base.margin, margin, 40, 30), format2);
            g.DrawString(strArray[1], BankAccount.fontS, BankAccount.brush, new Rectangle(base.margin + 60, margin, 250, 30), format2);
            g.DrawString(strArray[2], BankAccount.fontS, BankAccount.brush, new Rectangle(base.margin + 0x11d, margin, 50, 30), format2);
            margin += 0x23;
            for (int j = page * TransactionsPerPage; j < Math.Min((page + 1) * TransactionsPerPage, list.Count); j++)
            {
                Transaction transaction = (Transaction) list[j];
                g.DrawString(transaction.Date.ToString("MM-dd"), BankAccount.fontS, BankAccount.brush, (float) (2 * base.margin), (float) margin);
                g.DrawString(transaction.Description, BankAccount.fontS, BankAccount.brush, (float) (base.margin + 60), (float) margin);
                if (transaction.Type == Transaction.TranType.Credit)
                {
                    g.DrawString(transaction.Amount.ToString("N2"), BankAccount.fontS, BankAccount.brush, 325f, (float) margin, BankAccount.sfr);
                }
                else
                {
                    g.DrawString(transaction.Amount.ToString("N2"), BankAccount.fontS, BankAccount.brush, 285f, (float) margin, BankAccount.sfr);
                }
                margin += 9;
            }
            base.StampStatus(g, year, month);
        }

        public override string AccountTypeFriendlyName
        {
            get
            {
                return A.Resources.GetString("Credit Card");
            }
        }

        public float CreditLimit
        {
            get
            {
                return this.creditLimit;
            }
        }

        public float LatePaymentFee
        {
            get
            {
                return this.latePaymentFee;
            }
        }
    }
}

