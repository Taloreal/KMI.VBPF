namespace KMI.Sim.Surveys
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSurveyResults : Form
    {
        private Button btnClose;
        private Button btnHelp;
        protected Button btnPrint;
        protected Button btnSegment;
        private ComboBox cboQualifier;
        protected ComboBox cboQuestion;
        private ComboBox cboSurvey;
        private Container components;
        public static string HelpTopic = "Market Research";
        protected KMIGraph kmiGraph1;
        private Label label1;
        private Label label2;
        private Label labQualifyingName;
        protected bool loaded;
        private Panel panBottom;
        private Panel panel4;
        protected Panel panMain;
        private Panel panTop;
        protected SurveyQuestion question;
        protected bool segmented;
        protected frmSurveySegment segmenterForm;
        protected bool showPercentagesForHistograms;
        protected Survey survey;
        protected ArrayList surveys;

        public frmSurveyResults()
        {
            this.components = null;
            this.showPercentagesForHistograms = true;
            this.InitializeComponent();
        }

        public frmSurveyResults(string playerName, string qualifyingQuestionResponse)
        {
            this.components = null;
            this.showPercentagesForHistograms = true;
            this.InitializeComponent();
            if (Survey.QualifyingQuestionShortName != null)
            {
                this.labQualifyingName.Visible = true;
                this.labQualifyingName.Text = Survey.QualifyingQuestionShortName + ":";
                this.cboQualifier.Visible = true;
                this.cboQualifier.Items.Add("{All}");
                this.cboQualifier.Items.AddRange(Survey.GetSurveyQuestionByName(Survey.QualifyingQuestionShortName).PossibleResponses);
                if (qualifyingQuestionResponse == null)
                {
                    this.cboQualifier.SelectedIndex = 0;
                }
                else
                {
                    this.cboQualifier.SelectedIndex = this.cboQualifier.FindString(qualifyingQuestionResponse);
                }
            }
            this.surveys = S.Adapter.getSurveys(playerName);
            this.kmiGraph1.Dock = DockStyle.Fill;
            foreach (Survey survey in this.surveys)
            {
                this.cboSurvey.Items.Add(survey.Date.ToShortDateString());
                survey.ClearSegmenters();
            }
            this.cboSurvey.SelectedIndex = this.cboSurvey.Items.Count - 1;
            if (this.surveys.Count == 0)
            {
                this.btnSegment.Enabled = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(HelpTopic);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.kmiGraph1.PrintGraph();
        }

        private void btnSegment_Click(object sender, EventArgs e)
        {
            frmSurveySegment segmenterForm;
            if (this.SegmenterForm == null)
            {
                segmenterForm = new frmSurveySegment();
            }
            else
            {
                segmenterForm = this.SegmenterForm;
            }
            segmenterForm.Survey = this.survey;
            segmenterForm.ShowDialog();
            this.segmented = this.survey.Segmented;
            this.UpdateGraph();
        }

        private void cboQualifier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboSurvey.SelectedIndex > -1)
            {
                this.UpdateGraph();
            }
        }

        protected virtual void cboQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.question = (SurveyQuestion) this.survey.SurveyQuestions[this.cboQuestion.SelectedIndex];
            this.UpdateGraph();
        }

        private void cboSurvey_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.survey = (Survey) this.surveys[this.cboSurvey.SelectedIndex];
            this.cboQuestion.Items.Clear();
            foreach (SurveyQuestion question in this.survey.SurveyQuestions)
            {
                this.cboQuestion.Items.Add(question.Question);
            }
            this.cboQuestion.SelectedIndex = 0;
            this.cboQuestion_SelectedIndexChanged(new object(), new EventArgs());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSurveyResults_Load(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.kmiGraph1 = new KMIGraph();
            this.btnClose = new Button();
            this.btnHelp = new Button();
            this.label1 = new Label();
            this.cboSurvey = new ComboBox();
            this.label2 = new Label();
            this.btnPrint = new Button();
            this.cboQuestion = new ComboBox();
            this.btnSegment = new Button();
            this.panTop = new Panel();
            this.labQualifyingName = new Label();
            this.cboQualifier = new ComboBox();
            this.panBottom = new Panel();
            this.panel4 = new Panel();
            this.panMain = new Panel();
            this.panTop.SuspendLayout();
            this.panBottom.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panMain.SuspendLayout();
            base.SuspendLayout();
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 9f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.BackColor = Color.White;
            this.kmiGraph1.Data = null;
            this.kmiGraph1.DataPointLabelFontSize = 9f;
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.DockPadding.All = 0x10;
            this.kmiGraph1.GraphType = 1;
            this.kmiGraph1.GridLinesX = false;
            this.kmiGraph1.GridLinesY = false;
            this.kmiGraph1.Legend = true;
            this.kmiGraph1.LegendFontSize = 9f;
            this.kmiGraph1.LineWidth = 3;
            this.kmiGraph1.Location = new Point(80, 120);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0x98, 0x58);
            this.kmiGraph1.TabIndex = 0;
            this.kmiGraph1.Title = null;
            this.kmiGraph1.TitleFontSize = 18f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = null;
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Location = new Point(0x10, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x60, 0x18);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnHelp.Location = new Point(0x10, 40);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label1.Location = new Point(0x10, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x70, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Survey Taken:";
            this.cboSurvey.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSurvey.Location = new Point(0x10, 0x18);
            this.cboSurvey.Name = "cboSurvey";
            this.cboSurvey.Size = new Size(0x70, 0x15);
            this.cboSurvey.TabIndex = 1;
            this.cboSurvey.SelectedIndexChanged += new EventHandler(this.cboSurvey_SelectedIndexChanged);
            this.label2.Location = new Point(0x90, 8);
            this.label2.Name = "label2";
            this.label2.Size = new Size(120, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "Question:";
            this.btnPrint.Location = new Point(0x148, 0x18);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x30, 0x20);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.cboQuestion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboQuestion.Location = new Point(0x90, 0x18);
            this.cboQuestion.Name = "cboQuestion";
            this.cboQuestion.Size = new Size(0xf8, 0x15);
            this.cboQuestion.TabIndex = 3;
            this.cboQuestion.SelectedIndexChanged += new EventHandler(this.cboQuestion_SelectedIndexChanged);
            this.btnSegment.Location = new Point(0x20, 0x10);
            this.btnSegment.Name = "btnSegment";
            this.btnSegment.Size = new Size(0xe8, 40);
            this.btnSegment.TabIndex = 0;
            this.btnSegment.Text = "Segment";
            this.btnSegment.Click += new EventHandler(this.btnSegment_Click);
            this.panTop.BorderStyle = BorderStyle.FixedSingle;
            this.panTop.Controls.Add(this.labQualifyingName);
            this.panTop.Controls.Add(this.cboQualifier);
            this.panTop.Controls.Add(this.cboSurvey);
            this.panTop.Controls.Add(this.label1);
            this.panTop.Controls.Add(this.label2);
            this.panTop.Controls.Add(this.cboQuestion);
            this.panTop.Dock = DockStyle.Top;
            this.panTop.Location = new Point(0, 0);
            this.panTop.Name = "panTop";
            this.panTop.Size = new Size(760, 0x38);
            this.panTop.TabIndex = 0;
            this.labQualifyingName.Location = new Point(0x198, 9);
            this.labQualifyingName.Name = "labQualifyingName";
            this.labQualifyingName.Size = new Size(120, 0x10);
            this.labQualifyingName.TabIndex = 4;
            this.labQualifyingName.Text = "Qualifying Name:";
            this.labQualifyingName.Visible = false;
            this.cboQualifier.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboQualifier.Location = new Point(0x198, 0x19);
            this.cboQualifier.Name = "cboQualifier";
            this.cboQualifier.Size = new Size(0x88, 0x15);
            this.cboQualifier.TabIndex = 5;
            this.cboQualifier.Visible = false;
            this.cboQualifier.SelectedIndexChanged += new EventHandler(this.cboQualifier_SelectedIndexChanged);
            this.panBottom.BorderStyle = BorderStyle.FixedSingle;
            this.panBottom.Controls.Add(this.panel4);
            this.panBottom.Controls.Add(this.btnSegment);
            this.panBottom.Controls.Add(this.btnPrint);
            this.panBottom.Dock = DockStyle.Bottom;
            this.panBottom.Location = new Point(0, 0x1bc);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new Size(760, 0x48);
            this.panBottom.TabIndex = 2;
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnHelp);
            this.panel4.Dock = DockStyle.Right;
            this.panel4.Location = new Point(0x27e, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(120, 70);
            this.panel4.TabIndex = 1;
            this.panMain.Controls.Add(this.kmiGraph1);
            this.panMain.Dock = DockStyle.Fill;
            this.panMain.Location = new Point(0, 0x38);
            this.panMain.Name = "panMain";
            this.panMain.Size = new Size(760, 0x184);
            this.panMain.TabIndex = 1;
            this.panMain.Resize += new EventHandler(this.panMain_Resize);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(760, 0x204);
            base.Controls.Add(this.panMain);
            base.Controls.Add(this.panBottom);
            base.Controls.Add(this.panTop);
            this.MinimumSize = new Size(0x238, 0x148);
            base.Name = "frmSurveyResults";
            base.ShowInTaskbar = false;
            this.Text = "Survey Results";
            base.Load += new EventHandler(this.frmSurveyResults_Load);
            this.panTop.ResumeLayout(false);
            this.panBottom.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void panMain_Resize(object sender, EventArgs e)
        {
        }

        protected void UpdateGraph()
        {
            object[,] objArray;
            int num;
            int num2;
            this.kmiGraph1.Title = this.question.Question;
            this.kmiGraph1.ShowPercentagesForHistograms = this.ShowPercentagesForHistograms;
            this.kmiGraph1.YAxisTitle = "# of " + Survey.SurveyableObjectName;
            if (this.kmiGraph1.ShowPercentagesForHistograms)
            {
                this.kmiGraph1.YAxisTitle = this.kmiGraph1.YAxisTitle.Replace("#", "%");
            }
            this.kmiGraph1.GraphType = 2;
            this.kmiGraph1.Legend = true;
            if (this.question.MultiEntity)
            {
                objArray = new object[this.survey.EntityNames.Length + 1, this.question.PossibleResponses.Length + 1];
                num = 0;
                foreach (string str in this.survey.EntityNames)
                {
                    objArray[++num, 0] = str;
                }
                if (this.segmented)
                {
                    this.kmiGraph1.YAxisTitle = "# of Custs (in Segment)";
                }
            }
            else if (this.segmented)
            {
                if (Survey.ShowAllSurveyedWhenSegmented)
                {
                    objArray = new object[3, this.question.PossibleResponses.Length + 1];
                    objArray[1, 0] = "All Surveyed";
                    objArray[2, 0] = "In Segment";
                    this.kmiGraph1.GraphType = 3;
                }
                else
                {
                    objArray = new object[2, this.question.PossibleResponses.Length + 1];
                    objArray[1, 0] = "In Segment";
                }
            }
            else
            {
                objArray = new object[2, this.question.PossibleResponses.Length + 1];
                this.kmiGraph1.Legend = false;
            }
            for (num = 1; num <= this.question.PossibleResponses.Length; num++)
            {
                objArray[0, num] = this.question.PossibleResponses[num - 1];
            }
            num = 1;
            while (num <= objArray.GetUpperBound(0))
            {
                num2 = 1;
                while (num2 <= objArray.GetUpperBound(1))
                {
                    objArray[num, num2] = 0;
                    num2++;
                }
                num++;
            }
            foreach (SurveyResponse response in this.survey.Responses)
            {
                if ((Survey.QualifyingQuestionShortName != null) && (this.cboQualifier.SelectedIndex > 0))
                {
                    num = 0;
                    while (num < this.survey.SurveyQuestions.Count)
                    {
                        if (((SurveyQuestion) this.survey.SurveyQuestions[num]).ShortName == Survey.QualifyingQuestionShortName)
                        {
                            break;
                        }
                        num++;
                    }
                    int[] numArray2 = (int[]) response.Answers[num];
                    if (numArray2[0] != (this.cboQualifier.SelectedIndex - 1))
                    {
                        continue;
                    }
                }
                int[] numArray = (int[]) response.Answers[this.cboQuestion.SelectedIndex];
                if (this.question.MultiEntity)
                {
                    for (num2 = 1; num2 <= this.survey.EntityNames.Length; num2++)
                    {
                        num = numArray[num2 - 1] + 1;
                        if (this.segmented)
                        {
                            if (this.survey.InAllSegments(response))
                            {
                                objArray[num2, num] = ((int) objArray[num2, num]) + 1;
                            }
                        }
                        else
                        {
                            objArray[num2, num] = ((int) objArray[num2, num]) + 1;
                        }
                    }
                }
                else
                {
                    num = numArray[0] + 1;
                    if (Survey.ShowAllSurveyedWhenSegmented)
                    {
                        objArray[1, num] = ((int) objArray[1, num]) + 1;
                        if (this.segmented && this.survey.InAllSegments(response))
                        {
                            objArray[2, num] = ((int) objArray[2, num]) + 1;
                        }
                    }
                    else if (this.survey.InAllSegments(response))
                    {
                        objArray[1, num] = ((int) objArray[1, num]) + 1;
                    }
                }
            }
            this.kmiGraph1.Draw(objArray);
        }

        public frmSurveySegment SegmenterForm
        {
            get
            {
                return this.segmenterForm;
            }
            set
            {
                this.segmenterForm = value;
            }
        }

        public bool ShowPercentagesForHistograms
        {
            get
            {
                return this.showPercentagesForHistograms;
            }
            set
            {
                this.showPercentagesForHistograms = value;
            }
        }
    }
}

