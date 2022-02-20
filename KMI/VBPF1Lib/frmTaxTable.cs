namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class frmTaxTable : Form
    {
        private Container components = null;

        public frmTaxTable()
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
            ResourceManager manager = new ResourceManager(typeof(frmTaxTable));
            this.AutoScaleBaseSize = new Size(5, 13);
            this.BackgroundImage = (Image) manager.GetObject("$this.BackgroundImage");
            base.ClientSize = new Size(0x252, 0x150);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmTaxTable";
            base.ShowInTaskbar = false;
            this.Text = "Tax Table";
        }
    }
}

