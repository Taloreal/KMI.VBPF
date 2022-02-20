namespace KMI.Sim.Queues
{
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    public interface ISimQueueObject
    {
        void ChangeActionState();
        Drawable GetDrawable();

        string ActionState { get; set; }

        string BaseImageName { get; set; }

        Point Location { get; set; }

        string Orientation { get; set; }

        bool Waiting { get; set; }

        int X { get; set; }

        int Y { get; set; }
    }
}

