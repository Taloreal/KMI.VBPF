namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class ComputerDrawable : Drawable
    {
        public ComputerDrawable(Point location, string image, string clickString) : base(location, image, clickString)
        {
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            if (!A.MainForm.Disabled(A.MainForm.mnuActionsMMOnlineBanking))
            {
                try
                {
                    new frmOnlineBanking().ShowDialog(A.MainForm);
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }
    }
}

