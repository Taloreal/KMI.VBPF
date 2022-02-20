using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmOpenBankAccount : Form {

        private SortedList accounts;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboAccounts;
        private Container components = null;
        private Label label1;
        private Label label2;
        private RadioButton optCash;
        private RadioButton optTransfer;
        private BankAccount protoAccount;
        private NumericUpDown updInitDeposit;

        public frmOpenBankAccount(int ave, int street, int lot, bool checking) {
            this.InitializeComponent();
            ArrayList list = A.Adapter.GetOfferings(ave, street, lot);
            if (checking) {
                this.protoAccount = (BankAccount) list[0];
                this.Text = A.Resources.GetString("Open Checking Account");
            }
            else {
                this.protoAccount = (BankAccount) list[1];
                this.Text = A.Resources.GetString("Open Savings Account");
            }
            this.accounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in this.accounts.Values) {
                if (account.BankName == ((BankAccount) list[0]).BankName) {
                    this.cboAccounts.Items.Add(account);
                }
            }
            if (this.cboAccounts.Items.Count > 0) {
                this.optTransfer.Enabled = true;
                this.cboAccounts.SelectedIndex = 0;
            }
            decimal cash = new decimal(
                A.Adapter.GetCash(A.MainForm.CurrentEntityID));
            if (cash <= updInitDeposit.Minimum) { return; }
            if (cash >= updInitDeposit.Maximum) { return; }
            updInitDeposit.Value = cash;
        }

        private void btnCancel_Click(object sender, EventArgs e) {
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
                Rectangle rect = this.RectangleToScreen(this.ClientRectangle);
                int height = rect.Top - this.Top;
                long transferFrom = -1L;
                if (this.optTransfer.Checked)
                {
                    transferFrom = ((BankAccount) this.cboAccounts.SelectedItem).AccountNumber;
                }
                A.Adapter.SetBankAccount(A.MainForm.CurrentEntityID, this.protoAccount, (float) this.updInitDeposit.Value, transferFrom);
                MessageBox.Show("Your new account has been opened.", "Congratulations");
                base.Close();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
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

        private void InitializeComponent() {
            this.label2 = new Label();
            this.updInitDeposit = new NumericUpDown();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.label1 = new Label();
            this.optCash = new RadioButton();
            this.optTransfer = new RadioButton();
            this.cboAccounts = new ComboBox();
            this.updInitDeposit.BeginInit();
            base.SuspendLayout();
            this.label2.Location = new Point(40, 20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x84, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "Source of Initial Deposit:";
            this.updInitDeposit.DecimalPlaces = 2;
            this.updInitDeposit.Location = new Point(0x2c, 0x98);
            this.updInitDeposit.Maximum = new decimal(new int[]{ 1000000, 0, 0, 0});
            this.updInitDeposit.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.updInitDeposit.Name = "updInitDeposit";
            this.updInitDeposit.Size = new Size(0x74, 20);
            this.updInitDeposit.TabIndex = 2;
            this.updInitDeposit.TextAlign = HorizontalAlignment.Right;
            this.updInitDeposit.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.btnOK.Location = new Point(0x18, 0xc0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(60, 0x18);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Location = new Point(0x68, 0xc0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(60, 0x18);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0xb8, 0xc0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(60, 0x18);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label1.Location = new Point(0x2c, 0x84);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x84, 0x10);
            this.label1.TabIndex = 10;
            this.label1.Text = "Amount of Initial Deposit:";
            this.optCash.Checked = true;
            this.optCash.Location = new Point(0x38, 0x24);
            this.optCash.Name = "optCash";
            this.optCash.TabIndex = 11;
            this.optCash.TabStop = true;
            this.optCash.Text = "Cash";
            this.optTransfer.Enabled = false;
            this.optTransfer.Location = new Point(0x38, 0x40);
            this.optTransfer.Name = "optTransfer";
            this.optTransfer.Size = new Size(0xb0, 0x10);
            this.optTransfer.TabIndex = 12;
            this.optTransfer.Text = "Other Account at this Bank:";
            this.optTransfer.CheckedChanged += new EventHandler(this.optTransfer_CheckedChanged);
            this.cboAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAccounts.Enabled = false;
            this.cboAccounts.Location = new Point(80, 0x58);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new Size(0x98, 0x15);
            this.cboAccounts.TabIndex = 13;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x10c, 0xea);
            base.Controls.Add(this.cboAccounts);
            base.Controls.Add(this.optTransfer);
            base.Controls.Add(this.optCash);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.updInitDeposit);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmOpenBankAccount";
            base.ShowInTaskbar = false;
            this.Text = "Open New Bank Account";
            this.updInitDeposit.EndInit();
            base.ResumeLayout(false);
        }

        private void optTransfer_CheckedChanged(object sender, EventArgs e)
        {
            this.cboAccounts.Enabled = this.optTransfer.Checked;
        }
    }
}

