namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmCreateMessage : Form
    {
        private Button btnCancel;
        private Button btnSend;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labFrom;
        private Label labTo;
        private TextBox txtMemo;

        public frmCreateMessage()
        {
            this.InitializeComponent();
            this.labFrom.Text = S.Instance.MultiplayerRoleName + ", " + S.Instance.ThisPlayerName;
            this.labTo.Text = S.Instance.ThisPlayerName + " " + S.Resources.GetString("Executive Team");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            S.Adapter.SendMessage(this.labFrom.Text, S.Instance.ThisPlayerName, this.txtMemo.Text);
            base.Close();
        }

        private void button2_Click(object sender, EventArgs e)
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
            this.btnSend = new Button();
            this.btnCancel = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.labFrom = new Label();
            this.labTo = new Label();
            this.txtMemo = new TextBox();
            this.label3 = new Label();
            base.SuspendLayout();
            this.btnSend.Location = new Point(0x38, 0xe0);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new Size(0x60, 0x18);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new EventHandler(this.btnSend_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb8, 0xe0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.button2_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(8, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(40, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "From:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(8, 0x20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(40, 0x10);
            this.label2.TabIndex = 3;
            this.label2.Text = "To:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.labFrom.Location = new Point(0x40, 0x10);
            this.labFrom.Name = "labFrom";
            this.labFrom.Size = new Size(160, 0x10);
            this.labFrom.TabIndex = 4;
            this.labFrom.Text = "labFrom";
            this.labTo.Location = new Point(0x40, 0x20);
            this.labTo.Name = "labTo";
            this.labTo.Size = new Size(160, 0x10);
            this.labTo.TabIndex = 5;
            this.labTo.Text = "labTo";
            this.txtMemo.Location = new Point(0x40, 0x30);
            this.txtMemo.Multiline = true;
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new Size(0xe8, 0x90);
            this.txtMemo.TabIndex = 6;
            this.txtMemo.Text = "";
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x10, 0x48);
            this.label3.Name = "label3";
            this.label3.Size = new Size(40, 0x30);
            this.label3.TabIndex = 7;
            this.label3.Text = "Body of Memo:";
            base.AcceptButton = this.btnSend;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x152, 0x108);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.txtMemo);
            base.Controls.Add(this.labTo);
            base.Controls.Add(this.labFrom);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnSend);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmCreateMessage";
            base.ShowInTaskbar = false;
            this.Text = "Create Memo";
            base.ResumeLayout(false);
        }
    }
}

