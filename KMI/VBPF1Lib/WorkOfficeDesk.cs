namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class WorkOfficeDesk : WorkTask
    {
        public int chair;
        public States State = States.Init;
        public int typingCounter;

        public WorkOfficeDesk()
        {
            base.HourlyWage = 11.25f;
            base.AcademicExperienceRequired.Add("Intro to Data Entry");
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
                    base.Owner.Location = (PointF) base.Building.Map.getNode("EntryPoint").Location;
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Chair" + this.chair).ToPoints();
                    this.State = States.ToChair;
                    break;

                case States.ToChair:
                    if (base.Owner.Move())
                    {
                        this.State = States.AtChair;
                        base.Owner.Pose = "SitNW";
                    }
                    break;

                case States.AtChair:
                {
                    if (A.State.Period == base.EndPeriod)
                    {
                        base.Owner.Path = base.Building.Map.findPath("Chair" + this.chair, "EntryPoint").ToPoints();
                        base.Owner.Pose = "Walk";
                        this.State = States.FromChair;
                        break;
                    }
                    int typingCounter = this.typingCounter;
                    this.typingCounter = typingCounter - 1;
                    if (typingCounter <= 0)
                    {
                        if (A.State.Random.NextDouble() < ((Office) base.Building).Busyness)
                        {
                            this.typingCounter = 50;
                        }
                        base.Owner.Pose = "SitNW";
                        break;
                    }
                    base.Owner.Pose = "SitTypeNW";
                    break;
                }
                case States.FromChair:
                    base.Owner.Location = (PointF) base.Building.Map.getNode("EntryPoint").Location;
                    if (base.Owner.Drone)
                    {
                        DateTime time = A.State.Now.AddDays(1.0);
                        base.Owner.WakeupTime = new DateTime(time.Year, time.Month, time.Day).AddHours((((float) base.StartPeriod) / 2f) - (0.20000000298023224 + (0.10000000149011612 * A.State.Random.NextDouble())));
                        base.Building.Persons.Remove(base.Owner);
                        this.State = States.Init;
                    }
                    return true;
            }
            return false;
        }

        public override string Name()
        {
            return "Data Entry Specialist";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Responsible for accurate and timely entry of all data records.");
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

