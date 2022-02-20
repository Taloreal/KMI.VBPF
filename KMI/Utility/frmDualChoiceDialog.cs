namespace KMI.Utility
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDualChoiceDialog : Form
    {
        private Button btnCancel;
        private Button btnChoice0;
        private Button btnChoice1;
        private Container components = null;
        private Label label1;

        public frmDualChoiceDialog(string caption, string button1Text, string button2Text, bool allowCancel)
        {
            this.InitializeComponent();
            this.label1.Text = caption;
            this.btnChoice0.Text = button1Text;
            this.btnChoice1.Text = button2Text;
            this.btnCancel.Visible = allowCancel;
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
            this.label1 = new Label();
            this.btnChoice0 = new Button();
            this.btnChoice1 = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.label1.Location = new Point(0x18, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x150, 80);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            this.label1.TextAlign = ContentAlignment.MiddleCenter;
            this.btnChoice0.DialogResult = DialogResult.Yes;
            this.btnChoice0.Location = new Point(0x20, 0x70);
            this.btnChoice0.Name = "btnChoice0";
            this.btnChoice0.Size = new Size(0x90, 40);
            this.btnChoice0.TabIndex = 1;
            this.btnChoice0.Text = "#";
            this.btnChoice1.DialogResult = DialogResult.No;
            this.btnChoice1.Location = new Point(200, 0x70);
            this.btnChoice1.Name = "btnChoice1";
            this.btnChoice1.Size = new Size(0x90, 40);
            this.btnChoice1.TabIndex = 2;
            this.btnChoice1.Text = "#";
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x88, 0xb0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x68, 0x18);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            base.AcceptButton = this.btnChoice0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x182, 0xd0);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnChoice1);
            base.Controls.Add(this.btnChoice0);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmDualChoiceDialog";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Please choose:";
            base.ResumeLayout(false);
        }
    }
}

