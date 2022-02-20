namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmRentersInsurance : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboDeductible;
        private Container components = null;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label labPremium;
        private InsurancePolicy policy;
        private NumericUpDown updAmount;

        public frmRentersInsurance()
        {
            this.InitializeComponent();
            this.policy = A.Adapter.GetRentersInsurance(A.MainForm.CurrentEntityID);
            this.cboDeductible.Items.Add(250f);
            this.cboDeductible.Items.Add(500f);
            this.cboDeductible.Items.Add(1000f);
            this.cboDeductible.Items.Add(2000f);
            this.cboDeductible.Items.Add("No Coverage");
            this.updAmount.Value = (decimal) this.policy.Limit;
            if (this.policy.Deductible == -1f)
            {
                this.cboDeductible.SelectedIndex = 4;
            }
            else
            {
                this.cboDeductible.SelectedIndex = this.cboDeductible.FindStringExact(this.policy.Deductible.ToString());
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Renters Insurance"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.cboDeductible_SelectedIndexChanged(new object(), new EventArgs());
                A.Adapter.SetRentersInsurance(A.MainForm.CurrentEntityID, this.policy);
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
                    this.policy.Deductible = -1f;
                }
                else
                {
                    this.policy.Deductible = (float) this.cboDeductible.SelectedItem;
                }
                if (this.updAmount.Value < ((decimal) this.policy.Deductible))
                {
                    this.updAmount.Value = (decimal) this.policy.Deductible;
                }
                this.policy.Limit = (float) this.updAmount.Value;
                this.policy.MonthlyPremium = 0f;
                if (this.policy.Deductible > 0f)
                {
                    this.policy.MonthlyPremium = (((float) this.updAmount.Value) / 5000f) * (10f + (2000f / this.policy.Deductible));
                }
                this.labPremium.Text = Utilities.FC(this.policy.MonthlyPremium * 12f, A.Instance.CurrencyConversion);
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
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.cboDeductible = new ComboBox();
            this.label4 = new Label();
            this.labPremium = new Label();
            this.label6 = new Label();
            this.label7 = new Label();
            this.updAmount = new NumericUpDown();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.btnHelp.Location = new Point(0xd8, 0xb8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 0x10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(120, 0xb8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Location = new Point(0x18, 0xb8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.cboDeductible.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDeductible.Location = new Point(0x90, 80);
            this.cboDeductible.Name = "cboDeductible";
            this.cboDeductible.Size = new Size(0x58, 0x15);
            this.cboDeductible.TabIndex = 0x11;
            this.cboDeductible.SelectedIndexChanged += new EventHandler(this.cboDeductible_SelectedIndexChanged);
            this.label4.Location = new Point(0x30, 0x80);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x58, 0x18);
            this.label4.TabIndex = 0x15;
            this.label4.Text = "Yearly Premium:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.labPremium.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPremium.Location = new Point(0x98, 0x80);
            this.labPremium.Name = "labPremium";
            this.labPremium.Size = new Size(0x70, 0x10);
            this.labPremium.TabIndex = 0x16;
            this.labPremium.Text = "$0";
            this.label6.Location = new Point(40, 80);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x58, 0x18);
            this.label6.TabIndex = 0x17;
            this.label6.Text = "Deductible:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.label7.Location = new Point(0x20, 0x20);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x20);
            this.label7.TabIndex = 0x19;
            this.label7.Text = "Amount of Coverage:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            int[] bits = new int[4];
            bits[0] = 100;
            this.updAmount.Increment = new decimal(bits);
            this.updAmount.Location = new Point(0x90, 40);
            bits = new int[4];
            bits[0] = 0xf4240;
            this.updAmount.Maximum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x58, 20);
            this.updAmount.TabIndex = 0x1a;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            this.updAmount.ValueChanged += new EventHandler(this.cboDeductible_SelectedIndexChanged);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x142, 0xe8);
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
            base.Name = "frmRentersInsurance";
            base.ShowInTaskbar = false;
            this.Text = "Renter's Insurance";
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }
    }
}

