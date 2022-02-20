namespace KMI.Biz
{
    using KMI.Utility;
    using System;
    using System.Drawing;

    [Serializable]
    public class ConsultantReportSection
    {
        protected string finding;
        protected float grade;
        private const int INDENT = 30;
        protected string topic;
        private const int VSPACE = 10;

        public int Print(int left, int top, int width, Graphics g, bool grades)
        {
            Font font = new Font("Arial", 12f, FontStyle.Bold);
            Font font2 = new Font("Arial", 11f);
            Font font3 = new Font("Arial", 11f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat {
                Trimming = StringTrimming.Word
            };
            SizeF ef = g.MeasureString(this.Topic, font, width, format);
            g.DrawString(this.Topic, font, brush, new RectangleF((float) left, (float) top, ef.Width, ef.Height), format);
            top += ((int) ef.Height) + 10;
            ef = g.MeasureString(this.Finding, font2, (int) (width - 30), format);
            g.DrawString(this.Finding, font2, brush, new RectangleF((float) (left + 30), (float) top, ef.Width, ef.Height), format);
            top += ((int) ef.Height) + 10;
            if (grades)
            {
                ef = g.MeasureString("Grade: " + Utilities.FP(this.Grade), font3, width, format);
                g.DrawString("Grade: " + Utilities.FP(this.Grade), font3, brush, new RectangleF((float) (left + 30), (float) top, ef.Width, ef.Height), format);
                top += ((int) ef.Height) + 20;
            }
            top += 10;
            return top;
        }

        public int PrintHeight(int width, Graphics g, bool grades)
        {
            Font font = new Font("Arial", 12f, FontStyle.Bold);
            Font font2 = new Font("Arial", 11f);
            Font font3 = new Font("Arial", 11f, FontStyle.Bold);
            StringFormat format = new StringFormat {
                Trimming = StringTrimming.Word
            };
            int num = 0;
            SizeF ef = g.MeasureString(this.Topic, font, width, format);
            num += ((int) ef.Height) + 10;
            ef = g.MeasureString(this.Finding, font2, (int) (width - 30), format);
            num += ((int) ef.Height) + 10;
            if (grades)
            {
                ef = g.MeasureString("Grade: " + Utilities.FP(this.Grade), font3, width, format);
                num += ((int) ef.Height) + 20;
            }
            return (num + 10);
        }

        public string Finding
        {
            get
            {
                return this.finding;
            }
            set
            {
                this.finding = value;
            }
        }

        public float Grade
        {
            get
            {
                return this.grade;
            }
            set
            {
                this.grade = value;
            }
        }

        public string Topic
        {
            get
            {
                return this.topic;
            }
            set
            {
                this.topic = value;
            }
        }
    }
}

