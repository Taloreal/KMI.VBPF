namespace KMI.Biz.City
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Traffic
    {
        protected PointF beginLane1;
        protected PointF beginLane1W;
        protected PointF beginLane2;
        protected PointF beginLane2W;
        protected float[,] density;
        protected PointF endLane1;
        protected PointF endLane1W;
        protected PointF endLane2;
        protected PointF endLane2W;
        protected int startPos;

        public Traffic(bool isAvenue, CityBlock block)
        {
            float num;
            this.density = new float[0x18, 3];
            this.startPos = S.State.Random.Next(0x3e8);
            if (!isAvenue)
            {
                this.beginLane1 = new PointF(78f, -6f);
                num = block.NumLots * KMI.Biz.City.City.LOT_SPACING;
                this.endLane1 = new PointF(this.beginLane1.X + num, this.beginLane1.Y - (num / 2f));
                this.beginLane2 = new PointF(this.endLane1.X - 10f, this.endLane1.Y - 5f);
                this.endLane2 = new PointF(this.beginLane1.X - 10f, this.beginLane1.Y - 5f);
            }
            else
            {
                this.beginLane1 = new PointF(53f, -11f);
                this.endLane1 = new PointF(this.beginLane1.X - KMI.Biz.City.City.LOT_SPACING, this.beginLane1.Y - (KMI.Biz.City.City.LOT_SPACING / 2f));
                this.beginLane2 = new PointF(this.endLane1.X - 10f, this.endLane1.Y + 5f);
                this.endLane2 = new PointF(this.beginLane1.X - 10f, this.beginLane1.Y + 5f);
            }
            if (!isAvenue)
            {
                this.beginLane1W = new PointF(26f, -2f);
                num = block.NumLots * KMI.Biz.City.City.LOT_SPACING;
                this.endLane1W = new PointF(this.beginLane1W.X + (num / 3f), this.beginLane1W.Y - ((num / 3f) / 2f));
                this.beginLane2W = new PointF(this.endLane1W.X - 3f, this.endLane1W.Y - 2f);
                this.endLane2W = new PointF(this.beginLane1W.X - 3f, this.beginLane1W.Y - 2f);
            }
            else
            {
                this.beginLane1W = new PointF(17f, -4f);
                this.endLane1W = new PointF(this.beginLane1W.X - (KMI.Biz.City.City.LOT_SPACING / 3f), this.beginLane1W.Y - ((KMI.Biz.City.City.LOT_SPACING / 3f) / 2f));
                this.beginLane2W = new PointF(this.endLane1W.X - 3f, this.endLane1W.Y + 2f);
                this.endLane2W = new PointF(this.beginLane1W.X - 3f, this.beginLane1W.Y + 2f);
            }
        }

        public float GetDensity()
        {
            float num = 0f;
            for (int i = 0; i <= this.density.GetUpperBound(0); i++)
            {
                num += this.GetDensity(i);
            }
            return num;
        }

        public float GetDensity(int hour)
        {
            float num = 0f;
            for (int i = 0; i <= this.density.GetUpperBound(1); i++)
            {
                num += this.density[hour, i];
            }
            return num;
        }

        public float GetDensity(int hour, int personType)
        {
            return this.density[hour, personType];
        }

        public ArrayList GetDrawables(CityBlock block, int centerAvenue, int centerStreet)
        {
            PointF tf = KMI.Biz.City.City.Transform2((float)block.Avenue, (float)block.Street, (float)block.NumLots, centerAvenue, centerStreet);
            ArrayList list = new ArrayList();
            float density = this.GetDensity(S.State.Hour);
            if (density >= 0.001f)
            {
                float spacing = 4f / density;
                list.Add(new TrafficDrawable(Utilities.AddPointFs(tf, this.beginLane1), Utilities.AddPointFs(tf, this.endLane1), ((float)(S.State.FrameCounter + this.startPos)) % spacing, spacing, true));
                list.Add(new TrafficDrawable(Utilities.AddPointFs(tf, this.beginLane2), Utilities.AddPointFs(tf, this.endLane2), ((float)(S.State.FrameCounter + this.startPos)) % spacing, spacing, true));
            }
            return list;
        }

        public ArrayList GetDrawablesWholeCity(CityBlock block)
        {
            PointF tf = KMI.Biz.City.City.TransformWholeCity((float)block.Avenue, (float)block.Street, (float)block.NumLots);
            ArrayList list = new ArrayList();
            float density = this.GetDensity(S.State.Hour);
            if (density >= 0.001f)
            {
                float spacing = 1.333333f / density;
                list.Add(new TrafficDrawable(Utilities.AddPointFs(tf, this.beginLane1W), Utilities.AddPointFs(tf, this.endLane1W), ((float)(S.State.FrameCounter + this.startPos)) % spacing, spacing, false));
                list.Add(new TrafficDrawable(Utilities.AddPointFs(tf, this.beginLane2W), Utilities.AddPointFs(tf, this.endLane2W), ((float)(S.State.FrameCounter + this.startPos)) % spacing, spacing, false));
            }
            return list;
        }

        public void IncrementDensity(int hour, int personType, float amount)
        {
            this.density[hour, personType] += amount;
            if (this.density[hour, personType] < 0f)
            {
                this.density[hour, personType] = 0f;
            }
            if (this.density[hour, personType] > 1f)
            {
                this.density[hour, personType] = 1f;
            }
        }
    }
}