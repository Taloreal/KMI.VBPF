namespace KMI.Utility
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmPassword : Form
    {
        private Button btnCancel;
        private Button btnOK;
        //private Container components = null;
        private readonly Container components;
        private Label label1;
        private TextBox txtPassword;
        //private string validPassword;
        private readonly string validPassword;

        public frmPassword(string validPassword)
        {
            this.InitializeComponent();
            this.validPassword = validPassword;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.txtPassword.Text.ToUpper() == this.validPassword.ToUpper())
            {
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show(this, "Invalid Password, try again!", "Password");
                base.ActiveControl = this.txtPassword;
                this.txtPassword.SelectAll();
                base.DialogResult = DialogResult.None;
            }
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
            this.btnCancel = new Button();
            this.label1 = new Label();
            this.txtPassword = new TextBox();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x30, 0x70);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb0, 0x70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.label1.Location = new Point(0x18, 40);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x40, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password:";
            this.txtPassword.Location = new Point(0x60, 40);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(0xc0, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "";
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x142, 160);
            base.Controls.Add(this.txtPassword);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmPassword";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Password Required";
            base.ResumeLayout(false);
        }
    }
}

