namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmHomeOwnersInsurance : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboDeductible;
        private Container components = null;
        private Input input;
        private Label label2;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label labInsuranceRequired;
        private Label labPremium;
        private Label labValue;
        private Offering offering;
        private NumericUpDown updAmount;

        public frmHomeOwnersInsurance(Offering offering)
        {
            this.InitializeComponent();
            this.input = A.Adapter.GetHomeOwnersInsurance(offering);
            this.offering = offering;
            this.cboDeductible.Items.Add(250f);
            this.cboDeductible.Items.Add(500f);
            this.cboDeductible.Items.Add(1000f);
            this.cboDeductible.Items.Add(2000f);
            this.labValue.Text = Utilities.FC(this.input.Value, A.Instance.CurrencyConversion);
            this.updAmount.Minimum = ((decimal) this.input.Value) / 2M;
            this.updAmount.Value = Math.Max(this.updAmount.Minimum, (decimal) this.input.Policy.Limit);
            this.cboDeductible.SelectedIndex = this.cboDeductible.FindStringExact(this.input.Policy.Deductible.ToString());
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Homeowners Insurance"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                A.Adapter.SetHomeOwnersInsurance(this.offering, this.input.Policy);
                this.btnCancel.Enabled = true;
                base.Close();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void cboDeductible_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboDeductible.SelectedIndex != -1)
            {
                if (this.cboDeductible.SelectedIndex == 4)
                {
                    this.input.Policy.Deductible = -1f;
                }
                else
                {
                    this.input.Policy.Deductible = (float) this.cboDeductible.SelectedItem;
                }
                if (this.updAmount.Value < ((decimal) this.input.Policy.Deductible))
                {
                    this.updAmount.Value = (decimal) this.input.Policy.Deductible;
                }
                this.input.Policy.Limit = (float) this.updAmount.Value;
                this.input.Policy.MonthlyPremium = 0f;
                if (this.input.Policy.Deductible > 0f)
                {
                    this.input.Policy.MonthlyPremium = (((float) this.updAmount.Value) / 50000f) * (10f + (2000f / this.input.Policy.Deductible));
                }
                this.labPremium.Text = Utilities.FC(this.input.Policy.MonthlyPremium * 12f, A.Instance.CurrencyConversion);
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

        private void frmHomeOwnersInsurance_Closing(object sender, CancelEventArgs e)
        {
            if ((sender is Control) && !this.btnCancel.Enabled)
            {
                e.Cancel = true;
            }
        }

        private void InitializeComponent()
        {
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.cboDeductible = new ComboBox();
            this.label4 = new Label();
            this.labPremium = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.updAmount = new NumericUpDown();
            this.label2 = new Label();
            this.labValue = new Label();
            this.labInsuranceRequired = new Label();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.btnHelp.Location = new Point(0xd8, 0xe8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 0x10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(120, 0xe8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Location = new Point(0x18, 0xe8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.cboDeductible.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDeductible.Location = new Point(0x90, 0x88);
            this.cboDeductible.Name = "cboDeductible";
            this.cboDeductible.Size = new Size(0x58, 0x15);
            this.cboDeductible.TabIndex = 0x11;
            this.cboDeductible.SelectedIndexChanged += new EventHandler(this.cboDeductible_SelectedIndexChanged);
            this.label4.Location = new Point(0x30, 0xb8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x58, 0x18);
            this.label4.TabIndex = 0x15;
            this.label4.Text = "Yearly Premium:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.labPremium.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPremium.Location = new Point(0x98, 0xb8);
            this.labPremium.Name = "labPremium";
            this.labPremium.Size = new Size(0x70, 0x10);
            this.labPremium.TabIndex = 0x16;
            this.labPremium.Text = "$0";
            this.label6.Location = new Point(40, 0x88);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x58, 0x18);
            this.label6.TabIndex = 0x17;
            this.label6.Text = "Deductible:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.label7.Location = new Point(0x20, 0x58);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x20);
            this.label7.TabIndex = 0x19;
            this.label7.Text = "Amount of Coverage:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            int[] bits = new int[4];
            bits[0] = 100;
            this.updAmount.Increment = new decimal(bits);
            this.updAmount.Location = new Point(0x90, 0x60);
            bits = new int[4];
            bits[0] = 0xf4240;
            this.updAmount.Maximum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x58, 20);
            this.updAmount.TabIndex = 0x1a;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            this.updAmount.ValueChanged += new EventHandler(this.cboDeductible_SelectedIndexChanged);
            this.label2.Location = new Point(40, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x20);
            this.label2.TabIndex = 0x1d;
            this.label2.Text = "Estimated Market Value:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.labValue.Location = new Point(0x98, 0x40);
            this.labValue.Name = "labValue";
            this.labValue.Size = new Size(80, 0x10);
            this.labValue.TabIndex = 30;
            this.labValue.Text = "$0";
            this.labValue.TextAlign = ContentAlignment.TopRight;
            this.labInsuranceRequired.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labInsuranceRequired.ForeColor = Color.Gray;
            this.labInsuranceRequired.Location = new Point(8, 0x10);
            this.labInsuranceRequired.Name = "labInsuranceRequired";
            this.labInsuranceRequired.Size = new Size(0x130, 0x18);
            this.labInsuranceRequired.TabIndex = 0x1f;
            this.labInsuranceRequired.Text = "You are required to have insurance on your new condo.";
            this.labInsuranceRequired.TextAlign = ContentAlignment.TopCenter;
            this.labInsuranceRequired.Visible = false;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x142, 0x110);
            base.Controls.Add(this.labInsuranceRequired);
            base.Controls.Add(this.labValue);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.labPremium);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.cboDeductible);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmHomeOwnersInsurance";
            base.ShowInTaskbar = false;
            this.Text = "Homeowner's Insurance";
            base.Closing += new CancelEventHandler(this.frmHomeOwnersInsurance_Closing);
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }

        public bool Cancellable
        {
            set
            {
                if (!value)
                {
                    this.btnCancel.Enabled = false;
                    this.labInsuranceRequired.Visible = true;
                }
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public float Value;
            public InsurancePolicy Policy;
        }
    }
}

