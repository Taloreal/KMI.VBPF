namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Pedestrian : TurboMovableActiveObject
    {
        private int count = 0;
        public const float SpeedOnStreet = 0.01f;

        public static int EstTimeInSteps(int fromAvenue, int fromStreet, int fromLot, int toAvenue, int toStreet, int toLot)
        {
            int num = 0;
            if (toStreet == fromStreet)
            {
                return (int) (Math.Abs((float) (TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X - TravelTask.DoorWayAt(toAvenue, toStreet, toLot).X)) / 0.01f);
            }
            int num2 = toAvenue;
            if (toAvenue > TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X)
            {
                num2 = toAvenue - 1;
            }
            num = (int) (Math.Abs((float) (TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X - num2)) / 0.01f);
            num += (int) (((float) Math.Abs((int) (fromStreet - toStreet))) / 0.02f);
            return (num + ((int) (Math.Abs((float) (TravelTask.DoorWayAt(toAvenue, toStreet, toLot).X - num2)) / 0.01f)));
        }

        public Drawable GetDrawable(int aveRegion, int streetRegion)
        {
            PointF tf = KMI.Biz.City.City.Transform2(base.Location.X, base.Location.Y, 2f, aveRegion, streetRegion);
            return new Drawable(new PointF(tf.X + 23f, tf.Y + 5f), "Walker", "Ave:" + base.Location.X.ToString() + " St:" + base.Location.Y.ToString());
        }

        public bool InRegion(int aveRegion, int streetRegion)
        {
            int num = (int) ((base.Location.X + 0.7f) / ((float) (KMI.Biz.City.City.NUM_AVENUES / KMI.Biz.City.City.AVENUE_VIEWING_REGIONS)));
            int num2 = (int) ((base.Location.Y + 0.4f) / ((float) (KMI.Biz.City.City.NUM_STREETS / KMI.Biz.City.City.STREET_VIEWING_REGIONS)));
            return ((num == aveRegion) && (num2 == streetRegion));
        }

        public override bool NewStep()
        {
            if (base.DX == 0f)
            {
                base.Speed = 0.04f;
                this.count++;
            }
            else
            {
                base.Speed = 0.01f;
            }
            bool flag = this.Move();
            if (flag)
            {
                A.Instance.UnSubscribe(this);
                this.count = 0;
            }
            return flag;
        }

        public void SendTo(AppBuilding bldg)
        {
            this.SendTo(bldg.Avenue, bldg.Street, bldg.Lot);
        }

        public void SendTo(int ave, int street, int lot)
        {
            if (street == base.Location.Y)
            {
                base.Destination = TravelTask.DoorWayAt(ave, street, lot);
            }
            else
            {
                int num = ave;
                if (ave > base.Location.X)
                {
                    num = ave - 1;
                }
                PointF[] c = new PointF[] { new PointF((float) num, base.Location.Y), new PointF((float) num, (float) street), TravelTask.DoorWayAt(ave, street, lot) };
                base.Path = new ArrayList(c);
            }
        }

        public void SetLocation(AppBuilding bldg)
        {
            this.SetLocation(bldg.Avenue, bldg.Street, bldg.Lot);
        }

        public void SetLocation(int ave, int street, int lot)
        {
            base.Location = TravelTask.DoorWayAt(ave, street, lot);
        }
    }
}

