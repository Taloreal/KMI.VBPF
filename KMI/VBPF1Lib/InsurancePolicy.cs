namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;

    [Serializable]
    public class InsurancePolicy
    {
        public bool ACV;
        public float Copay;
        public float Deductible;
        public float Limit;
        public float MonthlyPremium;

        public InsurancePolicy(float copay)
        {
            this.Copay = -1f;
            this.Copay = copay;
        }

        public InsurancePolicy(float deductible, bool acv, float limit)
        {
            this.Copay = -1f;
            this.Deductible = deductible;
            this.ACV = acv;
            this.Limit = limit;
        }

        public string PayOut(float replacementCost, float actualCashValue, GeneralLedger gl)
        {
            float limit;
            float num2;
            string str = "";
            if (this.ACV)
            {
                num2 = Math.Min(actualCashValue, this.Deductible);
                limit = actualCashValue - this.Deductible;
                object[] args = new object[] { Utilities.FC(actualCashValue, A.Instance.CurrencyConversion), Utilities.FC(limit, A.Instance.CurrencyConversion) };
                str = str + A.Resources.GetString("You have an actual cash value policy. The actual cash value of the property was {0}. Therefore, after subtracting your deductible, insurance will pay {1} .", args);
            }
            else
            {
                num2 = Math.Min(replacementCost, this.Deductible);
                limit = replacementCost - this.Deductible;
                object[] objArray2 = new object[] { Utilities.FC(limit, A.Instance.CurrencyConversion) };
                str = str + A.Resources.GetString("You have a replacement cost policy. Therefore, after subtracting your deductible, insurance will pay {0}. ", objArray2);
            }
            if (limit > this.Limit)
            {
                limit = this.Limit;
                object[] objArray3 = new object[] { Utilities.FC(limit, A.Instance.CurrencyConversion) };
                str = str + A.Resources.GetString("However, the coverage limit on your policy is {0}, so the insurance payout will only be {0}.", objArray3);
            }
            float amount = replacementCost - limit;
            gl.Post("Loss from Fire", amount, "Cash");
            return str;
        }
    }
}

