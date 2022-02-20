namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public class Drawable : IComparable
    {
        protected string clickString;
        protected bool hittable;
        protected string imageName;
        protected Point location;
        public int plane;
        public object Tag;
        protected string toolTipText;

        public Drawable(Point location, string imageName)
        {
            this.toolTipText = "";
            this.hittable = true;
            this.plane = 0;
            this.location = location;
            this.imageName = imageName;
        }

        public Drawable(PointF location, string imageName)
        {
            this.toolTipText = "";
            this.hittable = true;
            this.plane = 0;
            this.location = Point.Round(location);
            this.imageName = imageName;
        }

        public Drawable(Point location, string imageName, string clickString)
        {
            this.toolTipText = "";
            this.hittable = true;
            this.plane = 0;
            this.location = location;
            this.imageName = imageName;
            this.clickString = clickString;
        }

        public Drawable(PointF location, string imageName, string clickString)
        {
            this.toolTipText = "";
            this.hittable = true;
            this.plane = 0;
            this.location = Point.Round(location);
            this.imageName = imageName;
            this.clickString = clickString;
        }

        public virtual int CompareTo(object obj)
        {
            Drawable drawable = (Drawable) obj;
            if (this.plane == drawable.plane)
            {
                if (drawable.location.Y > this.location.Y)
                {
                    return -1;
                }
                return 1;
            }
            if (drawable.plane > this.plane)
            {
                return -1;
            }
            return 1;
        }

        public virtual void Draw(Graphics g)
        {
            g.DrawImageUnscaled(S.Resources.GetImage(this.imageName), this.location);
        }

        public virtual void Drawable_Click(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            if ((this.clickString != null) && (this.clickString != ""))
            {
                Point anchor = new Point(this.location.X + (this.Size.Width / 2), this.location.Y);
                Utilities.DrawComment(S.MainForm.BackBufferGraphics, this.clickString, anchor, S.MainForm.MainWindowBounds);
            }
            control.Refresh();
        }

        public virtual void Drawable_DoubleClick(object sender, EventArgs e)
        {
        }

        public virtual void Drawable_MouseMove(object sender, MouseEventArgs e)
        {
            Control control = (Control) sender;
            if ((this.clickString != null) && (this.clickString != ""))
            {
                control.Cursor = Cursors.Hand;
            }
            S.MainForm.ViewToolTip.SetToolTip((Control) sender, this.toolTipText);
            this.DrawSelected(control.CreateGraphics());
        }

        public virtual void DrawSelected(Graphics g)
        {
        }

        public virtual bool HitTest(int x, int y)
        {
            if (!this.hittable)
            {
                return false;
            }
            Bitmap image = S.Resources.GetImage(this.imageName);
            return ((((x >= this.location.X) && (x < (this.location.X + image.Width))) && ((y >= this.location.Y) && (y < (this.location.Y + image.Height)))) && (image.GetPixel(x - this.location.X, y - this.location.Y).A > 0));
        }

        public string ClickString
        {
            get
            {
                return this.clickString;
            }
        }

        public bool Hittable
        {
            get
            {
                return this.hittable;
            }
            set
            {
                this.hittable = value;
            }
        }

        public Point Location
        {
            get
            {
                return this.location;
            }
        }

        public int Plane
        {
            get
            {
                return this.plane;
            }
            set
            {
                this.plane = value;
            }
        }

        protected virtual System.Drawing.Size Size
        {
            get
            {
                if ((this.imageName != null) && (this.imageName != ""))
                {
                    return S.Resources.GetImage(this.imageName).Size;
                }
                return new System.Drawing.Size(0, 0);
            }
        }

        public string ToolTipText
        {
            get
            {
                return this.toolTipText;
            }
            set
            {
                this.toolTipText = value;
            }
        }
    }
}

