namespace KMI.Sim
{
    using System;

    [Serializable]
    public class ActiveObject
    {
        protected DateTime wakeupTime;

        public virtual void NewDay()
        {
            throw new NotImplementedException();
        }

        public virtual void NewHour()
        {
            throw new NotImplementedException();
        }

        public virtual bool NewStep()
        {
            throw new NotImplementedException();
        }

        public virtual void NewWeek()
        {
            throw new NotImplementedException();
        }

        public virtual void NewYear()
        {
            throw new NotImplementedException();
        }

        public virtual void Retire()
        {
            S.Instance.UnSubscribe(this);
        }

        public virtual DateTime WakeupTime
        {
            get
            {
                return this.wakeupTime;
            }
            set
            {
                this.wakeupTime = value;
            }
        }
    }
}

