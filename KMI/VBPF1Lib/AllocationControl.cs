namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AllocationControl : UserControl
    {
        private Container components = null;
        public Label labFundName;
        public NumericUpDown updPct;

        public AllocationControl()
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
            this.updPct = new NumericUpDown();
            this.labFundName = new Label();
            this.updPct.BeginInit();
            base.SuspendLayout();
            this.updPct.Location = new Point(12, 8);
            this.updPct.Name = "updPct";
            this.updPct.Size = new Size(0x2c, 20);
            this.updPct.TabIndex = 0;
            this.updPct.TextAlign = HorizontalAlignment.Right;
            this.labFundName.Location = new Point(0x40, 12);
            this.labFundName.Name = "labFundName";
            this.labFundName.Size = new Size(0xb0, 0x10);
            this.labFundName.TabIndex = 1;
            this.labFundName.Text = "label1";
            base.Controls.Add(this.labFundName);
            base.Controls.Add(this.updPct);
            base.Name = "AllocationControl";
            base.Size = new Size(0xec, 0x20);
            this.updPct.EndInit();
            base.ResumeLayout(false);
        }
    }
}

