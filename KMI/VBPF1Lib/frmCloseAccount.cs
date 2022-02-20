namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmCloseAccount : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboAccounts;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;

        public frmCloseAccount(string bankName)
        {
            this.InitializeComponent();
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (account.BankName == bankName)
                {
                    this.cboAccounts.Items.Add(account);
                }
            }
            SortedList creditCardAccounts = A.Adapter.GetCreditCardAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account2 in creditCardAccounts.Values)
            {
                if ((account2.BankName == bankName) && (account2.EndingBalance() <= 0f))
                {
                    this.cboAccounts.Items.Add(account2);
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
                A.Adapter.CloseAccount(A.MainForm.CurrentEntityID, (BankAccount) this.cboAccounts.SelectedItem);
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

        private void frmDepositWithdrawCash_Load(object sender, EventArgs e)
        {
            if (this.cboAccounts.Items.Count == 0)
            {
                MessageBox.Show(A.Resources.GetString("You do not have any accounts open at this bank."), Application.ProductName);
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
            this.label3 = new Label();
            base.SuspendLayout();
            this.cboAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAccounts.Location = new Point(40, 0x24);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new Size(0xd8, 0x15);
            this.cboAccounts.TabIndex = 2;
            this.label1.Location = new Point(40, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x94, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "Account to Close:";
            this.btnHelp.Location = new Point(0xc4, 0x98);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x44, 0x18);
            this.btnHelp.TabIndex = 6;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x6c, 0x98);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x44, 0x18);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(20, 0x98);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x44, 0x18);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label2.Location = new Point(60, 0x48);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xa4, 0x20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Any remaining funds will be refunded to you in cash. ";
            this.label2.TextAlign = ContentAlignment.TopCenter;
            this.label3.Location = new Point(60, 0x70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(160, 0x20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Only credit cards with zero balances are listed.";
            this.label3.TextAlign = ContentAlignment.TopCenter;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x120, 0xc2);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboAccounts);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmCloseAccount";
            base.ShowInTaskbar = false;
            this.Text = "Close Account";
            base.Load += new EventHandler(this.frmDepositWithdrawCash_Load);
            base.ResumeLayout(false);
        }
    }
}

