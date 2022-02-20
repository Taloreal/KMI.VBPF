namespace KMI.Biz
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmInputAmount : Form
    {
        public float Amount;
        private Label labMsg;
        private Button btnOK;
        private Button btnCancel;
        //private Container components;
        private readonly Container components;
        public NumericUpDown updAmount;

        public frmInputAmount()
        {
            this.components = null;
            this.Amount = 0f;
            this.InitializeComponent();
        }

        public frmInputAmount(string title, string msg, float min, float max, float defaultValue)
        {
            this.components = null;
            this.Amount = 0f;
            this.InitializeComponent();
            this.Text = title;
            this.labMsg.Text = msg;
            this.updAmount.Maximum = (decimal) max;
            this.updAmount.Minimum = (decimal) min;
            this.updAmount.Value = (decimal) defaultValue;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Amount = (float) this.updAmount.Value;
            base.Close();
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
            this.labMsg = new Label();
            this.btnOK = new Button();
            this.updAmount = new NumericUpDown();
            this.btnCancel = new Button();

            this.updAmount.BeginInit();
            base.SuspendLayout();

            this.labMsg.Location = new Point(0x20, 0x18);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new Size(0xe8, 0x90);
            this.labMsg.TabIndex = 0;

            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x20, 0xe8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            this.updAmount.Location = new Point(80, 0xb8);
            this.updAmount.Name = "updAmount";
            this.updAmount.TabIndex = 2;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;

            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(160, 0xe8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x124, 0x10a);

            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.labMsg);

            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmInputAmount";
            base.ShowInTaskbar = false;
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }

        public decimal Increment
        {
            set
            {
                this.updAmount.Increment = value;
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input {
            public string title;
            public string msg;
            public float min;
            public float max;
            public float defaultValue;
        }
    }
}

