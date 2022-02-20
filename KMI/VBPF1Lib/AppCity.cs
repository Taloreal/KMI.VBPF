namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class AppCity : KMI.Biz.City.City
    {
        protected Hashtable buildingByIDcache;
        public static int[] BuildingHeights;
        public ArrayList Buses;
        public ArrayList Cars;
        public int downtownAve;
        public int downtownStreet;
        public ArrayList Pedestrians;

        public AppCity()
        {
            CityBlock block;
            int num;
            int num2;
            int lowerBound;
            int[] numArray = new int[] { 6, 0x10, 20, 0x18 };
            this.buildingByIDcache = new Hashtable();
            this.Cars = new ArrayList();
            this.Buses = new ArrayList();
            this.Pedestrians = new ArrayList();
            this.downtownAve = A.State.Random.Next(0, 3);
            this.downtownStreet = A.State.Random.Next(0, 6);
            int downtownAve = this.downtownAve;
            while (downtownAve == this.downtownAve)
            {
                downtownAve = A.State.Random.Next(0, 3);
            }
            int num5 = A.State.Random.Next(0, 5);
            float numPlayers = 1f;
            if (A.State.Multiplayer)
            {
                numPlayers = 1f + (((float) A.Settings.ExpectedMultiplayerPlayers) / 15f);
            }
            double num7 = 0.0;
            CityBlock[,] blocks = base.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num9 = blocks.GetUpperBound(1);
            for (num2 = blocks.GetLowerBound(0); num2 <= upperBound; num2++)
            {
                lowerBound = blocks.GetLowerBound(1);
                while (lowerBound <= num9)
                {
                    block = blocks[num2, lowerBound];
                    num = 0;
                    while (num < block.NumLots)
                    {
                        if (!A.State.Multiplayer)
                        {
                            num7 = (Math.Pow((double) ((block.Avenue - this.downtownAve) * 2), 2.0) + Math.Pow((double) (block.Street - this.downtownStreet), 2.0)) / 200.0;
                        }
                        if ((block[num] == null) && (A.State.Random.NextDouble() > num7))
                        {
                            block[num] = new AppBuilding(block, num, KMI.Biz.City.City.BuildingTypes[0]);
                        }
                        num++;
                    }
                    lowerBound++;
                }
            }
            for (num = 0; num < KMI.Biz.City.City.NUM_STREETS; num++)
            {
                base[this.downtownAve, num, 2] = new AppBuilding(base[this.downtownAve, num], 2, KMI.Biz.City.City.BuildingTypes[6]);
            }
            for (int i = num5; i < (num5 + 3); i++)
            {
                for (int j = 0; j < KMI.Biz.City.City.LOTS_PER_BLOCK[0]; j++)
                {
                    base[downtownAve, i, j] = new Classroom(base[downtownAve, i], j, KMI.Biz.City.City.BuildingTypes[5]);
                }
            }
            this.CreateCourses(0x10, 0x20, false);
            this.CreateCourses(0x24, 0x2a, false);
            this.CreateCourses(0x10, 0x20, true);
            this.AddCourse(A.Resources.GetString("Medical Degree"), A.Resources.GetString("Four-year medical school program."), 119975f, 0x2080, 0x10, 0x20, false, A.Resources.GetString("Bachelors Degree"));
            num = 0;
            while (num < (7f * numPlayers))
            {
                AppBuilding building = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, true, 2.5f, 30);
                base[building.Avenue, building.Street, building.Lot] = new Office(building.Block, building.Lot, KMI.Biz.City.City.BuildingTypes[3]);
                num++;
            }
            this.GenJobs(numPlayers);
            this.GenDwellings();
            this.CompileShops();
            this.AddSpecialBuildings();
            this.CreateBanks();
            blocks = base.blocks;
            upperBound = blocks.GetUpperBound(0);
            num9 = blocks.GetUpperBound(1);
            for (num2 = blocks.GetLowerBound(0); num2 <= upperBound; num2++)
            {
                for (lowerBound = blocks.GetLowerBound(1); lowerBound <= num9; lowerBound++)
                {
                    block = blocks[num2, lowerBound];
                    for (num = 0; num < block.NumLots; num++)
                    {
                        if (block[num] != null)
                        {
                            this.buildingByIDcache.Add(((AppBuilding) block[num]).ID, block[num]);
                        }
                    }
                }
            }
            this.AddBuses();
        }

        public void Add401Ks(float likelihoodPerBuilding)
        {
            ArrayList buildings = this.GetBuildings();
            foreach (AppBuilding building in buildings)
            {
                if (((building.Offerings.Count > 0) && (building.Offerings[0] is Job)) && (A.State.Random.NextDouble() < likelihoodPerBuilding))
                {
                    bool flag2 = true;
                    foreach (Offering offering in building.Offerings)
                    {
                        if (offering.Taken)
                        {
                            flag2 = false;
                        }
                    }
                    float num = (1 + A.State.Random.Next(3)) * 0.01f;
                    if (flag2)
                    {
                        foreach (Offering offering2 in building.Offerings)
                        {
                            ((WorkTask) offering2.PrototypeTask).R401KMatch = num;
                        }
                        foreach (Player player in A.State.Player.Values)
                        {
                            player.SendPeriodicMessage(A.Resources.GetString("Some jobs in the city have added 401K retirement plans."), "", NotificationColor.Green, 5f);
                        }
                    }
                }
            }
        }

        public void AddBuses()
        {
            Bus bus = new Bus(this.downtownAve, 0, true, 1);
            this.Buses.Add(bus);
            bus = new Bus(this.downtownAve, 2, true, -1);
            this.Buses.Add(bus);
            bus = new Bus(this.downtownAve, 3, true, 1);
            this.Buses.Add(bus);
            bus = new Bus(this.downtownAve, 4, true, -1);
            this.Buses.Add(bus);
            bus = new Bus(this.downtownAve, 5, true, 1);
            this.Buses.Add(bus);
            bus = new Bus(this.downtownAve, 7, true, -1);
            this.Buses.Add(bus);
        }

        public void AddCourse(string name, string resumeDescription, float cost, int hours, int startPeriod, int endPeriod, bool weekend, string prerequisite)
        {
            int num = 150;
            int num2 = 0;
            AppBuilding b = null;
        Label_000B:;
            if (((b == null) || (b.Offerings.Count > 1)) && (num2++ < num))
            {
                b = (AppBuilding) base.GetRandomBuilding(5);
                goto Label_000B;
            }
            int num3 = (endPeriod - startPeriod) / 2;
            if (b != null)
            {
                Course course = new Course {
                    Building = b,
                    PrototypeTask = new AttendClass()
                };
                course.PrototypeTask.StartPeriod = startPeriod;
                course.PrototypeTask.EndPeriod = endPeriod;
                course.Name = name;
                course.ResumeDescription = resumeDescription;
                course.Cost = cost;
                course.Days = Math.Min(hours / num3, 0x618);
                if (weekend)
                {
                    course.Days = Math.Min(course.Days, 0x270);
                }
                course.PrototypeTask.Weekend = weekend;
                course.Prerequisite = prerequisite;
                b.Offerings.Add(course);
                int num4 = 6;
                if (A.State.Multiplayer)
                {
                    num4 -= 2;
                }
                for (int i = 0; i < num4; i++)
                {
                    VBPFPerson person = this.AddGenericPerson(course, b);
                    ((AttendClass) person.Task).Course = course;
                    course.Students.Add(person);
                }
            }
        }

        public void AddFastFood(int startPeriod, int endPeriod, bool cashierOpening, bool shiftOpening, bool mgrOpening)
        {
            Building building = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, true, 3.1f, 30);
            int busyness = 2;
            FastFoodStore store = new FastFoodStore(building.Block, building.Lot, KMI.Biz.City.City.BuildingTypes[2], busyness);
            building = store;
            base[building.Avenue, building.Street, building.Lot] = building;
            store.AddGenericWorker(new WorkCounterFastFood(0));
            store.AddGenericWorker(new WorkMgrFastFood(0));
            for (int i = 0; i < 2; i++)
            {
                WorkTask task = null;
                if (shiftOpening)
                {
                    store.AddGenericWorker(new WorkCounterFastFood(1));
                    task = new WorkMgrFastFood(1);
                }
                else if (cashierOpening)
                {
                    store.AddGenericWorker(new WorkMgrFastFood(1));
                    task = new WorkCounterFastFood(1);
                }
                else if (mgrOpening)
                {
                    store.AddGenericWorker(new WorkCounterFastFood(1));
                    task = new WorkStoreMgrFastFood(1);
                }
                task.Weekend = i == 1;
                if (A.Settings.HealthInsuranceForFastFoodJobs)
                {
                    task.HealthInsurance = new InsurancePolicy(25f);
                }
                store.Offerings.Add(new Job(store, task, startPeriod, endPeriod));
            }
        }

        protected VBPFPerson AddGenericPerson(Offering o, AppBuilding b)
        {
            VBPFPerson activeObject = new VBPFPerson();
            Task task = o.CreateTask();
            task.Building = b;
            task.Owner = activeObject;
            activeObject.Task = task;
            A.Instance.Subscribe(activeObject, A.State.Now.AddHours((((float) task.StartPeriod) / 2f) - (0.20000000298023224 + (0.10000000149011612 * A.State.Random.NextDouble()))));
            return activeObject;
        }

        public void AddHealthInsurance(float likelihoodPerBuilding)
        {
            ArrayList buildings = this.GetBuildings();
            foreach (AppBuilding building in buildings)
            {
                if (((building.Offerings.Count > 0) && (building.Offerings[0] is Job)) && (A.State.Random.NextDouble() < likelihoodPerBuilding))
                {
                    bool flag2 = true;
                    foreach (Offering offering in building.Offerings)
                    {
                        if (offering.Taken)
                        {
                            flag2 = false;
                        }
                    }
                    float copay = new float[] { 10f, 25f, 50f }[A.State.Random.Next(3)];
                    if (flag2)
                    {
                        foreach (Offering offering2 in building.Offerings)
                        {
                            ((WorkTask) offering2.PrototypeTask).HealthInsurance = new InsurancePolicy(copay);
                        }
                        foreach (Player player in A.State.Player.Values)
                        {
                            player.SendPeriodicMessage(A.Resources.GetString("Some jobs in the city have added health insurance coverage."), "", NotificationColor.Green, 5f);
                        }
                    }
                }
            }
        }

        public void AddJobInvisible(int startPeriod, int endPeriod, int buildingTypeIndex, WorkTask prototypeTask, bool weekend)
        {
            int num = 0;
            int num2 = 50;
            while (num++ < num2)
            {
                AppBuilding randomBuilding = (AppBuilding) base.GetRandomBuilding(buildingTypeIndex);
                if ((randomBuilding != null) && (randomBuilding.Offerings.Count < ((num + 10) / 10)))
                {
                    Job job = new Job {
                        Building = randomBuilding,
                        PrototypeTask = prototypeTask
                    };
                    job.PrototypeTask.StartPeriod = startPeriod;
                    job.PrototypeTask.EndPeriod = endPeriod;
                    job.PrototypeTask.Weekend = weekend;
                    randomBuilding.Offerings.Add(job);
                    break;
                }
            }
        }

        public void AddOffice(int startPeriod, int endPeriod, bool deskOpening, bool supervisorOpening, bool mgrOpening)
        {
            int num = 0;
            int num2 = 50;
            while (num++ < num2)
            {
                AppBuilding randomBuilding = (AppBuilding) base.GetRandomBuilding(3);
                if ((randomBuilding != null) && (randomBuilding.Offerings.Count < ((num + 10) / 10)))
                {
                    VBPFPerson person;
                    Job job = new Job {
                        Building = randomBuilding,
                        PrototypeTask = new WorkOfficeDesk()
                    };
                    job.PrototypeTask.StartPeriod = startPeriod;
                    job.PrototypeTask.EndPeriod = endPeriod;
                    ((WorkOfficeDesk) job.PrototypeTask).chair = 3;
                    int num3 = 3;
                    if (deskOpening)
                    {
                        randomBuilding.Offerings.Add(job);
                    }
                    else
                    {
                        num3 = 4;
                    }
                    for (int i = 0; i < num3; i++)
                    {
                        ((WorkOfficeDesk) this.AddGenericPerson(job, randomBuilding).Task).chair = i;
                    }
                    job = new Job {
                        Building = randomBuilding,
                        PrototypeTask = new WorkOfficeSup()
                    };
                    job.PrototypeTask.StartPeriod = startPeriod;
                    job.PrototypeTask.EndPeriod = endPeriod;
                    ((WorkOfficeSup) job.PrototypeTask).chair = 4;
                    if (supervisorOpening)
                    {
                        randomBuilding.Offerings.Add(job);
                    }
                    else
                    {
                        person = this.AddGenericPerson(job, randomBuilding);
                    }
                    job = new Job {
                        Building = randomBuilding,
                        PrototypeTask = new WorkOfficeMgr()
                    };
                    job.PrototypeTask.StartPeriod = startPeriod;
                    job.PrototypeTask.EndPeriod = endPeriod;
                    ((WorkOfficeMgr) job.PrototypeTask).chair = 5;
                    if (mgrOpening)
                    {
                        randomBuilding.Offerings.Add(job);
                    }
                    else
                    {
                        person = this.AddGenericPerson(job, randomBuilding);
                    }
                    break;
                }
            }
        }

        public void AddSpecialBuildings()
        {
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[9];
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[11];
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[13];
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[14];
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[15];
            base.GetRandomBuilding(0).BuildingType = KMI.Biz.City.City.BuildingTypes[0x10];
        }

        public AppBuilding BuildingByID(long ID)
        {
            return (AppBuilding) this.buildingByIDcache[ID];
        }

        public Bus BusAt(int street, int direction)
        {
            foreach (Bus bus in this.Buses)
            {
                if ((bus.Location.Y == street) && (Math.Sign(bus.DY) == direction))
                {
                    return bus;
                }
            }
            return null;
        }

        public void CompileShops()
        {
            for (int i = 0; i != 3; i++)
            {
                AppBuilding randomBuilding = (AppBuilding) base.GetRandomBuilding(0);
                randomBuilding.BuildingType = KMI.Biz.City.City.BuildingTypes[12];
                randomBuilding.Prices = new float[A.State.PurchasableItems.Count];
                for (int j = 0; j < randomBuilding.Prices.Length; j++)
                {
                    randomBuilding.Prices[j] = (float) ((A.State.Random.NextDouble() * 0.2) + 0.9);
                }
                randomBuilding.SaleDiscounts = new float[A.State.PurchasableItems.Count];
            }
        }

        protected void CreateBanks()
        {
            int index = 0;
            float[] numArray = this.GenerateSpreadRandoms(3, 0.25f, 50, A.State.Random);
            float[] numArray2 = this.GenerateSpreadRandoms(3, 0.25f, 50, A.State.Random);
            float[] numArray3 = this.GenerateSpreadRandoms(3, 0.25f, 50, A.State.Random);
            string[] textArray1 = new string[] { "HSN Bank", "Herald Bank", "Olympic Bank" };
            foreach (string str in textArray1)
            {
                AppBuilding randomBuilding = (AppBuilding) base.GetRandomBuilding(0);
                randomBuilding.BuildingType = KMI.Biz.City.City.BuildingTypes[8];
                CheckingAccount account1 = new CheckingAccount(Utilities.RoundUpToPowerOfTen(50f * numArray[index], 1), Utilities.RoundUpToPowerOfTen(1000f * numArray[index], 2)) {
                    Building = randomBuilding,
                    BankName = A.Resources.GetString(str)
                };
                BankAccount account = account1;
                randomBuilding.Offerings.Add(account);
                SavingsAccount account2 = new SavingsAccount(Utilities.RoundUpToPowerOfTen(50f * numArray2[index], 1), (float) Math.Round((double) ((0.025 * (1f - numArray2[index])) + 0.005), 3), Utilities.RoundUpToPowerOfTen(750f * numArray2[index], 2)) {
                    Building = randomBuilding,
                    BankName = A.Resources.GetString(str)
                };
                account = account2;
                randomBuilding.Offerings.Add(account);
                CreditCardAccount account3 = new CreditCardAccount(Utilities.RoundUpToPowerOfTen(4000f * numArray3[index], 1), (float) Math.Round((double) (0.02 + (0.12 * (1f - numArray3[index]))), 3), Utilities.RoundUpToPowerOfTen(20f + (80f * numArray3[index]), 1)) {
                    Building = randomBuilding,
                    BankName = A.Resources.GetString(str)
                };
                account = account3;
                randomBuilding.Offerings.Add(account);
                index++;
            }
        }

        public void CreateCourses(int startPeriod, int endPeriod, bool weekend)
        {
            this.AddCourse(A.Resources.GetString("Food Service Mgt I"), A.Resources.GetString("Topics included food preparation techniques, food safety procedures, and supervision techniques."), 975f, 520, startPeriod, endPeriod, weekend, null);
            this.AddCourse(A.Resources.GetString("Intro to Data Entry"), A.Resources.GetString("Course covering data entry formats and tools as well as data security procedures."), 2975f, 0x410, startPeriod, endPeriod, weekend, null);
            this.AddCourse(A.Resources.GetString("IT Management"), A.Resources.GetString("Studied personnel supervision, planning and control, and It budgeting."), 4975f, 0x820, startPeriod, endPeriod, weekend, null);
            this.AddCourse(A.Resources.GetString("Associates Degree"), A.Resources.GetString("Full associates degree program covering multiple disciplines."), 9975f, 0x1040, startPeriod, endPeriod, weekend, null);
            this.AddCourse(A.Resources.GetString("Bachelors Degree"), A.Resources.GetString("Four-year multidisciplinary educational program."), 20000f, 0x1040, startPeriod, endPeriod, weekend, "Associates Degree");
            this.AddCourse(A.Resources.GetString("Web Design"), A.Resources.GetString("Course providing general instruction in website design."), 2975f, 0x410, startPeriod, endPeriod, weekend, null);
            this.AddCourse(A.Resources.GetString("Nursing Degree"), A.Resources.GetString("Two-year nursing program."), 13475f, 0x1040, startPeriod, endPeriod, weekend, null);
        }

        public void DeleteOffering(long ID)
        {
            CityBlock[,] blocks = base.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num2 = blocks.GetUpperBound(1);
            for (int i = blocks.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = blocks.GetLowerBound(1); j <= num2; j++)
                {
                    CityBlock block = blocks[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        if (block[k] != null)
                        {
                            AppBuilding building = (AppBuilding) block[k];
                            foreach (Offering offering in building.Offerings)
                            {
                                if (offering.ID == ID)
                                {
                                    building.Offerings.Remove(offering);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public float Distance(int ave1, int street1, int ave2, int street2)
        {
            return (float) Math.Pow(Math.Pow((double) (2 * (ave1 - ave2)), 2.0) + Math.Pow((double) (street1 - street2), 2.0), 0.5);
        }

        public AppBuilding FindInsideBuilding(AppEntity e)
        {
            AppBuilding building = null;
            if ((e.Dwelling != null) && e.Dwelling.Persons.Contains(e.Person))
            {
                return e.Dwelling;
            }
            foreach (Task task in e.GetAllTasks())
            {
                if ((task.Building != null) && task.Building.Persons.Contains(e.Person))
                {
                    building = task.Building;
                    break;
                }
            }
            if (building == null)
            {
                foreach (AppBuilding building3 in this.GetBuildings())
                {
                    if ((building3.Persons.Count > 0) && building3.Persons.Contains(e.Person))
                    {
                        return building3;
                    }
                }
            }
            return building;
        }

        public Offering FindOfferingForTask(Task t)
        {
            ArrayList offerings = this.GetOfferings();
            foreach (Offering offering in offerings)
            {
                Task prototypeTask = offering.PrototypeTask;
                if ((((prototypeTask != null) && (t.Building == offering.Building)) && ((t.GetType() == prototypeTask.GetType()) && (t.StartPeriod == prototypeTask.StartPeriod))) && (t.EndPeriod == prototypeTask.EndPeriod))
                {
                    return offering;
                }
            }
            return null;
        }

        public void GenDwellings()
        {
            int num = 0;
            if (A.State.Multiplayer)
            {
                num = Math.Max(0, A.Settings.ExpectedMultiplayerPlayers - 2);
            }
            for (int i = 0; i < (12 + num); i++)
            {
                AppBuilding building = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, false, 4f, 30);
                if (i < 1)
                {
                    building = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, true, 2f, 30);
                }
                building = new Dwelling(building.Block, building.Lot, KMI.Biz.City.City.BuildingTypes[1]);
                base[building.Avenue, building.Street, building.Lot] = building;
                Offering offering = new DwellingOffer();
                float num3 = this.Distance(this.downtownAve, this.downtownStreet, building.Avenue, building.Street);
                building.Rent = (int) Math.Max((float) 200f, (float) (1200f * (1f - (num3 / 7f))));
                offering.Building = building;
                building.Offerings.Add(offering);
            }
        }

        protected float[] GenerateSpreadRandoms(int numValues, float targetSeparation, int maxTries, Random random)
        {
            float[] numArray = new float[numValues];
            float num = 0f;
            for (int i = 0; i < numValues; i++)
            {
                for (int j = 0; j < maxTries; j++)
                {
                    bool flag = false;
                    num = (float) random.NextDouble();
                    for (int k = 0; k < i; k++)
                    {
                        if (Math.Abs((float) (num - numArray[k])) < targetSeparation)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        break;
                    }
                }
                numArray[i] = num;
            }
            return numArray;
        }

        public void GenJobs(float numPlayers)
        {
            int num = 0;
            for (num = 0; num < (2f * numPlayers); num++)
            {
                this.AddOffice(0x10, 0x20, true, false, false);
                this.AddOffice(0x10, 0x20, false, true, false);
                this.AddOffice(0x10, 0x20, false, false, true);
            }
            this.AddOffice(0x24, 0x2a, true, false, false);
            this.AddOffice(0x24, 0x2a, true, false, false);
            this.AddOffice(0x24, 0x2a, false, true, false);
            this.AddOffice(0x10, 0x2a, false, false, true);
            for (num = 0; num < (4f * numPlayers); num++)
            {
                AppBuilding building = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, true, 2.5f, 30);
                base[building.Avenue, building.Street, building.Lot] = new AppBuilding(building.Block, building.Lot, KMI.Biz.City.City.BuildingTypes[0x12]);
            }
            for (num = 0; num < (2f * numPlayers); num++)
            {
                this.AddJobInvisible(0x10, 0x20, 0x12, new WorkInternet1(), false);
                this.AddJobInvisible(0x24, 0x2a, 0x12, new WorkInternet1(), false);
                this.AddJobInvisible(0x10, 0x20, 0x12, new WorkInternet2(), false);
                this.AddJobInvisible(0x10, 0x20, 0x12, new WorkInternet3(), false);
            }
            for (num = 0; num < (3f * numPlayers); num++)
            {
                AppBuilding building2 = this.GetRandomBuilding(this.downtownAve, this.downtownStreet, true, 2.5f, 30);
                base[building2.Avenue, building2.Street, building2.Lot] = new AppBuilding(building2.Block, building2.Lot, KMI.Biz.City.City.BuildingTypes[0x13]);
            }
            for (num = 0; num < (1f * numPlayers); num++)
            {
                this.AddJobInvisible(0x10, 0x20, 0x13, new WorkHospital1(), false);
                this.AddJobInvisible(30, 0x2e, 0x13, new WorkHospital1(), false);
                this.AddJobInvisible(0x10, 0x20, 0x13, new WorkHospital1(), true);
                this.AddJobInvisible(30, 0x2e, 0x13, new WorkHospital1(), true);
            }
            for (num = 0; num < (2f * numPlayers); num++)
            {
                this.AddJobInvisible(0x10, 0x2c, 0x13, new WorkHospital2(), false);
                this.AddJobInvisible(0x10, 0x20, 0x13, new WorkHospital3(), false);
            }
            for (num = 0; num < (4f * numPlayers); num++)
            {
                AppBuilding randomBuilding = (AppBuilding) base.GetRandomBuilding(0);
                randomBuilding.BuildingType = KMI.Biz.City.City.BuildingTypes[4];
                Job job = new Job {
                    Building = randomBuilding,
                    PrototypeTask = new WorkPizzaGuy()
                };
                job.PrototypeTask.StartPeriod = 0x10;
                job.PrototypeTask.EndPeriod = 0x20;
                job.PrototypeTask.Weekend = (num % 2) == 0;
                randomBuilding.Offerings.Add(job);
                job = new Job {
                    Building = randomBuilding,
                    PrototypeTask = new WorkPizzaGuy()
                };
                job.PrototypeTask.StartPeriod = 0x26;
                job.PrototypeTask.EndPeriod = 0x2c;
                job.PrototypeTask.Weekend = (num % 2) == 0;
                randomBuilding.Offerings.Add(job);
            }
            for (num = 0; num < (2f * numPlayers); num++)
            {
                AppBuilding building4 = (AppBuilding) base.GetRandomBuilding(0);
                building4.BuildingType = KMI.Biz.City.City.BuildingTypes[0x11];
                Job job2 = new Job {
                    Building = building4,
                    PrototypeTask = new WorkDrugRep()
                };
                job2.PrototypeTask.StartPeriod = 0x10;
                job2.PrototypeTask.EndPeriod = 0x20;
                building4.Offerings.Add(job2);
            }
            for (num = 0; num < 2; num++)
            {
                this.AddFastFood(0x10, 0x20, true, false, false);
                this.AddFastFood(0x24, 0x2a, true, false, false);
                this.AddFastFood(0x10, 0x20, false, true, false);
                this.AddFastFood(0x10, 0x20, false, false, true);
            }
            this.AddFastFood(0x24, 0x2a, false, true, false);
        }

        public ArrayList GetBuildings()
        {
            ArrayList list = new ArrayList();
            CityBlock[,] blocks = base.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num2 = blocks.GetUpperBound(1);
            for (int i = blocks.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = blocks.GetLowerBound(1); j <= num2; j++)
                {
                    CityBlock block = blocks[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        if (block[k] != null)
                        {
                            list.Add(block[k]);
                        }
                    }
                }
            }
            return list;
        }

        public Offering GetOffering(long ID)
        {
            ArrayList offerings = this.GetOfferings();
            foreach (Offering offering in offerings)
            {
                if (offering.ID == ID)
                {
                    return offering;
                }
            }
            return null;
        }

        public ArrayList GetOfferings()
        {
            ArrayList list = new ArrayList();
            CityBlock[,] blocks = base.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num2 = blocks.GetUpperBound(1);
            for (int i = blocks.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = blocks.GetLowerBound(1); j <= num2; j++)
                {
                    CityBlock block = blocks[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        if (block[k] != null)
                        {
                            AppBuilding building = (AppBuilding) block[k];
                            foreach (Offering offering in building.Offerings)
                            {
                                list.Add(offering);
                            }
                        }
                    }
                }
            }
            return list;
        }

        public ArrayList GetOfferings(Type type)
        {
            ArrayList list = new ArrayList();
            CityBlock[,] blocks = base.blocks;
            int upperBound = blocks.GetUpperBound(0);
            int num2 = blocks.GetUpperBound(1);
            for (int i = blocks.GetLowerBound(0); i <= upperBound; i++)
            {
                for (int j = blocks.GetLowerBound(1); j <= num2; j++)
                {
                    CityBlock block = blocks[i, j];
                    for (int k = 0; k < block.NumLots; k++)
                    {
                        if (block[k] != null)
                        {
                            AppBuilding building = (AppBuilding) block[k];
                            foreach (Offering offering in building.Offerings)
                            {
                                if (offering.GetType() == type)
                                {
                                    list.Add(offering);
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        public AppBuilding GetRandomBuilding(int avenue, int street, bool closeTo, float distance, int tries)
        {
            AppBuilding randomBuilding = null;
            while (tries-- > 0)
            {
                randomBuilding = (AppBuilding) base.GetRandomBuilding(0);
                float num = this.Distance(randomBuilding.Avenue, randomBuilding.Street, avenue, street);
                if ((closeTo && (num < distance)) || (!closeTo && (num > distance)))
                {
                    return randomBuilding;
                }
            }
            return (AppBuilding) base.GetRandomBuilding(0);
        }

        public float LikelihoodOfCrime(Building bldg)
        {
            float maxValue = float.MaxValue;
            ArrayList buildings = this.GetBuildings();
            foreach (Building building in buildings)
            {
                if (building.BuildingType.Index == 10)
                {
                    PointF tf = KMI.Biz.City.City.Transform((float) building.Avenue, (float) building.Street, (float) building.Lot);
                    PointF tf2 = KMI.Biz.City.City.Transform((float) bldg.Avenue, (float) bldg.Street, (float) bldg.Lot);
                    maxValue = Math.Min(maxValue, Utilities.DistanceBetweenIsometric(tf, tf2));
                }
            }
            if (maxValue > 500f)
            {
                return 0f;
            }
            return ((1f - (maxValue / 500f)) * 0.01f);
        }

        public void RaiseSomeWages(float likelihoodPerJob)
        {
            ArrayList offerings = this.GetOfferings(typeof(Job));
            foreach (Offering offering in offerings)
            {
                if (!offering.Taken && (A.State.Random.NextDouble() < likelihoodPerJob))
                {
                    WorkTask prototypeTask = (WorkTask) offering.PrototypeTask;
                    prototypeTask.HourlyWage *= 1.15f;
                    foreach (Player player in A.State.Player.Values)
                    {
                        player.SendPeriodicMessage(A.Resources.GetString("The rate of pay has increased for some jobs in the city."), "", NotificationColor.Green, 5f);
                    }
                }
            }
        }

        public void SetupCondoOfferings()
        {
            AppBuilding current;
            ArrayList list = new ArrayList();
            IEnumerator enumerator = this.GetBuildings().GetEnumerator();
            while (enumerator.MoveNext())
            {
                current = (AppBuilding) enumerator.Current;
                if ((current is Dwelling) && !((Offering) current.Offerings[0]).Taken)
                {
                    list.Add(current);
                }
            }
            Utilities.Shuffle(list, A.State.Random);
            for (int i = 0; i < Math.Max(6, list.Count / 3); i++)
            {
                current = (AppBuilding) list[i];
                current.Offerings.Clear();
                DwellingOffer offer = new DwellingOffer {
                    Condo = true,
                    Building = current
                };
                current.Offerings.Add(offer);
            }
        }
    }
}

