namespace KMI.Biz.City
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Building : ActiveObject
    {
        public Entity BillboardOwner;
        protected CityBlock block;
        public KMI.Biz.City.BuildingType BuildingType;
        protected int lotIndex;
        protected ArrayList occupants = new ArrayList();
        public Entity Owner;
        protected int reach;
        protected int rent;

        public Building(CityBlock block, int lotIndex, KMI.Biz.City.BuildingType type)
        {
            this.block = block;
            this.lotIndex = lotIndex;
            this.BuildingType = type;
        }

        public virtual ArrayList GetDrawables(int centerAvenue, int centerStreet)
        {
            ArrayList list = new ArrayList();
            Point location = Point.Round(KMI.Biz.City.City.Transform2((float) this.Avenue, (float) this.Street, (float) this.lotIndex, centerAvenue, centerStreet));
            long ownerID = -1L;
            if (this.Owner != null)
            {
                ownerID = this.Owner.ID;
            }
            string billboardOwnerName = null;
            if (this.BillboardOwner != null)
            {
                billboardOwnerName = this.BillboardOwner.Name;
            }
            list.Add(new BuildingDrawable(location, "Building" + this.BuildingType.Index, this.BuildingType, this.Avenue, this.Street, this.Lot, ownerID, (float) this.Reach, (float) this.Rent, billboardOwnerName));
            return list;
        }

        public virtual ArrayList GetDrawablesWholeCity()
        {
            ArrayList list = new ArrayList();
            Point location = Point.Round(KMI.Biz.City.City.TransformWholeCity((float) this.Avenue, (float) this.Street, (float) this.lotIndex));
            long ownerID = -1L;
            if (this.Owner != null)
            {
                ownerID = this.Owner.ID;
            }
            string billboardOwnerName = null;
            if (this.BillboardOwner != null)
            {
                billboardOwnerName = this.BillboardOwner.Name;
            }
            list.Add(new BuildingDrawable(location, "BuildingSmall" + this.BuildingType.Index, this.BuildingType, this.Avenue, this.Street, this.Lot, ownerID, (float) this.Reach, (float) this.Rent, billboardOwnerName));
            return list;
        }

        public int Avenue
        {
            get
            {
                return this.block.Avenue;
            }
        }

        public CityBlock Block
        {
            get
            {
                return this.block;
            }
        }

        public int Lot
        {
            get
            {
                return this.lotIndex;
            }
        }

        public ArrayList Occupants
        {
            get
            {
                return this.occupants;
            }
            set
            {
                this.occupants = value;
            }
        }

        public int OnAvenue
        {
            get
            {
                if ((this.Lot == 0) && (this.Avenue > 0))
                {
                    return (this.Avenue - 1);
                }
                if (this.Lot == (this.block.NumLots - 1))
                {
                    return this.Avenue;
                }
                return -1;
            }
        }

        public int Reach
        {
            get
            {
                return this.reach;
            }
            set
            {
                this.reach = value;
            }
        }

        public int Rent
        {
            get
            {
                return this.rent;
            }
            set
            {
                this.rent = value;
            }
        }

        public int Street
        {
            get
            {
                return this.block.Street;
            }
        }
    }
}

