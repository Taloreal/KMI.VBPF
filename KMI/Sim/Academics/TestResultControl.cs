namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class TestResultControl : UserControl
    {
        private Container components;
        private AcademicGod g;
        private int index;
        private Label labDetails;
        private Label labResult;

        public TestResultControl()
        {
            this.components = null;
            this.InitializeComponent();
        }

        public TestResultControl(int index, float score, AcademicGod g)
        {
            this.components = null;
            this.InitializeComponent();
            this.index = index;
            this.g = g;
            object[] args = new object[] { index + 1, Utilities.FP(score) };
            this.labResult.Text = S.Resources.GetString("Level {0} - {1}", args);
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
            this.labResult = new Label();
            this.labDetails = new Label();
            base.SuspendLayout();
            this.labResult.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labResult.Location = new Point(0x10, 8);
            this.labResult.Name = "labResult";
            this.labResult.Size = new Size(120, 0x10);
            this.labResult.TabIndex = 0;
            this.labResult.Text = "label1";
            this.labResult.TextAlign = ContentAlignment.BottomLeft;
            this.labDetails.Cursor = Cursors.Hand;
            this.labDetails.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.labDetails.ForeColor = Color.FromArgb(0, 0, 0xc0);
            this.labDetails.Location = new Point(0x98, 6);
            this.labDetails.Name = "labDetails";
            this.labDetails.Size = new Size(0x40, 0x10);
            this.labDetails.TabIndex = 1;
            this.labDetails.Text = "Details";
            this.labDetails.TextAlign = ContentAlignment.BottomLeft;
            this.labDetails.Click += new EventHandler(this.labDetails_Click);
            base.Controls.Add(this.labDetails);
            base.Controls.Add(this.labResult);
            base.Name = "TestResultControl";
            base.Size = new Size(0x108, 0x20);
            base.ResumeLayout(false);
        }

        private void labDetails_Click(object sender, EventArgs e)
        {
            new frmQuestions(frmQuestions.Modes.TestReview, this.g.FindAllAskedQuestions(this.index)).ShowDialog();
        }
    }
}

