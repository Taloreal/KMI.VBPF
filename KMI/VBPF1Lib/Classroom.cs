namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Classroom : AppBuilding
    {
        public Classroom(CityBlock block, int lotIndex, BuildingType type) : base(block, lotIndex, type)
        {
            base.Map = AppConstants.ClassMap;
            base.EntryPoint = (PointF) base.Map.getNode("EntryPoint").Location;
        }

        public Course DuringCourse()
        {
            foreach (Offering offering in base.Offerings)
            {
                if ((offering.PrototypeTask.StartPeriod <= A.State.Period) && (offering.PrototypeTask.EndPeriod > A.State.Period))
                {
                    return (Course) offering;
                }
            }
            return null;
        }

        public override string GetBackgroundImage()
        {
            return "ClassBack";
        }

        public override ArrayList GetInsideDrawables()
        {
            int num;
            Point location;
            FlexDrawable drawable;
            Point point2;
            ArrayList list = new ArrayList();
            list.Add(new Drawable(base.Map.getNode("TeacherDesk").Location, "TeacherDesk"));
            ArrayList c = new ArrayList();
            ArrayList list3 = new ArrayList();
            foreach (VBPFPerson person in base.Persons)
            {
                if ((person.Location.Y < ((0.5f * person.Location.X) + 207f)) && (person.Location.Y < ((-0.5f * person.Location.X) + 640f)))
                {
                    c.AddRange(person.GetDrawables());
                }
                else
                {
                    list3.AddRange(person.GetDrawables());
                }
            }
            c.Sort();
            list3.Sort();
            list.AddRange(c);
            for (num = 0; num < 2; num++)
            {
                location = base.Map.getNode("Table" + num).Location;
                drawable = new FlexDrawable(new Point(location.X + 0x52, location.Y + 0x3a), "Table") {
                    HorizontalAlignment = FlexDrawable.HorizontalAlignments.Center,
                    VerticalAlignment = FlexDrawable.VerticalAlignments.Middle
                };
                list.Add(drawable);
            }
            for (num = 0; num < 4; num++)
            {
                point2 = base.Map.getNode("Chair" + num).Location;
                list.Add(new Drawable(new Point(point2.X - 0x1d, point2.Y - 0x21), "SchoolChairBottom"));
            }
            list.AddRange(c);
            for (num = 2; num < 4; num++)
            {
                location = base.Map.getNode("Table" + num).Location;
                drawable = new FlexDrawable(new Point(location.X + 0x52, location.Y + 0x3a), "Table") {
                    HorizontalAlignment = FlexDrawable.HorizontalAlignments.Center,
                    VerticalAlignment = FlexDrawable.VerticalAlignments.Middle
                };
                list.Add(drawable);
            }
            for (num = 4; num < 8; num++)
            {
                point2 = base.Map.getNode("Chair" + num).Location;
                list.Add(new Drawable(new Point(point2.X - 0x1d, point2.Y - 0x21), "SchoolChairBottom"));
            }
            list.AddRange(list3);
            Course course = this.DuringCourse();
            if (course != null)
            {
                int num2 = A.State.FrameCounter % 0x1c;
                string str = num2.ToString().PadLeft(4, '0');
                list.Add(new Drawable(base.Map.getNode("Teacher").Location, course.TeacherGender + "TeacherBoardSW" + str));
            }
            return list;
        }
    }
}

