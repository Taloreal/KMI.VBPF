namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmUploadAlternative : Form
    {
        private Button btnClose;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox txtCipher;

        public frmUploadAlternative(string post)
        {
            this.InitializeComponent();
            string key = "prkdeGRNIK648593648qwcvhufUYTFVC3456748392KJHSDFftyfDFHCtwpolao82935";
            this.txtCipher.Text = Utilities.Encrypt(post, key);
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

        private void InitializeComponent()
        {
            this.btnClose = new Button();
            this.txtCipher = new TextBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            base.SuspendLayout();
            this.btnClose.Location = new Point(0x108, 0xc0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x68, 0x18);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.txtCipher.Location = new Point(0x10, 0x10);
            this.txtCipher.Multiline = true;
            this.txtCipher.Name = "txtCipher";
            this.txtCipher.Size = new Size(600, 40);
            this.txtCipher.TabIndex = 1;
            this.txtCipher.Text = "";
            this.label1.Location = new Point(0x70, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(440, 0x18);
            this.label1.TabIndex = 2;
            this.label1.Text = "1. Open your web browser and go to www.KnowledgeMatters.com/VBCUpload";
            this.label2.Location = new Point(0x70, 0x68);
            this.label2.Name = "label2";
            this.label2.Size = new Size(440, 0x18);
            this.label2.TabIndex = 3;
            this.label2.Text = "2. Copy all the text above and paste it into the box marked Encrypted Score.";
            this.label3.Location = new Point(0x70, 0x80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(440, 0x18);
            this.label3.TabIndex = 4;
            this.label3.Text = "3. Hit the Submit button on that web page.";
            this.label4.Location = new Point(0x70, 0x98);
            this.label4.Name = "label4";
            this.label4.Size = new Size(440, 0x18);
            this.label4.TabIndex = 5;
            this.label4.Text = "4. Check the VBC Rankings page at KnowledgeMatters.com for your score.";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x27a, 0xe8);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.txtCipher);
            base.Controls.Add(this.btnClose);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmUploadAlternative";
            base.ShowInTaskbar = false;
            this.Text = "Alternative Upload";
            base.ResumeLayout(false);
        }
    }
}

