namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Drawing;

    [Serializable]
    public class TurboMovableActiveObject : MovableActiveObject
    {
        public override bool Move()
        {
            float num = A.State.SimulatedTimePerStep / 0x4e20;
            this.location.X += base.dx * num;
            this.location.Y += base.dy * num;
            bool flag = ((Math.Sign((float) (this.destination.X - this.location.X)) != Math.Sign(base.dx)) || (Math.Sign((float) (this.destination.Y - this.location.Y)) != Math.Sign(base.dy))) || ((base.dx == 0f) && (base.dy == 0f));
            if (!flag)
            {
                return flag;
            }
            base.location = base.destination;
            if ((base.Path == null) || (base.Path.Count <= 0))
            {
                return flag;
            }
            base.Destination = (PointF) base.Path[0];
            base.Path.RemoveAt(0);
            if ((base.PathSpeeds != null) && (base.PathSpeeds.Count > 0))
            {
                base.Speed = (float) base.PathSpeeds[0];
                base.PathSpeeds.RemoveAt(0);
            }
            return false;
        }
    }
}

