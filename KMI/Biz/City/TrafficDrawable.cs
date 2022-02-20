namespace KMI.Biz.City
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class TrafficDrawable : Drawable
    {
        protected PointF begin;
        protected PointF end;
        protected float firstCar;
        protected float spacing;
        protected bool zoomed;

        public TrafficDrawable(PointF begin, PointF end, float firstCar, float spacing, bool zoomed) : base(begin, "CarNESW")
        {
            this.begin = begin;
            this.end = end;
            this.firstCar = firstCar;
            this.spacing = spacing;
            this.zoomed = zoomed;
        }

        public override void Draw(Graphics g)
        {
            Bitmap image;
            float num = 2 * Math.Sign((float) (this.end.X - this.begin.X));
            float num2 = Math.Sign((float) (this.end.Y - this.begin.Y));
            float num3 = this.begin.X + (num * this.firstCar);
            float num4 = this.begin.Y + (num2 * this.firstCar);
            string str = "Car";
            if (!this.zoomed)
            {
                str = str + "Small";
            }
            if (Math.Sign(num) != Math.Sign(num2))
            {
                image = S.Resources.GetImage(str + "NESW");
            }
            else
            {
                image = S.Resources.GetImage(str + "NWSE");
            }
            while ((Math.Sign((float) (this.end.X - num3)) == Math.Sign(num)) && (Math.Sign((float) (this.end.Y - num4)) == Math.Sign(num2)))
            {
                g.DrawImageUnscaled(image, (int) num3, (int) num4);
                num3 += num * this.spacing;
                num4 += num2 * this.spacing;
            }
        }
    }
}

