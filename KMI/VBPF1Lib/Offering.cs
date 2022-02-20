namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;

    [Serializable]
    public abstract class Offering
    {
        protected long buildingID;
        public long ID = A.State.GetNextID();
        public Task PrototypeTask;
        public bool Taken;

        protected Offering()
        {
        }

        public Task CreateTask()
        {
            Task task = (Task) Utilities.DeepCopyBySerialization(this.PrototypeTask);
            task.ID = A.State.GetNextID();
            task.StartDate = A.State.Now;
            return task;
        }

        public virtual string Description()
        {
            return null;
        }

        public virtual string JournalEntry()
        {
            return "";
        }

        public virtual string ThingName()
        {
            return null;
        }

        public AppBuilding Building
        {
            get
            {
                return A.State.City.BuildingByID(this.buildingID);
            }
            set
            {
                this.buildingID = value.ID;
            }
        }
    }
}

