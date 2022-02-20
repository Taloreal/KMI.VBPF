namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDepositWithdrawCash : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboAccounts;
        private Container components = null;
        private Label label1;
        private Label label2;
        public NumericUpDown updAmount;
        private bool withdraw;

        public frmDepositWithdrawCash(string bankName, bool withdraw)
        {
            this.InitializeComponent();
            this.withdraw = withdraw;
            if (withdraw)
            {
                this.Text = A.Resources.GetString("Withdraw Cash");
            }
            else
            {
                this.Text = A.Resources.GetString("Deposit Funds");
            }
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (!withdraw || (account.BankName == bankName))
                {
                    this.cboAccounts.Items.Add(account);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                A.Adapter.SetDepositWithdrawCash(A.MainForm.CurrentEntityID, this.withdraw, (float) this.updAmount.Value, ((BankAccount) this.cboAccounts.SelectedItem).AccountNumber);
                base.Close();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void cboAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.withdraw)
            {
                float num = ((BankAccount) this.cboAccounts.SelectedItem).EndingBalance();
                if (num < 0f)
                {
                    num = 0f;
                }
                this.updAmount.Value = Math.Min(this.updAmount.Value, (decimal) num);
                this.updAmount.Maximum = (decimal) num;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmDepositWithdrawCash_Load(object sender, EventArgs e)
        {
            if (this.cboAccounts.Items.Count == 0)
            {
                MessageBox.Show(A.Resources.GetString("You must open a bank account first before depositing or withdrawing funds."), Application.ProductName);
                base.Close();
            }
            else
            {
                this.cboAccounts.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.cboAccounts = new ComboBox();
            this.label1 = new Label();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.label2 = new Label();
            this.updAmount = new NumericUpDown();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.cboAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAccounts.Location = new Point(40, 0x2c);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new Size(0xd8, 0x15);
            this.cboAccounts.TabIndex = 2;
            this.cboAccounts.SelectedIndexChanged += new EventHandler(this.cboAccounts_SelectedIndexChanged);
            this.label1.Location = new Point(40, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5c, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "Account";
            this.btnHelp.Location = new Point(0xc4, 0x84);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x44, 0x18);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x6c, 0x84);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x44, 0x18);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(20, 0x84);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x44, 0x18);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label2.Location = new Point(40, 0x54);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "Amount:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            int[] bits = new int[4];
            bits[0] = 20;
            this.updAmount.Increment = new decimal(bits);
            this.updAmount.Location = new Point(0x74, 80);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x60, 20);
            this.updAmount.TabIndex = 8;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            bits = new int[4];
            bits[0] = 20;
            this.updAmount.Value = new decimal(bits);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x120, 0xae);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboAccounts);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmDepositWithdrawCash";
            base.ShowInTaskbar = false;
            this.Text = "#";
            base.Load += new EventHandler(this.frmDepositWithdrawCash_Load);
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }
    }
}

