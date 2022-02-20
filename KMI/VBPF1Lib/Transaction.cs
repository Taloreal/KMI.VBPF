namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class Transaction
    {
        public float Amount;
        public DateTime Date;
        public string Description;
        public TranType Type;

        public Transaction(float amount, TranType type, string description)
        {
            this.Amount = (float) Math.Round((double) amount, 2);
            this.Type = type;
            this.Description = description;
            this.Date = A.State.Now;
        }

        public Transaction(float amount, TranType type, string description, DateTime date)
        {
            this.Amount = (float) Math.Round((double) amount, 2);
            this.Type = type;
            this.Description = description;
            this.Date = date;
        }

        public enum TranType
        {
            Debit,
            Credit
        }
    }
}

