namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class NodeV2
    {
        public bool blocked = false;
        public float centerx = 0.5f;
        public float centery = 0.5f;
        [NonSerialized]
        public int distance;
        public int height = 8;
        [NonSerialized]
        public bool isDead;
        public string name = "";
        [NonSerialized]
        public NodeV2 nextLink;
        public ArrayList nodes = new ArrayList();
        public bool visited = false;
        public int weight = 0;
        public int width = 8;
        public int x;
        public int y;

        public Point DistributedLocation
        {
            get
            {
                int num = S.State.Random.Next(this.width);
                int num2 = S.State.Random.Next(this.height);
                PointF tf = Utilities.cartesianToIsometric((this.centerx * this.width) / 1f, (this.centery * this.height) / 1f, 0f, 0f, 1f, 1f);
                PointF tf2 = Utilities.cartesianToIsometric((float) num, (float) num2, this.x - tf.X, this.y - tf.Y, 1f, 1f);
                return new Point((int) tf2.X, (int) tf2.Y);
            }
        }

        public Point Location
        {
            get
            {
                return new Point(this.x, this.y);
            }
            set
            {
                this.x = value.X;
                this.y = value.Y;
            }
        }
    }
}

