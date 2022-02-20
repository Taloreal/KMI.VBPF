namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmW4 : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Container components = null;
        private Input input;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label labName;
        private Label labSSN;
        private Panel panel1;
        private Panel panMain;
        private long taskID;
        private TextBox txtAdditional;
        private TextBox txtAllowances;
        private TextBox txtExempt;

        public frmW4(long taskID)
        {
            this.InitializeComponent();
            this.input = A.Adapter.GetAllowances(A.MainForm.CurrentEntityID, taskID);
            this.txtAllowances.Text = this.input.Allowances.ToString();
            this.txtAdditional.Text = this.input.Additional.ToString("N2");
            this.taskID = taskID;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Getting Paid"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int allowances = int.Parse(this.txtAllowances.Text);
                float additional = 0f;
                if (this.txtAdditional.Text != "")
                {
                    additional = float.Parse(this.txtAdditional.Text);
                }
                if (additional < 0f)
                {
                    MessageBox.Show(A.Resources.GetString("Amount on Line 6 cannot be less than zero. Please try again."), A.Resources.GetString("Please Retry"));
                }
                else if (allowances < 0)
                {
                    MessageBox.Show(A.Resources.GetString("Allowances on Line 5 cannot be less than zero. Please try again."), A.Resources.GetString("Please Retry"));
                }
                else if (allowances > 2)
                {
                    MessageBox.Show(A.Resources.GetString("Allowances on Line 5 should not be more than 2. Please review lines A-H above and try again."), A.Resources.GetString("Please Retry"));
                }
                else
                {
                    A.Adapter.SetAllowances(A.MainForm.CurrentEntityID, this.taskID, this.txtExempt.Text.Trim().ToUpper() == "EXEMPT", allowances, additional);
                    base.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show(A.Resources.GetString("Incorrect amount. Please try again."), A.Resources.GetString("Please Retry"));
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

        private void frmW4_Load(object sender, EventArgs e)
        {
            if (this.input.DisabledForCompetition)
            {
                MessageBox.Show("For this competitive simulation, your allowances will be automatically set for you.", "Form W-4");
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(frmW4));
            this.panel1 = new Panel();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.panMain = new Panel();
            this.label3 = new Label();
            this.txtExempt = new TextBox();
            this.txtAdditional = new TextBox();
            this.txtAllowances = new TextBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.labSSN = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.labName = new Label();
            this.panel1.SuspendLayout();
            this.panMain.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x278, 0x38);
            this.panel1.TabIndex = 0;
            this.btnHelp.Location = new Point(0x18c, 0x10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(80, 0x18);
            this.btnHelp.TabIndex = 0x13;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x114, 0x10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(80, 0x18);
            this.btnCancel.TabIndex = 0x12;
            this.btnCancel.Text = "Cancel";
            this.btnOK.Location = new Point(0x9c, 0x10);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(80, 0x18);
            this.btnOK.TabIndex = 0x11;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.panMain.BackgroundImage = (Image) manager.GetObject("panMain.BackgroundImage");
            this.panMain.Controls.Add(this.label3);
            this.panMain.Controls.Add(this.txtExempt);
            this.panMain.Controls.Add(this.txtAdditional);
            this.panMain.Controls.Add(this.txtAllowances);
            this.panMain.Controls.Add(this.label7);
            this.panMain.Controls.Add(this.label6);
            this.panMain.Controls.Add(this.label5);
            this.panMain.Controls.Add(this.label4);
            this.panMain.Controls.Add(this.labSSN);
            this.panMain.Controls.Add(this.label2);
            this.panMain.Controls.Add(this.label1);
            this.panMain.Controls.Add(this.labName);
            this.panMain.Dock = DockStyle.Fill;
            this.panMain.Location = new Point(0, 0);
            this.panMain.Name = "panMain";
            this.panMain.Size = new Size(0x278, 0x256);
            this.panMain.TabIndex = 1;
            this.label3.BackColor = Color.White;
            this.label3.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(0x15c, 0x178);
            this.label3.Name = "label3";
            this.label3.Size = new Size(8, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "X";
            this.txtExempt.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.txtExempt.BorderStyle = BorderStyle.None;
            this.txtExempt.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtExempt.Location = new Point(0x1f0, 0x1f2);
            this.txtExempt.Name = "txtExempt";
            this.txtExempt.Size = new Size(120, 11);
            this.txtExempt.TabIndex = 10;
            this.txtAdditional.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.txtAdditional.BorderStyle = BorderStyle.None;
            this.txtAdditional.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtAdditional.Location = new Point(0x238, 0x1bb);
            this.txtAdditional.Name = "txtAdditional";
            this.txtAdditional.Size = new Size(0x30, 11);
            this.txtAdditional.TabIndex = 9;
            this.txtAdditional.TextAlign = HorizontalAlignment.Right;
            this.txtAllowances.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.txtAllowances.BorderStyle = BorderStyle.None;
            this.txtAllowances.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtAllowances.Location = new Point(0x238, 430);
            this.txtAllowances.Name = "txtAllowances";
            this.txtAllowances.Size = new Size(0x30, 11);
            this.txtAllowances.TabIndex = 8;
            this.txtAllowances.Text = "1";
            this.txtAllowances.TextAlign = HorizontalAlignment.Right;
            this.label7.BackColor = Color.White;
            this.label7.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label7.Location = new Point(440, 0x218);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0xa8, 12);
            this.label7.TabIndex = 7;
            this.label7.Text = "tt";
            this.label6.BackColor = Color.White;
            this.label6.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label6.Location = new Point(120, 0x218);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0xc0, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "tt";
            this.label5.BackColor = Color.White;
            this.label5.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label5.Location = new Point(0x30, 0x1a0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xc0, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Springfield, USA";
            this.label4.BackColor = Color.White;
            this.label4.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x30, 0x180);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0xc0, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "123 Any Street";
            this.labSSN.BackColor = Color.White;
            this.labSSN.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labSSN.Location = new Point(0x220, 360);
            this.labSSN.Name = "labSSN";
            this.labSSN.Size = new Size(0x20, 12);
            this.labSSN.TabIndex = 3;
            this.labSSN.Text = "0000";
            this.label2.BackColor = Color.White;
            this.label2.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x204, 360);
            this.label2.Name = "label2";
            this.label2.Size = new Size(20, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "XX";
            this.label1.BackColor = Color.White;
            this.label1.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(480, 360);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x20, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "XXX";
            this.labName.BackColor = Color.White;
            this.labName.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labName.Location = new Point(0x110, 360);
            this.labName.Name = "labName";
            this.labName.Size = new Size(0xc0, 12);
            this.labName.TabIndex = 0;
            this.labName.Text = "tt";
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x278, 0x28e);
            base.Controls.Add(this.panMain);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmW4";
            base.ShowInTaskbar = false;
            this.Text = "Fill Out W4 Tax Form";
            base.Load += new EventHandler(this.frmW4_Load);
            this.panel1.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            this.panMain.PerformLayout();
            base.ResumeLayout(false);
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public int Allowances;
            public float Additional;
            public bool DisabledForCompetition;
        }
    }
}

