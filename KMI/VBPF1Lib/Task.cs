namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Task
    {
        public DateTime ArrivedToday = DateTime.MinValue;
        protected long buildingID;
        public ArrayList DatesAbsent = new ArrayList();
        public ArrayList DatesLate = new ArrayList();
        public int DayLastStarted = -1;
        public DateTime EndDate = DateTime.MinValue;
        public int EndPeriod;
        public long ID;
        public DateTime OneTimeDay = DateTime.MinValue;
        public VBPFPerson Owner;
        public DateTime StartDate;
        public int StartPeriod;
        public bool Weekend;

        public Task()
        {
            if (!S.Instance.Client)
            {
                this.ID = A.State.GetNextID();
            }
        }

        public string BadAttendance()
        {
            if (A.Settings.FireForAbsencesLateness)
            {
                string str;
                TimeSpan span;
                int num = 0;
                foreach (DateTime time in this.DatesAbsent)
                {
                    span = (TimeSpan) (A.State.Now - time);
                    if (span.Days < 30)
                    {
                        num++;
                    }
                }
                if (num > 4)
                {
                    str = A.Resources.GetString("been fired from your job");
                    if (this is AttendClass)
                    {
                        str = A.Resources.GetString("flunked out of your class");
                    }
                    object[] args = new object[] { str, 4 };
                    return A.Resources.GetString("You have {0} because you were absent more than {1} times in the last month.", args);
                }
                if (num > 2)
                {
                    this.GetAppEntity().Player.SendPeriodicMessage(A.Resources.GetString("You have been absent a lot recently. You may be fired or flunk out soon!"), "", NotificationColor.Yellow, 30f);
                }
                int num2 = 0;
                foreach (DateTime time2 in this.DatesLate)
                {
                    span = (TimeSpan) (A.State.Now - time2);
                    if (span.Days < 30)
                    {
                        num2++;
                    }
                }
                if (num2 > 4)
                {
                    str = A.Resources.GetString("been fired from your job");
                    if (this is AttendClass)
                    {
                        str = A.Resources.GetString("flunked out of your class");
                    }
                    object[] objArray2 = new object[] { str, 4 };
                    return A.Resources.GetString("You have {0} because you were late more than {1} times in the last month.", objArray2);
                }
                if (num2 > 2)
                {
                    this.GetAppEntity().Player.SendPeriodicMessage(A.Resources.GetString("You have been late a lot recently. You may be fired or flunk out soon!"), "", NotificationColor.Yellow, 30f);
                }
            }
            return null;
        }

        public virtual string CategoryName()
        {
            return null;
        }

        public virtual void CleanUp()
        {
        }

        public virtual string Description()
        {
            return null;
        }

        public virtual bool Do() {
            return true;
        }

        public virtual bool Process(AppEntity entity) {
            return false;
        }

        public AppEntity GetAppEntity()
        {
            foreach (AppEntity entity in A.State.Entity.Values)
            {
                if (entity.Person == this.Owner)
                {
                    return entity;
                }
            }
            return null;
        }

        public virtual Color GetColor()
        {
            return Color.White;
        }

        public static string ToTimeString(int period)
        {
            DateTime time = new DateTime(0x7d0, 1, 1);
            return time.AddHours((double) (((float) period) / 2f)).ToShortTimeString();
        }

        public string WeekendString()
        {
            if (this.Weekend)
            {
                return A.Resources.GetString("Weekend");
            }
            return A.Resources.GetString("Weekday");
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

        public int Duration
        {
            get
            {
                if (this.StartPeriod <= this.EndPeriod)
                {
                    return (this.EndPeriod - this.StartPeriod);
                }
                return ((this.EndPeriod + 0x30) - this.StartPeriod);
            }
        }
    }
}

