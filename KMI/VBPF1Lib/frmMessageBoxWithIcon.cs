namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMessageBoxWithIcon : Form
    {
        private Button btnOK;
        private Container components = null;
        private Label labIcon;
        private Label labMessage;

        public frmMessageBoxWithIcon(string message, Bitmap icon)
        {
            this.InitializeComponent();
            this.Text = Application.ProductName;
            this.labIcon.Image = icon;
            this.labMessage.Text = message;
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

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.labIcon = new Label();
            this.labMessage = new Label();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x94, 0x7c);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x70, 0x18);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.labIcon.Location = new Point(20, 12);
            this.labIcon.Name = "labIcon";
            this.labIcon.Size = new Size(0x6c, 0x5c);
            this.labIcon.TabIndex = 1;
            this.labMessage.Location = new Point(0x90, 12);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new Size(0xf8, 0x58);
            this.labMessage.TabIndex = 2;
            this.labMessage.TextAlign = ContentAlignment.MiddleLeft;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x19c, 0x9e);
            base.Controls.Add(this.labMessage);
            base.Controls.Add(this.labIcon);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmMessageBoxWithIcon";
            base.ShowInTaskbar = false;
            this.Text = "frmMessageBoxWithIcon";
            base.ResumeLayout(false);
        }
    }
}

