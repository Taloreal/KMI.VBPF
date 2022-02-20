namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmLegend : Form
    {
        private Container components = null;
        private Panel panel1;

        public frmLegend()
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
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmLegend));
            this.panel1 = new Panel();
            base.SuspendLayout();
            this.panel1.BackgroundImage = (Image) manager.GetObject("panel1.BackgroundImage");
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(160, 0x357);
            this.panel1.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.AutoScroll = true;
            this.BackColor = Color.White;
            base.ClientSize = new Size(0xa4, 0xfe);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "frmLegend";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Legend";
            base.ResumeLayout(false);
        }
    }
}

