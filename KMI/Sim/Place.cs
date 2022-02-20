namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Place
    {
        public static float DefaultSpeedLimit = 0.18f;
        public DateTime EndConstruction;
        public ArrayList LinkedPlaces;
        public PointF Location;
        public static DateTime NoConstruction = DateTime.MinValue;
        public float SpeedLimit;
        public float tempDistance;
        public bool tempIsDead;
        public Place tempNextLink;

        public Place()
        {
            this.LinkedPlaces = new ArrayList();
            this.SpeedLimit = DefaultSpeedLimit;
            this.EndConstruction = NoConstruction;
        }

        public Place(PointF location)
        {
            this.LinkedPlaces = new ArrayList();
            this.SpeedLimit = DefaultSpeedLimit;
            this.EndConstruction = NoConstruction;
            this.Location = location;
        }

        public override bool Equals(object obj)
        {
            Place place = (Place) obj;
            return (Utilities.DistanceBetween(this.Location, place.Location) < 3f);
        }

        public void Link(Place otherPlace)
        {
            if (!this.LinkedPlaces.Contains(otherPlace))
            {
                this.LinkedPlaces.Add(otherPlace);
                otherPlace.Link(this);
            }
        }

        public bool UnderConstruction
        {
            get
            {
                return (S.State.Now < this.EndConstruction);
            }
        }

        [Serializable]
        public class PlaceLoader
        {
            protected string links;
            protected string name;
            protected int x;
            protected int y;

            public string Links
            {
                get
                {
                    return this.links;
                }
                set
                {
                    this.links = value;
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

            public int X
            {
                get
                {
                    return this.x;
                }
                set
                {
                    this.x = value;
                }
            }

            public int Y
            {
                get
                {
                    return this.y;
                }
                set
                {
                    this.y = value;
                }
            }
        }
    }
}

