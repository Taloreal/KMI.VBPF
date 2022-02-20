namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class Exercise : Task
    {
        private States State = States.Init;

        public override string CategoryName()
        {
            return A.Resources.GetString("Exercise");
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
                    base.Owner.Pose = "Walk";
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "TreadMillWalk").ToPoints();
                    this.State = States.ToTreadMill;
                    break;

                case States.ToTreadMill:
                    if (base.Owner.Move())
                    {
                        if (!base.GetAppEntity().Has("TreadMill"))
                        {
                            PointF location = (PointF) base.Building.Map.getNode("TreadMillWalk").Location;
                            base.Owner.Location = new PointF(location.X, location.Y + 24f);
                            base.Owner.Pose = "JumpingJacksSW";
                        }
                        this.State = States.Exercise;
                    }
                    break;

                case States.Exercise:
                    if (A.State.Period != base.EndPeriod)
                    {
                        break;
                    }
                    base.Owner.Location = (PointF) base.Building.Map.getNode("TreadMillWalk").Location;
                    base.Owner.Pose = "StandSW";
                    return true;
            }
            return false;
        }

        public override bool Process(AppEntity entity) {
            if (entity.Person.Task is Exercise) {
                if (entity.Has("TreadMill")) { entity.TodaysHealth[2]++; }
                else { entity.TodaysHealth[2] += 0.5f; }
            }
            else { return base.Process(entity); }
            return true;
        }

        public override Color GetColor()
        {
            return Color.Orange;
        }

        private enum States
        {
            Init,
            ToTreadMill,
            Exercise
        }
    }
}

