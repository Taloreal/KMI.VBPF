namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmTestResults : Form
    {
        private Button btnClose;
        private Container components = null;
        private AcademicGod g;
        private Label labAverage;
        private Panel panResults;

        public frmTestResults()
        {
            this.InitializeComponent();
            this.g = S.Adapter.GetAcademicGod();
            float num = 0f;
            for (int i = 0; i < this.g.AcademicLevel; i++)
            {
                float score = this.g.GradeForLevel(i);
                num += score;
                TestResultControl control = new TestResultControl(i, score, this.g) {
                    Top = (i + 1) * base.Height,
                    Left = 0x18
                };
                this.panResults.Controls.Add(control);
            }
            if (this.g.AcademicLevel > 0)
            {
                object[] args = new object[] { Utilities.FP(num / ((float) this.g.AcademicLevel)) };
                this.labAverage.Text = S.Resources.GetString("Average: {0}", args);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void InitializeComponent()
        {
            this.panResults = new Panel();
            this.btnClose = new Button();
            this.labAverage = new Label();
            base.SuspendLayout();
            this.panResults.AutoScroll = true;
            this.panResults.Location = new Point(0, 0);
            this.panResults.Name = "panResults";
            this.panResults.Size = new Size(360, 0xb0);
            this.panResults.TabIndex = 0;
            this.btnClose.Location = new Point(0x100, 0xb8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(80, 0x18);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.labAverage.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labAverage.Location = new Point(0x18, 0xc0);
            this.labAverage.Name = "labAverage";
            this.labAverage.Size = new Size(0x90, 0x18);
            this.labAverage.TabIndex = 2;
            this.labAverage.Text = "No test results yet.";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x16a, 0xe0);
            base.Controls.Add(this.labAverage);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.panResults);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmTestResults";
            this.Text = "Test Results";
            base.ResumeLayout(false);
        }
    }
}

