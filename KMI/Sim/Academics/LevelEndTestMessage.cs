namespace KMI.Sim.Academics
{
    using KMI.Sim;
    using System;

    [Serializable]
    public class LevelEndTestMessage : ModalMessage
    {
        public bool LastLevel;
        public int NewLevel;
        public Question[] Questions;

        public LevelEndTestMessage(string to, string message, int newLevel, Question[] questions, bool lastLevel)
            : base(to, message, null, System.Windows.Forms.MessageBoxIcon.None)
        {
            this.NewLevel = newLevel;
            this.Questions = questions;
            this.LastLevel = lastLevel;
        }
    }
}
