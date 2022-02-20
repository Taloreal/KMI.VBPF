namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class PayStub : ITaxForm
    {
        public int Allowances;
        protected static Brush brush = new SolidBrush(Color.Black);
        protected static Font font = new Font("Arial", 8f);
        protected static Font fontB = new Font("Arial", 8f, FontStyle.Bold);
        protected static Font fontL = new Font("Arial", 10f);
        protected static Font fontLB = new Font("Arial", 12f, FontStyle.Bold);
        protected static Font fontS = new Font("Arial", 7f);
        protected static Font fontSB = new Font("Arial", 7f, FontStyle.Bold);
        public float Hours;
        protected int margin = 10;
        public string OwnerName;
        public string PayDescription;
        public string Payor;
        public Hashtable PayValues = new Hashtable();
        protected static Pen pen = new Pen(brush, 1f);
        protected static Pen pen2 = new Pen(brush, 2f);
        protected static StringFormat sfc = new StringFormat();
        protected static StringFormat sfr = new StringFormat();
        public WorkTask Task;
        public DateTime WeekEnding;

        static PayStub()
        {
            sfc.Alignment = StringAlignment.Center;
            sfr.Alignment = StringAlignment.Far;
        }

        public PayStub(string payor, string ownerName, string payDescription, DateTime weekEnding, float hours, float grossPay, WorkTask task, float FICAPaidThisYear)
        {
            this.Payor = payor;
            this.PayDescription = payDescription;
            this.WeekEnding = weekEnding;
            this.Hours = hours;
            this.Allowances = task.Allowances;
            this.PayValues.Add("Gross Pay", grossPay);
            this.PayValues.Add("Soc Sec", Math.Min((float) (grossPay * 0.062f), (float) (5840.4f - FICAPaidThisYear)));
            this.PayValues.Add("Medicare", grossPay * 0.0145f);
            this.PayValues.Add("Fed WT", this.ComputeFedWithholding(grossPay, task));
            this.PayValues.Add("State WT", this.ComputeStateWithholding(grossPay, task));
            float num = (((this.GetValue("Gross Pay") - this.GetValue("Soc Sec")) - this.GetValue("Medicare")) - this.GetValue("Fed WT")) - this.GetValue("State WT");
            this.PayValues.Add("401K", Math.Min(num, grossPay * task.R401KPercentWitheld));
            this.OwnerName = ownerName;
            this.Task = task;
        }

        public float ComputeFedWithholding(float grossPay, WorkTask t)
        {
            if (t.ExemptFromWitholding)
            {
                return 0f;
            }
            float amount = (grossPay - (65.38f * t.Allowances)) - t.AdditionalWitholding;
            return (F1040EZ.ComputeTax(amount, new float[] { 51f, 195f, 645f, 1482f, 3131f, 6763f }, new float[] { 0.1f, 0.15f, 0.25f, 0.28f, 0.33f, 0.35f }) + t.AdditionalWitholding);
        }

        public float ComputeStateWithholding(float grossPay, WorkTask t)
        {
            if (t.ExemptFromWitholding)
            {
                return 0f;
            }
            float num = Math.Max((float) 0f, (float) (grossPay - (65.38f * t.Allowances)));
            return (0.05f * num);
        }

        public float GetValue(string lineItem)
        {
            return (float) this.PayValues[lineItem];
        }

        public void Print(Graphics g)
        {
            int y = 10;
            int num2 = 270;
            int num3 = 100;
            int num4 = 0x1fc;
            int num5 = 0x134;
            int num6 = 0xfc;
            int num7 = 260;
            int num8 = 60;
            int num9 = 100;
            int num10 = 180;
            int num11 = 330;
            int num12 = 420;
            int num13 = 150;
            int num14 = 260;
            int num15 = 0x181;
            int num16 = 5;
            g.DrawRectangle(pen, 0, 0, num4 - 1, num5 - 1);
            g.DrawString(A.Resources.GetString("Earnings Statement"), fontLB, brush, (float) num2, (float) y);
            y += 30;
            object[] args = new object[] { this.WeekEnding.AddDays(-6.0).ToShortDateString(), this.WeekEnding.ToShortDateString() };
            g.DrawString(A.Resources.GetString("Pay Period: {0} to {1}", args), fontS, brush, (float) num2, (float) y);
            y += 5;
            g.DrawString(this.Payor, fontB, brush, (float) (this.margin + 30), (float) y);
            y += 30;
            g.DrawString(A.Resources.GetString("Social Security #:"), fontS, brush, (float) this.margin, (float) y);
            object[] objArray2 = new object[] { this.Task.Owner.ID.ToString().PadLeft(4, '0') };
            g.DrawString(A.Resources.GetString("XXX-XX-{0}", objArray2), fontS, brush, (float) (this.margin + num3), (float) y);
            g.DrawString(this.OwnerName, fontB, brush, (float) num2, (float) y);
            y += 10;
            g.DrawString(A.Resources.GetString("Allowances:"), fontS, brush, (float) this.margin, (float) y);
            g.DrawString(this.Allowances.ToString(), fontS, brush, (float) (this.margin + num3), (float) y);
            y += 10;
            g.DrawString(A.Resources.GetString("Rate:"), fontS, brush, (float) this.margin, (float) y);
            g.DrawString(Utilities.FC(this.Task.HourlyWage, 2, A.Instance.CurrencyConversion), fontS, brush, (float) (this.margin + num3), (float) y);
            y += 0x19;
            g.DrawRectangle(pen2, this.margin, y, num4 - (2 * this.margin), (num6 - y) - this.margin);
            g.DrawLine(pen2, num7, y, num7, num6 - this.margin);
            g.DrawLine(pen, this.margin, y + 20, num4 - this.margin, y + 20);
            g.DrawLine(pen, this.margin, y + 0x23, num4 - this.margin, y + 0x23);
            y += 4;
            g.DrawString(A.Resources.GetString("Hours and Earnings"), fontB, brush, (float) ((this.margin + num7) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Taxes and Deductions"), fontB, brush, (float) ((num7 + num4) / 2), (float) y, sfc);
            y += 0x10;
            g.DrawLine(pen, num8, y, num8, num6 - this.margin);
            g.DrawLine(pen, num9, y, num9, num6 - this.margin);
            g.DrawLine(pen, num10, y, num10, num6 - this.margin);
            g.DrawLine(pen, num11, y, num11, num6 - this.margin);
            g.DrawLine(pen, num12, y, num12, num6 - this.margin);
            y += 3;
            g.DrawString(A.Resources.GetString("Description"), fontS, brush, (float) ((this.margin + num8) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Hours"), fontS, brush, (float) ((num8 + num9) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("This Period"), fontS, brush, (float) ((num9 + num10) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Year-To-Date"), fontS, brush, (float) ((num10 + num7) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Description"), fontS, brush, (float) ((num7 + num11) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("This Period"), fontS, brush, (float) ((num11 + num12) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Year-To-Date"), fontS, brush, (float) (((num12 + num4) - this.margin) / 2), (float) y, sfc);
            y += 15;
            g.DrawString(this.PayDescription, font, brush, (float) ((this.margin + num16) - 4), (float) y);
            g.DrawString(this.Hours.ToString(), font, brush, (float) (num9 - num16), (float) y, sfr);
            g.DrawString(this.GetValue("Gross Pay").ToString("N2"), font, brush, (float) (num10 - num16), (float) y, sfr);
            g.DrawString(this.Task.GetValueYTD("Gross Pay", this.WeekEnding).ToString("N2"), font, brush, (float) (num7 - num16), (float) y, sfr);
            string[] strArray = new string[] { "Soc Sec", "Medicare", "Fed WT", "State WT", "401K" };
            foreach (string str in strArray)
            {
                g.DrawString(str, font, brush, (float) (num11 - num16), (float) y, sfr);
                g.DrawString(this.GetValue(str).ToString("N2"), font, brush, (float) (num12 - num16), (float) y, sfr);
                g.DrawString(this.Task.GetValueYTD(str, this.WeekEnding).ToString("N2"), font, brush, (float) ((num4 - this.margin) - num16), (float) y, sfr);
                y += 12;
            }
            y = num6;
            g.DrawRectangle(pen2, this.margin, y, num4 - (2 * this.margin), 0x2d);
            g.DrawLine(pen, this.margin, y + 0x12, num4 - this.margin, y + 0x12);
            g.DrawLine(pen2, num13, y, num13, y + 0x2d);
            g.DrawLine(pen, num14, y, num14, y + 0x2d);
            g.DrawLine(pen, num15, y, num15, y + 0x2d);
            y += 4;
            g.DrawString(A.Resources.GetString("Gross Pay Year-To-Date"), fontS, brush, (float) ((this.margin + num13) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Gross Pay This Period"), fontS, brush, (float) ((num13 + num14) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Total Deductions This Period"), fontS, brush, (float) ((num14 + num15) / 2), (float) y, sfc);
            g.DrawString(A.Resources.GetString("Net Pay This Period"), fontS, brush, (float) ((num15 + num4) / 2), (float) y, sfc);
            y += 20;
            g.DrawString(Utilities.FC(this.Task.GetValueYTD("Gross Pay", this.WeekEnding), 2, A.Instance.CurrencyConversion), font, brush, (float) (num13 - this.margin), (float) y, sfr);
            g.DrawString(Utilities.FC(this.GetValue("Gross Pay"), 2, A.Instance.CurrencyConversion), font, brush, (float) (num14 - this.margin), (float) y, sfr);
            g.DrawString(Utilities.FC(this.GetValue("Gross Pay") - this.NetPay, 2, A.Instance.CurrencyConversion), font, brush, (float) (num15 - this.margin), (float) y, sfr);
            g.DrawString(Utilities.FC(this.NetPay, 2, A.Instance.CurrencyConversion), fontB, brush, (float) ((num4 - this.margin) - this.margin), (float) y, sfr);
        }

        public int Year()
        {
            return this.WeekEnding.Year;
        }

        public float NetPay
        {
            get
            {
                return (((((this.GetValue("Gross Pay") - this.GetValue("Soc Sec")) - this.GetValue("Medicare")) - this.GetValue("Fed WT")) - this.GetValue("State WT")) - this.GetValue("401K"));
            }
        }
    }
}

