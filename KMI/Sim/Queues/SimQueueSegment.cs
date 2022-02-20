namespace KMI.Sim.Queues
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public class SimQueueSegment
    {
        protected int dx;
        protected int dy;
        protected Point endPoint;
        protected Point forwardBackupPoint = new Point(-2147483648, -2147483648);
        protected ArrayList next = new ArrayList();
        protected string[] objectOrientation = new string[] { "", "" };
        protected int objectSeparationX = 0;
        protected int objectSeparationY = 0;
        protected ArrayList previous = new ArrayList();
        protected ArrayList queueObjects = new ArrayList();
        protected bool reverse = false;
        protected Point reverseBackupPoint = new Point(-2147483648, -2147483648);
        protected Point startPoint;

        public virtual bool CanAddToEnd()
        {
            if (this.queueObjects.Count == 0)
            {
                return true;
            }
            ISimQueueObject obj2 = this[this.Count - 1];
            return ((Math.Abs((int) (this.endPoint.X - obj2.X)) >= this.objectSeparationX) && (Math.Abs((int) (this.endPoint.Y - obj2.Y)) >= this.objectSeparationY));
        }

        public virtual bool CanAddToStart()
        {
            if (this.queueObjects.Count == 0)
            {
                return true;
            }
            ISimQueueObject obj2 = this[0];
            return ((Math.Abs((int) (obj2.X - this.startPoint.X)) >= this.objectSeparationX) && (Math.Abs((int) (obj2.Y - this.startPoint.Y)) >= this.objectSeparationY));
        }

        public virtual bool CanRemoveFromEnd()
        {
            if (this.queueObjects.Count == 0)
            {
                return false;
            }
            ISimQueueObject obj2 = this[this.Count - 1];
            return obj2.Location.Equals(this.endPoint);
        }

        public virtual bool CanRemoveFromStart()
        {
            if (this.queueObjects.Count == 0)
            {
                return false;
            }
            ISimQueueObject obj2 = this[0];
            return obj2.Location.Equals(this.startPoint);
        }

        public virtual void Clear()
        {
            this.queueObjects.Clear();
        }

        public virtual ArrayList GetDrawables()
        {
            ArrayList list = new ArrayList();
            foreach (ISimQueueObject obj2 in this.queueObjects)
            {
                list.Add(obj2.GetDrawable());
            }
            return list;
        }

        private int GetDX()
        {
            int dx = this.dx;
            if ((this.endPoint.X - this.startPoint.X) < 0)
            {
                dx *= -1;
            }
            return dx;
        }

        private int GetDY()
        {
            int dy = this.dy;
            if ((this.endPoint.Y - this.startPoint.Y) < 0)
            {
                dy *= -1;
            }
            return dy;
        }

        private SimQueueSegment GetNextSegment()
        {
            if (this.next.Count == 1)
            {
                return (SimQueueSegment) this.next[0];
            }
            return null;
        }

        private SimQueueSegment GetPreviousSegment()
        {
            if (this.previous.Count == 1)
            {
                return (SimQueueSegment) this.previous[0];
            }
            return null;
        }

        private void MoveBackwardHelper(ISimQueueObject obj)
        {
            SimQueueSegment previousSegment = this.GetPreviousSegment();
            if (obj.Location.Equals(this.reverseBackupPoint))
            {
                if ((previousSegment != null) && previousSegment.CanAddToEnd())
                {
                    obj.X -= this.GetDX();
                    obj.Y -= this.GetDY();
                }
            }
            else
            {
                obj.X -= this.GetDX();
                obj.Y -= this.GetDY();
            }
        }

        private void MoveForwardHelper(ISimQueueObject obj)
        {
            obj.Waiting = false;
            SimQueueSegment nextSegment = this.GetNextSegment();
            if (obj.Location.Equals(this.forwardBackupPoint))
            {
                if ((nextSegment != null) && nextSegment.CanAddToStart())
                {
                    obj.X += this.GetDX();
                    obj.Y += this.GetDY();
                }
            }
            else
            {
                obj.X += this.GetDX();
                obj.Y += this.GetDY();
            }
        }

        protected virtual void MoveQueueObjectBackward(ISimQueueObject obj, int indexOfObj)
        {
            if (indexOfObj == 0)
            {
                if (obj.Location.Equals(this.startPoint))
                {
                    SimQueueSegment previousSegment = this.GetPreviousSegment();
                    if (previousSegment != null)
                    {
                        if (previousSegment.TryAddToEnd(obj))
                        {
                            this.queueObjects.Remove(obj);
                            obj.Waiting = false;
                            obj.ChangeActionState();
                        }
                        else
                        {
                            obj.Waiting = true;
                            obj.ChangeActionState();
                        }
                    }
                    else
                    {
                        obj.Waiting = true;
                        obj.ChangeActionState();
                    }
                }
                else
                {
                    obj.Waiting = false;
                    obj.ChangeActionState();
                    this.MoveBackwardHelper(obj);
                }
            }
            else
            {
                ISimQueueObject obj2 = this[indexOfObj - 1];
                if ((this.objectSeparationX == 0) || (this.objectSeparationY == 0))
                {
                    obj.Waiting = true;
                    if ((this.objectSeparationX == 0) && (this.objectSeparationY == 0))
                    {
                        obj.Waiting = false;
                        this.MoveBackwardHelper(obj);
                    }
                    else if (this.objectSeparationX == 0)
                    {
                        if (Math.Abs((int) (obj.Y - obj2.Y)) > this.objectSeparationY)
                        {
                            obj.Waiting = false;
                            this.MoveBackwardHelper(obj);
                        }
                    }
                    else if ((this.objectSeparationY == 0) && (Math.Abs((int) (obj.X - obj2.X)) > this.objectSeparationX))
                    {
                        obj.Waiting = false;
                        this.MoveBackwardHelper(obj);
                    }
                    obj.ChangeActionState();
                }
                else if ((Math.Abs((int) (obj.X - obj2.X)) > this.objectSeparationX) && (Math.Abs((int) (obj.Y - obj2.Y)) > this.objectSeparationY))
                {
                    obj.Waiting = false;
                    obj.ChangeActionState();
                    this.MoveBackwardHelper(obj);
                }
                else
                {
                    obj.Waiting = true;
                    obj.ChangeActionState();
                }
            }
        }

        protected virtual void MoveQueueObjectForward(ISimQueueObject obj, int indexOfObj)
        {
            if (indexOfObj == (this.queueObjects.Count - 1))
            {
                if (obj.Location.Equals(this.endPoint))
                {
                    SimQueueSegment nextSegment = this.GetNextSegment();
                    if (nextSegment != null)
                    {
                        if (nextSegment.TryAddToStart(obj))
                        {
                            this.queueObjects.Remove(obj);
                            obj.Waiting = false;
                            obj.ChangeActionState();
                        }
                        else
                        {
                            obj.Waiting = true;
                            obj.ChangeActionState();
                        }
                    }
                    else
                    {
                        obj.Waiting = true;
                        obj.ChangeActionState();
                    }
                }
                else
                {
                    obj.Waiting = false;
                    obj.ChangeActionState();
                    this.MoveForwardHelper(obj);
                }
            }
            else
            {
                ISimQueueObject obj2 = this[indexOfObj + 1];
                if ((this.objectSeparationX == 0) || (this.objectSeparationY == 0))
                {
                    obj.Waiting = true;
                    if ((this.objectSeparationX == 0) && (this.objectSeparationY == 0))
                    {
                        obj.Waiting = false;
                        this.MoveForwardHelper(obj);
                    }
                    else if (this.objectSeparationX == 0)
                    {
                        if (Math.Abs((int) (obj2.Y - obj.Y)) > this.objectSeparationY)
                        {
                            obj.Waiting = false;
                            this.MoveForwardHelper(obj);
                        }
                    }
                    else if ((this.objectSeparationY == 0) && (Math.Abs((int) (obj2.X - obj.X)) > this.objectSeparationX))
                    {
                        obj.Waiting = false;
                        this.MoveForwardHelper(obj);
                    }
                    obj.ChangeActionState();
                }
                else if ((Math.Abs((int) (obj2.X - obj.X)) > this.objectSeparationX) && (Math.Abs((int) (obj2.Y - obj.Y)) > this.objectSeparationY))
                {
                    obj.Waiting = false;
                    obj.ChangeActionState();
                    this.MoveForwardHelper(obj);
                }
                else
                {
                    obj.Waiting = true;
                    obj.ChangeActionState();
                }
            }
        }

        public virtual void MoveQueueObjects()
        {
            int num;
            if (this.reverse)
            {
                for (num = 0; num < this.queueObjects.Count; num++)
                {
                    this.MoveQueueObjectBackward(this[num], num);
                }
            }
            else
            {
                for (num = this.queueObjects.Count - 1; num >= 0; num--)
                {
                    this.MoveQueueObjectForward(this[num], num);
                }
            }
        }

        public virtual void RemoveAt(int index)
        {
            this.queueObjects.RemoveAt(index);
        }

        public virtual bool TryAddToEnd(ISimQueueObject obj)
        {
            if (this.CanAddToEnd())
            {
                this.queueObjects.Add(obj);
                if (!this.reverse)
                {
                    obj.Orientation = this.objectOrientation[0];
                }
                else
                {
                    obj.Orientation = this.objectOrientation[1];
                }
                obj.Location = this.endPoint;
                obj.ChangeActionState();
                return true;
            }
            return false;
        }

        public virtual bool TryAddToStart(ISimQueueObject obj)
        {
            if (this.CanAddToStart())
            {
                this.queueObjects.Insert(0, obj);
                if (!this.reverse)
                {
                    obj.Orientation = this.objectOrientation[0];
                }
                else
                {
                    obj.Orientation = this.objectOrientation[1];
                }
                obj.Location = this.startPoint;
                obj.ChangeActionState();
                return true;
            }
            return false;
        }

        public virtual ISimQueueObject TryRemoveFromEnd()
        {
            if (this.CanRemoveFromEnd())
            {
                ISimQueueObject obj2 = this[this.Count - 1];
                this.queueObjects.RemoveAt(this.Count - 1);
                obj2.ChangeActionState();
                return obj2;
            }
            return null;
        }

        public virtual ISimQueueObject TryRemoveFromStart()
        {
            if (this.CanRemoveFromStart())
            {
                ISimQueueObject obj2 = this[0];
                this.queueObjects.RemoveAt(0);
                obj2.ChangeActionState();
                return obj2;
            }
            return null;
        }

        public int Count
        {
            get
            {
                return this.queueObjects.Count;
            }
        }

        public int DX
        {
            set
            {
                this.dx = Math.Abs(value);
            }
        }

        public int DY
        {
            set
            {
                this.dy = Math.Abs(value);
            }
        }

        public Point EndPoint
        {
            get
            {
                return this.endPoint;
            }
            set
            {
                this.endPoint = value;
            }
        }

        public Point ForwardBackupPoint
        {
            get
            {
                return this.forwardBackupPoint;
            }
            set
            {
                this.forwardBackupPoint = value;
            }
        }

        public ISimQueueObject this[int index]
        {
            get
            {
                return (ISimQueueObject) this.queueObjects[index];
            }
            set
            {
                this.queueObjects[index] = value;
            }
        }

        public ArrayList Next
        {
            get
            {
                return this.next;
            }
            set
            {
                this.next = value;
            }
        }

        public string[] ObjectOrientation
        {
            get
            {
                return this.objectOrientation;
            }
            set
            {
                this.objectOrientation = value;
            }
        }

        public int ObjectSeparationX
        {
            get
            {
                return this.objectSeparationX;
            }
            set
            {
                this.objectSeparationX = value;
            }
        }

        public int ObjectSeparationY
        {
            get
            {
                return this.objectSeparationY;
            }
            set
            {
                this.objectSeparationY = value;
            }
        }

        public ArrayList Previous
        {
            get
            {
                return this.previous;
            }
            set
            {
                this.previous = value;
            }
        }

        public ArrayList QueueObjects
        {
            get
            {
                return this.queueObjects;
            }
        }

        public bool Reverse
        {
            get
            {
                return this.reverse;
            }
            set
            {
                string str;
                this.reverse = value;
                if (!this.reverse)
                {
                    str = this.objectOrientation[0];
                }
                else
                {
                    str = this.objectOrientation[1];
                }
                foreach (ISimQueueObject obj2 in this.queueObjects)
                {
                    obj2.Orientation = str;
                }
            }
        }

        public Point ReverseBackupPoint
        {
            get
            {
                return this.reverseBackupPoint;
            }
            set
            {
                this.reverseBackupPoint = value;
            }
        }

        public Point StartPoint
        {
            get
            {
                return this.startPoint;
            }
            set
            {
                this.startPoint = value;
            }
        }
    }
}

