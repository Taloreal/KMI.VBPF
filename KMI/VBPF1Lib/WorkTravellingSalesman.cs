namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;

    [Serializable]
    public class WorkTravellingSalesman : WorkTask
    {
        public KMI.VBPF1Lib.Car Car;
        protected int count = 0;
        public int Mileage = 0;
        protected bool returning = false;
        public States State;
        public int VisitBuildingIndex = 0;

        public override void CleanUp()
        {
            base.CleanUp();
            this.State = States.Init;
            this.returning = false;
        }

        public override bool Do()
        {
            if (A.State.Period != base.EndPeriod)
            {
                AppBuilding randomBuilding;
                switch (this.State)
                {
                    case States.Init:
                        this.Car = base.GetAppEntity().Car;
                        if (((this.Car == null) || this.Car.Broken) || (this.Car.Gas <= 0f))
                        {
                            string str;
                            if (this.Car != null)
                            {
                                if (this.Car.Broken)
                                {
                                    str = A.Resources.GetString("your car is broken down");
                                }
                                else
                                {
                                    str = A.Resources.GetString("your car is out of gas");
                                }
                            }
                            else
                            {
                                str = A.Resources.GetString("you don't have a car");
                            }
                            object[] args = new object[] { str, this.Name().ToLower() };
                            base.GetAppEntity().Player.SendMessage(A.Resources.GetString("Because {0}, you are unable to do your job as a {1} and have been marked absent for the day.", args), "", NotificationColor.Yellow);
                            base.DatesAbsent.Add(A.State.Now);
                            goto Label_0220;
                        }
                        this.State = States.Wait;
                        goto Label_0225;

                    case States.Drive:
                    {
                        float num = A.State.SimulatedTimePerStep / 0x4e20;
                        this.Mileage += (int) num;
                        if (this.Car.NewStep())
                        {
                            this.State = States.Wait;
                            this.count = 20;
                        }
                        goto Label_0225;
                    }
                    case States.Wait:
                        this.count -= A.State.SimulatedTimePerStep / 0x4e20;
                        if (this.count >= 0)
                        {
                            goto Label_0225;
                        }
                        randomBuilding = null;
                        if (this.returning)
                        {
                            randomBuilding = base.Building;
                            break;
                        }
                        randomBuilding = (AppBuilding) A.State.City.GetRandomBuilding(this.VisitBuildingIndex);
                        break;

                    default:
                        goto Label_0225;
                }
                this.Car.SendTo(randomBuilding);
                this.returning = !this.returning;
                this.State = States.Drive;
                goto Label_0225;
            }
        Label_0220:
            return true;
        Label_0225:
            return false;
        }

        public enum States
        {
            Init,
            Drive,
            Wait
        }
    }
}

