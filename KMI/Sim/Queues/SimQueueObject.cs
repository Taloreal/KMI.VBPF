namespace KMI.Sim.Queues
{
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class SimQueueObject : ISimQueueObject
    {
        protected string actionState;
        protected string baseImageName;
        protected Point location = new Point(-2147483648, -2147483648);
        protected string orientation;
        protected bool waiting;

        public SimQueueObject(string baseImageName)
        {
            this.baseImageName = baseImageName;
            this.orientation = "";
            this.actionState = "";
            this.waiting = true;
        }

        public virtual void ChangeActionState()
        {
        }

        public virtual Drawable GetDrawable()
        {
            return new Drawable(this.location, this.baseImageName + this.orientation + this.actionState);
        }

        public string ActionState
        {
            get
            {
                return this.actionState;
            }
            set
            {
                this.actionState = value;
            }
        }

        public string BaseImageName
        {
            get
            {
                return this.baseImageName;
            }
            set
            {
                this.baseImageName = value;
            }
        }

        public Point Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
            }
        }

        public string Orientation
        {
            get
            {
                return this.orientation;
            }
            set
            {
                this.orientation = value;
            }
        }

        public bool Waiting
        {
            get
            {
                return this.waiting;
            }
            set
            {
                this.waiting = value;
            }
        }

        public int X
        {
            get
            {
                return this.location.X;
            }
            set
            {
                this.location.X = value;
            }
        }

        public int Y
        {
            get
            {
                return this.location.Y;
            }
            set
            {
                this.location.Y = value;
            }
        }
    }
}

