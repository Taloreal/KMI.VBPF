namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class Relax : Task
    {
        private States State = States.Init;

        public override string CategoryName()
        {
            return A.Resources.GetString("Relax");
        }

        public override void CleanUp()
        {
            base.CleanUp();
            base.Owner.Location = (PointF) base.Building.Map.getNode("Couch").Location;
            base.Owner.Pose = "StandSW";
            this.State = States.Init;
        }

        public override bool Do()
        {
            switch (this.State)
            {
                case States.Init:
                    base.Owner.Pose = "Walk";
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Couch").ToPoints();
                    this.State = States.ToCouch;
                    break;

                case States.ToCouch:
                    if (base.Owner.Move())
                    {
                        PointF location = (PointF) base.Building.Map.getNode("Couch").Location;
                        if (!base.GetAppEntity().Has("Couch"))
                        {
                            base.Owner.Pose = "StandSW";
                        }
                        else
                        {
                            base.Owner.Location = new PointF(location.X + 12f, location.Y - 6f);
                            base.Owner.Pose = "SitSW";
                        }
                        this.State = States.Relax;
                        break;
                    }
                    break;

                case States.Relax:
                    if (A.State.Period == base.EndPeriod)
                    {
                        base.Owner.Location = (PointF) base.Building.Map.getNode("Couch").Location;
                        base.Owner.Pose = "StandSW";
                        return true;
                    }
                    break;
            }
            return false;
        }

        public override bool Process(AppEntity entity) {
            if (entity.Person.Task is Relax) {
                float enjoyment = 0.4f;
                if (entity.Has("TV")) { enjoyment += 0.2f; }
                if (entity.Has("Couch")) { enjoyment += 0.4f; }
                entity.TodaysHealth[3] += enjoyment;
            }
            else { return base.Process(entity); }
            return true;
        }

        public override Color GetColor()
        {
            return Color.LightBlue;
        }

        private enum States
        {
            Init,
            ToCouch,
            Relax
        }
    }
}

