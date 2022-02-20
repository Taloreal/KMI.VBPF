namespace KMI.Sim.Queues
{
    using KMI.Sim;
    using System;
    using System.Drawing;

    [Serializable]
    public class ProcessingSimQueueSegment : SimQueueSegment
    {
        protected int countVarianceMax;
        protected int countVarianceMin;
        protected int processCount;
        protected int processingSteps;
        protected bool processOnReverse;
        protected Point processPoint;

        public ProcessingSimQueueSegment()
        {
            this.processPoint = base.endPoint;
            this.processingSteps = 0;
            this.countVarianceMin = 0;
            this.countVarianceMax = 1;
            this.processCount = 0;
            this.processOnReverse = false;
        }

        public override bool CanRemoveFromEnd()
        {
            bool flag = base.CanRemoveFromEnd();
            if (this.endPoint.Equals(this.processPoint))
            {
                return (flag && (this.processCount >= this.processingSteps));
            }
            return flag;
        }

        public override bool CanRemoveFromStart()
        {
            bool flag = base.CanRemoveFromStart();
            if (this.startPoint.Equals(this.processPoint))
            {
                return (flag && (this.processCount >= this.processingSteps));
            }
            return flag;
        }

        public override void MoveQueueObjects()
        {
            int num;
            ISimQueueObject obj2;
            if (base.reverse)
            {
                if (this.processOnReverse)
                {
                    for (num = 0; num < base.queueObjects.Count; num++)
                    {
                        obj2 = (ISimQueueObject) base.queueObjects[num];
                        if (obj2.Location.Equals(this.processPoint))
                        {
                            obj2.Waiting = true;
                            this.processCount += S.State.Random.Next(this.countVarianceMin, this.countVarianceMax);
                            if (this.processCount >= this.processingSteps)
                            {
                                base.MoveQueueObjectBackward(obj2, num);
                                if (!obj2.Waiting)
                                {
                                    this.processCount = 0;
                                }
                            }
                        }
                        else
                        {
                            base.MoveQueueObjectBackward(obj2, num);
                        }
                    }
                }
                else
                {
                    for (num = 0; num < base.queueObjects.Count; num++)
                    {
                        base.MoveQueueObjectBackward(base[num], num);
                    }
                }
            }
            else
            {
                for (num = base.queueObjects.Count - 1; num >= 0; num--)
                {
                    obj2 = (ISimQueueObject) base.queueObjects[num];
                    if (obj2.Location.Equals(this.processPoint))
                    {
                        obj2.Waiting = true;
                        this.processCount += S.State.Random.Next(this.countVarianceMin, this.countVarianceMax);
                        if (this.processCount >= this.processingSteps)
                        {
                            base.MoveQueueObjectForward(obj2, num);
                            if (!obj2.Waiting)
                            {
                                this.processCount = 0;
                            }
                        }
                    }
                    else
                    {
                        base.MoveQueueObjectForward(obj2, num);
                    }
                }
            }
        }

        public int CountVarianceMax
        {
            get
            {
                return this.countVarianceMax;
            }
            set
            {
                this.countVarianceMax = value;
            }
        }

        public int CountVarianceMin
        {
            get
            {
                return this.countVarianceMin;
            }
            set
            {
                this.countVarianceMin = value;
            }
        }

        public int ProcessCount
        {
            get
            {
                return this.processCount;
            }
            set
            {
                this.processCount = value;
            }
        }

        public int ProcessingSteps
        {
            get
            {
                return this.processingSteps;
            }
            set
            {
                this.processingSteps = value;
            }
        }

        public bool ProcessOnReverse
        {
            get
            {
                return this.processOnReverse;
            }
            set
            {
                this.processOnReverse = value;
            }
        }

        public Point ProcessPoint
        {
            get
            {
                return this.processPoint;
            }
            set
            {
                this.processPoint = value;
            }
        }
    }
}

