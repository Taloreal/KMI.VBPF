namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class RecurringPayment
    {
        public float Amount;
        public int Day;
        public long PayeeAccountNumber;
        public string PayeeName;
    }
}

