namespace KMI.Sim.Drawables
{
    using KMI.Utility;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    [Serializable]
    public class PolygonDrawable : Drawable
    {
        protected System.Drawing.Color color;
        protected bool fill;
        protected System.Drawing.Drawing2D.FillMode fillMode;
        protected int lineWidth;
        protected Point[] points;
        protected System.Drawing.Drawing2D.SmoothingMode smoothingMode;

        public PolygonDrawable(Point[] points) : base(points[0], "")
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.points = points;
            this.fill = true;
            this.fillMode = System.Drawing.Drawing2D.FillMode.Alternate;
            this.lineWidth = 1;
            this.color = System.Drawing.Color.Black;
        }

        public PolygonDrawable(Point[] points, string clickString) : base(points[0], "", clickString)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.points = points;
            this.fill = true;
            this.fillMode = System.Drawing.Drawing2D.FillMode.Alternate;
            this.lineWidth = 1;
            this.color = System.Drawing.Color.Black;
        }

        public override void Draw(Graphics g)
        {
            System.Drawing.Drawing2D.SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = this.smoothingMode;
            SolidBrush brush = new SolidBrush(this.color);
            if (this.fill)
            {
                g.FillPolygon(brush, this.points, this.fillMode);
            }
            else
            {
                g.DrawPolygon(new Pen(brush, (float) this.lineWidth), this.points);
            }
            g.SmoothingMode = smoothingMode;
        }

        public override bool HitTest(int x, int y)
        {
            if (!base.hittable)
            {
                return false;
            }
            PointF[] cornerPoints = new PointF[this.points.Length];
            for (int i = 0; i < cornerPoints.Length; i++)
            {
                cornerPoints[i] = (PointF) this.points[i];
            }
            return Utilities.PolygonContains(new PointF((float) x, (float) y), cornerPoints);
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

        public bool Fill
        {
            get
            {
                return this.fill;
            }
            set
            {
                this.fill = value;
            }
        }

        public System.Drawing.Drawing2D.FillMode FillMode
        {
            get
            {
                return this.fillMode;
            }
            set
            {
                this.fillMode = value;
            }
        }

        public int LineWidth
        {
            get
            {
                return this.lineWidth;
            }
            set
            {
                this.lineWidth = value;
            }
        }

        public Point[] Points
        {
            get
            {
                return this.points;
            }
            set
            {
                this.points = value;
            }
        }

        protected override System.Drawing.Size Size
        {
            get
            {
                int x = -2147483648;
                int y = -2147483648;
                int num3 = 0x7fffffff;
                int num4 = 0x7fffffff;
                foreach (Point point in this.Points)
                {
                    if (point.X > x)
                    {
                        x = point.X;
                    }
                    if (point.Y > y)
                    {
                        y = point.Y;
                    }
                    if (point.X < num3)
                    {
                        num3 = point.X;
                    }
                    if (point.Y < num4)
                    {
                        num4 = point.Y;
                    }
                }
                return new System.Drawing.Size(x - num3, y - num4);
            }
        }

        public System.Drawing.Drawing2D.SmoothingMode SmoothingMode
        {
            set
            {
                this.smoothingMode = value;
            }
        }
    }
}

