namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim.Drawables;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Dwelling : AppBuilding
    {
        public InsurancePolicy Insurance;
        public int MonthsLeftOnLease;

        public Dwelling(CityBlock block, int lotIndex, BuildingType type) : base(block, lotIndex, type)
        {
            this.Insurance = new InsurancePolicy(250f, false, 0f);
            base.Map = AppConstants.HomeMap;
            base.EntryPoint = (PointF) base.Map.getNode("EntryPoint").Location;
        }

        public override string GetBackgroundImage()
        {
            return "HomeBack";
        }

        public override ArrayList GetInsideDrawables()
        {
            int num;
            int num2;
            Drawable drawable4;
            AppEntity owner = (AppEntity) base.Owner;
            ArrayList list = new ArrayList();
            ArrayList c = new ArrayList();
            ArrayList list3 = new ArrayList();
            ArrayList list4 = new ArrayList();
            bool flag = (owner != null) && (owner.Dwelling == this);
            bool flag2 = false;
            foreach (VBPFPerson person in base.Persons)
            {
                if (person.Pose.EndsWith("EatSE"))
                {
                    flag2 = true;
                }
                if (person.Location.Y < ((-0.5f * (person.Location.X - 135f)) + 360f))
                {
                    c.AddRange(person.GetDrawables());
                }
                else if (person.Location.Y < ((0.5f * (person.Location.X - 365f)) + 242f))
                {
                    list3.AddRange(person.GetDrawables());
                }
                else
                {
                    list4.AddRange(person.GetDrawables());
                }
            }
            c.Sort();
            list3.Sort();
            list4.Sort();
            if (flag)
            {
                foreach (PurchasableItem item in owner.PurchasedItems)
                {
                    if (item.Name.StartsWith("Art"))
                    {
                        list.Add(new Drawable(base.Map.getNode(item.Name).Location, item.ImageName));
                    }
                }
                if (owner.Has("DDR"))
                {
                    int num6 = 0;
                    if (owner.DDRLockedBy > -1L)
                    {
                        num6 = A.State.Random.Next(5);
                    }
                    FlexDrawable drawable2 = new FlexDrawable(base.Map.getNode("DDR").Location, "DDR" + num6) {
                        VerticalAlignment = FlexDrawable.VerticalAlignments.Middle,
                        HorizontalAlignment = FlexDrawable.HorizontalAlignments.Center
                    };
                    list.Add(drawable2);
                }
                if (owner.Has("Carpet"))
                {
                    list.Add(new Drawable(base.Map.getNode("Carpet").Location, "Carpet" + owner.ImageIndexOf("Carpet")));
                }
                PointF tf2 = (PointF) base.Map.getNode("EndTable").Location;
                list.Add(new Drawable(tf2, "EndTable"));
                int num3 = 0x19;
                for (num = 0; num < Math.Min((int) (2 * num3), (int) (owner.BusTokens / 2)); num++)
                {
                    num2 = num / num3;
                    drawable4 = new Drawable(Point.Round(new PointF((tf2.X + 97f) + (num2 * 10), ((tf2.Y - ((num % num3) * 2)) + 3f) + (num2 * 5))), "BusToken");
                    object[] args = new object[] { owner.BusTokens };
                    drawable4.ToolTipText = A.Resources.GetString("{0} Bus Tokens", args);
                    Drawable drawable3 = drawable4;
                    list.Add(drawable3);
                }
                int num4 = Math.Min(20, 80 / Math.Max(1, owner.PartyFood.Count));
                PointF tf3 = (PointF) base.Map.getNode("Platter0").Location;
                int num5 = 0;
                ArrayList list5 = new ArrayList();
                foreach (PurchasableItem item2 in owner.PartyFood)
                {
                    Drawable drawable5 = new Drawable(new PointF(tf3.X - (num5 * num4), tf3.Y + ((num5++ * num4) / 2)), item2.ImageName + "Small") {
                        ToolTipText = item2.FriendlyName
                    };
                    list.Add(drawable5);
                }
            }
            list.Add(new Drawable(base.Map.getNode("Chair4").Location, "Chair"));
            list.Add(new Drawable(base.Map.getNode("Chair4b").Location, "Chair"));
            list.Add(new Drawable(base.Map.getNode("KitchenBar3").Location, "KitchenBar3"));
            list.AddRange(c);
            list.Add(new Drawable(base.Map.getNode("ApartmentKitchenInteriorWall").Location, "ApartmentKitchenInteriorWall"));
            Point location = base.Map.getNode("KitchenBar").Location;
            list.Add(new Drawable(location, "KitchenBar"));
            if (flag2)
            {
                list.Add(new Drawable(new Point(location.X + 0x36, location.Y + 8), "PlateOfFood"));
            }
            list.Add(new Drawable(base.Map.getNode("WallCabinet").Location, "WallCabinet"));
            Drawable drawable = new RefrigeratorDrawable(base.Map.getNode("Refrigerator").Location, "Refrigerator");
            if (owner != null)
            {
                object[] objArray2 = new object[] { FoodEntry.Count(((AppEntity) base.Owner).Food) };
                drawable.ToolTipText = A.Resources.GetString("Food for {0} meals.", objArray2);
            }
            list.Add(drawable);
            list.Add(new Drawable(base.Map.getNode("Oven").Location, "Oven"));
            list.Add(new Drawable(base.Map.getNode("InteriorWall2").Location, "InteriorWall2"));
            list.Add(new Drawable(base.Map.getNode("BuiltInDesk").Location, "BuiltInDesk"));
            if (flag)
            {
                if (owner.Has("Bed"))
                {
                    list.Add(new Drawable(base.Map.getNode("Bed1").Location, "Bed" + owner.ImageIndexOf("Bed")));
                    list.Add(new Drawable(base.Map.getNode("Lamp").Location, "Lamp"));
                }
                if (owner.Has("TreadMill"))
                {
                    list.Add(new Drawable(base.Map.getNode("TreadMill").Location, "TreadMill"));
                }
                if (owner.Has("Couch"))
                {
                    list.Add(new Drawable(base.Map.getNode("Couch1").Location, "Couch" + owner.ImageIndexOf("Couch")));
                    list.Add(new Drawable(base.Map.getNode("Chair3").Location, "Chair" + owner.ImageIndexOf("Couch")));
                    list.Add(new Drawable(base.Map.getNode("Chair3Back").Location, "ChairBack" + owner.ImageIndexOf("Couch")));
                    list.Add(new Drawable(base.Map.getNode("CofeeTable").Location, "CoffeeTable" + owner.ImageIndexOf("Couch")));
                }
                if (owner.Has("TV"))
                {
                    list.Add(new Drawable(base.Map.getNode("TVBack").Location, "TVBack" + owner.ImageIndexOf("TV")));
                }
                PointF tf4 = (PointF) base.Map.getNode("BuiltInDesk").Location;
                int num7 = 0x19;
                float cash = ((AppEntity) base.Owner).Cash;
                if (cash > 0.01f)
                {
                    for (num = 0; num < Math.Min((float) (4 * num7), cash / 20f); num++)
                    {
                        num2 = num / num7;
                        CashDrawable drawable9 = new CashDrawable(Point.Round(new PointF((tf4.X + 16f) + (num2 * 6), ((tf4.Y - ((num % num7) * 2)) + 41f) - (num2 * 3))), "Money", " ", cash) {
                            ToolTipText = Utilities.FC(cash, 2, A.Instance.CurrencyConversion)
                        };
                        Drawable drawable6 = drawable9;
                        list.Add(drawable6);
                    }
                }
                num = 0;
                while (num < ((AppEntity) base.Owner).Bills.Count)
                {
                    list.Add(new BillDrawable(Point.Round(new PointF(tf4.X + 74f, (tf4.Y - (num * 2)) + 12f)), "Paper", " "));
                    num++;
                }
                for (num = 0; num < ((AppEntity) base.Owner).Checks.Count; num++)
                {
                    list.Add(new CheckDrawable(Point.Round(new PointF(tf4.X + 96f, (tf4.Y - (num * 2)) + 11f)), "Check", " "));
                }
                if (owner.Has("Computer"))
                {
                    list.Add(new ComputerDrawable(Point.Round(new PointF(tf4.X + 45f, tf4.Y + 5f)), "Computer", " "));
                }
            }
            list.AddRange(list3);
            list.Add(new Drawable(base.Map.getNode("InteriorWall1").Location, "InteriorWall1"));
            if (flag)
            {
                if (owner.Has("Stereo"))
                {
                    list.Add(new Drawable(base.Map.getNode("Stereo").Location, "Stereo" + owner.ImageIndexOf("Stereo")));
                }
                if (owner != null)
                {
                    for (num = 0; num < Math.Min(8, owner.AcademicTaskHistory.Count); num++)
                    {
                        PointF tf5 = (PointF) base.Map.getNode("Diploma").Location;
                        int num9 = 0x1c;
                        drawable4 = new Drawable(new PointF(tf5.X + ((num % 4) * num9), (tf5.Y + (((num % 4) * num9) / 2)) + ((num / 4) * 0x1c)), "Diploma");
                        object[] objArray3 = new object[] { ((AttendClass) owner.AcademicTaskHistory.GetByIndex(num)).Course.Name };
                        drawable4.ToolTipText = A.Resources.GetString("Diploma for {0}", objArray3);
                        Drawable drawable7 = drawable4;
                        list.Add(drawable7);
                    }
                }
            }
            list.AddRange(list4);
            if (flag && (owner.Person.Task is BeSick))
            {
                PointF tf6 = (PointF) base.Map.getNode("Bed").Location;
                list.Add(new Drawable(new PointF(tf6.X + 84f, tf6.Y - 22f), "IceBag"));
            }
            return list;
        }
    }
}

