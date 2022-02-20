namespace KMI.Sim.Drawables
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [Serializable]
    public class LineDrawable : Drawable
    {
        private const int CLICK_THRESHOLD = 1;
        protected System.Drawing.Color color;
        protected Point connector;
        protected System.Drawing.Drawing2D.SmoothingMode smoothingMode;
        protected float width;

        public LineDrawable(Point location, Point connector) : base(location, "")
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.connector = connector;
            this.width = 1f;
            this.color = System.Drawing.Color.Black;
        }

        public LineDrawable(Point location, Point connector, string clickString) : base(location, "", clickString)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.connector = connector;
            this.width = 1f;
            this.color = System.Drawing.Color.Black;
        }

        public override void Draw(Graphics g)
        {
            System.Drawing.Drawing2D.SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = this.smoothingMode;
            SolidBrush brush = new SolidBrush(this.color);
            g.DrawLine(new Pen(brush, this.width), base.location, this.connector);
            g.SmoothingMode = smoothingMode;
        }

        public override bool HitTest(int x, int y)
        {
            if (base.hittable)
            {
                int num = Math.Min(this.location.X, this.connector.X);
                int num2 = Math.Max(this.location.X, this.connector.X);
                if ((x >= (num - 1)) && (x <= (num2 + 1)))
                {
                    float num3 = ((float) (this.location.Y - this.connector.Y)) / ((float) (this.location.X - this.connector.X));
                    float num4 = (num3 * (x - this.location.X)) + this.location.Y;
                    if ((num4 >= (y - 1f)) && (num4 <= (y + 1f)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
            }
        }

        protected override System.Drawing.Size Size
        {
            get
            {
                int width = Math.Abs((int) (this.location.X - this.connector.X));
                return new System.Drawing.Size(width, Math.Abs((int) (this.location.Y - this.connector.Y)));
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            set
            {
                this.smoothingMode = value;
            }
        }

        public float Width
        {
            get
            {
                return this.width;
            }
            set
            {
                this.width = value;
            }
        }
    }
}

