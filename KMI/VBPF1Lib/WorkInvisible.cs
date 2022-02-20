namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class WorkInvisible : WorkTask
    {
        public States State = States.Init;

        public override void CleanUp()
        {
            base.CleanUp();
            this.State = States.Init;
        }

        public override bool Do()
        {
            switch (this.State)
            {
                case States.Init:
                    if (!base.Building.Persons.Contains(base.Owner))
                    {
                        base.Building.Persons.Add(base.Owner);
                    }
                    this.State = States.Done;
                    break;

                case States.Done:
                    if (A.State.Period != base.EndPeriod)
                    {
                        return false;
                    }
                    return true;
            }
            return false;
        }

        public enum States
        {
            Init,
            Done
        }
    }
}

