namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAutoGrader : frmDrawnReport
    {
        public static float ExtraPointsPerEntity = 0.05f;
        private string Grades = "FFFFFFDCBAA";
        public static int margin = 0x10;
        public static string Notes = "Note: Use Actions->Consultant for critique of each topic.";
        private ConsultantReport[] reports = null;

        public frmAutoGrader()
        {
            this.InitializeComponent();
            base.picReport.Parent.BackColor = Color.White;
        }

        protected override void DrawReportVirtual(Graphics g)
        {
            int num;
            int num2;
            int num3;
            int num4 = 1;
            if (this.reports.Length > 1)
            {
                num4 = this.reports.Length + 1;
            }
            int count = this.reports[0].Sections.Count;
            int num6 = 220;
            int num7 = 90;
            int num8 = 90;
            int num9 = 30;
            Brush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1f);
            Pen pen2 = new Pen(new SolidBrush(Color.DarkGray), 1f);
            Font font = new Font("Arial", 14f);
            Font font2 = new Font("Arial", 10f);
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Far
            };
            for (num = 0; num < (count + 1); num++)
            {
                num2 = (margin + num8) + (num * num9);
                g.DrawLine(pen, new Point(margin, num2), new Point((margin + num6) + (num4 * num7), num2));
                if (num < count)
                {
                    g.DrawString(((ConsultantReportSection) this.reports[0].Sections[num]).Topic, font, brush, (float) margin, (float) (num2 + 5));
                }
            }
            g.DrawLine(pen, new Point(margin, margin + num8), new Point(margin, (margin + num8) + (num9 * count)));
            int index = 0;
            while (index < (num4 + 1))
            {
                num3 = (margin + num6) + (index * num7);
                g.DrawLine(pen, new Point(num3, margin + num8), new Point(num3, (margin + num8) + (num9 * count)));
                if (index < num4)
                {
                    g.DrawLine(pen2, new Point(num3 + (num7 / 2), margin + num8), new Point(num3 + (num7 / 2), (margin + num8) + (num9 * count)));
                    string s = "Average";
                    if ((index < (num4 - 1)) || ((index == 0) && (num4 == 1)))
                    {
                        s = this.reports[index].EntityName;
                    }
                    g.TranslateTransform((float) (num3 + (num7 / 2)), (float) ((margin + num8) - 20));
                    g.RotateTransform(-58f);
                    g.DrawString(s, font, brush, (float) 0f, (float) 0f);
                    g.RotateTransform(58f);
                    g.TranslateTransform((float) -(num3 + (num7 / 2)), (float) -((margin + num8) - 20));
                }
                g.DrawLine(pen, new Point(num3, margin + num8), new Point(num3 + ((2 * num7) / 3), margin));
                index++;
            }
            for (num = 0; num < count; num++)
            {
                float num11 = 0f;
                num2 = ((margin + num8) + (num * num9)) + 7;
                for (index = 0; index < num4; index++)
                {
                    float grade;
                    string str2 = "";
                    if ((index < (num4 - 1)) || ((index == 0) && (num4 == 1)))
                    {
                        grade = ((ConsultantReportSection) this.reports[index].Sections[num]).Grade;
                        num11 += grade / ((float) (num4 - 1));
                    }
                    else
                    {
                        grade = num11;
                        if ((num == (count - 1)) && (num4 > 1))
                        {
                            grade = Math.Min((float) 1f, (float) (grade + (ExtraPointsPerEntity * (num4 - 2))));
                            object[] args = new object[] { (int) (ExtraPointsPerEntity * 100f), S.Instance.EntityName.ToLower() };
                            g.DrawString(S.Resources.GetString("* Includes {0} points for each additional {1} managed.", args), font2, brush, (float) margin, (float) (num2 + 30));
                            str2 = "*";
                        }
                        if (num == (count - 1))
                        {
                            g.DrawString(Notes, font2, brush, (float) margin, (float) (num2 + 0x2d));
                        }
                    }
                    if ((index != 1) || (num4 != 1))
                    {
                        num3 = (((margin + num6) + (index * num7)) + (num7 / 2)) - 2;
                        g.DrawString(Utilities.FP(grade) + str2, font2, brush, (float) num3, (float) num2, format);
                        g.DrawString(this.LetterGrade(grade), font2, brush, (float) ((num3 + (num7 / 2)) - 12), (float) num2, format);
                    }
                }
            }
        }

        protected override void GetDataVirtual()
        {
            this.reports = ((BizStateAdapter) Simulator.Instance.SimStateAdapter).GetGrades(frmMainBase.Instance.CurrentEntityID);
        }

        private void InitializeComponent()
        {
            ((ISupportInitialize) base.picReport).BeginInit();
            base.SuspendLayout();
            base.pnlBottom.Location = new Point(0, 480);
            base.pnlBottom.Size = new Size(0x2a2, 40);
            base.picReport.BackColor = Color.White;
            base.picReport.Dock = DockStyle.Fill;
            base.picReport.Location = new Point(0, 0);
            base.picReport.Size = new Size(0x2a2, 520);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2a2, 520);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Name = "frmAutoGrader";
            this.Text = "AutoGrader";
            ((ISupportInitialize) base.picReport).EndInit();
            base.ResumeLayout(false);
        }

        private string LetterGrade(float f)
        {
            char ch = this.Grades[(int) (f * 10f)];
            return ch.ToString();
        }
    }
}

