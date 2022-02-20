namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class StockFund : Fund
    {
        protected float dividendPct;
        protected float volatility;

        public StockFund(string name, float volatility, float growth)
        {
            base.Name = name;
            DateTime time = new DateTime(0x7d5, 1, 1);
            base.Growth = growth;
            this.volatility = volatility;
            while (time < A.State.Now)
            {
                this.NewDay();
                time = time.AddDays(1.0);
            }
            this.dividendPct = (float) (A.State.Random.NextDouble() * Math.Max((float) 0f, (float) (0.05f - ((growth * 365f) / 5f))));
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
            if (num < 1f)
            {
                num = 1f;
            }
            num -= (base.TotalExpenseRatio / 365f) * num;
            base.sharePrice.Add(num);
        }

        public override string CategoryName
        {
            get
            {
                if (this.volatility > 2.5)
                {
                    return "Intl Stocks";
                }
                return "U.S. Stocks";
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

