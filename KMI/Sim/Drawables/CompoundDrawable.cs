namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    [Serializable]
    public class CompoundDrawable : Drawable
    {
        private static Drawable clickedDrawable = null;
        protected Point clickStringLocation;
        protected ArrayList drawables;
        private bool forwardClick;

        public CompoundDrawable(Point location, ArrayList drawables, string clickString) : base(location, null, clickString)
        {
            this.forwardClick = false;
            this.clickStringLocation = Point.Empty;
            this.drawables = drawables;
        }

        public override void Draw(Graphics g)
        {
            foreach (Drawable drawable in this.drawables)
            {
                drawable.Draw(g);
            }
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            if (this.forwardClick && (clickedDrawable != null))
            {
                clickedDrawable.Drawable_Click(sender, e);
            }
            Control control = (Control) sender;
            if ((base.clickString != null) && (base.clickString != ""))
            {
                Point anchor = new Point(this.ClickStringLocation.X + (this.Size.Width / 2), this.ClickStringLocation.Y);
                Utilities.DrawComment(S.MainForm.BackBufferGraphics, base.clickString, anchor, S.MainForm.MainWindowBounds);
            }
            control.Refresh();
        }

        public override void Drawable_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.forwardClick && (clickedDrawable != null))
            {
                clickedDrawable.Drawable_MouseMove(sender, e);
            }
            else
            {
                base.Drawable_MouseMove(sender, e);
            }
        }

        public override bool HitTest(int x, int y)
        {
            clickedDrawable = null;
            if (base.hittable)
            {
                for (int i = this.drawables.Count - 1; i >= 0; i--)
                {
                    if (((Drawable) this.drawables[i]).HitTest(x, y))
                    {
                        if (this.forwardClick)
                        {
                            clickedDrawable = (Drawable) this.drawables[i];
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public Point ClickStringLocation
        {
            get
            {
                if (this.clickStringLocation.IsEmpty)
                {
                    return base.location;
                }
                return this.clickStringLocation;
            }
            set
            {
                this.clickStringLocation = value;
            }
        }

        public ArrayList Drawables
        {
            get
            {
                if (this.drawables == null)
                {
                    this.drawables = new ArrayList();
                }
                return this.drawables;
            }
            set
            {
                this.drawables = value;
            }
        }

        public bool ForwardClick
        {
            get
            {
                return this.forwardClick;
            }
            set
            {
                this.forwardClick = value;
            }
        }
    }
}

