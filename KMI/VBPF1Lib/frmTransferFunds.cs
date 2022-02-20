namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTransferFunds : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboFromAccounts;
        private ComboBox cboToAccounts;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        public NumericUpDown updAmount;

        public frmTransferFunds(string bankName)
        {
            this.InitializeComponent();
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (account.BankName == bankName)
                {
                    this.cboFromAccounts.Items.Add(account);
                    this.cboToAccounts.Items.Add(account);
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
            if (this.cboFromAccounts.SelectedIndex == this.cboToAccounts.SelectedIndex)
            {
                MessageBox.Show("Please make sure the From Account is different from the To Account.", "Please Retry");
            }
            else
            {
                try
                {
                    A.Adapter.TransferFunds(A.MainForm.CurrentEntityID, ((BankAccount) this.cboFromAccounts.SelectedItem).AccountNumber, ((BankAccount) this.cboToAccounts.SelectedItem).AccountNumber, (float) this.updAmount.Value);
                    base.Close();
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }

        private void cboFromAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            float num = ((BankAccount) this.cboFromAccounts.SelectedItem).EndingBalance();
            this.updAmount.Value = Math.Max(Math.Min(this.updAmount.Value, (decimal) num), this.updAmount.Minimum);
            this.updAmount.Maximum = (decimal) num;
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
            if (this.cboFromAccounts.Items.Count < 2)
            {
                MessageBox.Show(A.Resources.GetString("You have at least two bank accounts to transfer funds."), Application.ProductName);
                base.Close();
            }
            else
            {
                this.cboFromAccounts.SelectedIndex = 0;
                this.cboToAccounts.SelectedIndex = 1;
            }
        }

        private void InitializeComponent()
        {
            this.cboFromAccounts = new ComboBox();
            this.label1 = new Label();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.label2 = new Label();
            this.updAmount = new NumericUpDown();
            this.label3 = new Label();
            this.cboToAccounts = new ComboBox();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.cboFromAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFromAccounts.Location = new Point(40, 40);
            this.cboFromAccounts.Name = "cboFromAccounts";
            this.cboFromAccounts.Size = new Size(0xd8, 0x15);
            this.cboFromAccounts.TabIndex = 2;
            this.cboFromAccounts.SelectedIndexChanged += new EventHandler(this.cboFromAccounts_SelectedIndexChanged);
            this.label1.Location = new Point(40, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x5c, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "From Account";
            this.btnHelp.Location = new Point(0xc4, 0xa8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x44, 0x18);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x6c, 0xa8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x44, 0x18);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(20, 0xa8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x44, 0x18);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label2.Location = new Point(40, 0x7c);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x10);
            this.label2.TabIndex = 7;
            this.label2.Text = "Amount:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.updAmount.Location = new Point(0x74, 120);
            int[] bits = new int[4];
            bits[0] = 1;
            this.updAmount.Minimum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x60, 20);
            this.updAmount.TabIndex = 8;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            bits = new int[4];
            bits[0] = 20;
            this.updAmount.Value = new decimal(bits);
            this.label3.Location = new Point(40, 0x48);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x5c, 0x10);
            this.label3.TabIndex = 10;
            this.label3.Text = "To Account";
            this.cboToAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboToAccounts.Location = new Point(40, 0x58);
            this.cboToAccounts.Name = "cboToAccounts";
            this.cboToAccounts.Size = new Size(0xd8, 0x15);
            this.cboToAccounts.TabIndex = 9;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x120, 0xce);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cboToAccounts);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboFromAccounts);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmTransferFunds";
            base.ShowInTaskbar = false;
            this.Text = "Transfer Funds";
            base.Load += new EventHandler(this.frmDepositWithdrawCash_Load);
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }
    }
}

