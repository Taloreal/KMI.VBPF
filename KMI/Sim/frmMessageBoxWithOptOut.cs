namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMessageBoxWithOptOut : Form
    {
        private Button btnClose;
        private CheckBox chkDontShow;
        private Container components;
        private Label labText;

        public frmMessageBoxWithOptOut(string title, string message) : this(title, message, true)
        {
        }

        public frmMessageBoxWithOptOut(string title, string message, bool optOutEnabled)
        {
            this.components = null;
            this.InitializeComponent();
            this.Text = title;
            this.labText.Text = message;
            this.chkDontShow.Visible = optOutEnabled;
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void frmMessageBoxWithOptOut_Closed(object sender, EventArgs e)
        {
            if (this.chkDontShow.Checked)
            {
                S.Instance.DontShowAgain(this.Text);
            }
        }

        private void InitializeComponent()
        {
            this.labText = new Label();
            this.btnClose = new Button();
            this.chkDontShow = new CheckBox();
            base.SuspendLayout();
            this.labText.Location = new Point(0x18, 0x18);
            this.labText.Name = "labText";
            this.labText.Size = new Size(0x150, 0x60);
            this.labText.TabIndex = 0;
            this.labText.Text = "#";
            this.btnClose.Location = new Point(0x130, 0x80);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x38, 0x18);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "OK";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.chkDontShow.Location = new Point(0x20, 0x80);
            this.chkDontShow.Name = "chkDontShow";
            this.chkDontShow.Size = new Size(0xc0, 0x10);
            this.chkDontShow.TabIndex = 2;
            this.chkDontShow.Text = "Don't show me this again";
            base.AcceptButton = this.btnClose;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x180, 0xa6);
            base.Controls.Add(this.chkDontShow);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.labText);
            base.Name = "frmMessageBoxWithOptOut";
            base.ShowInTaskbar = false;
            base.Closed += new EventHandler(this.frmMessageBoxWithOptOut_Closed);
            base.ResumeLayout(false);
        }
    }
}

