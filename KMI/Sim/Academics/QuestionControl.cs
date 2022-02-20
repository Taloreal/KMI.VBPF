namespace KMI.Sim.Academics
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class QuestionControl : UserControl
    {
        private IContainer components;
        private ImageList imlRightWrong;
        private Label labQuestion;
        private Label labRightWrong;
        protected KMI.Sim.Academics.Question question;
        public TextBox txtAnswer;

        public QuestionControl()
        {
            this.InitializeComponent();
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
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(QuestionControl));
            this.txtAnswer = new TextBox();
            this.labRightWrong = new Label();
            this.labQuestion = new Label();
            this.imlRightWrong = new ImageList(this.components);
            base.SuspendLayout();
            this.txtAnswer.Location = new Point(8, 80);
            this.txtAnswer.Name = "txtAnswer";
            this.txtAnswer.Size = new Size(240, 20);
            this.txtAnswer.TabIndex = 1;
            this.txtAnswer.Text = "";
            this.txtAnswer.TextAlign = HorizontalAlignment.Right;
            this.labRightWrong.Location = new Point(280, 80);
            this.labRightWrong.Name = "labRightWrong";
            this.labRightWrong.Size = new Size(0x18, 0x18);
            this.labRightWrong.TabIndex = 2;
            this.labQuestion.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labQuestion.Location = new Point(8, 8);
            this.labQuestion.Name = "labQuestion";
            this.labQuestion.Size = new Size(0x1c0, 0x40);
            this.labQuestion.TabIndex = 3;
            this.labQuestion.Text = "label1";
            this.imlRightWrong.ColorDepth = ColorDepth.Depth32Bit;
            this.imlRightWrong.ImageSize = new Size(0x10, 14);
            this.imlRightWrong.ImageStream = (ImageListStreamer) manager.GetObject("imlRightWrong.ImageStream");
            this.imlRightWrong.TransparentColor = Color.Transparent;
            base.Controls.Add(this.labQuestion);
            base.Controls.Add(this.labRightWrong);
            base.Controls.Add(this.txtAnswer);
            base.Name = "QuestionControl";
            base.Size = new Size(0x1c8, 0x70);
            base.ResumeLayout(false);
        }

        public KMI.Sim.Academics.Question Question
        {
            set
            {
                this.labQuestion.Text = value.Text;
                this.txtAnswer.Text = value.Answer;
                if (value.Answer != null)
                {
                    if (value.Correct)
                    {
                        this.labRightWrong.Image = this.imlRightWrong.Images[0];
                        this.txtAnswer.Enabled = false;
                    }
                    else
                    {
                        this.labRightWrong.Image = this.imlRightWrong.Images[1];
                    }
                }
                this.txtAnswer.Select(0, 0);
            }
        }
    }
}

