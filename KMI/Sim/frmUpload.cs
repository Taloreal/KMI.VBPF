namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;

    public class frmUpload : Form
    {
        private Button btnCancel;
        private Button btnUpload;
        private IContainer components;
        private Label label1;
        private Label label2;
        private Label labMsg;
        private float score;
        private System.Windows.Forms.Timer timer1;
        private TextBox txtTeamCode;

        public frmUpload()
        {
            this.InitializeComponent();
            this.score = S.Adapter.getHumanScore(Journal.ScoreSeriesName);
            this.Cursor = Cursors.WaitCursor;
            this.labMsg.Text = "Checking for Internet connection ...";
            this.btnUpload.Enabled = false;
            this.timer1.Interval = 0x3e8;
            this.timer1.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (this.txtTeamCode.Text == "")
            {
                MessageBox.Show("You must enter a team code. Please try again.");
            }
            else
            {
                WebRequest r = WebRequest.Create("http://vbc.knowledgematters.com/cgi-bin/vbccybercgi20.exe");
                string postString = this.GetPostString();
                r.Method = "POST";
                r.ContentType = "application/x-www-form-urlencoded";
                r.ContentLength = postString.Length;
                StreamWriter writer = new StreamWriter(r.GetRequestStream());
                writer.Write(postString);
                writer.Close();
                string str2 = Utilities.GetWebPage(r, S.Instance.UserAdminSettings.ProxyAddress, S.Instance.UserAdminSettings.ProxyBypassList);
                if (str2 == "")
                {
                    MessageBox.Show("Could not upload your score. Please recheck your connection to the Internet and click Upload on the File menu to upload your score.");
                    base.Close();
                }
                else
                {
                    if (str2.IndexOf("VBCSuccess") == -1)
                    {
                        string str3 = "";
                        int index = str2.IndexOf("VBCFail");
                        if (index > -1)
                        {
                            str3 = str2.Substring(index + 7, str2.Length - (index + 7));
                        }
                        MessageBox.Show("The upload failed. " + str3);
                    }
                    else
                    {
                        MessageBox.Show("Your upload succeeded! Go to www.KnowledgeMatters.com to check out your results!");
                    }
                    base.Close();
                }
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

        private string GetPostString()
        {
            string str = S.State.GUID.ToString().ToLower();
            str = str.Substring(str.Length - 12, 12);
            object[] objArray1 = new object[] { this.txtTeamCode.Text, "|", str, "|0|", this.score, "|", S.Settings.StudentOrg };
            return string.Concat(objArray1);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.btnUpload = new Button();
            this.btnCancel = new Button();
            this.labMsg = new Label();
            this.label1 = new Label();
            this.txtTeamCode = new TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label2 = new Label();
            base.SuspendLayout();
            this.btnUpload.Location = new Point(0x40, 0xb0);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new Size(0x60, 0x18);
            this.btnUpload.TabIndex = 3;
            this.btnUpload.Text = "Upload";
            this.btnUpload.Click += new EventHandler(this.btnUpload_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xd0, 0xb0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.labMsg.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labMsg.Location = new Point(0x20, 0x18);
            this.labMsg.Name = "labMsg";
            this.labMsg.Size = new Size(0x138, 40);
            this.labMsg.TabIndex = 0;
            this.labMsg.TextAlign = ContentAlignment.MiddleCenter;
            this.label1.Location = new Point(120, 80);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "Team Code:";
            this.txtTeamCode.Location = new Point(120, 0x60);
            this.txtTeamCode.Name = "txtTeamCode";
            this.txtTeamCode.Size = new Size(120, 20);
            this.txtTeamCode.TabIndex = 2;
            this.txtTeamCode.Text = "";
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.label2.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.label2.ForeColor = Color.Blue;
            this.label2.Location = new Point(0x100, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 40);
            this.label2.TabIndex = 5;
            this.label2.Text = "Upload Not Working? Click here for alternative method.";
            this.label2.Click += new EventHandler(this.label2_Click);
            base.AcceptButton = this.btnUpload;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x170, 0xde);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.txtTeamCode);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.labMsg);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnUpload);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmUpload";
            base.ShowInTaskbar = false;
            this.Text = "Upload Your Score!";
            base.ResumeLayout(false);
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (this.txtTeamCode.Text == "")
            {
                MessageBox.Show("Please enter your Team Code then try again.", Application.ProductName);
            }
            else
            {
                new frmUploadAlternative(this.GetPostString()).ShowDialog(this);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.Cursor = Cursors.WaitCursor;
            string str = Utilities.GetWebPage(WebRequest.Create("http://vbc.knowledgematters.com/vbccommon/vbctestpage.htm"), S.Instance.UserAdminSettings.ProxyAddress, S.Instance.UserAdminSettings.ProxyBypassList);
            this.Cursor = Cursors.Default;
            if (str == "")
            {
                MessageBox.Show("Could not connect to the Internet. Please connect to the Internet or click the blue link to try an alternative upload method.", "No Internet Connection");
            }
            else
            {
                this.btnUpload.Enabled = true;
            }
            this.labMsg.Text = "Your score is " + Utilities.FC(this.score, S.Instance.CurrencyConversion) + "!";
        }
    }
}

