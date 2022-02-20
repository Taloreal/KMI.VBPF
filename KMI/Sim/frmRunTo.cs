namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmRunTo : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOk;
        private MonthCalendar cal1;
        private ComboBox cboUnits;
        private Container components = null;
        private Input input;
        private Label label1;
        private RadioButton radCancel;
        private RadioButton radFor;
        private RadioButton radTo;
        private NumericUpDown updPeriods;

        public frmRunTo()
        {
            this.InitializeComponent();
            this.input = S.Adapter.GetRunTo();
            this.cal1.TodayDate = this.input.now;
            this.cal1.MinDate = this.input.now.AddDays(1.0);
            if (this.input.runTo < DateTime.MaxValue)
            {
                this.cal1.SelectionStart = (this.input.runTo < this.cal1.MinDate) ? this.cal1.MinDate : this.input.runTo;
                this.radTo.Checked = true;
            }
            else
            {
                this.cal1.SelectionStart = this.input.now.AddDays(1.0);
            }
            this.cboUnits.SelectedIndex = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp("Run to");
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.radTo.Checked)
            {
                S.Adapter.SetRunTo(this.cal1.SelectionStart);
            }
            else if (this.radFor.Checked)
            {
                int daysAhead = (int) this.updPeriods.Value;
                if (this.cboUnits.SelectedIndex == 1)
                {
                    daysAhead *= 7;
                }
                S.Adapter.SetRunTo(daysAhead);
            }
            else
            {
                if (!this.radCancel.Checked)
                {
                    return;
                }
                S.Adapter.SetRunTo(DateTime.MaxValue);
            }
            base.Close();
        }

        private void cal1_MouseDown(object sender, MouseEventArgs e)
        {
            this.radTo.Checked = true;
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
            this.radTo = new RadioButton();
            this.cal1 = new MonthCalendar();
            this.radFor = new RadioButton();
            this.label1 = new Label();
            this.updPeriods = new NumericUpDown();
            this.radCancel = new RadioButton();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.btnHelp = new Button();
            this.cboUnits = new ComboBox();
            this.updPeriods.BeginInit();
            base.SuspendLayout();
            this.radTo.Location = new Point(0x20, 0x18);
            this.radTo.Name = "radTo";
            this.radTo.Size = new Size(0x60, 0x18);
            this.radTo.TabIndex = 0;
            this.radTo.Text = "Run to Date:";
            this.cal1.Location = new Point(0x88, 0x18);
            this.cal1.MaxSelectionCount = 1;
            this.cal1.Name = "cal1";
            this.cal1.TabIndex = 1;
            this.cal1.MouseDown += new MouseEventHandler(this.cal1_MouseDown);
            this.radFor.Location = new Point(0x20, 200);
            this.radFor.Name = "radFor";
            this.radFor.Size = new Size(0x90, 0x10);
            this.radFor.TabIndex = 2;
            this.radFor.Text = "Run for Period of Time:";
            this.label1.Location = new Point(0x68, 240);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x18);
            this.label1.TabIndex = 3;
            this.label1.Text = "Run for";
            this.updPeriods.Location = new Point(0x98, 240);
            this.updPeriods.Maximum = 10000M;
            this.updPeriods.Minimum = 1M;
            this.updPeriods.Name = "updPeriods";
            this.updPeriods.Size = new Size(0x40, 20);
            this.updPeriods.TabIndex = 4;
            this.updPeriods.TextAlign = HorizontalAlignment.Right;
            this.updPeriods.Value = 1M;
            this.updPeriods.MouseDown += new MouseEventHandler(this.updPeriods_MouseDown);
            this.radCancel.Location = new Point(0x20, 0x128);
            this.radCancel.Name = "radCancel";
            this.radCancel.Size = new Size(0xc0, 0x10);
            this.radCancel.TabIndex = 5;
            this.radCancel.Text = "Cancel \"Run to\" -- Run Normally";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x90, 0x150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOk.Location = new Point(0x18, 0x150);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x60, 0x18);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnHelp.DialogResult = DialogResult.Cancel;
            this.btnHelp.Location = new Point(0x108, 0x150);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 8;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.cboUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            object[] items = new object[] { "Days", "Weeks" };
            this.cboUnits.Items.AddRange(items);
            this.cboUnits.Location = new Point(0xe0, 240);
            this.cboUnits.Name = "cboUnits";
            this.cboUnits.Size = new Size(80, 0x15);
            this.cboUnits.TabIndex = 9;
            this.cboUnits.MouseDown += new MouseEventHandler(this.updPeriods_MouseDown);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x182, 0x180);
            base.Controls.Add(this.cboUnits);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Controls.Add(this.radCancel);
            base.Controls.Add(this.updPeriods);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.radFor);
            base.Controls.Add(this.cal1);
            base.Controls.Add(this.radTo);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmRunTo";
            base.ShowInTaskbar = false;
            this.Text = "Run to...";
            this.updPeriods.EndInit();
            base.ResumeLayout(false);
        }

        private void updPeriods_MouseDown(object sender, MouseEventArgs e)
        {
            this.radFor.Checked = true;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public DateTime runTo;
            public DateTime now;
        }
    }
}

