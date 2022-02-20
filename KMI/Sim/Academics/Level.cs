namespace KMI.Sim.Academics
{
    using System;

    [Serializable]
    public class Level
    {
        public float Goal;
        public string LevelIntroMessage = "";
        public Page[] Pages = new Page[0];
        public string[] Powers = new string[0];
    }
}

