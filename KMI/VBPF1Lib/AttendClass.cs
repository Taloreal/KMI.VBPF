namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Drawing;

    [Serializable]
    public class AttendClass : Task
    {
        private int chair;
        public string ClassEndDate = "";
        public KMI.VBPF1Lib.Course Course;
        public int daysInCourse = 0;
        public States State = States.Init;

        public override string CategoryName()
        {
            if ((this.ClassEndDate == "") || (this.ClassEndDate == null))
            {
                this.ClassEndDate = this.FindEndDate();
            }
            return A.Resources.GetString(this.Course.Name + " Ends: \r\n" + this.ClassEndDate);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            this.State = States.Init;
        }

        public override bool Do()
        {
            switch (this.State)
            {
                case States.Init:
                    if (!base.Building.Persons.Contains(base.Owner))
                    {
                        base.Building.Persons.Add(base.Owner);
                    }
                    this.chair = this.Course.Students.IndexOf(base.Owner);
                    base.Owner.Location = (PointF) base.Building.Map.getNode("EntryPoint").Location;
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Chair" + this.chair).ToPoints();
                    this.State = States.ToChair;
                    goto Label_032D;

                case States.ToChair:
                    if (base.Owner.Move())
                    {
                        this.State = States.AtChair;
                        base.Owner.Pose = "SitNE";
                    }
                    goto Label_032D;

                case States.AtChair:
                    if (A.State.Period == base.EndPeriod)
                    {
                        base.Owner.Path = base.Building.Map.findPath("Chair" + this.chair, "EntryPoint").ToPoints();
                        base.Owner.Pose = "Walk";
                        this.State = States.FromChair;
                        this.daysInCourse++;
                    }
                    goto Label_032D;

                case States.FromChair:
                    base.Owner.Location = (PointF) base.Building.Map.getNode("EntryPoint").Location;
                    if (base.Owner.Drone)
                    {
                        DateTime time = A.State.Now.AddDays(1.0);
                        base.Owner.WakeupTime = new DateTime(time.Year, time.Month, time.Day).AddHours((((float) base.StartPeriod) / 2f) - (0.20000000298023224 + (0.10000000149011612 * A.State.Random.NextDouble())));
                        base.Building.Persons.Remove(base.Owner);
                        this.State = States.Init;
                        break;
                    }
                    if (this.daysInCourse >= this.Course.Days)
                    {
                        this.Course.Students.Remove(base.Owner);
                        AppEntity appEntity = base.GetAppEntity();
                        A.Adapter.DeleteTask(appEntity.ID, base.ID);
                        appEntity.AcademicTaskHistory.Add(base.ID, this);
                        object[] args = new object[] { this.Course.Name };
                        appEntity.Player.SendMessage(A.Resources.GetString("Congratulations! You completed your course: {0}. A diploma will now appear on your wall.", args), "", NotificationColor.Green);
                    }
                    break;

                default:
                    goto Label_032D;
            }
            return true;
        Label_032D:
            return false;
        }

        public string FindEndDate()
        {
            DateTime now = A.State.Now;
            int num = this.Course.Days - this.daysInCourse;
            while (num > 0)
            {
                if (this.Weekday(now) == !base.Weekend)
                {
                    num--;
                }
                now = now.AddDays(1.0);
            }
            object[] objArray1 = new object[] { now.Month, "/", now.Day, "/", now.Year.ToString().Substring(2) };
            return string.Concat(objArray1);
        }

        public override Color GetColor()
        {
            return Color.LightCoral;
        }

        private bool Weekday(DateTime ToTest)
        {
            return ((ToTest.DayOfWeek != DayOfWeek.Sunday) && (ToTest.DayOfWeek != DayOfWeek.Saturday));
        }

        public enum States
        {
            Init,
            ToChair,
            AtChair,
            FromChair
        }
    }
}

