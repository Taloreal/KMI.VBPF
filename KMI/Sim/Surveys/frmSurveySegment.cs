namespace KMI.Sim.Surveys
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSurveySegment : Form
    {
        private Button btnApply;
        private Button btnBuyMailingList;
        private Button btnCancel;
        private Button btnClear;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboEntity;
        private ComboBox cboQuestion;
        private CheckBox chkAnswer;
        private Container components = null;
        protected long[] currentSegmentIDs;
        public static string HelpTopic = "Market Research Example";
        private Label label2;
        private Label label3;
        private Label labEntity;
        private Label labTotal;
        private Panel panel1;
        private PictureBox picCanvas;
        protected SurveyQuestion question;
        protected KMI.Sim.Surveys.Survey survey;

        public frmSurveySegment()
        {
            this.InitializeComponent();
            this.btnBuyMailingList.Visible = KMI.Sim.Surveys.Survey.ShowBuyMailingList;
            this.labTotal.Text = "Total " + KMI.Sim.Surveys.Survey.SurveyableObjectName + " in Survey";
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            int index = 0;
            bool[] includesAnswer = new bool[this.question.PossibleResponses.Length];
            foreach (CheckBox box in this.panel1.Controls)
            {
                includesAnswer[index] = box.Checked;
                index++;
            }
            if (this.question.MultiEntity)
            {
                this.survey.AddUpdateSegmenter(this.question, includesAnswer, this.cboEntity.SelectedIndex);
            }
            else
            {
                this.survey.AddUpdateSegmenter(this.question, includesAnswer);
            }
            this.picCanvas.Refresh();
        }

        private void btnBuyMailingList_Click(object sender, EventArgs e)
        {
            this.survey.BuyMailingListHook();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.survey.ClearSegmenters();
            this.picCanvas.Refresh();
            this.cboQuestion_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(HelpTopic);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.btnApply.PerformClick();
            base.Close();
        }

        private void cboEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboQuestion_SelectedIndexChanged(new object(), new EventArgs());
        }

        private void cboQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int num;
            for (num = 0; num < this.survey.SurveyQuestions.Count; num++)
            {
                this.question = (SurveyQuestion) this.survey.SurveyQuestions[num];
                if (this.question.Question == ((string) this.cboQuestion.Items[this.cboQuestion.SelectedIndex]))
                {
                    break;
                }
            }
            this.cboEntity.Visible = this.question.MultiEntity;
            this.labEntity.Visible = this.question.MultiEntity;
            this.panel1.BorderStyle = BorderStyle.None;
            for (num = this.panel1.Controls.Count - 1; num > 0; num--)
            {
                this.panel1.Controls.RemoveAt(num);
            }
            for (num = 0; num < this.question.PossibleResponses.Length; num++)
            {
                SurveySegmenter segmenter;
                if (this.question.MultiEntity)
                {
                    segmenter = this.survey.GetSegmenter(this.question, this.cboEntity.SelectedIndex);
                }
                else
                {
                    segmenter = this.survey.GetSegmenter(this.question);
                }
                if (num > 0)
                {
                    CheckBox box = new CheckBox {
                        Size = this.chkAnswer.Size,
                        Left = this.chkAnswer.Left,
                        Top = (num * 20) + this.chkAnswer.Top
                    };
                    this.panel1.Controls.Add(box);
                    box.Text = this.question.PossibleResponses[num];
                    if (segmenter != null)
                    {
                        box.Checked = segmenter.IncludesAnswer[num];
                    }
                    else
                    {
                        box.Checked = true;
                    }
                }
                else
                {
                    this.chkAnswer.Text = this.question.PossibleResponses[num];
                    if (segmenter != null)
                    {
                        this.chkAnswer.Checked = segmenter.IncludesAnswer[num];
                    }
                    else
                    {
                        this.chkAnswer.Checked = true;
                    }
                }
            }
            if (this.question.PossibleResponses.Length > 8)
            {
                this.panel1.BorderStyle = BorderStyle.Fixed3D;
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
            this.picCanvas = new PictureBox();
            this.labTotal = new Label();
            this.label2 = new Label();
            this.cboQuestion = new ComboBox();
            this.label3 = new Label();
            this.btnOK = new Button();
            this.btnApply = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.labEntity = new Label();
            this.cboEntity = new ComboBox();
            this.panel1 = new Panel();
            this.chkAnswer = new CheckBox();
            this.btnClear = new Button();
            this.btnBuyMailingList = new Button();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.picCanvas.BackColor = Color.Blue;
            this.picCanvas.BorderStyle = BorderStyle.Fixed3D;
            this.picCanvas.Location = new Point(0x18, 0x20);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new Size(0x110, 260);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            this.picCanvas.Paint += new PaintEventHandler(this.picCanvas_Paint);
            this.labTotal.Location = new Point(0x10, 8);
            this.labTotal.Name = "labTotal";
            this.labTotal.Size = new Size(0xc0, 0x10);
            this.labTotal.TabIndex = 0;
            this.labTotal.Text = "Total XXX in Survey";
            this.label2.Location = new Point(0x148, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xd8, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "Select by answer to question:";
            this.cboQuestion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboQuestion.Location = new Point(0x148, 0x20);
            this.cboQuestion.Name = "cboQuestion";
            this.cboQuestion.Size = new Size(0x120, 0x15);
            this.cboQuestion.TabIndex = 2;
            this.cboQuestion.SelectedIndexChanged += new EventHandler(this.cboQuestion_SelectedIndexChanged);
            this.label3.Location = new Point(0x148, 0x60);
            this.label3.Name = "label3";
            this.label3.Size = new Size(280, 0x10);
            this.label3.TabIndex = 5;
            this.label3.Text = "Select only customers who answered:";
            this.btnOK.Location = new Point(0x60, 0x158);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnApply.Location = new Point(0xd0, 0x158);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new Size(0x60, 0x18);
            this.btnApply.TabIndex = 9;
            this.btnApply.Text = "Apply";
            this.btnApply.Click += new EventHandler(this.btnApply_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(320, 0x158);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x1b0, 0x158);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 11;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.labEntity.Location = new Point(0x148, 0x40);
            this.labEntity.Name = "labEntity";
            this.labEntity.Size = new Size(0x80, 0x10);
            this.labEntity.TabIndex = 3;
            this.labEntity.Text = "Select by rating for:";
            this.cboEntity.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboEntity.Location = new Point(0x1c0, 0x40);
            this.cboEntity.Name = "cboEntity";
            this.cboEntity.Size = new Size(0xa8, 0x15);
            this.cboEntity.TabIndex = 4;
            this.cboEntity.SelectedIndexChanged += new EventHandler(this.cboEntity_SelectedIndexChanged);
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.chkAnswer);
            this.panel1.Location = new Point(320, 120);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x130, 0xa8);
            this.panel1.TabIndex = 6;
            this.chkAnswer.Location = new Point(12, 8);
            this.chkAnswer.Name = "chkAnswer";
            this.chkAnswer.Size = new Size(0x10c, 0x10);
            this.chkAnswer.TabIndex = 0;
            this.btnClear.Location = new Point(0xa8, 0x128);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new Size(0x80, 0x18);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.Click += new EventHandler(this.btnClear_Click);
            this.btnBuyMailingList.Location = new Point(0x18, 0x128);
            this.btnBuyMailingList.Name = "btnBuyMailingList";
            this.btnBuyMailingList.Size = new Size(0x80, 0x18);
            this.btnBuyMailingList.TabIndex = 12;
            this.btnBuyMailingList.Text = "Buy Mailing List";
            this.btnBuyMailingList.Click += new EventHandler(this.btnBuyMailingList_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x278, 0x176);
            base.Controls.Add(this.btnBuyMailingList);
            base.Controls.Add(this.btnClear);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.cboEntity);
            base.Controls.Add(this.labEntity);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnApply);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.cboQuestion);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.labTotal);
            base.Controls.Add(this.picCanvas);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmSurveySegment";
            base.ShowInTaskbar = false;
            this.Text = "Select Segment";
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(this.picCanvas.BackColor);
            this.currentSegmentIDs = this.survey.DrawSegments(g, this.picCanvas.ClientRectangle);
        }

        public KMI.Sim.Surveys.Survey Survey
        {
            set
            {
                this.survey = value;
                this.cboQuestion.Items.Clear();
                foreach (SurveyQuestion question in this.survey.SurveyQuestions)
                {
                    if (((question.ShortName != "live") && (question.ShortName != "work")) && (question.ShortName != "lastmovie"))
                    {
                        this.cboQuestion.Items.Add(question.Question);
                    }
                }
                this.cboQuestion.SelectedIndex = 0;
                foreach (string str in this.survey.EntityNames)
                {
                    this.cboEntity.Items.Add(str);
                }
                if (this.survey.EntityNames.Length > 0)
                {
                    this.cboEntity.SelectedIndex = 0;
                }
            }
        }
    }
}

