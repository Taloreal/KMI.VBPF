namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAbout : Form
    {
        private Button btnCurrentSimInfo;
        private Button btnOK;
        //private IContainer components;
        private readonly IContainer components;
        protected Label labCopyrightInfo;
        private Label labProductName;
        private Label labVersion;

        public frmAbout() {
            this.InitializeComponent();
            this.btnCurrentSimInfo.Visible = S.MainForm.DesignerMode;
            this.Text = S.Resources.GetString("About") + " " + Application.ProductName;
            this.labProductName.Text = S.Resources.GetString("Product Name") + ": " + Application.ProductName;
            if (S.Instance.VBC) { labProductName.Text += S.Resources.GetString(" - VBC Edition"); }
            if (S.Instance.Demo) { labProductName.Text += S.Resources.GetString(" - Demo Edition"); }
            this.labVersion.Text = S.Resources.GetString("Product Version") + ": " + Application.ProductVersion;
            string[] textArray1 = new string[] { 
                S.Resources.GetString("Copyright"), 
                " ", DateTime.Now.Year.ToString(), 
                " ", Application.CompanyName, 
                ". ", S.Resources.GetString("All rights reserved worldwide.") 
            };
            this.labCopyrightInfo.Text = string.Concat(textArray1);
        }

        private void btnCurrentSimInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sim Guid = " + S.State.GUID.ToString());
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
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

        private void frmAbout_Load(object sender, EventArgs e)
        {
        }

        private void frmSplashAbout_MouseUp(object sender, MouseEventArgs e)
        {
            if ((((e.X > 8) && (e.X < 0x20)) && (e.Y > 8)) && (e.Y < 0x20))
            {
                frmPassword password = new frmPassword("ponkey");
                if (password.ShowDialog(this) == DialogResult.OK)
                {
                    S.MainForm.DesignerMode = !S.MainForm.DesignerMode;
                    this.btnCurrentSimInfo.Visible = S.MainForm.DesignerMode;
                    S.MainForm.EnableDisable();
                }
            }
        }

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.btnCurrentSimInfo = new Button();
            this.labProductName = new Label();
            this.labVersion = new Label();
            this.labCopyrightInfo = new Label();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x70, 0xb0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x68, 0x18);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCurrentSimInfo.Location = new Point(0xe8, 0xa8);
            this.btnCurrentSimInfo.Name = "btnCurrentSimInfo";
            this.btnCurrentSimInfo.Size = new Size(0x40, 0x20);
            this.btnCurrentSimInfo.TabIndex = 3;
            this.btnCurrentSimInfo.Text = "Current Sim Info";
            this.btnCurrentSimInfo.Click += new EventHandler(this.btnCurrentSimInfo_Click);
            this.labProductName.Location = new Point(0x20, 0x20);
            this.labProductName.Name = "labProductName";
            this.labProductName.Size = new Size(0x108, 0x20);
            this.labProductName.TabIndex = 4;
            this.labProductName.Text = "label1";
            this.labProductName.TextAlign = ContentAlignment.BottomLeft;
            this.labVersion.Location = new Point(0x20, 0x48);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new Size(0x108, 0x10);
            this.labVersion.TabIndex = 5;
            this.labVersion.Text = "label1";
            this.labCopyrightInfo.Location = new Point(0x20, 0x68);
            this.labCopyrightInfo.Name = "labCopyrightInfo";
            this.labCopyrightInfo.Size = new Size(0x108, 0x30);
            this.labCopyrightInfo.TabIndex = 6;
            this.labCopyrightInfo.Text = "label1";
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x142, 0xd8);
            base.Controls.Add(this.labCopyrightInfo);
            base.Controls.Add(this.labVersion);
            base.Controls.Add(this.labProductName);
            base.Controls.Add(this.btnCurrentSimInfo);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Name = "frmAbout";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmSplashAbout";
            base.Load += new EventHandler(this.frmAbout_Load);
            base.MouseUp += new MouseEventHandler(this.frmSplashAbout_MouseUp);
            base.ResumeLayout(false);
        }
    }
}

