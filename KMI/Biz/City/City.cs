namespace KMI.Biz.City
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Reflection;

    [Serializable]
    public class City : ActiveObject
    {
        public static int AVENUE_INVERSE_ADJUSTMENT = 0;
        public static int AVENUE_REGION_OFFSET = 0x16b;
        public static float[] AVENUE_SPACING = new float[] { 135.17f, 115.46f, 115.46f };
        public static int AVENUE_VIEWING_REGIONS = 1;
        public static float AVENUE_WIDTH = 24.6f;
        public static float[] BLOCK_HEIGHTS = new float[] { 153.17f, 127.46f, 127.46f };
        public static float BLOCK_WIDTH = 50.73f;
        protected CityBlock[,] blocks = new CityBlock[NUM_AVENUES, NUM_STREETS];
        public static int BuildingTypeCount = 6;
        public static BuildingType[] BuildingTypes;
        protected int directionOfMigration = -1;
        public static float LOT_SPACING = 23f;
        public static int[] LOTS_PER_BLOCK = new int[] { 5, 4, 4 };
        public static int NUM_AVENUES = 3;
        public static int NUM_STREETS = 8;
        public static PointF ORIGIN = new PointF(342f, 69f);
        public static int STREET_INVERSE_ADJUSTMENT = 0;
        public static int STREET_REGION_OFFSET = 0x16b;
        public static float STREET_SPACING = 45.42f;
        public static int STREET_VIEWING_REGIONS = 1;
        public static float STREET_WIDTH = 28.83f;
        public int[,,] VacantLotImageIndices;
        public string VacantLotImageName;
        public int VacantLotImages;
        public static Point WHOLE_CITY_OFFSET = new Point(300, 100);
        public static int ZONES = 9;

        public City()
        {
            for (int i = 0; i < NUM_STREETS; i++)
            {
                float y = 0f;
                for (int k = 0; k < NUM_AVENUES; k++)
                {
                    float x = i * BLOCK_WIDTH;
                    this.blocks[k, i] = new CityBlock(this, k, i, LOTS_PER_BLOCK[k], new SizeF(BLOCK_WIDTH, BLOCK_HEIGHTS[k]), new PointF(x, y), AVENUE_WIDTH, STREET_WIDTH);
                    y += BLOCK_HEIGHTS[k];
                }
            }
            float num = 0f;
            for (int j = 0; j < NUM_AVENUES; j++)
            {
                num += BLOCK_HEIGHTS[j];
            }
        }

        public static void BoundCenteringAveAndStreet(ref int avenue, ref int street)
        {
            int num = NUM_AVENUES / AVENUE_VIEWING_REGIONS;
            int num2 = NUM_STREETS / STREET_VIEWING_REGIONS;
            int num3 = avenue - (num / 2);
            int num4 = (num3 + num) - 1;
            if (num3 < 0)
            {
                num4 = num - 1;
                num3 = 0;
                avenue = num / 2;
            }
            if (num4 > (NUM_AVENUES - 1))
            {
                num3 = NUM_AVENUES - num;
                num4 = NUM_AVENUES - 1;
                avenue = num3 + (num / 2);
            }
            int num5 = street - (num2 / 2);
            int num6 = (num5 + num2) - 1;
            if (num5 < 0)
            {
                num6 = num2 - 1;
                num5 = 0;
                street = num2 / 2;
            }
            if (num6 > (NUM_STREETS - 1))
            {
                num5 = NUM_STREETS - num2;
                num6 = NUM_STREETS - 1;
                street = num5 + (num2 / 2);
            }
        }

        public static float Distance(CityBlock b1, CityBlock b2)
        {
            return (float) (Math.Abs((int) (b1.Street - b2.Street)) + (2 * Math.Abs((int) (b1.Avenue - b2.Avenue))));
        }

        public ArrayList GetDrawables(int centerAvenue, int centerStreet)
        {
            int num = (centerAvenue * NUM_AVENUES) / AVENUE_VIEWING_REGIONS;
            int num2 = ((centerAvenue + 1) * NUM_AVENUES) / AVENUE_VIEWING_REGIONS;
            int num3 = (centerStreet * NUM_STREETS) / STREET_VIEWING_REGIONS;
            int num4 = ((centerStreet + 1) * NUM_STREETS) / STREET_VIEWING_REGIONS;
            ArrayList list = new ArrayList();
            for (int i = num; i < num2; i++)
            {
                for (int j = num3; j < num4; j++)
                {
                    list.AddRange(this[i, j].GetDrawables(centerAvenue, centerStreet));
                }
            }
            return list;
        }

        public ArrayList GetDrawablesCenteredOn(int avenue, int street)
        {
            int num = NUM_AVENUES / AVENUE_VIEWING_REGIONS;
            int num2 = NUM_STREETS / STREET_VIEWING_REGIONS;
            int num3 = avenue - (num / 2);
            int num4 = (num3 + num) - 1;
            int num5 = street - (num2 / 2);
            int num6 = (num5 + num2) - 1;
            ArrayList list = new ArrayList();
            for (int i = num3; i <= num4; i++)
            {
                for (int j = num5; j <= num6; j++)
                {
                    list.AddRange(this[i, j].GetDrawables(avenue, street));
                }
            }
            return list;
        }

        public ArrayList GetDrawablesWholeCity()
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < NUM_AVENUES; i++)
            {
                for (int j = 0; j < NUM_STREETS; j++)
                {
                    list.AddRange(this[i, j].GetDrawablesWholeCity());
                }
            }
            return list;
        }

        public Building GetRandomBuilding(int buildingTypeIndex)
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < NUM_AVENUES; i++)
            {
                for (int j = 0; j < NUM_STREETS; j++)
                {
                    for (int k = 0; k < this[i, j].NumLots; k++)
                    {
                        if ((this[i, j, k] != null) && (this[i, j, k].BuildingType.Index == buildingTypeIndex))
                        {
                            list.Add(this[i, j, k]);
                        }
                    }
                }
            }
            int count = list.Count;
            if (count == 0)
            {
                return null;
            }
            return (Building) list[S.State.Random.Next(count)];
        }

        public static bool InverseTransformWholeCity(PointF screenLoc, ref int avenue, ref int street, ref int lot)
        {
            float num = AVENUE_VIEWING_REGIONS;
            float x = screenLoc.X;
            float y = screenLoc.Y;
            float num4 = ((((-num * (((x - WHOLE_CITY_OFFSET.X) - (2f * y)) + (2 * WHOLE_CITY_OFFSET.Y))) - ORIGIN.X) + (2f * ORIGIN.Y)) + AVENUE_INVERSE_ADJUSTMENT) / (2f * AVENUE_SPACING[0]);
            float num5 = ((((num * (((x - WHOLE_CITY_OFFSET.X) + (2f * y)) - (2 * WHOLE_CITY_OFFSET.Y))) - ORIGIN.X) - (2f * ORIGIN.Y)) + STREET_INVERSE_ADJUSTMENT) / (2f * STREET_SPACING);
            avenue = (int) Math.Floor((double) num4);
            street = (int) Math.Floor((double) num5);
            lot = (int) Math.Min(Math.Floor((double) ((num4 - ((float) avenue)) * LOTS_PER_BLOCK[0])), (double) (LOTS_PER_BLOCK[0] - 1));
            return ((((num4 < NUM_AVENUES) && (num5 < NUM_STREETS)) && (num4 >= 0f)) && (num5 >= 0f));
        }

        public bool InViewCenteredOn(int avenue, int street, Point location)
        {
            int num = NUM_AVENUES / AVENUE_VIEWING_REGIONS;
            int num2 = NUM_STREETS / STREET_VIEWING_REGIONS;
            int num3 = avenue - (num / 2);
            int num4 = (num3 + num) - 1;
            int num5 = street - (num2 / 2);
            int num6 = (num5 + num2) - 1;
            return (((location.X >= num3) && (location.X <= num4)) && ((location.Y >= num5) & (location.Y <= num6)));
        }

        public void PickVacantBuilding(int buildingTypeIndex, ref int avenue, ref int street, ref int lot)
        {
            ArrayList list = new ArrayList();
            CityBlock[,] blocks = this.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num2 = blocks.GetUpperBound(1);
            for (int i = blocks.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = blocks.GetLowerBound(1); j <= num2; j++)
                {
                    CityBlock block = blocks[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        Building building = block[k];
                        if ((building != null) && (((building != null) && (building.BuildingType == BuildingTypes[buildingTypeIndex])) && (building.Owner == null)))
                        {
                            list.Add(building);
                        }
                    }
                }
            }
            if (list.Count > 0)
            {
                Building building2 = (Building) list[S.State.Random.Next(list.Count)];
                avenue = building2.Block.Avenue;
                street = building2.Block.Street;
                lot = building2.Lot;
            }
        }

        public void RecomputeReachForBuildings(float scaleFactor)
        {
            for (int i = 0; i < NUM_AVENUES; i++)
            {
                for (int j = 0; j < NUM_STREETS; j++)
                {
                    CityBlock block = this[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        Building building = block[k];
                        if (building != null)
                        {
                            float num4 = 0f;
                            if (building.OnAvenue > -1)
                            {
                                num4 += this[building.OnAvenue, block.Street].AvenueTraffic.GetDensity();
                            }
                            num4 += block.StreetTraffic.GetDensity();
                            building.Reach = (int) (num4 * scaleFactor);
                        }
                    }
                }
            }
        }

        public static PointF Transform(float avenue, float street, float lotIndex)
        {
            float num = 0f;
            for (int i = 0; i < avenue; i++)
            {
                num += AVENUE_SPACING[i];
            }
            num += lotIndex * LOT_SPACING;
            float num2 = street * STREET_SPACING;
            return new PointF((ORIGIN.X - num) + num2, (ORIGIN.Y + (num / 2f)) + (num2 / 2f));
        }

        public static PointF Transform2(float avenue, float street, float lotIndex, int centeringAve, int centeringStreet)
        {
            float num = centeringAve * AVENUE_SPACING[0];
            float num2 = centeringStreet * STREET_SPACING;
            float num3 = avenue * AVENUE_SPACING[0];
            num3 += lotIndex * LOT_SPACING;
            float num4 = street * STREET_SPACING;
            return new PointF((((ORIGIN.X - num3) + num4) + num) - num2, (((ORIGIN.Y + (num3 / 2f)) + (num4 / 2f)) - (num / 2f)) - (num2 / 2f));
        }

        public static PointF TransformWholeCity(float ave, float street, float lot)
        {
            float num = ave * AVENUE_SPACING[0];
            num += lot * LOT_SPACING;
            float num2 = street * STREET_SPACING;
            return new PointF((((ORIGIN.X - num) + num2) / ((float) AVENUE_VIEWING_REGIONS)) + WHOLE_CITY_OFFSET.X, (((ORIGIN.Y + (num / 2f)) + (num2 / 2f)) / ((float) STREET_VIEWING_REGIONS)) + WHOLE_CITY_OFFSET.Y);
        }

        public int DirectionOfMigration
        {
            set
            {
                this.directionOfMigration = value;
            }
        }

        public bool Filled
        {
            get
            {
                for (int i = 0; i < NUM_AVENUES; i++)
                {
                    for (int j = 0; j < NUM_STREETS; j++)
                    {
                        for (int k = 0; k < this[i, j].NumLots; k++)
                        {
                            if (this[i, j, k] == null)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public Building this[int avenueIndex, int streetIndex, int lotIndex]
        {
            get
            {
                return this.blocks[avenueIndex, streetIndex][lotIndex];
            }
            set
            {
                this.blocks[avenueIndex, streetIndex][lotIndex] = value;
            }
        }

        public CityBlock this[int avenueIndex, int streetIndex]
        {
            get
            {
                return this.blocks[avenueIndex, streetIndex];
            }
        }
    }
}

