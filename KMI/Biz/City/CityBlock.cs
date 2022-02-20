namespace KMI.Biz.City
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public class CityBlock : ActiveObject
    {
        public int Avenue;
        protected Traffic avenueTraffic;
        protected float avenueWidth;
        protected Building[] buildings;
        protected KMI.Biz.City.City City;
        protected PointF location;
        protected int numLots;
        public int Street;
        protected Traffic streetTraffic;

        public CityBlock(KMI.Biz.City.City city, int avenue, int street, int numLots, SizeF size, PointF location, float avenueWidth, float streetWidth)
        {
            this.City = city;
            this.Avenue = avenue;
            this.Street = street;
            this.numLots = numLots;
            this.location = location;
            this.avenueWidth = 0f;
            this.buildings = new Building[numLots];
            this.streetTraffic = new Traffic(false, this);
            this.avenueTraffic = new Traffic(true, this);
        }

        public bool GetConstruction()
        {
            return (this.avenueWidth > 0f);
        }

        public ArrayList GetDrawables(int centerAvenue, int centerStreet)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < this.numLots; i++)
            {
                if (this.buildings[i] != null)
                {
                    list.AddRange(this.buildings[i].GetDrawables(centerAvenue, centerStreet));
                }
                else if (this.City.VacantLotImageName != null)
                {
                    FlexDrawable drawable = new FlexDrawable(KMI.Biz.City.City.Transform2((float) this.Avenue, (float) this.Street, (float) i, centerAvenue, centerStreet), this.City.VacantLotImageName + this.City.VacantLotImageIndices[this.Avenue, this.Street, i]) {
                        VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom
                    };
                    list.Add(drawable);
                }
            }
            if (!this.GetConstruction())
            {
                list.AddRange(this.streetTraffic.GetDrawables(this, centerAvenue, centerStreet));
                list.AddRange(this.avenueTraffic.GetDrawables(this, centerAvenue, centerStreet));
            }
            return list;
        }

        public ArrayList GetDrawablesWholeCity()
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < this.numLots; i++)
            {
                if (this.buildings[i] != null)
                {
                    list.AddRange(this.buildings[i].GetDrawablesWholeCity());
                }
                else if (this.City.VacantLotImageName != null)
                {
                    FlexDrawable drawable = new FlexDrawable(KMI.Biz.City.City.TransformWholeCity((float) this.Avenue, (float) this.Street, (float) i), this.City.VacantLotImageName + this.City.VacantLotImageIndices[this.Avenue, this.Street, i] + "Small") {
                        VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom
                    };
                    list.Add(drawable);
                }
            }
            if (!this.GetConstruction())
            {
                list.AddRange(this.streetTraffic.GetDrawablesWholeCity(this));
                list.AddRange(this.avenueTraffic.GetDrawablesWholeCity(this));
            }
            return list;
        }

        public override void NewWeek()
        {
            for (int i = 0; i < this.NumLots; i++)
            {
                Building building = this[i];
                if (building != null)
                {
                    building.NewWeek();
                }
            }
            if (this.avenueWidth > 0f)
            {
                this.avenueWidth--;
            }
        }

        public void SetConstruction(int weeksRemaining)
        {
            this.avenueWidth = weeksRemaining;
        }

        public Traffic AvenueTraffic
        {
            get
            {
                return this.avenueTraffic;
            }
            set
            {
                this.avenueTraffic = value;
            }
        }

        public Building this[int lotIndex]
        {
            get
            {
                return this.buildings[lotIndex];
            }
            set
            {
                this.buildings[lotIndex] = value;
            }
        }

        public int NumLots
        {
            get
            {
                return this.numLots;
            }
        }

        public Traffic StreetTraffic
        {
            get
            {
                return this.streetTraffic;
            }
            set
            {
                this.streetTraffic = value;
            }
        }
    }
}

