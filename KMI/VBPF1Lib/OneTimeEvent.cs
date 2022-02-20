namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Drawing;

    [Serializable]
    public class OneTimeEvent : Task
    {
        public long HostID = -1L;
        protected DateTime key = DateTime.MinValue;
        public float rnd = 1f;

        public void AddAIAttendees(int number)
        {
            for (int i = 0; i < number; i++)
            {
                VBPFPerson activeObject = new VBPFPerson {
                    Task = (Task) Utilities.DeepCopyBySerialization(this)
                };
                activeObject.Task.ID = A.State.GetNextID();
                activeObject.Task.Owner = activeObject;
                activeObject.Location = (PointF) activeObject.Task.Building.Map.getNode("EntryPoint").Location;
                DateTime wakeupTime = this.OneTimeDay.AddHours((((float) base.StartPeriod) / 2f) + (A.State.Random.NextDouble() - 0.5));
                A.Instance.Subscribe(activeObject, wakeupTime);
            }
        }

        public override string ToString()
        {
            DateTime oneTimeDay = base.OneTimeDay;
            object[] args = new object[] { this.CategoryName(), oneTimeDay.ToShortDateString(), Task.ToTimeString(base.StartPeriod), Task.ToTimeString(base.EndPeriod) };
            return A.Resources.GetString("{0} on {1} from {2} to {3}", args);
        }

        public DateTime Key
        {
            get
            {
                if (this.key == DateTime.MinValue)
                {
                    this.key = new DateTime(this.OneTimeDay.Year, this.OneTimeDay.Month, this.OneTimeDay.Day).AddMinutes(A.State.Random.NextDouble());
                }
                return this.key;
            }
        }
    }
}

