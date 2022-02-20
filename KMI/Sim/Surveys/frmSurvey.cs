namespace KMI.Sim.Surveys
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmSurvey : Form
    {
        private CheckBox chkQuestion;
        private Button cmdCancel;
        private Button cmdHelp;
        private Button cmdOK;
        private Container components = null;
        private GroupBox grpBoxCost;
        private GroupBox grpBoxNumber;
        public static string HelpTopic = "Market Research";
        private Label labBaseCost;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label7;
        private Label labExecutionCosts;
        private Label labQuestions;
        private Label labRecruitingCosts;
        private Label labTotalCost;
        private Panel panel1;
        private float totalCost = 0f;
        public NumericUpDown updNumToSurvey;

        public frmSurvey()
        {
            this.InitializeComponent();
            if (!Survey.BillForSurveys)
            {
                this.grpBoxCost.Visible = false;
                this.grpBoxNumber.Visible = false;
                base.Width -= this.grpBoxCost.Width;
                this.cmdOK.Left -= this.grpBoxCost.Width / 2;
                this.cmdCancel.Left -= this.grpBoxCost.Width / 2;
                this.cmdHelp.Left -= this.grpBoxCost.Width / 2;
            }
            for (int i = 0; i < Survey.PossibleSurveyQuestions.Length; i++)
            {
                if (i > 0)
                {
                    CheckBox box = new CheckBox {
                        Size = this.chkQuestion.Size,
                        Left = this.chkQuestion.Left,
                        Top = (i * 20) + this.chkQuestion.Top
                    };
                    this.panel1.Controls.Add(box);
                    box.Text = Survey.PossibleSurveyQuestions[i].Question;
                    box.CheckedChanged += new EventHandler(this.chkQuestion_CheckedChanged);
                }
                else
                {
                    this.chkQuestion.Text = Survey.PossibleSurveyQuestions[i].Question;
                }
            }
            this.labQuestions.Text = "Questions to Ask " + Survey.SurveyableObjectName;
            this.grpBoxNumber.Text = "Number of " + Survey.SurveyableObjectName + " to Survey";
            this.UpdateCosts();
        }

        private void chkQuestion_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateCosts();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(HelpTopic);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList questions = new ArrayList();
                int index = 0;
                foreach (CheckBox box in this.panel1.Controls)
                {
                    if (box.Checked)
                    {
                        questions.Add(Survey.PossibleSurveyQuestions[index]);
                    }
                    index++;
                }
                if (questions.Count == 0)
                {
                    MessageBox.Show("You must ask at least one question in a survey. Please try again.", "No Question Checked");
                }
                else
                {
                    Survey survey = S.Adapter.ConductAndAddSurvey(S.Instance.ThisPlayerName, S.MainForm.CurrentEntityID, questions, (int) this.updNumToSurvey.Value, this.totalCost);
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
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
            this.panel1 = new Panel();
            this.chkQuestion = new CheckBox();
            this.grpBoxNumber = new GroupBox();
            this.updNumToSurvey = new NumericUpDown();
            this.label7 = new Label();
            this.grpBoxCost = new GroupBox();
            this.labTotalCost = new Label();
            this.labExecutionCosts = new Label();
            this.labRecruitingCosts = new Label();
            this.labBaseCost = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.cmdOK = new Button();
            this.cmdCancel = new Button();
            this.cmdHelp = new Button();
            this.labQuestions = new Label();
            this.panel1.SuspendLayout();
            this.grpBoxNumber.SuspendLayout();
            this.updNumToSurvey.BeginInit();
            this.grpBoxCost.SuspendLayout();
            base.SuspendLayout();
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chkQuestion);
            this.panel1.Location = new Point(0x10, 0x30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x180, 0x120);
            this.panel1.TabIndex = 1;
            this.chkQuestion.Location = new Point(0x10, 0x10);
            this.chkQuestion.Name = "chkQuestion";
            this.chkQuestion.Size = new Size(0x160, 0x10);
            this.chkQuestion.TabIndex = 0;
            this.chkQuestion.Text = "Question 1";
            this.chkQuestion.CheckedChanged += new EventHandler(this.chkQuestion_CheckedChanged);
            this.grpBoxNumber.Controls.Add(this.updNumToSurvey);
            this.grpBoxNumber.Controls.Add(this.label7);
            this.grpBoxNumber.Location = new Point(0x198, 40);
            this.grpBoxNumber.Name = "grpBoxNumber";
            this.grpBoxNumber.Size = new Size(0xd0, 0x58);
            this.grpBoxNumber.TabIndex = 2;
            this.grpBoxNumber.TabStop = false;
            this.grpBoxNumber.Text = "Number of XXX to Survey";
            int[] bits = new int[4];
            bits[0] = 0x19;
            this.updNumToSurvey.Increment = new decimal(bits);
            this.updNumToSurvey.Location = new Point(0x80, 0x20);
            bits = new int[4];
            bits[0] = 0x2710;
            this.updNumToSurvey.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.updNumToSurvey.Minimum = new decimal(bits);
            this.updNumToSurvey.Name = "updNumToSurvey";
            this.updNumToSurvey.Size = new Size(0x40, 20);
            this.updNumToSurvey.TabIndex = 1;
            this.updNumToSurvey.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 100;
            this.updNumToSurvey.Value = new decimal(bits);
            this.updNumToSurvey.ValueChanged += new EventHandler(this.updNumToSurvey_ValueChanged);
            this.label7.Location = new Point(0x10, 40);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x10);
            this.label7.TabIndex = 0;
            this.label7.Text = "Total Number";
            this.grpBoxCost.Controls.Add(this.labTotalCost);
            this.grpBoxCost.Controls.Add(this.labExecutionCosts);
            this.grpBoxCost.Controls.Add(this.labRecruitingCosts);
            this.grpBoxCost.Controls.Add(this.labBaseCost);
            this.grpBoxCost.Controls.Add(this.label5);
            this.grpBoxCost.Controls.Add(this.label4);
            this.grpBoxCost.Controls.Add(this.label3);
            this.grpBoxCost.Controls.Add(this.label2);
            this.grpBoxCost.Controls.Add(this.label1);
            this.grpBoxCost.Location = new Point(0x198, 0x98);
            this.grpBoxCost.Name = "grpBoxCost";
            this.grpBoxCost.Size = new Size(0xd0, 120);
            this.grpBoxCost.TabIndex = 3;
            this.grpBoxCost.TabStop = false;
            this.grpBoxCost.Text = "Estimated Cost";
            this.labTotalCost.Location = new Point(120, 0x60);
            this.labTotalCost.Name = "labTotalCost";
            this.labTotalCost.Size = new Size(0x48, 0x10);
            this.labTotalCost.TabIndex = 8;
            this.labTotalCost.Text = "XX";
            this.labTotalCost.TextAlign = ContentAlignment.TopRight;
            this.labExecutionCosts.Location = new Point(120, 0x48);
            this.labExecutionCosts.Name = "labExecutionCosts";
            this.labExecutionCosts.Size = new Size(0x48, 0x10);
            this.labExecutionCosts.TabIndex = 5;
            this.labExecutionCosts.Text = "XX";
            this.labExecutionCosts.TextAlign = ContentAlignment.TopRight;
            this.labRecruitingCosts.Location = new Point(120, 0x30);
            this.labRecruitingCosts.Name = "labRecruitingCosts";
            this.labRecruitingCosts.Size = new Size(0x48, 0x10);
            this.labRecruitingCosts.TabIndex = 3;
            this.labRecruitingCosts.Text = "XX";
            this.labRecruitingCosts.TextAlign = ContentAlignment.TopRight;
            this.labBaseCost.Location = new Point(120, 0x18);
            this.labBaseCost.Name = "labBaseCost";
            this.labBaseCost.Size = new Size(0x48, 0x10);
            this.labBaseCost.TabIndex = 1;
            this.labBaseCost.Text = "XX";
            this.labBaseCost.TextAlign = ContentAlignment.TopRight;
            this.label5.BorderStyle = BorderStyle.FixedSingle;
            this.label5.Location = new Point(8, 0x58);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0xb8, 1);
            this.label5.TabIndex = 6;
            this.label4.Location = new Point(8, 0x60);
            this.label4.Name = "label4";
            this.label4.Size = new Size(120, 0x10);
            this.label4.TabIndex = 7;
            this.label4.Text = "   Total Cost";
            this.label3.Location = new Point(8, 0x48);
            this.label3.Name = "label3";
            this.label3.Size = new Size(120, 0x10);
            this.label3.TabIndex = 4;
            this.label3.Text = "+ Execution Costs";
            this.label2.Location = new Point(8, 0x30);
            this.label2.Name = "label2";
            this.label2.Size = new Size(120, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "+ Recruiting Costs";
            this.label1.Location = new Point(8, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "   Base Cost";
            this.cmdOK.Location = new Point(0xb0, 360);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new Size(0x60, 0x18);
            this.cmdOK.TabIndex = 4;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
            this.cmdCancel.Location = new Point(0x128, 360);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(0x60, 0x18);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
            this.cmdHelp.Location = new Point(0x1a0, 360);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new Size(0x60, 0x18);
            this.cmdHelp.TabIndex = 6;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.Click += new EventHandler(this.cmdHelp_Click);
            this.labQuestions.Location = new Point(0x18, 0x18);
            this.labQuestions.Name = "labQuestions";
            this.labQuestions.Size = new Size(0xe0, 0x10);
            this.labQuestions.TabIndex = 0;
            this.labQuestions.Text = "Questions to Ask XXX";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x278, 0x18e);
            base.Controls.Add(this.labQuestions);
            base.Controls.Add(this.cmdHelp);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdOK);
            base.Controls.Add(this.grpBoxCost);
            base.Controls.Add(this.grpBoxNumber);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmSurvey";
            base.ShowInTaskbar = false;
            this.Text = "New Survey";
            this.panel1.ResumeLayout(false);
            this.grpBoxNumber.ResumeLayout(false);
            this.updNumToSurvey.EndInit();
            this.grpBoxCost.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void UpdateCosts()
        {
            int num = 0;
            foreach (CheckBox box in this.panel1.Controls)
            {
                if (box.Checked)
                {
                    num++;
                }
            }
            float amount = Survey.RecruitingCostPerPerson * ((float) this.updNumToSurvey.Value);
            float num3 = (Survey.ExecutionCostPerQuestionPerPerson * num) * ((float) this.updNumToSurvey.Value);
            this.totalCost = (Survey.BaseCost + amount) + num3;
            this.labBaseCost.Text = Utilities.FC(Survey.BaseCost, S.Instance.CurrencyConversion);
            this.labRecruitingCosts.Text = Utilities.FC(amount, S.Instance.CurrencyConversion);
            this.labExecutionCosts.Text = Utilities.FC(num3, S.Instance.CurrencyConversion);
            this.labTotalCost.Text = Utilities.FC(this.totalCost, S.Instance.CurrencyConversion);
        }

        private void updNumToSurvey_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateCosts();
        }
    }
}

