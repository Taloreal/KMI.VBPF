namespace KMI.VBPF1Lib
{
    using System;
    using System.Drawing;

    [Serializable]
    public class WorkStoreMgrFastFood : WorkTask
    {
        public int Register;
        private States State = States.Init;

        public WorkStoreMgrFastFood(int register)
        {
            this.Register = register;
            base.WorkExperienceRequired.Add("Shift Manager", 2f);
            base.HourlyWage = 22.5f;
        }

        public override string CategoryName()
        {
            return A.Resources.GetString("Work");
        }

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
                    base.Owner.Pose = "FFWalk";
                    base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Serve" + this.Register).ToPoints();
                    this.State = States.MoveToServe;
                    break;

                case States.MoveToServe:
                    if (base.Owner.Move())
                    {
                        if (base.Owner.Pose == "FFCarryFood")
                        {
                            ((FastFoodStore) base.Building).OrderUp[this.Register] = true;
                        }
                        base.Owner.Pose = "FFStandSW";
                        this.State = States.WaitForOrder;
                    }
                    break;

                case States.WaitForOrder:
                    if (A.State.Period == base.EndPeriod)
                    {
                        this.State = States.Exit;
                        base.Owner.Pose = "FFWalk";
                        base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "EntryPoint").ToPoints();
                        break;
                    }
                    if (((FastFoodStore) base.Building).OrderIn[this.Register])
                    {
                        ((FastFoodStore) base.Building).OrderIn[this.Register] = false;
                        base.Owner.Pose = "FFWalk";
                        base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "ServeBack" + this.Register).ToPoints();
                        this.State = States.GetOrder;
                    }
                    break;

                case States.GetOrder:
                    if (base.Owner.Move())
                    {
                        base.Owner.Pose = "FFCarryFood";
                        base.Owner.Path = base.Building.Map.findPath(base.Owner.Location, "Serve" + this.Register).ToPoints();
                        this.State = States.MoveToServe;
                        ((FastFoodStore) base.Building).TakeItem(this.Register);
                    }
                    break;

                case States.Exit:
                    base.Owner.Location = (PointF) base.Building.Map.getNode("EntryPoint").Location;
                    base.Owner.Pose = "StandNW";
                    return true;
            }
            return false;
        }

        public override string Name()
        {
            return "Store Manager";
        }

        public override string ResumeDescription()
        {
            return A.Resources.GetString("Managed all facets of fast food restaurant operation.");
        }

        private enum States
        {
            Init,
            MoveToServe,
            WaitForOrder,
            GetOrder,
            Exit
        }
    }
}

