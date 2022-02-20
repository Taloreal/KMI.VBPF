namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Car : TurboMovableActiveObject
    {
        public bool Broken = false;
        public float Gas = 0f;
        public InsurancePolicy Insurance = new InsurancePolicy(250f, false, 1000000f);
        public DateTime LastTuneup = A.State.Now;
        public float LeaseCost = 0f;
        public InstallmentLoan Loan;
        public float OriginalPrice;
        public DateTime Purchased;
        public const float SpeedOnStreet = 0.08f;

        public Car()
        {
            this.Gas = 20f;
        }

        public int CarIndex()
        {
            if (A.State.Reserved[this] != null)
            {
                return (int) A.State.Reserved[this];
            }
            return 2;
        }

        public float ComputeResalePrice(DateTime now)
        {
            TimeSpan span = (TimeSpan) (now - this.Purchased);
            float days = span.Days;
            return Math.Max((float) 0f, (float) (this.OriginalPrice * (1f - (days / 2920f))));
        }

        public static int EstTimeInSteps(int fromAvenue, int fromStreet, int fromLot, int toAvenue, int toStreet, int toLot)
        {
            int num = 0;
            if (toStreet == fromStreet)
            {
                return (int) (Math.Abs((float) (TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X - TravelTask.DoorWayAt(toAvenue, toStreet, toLot).X)) / 0.08f);
            }
            int num2 = toAvenue;
            if (toAvenue > TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X)
            {
                num2 = toAvenue - 1;
            }
            num = (int) (Math.Abs((float) (TravelTask.DoorWayAt(fromAvenue, fromStreet, fromLot).X - num2)) / 0.08f);
            num += (int) (((float) Math.Abs((int) (fromStreet - toStreet))) / 0.16f);
            return (num + ((int) (Math.Abs((float) (TravelTask.DoorWayAt(toAvenue, toStreet, toLot).X - num2)) / 0.08f)));
        }

        public Drawable GetDrawable(int aveRegion, int streetRegion)
        {
            string str;
            PointF location = KMI.Biz.City.City.Transform2(base.Location.X, base.Location.Y, 2f, aveRegion, streetRegion);
            location = new PointF(location.X + 23f, location.Y + 5f);
            if (!(base.DY == 0f))
            {
                str = "NWSE";
            }
            else
            {
                str = "NESW";
            }
            Drawable drawable = new Drawable(location, "Car" + str);
            object[] args = new object[] { this.Gas.ToString("N1") };
            drawable.ToolTipText = A.Resources.GetString("Gas Remaining: {0} gals.", args);
            return drawable;
        }

        public bool InRegion(int aveRegion, int streetRegion)
        {
            int num = (int) ((base.Location.X + 0.7f) / ((float) (KMI.Biz.City.City.NUM_AVENUES / KMI.Biz.City.City.AVENUE_VIEWING_REGIONS)));
            int num2 = (int) ((base.Location.Y + 0.4f) / ((float) (KMI.Biz.City.City.NUM_STREETS / KMI.Biz.City.City.STREET_VIEWING_REGIONS)));
            return ((num == aveRegion) && (num2 == streetRegion));
        }

        public float LikelihoodOfBreakDown()
        {
            TimeSpan span = (TimeSpan) (A.State.Now - this.LastTuneup);
            int num = span.Days - 120;
            span = (TimeSpan) (A.State.Now - this.Purchased);
            int days = span.Days;
            float num3 = ((float) num) / 245f;
            return (Math.Max(num3, ((float) (days - 730)) / 4380f) * (50000f / Math.Min(35000f, this.OriginalPrice)));
        }

        public override bool NewStep()
        {
            float num = A.State.SimulatedTimePerStep / 0x4e20;
            this.Gas -= ((0.003f * num) * 20f) / AppConstants.MPGs[this.CarIndex()];
            if (this.Gas < 0f)
            {
                this.Gas = 0f;
            }
            if (base.DX == 0f)
            {
                base.Speed = 0.32f;
            }
            else
            {
                base.Speed = 0.08f;
            }
            bool flag = this.Move();
            if (flag)
            {
                A.Instance.UnSubscribe(this);
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
                PointF[] c = new PointF[] { new PointF((float) num, base.Location.Y), new PointF((float) num, (float) street), new PointF(ave - (0.25f * (3 - lot)), (float) street) };
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

