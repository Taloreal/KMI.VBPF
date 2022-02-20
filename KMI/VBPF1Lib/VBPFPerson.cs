namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class VBPFPerson : TurboMovableActiveObject
    {
        public string GenderString;
        public long ID;
        public int PaletteIndex;
        public string Pose;
        public KMI.VBPF1Lib.Task Task;

        public VBPFPerson()
        {
            this.Pose = "Walk";
            base.Speed = 20f;
            this.ID = A.State.GetNextID();
            this.PaletteIndex = A.State.Random.Next(6);
            this.GenderString = "Female";
            if (A.State.Random.Next(2) == 0)
            {
                this.GenderString = "Male";
            }
        }

        public VBPFPerson(Person.GenderType gender, Person.RaceType race, string firstName, string lastName)
        {
            this.Pose = "Walk";
            base.Speed = 16f;
            this.ID = A.State.GetNextID();
            this.PaletteIndex = A.State.Random.Next(6);
            this.GenderString = "Female";
            if (gender == Person.GenderType.Male)
            {
                this.GenderString = "Male";
            }
        }

        public ArrayList GetDrawables() {
            int num;
            string clickString = "";
            if ((this.Task != null) && !this.Drone) {
                if (this.Task is AttendClass) {
                    AttendClass class2 = (AttendClass) this.Task;
                    int num2 = 5;
                    if (class2.Weekend) { num2 = 2; }
                    object[] args = new object[] { class2.Course.Name, class2.daysInCourse / num2, (class2.Course.Days - class2.daysInCourse) / num2 };
                    clickString = A.Resources.GetString("I'm tired of sitting in {0}. I've completed {1} weeks and have {2} weeks to go. Boy will I be glad when I'm done!", args);
                }
                if (this.Task is WorkTask) {
                    WorkTask task = (WorkTask) this.Task;
                    object[] objArray = new object[2];
                    objArray[0] = task.Name();
                    TimeSpan span = (TimeSpan) (A.State.Now - task.StartDate);
                    objArray[1] = span.Days / 30;
                    clickString = A.Resources.GetString("I'm working away here as a {0}. I've got {1} months of experience which should help me advance some day!", objArray);
                }
            }
            ArrayList list = new ArrayList();
            string str2 = "";
            string str3 = base.Orientation();
            if (((this.Pose.EndsWith("SE") || this.Pose.EndsWith("NE")) || this.Pose.EndsWith("SW")) || this.Pose.EndsWith("NW")) {
                str3 = "";
            }
            if (this.Pose.EndsWith("Walk") || this.Pose.EndsWith("CarryFood")) {
                str2 = "00" + ((2 * A.State.FrameCounter) % 10);
            }
            if (this.Pose.EndsWith("JumpingJacksSW")) {
                str2 = "00" + (A.State.FrameCounter % 9);
            }
            if (this.Pose.EndsWith("DanceSW") || this.Pose.EndsWith("DanceSE")) {
                long num3 = ((2 * A.State.FrameCounter) + this.ID) % 0x1bL;
                str2 = num3.ToString().PadLeft(3, '0');
                if ((this.GetHashCode() % 2) == 0) {
                    this.Pose = this.Pose.Replace("SW", "SE");
                }
            }
            if (this.Pose.EndsWith("EatSE")) {
                str2 = "00" + (A.State.FrameCounter % 10);
            }
            if (this.Pose.EndsWith("TakeOrder")) {
                num = ((WorkCounterFastFood) this.Task).frame % 0x13;
                str2 = num.ToString().PadLeft(3, '0');
            }
            if (this.Pose.EndsWith("TypeNW")) {
                num = A.State.FrameCounter % 9;
                str2 = num.ToString().PadLeft(3, '0');
            }
            string imageName = this.GenderString + this.Pose + str3 + str2;
            PaletteDrawable drawable = new PaletteDrawable(base.Location, imageName, clickString, this.PaletteIndex) {
                VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom,
                HorizontalAlignment = FlexDrawable.HorizontalAlignments.Center
            };
            if (this.Pose.EndsWith("CarryFood")) {
                Point point = (Point) AppConstants.CarryAnchorPoints[imageName];
                PointF location = new PointF(base.Location.X + point.X, (base.Location.Y + point.Y) - 10f);
                FlexDrawable drawable2 = new FlexDrawable(location, "BagOfFood") {
                    VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom,
                    HorizontalAlignment = FlexDrawable.HorizontalAlignments.Center
                };
                ArrayList drawables = new ArrayList();
                drawables.Add(drawable);
                if (str3.StartsWith("N")) { drawables.Insert(0, drawable2); }
                else { drawables.Add(drawable2); }
                CompoundDrawable drawable3 = new CompoundDrawable(Point.Round(base.Location), drawables, "");
                list.Add(drawable3);
                return list;
            }
            list.Add(drawable);
            return list;
        }

        public override bool NewStep()
        {
            return ((this.Task != null) && this.Task.Do());
        }

        public bool Drone
        {
            get
            {
                return (this.PaletteIndex < 6);
            }
        }
    }
}

