namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class TaxReturn
    {
        public DateTime FilingDate;
        public string FirstNameAndInitial;
        public string HomeAddress;
        public string HomeCityStateZip;
        public string LastName;
        public string[] Lines = new string[30];
        public string Occupation;
        public string SSN;
        public int[] Values = new int[30];
        public int Year;

        public TaxReturn(int year)
        {
            this.Year = year;
        }

        public override string ToString()
        {
            return this.Year.ToString();
        }
    }
}

