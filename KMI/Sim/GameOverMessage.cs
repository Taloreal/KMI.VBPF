namespace KMI.Sim
{
    using System;

    [Serializable]
    public class GameOverMessage : ModalMessage
    {
        public GameOverMessage(string to, string message)
            : base(to, message, S.Resources.GetString("Game Over"), System.Windows.Forms.MessageBoxIcon.Hand)
        {
        }
    }
}