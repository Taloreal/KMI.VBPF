namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAutoInsurance : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Car car;
        private float carValue;
        private ComboBox cboDeductible;
        private Container components = null;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label label7;
        private Label labPremium;
        private Label labValue;

        public frmAutoInsurance()
        {
            this.InitializeComponent();
            this.car = A.Adapter.GetAutoInsurance(A.MainForm.CurrentEntityID);
            this.cboDeductible.Items.Add(250f);
            this.cboDeductible.Items.Add(500f);
            this.cboDeductible.Items.Add(1000f);
            this.cboDeductible.Items.Add(2000f);
            this.cboDeductible.Items.Add("No Coverage");
            this.carValue = this.car.ComputeResalePrice(A.MainForm.Now);
            this.labValue.Text = Utilities.FC(this.carValue, A.Instance.CurrencyConversion);
            if (this.car.Insurance.Deductible == -1f)
            {
                this.cboDeductible.SelectedIndex = 4;
            }
            else
            {
                this.cboDeductible.SelectedIndex = this.cboDeductible.FindStringExact(this.car.Insurance.Deductible.ToString());
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                A.Adapter.SetAutoInsurance(A.MainForm.CurrentEntityID, this.car.Insurance);
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
            if (this.cboDeductible.SelectedIndex == 4)
            {
                this.car.Insurance.Deductible = -1f;
            }
            else
            {
                this.car.Insurance.Deductible = (float) this.cboDeductible.SelectedItem;
            }
            this.car.Insurance.MonthlyPremium = 80f;
            if (this.car.Insurance.Deductible > 0f)
            {
                this.car.Insurance.MonthlyPremium += 20000f / this.car.Insurance.Deductible;
            }
            this.labPremium.Text = Utilities.FC(this.car.Insurance.MonthlyPremium * 12f, A.Instance.CurrencyConversion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmAutoInsurance_Closing(object sender, CancelEventArgs e)
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
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.labPremium = new Label();
            this.label6 = new Label();
            this.labValue = new Label();
            this.label7 = new Label();
            base.SuspendLayout();
            this.btnHelp.Location = new Point(0xe0, 0xe8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 0x10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x80, 0xe8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Location = new Point(0x20, 0xe8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.cboDeductible.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboDeductible.Location = new Point(0x90, 0x48);
            this.cboDeductible.Name = "cboDeductible";
            this.cboDeductible.Size = new Size(0x58, 0x15);
            this.cboDeductible.TabIndex = 0x11;
            this.cboDeductible.SelectedIndexChanged += new EventHandler(this.cboDeductible_SelectedIndexChanged);
            this.label2.Location = new Point(0x30, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(80, 40);
            this.label2.TabIndex = 0x13;
            this.label2.Text = "Mandatory Liability Coverage:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label3.Location = new Point(0x98, 0x80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x90, 0x20);
            this.label3.TabIndex = 20;
            this.label3.Text = "$100,000 Bodily Injury";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
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
            this.label6.Location = new Point(0x40, 0x40);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x40, 0x20);
            this.label6.TabIndex = 0x17;
            this.label6.Text = "Collision Deductible:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.labValue.Location = new Point(0x90, 0x20);
            this.labValue.Name = "labValue";
            this.labValue.Size = new Size(0x70, 0x10);
            this.labValue.TabIndex = 0x1a;
            this.label7.Location = new Point(0x20, 0x20);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x10);
            this.label7.TabIndex = 0x19;
            this.label7.Text = "Estimated Value:";
            this.label7.TextAlign = ContentAlignment.TopRight;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(330, 280);
            base.Controls.Add(this.labValue);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.labPremium);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cboDeductible);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmAutoInsurance";
            base.ShowInTaskbar = false;
            this.Text = "Auto Insurance";
            base.Closing += new CancelEventHandler(this.frmAutoInsurance_Closing);
            base.ResumeLayout(false);
        }

        public bool Cancellable
        {
            set
            {
                if (!value)
                {
                    this.btnCancel.Enabled = false;
                }
            }
        }
    }
}

