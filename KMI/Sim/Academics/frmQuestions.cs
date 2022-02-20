namespace KMI.Sim.Academics
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmQuestions : Form
    {
        private Button btnContinue;
        private Button btnSubmit;
        private Container components = null;
        private Label labScore;
        public Modes Mode = Modes.Quiz;
        private Panel panel1;
        private Panel panel3;
        private Panel panQuestions;
        private Panel panScore;
        private Question[] questions;

        public frmQuestions(Modes mode, Question[] questions)
        {
            this.InitializeComponent();
            this.Mode = mode;
            this.questions = questions;
            int num = 0;
            int num2 = 0;
            foreach (Question question in questions)
            {
                if (this.Mode == Modes.LevelEndTest)
                {
                    question.Answer = null;
                }
                QuestionControl control = new QuestionControl {
                    Question = question
                };
                if ((num++ % 2) == 0)
                {
                    control.BackColor = Color.FromArgb(240, 240, 240);
                }
                else
                {
                    control.BackColor = Color.FromArgb(230, 230, 230);
                }
                control.Top = num2;
                num2 += control.Height;
                this.panQuestions.Controls.Add(control);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            int num;
            QuestionControl control;
            if ((this.Mode == Modes.LevelEndTest) || (this.Mode == Modes.TestReview))
            {
                float num2 = 0f;
                for (num = 0; num < this.questions.Length; num++)
                {
                    control = (QuestionControl) this.panQuestions.Controls[num];
                    this.questions[num].Answer = control.txtAnswer.Text;
                    control.Question = this.questions[num];
                    control.txtAnswer.Enabled = false;
                    if (this.questions[num].Correct)
                    {
                        num2++;
                    }
                }
                this.labScore.Text = Utilities.FP(num2 / ((float) this.questions.Length));
                this.btnSubmit.Enabled = false;
                this.btnContinue.Enabled = true;
                this.panScore.Visible = true;
            }
            else
            {
                bool flag4 = true;
                for (num = 0; num < this.questions.Length; num++)
                {
                    control = (QuestionControl) this.panQuestions.Controls[num];
                    this.questions[num].Answer = control.txtAnswer.Text;
                    control.Question = this.questions[num];
                    flag4 = this.questions[num].Correct & flag4;
                }
                this.btnSubmit.Enabled = !flag4;
                this.btnContinue.Enabled = flag4;
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void frmQuestions_Closing(object sender, CancelEventArgs e)
        {
            if (!this.btnContinue.Enabled)
            {
                e.Cancel = true;
            }
            if (base.Owner != null)
            {
                ((frmPage) base.Owner).okToClose = true;
                base.Owner.Close();
            }
        }

        private void frmQuestions_Load(object sender, EventArgs e)
        {
            if (this.Mode == Modes.TestReview)
            {
                this.btnSubmit.PerformClick();
            }
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.btnContinue = new Button();
            this.btnSubmit = new Button();
            this.panQuestions = new Panel();
            this.panScore = new Panel();
            this.panel3 = new Panel();
            this.labScore = new Label();
            this.panel1.SuspendLayout();
            this.panScore.SuspendLayout();
            this.panel3.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.btnContinue);
            this.panel1.Controls.Add(this.btnSubmit);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x12e);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x1d8, 40);
            this.panel1.TabIndex = 0;
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new Point(0x178, 8);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new Size(0x58, 0x18);
            this.btnContinue.TabIndex = 1;
            this.btnContinue.Text = "Continue";
            this.btnContinue.Click += new EventHandler(this.button1_Click);
            this.btnSubmit.Location = new Point(0x100, 8);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new Size(0x58, 0x18);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.Click += new EventHandler(this.btnSubmit_Click);
            this.panQuestions.AutoScroll = true;
            this.panQuestions.Dock = DockStyle.Fill;
            this.panQuestions.Location = new Point(0, 0x2a);
            this.panQuestions.Name = "panQuestions";
            this.panQuestions.Size = new Size(0x1d8, 260);
            this.panQuestions.TabIndex = 1;
            this.panScore.Controls.Add(this.panel3);
            this.panScore.Dock = DockStyle.Top;
            this.panScore.Location = new Point(0, 0);
            this.panScore.Name = "panScore";
            this.panScore.Size = new Size(0x1d8, 0x2a);
            this.panScore.TabIndex = 2;
            this.panScore.Visible = false;
            this.panel3.Controls.Add(this.labScore);
            this.panel3.Dock = DockStyle.Right;
            this.panel3.Location = new Point(0x158, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x80, 0x2a);
            this.panel3.TabIndex = 0;
            this.labScore.Font = new Font("Microsoft Sans Serif", 24f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labScore.Location = new Point(8, 8);
            this.labScore.Name = "labScore";
            this.labScore.Size = new Size(0x68, 0x20);
            this.labScore.TabIndex = 0;
            this.labScore.Text = "100%";
            this.labScore.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1d8, 0x156);
            base.Controls.Add(this.panQuestions);
            base.Controls.Add(this.panScore);
            base.Controls.Add(this.panel1);
            base.MaximizeBox = false;
            this.MaximumSize = new Size(0x1e8, 0x7d0);
            base.MinimizeBox = false;
            this.MinimumSize = new Size(0x1e8, 100);
            base.Name = "frmQuestions";
            base.ShowInTaskbar = false;
            this.Text = "Questions";
            base.Closing += new CancelEventHandler(this.frmQuestions_Closing);
            base.Load += new EventHandler(this.frmQuestions_Load);
            this.panel1.ResumeLayout(false);
            this.panScore.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        public enum Modes
        {
            Quiz,
            LevelEndTest,
            TestReview
        }
    }
}

