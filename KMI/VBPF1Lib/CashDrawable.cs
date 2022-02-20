namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class CashDrawable : Drawable
    {
        protected float cash;

        public CashDrawable(Point location, string image, string clickString, float cash) : base(location, image, clickString)
        {
            this.cash = cash;
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            if (!A.MainForm.Disabled(A.MainForm.mnuActionsMMBanking))
            {
                try
                {
                    new frmDepositWithdrawCash(null, false) { updAmount = { Maximum = (decimal) this.cash } }.ShowDialog(A.MainForm);
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }
    }
}

