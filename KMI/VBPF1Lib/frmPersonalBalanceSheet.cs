namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmPersonalBalanceSheet : frmDrawnReport
    {
        private static Brush b = new SolidBrush(Color.Black);
        private Timer BalanceUpdater;
        private static Font Bold = new Font("Arial", 9f, FontStyle.Bold);
        private IContainer components;
        private static Font f = new Font("Arial", 9f);
        private static Font fs = new Font("Arial", 8f);
        protected Input input;
        private static Pen p = new Pen(b, 1f);
        private static StringFormat sfc = new StringFormat();
        private static StringFormat sfr = new StringFormat();

        public frmPersonalBalanceSheet()
        {
            this.InitializeComponent();
            A.MainForm.NewDay += new EventHandler(this.NewDayHandler);
        }

        private void BalanceUpdater_Tick(object sender, EventArgs e)
        {
            if (base.GetData())
            {
                base.picReport.Refresh();
            }
        }

        protected override void DrawReportVirtual(Graphics g)
        {
            sfc.Alignment = StringAlignment.Center;
            sfr.Alignment = StringAlignment.Far;
            int width = base.picReport.Width;
            int num2 = 10;
            int num3 = 300;
            int num4 = 370;
            int num5 = 5;
            object[] args = new object[] { this.input.now.ToShortDateString() };
            g.DrawString(A.Resources.GetString("Personal Balance Sheet as of {0}", args), Bold, b, (float) (width / 2), (float) num5, sfc);
            num5 += 0x23;
            g.DrawString(A.Resources.GetString("Assets"), Bold, b, (float) (num2 * 14), (float) num5);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Liquid Assets"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            float amount = 0f;
            g.DrawString(A.Resources.GetString("Cash"), fs, b, (float) num2, (float) num5);
            g.DrawString(Utilities.FC(this.input.cash, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
            num5 += 0x19;
            amount += this.input.cash;
            foreach (BankAccount account in this.input.bankAccounts.Values)
            {
                g.DrawString(account.ToString(), fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(account.EndingBalance(), A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num5 += 0x19;
                amount += account.EndingBalance();
            }
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total liquid assets"), fs, b, (float) (2 * num2), (float) num5);
            g.DrawString(Utilities.FC(amount, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Investment Assets"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            float num7 = 0f;
            foreach (InvestmentAccount account2 in this.input.investmentAccounts.Values)
            {
                g.DrawString(account2.Fund.Name, fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(account2.Value, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num7 += account2.Value;
                num5 += 0x19;
            }
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total investment assets"), fs, b, (float) (2 * num2), (float) num5);
            g.DrawString(Utilities.FC(num7, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Retirement Assets"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            float num8 = 0f;
            foreach (InvestmentAccount account3 in this.input.retirementAccounts.Values)
            {
                g.DrawString(account3.Fund.Name, fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(account3.Value, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num8 += account3.Value;
                num5 += 0x19;
            }
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total retirement assets"), fs, b, (float) (2 * num2), (float) num5);
            g.DrawString(Utilities.FC(num8, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            float num9 = 0f;
            g.DrawString(A.Resources.GetString("Real Property"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            if (this.input.carValue > 0f)
            {
                g.DrawString(A.Resources.GetString("Car"), fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(this.input.carValue, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num5 += 0x19;
            }
            g.DrawString(A.Resources.GetString("Real Estate"), fs, b, (float) num2, (float) num5);
            g.DrawString(Utilities.FC(this.input.realEstateValue, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
            num5 += 0x19;
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total real property"), fs, b, (float) (2 * num2), (float) num5);
            num9 = this.input.carValue + this.input.realEstateValue;
            g.DrawString(Utilities.FC(num9, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Total Assets"), Bold, b, 0f, (float) num5);
            g.DrawString(Utilities.FC(((amount + num7) + num8) + num9, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawLine(p, num4 - 50, num5 - 10, num4, num5 - 10);
            g.DrawLine(p, num4 - 50, num5 - 7, num4, num5 - 7);
            num5 += 10;
            g.DrawString(A.Resources.GetString("Liabilities"), Bold, b, (float) (num2 * 14), (float) num5);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Current Liabilities"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            float num10 = 0f;
            float num11 = 0f;
            foreach (CreditCardAccount account4 in this.input.creditCardAccounts.Values)
            {
                num11 += account4.EndingBalance();
            }
            g.DrawString(A.Resources.GetString("Credit Card Balances"), fs, b, (float) num2, (float) num5);
            g.DrawString(Utilities.FC(num11, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
            num10 += num11;
            num5 += 0x19;
            if (this.input.includeOtherLiabilities)
            {
                float num13 = 0f;
                foreach (BankAccount account5 in this.input.merchantAccounts.Values)
                {
                    if (account5.EndingBalance() > 0f)
                    {
                        num13 += account5.EndingBalance();
                    }
                }
                g.DrawString(A.Resources.GetString("Other Liabilities"), fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(num13, A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num10 += num13;
                num5 += 0x19;
            }
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total current liabilities"), fs, b, (float) (2 * num2), (float) num5);
            g.DrawString(Utilities.FC(num10, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Long-term Liabilities"), Bold, b, 0f, (float) num5);
            num5 += 0x19;
            float num12 = 0f;
            foreach (InstallmentLoan loan in this.input.installmentLoans.Values)
            {
                g.DrawString(loan.BankName, fs, b, (float) num2, (float) num5);
                g.DrawString(Utilities.FC(loan.EndingBalance(), A.Instance.CurrencyConversion), fs, b, (float) num3, (float) num5, sfr);
                num12 += loan.EndingBalance();
                num5 += 0x19;
            }
            g.DrawLine(p, num3 - 50, num5 - 10, num3, num5 - 10);
            g.DrawString(A.Resources.GetString("Total long-term liabilities"), fs, b, (float) (2 * num2), (float) num5);
            g.DrawString(Utilities.FC(num12, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawString(A.Resources.GetString("Total Liabilities"), Bold, b, 0f, (float) num5);
            g.DrawString(Utilities.FC(num10 + num12, A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawLine(p, num4 - 50, num5 - 10, num4, num5 - 10);
            g.DrawLine(p, num4 - 50, num5 - 7, num4, num5 - 7);
            num5 += 10;
            g.DrawString(A.Resources.GetString("Net Worth"), Bold, b, 0f, (float) num5);
            g.DrawString(Utilities.FC((((amount + num7) + num8) + num9) - (num10 + num12), A.Instance.CurrencyConversion), fs, b, (float) num4, (float) num5, sfr);
            num5 += 0x19;
            g.DrawLine(p, num4 - 50, num5 - 10, num4, num5 - 10);
            g.DrawLine(p, num4 - 50, num5 - 7, num4, num5 - 7);
            num5 += 10;
            base.picReport.Height = num5 + 50;
        }

        protected override void frmReport_Closed(object sender, EventArgs e)
        {
            base.frmReport_Closed(sender, e);
            A.MainForm.NewDay -= new EventHandler(this.NewDayHandler);
        }

        protected override void GetDataVirtual()
        {
            this.input = A.Adapter.GetPersonalBalanceSheet(A.MainForm.CurrentEntityID);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.BalanceUpdater = new Timer(this.components);
            ((ISupportInitialize) base.picReport).BeginInit();
            base.SuspendLayout();
            base.pnlBottom.Location = new Point(0, 0x17e);
            base.pnlBottom.Size = new Size(450, 40);
            this.BalanceUpdater.Enabled = true;
            this.BalanceUpdater.Interval = 0x1388;
            this.BalanceUpdater.Tick += new EventHandler(this.BalanceUpdater_Tick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(450, 0x1a6);
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmPersonalBalanceSheet";
            this.Text = "Wealth";
            ((ISupportInitialize) base.picReport).EndInit();
            base.ResumeLayout(false);
        }

        protected void NewDayHandler(object sender, EventArgs e)
        {
            if (base.GetData())
            {
                base.picReport.Refresh();
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public DateTime now;
            public float cash;
            public SortedList bankAccounts;
            public SortedList investmentAccounts;
            public SortedList retirementAccounts;
            public float realEstateValue;
            public SortedList creditCardAccounts;
            public Hashtable merchantAccounts;
            public SortedList installmentLoans;
            public float carValue;
            public bool includeOtherLiabilities;
        }
    }
}

