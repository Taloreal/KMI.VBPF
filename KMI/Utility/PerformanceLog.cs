namespace KMI.Utility
{
    using System;
    using System.Collections;

    [Serializable]
    public class PerformanceLog
    {
        protected ArrayList data;
        protected int entriesToKeep;

        public PerformanceLog()
        {
            this.data = new ArrayList();
            this.entriesToKeep = 10;
        }

        public PerformanceLog(int entriesToKeep)
        {
            this.data = new ArrayList();
            this.entriesToKeep = 10;
            this.entriesToKeep = entriesToKeep;
        }

        public PerformanceLog(float initialValue)
        {
            this.data = new ArrayList();
            this.entriesToKeep = 10;
            for (int i = 0; i < this.entriesToKeep; i++)
            {
                this.AddEntry(initialValue);
            }
        }

        public PerformanceLog(int entriesToKeep, float initialValue)
        {
            this.data = new ArrayList();
            this.entriesToKeep = 10;
            this.entriesToKeep = entriesToKeep;
            for (int i = 0; i < entriesToKeep; i++)
            {
                this.AddEntry(initialValue);
            }
        }

        public void AddEntry(int entry)
        {
            this.AddEntry((float) entry);
        }

        public void AddEntry(float entry)
        {
            this.data.Add(entry);
            if (this.data.Count > this.entriesToKeep)
            {
                this.data.RemoveAt(0);
            }
        }

        public float GetAvg()
        {
            if (this.IsEmpty)
            {
                return 0f;
            }
            float num = 0f;
            foreach (float num3 in this.data)
            {
                num += num3;
            }
            return (num / ((float) this.data.Count));
        }

        public int NumberBelow(float level)
        {
            int num = 0;
            foreach (float num2 in this.data)
            {
                if (num2 < level)
                {
                    num++;
                }
            }
            return num;
        }

        public int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return (this.data.Count == 0);
            }
        }

        public int MaxEntries
        {
            get
            {
                return this.entriesToKeep;
            }
        }
    }
}

