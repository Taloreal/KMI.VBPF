namespace KMI.Utility
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    [Serializable]
    public class ResponseCurve
    {
        protected PointF[] points;
        protected float variance;

        public void ReadPointsFromFile(Assembly assembly, string resource)
        {
            StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resource));
            ArrayList list = new ArrayList();
            for (string str = reader.ReadLine(); str != null; str = reader.ReadLine())
            {
                string[] strArray = str.Split(new char[] { ' ', '\t' }, 2);
                PointF tf = new PointF(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]));
                list.Add(tf);
            }
            this.points = new PointF[list.Count];
            for (int i = 0; i < this.points.Length; i++)
            {
                this.points[i] = (PointF)list[i];
            }
        }

        public float Response(float input)
        {
            return this.Response(input, null);
        }

        public float Response(float input, Random random)
        {
            float negativeInfinity = float.NegativeInfinity;
            for (int i = 0; i < this.points.Length; i++)
            {
                if (input < this.points[i].X)
                {
                    if (i == 0)
                    {
                        negativeInfinity = this.points[i].Y;
                    }
                    else
                    {
                        negativeInfinity = this.points[i - 1].Y + (((this.points[i].Y - this.points[i - 1].Y) * (input - this.points[i - 1].X)) / (this.points[i].X - this.points[i - 1].X));
                    }
                    break;
                }
            }
            if (float.IsNegativeInfinity(negativeInfinity))
            {
                negativeInfinity = this.points[this.points.Length - 1].Y;
            }
            if (random != null)
            {
                return (negativeInfinity + ((float)((random.NextDouble() - 0.5) * this.Variance)));
            }
            return negativeInfinity;
        }

        public PointF[] Points
        {
            get
            {
                return this.points;
            }
            set
            {
                this.points = value;
                for (int i = 0; i < this.points.Length; i++)
                {
                    for (int j = 0; j < this.points.Length; j++)
                    {
                        if ((i != j) && (this.points[i].X == this.points[j].X))
                        {
                            throw new Exception("Duplicate x values in response curve.");
                        }
                    }
                }
            }
        }

        public float Variance
        {
            get
            {
                return this.variance;
            }
            set
            {
                this.variance = value;
            }
        }
    }
}