namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Drawing;

    [Serializable]
    public class Eat : Task
    {
        private States State = States.Init;

        public override string CategoryName()
        {
            return A.Resources.GetString("Eat");
        }

        public override void CleanUp()
        {
            base.CleanUp();
            this.State = States.Init;
        }

        public override bool Do()
        {
            if (A.State.Period == base.EndPeriod)
            {
                base.Owner.Location = (PointF) base.Building.Map.getNode("Eat").Location;
                base.Owner.Pose = "StandSE";
                return true;
            }
            switch (this.State)
            {
                case States.Init:
                    base.Owner.Pose = "Walk";
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Eat").ToPoints();
                    this.State = States.ToEat;
                    break;

                case States.ToEat:
                    if (base.Owner.Move())
                    {
                        PointF location = (PointF) base.Building.Map.getNode("Eat").Location;
                        base.Owner.Location = new PointF(location.X - 50f, location.Y + 5f);
                        this.State = States.Eat;
                    }
                    break;

                case States.Eat:
                {
                    AppEntity appEntity = base.GetAppEntity();
                    if (appEntity.Food.Count > 0)
                    {
                        base.Owner.Pose = "EatSE";
                        break;
                    }
                    base.Owner.Pose = "SitSE";
                    appEntity.Player.SendPeriodicMessage(A.Resources.GetString("You tried to eat, but there's no more food in fridge!"), "", NotificationColor.Yellow, 1f);
                    break;
                }
            }
            return false;
        }

        public override bool Process(AppEntity entity) {
            if ((entity.Person.Task is Eat) && (entity.Food.Count > 0)) {
                TimeSpan span = (TimeSpan)(A.State.Now - entity.timeLastAte);
                if (span.Hours > 3) {
                    entity.TodaysHealth[0]++;
                    entity.timeLastAte = A.State.Now;
                    if (!entity.Food[0].Eat()) { entity.Food.RemoveAt(0); }
                }
            } 
            else { return base.Process(entity); }
            return true;
        }

        public override Color GetColor()
        {
            return Color.LightGreen;
        }

        private enum States
        {
            Init,
            ToEat,
            Eat
        }
    }
}

