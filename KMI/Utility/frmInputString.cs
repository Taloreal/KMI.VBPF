namespace KMI.Utility
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmInputString : Form
    {
        private Button btnCancel;
        private Button btnOK;
        //private Container components = null;
        private readonly Container components;
        private Label labText;
        protected bool requireResponse;
        private TextBox txtResponse;

        public frmInputString(string title, string text, string defaultResponse)
        {
            this.InitializeComponent();
            this.Text = title;
            this.labText.Text = text;
            this.txtResponse.Text = defaultResponse;
            this.RequireResponse = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtResponse.Text = "";
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
            this.labText = new Label();
            this.txtResponse = new TextBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.labText.Location = new Point(0x10, 0x10);
            this.labText.Name = "labText";
            this.labText.Size = new Size(0x120, 0x48);
            this.labText.TabIndex = 0;
            this.labText.Text = "label1";
            this.labText.TextAlign = ContentAlignment.MiddleLeft;
            this.txtResponse.Location = new Point(0x10, 0x60);
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new Size(0x120, 20);
            this.txtResponse.TabIndex = 1;
            this.txtResponse.Text = "";
            this.txtResponse.Validating += new CancelEventHandler(this.txtResponse_Validating);
            this.btnOK.DialogResult = DialogResult.OK;
            this.btnOK.Location = new Point(0x148, 0x10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x38, 0x18);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnCancel.CausesValidation = false;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x148, 0x30);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x38, 0x18);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(400, 0x86);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.txtResponse);
            base.Controls.Add(this.labText);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmInputString";
            base.ShowInTaskbar = false;
            this.Text = "#";
            base.Closing += new CancelEventHandler(this.txtResponse_Validating);
            base.ResumeLayout(false);
        }

        private void txtResponse_Validating(object sender, CancelEventArgs e)
        {
            if ((this.txtResponse.Text == "") && this.requireResponse)
            {
                MessageBox.Show("You must enter a value.", "Please Retry");
                e.Cancel = true;
            }
        }

        public bool RequireResponse
        {
            set
            {
                this.requireResponse = value;
                this.btnCancel.Visible = !this.requireResponse;
                base.ControlBox = !this.requireResponse;
            }
        }

        public string Response
        {
            get
            {
                return this.txtResponse.Text;
            }
        }
    }
}

