namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public class PageDrawable : Drawable
    {
        private int col;
        private bool flipX;
        private int row;

        public PageDrawable(Point location, string imageName) : base(location, imageName)
        {
            this.col = 0;
            this.row = 0;
            this.flipX = false;
            base.location = location;
            base.imageName = imageName;
        }

        public PageDrawable(PointF location, string imageName) : base(location, imageName)
        {
            this.col = 0;
            this.row = 0;
            this.flipX = false;
            base.location = Point.Round(location);
            base.imageName = imageName;
        }

        public PageDrawable(Point location, string imageName, string clickString) : base(location, imageName, clickString)
        {
            this.col = 0;
            this.row = 0;
            this.flipX = false;
            base.location = location;
            base.imageName = imageName;
            base.clickString = clickString;
        }

        public PageDrawable(PointF location, string imageName, string clickString) : base(location, clickString)
        {
            this.col = 0;
            this.row = 0;
            this.flipX = false;
            base.location = Point.Round(location);
            base.imageName = imageName;
            base.clickString = clickString;
        }

        public override void Draw(Graphics g)
        {
            RectangleF ef;
            Page page = S.Resources.GetPage(base.imageName);
            if (this.flipX)
            {
                ef = new RectangleF(new PointF((float) ((this.location.X - page.AnchorX) + page.CellWidth), (float) (this.location.Y - page.AnchorY)), new SizeF((float) -page.CellWidth, (float) page.CellHeight));
            }
            else
            {
                ef = new RectangleF(new PointF((float) (this.location.X - page.AnchorX), (float) (this.location.Y - page.AnchorY)), new SizeF((float) page.CellWidth, (float) page.CellHeight));
            }
            RectangleF srcRect = new RectangleF(new PointF((float) (this.col * page.CellWidth), (float) (this.row * page.CellHeight)), new SizeF((float) page.CellWidth, (float) page.CellHeight));
            g.DrawImage(page.Bitmap, ef, srcRect, GraphicsUnit.Pixel);
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            Page page = S.Resources.GetPage(base.imageName);
            if ((base.clickString != null) && (base.clickString != ""))
            {
                Point anchor = new Point((this.location.X - page.AnchorX) + (page.CellWidth / 2), this.location.Y - page.AnchorY);
                Utilities.DrawComment(S.MainForm.BackBufferGraphics, base.clickString, anchor, S.MainForm.MainWindowBounds);
            }
            control.Refresh();
        }

        public override bool HitTest(int x, int y)
        {
            if (!base.hittable)
            {
                return false;
            }
            Page page = S.Resources.GetPage(base.imageName);
            if (this.flipX)
            {
                return ((((x >= (this.location.X - page.AnchorX)) && (x < ((this.location.X - page.AnchorX) + page.CellWidth))) && ((y >= (this.location.Y - page.AnchorY)) && (y < ((this.location.Y - page.AnchorY) + page.CellHeight)))) && (page.Bitmap.GetPixel(-(((x + 1) - this.location.X) + page.AnchorX) + ((this.col + 1) * page.CellWidth), ((y - this.location.Y) + page.AnchorY) + (this.row * page.CellHeight)).A > 0));
            }
            return ((((x >= (this.location.X - page.AnchorX)) && (x < ((this.location.X - page.AnchorX) + page.CellWidth))) && ((y >= (this.location.Y - page.AnchorY)) && (y < ((this.location.Y - page.AnchorY) + page.CellHeight)))) && (page.Bitmap.GetPixel(((x - this.location.X) + page.AnchorX) + (this.col * page.CellWidth), ((y - this.location.Y) + page.AnchorY) + (this.row * page.CellHeight)).A > 0));
        }

        public int Col
        {
            get
            {
                return this.col;
            }
            set
            {
                this.col = value;
            }
        }

        public bool FlipX
        {
            get
            {
                return this.flipX;
            }
            set
            {
                this.flipX = value;
            }
        }

        public int Row
        {
            get
            {
                return this.row;
            }
            set
            {
                this.row = value;
            }
        }

        protected override System.Drawing.Size Size
        {
            get
            {
                if ((base.imageName != null) && (base.imageName != ""))
                {
                    Page page = S.Resources.GetPage(base.imageName);
                    return new System.Drawing.Size(page.CellWidth, page.CellHeight);
                }
                return new System.Drawing.Size(0, 0);
            }
        }
    }
}

