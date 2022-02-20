namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FundControl : UserControl
    {
        private InvestmentAccount account;
        private Container components;
        private Label labChange;
        private Label labPrice;
        private Label labShares;
        private Label labValue;
        private LinkLabel lnkBuy;
        private LinkLabel lnkFundName;
        private LinkLabel lnkSell;
        private bool retirement;

        public FundControl()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public FundControl(InvestmentAccount f, bool retirement)
        {
            this.components = null;
            this.InitializeComponent();
            this.account = f;
            this.lnkFundName.Text = this.account.Fund.Name;
            this.labShares.Text = this.account.EndingBalance().ToString("N2");
            this.labPrice.Text = this.account.Fund.Price.ToString("C3");
            float num = this.account.Fund.Price - this.account.Fund.Previous;
            if (num > 0f)
            {
                this.labChange.Text = "+" + num.ToString("N2");
                this.labChange.ForeColor = Color.Green;
            }
            else if (num < 0f)
            {
                this.labChange.Text = num.ToString("N2");
                this.labChange.ForeColor = Color.Red;
            }
            else
            {
                this.labChange.Text = "--";
            }
            this.labValue.Text = Utilities.FC(this.account.EndingBalance() * this.account.Fund.Price, 2, A.Instance.CurrencyConversion);
            this.retirement = retirement;
            this.lnkBuy.Visible = !retirement;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labShares = new Label();
            this.labPrice = new Label();
            this.labChange = new Label();
            this.labValue = new Label();
            this.lnkFundName = new LinkLabel();
            this.lnkBuy = new LinkLabel();
            this.lnkSell = new LinkLabel();
            base.SuspendLayout();
            this.labShares.Location = new Point(0x80, 8);
            this.labShares.Name = "labShares";
            this.labShares.Size = new Size(40, 0x18);
            this.labShares.TabIndex = 1;
            this.labShares.Text = "label2";
            this.labShares.TextAlign = ContentAlignment.MiddleRight;
            this.labPrice.Location = new Point(0xac, 8);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(0x38, 0x18);
            this.labPrice.TabIndex = 2;
            this.labPrice.Text = "label3";
            this.labPrice.TextAlign = ContentAlignment.MiddleRight;
            this.labChange.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labChange.Location = new Point(0xe4, 8);
            this.labChange.Name = "labChange";
            this.labChange.Size = new Size(0x24, 0x18);
            this.labChange.TabIndex = 3;
            this.labChange.Text = "label4";
            this.labChange.TextAlign = ContentAlignment.MiddleRight;
            this.labValue.Location = new Point(0x110, 8);
            this.labValue.Name = "labValue";
            this.labValue.Size = new Size(0x40, 0x18);
            this.labValue.TabIndex = 4;
            this.labValue.Text = "label5";
            this.labValue.TextAlign = ContentAlignment.MiddleRight;
            this.lnkFundName.Location = new Point(4, 4);
            this.lnkFundName.Name = "lnkFundName";
            this.lnkFundName.Size = new Size(120, 0x20);
            this.lnkFundName.TabIndex = 5;
            this.lnkFundName.TabStop = true;
            this.lnkFundName.Text = "linkLabel1";
            this.lnkFundName.TextAlign = ContentAlignment.MiddleLeft;
            this.lnkFundName.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkFundName_LinkClicked);
            this.lnkBuy.Location = new Point(0x158, 8);
            this.lnkBuy.Name = "lnkBuy";
            this.lnkBuy.Size = new Size(0x1a, 0x18);
            this.lnkBuy.TabIndex = 6;
            this.lnkBuy.TabStop = true;
            this.lnkBuy.Text = "Buy";
            this.lnkBuy.TextAlign = ContentAlignment.MiddleLeft;
            this.lnkBuy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkBuy_LinkClicked);
            this.lnkSell.Location = new Point(0x174, 8);
            this.lnkSell.Name = "lnkSell";
            this.lnkSell.Size = new Size(0x1a, 0x18);
            this.lnkSell.TabIndex = 7;
            this.lnkSell.TabStop = true;
            this.lnkSell.Text = "Sell";
            this.lnkSell.TextAlign = ContentAlignment.MiddleLeft;
            this.lnkSell.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkSell_LinkClicked);
            this.BackColor = Color.White;
            base.Controls.Add(this.lnkSell);
            base.Controls.Add(this.lnkBuy);
            base.Controls.Add(this.lnkFundName);
            base.Controls.Add(this.labValue);
            base.Controls.Add(this.labChange);
            base.Controls.Add(this.labPrice);
            base.Controls.Add(this.labShares);
            base.Name = "FundControl";
            base.Size = new Size(0x194, 40);
            base.ResumeLayout(false);
        }

        private void lnkBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                new frmTrade(this.retirement, true, this.account.Fund).ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void lnkFundName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            A.MainForm.mnuActionsInvestingResearchFunds_Click(this.lnkFundName.Text, new EventArgs());
        }

        private void lnkSell_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (!this.retirement || (MessageBox.Show("If you are under 59 1/2 years old, you will incur a 10% penalty by withdrawing money from this account. Assume you were born in January 1, 1992. Do you want to continue?", "Confirm Withdrawal", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    new frmTrade(this.retirement, false, this.account.Fund).ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }
    }
}

