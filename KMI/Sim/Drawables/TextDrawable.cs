namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    [Serializable]
    public class TextDrawable : Drawable
    {
        protected bool center;
        protected Color color;
        protected string fontName;
        protected int fontSize;
        protected SizeF size;
        protected float skewFactor;
        protected System.Drawing.Drawing2D.SmoothingMode smoothingMode;
        protected string text;

        public TextDrawable(Point location, string text, string fontName, int fontSize, Color color) : base(location, null)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.center = false;
            this.text = text;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.color = color;
            this.skewFactor = 0f;
        }

        public TextDrawable(Point location, string text, string fontName, int fontSize, Color color, float skewFactor) : base(location, null)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.center = false;
            this.text = text;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.color = color;
            this.skewFactor = skewFactor;
        }

        public TextDrawable(Point location, string text, string fontName, int fontSize, Color color, string clickString) : base(location, null, clickString)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.center = false;
            this.text = text;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.color = color;
            this.skewFactor = 0f;
        }

        public TextDrawable(Point location, string text, string fontName, int fontSize, Color color, float skewFactor, string clickString) : base(location, null, clickString)
        {
            this.smoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.center = false;
            this.text = text;
            this.fontName = fontName;
            this.fontSize = fontSize;
            this.color = color;
            this.skewFactor = skewFactor;
        }

        public override void Draw(Graphics g)
        {
            SolidBrush brush = new SolidBrush(this.color);
            Font font = new Font(this.fontName, (float) this.fontSize);
            StringFormat format = new StringFormat();
            if (this.center)
            {
                format.Alignment = StringAlignment.Center;
            }
            System.Drawing.Drawing2D.SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = this.smoothingMode;
            if ((this.text != null) && !(this.text == ""))
            {
                this.size = g.MeasureString(this.text, font);
                Rectangle rect = new Rectangle(this.location.X, this.location.Y, (int) this.size.Width, (int) this.size.Height);
                Point[] plgpts = new Point[] { new Point(this.location.X, this.location.Y), new Point(this.location.X + ((int) this.size.Width), this.location.Y - ((int) (this.size.Width * this.skewFactor))), new Point(this.location.X, this.location.Y + ((int) this.size.Height)) };
                Matrix matrix = new Matrix(rect, plgpts);
                Matrix transform = g.Transform;
                g.Transform = matrix;
                g.DrawString(this.text, font, brush, (PointF) base.location, format);
                g.Transform = transform;
                g.SmoothingMode = smoothingMode;
            }
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            Control control = (Control) sender;
            if ((base.clickString != null) && (base.clickString != ""))
            {
                Point anchor = new Point(this.location.X + (this.Size.Width / 2), this.location.Y - ((int) (((float) (this.Size.Width / 2)) / this.skewFactor)));
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
            if ((x < this.location.X) || (x > (this.location.X + this.size.Width)))
            {
                return false;
            }
            if (y < (this.location.Y - ((x - this.location.X) * this.skewFactor)))
            {
                return false;
            }
            if (y > ((this.location.Y + this.size.Height) - ((x - this.location.X) * this.skewFactor)))
            {
                return false;
            }
            return true;
        }

        public bool Center
        {
            set
            {
                this.center = value;
            }
        }

        protected override System.Drawing.Size Size
        {
            get
            {
                return System.Drawing.Size.Round(this.size);
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

