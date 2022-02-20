namespace KMI.Sim.Drawables
{
    using System;
    using System.Drawing;

    [Serializable]
    public class PixelDrawable : Drawable
    {
        private static Bitmap bitmap = new Bitmap(1, 1);
        private const int CLICK_THRESHOLD = 1;
        protected System.Drawing.Color color;

        public PixelDrawable(Point location, System.Drawing.Color color) : base(location, "")
        {
            this.color = color;
        }

        public PixelDrawable(Point location, System.Drawing.Color color, string clickString) : base(location, "", clickString)
        {
            this.color = color;
        }

        public override void Draw(Graphics g)
        {
            bitmap.SetPixel(0, 0, this.color);
            g.DrawImageUnscaled(bitmap, this.location.X, this.location.Y);
        }

        public override bool HitTest(int x, int y)
        {
            if (!base.hittable)
            {
                return false;
            }
            return ((((this.location.X <= (x + 1)) && (this.location.X >= (x - 1))) && (this.location.Y <= (y + 1))) && (this.location.Y >= (y - 1)));
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
    }
}

