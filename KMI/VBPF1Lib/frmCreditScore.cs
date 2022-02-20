namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmCreditScore : frmDrawnReport
    {
        private Container components = null;
        private static ResponseCurve ficoPercentiles = new ResponseCurve();
        private Input input;

        static frmCreditScore()
        {
            ficoPercentiles.Points = new PointF[] { new PointF(500f, 0.01f), new PointF(600f, 0.14f), new PointF(723f, 0.5f), new PointF(750f, 0.61f), new PointF(800f, 0.89f), new PointF(850f, 0.99f) };
        }

        public frmCreditScore()
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

        protected override void DrawReportVirtual(Graphics g)
        {
            int num;
            int num2;
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Center
            };
            StringFormat format2 = new StringFormat {
                Alignment = StringAlignment.Far
            };
            Font font = new Font("Arial", 20f);
            Font font2 = new Font("Arial", 10f, FontStyle.Bold);
            Font font3 = new Font("Arial", 8f, FontStyle.Bold);
            Brush brush = new SolidBrush(Color.Red);
            Brush brush2 = new SolidBrush(Color.Black);
            Brush brush3 = new SolidBrush(Color.DarkGray);
            Brush brush4 = new SolidBrush(Color.Orange);
            Pen pen = new Pen(brush2, 2f);
            int y = 10;
            object[] args = new object[] { this.input.Score };
            g.DrawString(A.Resources.GetString("Your credit score is {0}", args), font, brush, g.ClipBounds.Width / 2f, (float) y, format);
            y += 80;
            int width = (int) (g.ClipBounds.Width * 0.7f);
            int x = (int) (g.ClipBounds.Width * 0.15f);
            g.FillRectangle(brush3, x, y, width, 20);
            g.DrawRectangle(pen, x, y, width, 20);
            float num6 = ((float) width) / 6f;
            for (num = 0; num < 7; num++)
            {
                num2 = x + ((int) (num * num6));
                g.DrawString((350 + (num * 100)).ToString(), font2, brush2, (float) num2, (float) (y - 20), format);
                g.DrawLine(pen, num2, y, num2, y + 20);
            }
            g.DrawString(A.Resources.GetString("Lowest"), font3, brush2, (float) (x - 5), (float) (y + 3), format2);
            g.DrawString(A.Resources.GetString("Highest"), font3, brush2, (float) ((x + width) + 5), (float) (y + 3));
            int num7 = x + (((this.input.Score - 350) * width) / 600);
            Point[] points = new Point[] { new Point(num7, y + 0x1c), new Point(num7 + 10, y + 0x26), new Point(num7 - 10, y + 0x26) };
            g.FillPolygon(brush4, points);
            g.DrawString(A.Resources.GetString("You Are Here"), font3, brush4, (float) num7, (float) (y + 40), format);
            float amount = ficoPercentiles.Response((float) this.input.Score);
            y += 80;
            object[] objArray2 = new object[] { Utilities.FP(amount) };
            g.DrawString(A.Resources.GetString("Your credit score is higher than {0} of the population", objArray2), font2, brush, g.ClipBounds.Width / 2f, (float) y, format);
            y += 40;
            g.FillRectangle(brush3, x, y, width, 20);
            g.DrawRectangle(pen, x, y, width, 20);
            num6 = ((float) width) / 5f;
            for (num = 0; num < 6; num++)
            {
                num2 = x + ((int) (num * num6));
                g.DrawString((num * 20).ToString(), font2, brush2, (float) num2, (float) (y - 20), format);
                g.DrawLine(pen, num2, y, num2, y + 20);
            }
            g.DrawString(A.Resources.GetString("Lowest"), font3, brush2, (float) (x - 5), (float) (y + 3), format2);
            g.DrawString(A.Resources.GetString("Highest"), font3, brush2, (float) ((x + width) + 5), (float) (y + 3));
            num7 = x + ((int) (amount * width));
            points = new Point[] { new Point(num7, y + 0x1c), new Point(num7 + 10, y + 0x26), new Point(num7 - 10, y + 0x26) };
            g.FillPolygon(brush4, points);
            g.DrawString(A.Resources.GetString("You Are Here"), font3, brush4, (float) num7, (float) (y + 40), format);
        }

        protected override void GetDataVirtual()
        {
            this.input = A.Adapter.GetCreditScore(A.MainForm.CurrentEntityID);
        }

        private void InitializeComponent()
        {
            base.pnlBottom.Name = "pnlBottom";
            base.pnlBottom.Size = new Size(0x1e2, 40);
            base.picReport.Name = "picReport";
            base.picReport.Size = new Size(0x1c0, 280);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1e2, 0x158);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmCreditScore";
            this.Text = "Credit Score";
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public int Score;
            public ArrayList Reasons;
        }
    }
}

