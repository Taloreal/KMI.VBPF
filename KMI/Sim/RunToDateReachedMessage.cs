namespace KMI.Sim
{
    using System;

    [Serializable]
    public class RunToDateReachedMessage : ModalMessage
    {
        public RunToDateReachedMessage() : base("", null, null, System.Windows.Forms.MessageBoxIcon.Hand)
        {
        }
    }
}