namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class F1040EZ : TaxReturn
    {
        public F1040EZ(int year) : base(year)
        {
        }

        public static float ComputeTax(float amount, float[] dollarBrackets, float[] marginalRates)
        {
            float num = 0f;
            for (int i = 0; i < marginalRates.Length; i++)
            {
                float maxValue = float.MaxValue;
                if ((i + 1) < dollarBrackets.Length)
                {
                    maxValue = dollarBrackets[i + 1];
                }
                float num4 = Math.Max(0f, Math.Min((float) (maxValue - dollarBrackets[i]), (float) (amount - dollarBrackets[i]))) * marginalRates[i];
                num += num4;
            }
            return num;
        }
    }
}

