namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;

    [Serializable]
    public class FastFoodStore : AppBuilding
    {
        private float busyness;
        public bool Closed;
        public ArrayList Customers;
        private int[,] foodstacks;
        private const int MaxStack = 5;
        public bool[] OrderIn;
        public bool[] OrderUp;

        public FastFoodStore(CityBlock block, int lotIndex, BuildingType type, int busyness) : base(block, lotIndex, type)
        {
            this.foodstacks = new int[2, 8];
            this.OrderIn = new bool[2];
            this.OrderUp = new bool[2];
            this.Customers = new ArrayList();
            this.Closed = false;
            base.Map = AppConstants.Work0Map;
            base.EntryPoint = (PointF)base.Map.getNode("EntryPoint").Location;
            S.Instance.Subscribe(this, Simulator.TimePeriod.Step);
            S.Instance.Subscribe(this, Simulator.TimePeriod.Week);
            for (int i = 0; i < (10 * busyness); i++)
            {
                VBPFPerson person = new VBPFPerson();
                person.Task = new ShopFastFood(person, this);
                DateTime wakeupTime = A.State.Now.AddHours((double)Utilities.GetNormalDistribution(12f, 9f, A.State.Random));
                S.Instance.Subscribe(person, wakeupTime);
            }
            this.busyness = busyness;
        }

        public void AddGenericWorker(Task task)
        {
            VBPFPerson activeObject = new VBPFPerson
            {
                Task = task
            };
            activeObject.Task.Owner = activeObject;
            activeObject.Task.Building = this;
            activeObject.Task.EndPeriod = -1;
            activeObject.Location = (PointF)base.Map.getNode("EntryPoint").Location;
            S.Instance.Subscribe(activeObject, Simulator.TimePeriod.Step);
            base.Persons.Add(activeObject);
        }

        public bool CustomerReadyAtRegister(int register)
        {
            foreach (VBPFPerson person in this.Customers)
            {
                if (person.Task is ShopFastFood)
                {
                    ShopFastFood task = (ShopFastFood)person.Task;
                    if (!((task.State != ShopFastFood.States.AtCounter) || task.OrderTaken) && (task.Register == register))
                    {
                        task.OrderTaken = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public override string GetBackgroundImage()
        {
            return "WorkBack";
        }

        public override ArrayList GetInsideDrawables()
        {
            ArrayList list = new ArrayList();
            ArrayList c = new ArrayList();
            ArrayList list3 = new ArrayList();
            foreach (VBPFPerson person in this.Customers)
            {
                if ((person.Location.X - (person.Location.Y * 2f)) > -380f)
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
            list.Add(new Drawable(base.Map.getNode("FoodWall0").Location, "FoodWall"));
            list.Add(new Drawable(base.Map.getNode("SodaMachine").Location, "SodaMachine"));
            list.Add(new Drawable(base.Map.getNode("FoodWall1").Location, "FoodWall"));
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= this.foodstacks.GetUpperBound(1); j++)
                {
                    PointF location = (PointF)base.Map.getNode("FoodWall" + i).Location;
                    for (int k = 0; k < this.foodstacks[i, j]; k++)
                    {
                        list.Add(new Drawable(new PointF(((location.X + 6f) + (12 * j)) + (2 * k), ((location.Y + 44f) + (6 * j)) - (8 * k)), "FoodContainer" + (j % 2)));
                    }
                }
            }
            list.Add(new Drawable(base.Map.getNode("FoodWallTopa").Location, "FoodWallTop"));
            list.Add(new Drawable(base.Map.getNode("FoodWallTopb").Location, "FoodWallTop"));
            list.Add(new Drawable(base.Map.getNode("TreeFastFooda").Location, "TreeFastFood"));
            list.Add(new Drawable(base.Map.getNode("TreeFastFoodb").Location, "TreeFastFood"));
            ArrayList list4 = new ArrayList();
            foreach (VBPFPerson person2 in base.Persons)
            {
                list4.AddRange(person2.GetDrawables());
            }
            list4.Sort();
            list.AddRange(list4);
            list.Add(new Drawable(base.Map.getNode("CounterFastFood").Location, "CounterFastFood"));
            list.AddRange(c);
            list.Add(new Drawable(base.Map.getNode("LeftGlass").Location, "LeftGlass"));
            list.Add(new Drawable(base.Map.getNode("RightGlass").Location, "RightGlass"));
            list.Add(new Drawable(base.Map.getNode("PlantsFrontLeft").Location, "PlantsFrontLeft"));
            list.Add(new Drawable(base.Map.getNode("PlantsFrontRight").Location, "PlantsFrontRight"));
            list.AddRange(list3);
            list.Add(new Drawable(base.Map.getNode("Bar").Location, "Bar"));
            return list;
        }

        public override bool NewStep()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j <= this.foodstacks.GetUpperBound(1); j++)
                {
                    if (this.foodstacks[i, j] < 1)
                    {
                        this.foodstacks[i, j] = 5;
                    }
                }
            }
            return false;
        }

        public override void NewWeek()
        {
            if ((A.Settings.CloseSomeBusinesses && (this.busyness < 2f)) && (A.State.Random.NextDouble() < 0.01))
            {
                this.Retire();
            }
        }

        public override void Retire()
        {
            base.Retire();
            foreach (VBPFPerson person in base.Persons)
            {
                if (person.Drone)
                {
                    person.Retire();
                }
                else if ((person.Task is WorkMgrFastFood) || (person.Task is WorkCounterFastFood))
                {
                    person.Task.CleanUp();
                }
            }
            base.Persons.Clear();
            foreach (VBPFPerson person in this.Customers)
            {
                person.Retire();
            }
            this.Customers.Clear();
            this.Closed = true;
            base.Offerings.Clear();
            foreach (AppEntity entity in A.State.Entity.Values)
            {
                ArrayList list = new ArrayList(entity.GetAllTasks());
                foreach (Task task in list)
                {
                    if (!((task is TravelTask) || (task.Building != this)))
                    {
                        A.Adapter.DeleteTask(entity.ID, task.ID);
                        entity.Player.SendMessage(A.Resources.GetString("Trouble...You have lost your job as a {0} because the business closed due to lack of revenue.", new object[] { ((WorkTask)task).Name() }), "", NotificationColor.Yellow);
                    }
                }
            }
        }

        public int SmallestOpenLine()
        {
            bool[] flagArray = new bool[2];
            bool[] flagArray2 = new bool[2];
            foreach (VBPFPerson person in base.Persons)
            {
                if (person.Task is WorkCounterFastFood)
                {
                    flagArray[((WorkCounterFastFood)person.Task).Register] = true;
                }
                if (person.Task is WorkMgrFastFood)
                {
                    flagArray2[((WorkMgrFastFood)person.Task).Register] = true;
                }
            }
            if (flagArray[0] && flagArray2[0])
            {
                if (!(flagArray[1] && flagArray2[1]))
                {
                    return 0;
                }
                int[] numArray = new int[2];
                foreach (VBPFPerson person in this.Customers)
                {
                    if (person.Task is ShopFastFood)
                    {
                        numArray[((ShopFastFood)person.Task).Register]++;
                    }
                }
                if (numArray[0] < numArray[1])
                {
                    return 0;
                }
            }
            return 1;
        }

        public void StressOutWorkers(int register)
        {
        }

        public void TakeItem(int register)
        {
            this.foodstacks[register, A.State.Random.Next(8)]--;
        }
    }
}