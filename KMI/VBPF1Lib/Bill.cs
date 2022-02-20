namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Bill
    {
        public BankAccount Account;
        public float Amount;
        protected static Brush brush = new SolidBrush(Color.Black);
        protected int col1Width = 120;
        protected int colWidth = 60;
        public DateTime Date;
        protected static Font font = new Font("Arial", 8f);
        protected static Font font2;
        protected static Font fontL = new Font("Arial", 10f, FontStyle.Bold);
        protected static Font fontXL = new Font("Arial", 24f, FontStyle.Bold);
        public string From;
        public long ID;
        protected int margin = 5;
        protected static Pen pen = new Pen(brush, 1f);
        protected static Brush red = new SolidBrush(Color.FromArgb(0x80, 0xff, 0, 0));
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        protected static Brush white = new SolidBrush(Color.White);

        static Bill()
        {
            sfc.Alignment = StringAlignment.Center;
            sfr.Alignment = StringAlignment.Far;
            try
            {
                font2 = new Font("Courier New", 8f);
            }
            catch (Exception)
            {
                font2 = new Font("Arial", 8f);
            }
        }

        public Bill(string from, string description, float amount, BankAccount account)
        {
            this.From = from;
            this.Amount = amount;
            this.Account = account;
            this.Date = A.State.Now;
            this.ID = A.State.GetNextID();
            if ((account != null) && (!(account is CreditCardAccount) && !(account is InstallmentLoan)))
            {
                this.Account.Transactions.Add(new Transaction(amount, Transaction.TranType.Credit, description, A.State.Now.AddDays(-1.0)));
            }
        }

        public void PrintPage(Graphics g)
        {
            int x = 80;
            int num2 = 150;
            int num3 = 200;
            int num4 = 0x141;
            int num5 = 14;
            int num6 = 0x40;
            int num7 = 0xf8;
            int num8 = 0x141;
            int num9 = 50;
            int num10 = 130;
            int num11 = 210;
            int num12 = 0x141;
            int height = 0x12;
            int num14 = 0x1a9;
            int year = this.Date.Year;
            int month = this.Date.Month;
            int y = 20;
            BankAccount account = this.Account;
            g.DrawImageUnscaled(A.Resources.GetImage("Logo" + account.BankName), 4, 4);
            object[] args = new object[] { this.ID };
            g.DrawString(A.Resources.GetString("Invoice #: {0}", args), fontL, brush, (float) num12, (float) (y - 12), sfr);
            y += 70;
            g.FillRectangle(brush, x, y, num4 - x, height);
            g.DrawString(A.Resources.GetString("DATE"), font, white, (float) ((num2 + x) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("TERMS"), font, white, (float) ((num3 + num2) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("CUSTOMER NAME"), font, white, (float) ((num4 + num3) / 2), (float) (y + 2), sfc);
            g.DrawString(this.Date.ToShortDateString(), font2, brush, (float) ((num2 + x) / 2), (float) ((y + height) + 3), sfc);
            g.DrawString(A.Resources.GetString("Net 30"), font2, brush, (float) ((num3 + num2) / 2), (float) ((y + height) + 3), sfc);
            g.DrawString(this.Account.OwnerName.ToUpper(), font2, brush, (float) ((num4 + num3) / 2), (float) ((y + height) + 3), sfc);
            g.DrawLine(pen, x, y, x, y + (2 * height));
            g.DrawLine(pen, num2, y, num2, y + (2 * height));
            g.DrawLine(pen, num3, y, num3, y + (2 * height));
            g.DrawLine(pen, num4, y, num4, y + (2 * height));
            y += 2 * height;
            g.FillRectangle(brush, num5, y, num8 - num5, height);
            g.DrawString(A.Resources.GetString("DATE"), font, white, (float) ((num6 + num5) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("DESCRIPTION"), font, white, (float) ((num7 + num6) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("AMOUNT"), font, white, (float) ((num8 + num7) / 2), (float) (y + 2), sfc);
            g.DrawLine(pen, num5, y, num5, num14);
            g.DrawLine(pen, num6, y, num6, num14);
            g.DrawLine(pen, num7, y, num7, num14);
            g.DrawLine(pen, num8, y, num8, num14);
            g.DrawString(A.Resources.GetString("Balance Forward"), font2, brush, (float) (num6 + this.margin), (float) ((y + height) + 3));
            g.DrawString(this.Account.BeginningBalance(year, month).ToString("N2"), font2, brush, (float) (num8 - this.margin), (float) ((y + height) + 3), sfr);
            y += height;
            ArrayList list = this.Account.TransactionsForMonth(year, month);
            foreach (Transaction transaction in list)
            {
                g.DrawString(transaction.Date.ToString("MM/dd"), font2, brush, (float) (num5 + this.margin), (float) ((y + height) + 3));
                g.DrawString(transaction.Description, font2, brush, (float) (num6 + this.margin), (float) ((y + height) + 3));
                float amount = transaction.Amount;
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    amount = -amount;
                }
                g.DrawString(amount.ToString("N2"), font2, brush, (float) (num8 - this.margin), (float) ((y + height) + 3), sfr);
                y += height;
            }
            y = num14;
            g.FillRectangle(brush, num5, y, num8 - num5, height);
            g.DrawString(A.Resources.GetString("PAST DUE"), font, white, (float) ((num10 + num9) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("CURRENT"), font, white, (float) ((num11 + num10) / 2), (float) (y + 2), sfc);
            g.DrawString(A.Resources.GetString("AMOUNT DUE"), font, white, (float) ((num12 + num11) / 2), (float) (y + 2), sfc);
            g.DrawLine(pen, num9, y, num9, y + (2 * height));
            g.DrawLine(pen, num10, y, num10, y + (2 * height));
            g.DrawLine(pen, num11, y, num11, y + (2 * height));
            g.DrawLine(pen, num12, y, num12, y + (2 * height));
            g.DrawLine(pen, num9, y + (2 * height), num12, y + (2 * height));
            g.DrawString(this.Account.PastDueAmount(this.Date).ToString("N2"), font2, brush, (float) (num10 - this.margin), (float) ((y + height) + 3), sfr);
            g.DrawString(this.Account.CurrentCharges(this.Date).ToString("N2"), font2, brush, (float) (num11 - this.margin), (float) ((y + height) + 3), sfr);
            g.DrawString(this.Account.EndingBalance(year, month).ToString("N2"), font2, brush, (float) (num12 - this.margin), (float) ((y + height) + 3), sfr);
            this.Account.StampStatus(g, year, month);
        }
    }
}

