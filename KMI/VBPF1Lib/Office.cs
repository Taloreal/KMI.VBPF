namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Office : AppBuilding
    {
        public float Busyness;

        public Office(CityBlock block, int lotIndex, BuildingType type) : base(block, lotIndex, type)
        {
            this.Busyness = 0.02f * ((float) A.State.Random.NextDouble());
            base.Map = AppConstants.Work1Map;
            base.EntryPoint = (PointF) base.Map.getNode("EntryPoint").Location;
        }

        public override string GetBackgroundImage()
        {
            return "OfficeBack";
        }

        public override ArrayList GetInsideDrawables()
        {
            int num;
            PointF tf;
            ArrayList list = new ArrayList();
            ArrayList c = new ArrayList();
            ArrayList list3 = new ArrayList();
            foreach (VBPFPerson person in base.Persons)
            {
                if (person.Location.Y < ((0.5f * person.Location.X) + 47f))
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
            list.Add(new Drawable(base.Map.getNode("OfficeSupervisorDesk").Location, "OfficeSupervisorDesk"));
            list.Add(new Drawable(base.Map.getNode("OfficeSupervisorBookshelf").Location, "OfficeSupervisorBookshelf"));
            list.Add(new Drawable(base.Map.getNode("SWWall").Location, "SWWall"));
            list.Add(new Drawable(base.Map.getNode("OfficeCouch").Location, "OfficeCouch"));
            list.Add(new Drawable(base.Map.getNode("OfficeManagerDesk").Location, "OfficeManagerDesk"));
            list.Add(new Drawable(base.Map.getNode("OfficeManagerPainting").Location, "OfficeManagerPainting"));
            list.Add(new Drawable(base.Map.getNode("OfficeManagerPlant").Location, "OfficeManagerPlant"));
            for (num = 4; num < 6; num++)
            {
                tf = (PointF) base.Map.getNode("Chair" + num).Location;
                list.Add(new Drawable(new PointF(tf.X - 10f, tf.Y - 36f), "OfficeWorkerChair"));
            }
            list.AddRange(c);
            float[] numArray = new float[3];
            numArray[1] = 134f;
            numArray[2] = 320f;
            float[] numArray2 = numArray;
            PointF location = (PointF) base.Map.getNode("SEWall").Location;
            for (num = 0; num < 3; num++)
            {
                list.Add(new Drawable(new PointF(location.X + numArray2[num], location.Y + (numArray2[num] / 2f)), "SEWall"));
            }
            list.Add(new Drawable(base.Map.getNode("OfficePlant").Location, "OfficePlant"));
            list.Add(new Drawable(base.Map.getNode("OfficePrinter").Location, "OfficePrinter"));
            for (num = 0; num < 4; num++)
            {
                list.Add(new Drawable(base.Map.getNode("OfficeWorkerDesk" + num).Location, "OfficeWorkerDesk"));
            }
            for (num = 0; num < 4; num++)
            {
                tf = (PointF) base.Map.getNode("Chair" + num).Location;
                list.Add(new Drawable(new PointF(tf.X - 10f, tf.Y - 36f), "OfficeWorkerChair"));
            }
            list.AddRange(list3);
            return list;
        }

        public void StressOutWorkers()
        {
        }
    }
}

