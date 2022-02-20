namespace KMI.Biz
{
    using System;
    using System.Collections;

    public class ValueComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((AmountNamePair) x).Amount < ((AmountNamePair) y).Amount)
            {
                return 1;
            }
            if (((AmountNamePair) x).Amount > ((AmountNamePair) y).Amount)
            {
                return -1;
            }
            return 0;
        }
    }
}

