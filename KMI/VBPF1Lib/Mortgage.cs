namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class Mortgage : InstallmentLoan
    {
        public long BuildingID;

        public Mortgage(DateTime date, float amount, float interestRate, int term) : base(date, amount, interestRate, term)
        {
        }

        public int InterestPaid(int year)
        {
            float num = 0f;
            float num2 = 0f;
            for (int i = 1; i <= 12; i++)
            {
                foreach (Transaction transaction in base.TransactionsForMonth(year, i))
                {
                    if (transaction.Description.StartsWith("Payment"))
                    {
                        num += transaction.Amount;
                    }
                    if (transaction.Description.StartsWith("Interest"))
                    {
                        num2 += transaction.Amount;
                    }
                }
            }
            return (int) Math.Min(num2, num);
        }

        public float PMI(float valueOfCondo)
        {
            float num = 0f;
            if ((base.OriginalBalance / Math.Max(1f, valueOfCondo)) > 0.8)
            {
                num = (base.OriginalBalance * 0.005f) / 12f;
            }
            return num;
        }

        public override string ToString()
        {
            object[] args = new object[] { base.Term / 12 };
            return A.Resources.GetString("{0}-Year Fixed Rate", args);
        }

        public float ClosingCosts
        {
            get
            {
                return (2000f + (0.01f * base.OriginalBalance));
            }
        }
    }
}

