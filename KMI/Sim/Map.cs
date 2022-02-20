namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class Map
    {
        public ArrayList places = new ArrayList();

        protected static float PathLength(ArrayList points)
        {
            float num = 0f;
            PointF empty = PointF.Empty;
            foreach (PointF tf2 in points)
            {
                if (!empty.IsEmpty)
                {
                    num += Utilities.DistanceBetweenIsometric(empty, tf2);
                }
                empty = tf2;
            }
            return num;
        }

        public ArrayList ShortestPath(Place begin, Place end, ref ArrayList speeds, ref float totalTime)
        {
            foreach (Place place2 in this.places)
            {
                place2.tempDistance = float.MaxValue;
                place2.tempIsDead = false;
                place2.tempNextLink = null;
                if (place2 == end)
                {
                    place2.tempDistance = 0f;
                }
            }
            Place place = null;
            foreach (Place place3 in this.places)
            {
                float maxValue = float.MaxValue;
                foreach (Place place4 in this.places)
                {
                    if (!place4.tempIsDead && (place4.tempDistance < maxValue))
                    {
                        place = place4;
                        maxValue = place4.tempDistance;
                    }
                }
                foreach (Place place5 in place.LinkedPlaces)
                {
                    float num2 = place.tempDistance + (Utilities.DistanceBetweenIsometric(place.Location, place5.Location) / Math.Min(place.SpeedLimit, place5.SpeedLimit));
                    if (place5.UnderConstruction)
                    {
                        num2 = float.MaxValue;
                    }
                    if (place5.tempDistance > num2)
                    {
                        place5.tempDistance = num2;
                        place5.tempNextLink = place;
                    }
                }
                place.tempIsDead = true;
            }
            totalTime = begin.tempDistance;
            if (begin.tempDistance > 1.701412E+38f)
            {
                return null;
            }
            ArrayList list = new ArrayList();
            speeds = new ArrayList();
            for (place = begin; place != null; place = place.tempNextLink)
            {
                list.Add(place.Location);
                if (place.tempNextLink != null)
                {
                    float num3 = Math.Min(place.SpeedLimit, place.tempNextLink.SpeedLimit);
                    speeds.Add(num3);
                }
            }
            return list;
        }
    }
}

