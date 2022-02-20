namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPlayMacro : Form
    {
        private Button butCancel;
        private Button butOK;
        private Container components = null;
        private OpenFileDialog dlgOpenFile;
        private GroupBox groupBox1;
        private Label labMs;
        public RadioButton optContinuously;
        public RadioButton optSimDates;
        public NumericUpDown updInterval;

        public frmPlayMacro()
        {
            this.InitializeComponent();
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
            this.updInterval = new NumericUpDown();
            this.labMs = new Label();
            this.dlgOpenFile = new OpenFileDialog();
            this.butOK = new Button();
            this.optSimDates = new RadioButton();
            this.optContinuously = new RadioButton();
            this.groupBox1 = new GroupBox();
            this.butCancel = new Button();
            this.updInterval.BeginInit();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.updInterval.Enabled = false;
            int[] bits = new int[4];
            bits[0] = 20;
            this.updInterval.Increment = new decimal(bits);
            this.updInterval.Location = new Point(0x38, 0x48);
            bits = new int[4];
            bits[0] = 0x186a0;
            this.updInterval.Maximum = new decimal(bits);
            this.updInterval.Name = "updInterval";
            this.updInterval.Size = new Size(0x40, 20);
            this.updInterval.TabIndex = 4;
            this.updInterval.TextAlign = HorizontalAlignment.Right;
            this.labMs.Enabled = false;
            this.labMs.Location = new Point(0x80, 0x48);
            this.labMs.Name = "labMs";
            this.labMs.Size = new Size(0x38, 0x17);
            this.labMs.TabIndex = 6;
            this.labMs.Text = "ms/action";
            this.labMs.TextAlign = ContentAlignment.MiddleLeft;
            this.butOK.DialogResult = DialogResult.OK;
            this.butOK.Location = new Point(0x10, 0x80);
            this.butOK.Name = "butOK";
            this.butOK.Size = new Size(80, 0x18);
            this.butOK.TabIndex = 7;
            this.butOK.Text = "OK";
            this.optSimDates.Checked = true;
            this.optSimDates.Location = new Point(0x18, 0x18);
            this.optSimDates.Name = "optSimDates";
            this.optSimDates.Size = new Size(0x60, 0x10);
            this.optSimDates.TabIndex = 8;
            this.optSimDates.TabStop = true;
            this.optSimDates.Text = "At Sim Dates";
            this.optSimDates.CheckedChanged += new EventHandler(this.optSimDates_CheckedChanged);
            this.optContinuously.Location = new Point(0x18, 0x30);
            this.optContinuously.Name = "optContinuously";
            this.optContinuously.Size = new Size(0x60, 0x10);
            this.optContinuously.TabIndex = 9;
            this.optContinuously.Text = "Continuously";
            this.groupBox1.Controls.Add(this.optContinuously);
            this.groupBox1.Controls.Add(this.optSimDates);
            this.groupBox1.Controls.Add(this.labMs);
            this.groupBox1.Controls.Add(this.updInterval);
            this.groupBox1.Location = new Point(0x10, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xc0, 0x68);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Play Actions";
            this.butCancel.DialogResult = DialogResult.Cancel;
            this.butCancel.Location = new Point(0x80, 0x80);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new Size(80, 0x18);
            this.butCancel.TabIndex = 11;
            this.butCancel.Text = "Cancel";
            base.AcceptButton = this.butOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.butCancel;
            base.ClientSize = new Size(0xe2, 0xa6);
            base.Controls.Add(this.butCancel);
            base.Controls.Add(this.butOK);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmPlayMacro";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Macro Play Settings";
            this.updInterval.EndInit();
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void optSimDates_CheckedChanged(object sender, EventArgs e)
        {
            this.updInterval.Enabled = !this.optSimDates.Checked;
        }
    }
}

