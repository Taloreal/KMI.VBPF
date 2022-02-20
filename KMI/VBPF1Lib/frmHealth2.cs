namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class frmHealth2 : frmDrawnReport
    {
        private static Brush brush = new SolidBrush(Color.Black);
        private static Font font = new Font("Arial", 12f);
        private static Font fontB = new Font("Arial", 12f);
        public float[] healthFactors;

        public frmHealth2()
        {
            this.InitializeComponent();
            A.MainForm.NewDay += new EventHandler(this.NewDayHandler);
            this.Text = A.Resources.GetString("Health");
        }

        protected override void DrawReportVirtual(Graphics g)
        {
            int index = 0;
            float num2 = 0f;
            foreach (string str in AppConstants.HealthFactorNames)
            {
                if (index < this.healthFactors.Length)
                {
                    float score = this.healthFactors[index];
                    this.DrawScore(g, A.Resources.GetString(str), 0x23 + (index * 20), score);
                    num2 += score;
                    index++;
                }
            }
        }

        protected void DrawScore(Graphics g, string name, int y, float score)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawString(A.Resources.GetString("Worst"), fontB, frmHealth2.brush, (float) 76f, (float) 0f);
            g.DrawString(A.Resources.GetString("Best"), fontB, frmHealth2.brush, (float) 220f, (float) 0f);
            Brush brush = new SolidBrush(Color.Green);
            if (score < 0.1)
            {
                brush = new SolidBrush(Color.Red);
            }
            else if (score < 0.66)
            {
                brush = new SolidBrush(Color.Yellow);
            }
            g.DrawString(name, font, frmHealth2.brush, 0f, (float) y);
            g.FillRectangle(brush, 100f, (float) y, 5f + (score * 130f), 14f);
        }

        protected override void frmReport_Closed(object sender, EventArgs e)
        {
            base.frmReport_Closed(sender, e);
            A.MainForm.NewDay -= new EventHandler(this.NewDayHandler);
        }

        protected override void GetDataVirtual()
        {
            this.healthFactors = A.Adapter.GetHealth(A.MainForm.CurrentEntityID);
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.pnlBottom.Location = new Point(0, 0x9e);
            base.pnlBottom.Name = "pnlBottom";
            base.pnlBottom.Size = new Size(0x12a, 40);
            base.picReport.Name = "picReport";
            base.picReport.Size = new Size(0x108, 0x84);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x12a, 0xc6);
            base.Name = "frmHealth2";
            base.ResumeLayout(false);
        }

        protected void NewDayHandler(object sender, EventArgs e)
        {
            if (base.GetData())
            {
                base.picReport.Refresh();
            }
        }
    }
}

