namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    public class CityView : View
    {
        private static Color PointerBlack = Color.FromArgb(0x99, 0, 0, 0);
        private static Color PointerBlue = Color.FromArgb(0x99, 200, 0xf1, 0xfe);
        private static Color PointerGray = Color.FromArgb(0x99, 0xe3, 0xe3, 0xe3);

        public CityView(string name, Bitmap background) : base(name, background)
        {
            base.ViewerOptions = new object[] { 0, 0, PointF.Empty, "" };
            KMI.Biz.City.City.NUM_AVENUES = 4;
            KMI.Biz.City.City.NUM_STREETS = 8;
            KMI.Biz.City.City.ORIGIN = new PointF(368f, 108f);
            KMI.Biz.City.City.STREET_SPACING = 49f;
            KMI.Biz.City.City.BLOCK_HEIGHTS = new float[12];
            KMI.Biz.City.City.LOTS_PER_BLOCK = new int[12];
            KMI.Biz.City.City.LOT_SPACING = 24f;
            KMI.Biz.City.City.AVENUE_SPACING = new float[12];
            for (int i = 0; i < 12; i++)
            {
                KMI.Biz.City.City.LOTS_PER_BLOCK[i] = 3;
                KMI.Biz.City.City.AVENUE_SPACING[i] = 98f;
            }
            KMI.Biz.City.City.AVENUE_VIEWING_REGIONS = 1;
            KMI.Biz.City.City.STREET_VIEWING_REGIONS = 1;
            KMI.Biz.City.City.AVENUE_REGION_OFFSET = 0x188;
            KMI.Biz.City.City.STREET_REGION_OFFSET = 0x188;
        }

        public override Drawable[] BuildDrawables(long entityID, params object[] args)
        {
            Car current;
            ArrayList list = new ArrayList();
            int centerAvenue = (int) args[0];
            int centerStreet = (int) args[1];
            PointF tf = (PointF) args[2];
            string playerName = (string) args[3];
            list.AddRange(A.State.City.GetDrawables(centerAvenue, centerStreet));
            foreach (Bus bus in A.State.City.Buses)
            {
                if (bus.InRegion(centerAvenue, centerStreet))
                {
                    list.Add(bus.GetDrawable(centerAvenue, centerStreet));
                }
            }
            IEnumerator enumerator = A.State.City.Cars.GetEnumerator();
            while (enumerator.MoveNext())
            {
                current = (Car) enumerator.Current;
                if (current.InRegion(centerAvenue, centerStreet))
                {
                    list.Add(current.GetDrawable(centerAvenue, centerStreet));
                }
            }
            foreach (Pedestrian pedestrian in A.State.City.Pedestrians)
            {
                if (pedestrian.InRegion(centerAvenue, centerStreet))
                {
                    list.Add(pedestrian.GetDrawable(centerAvenue, centerStreet));
                }
            }
            list.Sort();
            foreach (AppEntity entity in A.State.Entity.Values)
            {
                Color pointerGray = PointerGray;
                string str2 = "Gray";
                if ((entity.ID == entityID) && A.Adapter.HasEntity(playerName))
                {
                    pointerGray = PointerBlue;
                    str2 = "";
                }
                if (entity.Dwelling != null)
                {
                    PointF empty = (PointF) Point.Empty;
                    Point point = Point.Empty;
                    int height = 0;
                    if (entity.Person.Task is TravelByFoot)
                    {
                        empty = ((TravelByFoot) entity.Person.Task).Pedestrian.Location;
                    }
                    else if (entity.Person.Task is TravelByCar)
                    {
                        empty = ((TravelByCar) entity.Person.Task).Car.Location;
                    }
                    else if (entity.Person.Task is TravelByBus)
                    {
                        empty = ((TravelByBus) entity.Person.Task).Pedestrian.Location;
                    }
                    else if (entity.Person.Task is WorkPizzaGuy)
                    {
                        current = ((WorkPizzaGuy) entity.Person.Task).Car;
                        if (current != null)
                        {
                            empty = current.Location;
                        }
                        else
                        {
                            empty = new PointF((float) entity.Person.Task.Building.Avenue, (float) entity.Person.Task.Building.Street);
                        }
                    }
                    else
                    {
                        AppBuilding building = A.State.City.FindInsideBuilding(entity);
                        if (building != null)
                        {
                            point = Point.Round(KMI.Biz.City.City.Transform2((float) building.Avenue, (float) building.Street, (float) building.Lot, 0, 0));
                            height = building.BuildingType.Height;
                        }
                    }
                    if (!empty.IsEmpty)
                    {
                        point = Point.Round(KMI.Biz.City.City.Transform2(empty.X, empty.Y, 2f, 0, 0));
                    }
                    LineDrawable drawable = new LineDrawable(new Point(point.X + 0x18, point.Y - height), new Point(point.X + 0x18, point.Y - 0x69)) {
                        Color = PointerBlack
                    };
                    LineDrawable drawable2 = new LineDrawable(new Point(point.X + 0x19, point.Y - height), new Point(point.X + 0x19, point.Y - 0x69)) {
                        Color = pointerGray
                    };
                    LineDrawable drawable3 = new LineDrawable(new Point(point.X + 0x1a, point.Y - height), new Point(point.X + 0x1a, point.Y - 0x69)) {
                        Color = PointerBlack
                    };
                    Drawable drawable4 = new Drawable(new Point(point.X + 20, point.Y - 0x74), "PointerPerson" + str2);
                    list.Add(drawable);
                    list.Add(drawable2);
                    list.Add(drawable3);
                    list.Add(drawable4);
                }
                if (entity.Dwelling != null)
                {
                    list.Add(this.PointOutBuilding(entity.Dwelling, "PointerHome" + str2));
                }
                foreach (Task task in entity.GetAllTasks())
                {
                    if (task is WorkTask)
                    {
                        list.Add(this.PointOutBuilding(task.Building, "PointerWork" + str2));
                    }
                    else if (task is AttendClass)
                    {
                        list.Add(this.PointOutBuilding(task.Building, "PointerSchool" + str2));
                    }
                }
                if (A.Settings.CondosForSaleEnabledForOwner)
                {
                    foreach (AppBuilding building2 in A.State.City.GetBuildings())
                    {
                        if ((building2.Owner == entity) && ((DwellingOffer) building2.Offerings[0]).Condo)
                        {
                            list.Add(this.PointOutBuilding(building2, "PointerCondo" + str2));
                        }
                    }
                }
            }
            return (Drawable[]) list.ToArray(typeof(Drawable));
        }

        private Drawable PointOutBuilding(Building b, string imageName)
        {
            Point point = Point.Round(KMI.Biz.City.City.Transform2((float) b.Avenue, (float) b.Street, (float) b.Lot, 0, 0));
            int height = b.BuildingType.Height;
            if (height == 0x60)
            {
                height = 0x4b;
            }
            return new Drawable(new Point(point.X + 0x10, (point.Y - height) - 0x1a), imageName);
        }
    }
}

