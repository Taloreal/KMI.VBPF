namespace KMI.Sim
{
    using System;

    [Serializable]
    public class SimSpeed
    {
        public int SkipFactor;
        public int StepPeriod;

        public SimSpeed(int stepPeriod, int skipFactor)
        {
            this.StepPeriod = stepPeriod;
            this.SkipFactor = skipFactor;
        }
    }
}

