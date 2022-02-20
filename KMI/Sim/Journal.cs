namespace KMI.Sim
{
    using System;
    using System.Collections;

    [Serializable]
    public class Journal
    {
        protected float daysPerPeriod;
        protected string entityName;
        protected ArrayList entries = new ArrayList();
        protected static int journalDaysPerPeriod = 7;
        protected static string[] journalNumericDataSeriesNames = new string[] { "Profit", "Cumulative Profit" };
        protected static string journalSeriesName = "Profit";
        protected Hashtable numericDataSeries = new Hashtable();
        protected string[] numericDataSeriesNames;
        protected static string scoreSeriesName = "Cumulative Profit";

        public Journal(string entityName, string[] numericDataSeriesNames, float daysPerPeriod)
        {
            this.entityName = entityName;
            this.numericDataSeriesNames = numericDataSeriesNames;
            for (int i = 0; i < numericDataSeriesNames.Length; i++)
            {
                this.numericDataSeries.Add(numericDataSeriesNames[i], new ArrayList());
            }
            this.daysPerPeriod = daysPerPeriod;
        }

        public void AddEntry(string description)
        {
            this.AddEntry(Simulator.Instance.SimState.Now, description, new FlagsAttribute());
        }

        public void AddEntry(DateTime timeStamp, string description)
        {
            this.AddEntry(timeStamp, description, new FlagsAttribute());
        }

        public void AddEntry(string description, FlagsAttribute flags)
        {
            this.AddEntry(Simulator.Instance.SimState.Now, description, flags);
        }

        public void AddEntry(DateTime timeStamp, string description, FlagsAttribute flags)
        {
            if (!S.MainForm.DesignerMode)
            {
                JournalEntry entry = new JournalEntry(timeStamp, this.entityName, description, flags);
                for (int i = this.entries.Count - 1; i >= -1; i--)
                {
                    if (i == -1)
                    {
                        this.entries.Insert(0, entry);
                        break;
                    }
                    if (entry.TimeStamp >= ((JournalEntry) this.entries[i]).TimeStamp)
                    {
                        this.entries.Insert(i + 1, entry);
                        break;
                    }
                }
            }
        }

        public void AddNumericData(string seriesName, float amount)
        {
            ((ArrayList) this.numericDataSeries[seriesName]).Add(amount);
        }

        public ArrayList NumericDataSeries(string seriesName)
        {
            return (ArrayList) this.numericDataSeries[seriesName];
        }

        public float NumericDataSeriesLastEntry(string seriesName)
        {
            ArrayList list = (ArrayList) this.numericDataSeries[seriesName];
            if (list.Count == 0)
            {
                return 0f;
            }
            return (float) list[list.Count - 1];
        }

        public int DataSeriesCount
        {
            get
            {
                return this.numericDataSeries.Count;
            }
        }

        public float DaysPerPeriod
        {
            get
            {
                return this.daysPerPeriod;
            }
        }

        public string EntityName
        {
            get
            {
                return this.entityName;
            }
        }

        public ArrayList Entries
        {
            get
            {
                return this.entries;
            }
        }

        public static int JournalDaysPerPeriod
        {
            get
            {
                return journalDaysPerPeriod;
            }
            set
            {
                journalDaysPerPeriod = value;
            }
        }

        public static string[] JournalNumericDataSeriesNames
        {
            get
            {
                return journalNumericDataSeriesNames;
            }
            set
            {
                journalNumericDataSeriesNames = value;
            }
        }

        public static string JournalSeriesName
        {
            get
            {
                return journalSeriesName;
            }
            set
            {
                journalSeriesName = value;
            }
        }

        public string[] NumericDataSeriesNames
        {
            get
            {
                return this.numericDataSeriesNames;
            }
        }

        public int Periods
        {
            get
            {
                return (this.numericDataSeries[this.numericDataSeriesNames[0]] as ArrayList).Count;
            }
        }

        public static string ScoreSeriesName
        {
            get
            {
                return scoreSeriesName;
            }
            set
            {
                scoreSeriesName = value;
            }
        }
    }
}

