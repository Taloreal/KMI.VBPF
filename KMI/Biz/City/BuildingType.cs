namespace KMI.Biz.City
{
    using KMI.Sim;
    using System;

    [Serializable]
    public class BuildingType
    {
        protected bool canTakeSign;
        public int Height;
        protected int index;
        protected int maxOccupants;
        protected string name;

        public int CompareTo(object obj)
        {
            return (this.index - ((BuildingType)obj).index);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return (this.index == ((BuildingType)obj).index);
        }

        public override int GetHashCode()
        {
            return this.index;
        }

        public static bool operator ==(BuildingType obj1, BuildingType obj2)
        {
            if (object.ReferenceEquals(obj1, null))
            {
                return object.ReferenceEquals(obj2, null);
            }
            return obj1.Equals(obj2);
        }

        public static bool operator !=(BuildingType obj1, BuildingType obj2)
        {
            return !(obj1 == obj2);
        }

        public bool CanTakeSign
        {
            get
            {
                return this.canTakeSign;
            }
            set
            {
                this.canTakeSign = value;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
                this.Height = S.Resources.GetImage("Building" + this.index).Height;
            }
        }

        public int MaxOccupants
        {
            get
            {
                return this.maxOccupants;
            }
            set
            {
                this.maxOccupants = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
    }
}
