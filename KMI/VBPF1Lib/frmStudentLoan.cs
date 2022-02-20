using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmStudentLoan : Form {

        private Button btnCancel, btnHelp, btnOK;
        private ComboBox cboBalanceSource;
        private Container components = null;
        private Course course;

        private Label label1, label10, label12, label14, label3;
        private Label label5, label6, label7, label8;
        private Label labFirstPaymentDue, labPayment;
        private Label labPrice, labRate, labRemainingBalance;
        private Label labTerm, labTotalPayments;

        private InstallmentLoan loan;
        public AppBuildingDrawable.AddOfferingInfo Result;
        private NumericUpDown updLoanAmount;

        public frmStudentLoan(Course course) {
            this.InitializeComponent();
            this.course = course;
            this.loan = A.Adapter.GetStudentLoan(A.MainForm.CurrentEntityID, course);
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (account is CheckingAccount)
                {
                    this.cboBalanceSource.Items.Add(account);
                }
            }
            if (this.cboBalanceSource.Items.Count == 0)
            {
                this.cboBalanceSource.Enabled = false;
                this.updLoanAmount.Enabled = false;
            }
            else
            {
                this.cboBalanceSource.SelectedIndex = 0;
            }
            this.labPrice.Text = Utilities.FC(course.Cost, A.Instance.CurrencyConversion);
            this.updLoanAmount.Maximum = (decimal) course.Cost;
            this.updLoanAmount.Increment = 50M;
            this.labRate.Text = Utilities.FP(this.loan.InterestRate);
            this.labTerm.Text = this.loan.Term.ToString();
            this.labFirstPaymentDue.Text = this.loan.BeginBilling.AddDays(30.0).ToShortDateString();
            this.updLoanAmount.Value = (decimal) course.Cost;
            if (this.loan.InterestRate == 0f)
            {
                object[] args = new object[] { 0 };
                MessageBox.Show(A.Resources.GetString("Your credit score was too low. You were denied a student loan. You must pay for the class in full if you want to take it now. Otherwise, try raising your credit score above {0}.", args), "Credit Denied");
                this.updLoanAmount.Value = decimal.Zero;
                this.updLoanAmount.Maximum = decimal.Zero;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Student Loans"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.updLoanAmount_ValueChanged(new object(), new EventArgs());
                long accountNumber = -1L;
                BankAccount selectedItem = (BankAccount) this.cboBalanceSource.SelectedItem;
                if (selectedItem != null)
                {
                    accountNumber = selectedItem.AccountNumber;
                }
                AppBuildingDrawable.AddOfferingInfo info = A.Adapter.Enroll(A.MainForm.CurrentEntityID, this.course.ID, this.loan, accountNumber);
                MessageBox.Show("Congratulations. You've been enrolled!", "Application Accepted");
                if (info.IsFirstTravel)
                {
                    AppBuildingDrawable.PickTravelMode();
                }
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

        private void InitializeComponent()
        {
            this.updLoanAmount = new NumericUpDown();
            this.label1 = new Label();
            this.labTotalPayments = new Label();
            this.label6 = new Label();
            this.labPayment = new Label();
            this.label8 = new Label();
            this.labTerm = new Label();
            this.label10 = new Label();
            this.labRate = new Label();
            this.label12 = new Label();
            this.labPrice = new Label();
            this.label14 = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.labFirstPaymentDue = new Label();
            this.label3 = new Label();
            this.labRemainingBalance = new Label();
            this.label5 = new Label();
            this.label7 = new Label();
            this.cboBalanceSource = new ComboBox();
            this.updLoanAmount.BeginInit();
            base.SuspendLayout();
            this.updLoanAmount.Location = new Point(0x100, 0x34);
            this.updLoanAmount.Name = "updLoanAmount";
            this.updLoanAmount.Size = new Size(0x54, 20);
            this.updLoanAmount.TabIndex = 2;
            this.updLoanAmount.TextAlign = HorizontalAlignment.Right;
            this.updLoanAmount.ThousandsSeparator = true;
            this.updLoanAmount.ValueChanged += new EventHandler(this.updLoanAmount_ValueChanged);
            this.label1.Location = new Point(0x24, 0x38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xa8, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Amount of Loan Requested:";
            this.labTotalPayments.Location = new Point(200, 0xac);
            this.labTotalPayments.Name = "labTotalPayments";
            this.labTotalPayments.Size = new Size(140, 20);
            this.labTotalPayments.TabIndex = 9;
            this.labTotalPayments.Text = "label5";
            this.labTotalPayments.TextAlign = ContentAlignment.TopRight;
            this.label6.Location = new Point(0x24, 0xac);
            this.label6.Name = "label6";
            this.label6.Size = new Size(140, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Total Payments:";
            this.labPayment.Location = new Point(200, 0x98);
            this.labPayment.Name = "labPayment";
            this.labPayment.Size = new Size(140, 20);
            this.labPayment.TabIndex = 11;
            this.labPayment.Text = "label7";
            this.labPayment.TextAlign = ContentAlignment.TopRight;
            this.label8.Location = new Point(0x24, 0x98);
            this.label8.Name = "label8";
            this.label8.Size = new Size(140, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Monthly Payment:";
            this.labTerm.Location = new Point(200, 0x84);
            this.labTerm.Name = "labTerm";
            this.labTerm.Size = new Size(140, 20);
            this.labTerm.TabIndex = 13;
            this.labTerm.Text = "label9";
            this.labTerm.TextAlign = ContentAlignment.TopRight;
            this.label10.Location = new Point(0x24, 0x84);
            this.label10.Name = "label10";
            this.label10.Size = new Size(140, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Term (Months):";
            this.labRate.Location = new Point(200, 0x5c);
            this.labRate.Name = "labRate";
            this.labRate.Size = new Size(140, 20);
            this.labRate.TabIndex = 15;
            this.labRate.Text = "label11";
            this.labRate.TextAlign = ContentAlignment.TopRight;
            this.label12.Location = new Point(0x24, 0x5c);
            this.label12.Name = "label12";
            this.label12.Size = new Size(140, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "Interest Rate:";
            this.labPrice.Location = new Point(200, 0x1c);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(140, 20);
            this.labPrice.TabIndex = 0x11;
            this.labPrice.Text = "label13";
            this.labPrice.TextAlign = ContentAlignment.TopRight;
            this.label14.Location = new Point(0x24, 0x1c);
            this.label14.Name = "label14";
            this.label14.Size = new Size(140, 20);
            this.label14.TabIndex = 0x10;
            this.label14.Text = "Cost of Course:";
            this.btnOK.Location = new Point(0x10, 0x11c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Location = new Point(0x88, 0x11c);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 0x15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x100, 0x11c);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 0x16;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.labFirstPaymentDue.Location = new Point(200, 0x70);
            this.labFirstPaymentDue.Name = "labFirstPaymentDue";
            this.labFirstPaymentDue.Size = new Size(140, 20);
            this.labFirstPaymentDue.TabIndex = 0x18;
            this.labFirstPaymentDue.Text = "label11";
            this.labFirstPaymentDue.TextAlign = ContentAlignment.TopRight;
            this.label3.Location = new Point(0x24, 0x70);
            this.label3.Name = "label3";
            this.label3.Size = new Size(140, 20);
            this.label3.TabIndex = 0x17;
            this.label3.Text = "First Payment Due:";
            this.labRemainingBalance.Location = new Point(200, 0xd4);
            this.labRemainingBalance.Name = "labRemainingBalance";
            this.labRemainingBalance.Size = new Size(140, 20);
            this.labRemainingBalance.TabIndex = 0x1a;
            this.labRemainingBalance.Text = "label5";
            this.labRemainingBalance.TextAlign = ContentAlignment.TopRight;
            this.label5.Location = new Point(0x24, 0xd4);
            this.label5.Name = "label5";
            this.label5.Size = new Size(140, 20);
            this.label5.TabIndex = 0x19;
            this.label5.Text = "Remaining Balance:";
            this.label7.Location = new Point(0x24, 0xec);
            this.label7.Name = "label7";
            this.label7.Size = new Size(140, 20);
            this.label7.TabIndex = 0x1c;
            this.label7.Text = "To Be Paid from:";
            this.cboBalanceSource.Location = new Point(200, 0xe8);
            this.cboBalanceSource.Name = "cboBalanceSource";
            this.cboBalanceSource.Size = new Size(140, 0x15);
            this.cboBalanceSource.TabIndex = 0x1b;
            this.cboBalanceSource.Text = "None";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x174, 330);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.cboBalanceSource);
            base.Controls.Add(this.labRemainingBalance);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.labFirstPaymentDue);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.labPrice);
            base.Controls.Add(this.label14);
            base.Controls.Add(this.labRate);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.labTerm);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.labPayment);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.labTotalPayments);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.updLoanAmount);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmStudentLoan";
            base.ShowInTaskbar = false;
            this.Text = "Request Student Loan";
            this.updLoanAmount.EndInit();
            base.ResumeLayout(false);
        }

        private void updLoanAmount_ValueChanged(object sender, EventArgs e)
        {
            this.loan.OriginalBalance = (float) this.updLoanAmount.Value;
            this.loan.ReComputePayment();
            this.labPayment.Text = Utilities.FC(this.loan.Payment, A.Instance.CurrencyConversion);
            this.labTotalPayments.Text = Utilities.FC(this.loan.Payment * this.loan.Term, A.Instance.CurrencyConversion);
            this.labRemainingBalance.Text = Utilities.FC(this.course.Cost - this.loan.OriginalBalance, A.Instance.CurrencyConversion);
        }
    }
}

