namespace KMI.Sim
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSetupWizard : Form
    {
        private Button btnBack;
        private Button btnFinish;
        private Button btnNext;
        private Button btnShow;
        private Container components;
        protected System.Type currentType;
        protected System.Type[] formsToShow;
        private Label labText;
        private Label labTitle;
        private Label lblStep0;
        private Panel panTitles;
        private PictureBox picImage;
        protected object[] stepArgs;
        protected object[] stepArgTypes;
        protected int stepIndex;
        protected string[] stepText;
        protected string[] stepTitles;

        public frmSetupWizard(System.Type[] forms, object[] args, string[] titles, string[] text)
        {
            int num;
            this.components = null;
            this.InitializeComponent();
            this.stepIndex = -1;
            this.currentType = null;
            this.formsToShow = forms;
            this.stepArgs = args;
            this.stepTitles = titles;
            this.stepText = text;
            this.stepArgTypes = new object[this.stepArgs.Length];
            int num2 = 0;
            foreach (object[] objArray2 in this.stepArgs)
            {
                ArrayList list = new ArrayList();
                num = 0;
                while (num < objArray2.Length)
                {
                    list.Add(objArray2[num].GetType());
                    num++;
                }
                this.stepArgTypes[num2++] = list.ToArray(typeof(System.Type));
            }
            this.panTitles.Controls.Clear();
            for (num = 0; num < this.stepTitles.Length; num++)
            {
                Label label = new Label {
                    ForeColor = this.labTitle.ForeColor,
                    Font = this.labTitle.Font,
                    Location = new Point(this.labTitle.Left, this.labTitle.Top + (num * 0x2d)),
                    Size = this.labTitle.Size,
                    Text = this.stepTitles[num]
                };
                this.panTitles.Controls.Add(label);
            }
            this.UpdateWizard();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.stepIndex--;
            this.UpdateWizard();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.stepIndex++;
            this.UpdateWizard();
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            if (((Form) this.formsToShow[this.stepIndex].GetConstructor((System.Type[]) this.stepArgTypes[this.stepIndex]).Invoke((object[]) this.stepArgs[this.stepIndex])).ShowDialog() != DialogResult.Cancel)
            {
                this.stepIndex++;
                this.UpdateWizard();
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
            this.btnBack = new Button();
            this.btnNext = new Button();
            this.btnFinish = new Button();
            this.picImage = new PictureBox();
            this.btnShow = new Button();
            this.lblStep0 = new Label();
            this.panTitles = new Panel();
            this.labTitle = new Label();
            this.labText = new Label();
            this.panTitles.SuspendLayout();
            base.SuspendLayout();
            this.btnBack.Location = new Point(200, 280);
            this.btnBack.Name = "btnBack";
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "&Back";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnNext.Location = new Point(0x120, 280);
            this.btnNext.Name = "btnNext";
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = "&Next";
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.btnFinish.Location = new Point(0x178, 280);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.TabIndex = 6;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.Click += new EventHandler(this.btnFinish_Click);
            this.picImage.Location = new Point(480, 0x10);
            this.picImage.Name = "picImage";
            this.picImage.Size = new Size(100, 0xd0);
            this.picImage.TabIndex = 4;
            this.picImage.TabStop = false;
            this.btnShow.Location = new Point(0xd8, 0xe0);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new Size(0xd0, 0x17);
            this.btnShow.TabIndex = 2;
            this.btnShow.Text = "&Setup";
            this.btnShow.Click += new EventHandler(this.btnShow_Click);
            this.lblStep0.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.lblStep0.Location = new Point(8, 0x10);
            this.lblStep0.Name = "lblStep0";
            this.lblStep0.Size = new Size(0x98, 0x17);
            this.lblStep0.TabIndex = 0;
            this.lblStep0.Text = "lblStep";
            this.panTitles.Controls.Add(this.labTitle);
            this.panTitles.Location = new Point(8, 0x10);
            this.panTitles.Name = "panTitles";
            this.panTitles.Size = new Size(0x98, 0x128);
            this.panTitles.TabIndex = 0;
            this.labTitle.Font = new Font("Times New Roman", 15.75f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labTitle.ForeColor = Color.DarkBlue;
            this.labTitle.Location = new Point(8, 40);
            this.labTitle.Name = "labTitle";
            this.labTitle.Size = new Size(0x88, 0x20);
            this.labTitle.TabIndex = 0;
            this.labTitle.Text = "Title";
            this.labText.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labText.Location = new Point(0xc0, 0x18);
            this.labText.Name = "labText";
            this.labText.Size = new Size(0x100, 0xb8);
            this.labText.TabIndex = 1;
            this.labText.Text = "Text";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x26e, 0x153);
            base.Controls.Add(this.labText);
            base.Controls.Add(this.panTitles);
            base.Controls.Add(this.btnShow);
            base.Controls.Add(this.picImage);
            base.Controls.Add(this.btnFinish);
            base.Controls.Add(this.btnNext);
            base.Controls.Add(this.btnBack);
            base.FormBorderStyle = FormBorderStyle.Fixed3D;
            base.Name = "SetupWizard";
            base.ShowInTaskbar = false;
            this.Text = "Setup Wizard";
            this.panTitles.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected void UpdateWizard()
        {
            this.labText.Text = this.stepText[this.stepIndex + 1].Replace(@"\n", "\n");
            if ((this.stepIndex > -1) && (this.stepIndex < this.panTitles.Controls.Count))
            {
                this.btnShow.Visible = true;
                this.btnShow.Text = "&Setup " + this.stepTitles[this.stepIndex];
            }
            else
            {
                this.btnShow.Visible = false;
            }
            this.btnBack.Enabled = this.stepIndex > -1;
            this.btnNext.Enabled = this.stepIndex < this.panTitles.Controls.Count;
            for (int i = 0; i < this.panTitles.Controls.Count; i++)
            {
                this.panTitles.Controls[i].ForeColor = Color.DarkBlue;
                if (i == this.stepIndex)
                {
                    this.panTitles.Controls[i].ForeColor = Color.Yellow;
                }
            }
        }
    }
}

