namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class TravelByCar : TravelTask
    {
        public KMI.VBPF1Lib.Car Car;
        private States State = States.Init;

        public override string CategoryName()
        {
            return A.Resources.GetString("Travel by Car");
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
                    base.From.Persons.Remove(base.Owner);
                    this.Car.SendTo(base.To);
                    this.State = States.Drive;
                    break;

                case States.Drive:
                    if (!this.Car.NewStep())
                    {
                        break;
                    }
                    base.Owner.Pose = "StandSW";
                    base.To.Persons.Add(base.Owner);
                    if (base.To.Map != null)
                    {
                        base.Owner.Location = (PointF) base.To.Map.getNode("EntryPoint").Location;
                    }
                    return true;
            }
            return false;
        }

        public override int EstTimeInSteps(AppBuilding from, AppBuilding to)
        {
            return (KMI.VBPF1Lib.Car.EstTimeInSteps(from.Avenue, from.Street, from.Lot, to.Avenue, to.Street, to.Lot) + 4);
        }

        private enum States
        {
            Init,
            ToDoor,
            Drive
        }
    }
}

