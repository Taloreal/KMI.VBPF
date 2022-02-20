namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;

    [Serializable]
    public class AccountantsReport : TaxReturn
    {
        public int FedWT;
        public int Owed;
        public string Report;
        public int Tax;

        public AccountantsReport(int year) : base(year)
        {
        }

        public void PrepareTaxes(int year, AppEntity e, bool itemizeIfBetter)
        {
            int num = 0x2102;
            float amount = 0f;
            float num3 = 0f;
            int stateWT = 0;
            string newLine = Environment.NewLine;
            this.FedWT = 0;
            string[] textArray1 = new string[] { "Tax Professional's Tax Return for {0}", newLine, "Tax Year {1}", newLine, newLine };
            object[] args = new object[] { e.Name, year };
            this.Report = this.Report + A.Resources.GetString(string.Concat(textArray1), args).ToUpper();
            foreach (FW2 fw in e.FW2s.Values)
            {
                if (fw.Year() == year)
                {
                    amount += fw.Wages;
                    this.FedWT += (int) Math.Ceiling((double) fw.FedWT);
                    stateWT = (int) fw.StateWT;
                }
            }
            object[] objArray2 = new object[] { Utilities.FC(amount, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("  Wages: {0}" + newLine, objArray2);
            foreach (F1099Int num13 in e.F1099s.Values)
            {
                if (num13.Year() == year)
                {
                    num3 += num13.Interest;
                }
            }
            object[] objArray3 = new object[] { Utilities.FC(num3, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("+ Interest Income: {0}" + newLine, objArray3);
            float num5 = 0f;
            foreach (InvestmentAccount account in e.InvestmentAccounts.Values)
            {
                foreach (Transaction transaction in account.Transactions)
                {
                    if ((transaction.Date.Year == year) && (transaction.Description == A.Resources.GetString("Dividend Purchase")))
                    {
                        num5 += transaction.Amount * account.Fund.PriceOn(transaction.Date);
                    }
                }
            }
            object[] objArray4 = new object[] { Utilities.FC(num5, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("+ Ordinary Dividends: {0}" + newLine, objArray4);
            float num6 = 0f;
            if (e.STCapitalGains.ContainsKey(year))
            {
                num6 = (float) e.STCapitalGains[year];
            }
            object[] objArray5 = new object[] { Utilities.FC(num6, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("+ Short-term Capital Gains: {0}" + newLine, objArray5);
            int num7 = (int) Math.Round((double) (((Math.Round((double) amount) + Math.Round((double) num3)) + Math.Round((double) num5)) + Math.Round((double) num6)));
            object[] objArray6 = new object[] { Utilities.FC((float) num7, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("= Total Ordinary Income: {0}" + newLine, objArray6);
            int num8 = num;
            if (itemizeIfBetter)
            {
                int num14 = 0;
                foreach (Mortgage mortgage in e.Mortgages.Values)
                {
                    num14 += mortgage.InterestPaid(year);
                }
                if ((num14 + stateWT) > num)
                {
                    num8 = num14 + stateWT;
                    object[] objArray7 = new object[] { Utilities.FC((float) num8, A.Instance.CurrencyConversion) };
                    this.Report = this.Report + A.Resources.GetString("- Itemized Deduction: {0}" + newLine, objArray7);
                    if (num14 > 0)
                    {
                        object[] objArray8 = new object[] { Utilities.FC((float) num14, A.Instance.CurrencyConversion) };
                        this.Report = this.Report + A.Resources.GetString("    Mortgage Interest: {0}" + newLine, objArray8);
                    }
                    object[] objArray9 = new object[] { Utilities.FC((float) stateWT, A.Instance.CurrencyConversion) };
                    this.Report = this.Report + A.Resources.GetString("    State Taxes: {0}" + newLine, objArray9);
                }
            }
            if (num8 == num)
            {
                object[] objArray10 = new object[] { Utilities.FC((float) num8, A.Instance.CurrencyConversion) };
                this.Report = this.Report + A.Resources.GetString("- Standard Deduction: {0}" + newLine, objArray10);
            }
            int num9 = Math.Max(0, num7 - num8);
            object[] objArray11 = new object[] { Utilities.FC((float) num9, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("= Taxable Ordinary Income: {0}" + newLine, objArray11);
            float[] dollarBrackets = new float[] { 0f, 7550f, 30650f, 74200f, 154800f, 336550f };
            this.Tax = (int) Math.Round((double) F1040EZ.ComputeTax((float) num9, dollarBrackets, new float[] { 0.1f, 0.15f, 0.25f, 0.28f, 0.33f, 0.35f }));
            object[] objArray12 = new object[] { Utilities.FC((float) this.Tax, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("  Tax On Ordinary Income: {0}" + newLine + newLine, objArray12);
            float num10 = 0f;
            if (e.LTCapitalGains.ContainsKey(year))
            {
                num10 = (float) e.LTCapitalGains[year];
            }
            object[] objArray13 = new object[] { Utilities.FC(num10, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("  Long-term Capital Gains: {0}" + newLine, objArray13);
            float num11 = 0.05f;
            if ((num9 + ((int) num10)) > dollarBrackets[1])
            {
                num11 = 0.15f;
            }
            int num12 = (int) (num11 * ((int) num10));
            object[] objArray14 = new object[] { Utilities.FP(num11) };
            this.Report = this.Report + A.Resources.GetString("x Capital Gains Tax Rate: {0}" + newLine, objArray14);
            object[] objArray15 = new object[] { Utilities.FC((float) num12, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("= Tax on Long-term Capital Gains: {0}" + newLine + newLine, objArray15);
            this.Tax += num12;
            this.Owed = this.Tax - this.FedWT;
            object[] objArray16 = new object[] { Utilities.FC((float) this.Tax, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("  Total Tax Liability: {0}" + newLine, objArray16);
            object[] objArray17 = new object[] { Utilities.FC((float) this.FedWT, A.Instance.CurrencyConversion) };
            this.Report = this.Report + A.Resources.GetString("- Total Federal Withholding: {0}" + newLine, objArray17);
            if (this.Owed > 0)
            {
                object[] objArray18 = new object[] { Utilities.FC((float) this.Owed, A.Instance.CurrencyConversion) };
                this.Report = this.Report + A.Resources.GetString("= Tax Due: {0}" + newLine, objArray18);
                base.Values[8] = this.Owed;
            }
            else
            {
                object[] objArray19 = new object[] { Utilities.FC((float) -this.Owed, A.Instance.CurrencyConversion) };
                this.Report = this.Report + A.Resources.GetString("= Refund: {0}" + newLine, objArray19);
                base.Values[7] = -this.Owed;
            }
        }
    }
}

