using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmOnlineBanking : Form
    {
        private BankAccount account;
        private SortedList bankAccounts;
        protected static Brush brush = new SolidBrush(Color.Gray);
        private Button btnDoneRecurring;
        private Button btnSchedulePayments;
        private Button btnTransfer;
        private Button button1;
        private ComboBox cboFrom;
        private ComboBox cboPayAccount;
        private ComboBox cboPayAccount2;
        private ComboBox cboTo;
        private ComboBox cboURLs;
        protected int colWidth = 80;
        private Container components = null;
        protected static Font font = new Font("Arial", 10f);
        protected static Font fontS = new Font("Arial", 10f, FontStyle.Bold);
        private Label labAccountBalance;
        private Label labAccountNumber;
        private Label labBankName;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labFromBal;
        private Label labToBal;
        private LinkLabel linkBack2;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel3;
        private LinkLabel linkPaybills;
        private LinkLabel linkRecurring;
        private LinkLabel linkTransfer;
        protected int margin = 5;
        private Panel panAccountDetail;
        private Panel panAccounts;
        private Panel panel1;
        private Panel panHome;
        private Panel panPayBills;
        private Panel panPayees;
        private Panel panPayees2;
        private Panel panRecurring;
        private Panel panTransactions;
        private Panel panTransfer;
        private ArrayList payees;
        protected static Pen pen = new Pen(brush, 1f);
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        private NumericUpDown updAmount;
        protected string url = null;

        public frmOnlineBanking()
        {
            this.InitializeComponent();
            sfr.Alignment = StringAlignment.Far;
            base.Size = new Size(0x248, 0x1a8);
            this.bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            this.payees = A.Adapter.GetPayees(A.MainForm.CurrentEntityID);
            if (this.bankAccounts.Count == 0)
            {
                throw new SimApplicationException(A.Resources.GetString("You do not have any bank accounts open. You need a bank account to do online banking. To open a bank account, click a bank in the City view."));
            }
            foreach (BankAccount account in this.bankAccounts.Values)
            {
                if (!this.cboURLs.Items.Contains(account.BankURL))
                {
                    this.cboURLs.Items.Add(account.BankURL);
                }
            }
            this.cboURLs.SelectedIndex = 0;
        }

        private void btnDoneRecurring_Click(object sender, EventArgs e)
        {
            Exception exception;
            ArrayList payments = new ArrayList();
            foreach (RecurringPaymentControl control in this.panPayees2.Controls)
            {
                try
                {
                    RecurringPayment recurringPayment = control.RecurringPayment;
                    if (recurringPayment != null)
                    {
                        payments.Add(recurringPayment);
                    }
                }
                catch (Exception exception2)
                {
                    exception = exception2;
                    object[] args = new object[] { control.PayeeName.Text };
                    MessageBox.Show(A.Resources.GetString("Invalid amount entered for {0}. Please correct.", args), A.Resources.GetString("Invalid Entry"));
                    return;
                }
            }
            try
            {
                A.Adapter.SetRecurringPayments(A.MainForm.CurrentEntityID, ((BankAccount) this.cboPayAccount2.SelectedItem).AccountNumber, payments);
                MessageBox.Show(A.Resources.GetString("Your recurring payments have been set up."), A.Resources.GetString("Success"));
            }
            catch (Exception exception3)
            {
                exception = exception3;
                frmExceptionHandler.Handle(exception, this);
            }
            this.ReturnToHome();
        }

        private void btnSchedulePayments_Click(object sender, EventArgs e)
        {
            Exception exception;
            Hashtable payments = new Hashtable();
            foreach (Control control in this.panPayees.Controls)
            {
                if ((control is TextBox) && (control.Text != ""))
                {
                    float num = 0f;
                    try
                    {
                        num = float.Parse(control.Text);
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        object[] args = new object[] { control.Tag };
                        MessageBox.Show(A.Resources.GetString("Invalid amount entered for {0}. Please correct.", args), A.Resources.GetString("Invalid Entry"));
                        return;
                    }
                    if (num > 0f)
                    {
                        payments.Add(control.Tag, num);
                    }
                }
            }
            try
            {
                A.Adapter.SchedulePayments(A.MainForm.CurrentEntityID, ((BankAccount) this.cboPayAccount.SelectedItem).AccountNumber, payments);
                MessageBox.Show(A.Resources.GetString("Your payments have been scheduled."), A.Resources.GetString("Success"));
            }
            catch (Exception exception3)
            {
                exception = exception3;
                frmExceptionHandler.Handle(exception, this);
            }
            this.ReturnToHome();
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                BankAccount selectedItem = (BankAccount) this.cboFrom.SelectedItem;
                BankAccount account2 = (BankAccount) this.cboTo.SelectedItem;
                A.Adapter.TransferFunds(A.MainForm.CurrentEntityID, selectedItem.AccountNumber, account2.AccountNumber, (float) this.updAmount.Value);
                MessageBox.Show(A.Resources.GetString("Funds transferred successfully."), A.Resources.GetString("Transfer Funds"));
                this.ReturnToHome();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void cboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboTo.Items.Clear();
            foreach (BankAccount account2 in this.bankAccounts.Values)
            {
                if (account2 != this.cboFrom.SelectedItem)
                {
                    this.cboTo.Items.Add(account2);
                }
            }
            this.cboTo.SelectedIndex = 0;
            BankAccount selectedItem = (BankAccount) this.cboFrom.SelectedItem;
            this.labFromBal.Text = Utilities.FC(selectedItem.EndingBalance(), 2, A.Instance.CurrencyConversion);
            this.updAmount.Maximum = (decimal) selectedItem.EndingBalance();
        }

        private void cboPayAccount2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (RecurringPayment payment in ((CheckingAccount) this.cboPayAccount2.SelectedItem).RecurringPayments)
            {
                foreach (RecurringPaymentControl control in this.panPayees2.Controls)
                {
                    if (payment.PayeeAccountNumber == control.PayeeAccountNumber)
                    {
                        control.RecurringPayment = payment;
                    }
                }
            }
        }

        private void cboTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labToBal.Text = Utilities.FC(((BankAccount) this.cboTo.SelectedItem).EndingBalance(), 2, A.Instance.CurrencyConversion);
        }

        private void cboURLs_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.URL = this.cboURLs.SelectedItem.ToString();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cboURLs = new System.Windows.Forms.ComboBox();
            this.panHome = new System.Windows.Forms.Panel();
            this.panAccounts = new System.Windows.Forms.Panel();
            this.linkRecurring = new System.Windows.Forms.LinkLabel();
            this.labBankName = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkTransfer = new System.Windows.Forms.LinkLabel();
            this.linkPaybills = new System.Windows.Forms.LinkLabel();
            this.panAccountDetail = new System.Windows.Forms.Panel();
            this.labAccountNumber = new System.Windows.Forms.Label();
            this.labAccountBalance = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.panTransactions = new System.Windows.Forms.Panel();
            this.panPayBills = new System.Windows.Forms.Panel();
            this.cboPayAccount = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSchedulePayments = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.linkBack2 = new System.Windows.Forms.LinkLabel();
            this.panPayees = new System.Windows.Forms.Panel();
            this.panTransfer = new System.Windows.Forms.Panel();
            this.labToBal = new System.Windows.Forms.Label();
            this.labFromBal = new System.Windows.Forms.Label();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.updAmount = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.cboTo = new System.Windows.Forms.ComboBox();
            this.cboFrom = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panRecurring = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.cboPayAccount2 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.btnDoneRecurring = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.panPayees2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panHome.SuspendLayout();
            this.panAccountDetail.SuspendLayout();
            this.panPayBills.SuspendLayout();
            this.panTransfer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updAmount)).BeginInit();
            this.panRecurring.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cboURLs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 46);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Location = new System.Drawing.Point(576, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 28);
            this.button1.TabIndex = 2;
            this.button1.Text = "Go";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Addresss";
            // 
            // cboURLs
            // 
            this.cboURLs.Location = new System.Drawing.Point(96, 9);
            this.cboURLs.Name = "cboURLs";
            this.cboURLs.Size = new System.Drawing.Size(461, 24);
            this.cboURLs.TabIndex = 0;
            this.cboURLs.SelectedIndexChanged += new System.EventHandler(this.cboURLs_SelectedIndexChanged);
            // 
            // panHome
            // 
            this.panHome.BackColor = System.Drawing.Color.White;
            this.panHome.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panHome.Controls.Add(this.panAccounts);
            this.panHome.Controls.Add(this.linkRecurring);
            this.panHome.Controls.Add(this.labBankName);
            this.panHome.Controls.Add(this.label3);
            this.panHome.Controls.Add(this.label2);
            this.panHome.Controls.Add(this.linkTransfer);
            this.panHome.Controls.Add(this.linkPaybills);
            this.panHome.Location = new System.Drawing.Point(0, 46);
            this.panHome.Name = "panHome";
            this.panHome.Size = new System.Drawing.Size(499, 332);
            this.panHome.TabIndex = 1;
            // 
            // panAccounts
            // 
            this.panAccounts.AutoScroll = true;
            this.panAccounts.Location = new System.Drawing.Point(29, 129);
            this.panAccounts.Name = "panAccounts";
            this.panAccounts.Size = new System.Drawing.Size(182, 231);
            this.panAccounts.TabIndex = 8;
            // 
            // linkRecurring
            // 
            this.linkRecurring.Location = new System.Drawing.Point(19, 46);
            this.linkRecurring.Name = "linkRecurring";
            this.linkRecurring.Size = new System.Drawing.Size(144, 19);
            this.linkRecurring.TabIndex = 7;
            this.linkRecurring.TabStop = true;
            this.linkRecurring.Text = "Recurring Payments";
            this.linkRecurring.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRecurring_LinkClicked);
            // 
            // labBankName
            // 
            this.labBankName.Font = new System.Drawing.Font("Impact", 26.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labBankName.ForeColor = System.Drawing.Color.Silver;
            this.labBankName.Location = new System.Drawing.Point(221, 249);
            this.labBankName.Name = "labBankName";
            this.labBankName.Size = new System.Drawing.Size(413, 83);
            this.labBankName.TabIndex = 6;
            this.labBankName.Text = "label4";
            this.labBankName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Impact", 36F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label3.Location = new System.Drawing.Point(221, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(413, 222);
            this.label3.TabIndex = 5;
            this.label3.Text = "Welcome to Online Banking at";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(19, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(183, 27);
            this.label2.TabIndex = 4;
            this.label2.Text = "Account Details:";
            // 
            // linkTransfer
            // 
            this.linkTransfer.Location = new System.Drawing.Point(19, 74);
            this.linkTransfer.Name = "linkTransfer";
            this.linkTransfer.Size = new System.Drawing.Size(115, 18);
            this.linkTransfer.TabIndex = 1;
            this.linkTransfer.TabStop = true;
            this.linkTransfer.Text = "Transfer Funds";
            this.linkTransfer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkTransfer_LinkClicked);
            // 
            // linkPaybills
            // 
            this.linkPaybills.Location = new System.Drawing.Point(19, 18);
            this.linkPaybills.Name = "linkPaybills";
            this.linkPaybills.Size = new System.Drawing.Size(115, 19);
            this.linkPaybills.TabIndex = 0;
            this.linkPaybills.TabStop = true;
            this.linkPaybills.Text = "Pay Bills";
            this.linkPaybills.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkPaybills_LinkClicked);
            // 
            // panAccountDetail
            // 
            this.panAccountDetail.AutoScroll = true;
            this.panAccountDetail.BackColor = System.Drawing.Color.White;
            this.panAccountDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panAccountDetail.Controls.Add(this.labAccountNumber);
            this.panAccountDetail.Controls.Add(this.labAccountBalance);
            this.panAccountDetail.Controls.Add(this.label14);
            this.panAccountDetail.Controls.Add(this.label13);
            this.panAccountDetail.Controls.Add(this.linkLabel1);
            this.panAccountDetail.Controls.Add(this.panTransactions);
            this.panAccountDetail.Location = new System.Drawing.Point(518, 65);
            this.panAccountDetail.Name = "panAccountDetail";
            this.panAccountDetail.Size = new System.Drawing.Size(672, 193);
            this.panAccountDetail.TabIndex = 2;
            this.panAccountDetail.Visible = false;
            // 
            // labAccountNumber
            // 
            this.labAccountNumber.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAccountNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.labAccountNumber.Location = new System.Drawing.Point(365, 9);
            this.labAccountNumber.Name = "labAccountNumber";
            this.labAccountNumber.Size = new System.Drawing.Size(173, 26);
            this.labAccountNumber.TabIndex = 7;
            // 
            // labAccountBalance
            // 
            this.labAccountBalance.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAccountBalance.ForeColor = System.Drawing.Color.Gray;
            this.labAccountBalance.Location = new System.Drawing.Point(317, 46);
            this.labAccountBalance.Name = "labAccountBalance";
            this.labAccountBalance.Size = new System.Drawing.Size(144, 19);
            this.labAccountBalance.TabIndex = 6;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Gray;
            this.label14.Location = new System.Drawing.Point(173, 46);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(144, 19);
            this.label14.TabIndex = 10;
            this.label14.Text = "Current Balance: ";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label13.Location = new System.Drawing.Point(19, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(336, 26);
            this.label13.TabIndex = 9;
            this.label13.Text = "Account History for  Account Number";
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(547, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(96, 19);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Back To Main";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack2_LinkClicked);
            // 
            // panTransactions
            // 
            this.panTransactions.Location = new System.Drawing.Point(29, 74);
            this.panTransactions.Name = "panTransactions";
            this.panTransactions.Size = new System.Drawing.Size(605, 129);
            this.panTransactions.TabIndex = 0;
            this.panTransactions.Paint += new System.Windows.Forms.PaintEventHandler(this.panTransactions_Paint);
            // 
            // panPayBills
            // 
            this.panPayBills.AutoScroll = true;
            this.panPayBills.BackColor = System.Drawing.Color.White;
            this.panPayBills.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panPayBills.Controls.Add(this.cboPayAccount);
            this.panPayBills.Controls.Add(this.label8);
            this.panPayBills.Controls.Add(this.btnSchedulePayments);
            this.panPayBills.Controls.Add(this.label5);
            this.panPayBills.Controls.Add(this.label4);
            this.panPayBills.Controls.Add(this.linkBack2);
            this.panPayBills.Controls.Add(this.panPayees);
            this.panPayBills.Location = new System.Drawing.Point(518, 369);
            this.panPayBills.Name = "panPayBills";
            this.panPayBills.Size = new System.Drawing.Size(672, 166);
            this.panPayBills.TabIndex = 3;
            this.panPayBills.Visible = false;
            // 
            // cboPayAccount
            // 
            this.cboPayAccount.Location = new System.Drawing.Point(250, 9);
            this.cboPayAccount.Name = "cboPayAccount";
            this.cboPayAccount.Size = new System.Drawing.Size(201, 24);
            this.cboPayAccount.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label8.Location = new System.Drawing.Point(19, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(221, 26);
            this.label8.TabIndex = 13;
            this.label8.Text = "Pay Bills from Account";
            // 
            // btnSchedulePayments
            // 
            this.btnSchedulePayments.BackColor = System.Drawing.SystemColors.Control;
            this.btnSchedulePayments.Location = new System.Drawing.Point(221, 129);
            this.btnSchedulePayments.Name = "btnSchedulePayments";
            this.btnSchedulePayments.Size = new System.Drawing.Size(211, 28);
            this.btnSchedulePayments.TabIndex = 12;
            this.btnSchedulePayments.Text = "Schedule Payments";
            this.btnSchedulePayments.UseVisualStyleBackColor = false;
            this.btnSchedulePayments.Click += new System.EventHandler(this.btnSchedulePayments_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Gray;
            this.label5.Location = new System.Drawing.Point(336, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 19);
            this.label5.TabIndex = 10;
            this.label5.Text = "Amount";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(77, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 19);
            this.label4.TabIndex = 9;
            this.label4.Text = "Payee Name";
            // 
            // linkBack2
            // 
            this.linkBack2.Location = new System.Drawing.Point(547, 9);
            this.linkBack2.Name = "linkBack2";
            this.linkBack2.Size = new System.Drawing.Size(96, 19);
            this.linkBack2.TabIndex = 8;
            this.linkBack2.TabStop = true;
            this.linkBack2.Text = "Back To Main";
            this.linkBack2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack2_LinkClicked);
            // 
            // panPayees
            // 
            this.panPayees.Location = new System.Drawing.Point(67, 92);
            this.panPayees.Name = "panPayees";
            this.panPayees.Size = new System.Drawing.Size(461, 19);
            this.panPayees.TabIndex = 0;
            // 
            // panTransfer
            // 
            this.panTransfer.AutoScroll = true;
            this.panTransfer.BackColor = System.Drawing.Color.White;
            this.panTransfer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panTransfer.Controls.Add(this.labToBal);
            this.panTransfer.Controls.Add(this.labFromBal);
            this.panTransfer.Controls.Add(this.btnTransfer);
            this.panTransfer.Controls.Add(this.updAmount);
            this.panTransfer.Controls.Add(this.label12);
            this.panTransfer.Controls.Add(this.cboTo);
            this.panTransfer.Controls.Add(this.cboFrom);
            this.panTransfer.Controls.Add(this.label10);
            this.panTransfer.Controls.Add(this.label11);
            this.panTransfer.Controls.Add(this.label9);
            this.panTransfer.Controls.Add(this.linkLabel2);
            this.panTransfer.Controls.Add(this.label6);
            this.panTransfer.Controls.Add(this.label7);
            this.panTransfer.Location = new System.Drawing.Point(518, 582);
            this.panTransfer.Name = "panTransfer";
            this.panTransfer.Size = new System.Drawing.Size(672, 230);
            this.panTransfer.TabIndex = 4;
            this.panTransfer.Visible = false;
            // 
            // labToBal
            // 
            this.labToBal.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labToBal.ForeColor = System.Drawing.Color.Gray;
            this.labToBal.Location = new System.Drawing.Point(509, 102);
            this.labToBal.Name = "labToBal";
            this.labToBal.Size = new System.Drawing.Size(125, 18);
            this.labToBal.TabIndex = 18;
            this.labToBal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labFromBal
            // 
            this.labFromBal.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labFromBal.ForeColor = System.Drawing.Color.Gray;
            this.labFromBal.Location = new System.Drawing.Point(509, 65);
            this.labFromBal.Name = "labFromBal";
            this.labFromBal.Size = new System.Drawing.Size(125, 18);
            this.labFromBal.TabIndex = 17;
            this.labFromBal.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.SystemColors.Control;
            this.btnTransfer.Location = new System.Drawing.Point(240, 194);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(182, 28);
            this.btnTransfer.TabIndex = 16;
            this.btnTransfer.Text = "Perform Transfer";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // updAmount
            // 
            this.updAmount.Location = new System.Drawing.Point(259, 148);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new System.Drawing.Size(135, 22);
            this.updAmount.TabIndex = 15;
            this.updAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Gray;
            this.label12.Location = new System.Drawing.Point(154, 148);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 18);
            this.label12.TabIndex = 14;
            this.label12.Text = "Amount: ";
            // 
            // cboTo
            // 
            this.cboTo.Location = new System.Drawing.Point(125, 102);
            this.cboTo.Name = "cboTo";
            this.cboTo.Size = new System.Drawing.Size(201, 24);
            this.cboTo.TabIndex = 13;
            this.cboTo.Text = "comboBox2";
            this.cboTo.SelectedIndexChanged += new System.EventHandler(this.cboTo_SelectedIndexChanged);
            // 
            // cboFrom
            // 
            this.cboFrom.Location = new System.Drawing.Point(125, 65);
            this.cboFrom.Name = "cboFrom";
            this.cboFrom.Size = new System.Drawing.Size(201, 24);
            this.cboFrom.TabIndex = 12;
            this.cboFrom.Text = "comboBox1";
            this.cboFrom.SelectedIndexChanged += new System.EventHandler(this.cboFrom_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(48, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 18);
            this.label10.TabIndex = 11;
            this.label10.Text = "To: ";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Gray;
            this.label11.Location = new System.Drawing.Point(355, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(144, 18);
            this.label11.TabIndex = 10;
            this.label11.Text = "Current Balance: ";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Gray;
            this.label9.Location = new System.Drawing.Point(48, 65);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 18);
            this.label9.TabIndex = 9;
            this.label9.Text = "From: ";
            // 
            // linkLabel2
            // 
            this.linkLabel2.Location = new System.Drawing.Point(547, 9);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(96, 19);
            this.linkLabel2.TabIndex = 8;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Back To Main";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack2_LinkClicked);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(19, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(490, 26);
            this.label6.TabIndex = 7;
            this.label6.Text = "Transfer Funds";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(355, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(144, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "Current Balance: ";
            // 
            // panRecurring
            // 
            this.panRecurring.AutoScroll = true;
            this.panRecurring.BackColor = System.Drawing.Color.White;
            this.panRecurring.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panRecurring.Controls.Add(this.label18);
            this.panRecurring.Controls.Add(this.cboPayAccount2);
            this.panRecurring.Controls.Add(this.label15);
            this.panRecurring.Controls.Add(this.btnDoneRecurring);
            this.panRecurring.Controls.Add(this.label16);
            this.panRecurring.Controls.Add(this.label17);
            this.panRecurring.Controls.Add(this.linkLabel3);
            this.panRecurring.Controls.Add(this.panPayees2);
            this.panRecurring.Location = new System.Drawing.Point(19, 462);
            this.panRecurring.Name = "panRecurring";
            this.panRecurring.Size = new System.Drawing.Size(471, 360);
            this.panRecurring.TabIndex = 5;
            this.panRecurring.Visible = false;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Gray;
            this.label18.Location = new System.Drawing.Point(494, 55);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(106, 37);
            this.label18.TabIndex = 15;
            this.label18.Text = "Day of Month to Pay";
            this.label18.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cboPayAccount2
            // 
            this.cboPayAccount2.Location = new System.Drawing.Point(250, 28);
            this.cboPayAccount2.Name = "cboPayAccount2";
            this.cboPayAccount2.Size = new System.Drawing.Size(201, 24);
            this.cboPayAccount2.TabIndex = 14;
            this.cboPayAccount2.SelectedIndexChanged += new System.EventHandler(this.cboPayAccount2_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Franklin Gothic Medium", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label15.Location = new System.Drawing.Point(19, 9);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(221, 46);
            this.label15.TabIndex = 13;
            this.label15.Text = "Setup Recurring Monthly Payments from";
            // 
            // btnDoneRecurring
            // 
            this.btnDoneRecurring.BackColor = System.Drawing.SystemColors.Control;
            this.btnDoneRecurring.Location = new System.Drawing.Point(221, 157);
            this.btnDoneRecurring.Name = "btnDoneRecurring";
            this.btnDoneRecurring.Size = new System.Drawing.Size(211, 28);
            this.btnDoneRecurring.TabIndex = 12;
            this.btnDoneRecurring.Text = "Done";
            this.btnDoneRecurring.UseVisualStyleBackColor = false;
            this.btnDoneRecurring.Click += new System.EventHandler(this.btnDoneRecurring_Click);
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.Gray;
            this.label16.Location = new System.Drawing.Point(336, 74);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(106, 18);
            this.label16.TabIndex = 10;
            this.label16.Text = "Amount";
            // 
            // label17
            // 
            this.label17.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.Gray;
            this.label17.Location = new System.Drawing.Point(77, 74);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(115, 18);
            this.label17.TabIndex = 9;
            this.label17.Text = "Payee Name";
            // 
            // linkLabel3
            // 
            this.linkLabel3.Location = new System.Drawing.Point(547, 9);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(96, 19);
            this.linkLabel3.TabIndex = 8;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Back To Main";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkBack2_LinkClicked);
            // 
            // panPayees2
            // 
            this.panPayees2.Location = new System.Drawing.Point(67, 120);
            this.panPayees2.Name = "panPayees2";
            this.panPayees2.Size = new System.Drawing.Size(547, 18);
            this.panPayees2.TabIndex = 0;
            // 
            // frmOnlineBanking
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(1000, 742);
            this.Controls.Add(this.panHome);
            this.Controls.Add(this.panRecurring);
            this.Controls.Add(this.panTransfer);
            this.Controls.Add(this.panPayBills);
            this.Controls.Add(this.panAccountDetail);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmOnlineBanking";
            this.ShowInTaskbar = false;
            this.Text = "Online Banking";
            this.panel1.ResumeLayout(false);
            this.panHome.ResumeLayout(false);
            this.panAccountDetail.ResumeLayout(false);
            this.panPayBills.ResumeLayout(false);
            this.panTransfer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.updAmount)).EndInit();
            this.panRecurring.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void linkAccountDetails_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel label = (LinkLabel) sender;
            BankAccount account = (BankAccount) this.bankAccounts[(long) label.Tag];
            this.account = account;
            this.labAccountNumber.Text = account.AccountNumber.ToString();
            this.labAccountBalance.Text = Utilities.FC(account.EndingBalance(), 2, A.Instance.CurrencyConversion);
            this.panHome.Visible = false;
            this.panAccountDetail.Visible = true;
            this.panAccountDetail.Dock = DockStyle.Fill;
        }

        private void linkBack2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.ReturnToHome();
        }

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            this.ReturnToHome();
        }

        private void linkPaybills_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.panHome.Visible = false;
            this.panPayBills.Visible = true;
            this.panPayBills.Dock = DockStyle.Fill;
            this.panPayees.Controls.Clear();
            this.cboPayAccount.Items.Clear();
            int y = 5;
            if (this.panPayees.Controls.Count == 0)
            {
                foreach (BankAccount account in this.payees)
                {
                    Label label = new Label {
                        AutoSize = true,
                        Location = new Point(5, y),
                        Text = account.BankName
                    };
                    if (account is InstallmentLoan)
                    {
                        object[] args = new object[] { account.AccountNumber };
                        label.Text = label.Text + A.Resources.GetString(" Acct# {0}", args);
                    }
                    TextBox box = new TextBox {
                        TextAlign = HorizontalAlignment.Right,
                        Location = new Point(220, y),
                        Tag = account
                    };
                    this.panPayees.Controls.Add(box);
                    this.panPayees.Controls.Add(label);
                    y += 0x23;
                }
                this.panPayees.Height = y;
                this.btnSchedulePayments.Top = this.panPayees.Bottom + 5;
            }
            foreach (BankAccount account2 in this.bankAccounts.Values)
            {
                if (account2 is CheckingAccount)
                {
                    this.cboPayAccount.Items.Add(account2);
                }
            }
            if (this.cboPayAccount.Items.Count > 0)
            {
                this.cboPayAccount.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(A.Resources.GetString("You need a checking account at this bank to pay bills."), A.Resources.GetString("Pay Bills"));
                this.ReturnToHome();
            }
        }

        private void linkRecurring_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.panHome.Visible = false;
            this.panRecurring.Visible = true;
            this.panRecurring.Dock = DockStyle.Fill;
            this.panPayees2.Controls.Clear();
            this.cboPayAccount2.Items.Clear();
            int y = 5;
            foreach (BankAccount account in this.payees) {
                RecurringPaymentControl control = new RecurringPaymentControl {
                    Location = new Point(0, y)
                };
                control.PayeeName.Text = account.BankName;
                if (account is InstallmentLoan) {
                    object[] args = new object[] { account.AccountNumber };
                    control.PayeeName.Text = control.PayeeName.Text + A.Resources.GetString(" Acct# {0}", args);
                    control.Amount.Text = ((InstallmentLoan)account).Payment.ToString();
                }
                else if (account is CreditCardAccount) {
                    DateTime dt = A.State.Now;
                    control.Amount.Text = ((CreditCardAccount)account).
                        MinimumPayment(dt.Year, dt.Month).ToString();
                }
                else { 
                    control.Amount.Text = account.CurrentCharges(
                        A.State.Now).ToString();
                }
                control.PayeeAccountNumber = account.AccountNumber;
                this.panPayees2.Controls.Add(control);
                y += 0x23;
            }
            this.panPayees2.Height = y;
            this.btnDoneRecurring.Top = this.panPayees2.Bottom + 5;
            foreach (BankAccount account2 in this.bankAccounts.Values)
            {
                if (account2 is CheckingAccount)
                {
                    this.cboPayAccount2.Items.Add(account2);
                }
            }
            if (this.cboPayAccount2.Items.Count > 0)
            {
                this.cboPayAccount2.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show(A.Resources.GetString("You need a checking account at this bank to pay bills."), A.Resources.GetString("Pay Bills"));
                this.ReturnToHome();
            }
        }

        private void linkTransfer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this.bankAccounts.Count < 2)
            {
                MessageBox.Show(A.Resources.GetString("You need at least two accounts open to transfer funds between them."), A.Resources.GetString("Cannot Transfer"));
            }
            else
            {
                this.panHome.Visible = false;
                this.panTransfer.Visible = true;
                this.panTransfer.Dock = DockStyle.Fill;
                this.cboFrom.Items.Clear();
                foreach (BankAccount account in this.bankAccounts.Values)
                {
                    this.cboFrom.Items.Add(account);
                }
                this.cboFrom.SelectedIndex = 0;
                this.updAmount.Value = decimal.Zero;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panTransactions_Paint(object sender, PaintEventArgs e)
        {
            int margin = this.margin;
            Graphics graphics = e.Graphics;
            string[] strArray = new string[] { A.Resources.GetString("Date"), A.Resources.GetString("Description"), A.Resources.GetString("Debit"), A.Resources.GetString("Credit"), A.Resources.GetString("Balance") };
            graphics.DrawString(strArray[0], fontS, brush, (float) this.margin, (float) margin);
            graphics.DrawString(strArray[1], fontS, brush, (float) this.colWidth, (float) margin);
            for (int i = 2; i < 5; i++)
            {
                graphics.DrawString(strArray[i], fontS, brush, (float) ((i + 2) * this.colWidth), (float) margin, sfr);
            }
            margin += 20;
            margin += this.margin;
            ArrayList list = (ArrayList) this.account.Transactions.Clone();
            list.Reverse();
            float num2 = this.account.EndingBalance();
            foreach (Transaction transaction in list)
            {
                graphics.DrawString(transaction.Date.ToString("M/d/yyyy"), font, brush, (float) this.margin, (float) margin);
                graphics.DrawString(transaction.Description, font, brush, new RectangleF((float) this.colWidth, (float) margin, (float) ((int) (2.5f * this.colWidth)), 45f));
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    graphics.DrawString(transaction.Amount.ToString("N2"), font, brush, (float) (4 * this.colWidth), (float) margin, sfr);
                }
                else
                {
                    graphics.DrawString(transaction.Amount.ToString("N2"), font, brush, (float) (5 * this.colWidth), (float) margin, sfr);
                }
                graphics.DrawString(num2.ToString("N2"), font, brush, (float) (6 * this.colWidth), (float) margin, sfr);
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num2 += transaction.Amount;
                }
                else
                {
                    num2 -= transaction.Amount;
                }
                margin += 0x2d;
            }
            this.panTransactions.Height = margin + 0x1f;
        }

        protected void ReturnToHome()
        {
            this.panPayBills.Visible = false;
            this.panAccountDetail.Visible = false;
            this.panTransfer.Visible = false;
            this.panRecurring.Visible = false;
            this.panHome.Visible = true;
            this.panHome.Dock = DockStyle.Fill;
        }

        protected string URL
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
                this.labBankName.Text = this.url;
                this.ReturnToHome();
                int num = 0;
                this.panAccounts.Controls.Clear();
                foreach (BankAccount account in this.bankAccounts.Values)
                {
                    if (account.BankURL == this.URL)
                    {
                        LinkLabel label = new LinkLabel {
                            Text = account.AccountTypeFriendlyName + " # " + account.AccountNumber,
                            Tag = account.AccountNumber,
                            Location = new Point(0, num++ * 0x19)
                        };
                        label.Size = new Size(label.Width + 10, label.Height);
                        label.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkAccountDetails_LinkClicked);
                        this.panAccounts.Controls.Add(label);
                    }
                }
            }
        }
    }
}

