namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class F1099Int : ITaxForm
    {
        protected static Brush brush = new SolidBrush(Color.Black);
        protected static Font font = new Font("Arial", 8f);
        protected static Font fontB = new Font("Arial", 8f, FontStyle.Bold);
        protected static Font fontL = new Font("Arial", 10f);
        protected static Font fontLB = new Font("Arial", 12f, FontStyle.Bold);
        protected static Font fontS = new Font("Arial", 7f);
        protected static Font fontSB = new Font("Arial", 7f, FontStyle.Bold);
        protected static Font fontXLB = new Font("Arial", 16f, FontStyle.Bold);
        public float Interest;
        public long OwnerID;
        public string OwnerName;
        public string Payer;
        protected static Pen pen = new Pen(brush, 1f);
        protected static Pen pen2 = new Pen(brush, 2f);
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        public float USTreasuryInterest;
        protected int year;

        static F1099Int()
        {
            sfc.Alignment = StringAlignment.Center;
            sfr.Alignment = StringAlignment.Far;
        }

        public F1099Int(int year, string payer, string ownerName, long ownerID)
        {
            this.year = year;
            this.Payer = payer;
            this.OwnerName = ownerName;
            this.OwnerID = ownerID;
        }

        public void Print(Graphics g)
        {
            int num = 0x1fc;
            int num2 = 0x134;
            int num3 = 10;
            int num4 = 1;
            int num5 = 12;
            int num6 = 110;
            int num7 = 220;
            int num8 = 310;
            int num9 = 0x18b;
            int num10 = 400;
            int num11 = 0x25;
            int num12 = 0x4a;
            int num13 = 0x6f;
            int num14 = 0x94;
            int num15 = 0xc5;
            int num16 = 0xea;
            int num17 = 0x10f;
            g.DrawRectangle(pen, 0, 0, num - 1, num2 - 1);
            g.DrawLine(pen, num7, num11, num8, num11);
            g.DrawLine(pen, num7, num12, num8, num12);
            g.DrawLine(pen, 0, num13, num, num13);
            g.DrawLine(pen, 0, num14, num10, num14);
            g.DrawLine(pen, 0, num15, num10, num15);
            g.DrawLine(pen, 0, num16, num10, num16);
            g.DrawLine(pen, 0, num17, num7, num17);
            g.DrawLine(pen, num6, num13, num6, num14);
            g.DrawLine(pen, num7, 0, num7, num2);
            g.DrawLine(pen, num8, 0, num8, num13);
            g.DrawLine(pen, num8, num14, num8, num2);
            g.DrawLine(pen, num9, 0, num9, num13);
            g.DrawLine(pen, num10, num13, num10, num2);
            g.DrawString("PAYER'S name", fontS, brush, (float) num4, (float) num4);
            g.DrawString(this.Payer, fontB, brush, (float) num3, (float) num5);
            g.DrawString("RECIPIENT'S ID number", fontS, brush, (float) (num6 + num4), (float) (num13 + num4));
            object[] args = new object[] { this.OwnerID.ToString().PadLeft(4, '0') };
            g.DrawString(A.Resources.GetString("XXX-XX-{0}", args), fontB, brush, (float) (num6 + num3), (float) (num13 + num5));
            g.DrawString("1. Interest income", fontS, brush, (float) (num7 + num4), (float) (num11 + num4));
            g.DrawString(this.Interest.ToString("N2"), fontB, brush, (float) (num8 - num3), (float) (num11 + num5), sfr);
            g.DrawString("RECIPIENT'S name", fontS, brush, (float) num4, (float) (num14 + num4));
            g.DrawString(this.OwnerName, fontB, brush, (float) num3, (float) (num14 + num5));
            g.DrawString(A.Resources.GetString("This is important tax information and is being furnished to the Internal Revenue Service. If you are required to file a return, a negligence penalty or other sanction may be imposed on you if this income is taxable and the IRS determines that it has not be reported."), fontS, brush, new Rectangle(num10 + num4, num14 + num5, (num - num10) - 4, (num2 - num14) - num5), sfr);
            g.DrawString(this.year.ToString(), fontXLB, brush, (float) ((num8 + num9) / 2), (float) (num11 + num5), sfc);
            g.DrawString(A.Resources.GetString("Form 1099-INT"), fontB, brush, (float) ((num8 + num9) / 2), (float) (num12 + num5), sfc);
            g.DrawString(A.Resources.GetString("Interest" + Environment.NewLine + "Income"), fontLB, brush, (float) ((num10 + num) / 2), (float) (num11 + num5), sfc);
            g.DrawString(A.Resources.GetString("Copy B" + Environment.NewLine + "For Recipient"), fontB, brush, (float) num, (float) (num13 + num5), sfr);
        }

        public int Year()
        {
            return this.year;
        }
    }
}

