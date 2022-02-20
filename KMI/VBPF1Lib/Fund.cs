namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;

    [Serializable]
    public class Fund : ActiveObject
    {
        public float BackEndLoad;
        protected DateTime dateLastEntry;
        public float Fees12B1;
        public float FrontEndLoad;
        public float Growth;
        public string Name;
        public ArrayList sharePrice = new ArrayList();
        public float TotalExpenseRatio;

        public Fund()
        {
            A.Instance.Subscribe(this, Simulator.TimePeriod.Day);
            this.Fees12B1 = A.State.Random.Next(5) * 0.0025f;
            this.TotalExpenseRatio = this.Fees12B1 + (((float) A.State.Random.NextDouble()) / 100f);
            this.FrontEndLoad = ((float) A.State.Random.Next(4)) / 100f;
            this.BackEndLoad = ((float) A.State.Random.Next(4)) / 100f;
        }

        public override void NewDay()
        {
            this.dateLastEntry = A.State.Now;
        }

        public float PriceOn(DateTime t)
        {
            TimeSpan span = (TimeSpan) (this.DateLastEntry - t);
            int days = span.Days;
            if (days < 1)
            {
                return this.Price;
            }
            return (float) this.sharePrice[this.sharePrice.Count - days];
        }

        public float Return(int years)
        {
            float num = (float) this.sharePrice[this.sharePrice.Count - (0x34 * years)];
            return (((float) Math.Pow((double) (this.Price / num), (double) (1f / ((float) years)))) - 1f);
        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual string CategoryName
        {
            get
            {
                return "";
            }
        }

        public DateTime DateLastEntry
        {
            get
            {
                return this.dateLastEntry;
            }
        }

        public virtual float Dividend
        {
            get
            {
                return 0f;
            }
        }

        public float Previous
        {
            get
            {
                return (float) this.sharePrice[this.sharePrice.Count - 2];
            }
        }

        public float Price
        {
            get
            {
                return (float) this.sharePrice[this.sharePrice.Count - 1];
            }
        }
    }
}

