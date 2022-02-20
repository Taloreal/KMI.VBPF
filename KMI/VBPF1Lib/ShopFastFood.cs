namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Drawing;

    [Serializable]
    public class ShopFastFood : Task
    {
        public bool OrderTaken;
        public int Register;
        public States State = States.Init;
        public int waitCounter;

        public ShopFastFood(VBPFPerson owner, FastFoodStore store)
        {
            base.Owner = owner;
            base.Building = store;
        }

        public override bool Do()
        {
            FastFoodStore building = (FastFoodStore) base.Building;
            switch (this.State)
            {
                case States.Init:
                    building.Customers.Add(base.Owner);
                    this.Register = building.SmallestOpenLine();
                    base.Owner.Location = (PointF) base.Building.Map.getNode("Enter" + this.side).Location;
                    base.Owner.Path = base.Building.Map.findPath("Enter" + this.side, "Cashier" + this.side).ToPoints();
                    this.OrderTaken = false;
                    this.waitCounter = 0;
                    this.State = States.ToCounter;
                    break;

                case States.ToCounter:
                    if (this.Move())
                    {
                        this.State = States.AtCounter;
                        base.Owner.Pose = "Stand";
                    }
                    break;

                case States.AtCounter:
                    if (building.OrderUp[this.Register])
                    {
                        base.Owner.Pose = "CarryFood";
                        building.OrderUp[this.Register] = false;
                        base.Owner.Path = base.Building.Map.findPath("Cashier" + this.side, "Exit" + this.side).ToPoints();
                        this.State = States.FromCounter;
                    }
                    break;

                case States.FromCounter:
                    if (this.Move())
                    {
                        ((FastFoodStore) base.Building).Customers.Remove(base.Owner);
                        this.State = States.Init;
                        base.Owner.WakeupTime = base.Owner.WakeupTime.AddDays(1.0);
                        return true;
                    }
                    break;
            }
            return false;
        }

        public bool Move()
        {
            if (this.State == States.ToCounter)
            {
                base.Owner.Pose = "Walk";
            }
            else
            {
                base.Owner.Pose = "CarryFood";
            }
            foreach (VBPFPerson person in ((FastFoodStore) base.Building).Customers)
            {
                ShopFastFood task = (ShopFastFood) person.Task;
                if (((base.Owner.ID > person.ID) && (this.State == States.ToCounter)) && ((task.State == States.AtCounter) || (task.State == States.ToCounter)))
                {
                    PointF tf = new PointF(base.Owner.Location.X + base.Owner.DX, base.Owner.Location.Y + base.Owner.DY);
                    if (Utilities.DistanceBetween(tf, person.Location) < 20f)
                    {
                        base.Owner.Pose = "Stand";
                        this.waitCounter++;
                        return false;
                    }
                }
            }
            return base.Owner.Move();
        }

        public string side
        {
            get
            {
                if (this.Register == 0)
                {
                    return "left";
                }
                return "right";
            }
        }

        public enum States
        {
            Init,
            ToCounter,
            AtCounter,
            FromCounter
        }
    }
}

