namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;

    [Serializable]
    public class Course : Offering
    {
        public float Cost;
        public int Days;
        public string Name;
        public string Prerequisite = null;
        public string ResumeDescription;
        public ArrayList Students = new ArrayList();
        public string TeacherGender;

        public Course()
        {
            if (A.State.Random.Next(2) == 0)
            {
                this.TeacherGender = "Female";
            }
            else
            {
                this.TeacherGender = "Male";
            }
        }

        public override string Description()
        {
            DateTime time = new DateTime(0x7d0, 1, 1);
            string str = A.Resources.GetString("Mon. - Fri.");
            int num = this.Days / 5;
            if (base.PrototypeTask.Weekend)
            {
                str = A.Resources.GetString("Sat. & Sun.");
                num = this.Days / 2;
            }
            object[] args = new object[] { this.Name.ToUpper(), time.AddHours((double) (((float) base.PrototypeTask.StartPeriod) / 2f)).ToShortTimeString(), time.AddHours((double) (((float) base.PrototypeTask.EndPeriod) / 2f)).ToShortTimeString(), str, Utilities.FC(this.Cost, A.Instance.CurrencyConversion), num };
            string str2 = A.Resources.GetString("{0}|Hours: {1} to {2}|Days: {3}|Cost: {4}|Length: {5} weeks", args);
            if (this.Prerequisite != null)
            {
                object[] objArray2 = new object[] { this.Prerequisite };
                str2 = str2 + A.Resources.GetString("|Prerequisite: {0}", objArray2);
            }
            return str2.Replace("|", Environment.NewLine);
        }

        public override string JournalEntry()
        {
            object[] args = new object[] { this.Name, Utilities.FC(this.Cost, A.Instance.CurrencyConversion) };
            return A.Resources.GetString("Enrolled in course, {0}, at a cost of {1}.", args);
        }

        public override string ThingName()
        {
            return A.Resources.GetString("Courses");
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

