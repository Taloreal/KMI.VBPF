namespace KMI.Biz
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    [Serializable]
    public class CommentLog : ActiveObject
    {
        private ArrayList cache = new ArrayList();
        private int dayCounter = 0;
        private int daysToKeep;
        private int frequencyInDays = 0;
        private DateTime startDate;

        public CommentLog(int frequencyInDays, int daysToKeep)
        {
            this.frequencyInDays = frequencyInDays;
            this.dayCounter = frequencyInDays;
            this.daysToKeep = daysToKeep;
            this.cache.Add(new Hashtable());
            this.startDate = Simulator.Instance.SimState.Now;
            Simulator.Instance.Subscribe(this, Simulator.TimePeriod.Day);
        }

        public void Comment(string category, string subCategory, string comment)
        {
            if (!S.Instance.BlockMessage(comment))
            {
                Hashtable hashtable;
                Hashtable hashtable2;
                Hashtable hashtable3 = (Hashtable) this.cache[this.cache.Count - 1];
                if (hashtable3.Contains(category))
                {
                    hashtable = (Hashtable) hashtable3[category];
                }
                else
                {
                    hashtable = new Hashtable();
                    hashtable3.Add(category, hashtable);
                }
                if (hashtable.Contains(subCategory))
                {
                    hashtable2 = (Hashtable) hashtable[subCategory];
                }
                else
                {
                    hashtable2 = new Hashtable();
                    hashtable.Add(subCategory, hashtable2);
                }
                if (hashtable2.Contains(comment))
                {
                    hashtable2[comment] = ((int) hashtable2[comment]) + 1;
                }
                else
                {
                    hashtable2.Add(comment, 1);
                }
            }
        }

        public override void NewDay()
        {
            if (this.dayCounter >= this.frequencyInDays)
            {
                this.dayCounter = 0;
                if (this.cache.Count > this.daysToKeep)
                {
                    this.cache[(this.cache.Count - this.daysToKeep) - 1] = null;
                }
                this.cache.Add(new Hashtable());
            }
            this.dayCounter++;
        }

        public ArrayList Comments
        {
            get
            {
                return this.cache;
            }
        }

        public int DaysToKeep
        {
            get
            {
                return this.daysToKeep;
            }
        }

        public int FrequencyInDays
        {
            get
            {
                return this.frequencyInDays;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public CommentLog log;
            public int frequencyInDays;
        }
    }
}

