namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class InvestmentAccount : BankAccount
    {
        public float CostBasis = 0f;
        protected static Font fontB = new Font("Arial", 8f, FontStyle.Bold);
        protected static Font fontSB = new Font("Arial", 7f, FontStyle.Bold);
        public KMI.VBPF1Lib.Fund Fund;
        protected static Font headerFont = new Font("Arial", 12f, FontStyle.Italic | FontStyle.Bold);
        public bool Retirement = false;
        protected static Font titleFont = new Font("Arial", 12f, FontStyle.Bold);

        public InvestmentAccount(KMI.VBPF1Lib.Fund fund)
        {
            this.Fund = fund;
        }

        public override string Description()
        {
            return A.Resources.GetString("Investment account");
        }

        public override void EndMonth()
        {
            float num = (this.Fund.Dividend / 12f) * base.EndingBalance();
            base.Transactions.Add(new Transaction(num / this.Fund.Price, Transaction.TranType.Credit, A.Resources.GetString("Dividend Purchase"), A.State.Now.AddDays(-1.0)));
            if (!this.Retirement)
            {
                this.CostBasis += num;
            }
        }

        public override void PrintPage(int page, Graphics g, int year, int month, int Pages, int TransactionsPerPage)
        {
            int margin = base.margin;
            int num2 = 220;
            int num3 = 330;
            int num4 = 15;
            int num5 = 60;
            int num6 = 220;
            int num7 = 0x113;
            StringFormat format = new StringFormat(BankAccount.sfr) {
                LineAlignment = StringAlignment.Far
            };
            StringFormat format2 = new StringFormat {
                LineAlignment = StringAlignment.Far
            };
            ArrayList list = base.TransactionsForMonth(year, month);
            g.DrawImageUnscaled(A.Resources.GetImage("Logo" + base.BankName), 2, 4);
            margin += 20;
            g.DrawString(A.Resources.GetString("Monthly Fund"), titleFont, BankAccount.brush, (float) num2, (float) margin);
            margin += 20;
            g.DrawString(A.Resources.GetString("Summary"), titleFont, BankAccount.brush, (float) num2, (float) margin);
            margin += 40;
            g.DrawString(A.Resources.GetString(this.Fund.Name), headerFont, BankAccount.brush, (float) base.margin, (float) margin);
            margin += 20;
            g.DrawLine(BankAccount.pen, base.margin, margin, num3, margin);
            margin += 30;
            DateTime t = new DateTime(year, month, 1).AddDays(-1.0);
            DateTime time2 = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            g.DrawString(A.Resources.GetString("Account Value"), fontB, BankAccount.brush, (float) (num3 - 240), (float) margin);
            object[] args = new object[] { t.ToShortDateString() };
            g.DrawString(A.Resources.GetString("On {0}", args), BankAccount.font, BankAccount.brush, (float) (num3 - 0x4b), (float) margin, BankAccount.sfr);
            object[] objArray2 = new object[] { time2.ToShortDateString() };
            g.DrawString(A.Resources.GetString("On {0}", objArray2), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 12;
            g.DrawLine(BankAccount.pen, num3 - 240, margin, num3, margin);
            margin += 3;
            g.DrawString(Utilities.FC(base.BeginningBalance(year, month) * this.Fund.PriceOn(t), 2, A.Instance.CurrencyConversion), BankAccount.font, BankAccount.brush, (float) (num3 - 0x4b), (float) margin, BankAccount.sfr);
            g.DrawString(Utilities.FC(base.EndingBalance(year, month) * this.Fund.PriceOn(time2), 2, A.Instance.CurrencyConversion), fontB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 4 * num4;
            string newLine = Environment.NewLine;
            g.DrawString(A.Resources.GetString("Trade" + newLine + "Date"), fontSB, BankAccount.brush, (float) base.margin, (float) margin, format2);
            g.DrawString(A.Resources.GetString("Transaction"), fontSB, BankAccount.brush, (float) num5, (float) margin, format2);
            g.DrawString(A.Resources.GetString("Dollar" + newLine + "Amount"), fontSB, BankAccount.brush, (float) num6, (float) margin, format);
            g.DrawString(A.Resources.GetString("Shares"), fontSB, BankAccount.brush, (float) num7, (float) margin, format);
            string[] textArray1 = new string[] { "Total", newLine, "Shares", newLine, "Owned" };
            g.DrawString(A.Resources.GetString(string.Concat(textArray1)), fontSB, BankAccount.brush, (float) num3, (float) margin, format);
            margin += 4;
            g.DrawLine(BankAccount.pen, base.margin, margin, num3, margin);
            margin += num4;
            object[] objArray3 = new object[] { t.ToShortDateString() };
            g.DrawString(A.Resources.GetString("Balance on {0}", objArray3), fontSB, BankAccount.brush, (float) num5, (float) margin);
            g.DrawString(base.BeginningBalance(year, month).ToString("N3"), fontSB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            foreach (Transaction transaction in list)
            {
                g.DrawString(transaction.Date.ToString("MM/dd"), fontSB, BankAccount.brush, (float) base.margin, (float) margin);
                g.DrawString(transaction.Description, fontSB, BankAccount.brush, (float) num5, (float) margin);
                g.DrawString(Utilities.FC(transaction.Amount * this.Fund.PriceOn(transaction.Date), 2, A.Instance.CurrencyConversion), fontSB, BankAccount.brush, (float) num6, (float) margin, BankAccount.sfr);
                g.DrawString(transaction.Amount.ToString("N3"), fontSB, BankAccount.brush, (float) num7, (float) margin, BankAccount.sfr);
                g.DrawString(base.BalanceThru(transaction).ToString("N3"), fontSB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
                margin += num4;
            }
            object[] objArray4 = new object[] { time2.ToShortDateString() };
            g.DrawString(A.Resources.GetString("Balance on {0}", objArray4), fontSB, BankAccount.brush, (float) num5, (float) margin);
            g.DrawString(base.EndingBalance(year, month).ToString("N3"), fontSB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
        }

        public int ShareAge()
        {
            float num = 0f;
            foreach (Transaction transaction in base.Transactions)
            {
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num += transaction.Amount;
                }
            }
            foreach (Transaction transaction2 in base.Transactions)
            {
                if (transaction2.Type == Transaction.TranType.Credit)
                {
                    num -= transaction2.Amount;
                    if (num < 0f)
                    {
                        TimeSpan span = (TimeSpan) (A.State.Now - transaction2.Date);
                        return span.Days;
                    }
                }
            }
            return 0;
        }

        public override string AccountTypeFriendlyName
        {
            get
            {
                return A.Resources.GetString("Investment");
            }
        }

        public float Value
        {
            get
            {
                return (this.Fund.Price * base.EndingBalance());
            }
        }
    }
}

