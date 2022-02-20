namespace KMI.Sim
{
    using System;

    [Serializable]
    public class StopDateReachedMessage : ModalMessage
    {
        public StopDateReachedMessage() : base("", null, null, System.Windows.Forms.MessageBoxIcon.Hand)
        {
        }
    }
}