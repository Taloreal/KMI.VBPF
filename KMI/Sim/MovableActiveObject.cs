namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class MovableActiveObject : ActiveObject
    {
        protected PointF destination = new PointF(0f, 0f);
        protected float dx = 0f;
        protected float dy = 0f;
        protected PointF location = new PointF(0f, 0f);
        protected ArrayList path;
        protected ArrayList pathSpeeds;
        protected float speed = 1f;

        public float DistanceFrom(PointF pt)
        {
            double introduced2 = Math.Pow((double) (this.location.X - pt.X), 2.0);
            return (float) Math.Sqrt(introduced2 + Math.Pow((double) (this.location.Y - pt.Y), 2.0));
        }

        public virtual bool Move()
        {
            this.location.X += this.dx;
            this.location.Y += this.dy;
            bool flag = ((Math.Sign((float) (this.destination.X - this.location.X)) != Math.Sign(this.dx)) || (Math.Sign((float) (this.destination.Y - this.location.Y)) != Math.Sign(this.dy))) || ((this.dx == 0f) && (this.dy == 0f));
            if (!flag)
            {
                return flag;
            }
            this.location = this.destination;
            if ((this.Path == null) || (this.Path.Count <= 0))
            {
                return flag;
            }
            this.Destination = (PointF) this.Path[0];
            this.Path.RemoveAt(0);
            if ((this.PathSpeeds != null) && (this.PathSpeeds.Count > 0))
            {
                this.Speed = (float) this.PathSpeeds[0];
                this.PathSpeeds.RemoveAt(0);
            }
            return false;
        }

        public string Orientation()
        {
            string str = "";
            if (this.dy > 0f)
            {
                str = str + "S";
            }
            else
            {
                str = str + "N";
            }
            if (this.dx > 0f)
            {
                return (str + "E");
            }
            return (str + "W");
        }

        public string Orientation8()
        {
            double num = Math.Atan2((double) -this.dy, (double) this.dx) * 57.295779513082323;
            if (num < 0.0)
            {
                num += 360.0;
            }
            if (((num >= 345.0) && (num < 360.0)) || ((num >= 0.0) && (num < 15.0)))
            {
                return "E";
            }
            if ((num >= 15.0) && (num < 67.5))
            {
                return "NE";
            }
            if ((num >= 67.5) && (num < 112.5))
            {
                return "N";
            }
            if ((num >= 112.5) && (num < 165.0))
            {
                return "NW";
            }
            if ((num >= 165.0) && (num < 195.0))
            {
                return "W";
            }
            if ((num >= 195.0) && (num < 248.5))
            {
                return "SW";
            }
            if ((num >= 248.5) && (num < 292.5))
            {
                return "S";
            }
            if ((num >= 292.5) && (num < 345.0))
            {
                return "SE";
            }
            return (num.ToString() + "Error");
        }

        public int OrientationIndex8()
        {
            double num = Math.Atan2((double) -this.dy, (double) this.dx) * 57.295779513082323;
            if (num < 0.0)
            {
                num += 360.0;
            }
            if (((num >= 345.0) && (num < 360.0)) || ((num >= 0.0) && (num < 15.0)))
            {
                return 2;
            }
            if ((num >= 15.0) && (num < 67.5))
            {
                return 3;
            }
            if ((num >= 67.5) && (num < 112.5))
            {
                return 4;
            }
            if ((num >= 112.5) && (num < 165.0))
            {
                return 5;
            }
            if ((num >= 165.0) && (num < 195.0))
            {
                return 6;
            }
            if ((num >= 195.0) && (num < 248.5))
            {
                return 7;
            }
            if ((num >= 248.5) && (num < 292.5))
            {
                return 0;
            }
            if ((num >= 292.5) && (num < 345.0))
            {
                return 1;
            }
            return -1;
        }

        public int OrientationIndexWithFlip(ref bool flipX)
        {
            if ((this.dx < 0f) && (this.dy < 0f))
            {
                flipX = false;
                return 0;
            }
            if ((this.dx > 0f) && (this.dy < 0f))
            {
                flipX = true;
                return 0;
            }
            if ((this.dx < 0f) && (this.dy > 0f))
            {
                flipX = false;
                return 1;
            }
            if ((this.dx > 0f) && (this.dy > 0f))
            {
                flipX = true;
                return 1;
            }
            flipX = false;
            return 1;
        }

        protected void RecalculateDXDY()
        {
            float num = Utilities.DistanceBetweenIsometric(this.destination, this.location);
            if (num == 0f)
            {
                this.dx = 0f;
                this.dy = 0f;
            }
            else
            {
                this.dx = ((this.destination.X - this.location.X) / num) * this.speed;
                this.dy = ((this.destination.Y - this.location.Y) / num) * this.speed;
            }
        }

        public PointF Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = value;
                this.RecalculateDXDY();
            }
        }

        public float DX
        {
            get
            {
                return this.dx;
            }
        }

        public float DY
        {
            get
            {
                return this.dy;
            }
        }

        public PointF Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
                this.RecalculateDXDY();
            }
        }

        public ArrayList Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if ((value != null) && (value.Count > 0))
                {
                    this.Destination = (PointF) value[0];
                    value.RemoveAt(0);
                }
                else
                {
                    this.Destination = this.Location;
                }
                this.path = value;
            }
        }

        public ArrayList PathSpeeds
        {
            get
            {
                return this.pathSpeeds;
            }
            set
            {
                if ((value != null) && (value.Count > 0))
                {
                    this.Speed = (float) value[0];
                    value.RemoveAt(0);
                }
                this.pathSpeeds = value;
            }
        }

        public float Speed
        {
            get
            {
                return this.speed;
            }
            set
            {
                this.speed = value;
                this.RecalculateDXDY();
            }
        }
    }
}

