namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPayForCar : Form
    {
        private Bill bill;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboDownPaymentSource;
        private Container components = null;
        private ArrayList itemNames;
        private Label label1;
        private Label label10;
        private Label label12;
        private Label label14;
        private Label label16;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label labFinanced;
        private Label labLeasePayment;
        private Label labNoBuy;
        private Label labPayment;
        private Label labPrice;
        private Label labRate;
        private Label labTerm;
        private Label labTotalPayments;
        private float leaseCost = 0f;
        private InstallmentLoan loan;
        private RadioButton radBuy;
        private RadioButton radLease;
        private NumericUpDown updDownPayment;

        public frmPayForCar(Bill bill, ArrayList itemNames)
        {
            this.InitializeComponent();
            this.bill = bill;
            this.itemNames = itemNames;
            this.loan = A.Adapter.GetPayForCar(A.MainForm.CurrentEntityID, bill.Amount);
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            this.labPrice.Text = Utilities.FC(bill.Amount, A.Instance.CurrencyConversion);
            this.updDownPayment.Maximum = (decimal) bill.Amount;
            this.updDownPayment.Value = (decimal) (0.1f * bill.Amount);
            this.updDownPayment.Minimum = this.updDownPayment.Value;
            if (this.loan.InterestRate == 0f)
            {
                object[] args = new object[] { 510 };
                MessageBox.Show(A.Resources.GetString("Your credit score was too low. You cannot get a loan. You can only purchase now by paying the full amount as your down payment. Otherwise, try raising your credit score above {0}.", args), "Credit Denied");
                this.updDownPayment.Value = (decimal) bill.Amount;
                this.updDownPayment.Minimum = (decimal) bill.Amount;
            }
            this.updDownPayment.Increment = 500M;
            this.labRate.Text = Utilities.FP(this.loan.InterestRate);
            this.labTerm.Text = this.loan.Term.ToString();
            this.leaseCost = this.loan.Payment * 0.8f;
            if (this.loan.InterestRate == 0f)
            {
                InstallmentLoan loan = new InstallmentLoan(DateTime.MinValue, bill.Amount, 0.07f, 0x24);
                this.leaseCost = loan.Payment * 0.8f;
            }
            this.labLeasePayment.Text = Utilities.FC(this.leaseCost, A.Instance.CurrencyConversion);
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (account is CheckingAccount)
                {
                    this.cboDownPaymentSource.Items.Add(account);
                }
            }
            if (this.cboDownPaymentSource.Items.Count == 0)
            {
                this.labNoBuy.Visible = true;
                this.labNoBuy.BringToFront();
                this.radBuy.Enabled = false;
                this.radLease.Checked = true;
            }
            else
            {
                this.cboDownPaymentSource.SelectedIndex = 0;
                this.radBuy.Checked = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Shop For Car"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string carName = (string) this.itemNames[0];
                if (this.radLease.Checked)
                {
                    A.Adapter.LeaseCar(A.MainForm.CurrentEntityID, this.leaseCost, carName);
                }
                else
                {
                    A.Adapter.PurchaseCar(A.MainForm.CurrentEntityID, this.loan, carName, ((BankAccount) this.cboDownPaymentSource.SelectedItem).AccountNumber, (float) this.updDownPayment.Value);
                }
                base.Close();
                new frmAutoInsurance { Cancellable = false }.ShowDialog(this);
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
            this.radBuy = new RadioButton();
            this.radLease = new RadioButton();
            this.updDownPayment = new NumericUpDown();
            this.cboDownPaymentSource = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.labFinanced = new Label();
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
            this.labLeasePayment = new Label();
            this.label16 = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.labNoBuy = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label7 = new Label();
            this.label9 = new Label();
            this.updDownPayment.BeginInit();
            base.SuspendLayout();
            this.radBuy.Location = new Point(0x20, 0x18);
            this.radBuy.Name = "radBuy";
            this.radBuy.Size = new Size(0xc4, 0x18);
            this.radBuy.TabIndex = 0;
            this.radBuy.Text = "Buy Car";
            this.radBuy.CheckedChanged += new EventHandler(this.radBuy_CheckedChanged);
            this.radLease.Location = new Point(0x20, 0x100);
            this.radLease.Name = "radLease";
            this.radLease.Size = new Size(0xa8, 0x18);
            this.radLease.TabIndex = 1;
            this.radLease.Text = "Lease Car";
            this.radLease.CheckedChanged += new EventHandler(this.radBuy_CheckedChanged);
            this.updDownPayment.Enabled = false;
            this.updDownPayment.Location = new Point(0x120, 0x4c);
            this.updDownPayment.Name = "updDownPayment";
            this.updDownPayment.Size = new Size(0x54, 20);
            this.updDownPayment.TabIndex = 2;
            this.updDownPayment.TextAlign = HorizontalAlignment.Right;
            this.updDownPayment.ThousandsSeparator = true;
            this.updDownPayment.ValueChanged += new EventHandler(this.updDownPayment_ValueChanged);
            this.cboDownPaymentSource.Enabled = false;
            this.cboDownPaymentSource.Location = new Point(0xe8, 0x68);
            this.cboDownPaymentSource.Name = "cboDownPaymentSource";
            this.cboDownPaymentSource.Size = new Size(140, 0x15);
            this.cboDownPaymentSource.TabIndex = 3;
            this.cboDownPaymentSource.Text = "comboBox1";
            this.label1.Location = new Point(0x44, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(140, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Down Payment Amount:";
            this.label2.Location = new Point(0x44, 0x88);
            this.label2.Name = "label2";
            this.label2.Size = new Size(140, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Amount Financed:";
            this.label3.Location = new Point(0x44, 0x6c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(140, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Down Payment Source:";
            this.labFinanced.Location = new Point(0xe8, 0x88);
            this.labFinanced.Name = "labFinanced";
            this.labFinanced.Size = new Size(140, 20);
            this.labFinanced.TabIndex = 7;
            this.labFinanced.Text = "label4";
            this.labFinanced.TextAlign = ContentAlignment.TopRight;
            this.labTotalPayments.Location = new Point(0xe8, 0xd8);
            this.labTotalPayments.Name = "labTotalPayments";
            this.labTotalPayments.Size = new Size(140, 20);
            this.labTotalPayments.TabIndex = 9;
            this.labTotalPayments.Text = "label5";
            this.labTotalPayments.TextAlign = ContentAlignment.TopRight;
            this.label6.Location = new Point(0x44, 0xd8);
            this.label6.Name = "label6";
            this.label6.Size = new Size(140, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Total Payments:";
            this.labPayment.Location = new Point(0xe8, 0xc4);
            this.labPayment.Name = "labPayment";
            this.labPayment.Size = new Size(140, 20);
            this.labPayment.TabIndex = 11;
            this.labPayment.Text = "label7";
            this.labPayment.TextAlign = ContentAlignment.TopRight;
            this.label8.Location = new Point(0x44, 0xc4);
            this.label8.Name = "label8";
            this.label8.Size = new Size(140, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Monthly Payment:";
            this.labTerm.Location = new Point(0xe8, 0xb0);
            this.labTerm.Name = "labTerm";
            this.labTerm.Size = new Size(140, 20);
            this.labTerm.TabIndex = 13;
            this.labTerm.Text = "label9";
            this.labTerm.TextAlign = ContentAlignment.TopRight;
            this.label10.Location = new Point(0x44, 0xb0);
            this.label10.Name = "label10";
            this.label10.Size = new Size(140, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Term (Months):";
            this.labRate.Location = new Point(0xe8, 0x9c);
            this.labRate.Name = "labRate";
            this.labRate.Size = new Size(140, 20);
            this.labRate.TabIndex = 15;
            this.labRate.Text = "label11";
            this.labRate.TextAlign = ContentAlignment.TopRight;
            this.label12.Location = new Point(0x44, 0x9c);
            this.label12.Name = "label12";
            this.label12.Size = new Size(140, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "Interest Rate:";
            this.labPrice.Location = new Point(0xe8, 0x34);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(140, 20);
            this.labPrice.TabIndex = 0x11;
            this.labPrice.Text = "label13";
            this.labPrice.TextAlign = ContentAlignment.TopRight;
            this.label14.Location = new Point(0x44, 0x34);
            this.label14.Name = "label14";
            this.label14.Size = new Size(140, 20);
            this.label14.TabIndex = 0x10;
            this.label14.Text = "Sale Price:";
            this.labLeasePayment.Location = new Point(0xe8, 0x120);
            this.labLeasePayment.Name = "labLeasePayment";
            this.labLeasePayment.Size = new Size(140, 20);
            this.labLeasePayment.TabIndex = 0x13;
            this.labLeasePayment.Text = "label15";
            this.labLeasePayment.TextAlign = ContentAlignment.TopRight;
            this.label16.Location = new Point(0x44, 0x120);
            this.label16.Name = "label16";
            this.label16.Size = new Size(140, 20);
            this.label16.TabIndex = 0x12;
            this.label16.Text = "Monthly Lease Payment:";
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(40, 0x16c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Location = new Point(160, 0x16c);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 0x15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(280, 0x16c);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 0x16;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.labNoBuy.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labNoBuy.ForeColor = SystemColors.ControlDark;
            this.labNoBuy.Location = new Point(0x38, 0x30);
            this.labNoBuy.Name = "labNoBuy";
            this.labNoBuy.Size = new Size(0x14c, 0xc0);
            this.labNoBuy.TabIndex = 0x17;
            this.labNoBuy.Text = "Option unavailable. No checking account for down payment bank check.";
            this.labNoBuy.TextAlign = ContentAlignment.MiddleCenter;
            this.labNoBuy.Visible = false;
            this.label4.Location = new Point(0x44, 0x134);
            this.label4.Name = "label4";
            this.label4.Size = new Size(140, 20);
            this.label4.TabIndex = 0x18;
            this.label4.Text = "Length of  Lease";
            this.label5.Location = new Point(0x44, 0x148);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xac, 20);
            this.label5.TabIndex = 0x19;
            this.label5.Text = "Penalty for Early Termination";
            this.label7.Location = new Point(0xe8, 0x134);
            this.label7.Name = "label7";
            this.label7.Size = new Size(140, 20);
            this.label7.TabIndex = 0x1a;
            this.label7.Text = "2 Years";
            this.label7.TextAlign = ContentAlignment.TopRight;
            this.label9.Location = new Point(0xe8, 0x148);
            this.label9.Name = "label9";
            this.label9.Size = new Size(140, 20);
            this.label9.TabIndex = 0x1b;
            this.label9.Text = "3 Month's Payments";
            this.label9.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(420, 0x192);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.labLeasePayment);
            base.Controls.Add(this.label16);
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
            base.Controls.Add(this.labFinanced);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboDownPaymentSource);
            base.Controls.Add(this.radBuy);
            base.Controls.Add(this.radLease);
            base.Controls.Add(this.updDownPayment);
            base.Controls.Add(this.labNoBuy);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmPayForCar";
            base.ShowInTaskbar = false;
            this.Text = "Pay For Car";
            this.updDownPayment.EndInit();
            base.ResumeLayout(false);
        }

        private void radBuy_CheckedChanged(object sender, EventArgs e)
        {
            this.updDownPayment.Enabled = this.radBuy.Checked;
            this.cboDownPaymentSource.Enabled = this.radBuy.Checked;
            this.btnOK.Enabled = true;
        }

        private void updDownPayment_ValueChanged(object sender, EventArgs e)
        {
            this.loan.OriginalBalance = this.bill.Amount - ((float) this.updDownPayment.Value);
            this.loan.ReComputePayment();
            this.labFinanced.Text = Utilities.FC(this.bill.Amount - ((float) this.updDownPayment.Value), A.Instance.CurrencyConversion);
            this.labPayment.Text = Utilities.FC(this.loan.Payment, A.Instance.CurrencyConversion);
            this.labTotalPayments.Text = Utilities.FC((this.loan.Payment * this.loan.Term) + ((float) this.updDownPayment.Value), A.Instance.CurrencyConversion);
        }
    }
}

