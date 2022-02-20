namespace KMI.Sim
{
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public abstract class View
    {
        protected Bitmap background;
        protected bool clearDrawSelectedOnMouseMove;
        protected static Drawable currentHit;
        protected Drawable[] drawables;
        protected string name;
        protected System.Drawing.Size size;
        protected int skewFactor;
        public object[] ViewerOptions;

        public View(string name, Bitmap background)
        {
            this.skewFactor = 2;
            this.clearDrawSelectedOnMouseMove = false;
            this.name = name;
            this.background = background;
            this.size = background.Size;
        }

        public View(string name, System.Drawing.Size size)
        {
            this.skewFactor = 2;
            this.clearDrawSelectedOnMouseMove = false;
            this.name = name;
            this.size = size;
            this.background = null;
        }

        public abstract Drawable[] BuildDrawables(long entityID, params object[] args);
        public static void ClearCurrentHit()
        {
            currentHit = null;
        }

        public void Draw(Graphics g)
        {
            if (this.background != null)
            {
                g.DrawImageUnscaled(this.background, 0, 0);
            }
            foreach (Drawable drawable in this.drawables)
            {
                drawable.Draw(g);
            }
            if (currentHit != null)
            {
                currentHit.DrawSelected(g);
            }
        }

        public Drawable HitTest(int x, int y)
        {
            if (this.drawables != null)
            {
                for (int i = this.drawables.Length - 1; i >= 0; i--)
                {
                    if (this.drawables[i].HitTest(x, y))
                    {
                        return this.drawables[i];
                    }
                }
            }
            return null;
        }

        protected internal void UpdateCurrentHit(MouseEventArgs e)
        {
            currentHit = this.HitTest(e.X, e.Y);
        }

        public virtual void View_Click(object sender, EventArgs e)
        {
            if (currentHit != null)
            {
                currentHit.Drawable_Click(sender, e);
            }
        }

        public virtual void View_DoubleClick(object sender, EventArgs e)
        {
            if (currentHit != null)
            {
                currentHit.Drawable_DoubleClick(sender, e);
            }
        }

        public virtual void View_MouseDown(object sender, MouseEventArgs e)
        {
        }

        public virtual void View_MouseMove(object sender, MouseEventArgs e)
        {
            if (!S.Instance.SimTimeRunning && this.ClearDrawSelectedOnMouseMove)
            {
                S.MainForm.picMain.Refresh();
            }
            Control control = (Control) sender;
            control.Cursor = Cursors.Default;
            if (currentHit != null)
            {
                currentHit.Drawable_MouseMove(sender, e);
            }
            else
            {
                S.MainForm.ViewToolTip.SetToolTip(control, "");
            }
        }

        public virtual void View_MouseUp(object sender, MouseEventArgs e)
        {
        }

        public bool ClearDrawSelectedOnMouseMove
        {
            get
            {
                return this.clearDrawSelectedOnMouseMove;
            }
            set
            {
                this.clearDrawSelectedOnMouseMove = value;
            }
        }

        public Drawable[] Drawables
        {
            get
            {
                return this.drawables;
            }
            set
            {
                this.drawables = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return this.size;
            }
        }
    }
}

