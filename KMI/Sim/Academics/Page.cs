namespace KMI.Sim.Academics
{
    using System;

    [Serializable]
    public class Page
    {
        public string BodyURL;
        public float MinScore = float.MinValue;
        public string Name;
        public string Power;
        public Question[] Questions;
    }
}

