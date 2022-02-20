namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTrade : Form
    {
        private SortedList bankAccounts;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private bool buy;
        private ComboBox cboFund;
        private ComboBox cboSource;
        private Container components = null;
        private ArrayList funds;
        private Label label4;
        private Label label5;
        private Label labSource;
        private SortedList myAccounts;
        private bool retirement = false;
        private NumericUpDown updAmount;

        public frmTrade(bool retirement, bool buy, Fund targetFund)
        {
            this.InitializeComponent();
            this.funds = A.Adapter.GetFunds();
            this.bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            this.myAccounts = A.Adapter.GetInvestmentAccounts(A.MainForm.CurrentEntityID, retirement);
            foreach (Fund fund in this.funds)
            {
                this.cboFund.Items.Add(fund.Name);
                if ((targetFund != null) && (targetFund.Name == fund.Name))
                {
                    this.cboFund.SelectedIndex = this.cboFund.Items.Count - 1;
                }
            }
            this.buy = buy;
            this.LimitAmounts();
            foreach (BankAccount account in this.bankAccounts.Values)
            {
                if (account is CheckingAccount)
                {
                    this.cboSource.Items.Add(account);
                }
            }
            this.cboSource.Items.Add("Cash");
            this.cboSource.SelectedIndex = 0;
            this.retirement = retirement;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cboFund.SelectedItem == null)
                {
                    MessageBox.Show(A.Resources.GetString("You must select a fund to buy or sell."));
                }
                else
                {
                    long accountNumber = -1L;
                    if (this.cboSource.SelectedItem is BankAccount)
                    {
                        accountNumber = ((BankAccount) this.cboSource.SelectedItem).AccountNumber;
                    }
                    if (this.buy)
                    {
                        A.Adapter.BuyFund(A.MainForm.CurrentEntityID, this.cboFund.SelectedItem.ToString(), (float) this.updAmount.Value, accountNumber);
                    }
                    else
                    {
                        A.Adapter.SellFund(A.MainForm.CurrentEntityID, this.cboFund.SelectedItem.ToString(), (float) this.updAmount.Value, accountNumber, this.retirement);
                    }
                    base.Close();
                    foreach (Form form in A.MainForm.OwnedForms)
                    {
                        if (form is frmMyPortfolio)
                        {
                            ((frmMyPortfolio) form).RefreshData();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            base.Close();
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
            this.updAmount = new NumericUpDown();
            this.label4 = new Label();
            this.label5 = new Label();
            this.cboFund = new ComboBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.cboSource = new ComboBox();
            this.labSource = new Label();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.updAmount.Increment = new decimal(bits);
            this.updAmount.Location = new Point(0x88, 0x40);
            bits = new int[4];
            bits[0] = 0xf4240;
            this.updAmount.Maximum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.TabIndex = 9;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.label4.Location = new Point(0x60, 0x18);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 0x10);
            this.label4.TabIndex = 15;
            this.label4.Text = "Fund:";
            this.label5.Location = new Point(0x38, 0x40);
            this.label5.Name = "label5";
            this.label5.Size = new Size(80, 0x10);
            this.label5.TabIndex = 0x10;
            this.label5.Text = "Dollar Amount:";
            this.cboFund.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFund.Location = new Point(0x88, 0x18);
            this.cboFund.Name = "cboFund";
            this.cboFund.Size = new Size(120, 0x15);
            this.cboFund.TabIndex = 0x11;
            this.btnOK.Location = new Point(0x18, 160);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 0x13;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x88, 160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.button2_Click);
            this.btnHelp.Location = new Point(0xf8, 160);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 0x15;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSource.Location = new Point(0x88, 0x68);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new Size(120, 0x15);
            this.cboSource.TabIndex = 0x16;
            this.labSource.Location = new Point(8, 0x68);
            this.labSource.Name = "labSource";
            this.labSource.Size = new Size(120, 0x18);
            this.labSource.TabIndex = 0x17;
            this.labSource.Text = "Withdraw money from:";
            this.labSource.TextAlign = ContentAlignment.TopRight;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x170, 0xce);
            base.Controls.Add(this.labSource);
            base.Controls.Add(this.cboSource);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.cboFund);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.updAmount);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmTrade";
            base.ShowInTaskbar = false;
            this.Text = "Trade";
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }

        private void LimitAmounts()
        {
            if (this.buy)
            {
                this.updAmount.Value = Math.Min(1000000M, this.updAmount.Value);
                this.updAmount.Maximum = 1000000M;
                this.labSource.Text = "Withdraw money from:";
                this.Text = "Buy Shares";
            }
            else
            {
                decimal num = new decimal();
                InvestmentAccount account = null;
                foreach (InvestmentAccount account2 in this.myAccounts.Values)
                {
                    if ((this.cboFund.SelectedIndex > -1) && (account2.Fund.Name == this.cboFund.SelectedItem.ToString()))
                    {
                        account = account2;
                    }
                }
                if (account != null)
                {
                    num = (decimal) account.Value;
                    if (num < decimal.Zero)
                    {
                        num = new decimal();
                    }
                    this.updAmount.Value = Math.Min(num, this.updAmount.Value);
                    this.updAmount.Maximum = num;
                    this.labSource.Text = "Deposit money to:";
                }
                this.Text = "Sell Shares";
            }
        }
    }
}

