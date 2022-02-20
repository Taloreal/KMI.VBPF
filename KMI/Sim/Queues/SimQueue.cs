namespace KMI.Sim.Queues
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public class SimQueue
    {
        protected ArrayList segments = new ArrayList();

        public virtual void AppendSegment(SimQueueSegment segment)
        {
            if (this.SegmentCount == 0)
            {
                this.segments.Add(segment);
            }
            else
            {
                this[this.SegmentCount - 1].Next.Add(segment);
                segment.Previous.Add(this[this.SegmentCount - 1]);
                this.segments.Add(segment);
            }
        }

        public virtual bool CanAddToEnd()
        {
            return ((this.segments.Count > 0) && ((SimQueueSegment) this.segments[this.segments.Count - 1]).CanAddToEnd());
        }

        public virtual bool CanAddToStart()
        {
            return ((this.segments.Count > 0) && ((SimQueueSegment) this.segments[0]).CanAddToStart());
        }

        public virtual bool CanRemoveFromEnd()
        {
            return ((this.segments.Count > 0) && ((SimQueueSegment) this.segments[this.segments.Count - 1]).CanRemoveFromEnd());
        }

        public virtual bool CanRemoveFromStart()
        {
            return ((this.segments.Count > 0) && ((SimQueueSegment) this.segments[0]).CanRemoveFromStart());
        }

        public virtual void Clear()
        {
            for (int i = 0; i < this.segments.Count; i++)
            {
                this[i].Clear();
            }
        }

        public virtual void Connect(SimQueue queue)
        {
            if (!this[this.SegmentCount - 1].Next.Contains(queue[0]))
            {
                this[this.SegmentCount - 1].Next.Add(queue[0]);
            }
            if (!queue[0].Previous.Contains(this[this.SegmentCount - 1]))
            {
                queue[0].Previous.Add(this[this.SegmentCount - 1]);
            }
        }

        public static SimQueue Create(int numSegments, Point[,] segmentPoints, string[][] objectOrientations, int dx, int dy, int objectSeparationX, int objectSeparationY)
        {
            SimQueue queue = new SimQueue();
            for (int i = 0; i < numSegments; i++)
            {
                SimQueueSegment segment = new SimQueueSegment();
                queue.AppendSegment(segment);
            }
            queue.SetDXDY(dx, dy);
            queue.SetObjectSeparation(objectSeparationX, objectSeparationY);
            queue.SetObjectOrientations(objectOrientations);
            queue.SetSegmentPoints(segmentPoints);
            return queue;
        }

        public static SimQueue Create(int numSegments, Point[,] segmentPoints, string[] objectOrientation, int dx, int dy, int objectSeparationX, int objectSeparationY)
        {
            SimQueue queue = new SimQueue();
            for (int i = 0; i < numSegments; i++)
            {
                SimQueueSegment segment = new SimQueueSegment();
                queue.AppendSegment(segment);
            }
            queue.SetDXDY(dx, dy);
            queue.SetObjectSeparation(objectSeparationX, objectSeparationY);
            queue.SetObjectOrientations(objectOrientation);
            queue.SetSegmentPoints(segmentPoints);
            return queue;
        }

        public virtual void Disconnect(SimQueue queue)
        {
            if (this[this.SegmentCount - 1].Next.Contains(queue[0]))
            {
                this[this.SegmentCount - 1].Next.Remove(queue[0]);
            }
            if (queue[0].Previous.Contains(this[this.SegmentCount - 1]))
            {
                queue[0].Previous.Remove(this[this.SegmentCount - 1]);
            }
        }

        public virtual void ExecuteStep()
        {
            for (int i = this.segments.Count - 1; i >= 0; i--)
            {
                this[i].MoveQueueObjects();
            }
        }

        public virtual ArrayList GetDrawables()
        {
            ArrayList list = new ArrayList();
            foreach (SimQueueSegment segment in this.segments)
            {
                list.AddRange(segment.GetDrawables());
            }
            return list;
        }

        public virtual ArrayList GetQueueObjects()
        {
            ArrayList list = new ArrayList();
            foreach (SimQueueSegment segment in this.segments)
            {
                list.AddRange(segment.QueueObjects);
            }
            return list;
        }

        public void ReverseAllSegments()
        {
            foreach (SimQueueSegment segment in this.segments)
            {
                segment.Reverse = true;
            }
        }

        public virtual void SetDXDY(int dx, int dy)
        {
            for (int i = 0; i < this.SegmentCount; i++)
            {
                this[i].DX = dx;
                this[i].DY = dy;
            }
        }

        public virtual void SetObjectOrientations(string[][] orientations)
        {
            if ((orientations != null) && (this.segments.Count == orientations.Length))
            {
                for (int i = 0; i < this.segments.Count; i++)
                {
                    this[i].ObjectOrientation = orientations[i];
                }
            }
        }

        public virtual void SetObjectOrientations(string[] orientation)
        {
            if (orientation != null)
            {
                for (int i = 0; i < this.segments.Count; i++)
                {
                    this[i].ObjectOrientation = orientation;
                }
            }
        }

        public virtual void SetObjectSeparation(int objectSeparationX, int objectSeparationY)
        {
            for (int i = 0; i < this.SegmentCount; i++)
            {
                this[i].ObjectSeparationX = objectSeparationX;
                this[i].ObjectSeparationY = objectSeparationY;
            }
        }

        public virtual void SetSegmentPoints(Point[,] points)
        {
            if (points != null)
            {
                for (int i = 0; i < this.SegmentCount; i++)
                {
                    this[i].StartPoint = points[i, 0];
                    this[i].EndPoint = points[i, 1];
                }
            }
        }

        public virtual bool TryAddToEnd(ISimQueueObject obj)
        {
            return (this.CanAddToEnd() && ((SimQueueSegment) this.segments[this.segments.Count - 1]).TryAddToEnd(obj));
        }

        public virtual bool TryAddToStart(ISimQueueObject obj)
        {
            return (this.CanAddToStart() && ((SimQueueSegment) this.segments[0]).TryAddToStart(obj));
        }

        public virtual ISimQueueObject TryRemoveFromEnd()
        {
            if (this.CanRemoveFromEnd())
            {
                return ((SimQueueSegment) this.segments[this.segments.Count - 1]).TryRemoveFromEnd();
            }
            return null;
        }

        public virtual ISimQueueObject TryRemoveFromStart()
        {
            if (this.CanRemoveFromStart())
            {
                return ((SimQueueSegment) this.segments[0]).TryRemoveFromStart();
            }
            return null;
        }

        public void UnreverseAllSegments()
        {
            foreach (SimQueueSegment segment in this.segments)
            {
                segment.Reverse = false;
            }
        }

        public SimQueueSegment this[int index]
        {
            get
            {
                return (SimQueueSegment) this.segments[index];
            }
            set
            {
                this.segments[index] = value;
            }
        }

        public int ObjectCount
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.segments.Count; i++)
                {
                    num += this[i].Count;
                }
                return num;
            }
        }

        public int SegmentCount
        {
            get
            {
                return this.segments.Count;
            }
        }
    }
}

