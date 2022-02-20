namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmHealthInsurance : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboCopay;
        private Container components = null;
        private Label label3;
        private Label label4;
        private Label label6;
        private Label labPremium;
        private InsurancePolicy policy;

        public frmHealthInsurance()
        {
            this.InitializeComponent();
            this.policy = A.Adapter.GetHealthInsurance(A.MainForm.CurrentEntityID);
            if (A.Adapter.HasHealthInsuranceThruWork(A.MainForm.CurrentEntityID))
            {
                MessageBox.Show("You already have health insurance through your work. You may not want to purchase additional coverage.", "Health Insurance");
            }
            this.cboCopay.Items.Add(5f);
            this.cboCopay.Items.Add(10f);
            this.cboCopay.Items.Add(25f);
            this.cboCopay.Items.Add(50f);
            this.cboCopay.Items.Add("No Coverage");
            if (this.policy.Copay == -1f)
            {
                this.cboCopay.SelectedIndex = 4;
            }
            else
            {
                this.cboCopay.SelectedIndex = this.cboCopay.FindStringExact(this.policy.Copay.ToString());
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
                A.Adapter.SetHealthInsurance(A.MainForm.CurrentEntityID, this.policy);
                base.Close();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void cboCopay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboCopay.SelectedIndex == 4)
            {
                this.policy.Copay = -1f;
            }
            else
            {
                this.policy.Copay = (float) this.cboCopay.SelectedItem;
            }
            this.policy.MonthlyPremium = 0f;
            if (this.policy.Copay > 0f)
            {
                this.policy.MonthlyPremium = 200f + (500f / this.policy.Copay);
            }
            this.labPremium.Text = Utilities.FC(this.policy.MonthlyPremium * 12f, A.Instance.CurrencyConversion);
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
            this.label3 = new Label();
            this.label4 = new Label();
            this.labPremium = new Label();
            this.label6 = new Label();
            this.cboCopay = new ComboBox();
            base.SuspendLayout();
            this.btnHelp.Location = new Point(0xd8, 0xc0);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 0x10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(120, 0xc0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Location = new Point(0x18, 0xc0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label3.Location = new Point(0x20, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x100, 0x48);
            this.label3.TabIndex = 20;
            this.label3.Text = "Covers hospital expense, surgical expense, and physician expense as well as major medical expense. Includes prescription drug coverage. ";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.label4.Location = new Point(40, 0x90);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x58, 0x18);
            this.label4.TabIndex = 0x15;
            this.label4.Text = "Yearly Premium:";
            this.label4.TextAlign = ContentAlignment.TopRight;
            this.labPremium.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPremium.Location = new Point(0x90, 0x90);
            this.labPremium.Name = "labPremium";
            this.labPremium.Size = new Size(0x70, 0x10);
            this.labPremium.TabIndex = 0x16;
            this.labPremium.Text = "$0";
            this.label6.Location = new Point(0x10, 0x60);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x88, 0x20);
            this.label6.TabIndex = 0x19;
            this.label6.Text = "Copay on Office Visits and Prescription Drugs:";
            this.label6.TextAlign = ContentAlignment.TopRight;
            this.cboCopay.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboCopay.Location = new Point(0xa8, 0x68);
            this.cboCopay.Name = "cboCopay";
            this.cboCopay.Size = new Size(0x58, 0x15);
            this.cboCopay.TabIndex = 0x18;
            this.cboCopay.SelectedIndexChanged += new EventHandler(this.cboCopay_SelectedIndexChanged);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x142, 0xe8);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.cboCopay);
            base.Controls.Add(this.labPremium);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmHealthInsurance";
            base.ShowInTaskbar = false;
            this.Text = "Health Insurance";
            base.ResumeLayout(false);
        }
    }
}

