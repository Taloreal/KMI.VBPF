namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPayInCash : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Container components = null;
        private Label labCash;
        private Label label2;
        private Label label3;
        public NumericUpDown updAmount;

        public frmPayInCash()
        {
            this.InitializeComponent();
            this.labCash.Text = Utilities.FC(A.Adapter.GetCash(A.MainForm.CurrentEntityID), A.Instance.CurrencyConversion);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Pay Bills"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
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
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.btnOK = new Button();
            this.updAmount = new NumericUpDown();
            this.label2 = new Label();
            this.label3 = new Label();
            this.labCash = new Label();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x6c, 0xb8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0xd0, 0xb8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 13;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(12, 0xb8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x4c, 0x18);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.updAmount.DecimalPlaces = 2;
            this.updAmount.Location = new Point(0x98, 0x74);
            int[] bits = new int[4];
            bits[0] = 0xf4240;
            this.updAmount.Maximum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x60, 20);
            this.updAmount.TabIndex = 0x10;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            bits = new int[4];
            bits[0] = 20;
            this.updAmount.Value = new decimal(bits);
            this.label2.Location = new Point(0x34, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x10);
            this.label2.TabIndex = 15;
            this.label2.Text = "Amount to Pay:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.label3.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x20, 0x18);
            this.label3.Name = "label3";
            this.label3.Size = new Size(240, 0x18);
            this.label3.TabIndex = 0x12;
            this.label3.Text = "Current Cash Balance:";
            this.label3.TextAlign = ContentAlignment.TopCenter;
            this.labCash.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labCash.Location = new Point(0x20, 0x34);
            this.labCash.Name = "labCash";
            this.labCash.Size = new Size(240, 0x18);
            this.labCash.TabIndex = 0x13;
            this.labCash.TextAlign = ContentAlignment.TopCenter;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(300, 0xe2);
            base.Controls.Add(this.labCash);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmPayInCash";
            base.ShowInTaskbar = false;
            this.Text = "Pay In Cash";
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }
    }
}

