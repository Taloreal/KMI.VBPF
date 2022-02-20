namespace KMI.VBPF1Lib
{
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class BillDrawable : Drawable
    {
        public BillDrawable(Point location, string image, string clickString) : base(location, image, clickString)
        {
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            if (!A.MainForm.Disabled(A.MainForm.mnuActionsMMBills))
            {
                A.MainForm.mnuActionsMMBills.PerformClick();
            }
        }
    }
}

