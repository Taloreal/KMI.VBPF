namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using System;

    [Serializable]
    public class ShowPageMessage : ModalMessage
    {
        public KMI.Sim.Academics.Page Page;

        public ShowPageMessage(string to, KMI.Sim.Academics.Page page)
            : base(to, null, null, System.Windows.Forms.MessageBoxIcon.None)
        {
            this.Page = page;
        }
    }
}
