namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class TravelTask : Task
    {
        public AppBuilding From;
        public AppBuilding To;

        public static TravelTask CreateTravelTask(int modeOfTransportation)
        {
            if (modeOfTransportation == 1)
            {
                return new TravelByBus();
            }
            if (modeOfTransportation == 2)
            {
                return new TravelByCar();
            }
            return new TravelByFoot();
        }

        public static PointF DoorWayAt(int ave, int street, int lot)
        {
            return new PointF(ave - (0.25f * (3 - lot)), (float) street);
        }

        public virtual int EstTimeInSteps(AppBuilding from, AppBuilding to)
        {
            throw new NotImplementedException();
        }

        public override Color GetColor()
        {
            return Color.Violet;
        }
    }
}

