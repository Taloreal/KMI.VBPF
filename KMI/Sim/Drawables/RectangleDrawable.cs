namespace KMI.Sim.Drawables
{
    using System;
    using System.Drawing;

    [Serializable]
    public class RectangleDrawable : Drawable
    {
        protected Color color;
        protected bool fill;
        protected System.Drawing.Size size;

        public RectangleDrawable(Point location, System.Drawing.Size size, Color color, bool fill) : base(location, null)
        {
            this.fill = fill;
            this.size = size;
            this.color = color;
        }

        public RectangleDrawable(Point location, string clickString, System.Drawing.Size size, Color color, bool fill) : base(location, null, clickString)
        {
            this.fill = fill;
            this.size = size;
            this.color = color;
        }

        public override void Draw(Graphics g)
        {
            if (this.fill)
            {
                g.FillRectangle(new SolidBrush(this.color), new Rectangle(base.location, this.size));
            }
            else
            {
                g.DrawRectangle(new Pen(this.color, 1f), new Rectangle(base.location, this.size));
            }
        }

        public override bool HitTest(int x, int y)
        {
            if (!base.hittable)
            {
                return false;
            }
            Rectangle rectangle = new Rectangle(base.location, this.size);
            return rectangle.Contains(x, y);
        }

        protected override System.Drawing.Size Size
        {
            get
            {
                return this.size;
            }
        }
    }
}

