namespace KMI.Utility
{
    using System;

    public class Phraser
    {
        protected double[] breakPoints;
        protected string[] phrases;

        public Phraser(string[] phrases)
        {
            this.phrases = phrases;
            this.breakPoints = new double[phrases.Length - 1];
            for (int i = 0; i < this.breakPoints.Length; i++)
            {
                this.breakPoints[i] = ((double) (i + 1)) / ((double) phrases.Length);
            }
        }

        public Phraser(string delimitedPhrases)
        {
            char[] separator = new char[] { '|' };
            this.phrases = delimitedPhrases.Split(separator);
            this.breakPoints = new double[this.phrases.Length - 1];
            for (int i = 0; i < this.breakPoints.Length; i++)
            {
                this.breakPoints[i] = ((double) (i + 1)) / ((double) this.phrases.Length);
            }
        }

        public Phraser(double[] breakPoints, string[] phrases)
        {
            this.phrases = phrases;
            this.breakPoints = breakPoints;
            if (this.phrases.Length != (this.breakPoints.Length + 1))
            {
                throw new Exception("Must supply one fewer breakpoints than phrases.");
            }
        }

        public Phraser(double breakPoint, string delimitedPhrases)
        {
            char[] separator = new char[] { '|' };
            this.phrases = delimitedPhrases.Split(separator);
            this.breakPoints = new double[] { breakPoint };
            if (this.phrases.Length != (this.breakPoints.Length + 1))
            {
                throw new Exception("Must supply one fewer breakpoints than phrases.");
            }
        }

        public Phraser(double[] breakPoints, string delimitedPhrases)
        {
            char[] separator = new char[] { '|' };
            this.phrases = delimitedPhrases.Split(separator);
            this.breakPoints = breakPoints;
            if (this.phrases.Length != (this.breakPoints.Length + 1))
            {
                throw new Exception("Must supply one fewer breakpoints than phrases.");
            }
        }

        public Phraser(double breakPoint, string phrase1, string phrase2)
        {
            this.phrases = new string[] { phrase1, phrase2 };
            this.breakPoints = new double[] { breakPoint };
        }

        public string GetPhrase(double val)
        {
            for (int i = 0; i < this.breakPoints.Length; i++)
            {
                if (val < this.breakPoints[i])
                {
                    return this.phrases[i];
                }
            }
            return this.phrases[this.phrases.Length - 1];
        }
    }
}

