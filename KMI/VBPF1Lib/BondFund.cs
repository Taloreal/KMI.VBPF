namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class BondFund : Fund
    {
        protected float dividendPct;
        protected float volatility;

        public BondFund(string name, float growth)
        {
            base.Name = name;
            DateTime time = new DateTime(0x7d5, 1, 1);
            this.volatility = 0.5f;
            base.Growth = growth;
            while (time < A.State.Now)
            {
                this.NewDay();
                time = time.AddDays(1.0);
            }
            this.dividendPct = (float) ((0.75 + (0.25 * A.State.Random.NextDouble())) * Math.Max((float) 0f, (float) (A.State.PrimeRate() - ((growth * 365f) / 5f))));
        }

        public override void NewDay()
        {
            base.NewDay();
            float num = 20f;
            if (base.sharePrice.Count > 0)
            {
                num = (float) base.sharePrice[base.sharePrice.Count - 1];
            }
            num *= (1f + base.Growth) + ((float) (((A.State.Random.NextDouble() - 0.5) * 0.01) * this.volatility));
            num *= 1f + (0.2f * ((A.State.PrimeRate(A.State.Now.AddDays(-1.0)) / A.State.PrimeRate()) - 1f));
            num -= (base.TotalExpenseRatio / 365f) * num;
            if (num < 1f)
            {
                num = 1f;
            }
            base.sharePrice.Add(num);
        }

        public override string CategoryName
        {
            get
            {
                return "Bonds";
            }
        }

        public override float Dividend
        {
            get
            {
                return (this.dividendPct * base.Price);
            }
        }
    }
}

