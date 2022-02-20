namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class MoneyMarketFund : Fund
    {
        public float DiffToPrime;

        public MoneyMarketFund(string name, float diffToPrime)
        {
            base.Name = name;
            for (DateTime time = new DateTime(0x7d5, 1, 1); time < A.State.Now; time = time.AddDays(1.0))
            {
                this.NewDay();
            }
            this.DiffToPrime = diffToPrime;
            base.Fees12B1 = A.State.Random.Next(2) * 0.0025f;
            base.TotalExpenseRatio = base.Fees12B1 + ((((float) A.State.Random.NextDouble()) / 100f) / 4f);
        }

        public override void NewDay()
        {
            base.NewDay();
            base.sharePrice.Add(1f);
        }

        public override string CategoryName
        {
            get
            {
                return "Money Markets";
            }
        }

        public override float Dividend
        {
            get
            {
                return (((A.State.PrimeRate() + this.DiffToPrime) - base.TotalExpenseRatio) * base.Price);
            }
        }
    }
}

