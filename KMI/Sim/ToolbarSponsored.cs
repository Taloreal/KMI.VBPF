namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ToolbarSponsored : ToolBar
    {
        private Container components = null;
        public Label labLogo;

        public ToolbarSponsored()
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
            this.labLogo = new Label();
            base.SuspendLayout();
            this.labLogo.Dock = DockStyle.Right;
            this.labLogo.Location = new Point(110, 0);
            this.labLogo.Name = "labLogo";
            this.labLogo.Size = new Size(40, 150);
            this.labLogo.TabIndex = 0;
            base.Controls.Add(this.labLogo);
            base.Name = "ToolbarSponsored";
            base.ResumeLayout(false);
        }
    }
}

