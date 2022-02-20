namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class Bus : TurboMovableActiveObject
    {
        protected int avenue;
        protected int counter;
        protected int direction = 1;
        protected bool onAvenue;
        public const float SpeedOnStreet = 0.13f;
        public States State = States.AtStop;
        private const int StopEvery = 2;
        private const int StopTime = 2;
        protected int street;

        public Bus(int avenue, int street, bool onAvenue, int direction)
        {
            base.Location = new PointF((float) avenue, street + ((float) (0.6 * (A.State.Random.NextDouble() - 0.5))));
            base.Destination = this.GetNextDestination();
            this.street = street;
            this.avenue = avenue;
            this.onAvenue = onAvenue;
            this.direction = direction;
            base.Speed = 0.52f;
            S.Instance.Subscribe(this, Simulator.TimePeriod.Step);
        }

        public static int EstTimeInSteps(int fromAvenue, int fromStreet, int fromLot, int toAvenue, int toStreet, int toLot)
        {
            int num = 0;
            int num2 = Math.Abs((int) (fromStreet - toStreet));
            num += (int) (((float) num2) / 0.26f);
            return (num + (2 * num2));
        }

        public Drawable GetDrawable(int aveRegion, int streetRegion)
        {
            PointF tf = KMI.Biz.City.City.Transform2(base.Location.X, base.Location.Y, 2f, aveRegion, streetRegion);
            string str = "SE";
            if (base.DY < 0f)
            {
                str = "NW";
            }
            return new Drawable(new PointF(tf.X - 5f, tf.Y - 13f), "Bus" + str);
        }

        public PointF GetNextDestination()
        {
            if (this.onAvenue)
            {
                int num2 = this.street + this.direction;
                if ((num2 < 0) || (num2 >= KMI.Biz.City.City.NUM_STREETS))
                {
                    this.direction = -this.direction;
                    num2 = this.street + this.direction;
                }
                this.street = num2;
                return new PointF((float) this.avenue, (float) this.street);
            }
            int num = this.avenue + this.direction;
            if ((num < 0) || (num >= KMI.Biz.City.City.NUM_AVENUES))
            {
                this.direction = -this.direction;
                num = this.avenue + this.direction;
            }
            this.avenue = num;
            return new PointF((float) this.avenue, (float) this.street);
        }

        public bool InRegion(int aveRegion, int streetRegion)
        {
            int num = (int) ((base.Location.X + 0.7f) / ((float) (KMI.Biz.City.City.NUM_AVENUES / KMI.Biz.City.City.AVENUE_VIEWING_REGIONS)));
            int num2 = (int) ((base.Location.Y + 0.4f) / ((float) (KMI.Biz.City.City.NUM_STREETS / KMI.Biz.City.City.STREET_VIEWING_REGIONS)));
            return ((num == aveRegion) && (num2 == streetRegion));
        }

        public override bool NewStep()
        {
            switch (this.State)
            {
                case States.AtStop:
                    this.counter -= A.State.SimulatedTimePerStep / 0x4e20;
                    if (this.counter <= 0)
                    {
                        base.Destination = this.GetNextDestination();
                        this.State = States.Enroute;
                    }
                    break;

                case States.Enroute:
                    if (this.Move())
                    {
                        this.counter = 2;
                        this.State = States.AtStop;
                    }
                    break;
            }
            return false;
        }

        public enum States
        {
            AtStop,
            Enroute
        }
    }
}

