namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public class FlexDrawable : Drawable
    {
        protected bool flip;
        protected HorizontalAlignments horizontalAlignment;
        protected int offsetX;
        protected int offsetY;
        protected VerticalAlignments verticalAlignment;

        public FlexDrawable(Point location, string imageName) : base(location, imageName)
        {
            this.flip = false;
            this.horizontalAlignment = HorizontalAlignments.Left;
            this.verticalAlignment = VerticalAlignments.Top;
            this.offsetX = 0;
            this.offsetY = 0;
        }

        public FlexDrawable(PointF location, string imageName) : base(location, imageName)
        {
            this.flip = false;
            this.horizontalAlignment = HorizontalAlignments.Left;
            this.verticalAlignment = VerticalAlignments.Top;
            this.offsetX = 0;
            this.offsetY = 0;
        }

        public FlexDrawable(Point location, string imageName, string clickString) : base(location, imageName, clickString)
        {
            this.flip = false;
            this.horizontalAlignment = HorizontalAlignments.Left;
            this.verticalAlignment = VerticalAlignments.Top;
            this.offsetX = 0;
            this.offsetY = 0;
        }

        public FlexDrawable(PointF location, string imageName, string clickString) : base(location, imageName, clickString)
        {
            this.flip = false;
            this.horizontalAlignment = HorizontalAlignments.Left;
            this.verticalAlignment = VerticalAlignments.Top;
            this.offsetX = 0;
            this.offsetY = 0;
        }

        public override void Draw(Graphics g)
        {
            Bitmap image = S.Resources.GetImage(base.imageName);
            switch (this.verticalAlignment)
            {
                case VerticalAlignments.Top:
                    this.offsetY = 0;
                    break;

                case VerticalAlignments.Middle:
                    this.offsetY = -image.Height / 2;
                    break;

                case VerticalAlignments.Bottom:
                    this.offsetY = -image.Height;
                    break;
            }
            switch (this.horizontalAlignment)
            {
                case HorizontalAlignments.Left:
                    this.offsetX = 0;
                    break;

                case HorizontalAlignments.Center:
                    this.offsetX = -image.Width / 2;
                    break;

                case HorizontalAlignments.Right:
                    this.offsetX = -image.Width;
                    break;
            }
            if (this.flip)
            {
                g.DrawImage(image, (this.location.X + this.offsetX) + image.Width, this.location.Y + this.offsetY, -image.Width, image.Height);
            }
            else
            {
                g.DrawImage(image, this.location.X + this.offsetX, this.location.Y + this.offsetY, image.Width, image.Height);
            }
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            if ((base.clickString != null) && (base.clickString != ""))
            {
                Point anchor = new Point((this.location.X + (this.Size.Width / 2)) + this.offsetX, this.location.Y + this.offsetY);
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
            return base.HitTest(x - this.offsetX, y - this.offsetY);
        }

        public bool Flip
        {
            get
            {
                return this.flip;
            }
            set
            {
                this.flip = value;
            }
        }

        public HorizontalAlignments HorizontalAlignment
        {
            get
            {
                return this.horizontalAlignment;
            }
            set
            {
                this.horizontalAlignment = value;
            }
        }

        public VerticalAlignments VerticalAlignment
        {
            get
            {
                return this.verticalAlignment;
            }
            set
            {
                this.verticalAlignment = value;
            }
        }

        public enum HorizontalAlignments
        {
            Left,
            Center,
            Right
        }

        public enum VerticalAlignments
        {
            Top,
            Middle,
            Bottom
        }
    }
}

