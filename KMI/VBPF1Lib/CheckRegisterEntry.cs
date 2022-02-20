namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class CheckRegisterEntry
    {
        public string Balance1;
        public string Balance2;
        public string Date;
        public string Deposit;
        public string Description1;
        public string Description2;
        public string Number;
        public string Payment;

        public CheckRegisterEntry()
        {
            this.Number = "";
            this.Date = "";
            this.Description1 = "";
            this.Description2 = "";
            this.Payment = "";
            this.Deposit = "";
            this.Balance1 = "";
            this.Balance2 = "";
        }

        public CheckRegisterEntry(KMI.VBPF1Lib.Check c)
        {
            this.Number = "";
            this.Date = "";
            this.Description1 = "";
            this.Description2 = "";
            this.Payment = "";
            this.Deposit = "";
            this.Balance1 = "";
            this.Balance2 = "";
            this.Number = c.Number.ToString();
            this.Date = c.Date.ToString("M/d/yy");
            this.Payment = c.Amount.ToString("N2");
            this.Description1 = c.Payee;
        }

        public CheckRegisterEntry(string number, string date, string description1, string description2, string payment, string deposit, string balance1, string balance2)
        {
            this.Number = "";
            this.Date = "";
            this.Description1 = "";
            this.Description2 = "";
            this.Payment = "";
            this.Deposit = "";
            this.Balance1 = "";
            this.Balance2 = "";
            this.Number = number;
            this.Date = date;
            this.Description1 = description1;
            this.Description2 = description2;
            this.Payment = payment;
            this.Deposit = deposit;
            this.Balance1 = balance1;
            this.Balance2 = balance2;
        }

        public bool IsEmpty()
        {
            return ((((((this.Number == "") && (this.Date == "")) && (this.Description1 == "")) && (((this.Description2 == "") || (this.Description2 == "^")) && ((this.Payment == "") && (this.Deposit == "")))) && (this.Balance1 == "")) && (this.Balance2 == ""));
        }
    }
}

