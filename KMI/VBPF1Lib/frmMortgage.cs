namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMortgage : Form
    {
        private float baseInterestRate;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private float cashAtClosing;
        private ComboBox cboDownPaymentSource;
        private ComboBox cboPoints;
        private ComboBox cboType;
        private Container components = null;
        private Label labCashAtClosing;
        private Label labClosingCosts;
        private Label label1;
        private Label label10;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label16;
        private Label label18;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private Label label9;
        private Label labFinanced;
        private Label labPandI;
        private Label labPayment;
        private Label labPMI;
        private Label labPrice;
        private Label labRate;
        private Label labTerm;
        private Label labTotalPayments;
        private bool loading;
        private Mortgage loan;
        private Offering offering;
        private float salePrice;
        private NumericUpDown updDownPayment;

        public frmMortgage(Offering offering)
        {
            this.InitializeComponent();
            this.loading = true;
            this.offering = offering;
            Mortgage[] mortgageArray = A.Adapter.GetMortgage(A.MainForm.CurrentEntityID, offering);
            foreach (Mortgage mortgage in mortgageArray)
            {
                this.cboType.Items.Add(mortgage);
            }
            this.loan = mortgageArray[0];
            this.salePrice = this.loan.OriginalBalance;
            this.baseInterestRate = this.loan.InterestRate;
            SortedList bankAccounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            this.cboType.SelectedIndex = 0;
            this.cboPoints.SelectedIndex = 0;
            this.labPrice.Text = Utilities.FC(this.salePrice, A.Instance.CurrencyConversion);
            this.updDownPayment.Maximum = (decimal) this.salePrice;
            this.updDownPayment.Value = (decimal) (0.1f * this.salePrice);
            this.updDownPayment.Minimum = (decimal) (0f * this.salePrice);
            if (this.loan.InterestRate == 0f)
            {
                object[] args = new object[] { 610 };
                MessageBox.Show(A.Resources.GetString("Your credit score was too low. You were denied a loan. You can only purchase now by paying the full amount as your down payment. Otherwise, try raising your credit score above {0}.", args), "Credit Denied");
                this.updDownPayment.Value = (decimal) this.salePrice;
                this.updDownPayment.Minimum = (decimal) this.salePrice;
            }
            this.updDownPayment.Increment = 1000M;
            this.labTerm.Text = (this.loan.Term / 12).ToString();
            foreach (BankAccount account in bankAccounts.Values)
            {
                if (account is CheckingAccount)
                {
                    this.cboDownPaymentSource.Items.Add(account);
                }
            }
            this.loading = false;
            this.UpdateAll(new object(), new EventArgs());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Condos For Sale"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.UpdateAll(new object(), new EventArgs());
            A.Adapter.SetMortgage(A.MainForm.CurrentEntityID, this.loan, this.offering.ID, ((BankAccount) this.cboDownPaymentSource.SelectedItem).AccountNumber, this.cashAtClosing);
            base.Close();
            base.DialogResult = DialogResult.OK;
            new frmHomeOwnersInsurance(this.offering) { Cancellable = false }.ShowDialog(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMortgage_Load(object sender, EventArgs e)
        {
            if (this.cboDownPaymentSource.Items.Count == 0)
            {
                MessageBox.Show("You will need a checking account as the source for cash due at closing. Please open a checking account first.", "Mortgage");
                base.Close();
            }
            else
            {
                this.cboDownPaymentSource.SelectedIndex = 0;
            }
        }

        private void InitializeComponent()
        {
            this.updDownPayment = new NumericUpDown();
            this.cboDownPaymentSource = new ComboBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.labFinanced = new Label();
            this.labTotalPayments = new Label();
            this.label6 = new Label();
            this.labPMI = new Label();
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
            this.label4 = new Label();
            this.cboType = new ComboBox();
            this.label5 = new Label();
            this.labPayment = new Label();
            this.label9 = new Label();
            this.labPandI = new Label();
            this.label13 = new Label();
            this.cboPoints = new ComboBox();
            this.labClosingCosts = new Label();
            this.label16 = new Label();
            this.labCashAtClosing = new Label();
            this.label18 = new Label();
            this.updDownPayment.BeginInit();
            base.SuspendLayout();
            this.updDownPayment.Location = new Point(0x100, 0x30);
            this.updDownPayment.Name = "updDownPayment";
            this.updDownPayment.Size = new Size(0x54, 20);
            this.updDownPayment.TabIndex = 2;
            this.updDownPayment.TextAlign = HorizontalAlignment.Right;
            this.updDownPayment.ThousandsSeparator = true;
            this.updDownPayment.ValueChanged += new EventHandler(this.UpdateAll);
            this.cboDownPaymentSource.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDownPaymentSource.Location = new Point(200, 0x4c);
            this.cboDownPaymentSource.Name = "cboDownPaymentSource";
            this.cboDownPaymentSource.Size = new Size(140, 0x15);
            this.cboDownPaymentSource.TabIndex = 3;
            this.label1.Location = new Point(0x24, 0x34);
            this.label1.Name = "label1";
            this.label1.Size = new Size(140, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Down Payment Amount:";
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x24, 0xac);
            this.label2.Name = "label2";
            this.label2.Size = new Size(140, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Amount of Mortgage:";
            this.label3.Location = new Point(0x24, 0x4c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(140, 0x1c);
            this.label3.TabIndex = 6;
            this.label3.Text = "Source of Cash Required at Closing:";
            this.labFinanced.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labFinanced.Location = new Point(200, 0xac);
            this.labFinanced.Name = "labFinanced";
            this.labFinanced.Size = new Size(140, 20);
            this.labFinanced.TabIndex = 7;
            this.labFinanced.Text = "label4";
            this.labFinanced.TextAlign = ContentAlignment.TopRight;
            this.labTotalPayments.Location = new Point(0xf4, 0x124);
            this.labTotalPayments.Name = "labTotalPayments";
            this.labTotalPayments.Size = new Size(0x60, 20);
            this.labTotalPayments.TabIndex = 9;
            this.labTotalPayments.Text = "label5";
            this.labTotalPayments.TextAlign = ContentAlignment.TopRight;
            this.label6.Location = new Point(0x24, 0x124);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0xcc, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Total Payments over Life of Mortgage:";
            this.labPMI.Location = new Point(200, 0xfc);
            this.labPMI.Name = "labPMI";
            this.labPMI.Size = new Size(140, 20);
            this.labPMI.TabIndex = 11;
            this.labPMI.Text = "label7";
            this.labPMI.TextAlign = ContentAlignment.TopRight;
            this.label8.Location = new Point(0x24, 0xfc);
            this.label8.Name = "label8";
            this.label8.Size = new Size(140, 20);
            this.label8.TabIndex = 10;
            this.label8.Text = "Monthly PMI Payment:";
            this.labTerm.Location = new Point(200, 0xd4);
            this.labTerm.Name = "labTerm";
            this.labTerm.Size = new Size(140, 20);
            this.labTerm.TabIndex = 13;
            this.labTerm.Text = "label9";
            this.labTerm.TextAlign = ContentAlignment.TopRight;
            this.label10.Location = new Point(0x24, 0xd4);
            this.label10.Name = "label10";
            this.label10.Size = new Size(140, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Term (Years):";
            this.labRate.Location = new Point(200, 0xc0);
            this.labRate.Name = "labRate";
            this.labRate.Size = new Size(140, 20);
            this.labRate.TabIndex = 15;
            this.labRate.Text = "label11";
            this.labRate.TextAlign = ContentAlignment.TopRight;
            this.label12.Location = new Point(0x24, 0xc0);
            this.label12.Name = "label12";
            this.label12.Size = new Size(140, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "Interest Rate:";
            this.labPrice.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPrice.Location = new Point(200, 0x18);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(140, 20);
            this.labPrice.TabIndex = 0x11;
            this.labPrice.Text = "label13";
            this.labPrice.TextAlign = ContentAlignment.TopRight;
            this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label14.Location = new Point(0x24, 0x18);
            this.label14.Name = "label14";
            this.label14.Size = new Size(140, 20);
            this.label14.TabIndex = 0x10;
            this.label14.Text = "Sale Price:";
            this.btnOK.Location = new Point(0x18, 0x174);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x90, 0x174);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 0x15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x108, 0x174);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 0x16;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label4.Location = new Point(0x24, 0x88);
            this.label4.Name = "label4";
            this.label4.Size = new Size(140, 20);
            this.label4.TabIndex = 0x19;
            this.label4.Text = "Type of Mortgage:";
            this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboType.Location = new Point(200, 0x84);
            this.cboType.Name = "cboType";
            this.cboType.Size = new Size(140, 0x15);
            this.cboType.TabIndex = 0x18;
            this.cboType.SelectedIndexChanged += new EventHandler(this.UpdateAll);
            this.label5.Location = new Point(0x24, 0x6c);
            this.label5.Name = "label5";
            this.label5.Size = new Size(140, 20);
            this.label5.TabIndex = 0x1b;
            this.label5.Text = "Points:";
            this.labPayment.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPayment.Location = new Point(200, 0x110);
            this.labPayment.Name = "labPayment";
            this.labPayment.Size = new Size(140, 20);
            this.labPayment.TabIndex = 0x1d;
            this.labPayment.Text = "label5";
            this.labPayment.TextAlign = ContentAlignment.TopRight;
            this.label9.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label9.Location = new Point(0x24, 0x110);
            this.label9.Name = "label9";
            this.label9.Size = new Size(140, 20);
            this.label9.TabIndex = 0x1c;
            this.label9.Text = "Total Monthly Payment:";
            this.labPandI.Location = new Point(0xec, 0xe8);
            this.labPandI.Name = "labPandI";
            this.labPandI.Size = new Size(0x68, 20);
            this.labPandI.TabIndex = 0x1f;
            this.labPandI.Text = "label7";
            this.labPandI.TextAlign = ContentAlignment.TopRight;
            this.label13.Location = new Point(0x24, 0xe8);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0xd0, 20);
            this.label13.TabIndex = 30;
            this.label13.Text = "Monthly Principle && Interest Payment:";
            this.cboPoints.DropDownStyle = ComboBoxStyle.DropDownList;
            object[] items = new object[] { "0", "0.5", "1", "1.5", "2" };
            this.cboPoints.Items.AddRange(items);
            this.cboPoints.Location = new Point(0x100, 0x68);
            this.cboPoints.Name = "cboPoints";
            this.cboPoints.Size = new Size(0x54, 0x15);
            this.cboPoints.TabIndex = 0x20;
            this.cboPoints.SelectedIndexChanged += new EventHandler(this.UpdateAll);
            this.labClosingCosts.Location = new Point(0xf4, 0x138);
            this.labClosingCosts.Name = "labClosingCosts";
            this.labClosingCosts.Size = new Size(0x60, 20);
            this.labClosingCosts.TabIndex = 0x22;
            this.labClosingCosts.Text = "label5";
            this.labClosingCosts.TextAlign = ContentAlignment.TopRight;
            this.label16.Location = new Point(0x24, 0x138);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0xcc, 20);
            this.label16.TabIndex = 0x21;
            this.label16.Text = "Closing Costs:";
            this.labCashAtClosing.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labCashAtClosing.Location = new Point(0xf4, 0x14c);
            this.labCashAtClosing.Name = "labCashAtClosing";
            this.labCashAtClosing.Size = new Size(0x60, 20);
            this.labCashAtClosing.TabIndex = 0x24;
            this.labCashAtClosing.Text = "label5";
            this.labCashAtClosing.TextAlign = ContentAlignment.TopRight;
            this.label18.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label18.Location = new Point(0x24, 0x14c);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0xcc, 20);
            this.label18.TabIndex = 0x23;
            this.label18.Text = "Cash Required at Closing:";
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x188, 410);
            base.Controls.Add(this.labCashAtClosing);
            base.Controls.Add(this.label18);
            base.Controls.Add(this.labClosingCosts);
            base.Controls.Add(this.label16);
            base.Controls.Add(this.cboPoints);
            base.Controls.Add(this.labPandI);
            base.Controls.Add(this.label13);
            base.Controls.Add(this.labPayment);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cboType);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.labPrice);
            base.Controls.Add(this.label14);
            base.Controls.Add(this.labRate);
            base.Controls.Add(this.label12);
            base.Controls.Add(this.labTerm);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.labPMI);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.labTotalPayments);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.labFinanced);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.cboDownPaymentSource);
            base.Controls.Add(this.updDownPayment);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmMortgage";
            base.ShowInTaskbar = false;
            this.Text = "Pay for Condo";
            base.Load += new EventHandler(this.frmMortgage_Load);
            this.updDownPayment.EndInit();
            base.ResumeLayout(false);
        }

        private void UpdateAll(object sender, EventArgs e)
        {
            if (!this.loading)
            {
                this.loan = (Mortgage) this.cboType.SelectedItem;
                this.loan.OriginalBalance = this.salePrice - ((float) this.updDownPayment.Value);
                float num = float.Parse(this.cboPoints.SelectedItem.ToString());
                this.loan.InterestRate = this.baseInterestRate - (num / 400f);
                this.loan.ReComputePayment();
                this.labRate.Text = Utilities.FP(this.loan.InterestRate, 2);
                this.labFinanced.Text = Utilities.FC(this.loan.OriginalBalance, A.Instance.CurrencyConversion);
                this.labPandI.Text = Utilities.FC(this.loan.Payment, A.Instance.CurrencyConversion);
                this.labPayment.Text = Utilities.FC(this.loan.Payment + this.loan.PMI(this.salePrice), A.Instance.CurrencyConversion);
                this.labPMI.Text = Utilities.FC(this.loan.PMI(this.salePrice), A.Instance.CurrencyConversion);
                this.labTotalPayments.Text = Utilities.FC((this.loan.Payment * this.loan.Term) + ((float) this.updDownPayment.Value), A.Instance.CurrencyConversion);
                this.labClosingCosts.Text = Utilities.FC(4100f, A.Instance.CurrencyConversion);
                this.cashAtClosing = (4100f + ((float) this.updDownPayment.Value)) + ((num / 100f) * this.loan.OriginalBalance);
                this.labCashAtClosing.Text = Utilities.FC(this.cashAtClosing, A.Instance.CurrencyConversion);
            }
        }
    }
}

