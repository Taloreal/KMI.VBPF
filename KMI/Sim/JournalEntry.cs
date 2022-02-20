namespace KMI.Sim
{
    using System;

    [Serializable]
    public class JournalEntry
    {
        protected string description;
        protected string entityName;
        protected FlagsAttribute flags;
        protected DateTime timeStamp;

        public JournalEntry(DateTime timeStamp, string entityName, string description, FlagsAttribute flags)
        {
            this.timeStamp = timeStamp;
            this.entityName = entityName;
            this.description = description;
            this.flags = flags;
        }

        public string Description
        {
            get
            {
                return this.description;
            }
        }

        public string EntityName
        {
            get
            {
                return this.entityName;
            }
        }

        public FlagsAttribute Flags
        {
            get
            {
                return this.flags;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return this.timeStamp;
            }
        }
    }
}

