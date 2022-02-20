namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class TravelByFoot : TravelTask
    {
        public KMI.VBPF1Lib.Pedestrian Pedestrian = new KMI.VBPF1Lib.Pedestrian();
        private States State = States.Init;

        public override string CategoryName()
        {
            return A.Resources.GetString("Travel by Foot");
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
                    this.Pedestrian.SetLocation(base.From);
                    base.From.Persons.Remove(base.Owner);
                    A.State.City.Pedestrians.Add(this.Pedestrian);
                    this.Pedestrian.SendTo(base.To);
                    this.State = States.Drive;
                    break;

                case States.Drive:
                    if (!this.Pedestrian.NewStep())
                    {
                        break;
                    }
                    base.Owner.Pose = "StandSW";
                    base.To.Persons.Add(base.Owner);
                    A.State.City.Pedestrians.Remove(this.Pedestrian);
                    if (base.To.Map != null)
                    {
                        base.Owner.Location = (PointF) base.To.Map.getNode("EntryPoint").Location;
                    }
                    return true;
            }
            return false;
        }

        public override bool Process(AppEntity entity) {
            if (entity.Person.Task is TravelByFoot) {
                entity.TodaysHealth[2] += 0.5f;
            }
            else { return base.Process(entity); }
            return true;
        }

        public override int EstTimeInSteps(AppBuilding from, AppBuilding to)
        {
            return (KMI.VBPF1Lib.Pedestrian.EstTimeInSteps(from.Avenue, from.Street, from.Lot, to.Avenue, to.Street, to.Lot) + 4);
        }

        private enum States
        {
            Init,
            ToDoor,
            Drive
        }
    }
}

