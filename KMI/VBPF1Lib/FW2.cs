namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class FW2 : ITaxForm
    {
        protected static Brush brush = new SolidBrush(Color.Black);
        public string Employer;
        public float FedWT;
        protected static Font font = new Font("Arial", 8f);
        protected static Font fontB = new Font("Arial", 8f, FontStyle.Bold);
        protected static Font fontL = new Font("Arial", 10f);
        protected static Font fontLB = new Font("Arial", 12f, FontStyle.Bold);
        protected static Font fontS = new Font("Arial", 7f);
        protected static Font fontSB = new Font("Arial", 7f, FontStyle.Bold);
        protected static Font fontXLB = new Font("Arial", 16f, FontStyle.Bold);
        public float MedicareWages;
        public long OwnerID;
        public string OwnerName;
        protected static Pen pen = new Pen(brush, 1f);
        protected static Pen pen2 = new Pen(brush, 2f);
        public bool RetirementPlan;
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        public float SSWages;
        public float StateWT;
        public float Wages;
        protected int year;

        static FW2()
        {
            sfc.Alignment = StringAlignment.Center;
            sfr.Alignment = StringAlignment.Far;
        }

        public FW2(int year, string employer, string ownerName, long ownerID)
        {
            this.year = year;
            this.Employer = employer;
            this.OwnerName = ownerName;
            this.OwnerID = ownerID;
        }

        public void Print(Graphics g)
        {
            int num = 0x1fc;
            int num2 = 0x134;
            int width = 10;
            int x = 1;
            int num5 = 12;
            int num6 = 110;
            int num7 = 220;
            int num8 = 330;
            int num9 = 0x19;
            int num10 = 50;
            int num11 = 0x4b;
            int num12 = 100;
            int num13 = 150;
            int num14 = 0xaf;
            int num15 = 200;
            int num16 = 0x109;
            int num17 = 0x11d;
            int num18 = 70;
            int num19 = 150;
            int num20 = 250;
            int num21 = 350;
            g.DrawRectangle(pen, 0, 0, num - 1, num2 - 1);
            g.DrawLine(pen, 0, num9, num, num9);
            g.DrawLine(pen, num7, num10, num, num10);
            g.DrawLine(pen, num7, num11, num, num11);
            g.DrawLine(pen, 0, num12, num, num12);
            g.DrawLine(pen, num7, num13, num8, num13);
            g.DrawLine(pen, num7, num14, num8, num14);
            g.DrawLine(pen, 0, num15, num, num15);
            g.DrawLine(pen, 0, num16, num, num16);
            g.DrawLine(pen, 0, num17, num, num17);
            g.DrawLine(pen, num6, 0, num6, num9);
            g.DrawLine(pen, num7, 0, num7, num16);
            g.DrawLine(pen, num8, 0, num8, num15);
            g.DrawLine(pen, num18, num16, num18, num2);
            g.DrawLine(pen, num19, num16, num19, num2);
            g.DrawLine(pen, num20, num16, num20, num2);
            g.DrawLine(pen, num21, num16, num21, num2);
            g.DrawString("a. Control Number", fontS, brush, (float) x, (float) x);
            g.DrawString("123456789", fontB, brush, (float) width, (float) num5);
            g.DrawString("d. Emp. Social Sec. No.", fontS, brush, (float) (num6 + x), (float) x);
            object[] args = new object[] { this.OwnerID.ToString().PadLeft(4, '0') };
            g.DrawString(A.Resources.GetString("XXX-XX-{0}", args), fontB, brush, (float) (num6 + width), (float) num5);
            g.DrawString("1. Wages,tips, other", fontS, brush, (float) (num7 + x), (float) x);
            g.DrawString(this.Wages.ToString("N2"), fontB, brush, (float) (num8 - width), (float) num5, sfr);
            g.DrawString("2. Fed income tax withheld", fontS, brush, (float) (num8 + x), (float) x);
            g.DrawString(this.FedWT.ToString("N2"), fontB, brush, (float) (num - width), (float) num5, sfr);
            g.DrawString("3. Social Sec. Wages", fontS, brush, (float) (num7 + x), (float) (num9 + x));
            g.DrawString(this.SSWages.ToString("N2"), fontB, brush, (float) (num8 - width), (float) (num9 + num5), sfr);
            g.DrawString("4. Social Sec. tax withheld", fontS, brush, (float) (num8 + x), (float) (num9 + x));
            g.DrawString((this.SSWages * 0.062f).ToString("N2"), fontB, brush, (float) (num - width), (float) (num9 + num5), sfr);
            g.DrawString("5. Medicare Wages", fontS, brush, (float) (num7 + x), (float) (num10 + x));
            g.DrawString(this.MedicareWages.ToString("N2"), fontB, brush, (float) (num8 - width), (float) (num10 + num5), sfr);
            g.DrawString("6. Medicare tax withheld", fontS, brush, (float) (num8 + x), (float) (num10 + x));
            g.DrawString((this.SSWages * 0.0145f).ToString("N2"), fontB, brush, (float) (num - width), (float) (num10 + num5), sfr);
            g.DrawString("13. Retirement plan", fontS, brush, (float) (num7 + x), (float) (num13 + x));
            g.DrawRectangle(pen, num7 + 0x17, num13 + 12, width, width);
            if (this.RetirementPlan)
            {
                g.DrawString("X", fontB, brush, (float) (num7 + 0x17), (float) (num13 + 12));
            }
            g.DrawString("c. Employer's name", fontS, brush, (float) x, (float) (num9 + x));
            g.DrawString(this.Employer, fontB, brush, (float) width, (float) (num9 + num5));
            g.DrawString("e. Employee's name", fontS, brush, (float) x, (float) (num12 + x));
            g.DrawString(this.OwnerName, fontB, brush, (float) width, (float) (num12 + num5));
            g.DrawString(A.Resources.GetString("This information is being furnished to the Internal Revenue Service. If you are required to file a tax return, a negligence penalty or other sanction may be imposed on you if this income is taxable & you fail to report it."), fontS, brush, new Rectangle(x, num15 + x, num7 - x, (num16 - num15) - x));
            g.DrawString(A.Resources.GetString("FORM"), fontB, brush, (float) ((num7 + num5) + 0x22), (float) ((num15 + width) - 4));
            object[] objArray2 = new object[] { this.year };
            g.DrawString(A.Resources.GetString("W-2 {0}", objArray2), fontXLB, brush, (float) (num7 + num5), (float) ((num15 + num5) + 6));
            g.DrawString(A.Resources.GetString("Wage and Tax"), fontLB, brush, (float) ((num8 + num) / 2), (float) (num15 + num5), sfc);
            g.DrawString(A.Resources.GetString("Statement"), fontLB, brush, (float) ((num8 + num) / 2), (float) ((num15 + num5) + 0x10), sfc);
            g.DrawString(A.Resources.GetString("Copy C for EMPLOYEE'S RECORDS"), font, brush, (float) (num7 + width), (float) ((num15 + num5) + 0x26));
            g.DrawString("15. State", fontS, brush, (float) x, (float) (num16 + x));
            g.DrawString("XY", fontB, brush, (float) num5, (float) (num17 + x));
            g.DrawString("16. State wages, etc.", fontS, brush, (float) (num19 + x), (float) (num16 + x));
            g.DrawString(this.Wages.ToString("N2"), fontB, brush, (float) (num19 + num5), (float) (num17 + x));
            g.DrawString("17. State income tax", fontS, brush, (float) (num20 + x), (float) (num16 + x));
            g.DrawString(this.StateWT.ToString("N2"), fontB, brush, (float) (num20 + num5), (float) (num17 + x));
        }

        public int Year()
        {
            return this.year;
        }
    }
}

