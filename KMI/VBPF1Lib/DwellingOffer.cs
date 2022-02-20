namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;

    [Serializable]
    public class DwellingOffer : Offering
    {
        public bool Condo = false;

        public override string Description()
        {
            if (this.Condo)
            {
                object[] objArray1 = new object[] { Utilities.FC(base.Building.Rent * A.State.RealEstateIndex, A.Instance.CurrencyConversion) };
                return A.Resources.GetString("One bedroom condo for sale! Priced to move at {0}.", objArray1);
            }
            object[] args = new object[] { Utilities.FC((float) base.Building.Rent, A.Instance.CurrencyConversion) };
            return A.Resources.GetString("One bedroom apartment. {0}/month. One year lease. Month-to-month after one year.", args);
        }

        public override string JournalEntry()
        {
            if (this.Condo)
            {
                object[] objArray1 = new object[] { Utilities.FC(base.Building.Rent * A.State.RealEstateIndex, A.Instance.CurrencyConversion) };
                return A.Resources.GetString("Bought condo for {0}.", objArray1);
            }
            object[] args = new object[] { Utilities.FC((float) base.Building.Rent, A.Instance.CurrencyConversion) };
            return A.Resources.GetString("Leased apartment for {0} per month.", args);
        }

        public override string ThingName()
        {
            return A.Resources.GetString("Housing");
        }
    }
}

