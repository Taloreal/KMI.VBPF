namespace KMI.Sim.Drawables
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Drawing;

    [Serializable]
    public class CommentDrawable : Drawable
    {
        protected string comment;

        public CommentDrawable(Point location, string comment) : base(location, null)
        {
            this.comment = comment;
            base.Hittable = false;
        }

        public override void Draw(Graphics g)
        {
            Utilities.DrawComment(g, this.comment, base.location, S.MainForm.MainWindowBounds);
        }
    }
}

