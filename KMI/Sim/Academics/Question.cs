namespace KMI.Sim.Academics
{
    using System;

    [Serializable]
    public class Question
    {
        public string Answer = null;
        public string[] Answers;
        public string Text;

        public bool Correct
        {
            get
            {
                foreach (string str in this.Answers)
                {
                    if (string.Compare(str.Trim(), this.Answer.Trim(), true) == 0)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

