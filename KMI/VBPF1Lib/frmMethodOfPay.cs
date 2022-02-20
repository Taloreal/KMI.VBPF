namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMethodOfPay : Form
    {
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboAccounts;
        private Container components = null;
        private Label label1;
        private RadioButton optCheck;
        private RadioButton optDirectDeposit;
        private long taskID;

        public frmMethodOfPay(long taskID)
        {
            this.InitializeComponent();
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            SortedList list2 = new SortedList();
            foreach (BankAccount account2 in bankAccounts.Values)
            {
                if (account2 is CheckingAccount)
                {
                    list2.Add(account2.AccountNumber, account2);
                }
            }
            BankAccount directDepositAccount = A.Adapter.GetDirectDepositAccount(A.MainForm.CurrentEntityID, taskID);
            if (list2.Count > 0)
            {
                this.optDirectDeposit.Enabled = true;
                foreach (BankAccount account3 in list2.Values)
                {
                    this.cboAccounts.Items.Add(account3);
                }
                if (directDepositAccount != null)
                {
                    this.optDirectDeposit.Checked = true;
                    this.cboAccounts.SelectedIndex = list2.IndexOfKey(directDepositAccount.AccountNumber);
                }
                else
                {
                    this.cboAccounts.SelectedIndex = 0;
                }
            }
            this.taskID = taskID;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Getting Paid"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                long accountNumber = -1L;
                if (this.optDirectDeposit.Checked)
                {
                    accountNumber = ((BankAccount) this.cboAccounts.SelectedItem).AccountNumber;
                }
                A.Adapter.SetDirectDepositAccount(A.MainForm.CurrentEntityID, this.taskID, accountNumber);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
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
            this.optCheck = new RadioButton();
            this.optDirectDeposit = new RadioButton();
            this.cboAccounts = new ComboBox();
            this.btnOK = new Button();
            this.label1 = new Label();
            this.btnHelp = new Button();
            base.SuspendLayout();
            this.optCheck.Checked = true;
            this.optCheck.Location = new Point(40, 0x18);
            this.optCheck.Name = "optCheck";
            this.optCheck.Size = new Size(0x90, 0x10);
            this.optCheck.TabIndex = 0;
            this.optCheck.TabStop = true;
            this.optCheck.Text = "Pay by Check";
            this.optDirectDeposit.Enabled = false;
            this.optDirectDeposit.Location = new Point(40, 0x34);
            this.optDirectDeposit.Name = "optDirectDeposit";
            this.optDirectDeposit.Size = new Size(0x90, 20);
            this.optDirectDeposit.TabIndex = 1;
            this.optDirectDeposit.Text = "Direct Deposit";
            this.optDirectDeposit.CheckedChanged += new EventHandler(this.optDirectDeposit_CheckedChanged);
            this.cboAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAccounts.Enabled = false;
            this.cboAccounts.Location = new Point(0x4c, 0x5c);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new Size(0xb0, 0x15);
            this.cboAccounts.TabIndex = 2;
            this.btnOK.Location = new Point(0x40, 0x94);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x44, 0x18);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label1.Location = new Point(0x4c, 0x4c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x80, 0x10);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bank Account:";
            this.btnHelp.Location = new Point(0x9c, 0x94);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x44, 0x18);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 190);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.cboAccounts);
            base.Controls.Add(this.optDirectDeposit);
            base.Controls.Add(this.optCheck);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmMethodOfPay";
            base.ShowInTaskbar = false;
            this.Text = "Method Of Pay";
            base.ResumeLayout(false);
        }

        private void optDirectDeposit_CheckedChanged(object sender, EventArgs e)
        {
            this.cboAccounts.Enabled = this.optDirectDeposit.Checked;
        }
    }
}

