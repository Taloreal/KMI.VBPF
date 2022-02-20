namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    [Serializable]
    public class ConsultantReport
    {
        public DateTime Date;
        public string EntityName;
        private bool printFull;
        private bool printGrades;
        protected string printLead;
        private int printPage;
        private int printSection;
        protected string reportTitle = "";
        protected ArrayList sections = new ArrayList();
        protected ConsultantReportSection worstSection;

        public ConsultantReport(Entity e)
        {
            this.reportTitle = "Report on " + e.Name;
            this.EntityName = e.Name;
        }

        protected void AddOverallGradeSection()
        {
            float num = 0f;
            foreach (ConsultantReportSection section2 in this.sections)
            {
                num += section2.Grade;
            }
            ConsultantReportSection section = new ConsultantReportSection {
                Topic = "Overall Grade",
                Finding = "Your overall grade based on all the elements I analyzed is shown below.",
                Grade = num /= (float) this.sections.Count
            };
            this.sections.Add(section);
        }

        public void AddSection(ConsultantReportSection crs)
        {
            this.sections.Add(crs);
        }

        public void Finish(string[] sectionOrdering)
        {
            ArrayList list = new ArrayList();
            foreach (string str in sectionOrdering)
            {
                foreach (ConsultantReportSection section in this.sections)
                {
                    if (section.Topic == str)
                    {
                        list.Add(section);
                    }
                }
            }
            this.sections = list;
            this.AddOverallGradeSection();
        }

        protected int PrintHeader(int left, int top, int width, Graphics g)
        {
            Font font = new Font("Arial", 14f, FontStyle.Bold);
            Font font2 = new Font("Arial", 13f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);
            int num = top;
            SizeF ef = g.MeasureString("Virtual Consulting, LLC", font, width);
            g.DrawString("Virtual Consulting, LLC", font, brush, new RectangleF((left + width) - ef.Width, (float) top, (float) width, 1000f));
            g.DrawLine(new Pen(brush, 3f), new PointF((float) left, top + (ef.Height / 2f)), new PointF(((left + width) - ef.Width) - 10f, top + (ef.Height / 2f)));
            num += ((int) ef.Height) + 10;
            string text = "Subject: " + this.reportTitle + "\r\nDate: " + this.Date.ToLongDateString();
            ef = g.MeasureString(text, font2, width);
            g.DrawString(text, font2, brush, new RectangleF((float) left, (float) num, (float) width, 1000f));
            num += ((int) ef.Height) + 10;
            g.DrawLine(new Pen(brush, 1f), new PointF((float) left, (float) num), new PointF(left + ef.Width, (float) num));
            return (num + 20);
        }

        protected int PrintLead(int left, int top, int width, Graphics g, string lead)
        {
            Font font = new Font("Arial", 11f);
            Brush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat {
                Trimming = StringTrimming.Word
            };
            g.DrawString(lead, font, brush, new RectangleF((float) left, (float) top, (float) width, 1000f), format);
            SizeF ef = g.MeasureString(lead, font, width, format);
            return ((top + ((int) ef.Height)) + 10);
        }

        public void PrintToPrinter(bool full, bool grades)
        {
            PrintDialog dialog = new PrintDialog();
            PrintDocument document = new PrintDocument();
            document.PrintPage += new PrintPageEventHandler(this.Report_PrintPage);
            dialog.Document = document;
            dialog.AllowPrintToFile = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                document.PrinterSettings = dialog.PrinterSettings;
                if (full)
                {
                    this.printSection = 0;
                }
                else
                {
                    this.printSection = this.sections.IndexOf(this.worstSection);
                }
                this.printPage = 1;
                this.printFull = full;
                this.printGrades = grades;
                document.Print();
            }
        }

        public int PrintToScreen(int width, Graphics g, bool full, bool grades)
        {
            string str;
            int top = 0;
            top = this.PrintHeader(0, top, width, g);
            if (full)
            {
                str = "I reviewed many different aspects of your business. Here are my results and advice in each area. ";
                top = this.PrintLead(0, top, width, g, str);
                foreach (ConsultantReportSection section in this.sections)
                {
                    if ((section != this.sections[this.sections.Count - 1]) | grades)
                    {
                        top = section.Print(0, top, width, g, grades);
                    }
                }
            }
            else
            {
                ConsultantReportSection[] array = (ConsultantReportSection[]) this.sections.ToArray(typeof(ConsultantReportSection));
                Array.Sort(array, new GradeComparer());
                this.worstSection = array[0];
                if (this.worstSection.Grade > 0.9)
                {
                    str = "Overall, your business looks in excellent condition. If I had to suggest improvement in any area, it would be this one.";
                }
                else
                {
                    str = "After looking over your business, here is the area I feel needs the most improvement. ";
                }
                top = this.PrintLead(0, top, width, g, str);
                top = this.worstSection.Print(0, top, width, g, grades);
            }
            this.printLead = str;
            return top;
        }

        public string RenderGradesAsHTML()
        {
            return "";
        }

        public string RenderTextAsHTML()
        {
            return "";
        }

        private void Report_PrintPage(object sender, PrintPageEventArgs e)
        {
            Utilities.ResetFPU();
            Graphics g = e.Graphics;
            RectangleF ef = new RectangleF((float) e.MarginBounds.Left, (float) e.MarginBounds.Top, (float) e.MarginBounds.Width, (float) e.MarginBounds.Height);
            if (this.printPage == 1)
            {
                int top = this.PrintHeader((int) ef.X, (int) ef.Top, (int) ef.Width, g);
                top = this.PrintLead((int) ef.X, top, (int) ef.Width, g, this.printLead);
                ef.Height -= top - ef.Top;
                ef.Y = top;
            }
            while (this.printSection < this.sections.Count)
            {
                if ((this.printSection < (this.sections.Count - 1)) || this.printGrades)
                {
                    ConsultantReportSection section = (ConsultantReportSection) this.sections[this.printSection];
                    int num2 = section.PrintHeight((int) ef.Width, g, this.printGrades);
                    if (num2 >= ef.Height)
                    {
                        break;
                    }
                    section.Print((int) ef.X, (int) ef.Y, (int) ef.Width, g, this.printGrades);
                    ef.Y += num2;
                    ef.Height -= num2;
                }
                if (this.printFull)
                {
                    this.printSection++;
                }
                else
                {
                    this.printSection = 0x7fffffff;
                }
            }
            this.printPage++;
            e.HasMorePages = this.printSection < this.sections.Count;
        }

        public ArrayList Sections
        {
            get
            {
                return this.sections;
            }
        }

        protected class GradeComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if (((ConsultantReportSection) x).Grade < ((ConsultantReportSection) y).Grade)
                {
                    return -1;
                }
                if (((ConsultantReportSection) x).Grade > ((ConsultantReportSection) y).Grade)
                {
                    return 1;
                }
                return 0;
            }
        }
    }
}

