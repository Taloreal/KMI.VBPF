namespace KMI.Sim.Queues
{
    using System;
    using System.Collections;

    public interface ISimQueueCollection
    {
        void Clear();
        ArrayList GetDrawables();
        void Go();
        bool NewStep();
        void Stop();
    }
}

