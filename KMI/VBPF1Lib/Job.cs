namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;

    [Serializable]
    public class Job : Offering
    {
        public float WeeklyPay;

        public Job()
        {
            this.WeeklyPay = 200f;
        }

        public Job(AppBuilding building, Task task, int startPeriod, int endPeriod)
        {
            this.WeeklyPay = 200f;
            base.Building = building;
            base.PrototypeTask = task;
            base.PrototypeTask.StartPeriod = startPeriod;
            base.PrototypeTask.EndPeriod = endPeriod;
        }

        public override string Description()
        {
            return base.PrototypeTask.Description();
        }

        public override string JournalEntry()
        {
            object[] args = new object[] { this.ToString(), Utilities.FC(this.WeeklyPay, 2, A.Instance.CurrencyConversion) };
            return A.Resources.GetString("Got job as a {0}, paying {1} per week.", args);
        }

        public override string ThingName()
        {
            return A.Resources.GetString("Jobs");
        }

        public override string ToString()
        {
            WorkTask prototypeTask = (WorkTask) base.PrototypeTask;
            return prototypeTask.Name();
        }
    }
}

