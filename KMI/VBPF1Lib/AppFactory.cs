namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Reflection;
    using System.Resources;

    public class TSortedList : SortedList {

        private static int Called = 0;
        private static List<string> Dumped = new List<string>();
        private static string Allowed = "abcdefghijklmnopqrstuvwxyz0123456789 _-";

        public override void Add(object key, object value) {
            if (key is string && value is Bitmap) {
                Dump((string)key, (Bitmap)value);
            }
            base.Add(key, value);
            Called += 1;
        }

        private void Dump(string key, Bitmap val) {
            key = Normalize(key.ToLower());
            if (Dumped.Contains(key)) { return; }
            Dumped.Add(key);
            Directory.CreateDirectory("C:\\Program Data\\VBPF_Dump\\");
            val.Save("C:\\Program Data\\VBPF_Dump\\" + key + ".bmp");
        }

        private string Normalize(string val) {
            string temp = "";
            while (val.Length > 0) {
                if (Allowed.Contains(val.Substring(0, 1))) {
                    temp += val[0];
                }
                val = val.Substring(1);
            }
            return temp;
        }
    }

    public class AppFactory : SimFactory
    {
        public override Entity CreateEntity(Player player, string entityName)
        {
            return new AppEntity(player, entityName);
        }

        public override SortedList CreateImageTable()
        {
            int num;
            string str;
            Bitmap bitmap;
            Bitmap bitmap2;
            SortedList table = new SortedList();
            Type typeFromAssembly = typeof(frmMain);
            table.Add("HRLogo", base.CBmp(typeFromAssembly, "Images.HRLogo.png"));
            table.Add("StatusBusTokens", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.BusTokens.png"));
            table.Add("StatusGas", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Gas.png"));
            table.Add("StatusFood", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Food.png"));
            table.Add("StatusHealth", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Health.png"));
            table.Add("PointerHome", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Home.png"));
            table.Add("PointerHomeGray", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.HomeGray.png"));
            table.Add("PointerSchool", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.School.png"));
            table.Add("PointerSchoolGray", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.SchoolGray.png"));
            table.Add("PointerWork", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Work.png"));
            table.Add("PointerWorkGray", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.WorkGray.png"));
            table.Add("PointerPerson", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Person.png"));
            table.Add("PointerPersonGray", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.PersonGray.png"));
            table.Add("PointerCondo", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Condo.png"));
            table.Add("PointerCondoGray", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.CondoGray.png"));
            table.Add("City", base.CBmp(typeFromAssembly, "Images.CityView.City.png"));
            table.Add("CarNESW", base.CBmp(typeFromAssembly, "Images.CityView.CarNESW.gif"));
            table.Add("CarNWSE", base.CBmp(typeFromAssembly, "Images.CityView.CarNWSE.gif"));
            for (num = 0; num < AppConstants.BuildingTypeCount; num++)
            {
                Bitmap bitmap3 = base.CBmp(typeFromAssembly, "Images.CityView.Building" + num + ".png");
                table.Add("Building" + num, bitmap3);
            }
            table.Add("Arrow", base.CBmp(typeFromAssembly, "Images.Arrow.png"));
            base.LoadWithCompassPoints(table, typeFromAssembly, "Images.CityView.Bus", "png");
            table.Add("Walker", base.CBmp(typeFromAssembly, "Images.CityView.Walker.png"));
            for (num = 0; num < 3; num++)
            {
                table.Add("GasCan" + num, base.CBmp(typeFromAssembly, "Images.CityView.GasCan" + num + ".png"));
            }
            table.Add("MotorOil", base.CBmp(typeFromAssembly, "Images.CityView.MotorOil.png"));
            table.Add("CarOnLift", base.CBmp(typeFromAssembly, "Images.CityView.CarOnLift.png"));
            table.Add("CityNavIcon", base.CBmp(typeFromAssembly, "Images.CityNavIcon.png"));
            for (num = 0; num < 6; num++)
            {
                table.Add("Car" + num, base.CBmp(typeFromAssembly, "Images.CityView.Car" + num + ".png"));
                table.Add("StatusCar" + num + "OK", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Car" + num + "OK.png"));
                table.Add("StatusCar" + num + "Broken", base.CBmp(typeFromAssembly, "Images.StatusIconsAndPointers.Car" + num + "Broken.png"));
            }
            table.Add("HomeBack", base.CBmp(typeFromAssembly, "Images.HomeView.ApartmentFloor2.png"));
            for (num = 0; num < 3; num++)
            {
                table.Add("Bed" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Bed" + num + ".png"));
            }
            for (num = 0; num < 4; num++)
            {
                table.Add("Couch" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Couch" + num + ".png"));
                table.Add("CoffeeTable" + num, base.CBmp(typeFromAssembly, "Images.HomeView.CoffeeTable" + num + ".png"));
                table.Add("Chair" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Chair" + num + ".png"));
                table.Add("ChairBack" + num, base.CBmp(typeFromAssembly, "Images.HomeView.ChairBack" + num + ".png"));
            }
            for (num = 0; num < 5; num++)
            {
                table.Add("Carpet" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Carpet" + num + ".png"));
            }
            table.Add("ApartmentKitchenInteriorWall", base.CBmp(typeFromAssembly, "Images.HomeView.ApartmentKitchenInteriorWall.png"));
            table.Add("InteriorWall1", base.CBmp(typeFromAssembly, "Images.HomeView.InteriorWall1.png"));
            table.Add("InteriorWall2", base.CBmp(typeFromAssembly, "Images.HomeView.InteriorWall2.png"));
            table.Add("Computer", base.CBmp(typeFromAssembly, "Images.HomeView.Computer.png"));
            table.Add("KitchenBar3", base.CBmp(typeFromAssembly, "Images.HomeView.KitchenBar3.png"));
            table.Add("KitchenBar", base.CBmp(typeFromAssembly, "Images.HomeView.KitchenBar.png"));
            table.Add("Lamp2", base.CBmp(typeFromAssembly, "Images.HomeView.Lamp2.png"));
            table.Add("Lamp", base.CBmp(typeFromAssembly, "Images.HomeView.Lamp.png"));
            for (num = 0; num < 2; num++)
            {
                table.Add("TVBack" + num, base.CBmp(typeFromAssembly, "Images.HomeView.TVBack" + num + ".png"));
                table.Add("TVFront" + num, base.CBmp(typeFromAssembly, "Images.HomeView.TVFront" + num + ".png"));
            }
            table.Add("WallCabinet", base.CBmp(typeFromAssembly, "Images.HomeView.WallCabinet.png"));
            table.Add("FloorCabinet", base.CBmp(typeFromAssembly, "Images.HomeView.FloorCabinet.png"));
            table.Add("TreadMill", base.CBmp(typeFromAssembly, "Images.HomeView.TreadMill.png"));
            table.Add("Chair", base.CBmp(typeFromAssembly, "Images.HomeView.Chair.png"));
            table.Add("Decor", base.CBmp(typeFromAssembly, "Images.HomeView.Decor.png"));
            table.Add("EndTable2", base.CBmp(typeFromAssembly, "Images.HomeView.EndTable2.png"));
            table.Add("BuiltInDesk", base.CBmp(typeFromAssembly, "Images.HomeView.BuiltInDesk.png"));
            table.Add("Oven", base.CBmp(typeFromAssembly, "Images.HomeView.Oven.png"));
            table.Add("Plant", base.CBmp(typeFromAssembly, "Images.HomeView.Plant.png"));
            table.Add("WallSconce", base.CBmp(typeFromAssembly, "Images.HomeView.WallSconce.png"));
            table.Add("Refrigerator", base.CBmp(typeFromAssembly, "Images.HomeView.Refrigerator.png"));
            table.Add("Paper", base.CBmp(typeFromAssembly, "Images.HomeView.Paper.png"));
            table.Add("Money", base.CBmp(typeFromAssembly, "Images.HomeView.Money.png"));
            table.Add("Check", base.CBmp(typeFromAssembly, "Images.HomeView.Check.png"));
            for (num = 0; num < 3; num++)
            {
                table.Add("BagOfFood" + num, base.CBmp(typeFromAssembly, "Images.HomeView.BagOfFood" + num + ".png"));
            }
            for (num = 0; num < 5; num++)
            {
                table.Add("Platter" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Platter" + num + ".png"));
                table.Add("Platter" + num + "Small", base.CBmp(typeFromAssembly, "Images.HomeView.Platter" + num + "Small.png"));
            }
            table.Add("PlateOfFood", base.CBmp(typeFromAssembly, "Images.HomeView.PlateOfFood.png"));
            table.Add("IceBag", base.CBmp(typeFromAssembly, "Images.HomeView.IceBag.png"));
            table.Add("EndTable", base.CBmp(typeFromAssembly, "Images.HomeView.EndTableSE.png"));
            table.Add("BusToken", base.CBmp(typeFromAssembly, "Images.HomeView.BusToken.png"));
            table.Add("Diploma", base.CBmp(typeFromAssembly, "Images.HomeView.Diploma.png"));
            for (num = 0; num < 3; num++)
            {
                table.Add("BusTokens" + num, base.CBmp(typeFromAssembly, "Images.HomeView.BusTokens" + num + ".png"));
            }
            for (num = 0; num < 5; num++)
            {
                table.Add("DDR" + num, base.CBmp(typeFromAssembly, "Images.HomeView.DDR" + num + ".png"));
            }
            for (num = 0; num < 7; num++)
            {
                table.Add("Art" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Art" + num + ".png"));
            }
            num = 0;
            while (num < 3)
            {
                table.Add("Stereo" + num, base.CBmp(typeFromAssembly, "Images.HomeView.Stereo" + num + ".png"));
                num++;
            }
            table.Add("WorkBack", base.CBmp(typeFromAssembly, "Images.WorkView0.FastFoodFloor.png"));
            table.Add("BagOfFood", base.CBmp(typeFromAssembly, "Images.WorkView0.BagOfFood.png"));
            table.Add("CounterFastFood", base.CBmp(typeFromAssembly, "Images.WorkView0.CounterFastFood.png"));
            table.Add("FoodContainer1", base.CBmp(typeFromAssembly, "Images.WorkView0.FoodContainer1.png"));
            table.Add("FoodContainer0", base.CBmp(typeFromAssembly, "Images.WorkView0.FoodContainer0.png"));
            table.Add("FoodWall", base.CBmp(typeFromAssembly, "Images.WorkView0.FoodWall.png"));
            table.Add("FoodWallTop", base.CBmp(typeFromAssembly, "Images.WorkView0.FoodWallTop.png"));
            table.Add("SodaMachine", base.CBmp(typeFromAssembly, "Images.WorkView0.SodaMachine.png"));
            table.Add("TreeFastFood", base.CBmp(typeFromAssembly, "Images.WorkView0.TreeFastFood.png"));
            table.Add("RightGlass", base.CBmp(typeFromAssembly, "Images.WorkView0.RightGlass.png"));
            table.Add("LeftGlass", base.CBmp(typeFromAssembly, "Images.WorkView0.LeftGlass.png"));
            table.Add("Bar", base.CBmp(typeFromAssembly, "Images.WorkView0.Bar.png"));
            table.Add("PlantsFrontLeft", base.CBmp(typeFromAssembly, "Images.WorkView0.PlantsFrontLeft.png"));
            table.Add("PlantsFrontRight", base.CBmp(typeFromAssembly, "Images.WorkView0.PlantsFrontRight.png"));
            table.Add("OfficeBack", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeBack.png"));
            table.Add("SWWall", base.CBmp(typeFromAssembly, "Images.WorkView1.SWWall.png"));
            table.Add("SEWall", base.CBmp(typeFromAssembly, "Images.WorkView1.SEWall.png"));
            table.Add("OfficeCouch", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeCouch.png"));
            table.Add("OfficeManagerDesk", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeManagerDesk.png"));
            table.Add("OfficeManagerPainting", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeManagerPainting.png"));
            table.Add("OfficeManagerPlant", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeManagerPlant.png"));
            table.Add("OfficePlant", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficePlant.png"));
            table.Add("OfficePrinter", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficePrinter.png"));
            table.Add("OfficeSupervisorBookshelf", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeSupervisorBookshelf.png"));
            table.Add("OfficeSupervisorDesk", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeSupervisorDesk.png"));
            table.Add("OfficeWorkerDesk", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeWorkerDesk.png"));
            table.Add("OfficeWorkerChair", base.CBmp(typeFromAssembly, "Images.WorkView1.OfficeWorkerChair.png"));
            table.Add("ClassBack", base.CBmp(typeFromAssembly, "Images.ClassView.SchoolFloor.png"));
            table.Add("TeacherDesk", base.CBmp(typeFromAssembly, "Images.ClassView.TeacherDesk.png"));
            table.Add("SchoolChairBack", base.CBmp(typeFromAssembly, "Images.ClassView.SchoolChairBack.png"));
            table.Add("SchoolChairBottom", base.CBmp(typeFromAssembly, "Images.ClassView.SchoolChairBottom.png"));
            table.Add("Table", base.CBmp(typeFromAssembly, "Images.ClassView.Table.png"));
            table.Add("1040EZ", base.CBmp(typeFromAssembly, "Images.1040EZ.png"));
            string[] textArray1 = new string[] { "Female", "Male" };
            foreach (string str2 in textArray1)
            {
                num = 0;
                while (num < 0x1c)
                {
                    str = num.ToString().PadLeft(4, '0');
                    string[] textArray2 = new string[] { "Images.ClassView.", str2, "TeacherBoardSW", str, ".png" };
                    table.Add(str2 + "TeacherBoardSW" + str, base.CBmp(typeFromAssembly, string.Concat(textArray2)));
                    num++;
                }
            }
            string[] textArray3 = new string[] { "Female", "Male" };
            foreach (string str3 in textArray3)
            {
                num = 0;
                while (num < 0x13)
                {
                    str = num.ToString().PadLeft(3, '0');
                    string[] textArray4 = new string[] { "Images.People.", str3, "FFTakeOrderSW", str, ".gif" };
                    bitmap = new Bitmap(typeFromAssembly, string.Concat(textArray4));
                    this.PalettizeAndInsert(table, bitmap, str3, "FFTakeOrder", "SW", str);
                    num++;
                }
                string[] textArray5 = new string[] { "NW", "SW" };
                foreach (string str4 in textArray5)
                {
                    num = 0;
                    while (num < 9)
                    {
                        str = num.ToString().PadLeft(3, '0');
                        string[] textArray6 = new string[] { "Images.People.", str3, "FastFoodWalk", str4, str, ".gif" };
                        bitmap = new Bitmap(typeFromAssembly, string.Concat(textArray6));
                        this.PalettizeFlipAndInsert(table, bitmap, str3, "FFWalk", str4, str);
                        num += 2;
                    }
                    string[] textArray7 = new string[] { "Images.People.", str3, "FastFoodStand", str4, ".gif" };
                    bitmap2 = new Bitmap(typeFromAssembly, string.Concat(textArray7));
                    this.PalettizeFlipAndInsert(table, bitmap2, str3, "FFStand", str4, "");
                }
            }
            string[] textArray8 = new string[] { "Female", "Male" };
            foreach (string str5 in textArray8)
            {
                for (num = 0; num < 9; num++)
                {
                    object[] objArray1 = new object[] { "Images.WorkView1.Type.", str5, "SitTypeNW00", num, ".gif" };
                    bitmap = new Bitmap(typeFromAssembly, string.Concat(objArray1));
                    this.PalettizeAndInsert(table, bitmap, str5, "SitType", "NW", "00" + num.ToString());
                }
            }
            string[] textArray9 = new string[] { "Stand", "Sit", "Sleep" };
            foreach (string str6 in textArray9)
            {
                string[] textArray10 = new string[] { "Female", "Male" };
                foreach (string str7 in textArray10)
                {
                    string[] textArray11 = new string[] { "NW", "SW" };
                    foreach (string str8 in textArray11)
                    {
                        string[] textArray12 = new string[] { "Images.People.", str7, str6, str8, ".gif" };
                        bitmap2 = new Bitmap(typeFromAssembly, string.Concat(textArray12));
                        this.PalettizeFlipAndInsert(table, bitmap2, str7, str6, str8, "");
                    }
                }
            }
            num = 0;
            while (num < 18)
            {
                if (num < 6)
                {
                    bitmap2 = new Bitmap(typeFromAssembly, "Images.People.Palette" + num + ".gif");
                }
                else if (num < 12)
                {
                    bitmap2 = new Bitmap(typeFromAssembly, "Images.People.UserPeople.Female" + (num - 6) + ".gif");
                }
                else
                {
                    bitmap2 = new Bitmap(typeFromAssembly, "Images.People.UserPeople.Male" + (num - 12) + ".gif");
                }
                ColorPalette palette = bitmap2.Palette;
                for (int i = 240; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(0, palette.Entries[i]);
                }
                bitmap2.Palette = palette;
                table.Add("Palette" + num, bitmap2);
                num++;
            }
            string[] textArray13 = new string[] { "Walk", "CarryFood", "FFCarryFood" };
            foreach (string str9 in textArray13)
            {
                string[] textArray14 = new string[] { "Female", "Male" };
                foreach (string str10 in textArray14)
                {
                    string[] textArray15 = new string[] { "NW", "SW" };
                    foreach (string str11 in textArray15)
                    {
                        num = 0;
                        while (num < 9)
                        {
                            object[] objArray2 = new object[] { "Images.People.", str10, str9, str11, "00", num, ".gif" };
                            bitmap = new Bitmap(typeFromAssembly, string.Concat(objArray2));
                            this.PalettizeFlipAndInsert(table, bitmap, str10, str9, str11, "00" + num);
                            num += 2;
                        }
                    }
                }
            }
            string[] textArray16 = new string[] { "Female", "Male" };
            foreach (string str12 in textArray16)
            {
                num = 0;
                while (num < 9)
                {
                    object[] objArray3 = new object[] { "Images.People.", str12, "JumpingJacksSW00", num, ".gif" };
                    bitmap = new Bitmap(typeFromAssembly, string.Concat(objArray3));
                    this.PalettizeAndInsert(table, bitmap, str12, "JumpingJacks", "SW", "00" + num);
                    num++;
                }
            }
            string[] textArray17 = new string[] { "Female", "Male" };
            foreach (string str13 in textArray17)
            {
                num = 0;
                while (num < 0x1b)
                {
                    string[] textArray18 = new string[] { "Images.People.", str13, "DanceSW", num.ToString().PadLeft(3, '0'), ".gif" };
                    bitmap = new Bitmap(typeFromAssembly, string.Concat(textArray18));
                    this.PalettizeFlipAndInsert(table, bitmap, str13, "Dance", "SW", num.ToString().PadLeft(3, '0'));
                    num++;
                }
            }
            string[] textArray19 = new string[] { "Female", "Male" };
            foreach (string str14 in textArray19)
            {
                num = 0;
                while (num < 10)
                {
                    object[] objArray4 = new object[] { "Images.People.", str14, "EatSW00", num, ".gif" };
                    bitmap = new Bitmap(typeFromAssembly, string.Concat(objArray4));
                    this.PalettizeFlipAndInsert(table, bitmap, str14, "Eat", "SW", "00" + num);
                    num++;
                }
            }
            AppConstants.CarryAnchorPoints = new Hashtable();
            string[] textArray20 = new string[] { "CarryFood", "FFCarryFood" };
            foreach (string str15 in textArray20)
            {
                string[] textArray21 = new string[] { "Female", "Male" };
                foreach (string str16 in textArray21)
                {
                    string[] textArray22 = new string[] { "NW", "SW", "NE", "SE" };
                    foreach (string str17 in textArray22)
                    {
                        for (num = 0; num < 9; num += 2)
                        {
                            object[] objArray5 = new object[] { str16, str15, str17, "00", num };
                            string key = string.Concat(objArray5);
                            bitmap = (Bitmap) table[key];
                            for (int j = 0; j < bitmap.Width; j++)
                            {
                                for (int k = 0; k < bitmap.Height; k++)
                                {
                                    Color pixel = bitmap.GetPixel(j, k);
                                    if (((pixel.R == 0) && (pixel.G == 0)) && (pixel.B == 0))
                                    {
                                        AppConstants.CarryAnchorPoints.Add(key, new Point(j - (bitmap.Width / 2), (k - bitmap.Height) + 15));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            table.Add("LogoHSN Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoHSN.png"));
            table.Add("LogoOlympic Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoOlympic.png"));
            table.Add("LogoHerald Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoHerald.png"));
            table.Add("CCardHSN Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.CCardHSN.png"));
            table.Add("CCardOlympic Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.CCardOlympic.png"));
            table.Add("CCardHerald Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.CCardHerald.png"));
            table.Add("DCardHSN Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.DCardHSN.png"));
            table.Add("DCardOlympic Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.DCardOlympic.png"));
            table.Add("DCardHerald Bank", base.CBmp(typeFromAssembly, "Images.LogosAndCards.DCardHerald.png"));
            table.Add("LogoTaranti Auto Loan", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoAuto.png"));
            table.Add("LogoTaranti Auto Lease", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoAuto.png"));
            table.Add("LogoNRG Electric", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoElectric.png"));
            table.Add("LogoS&W Insurance", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoInsurance.png"));
            table.Add("LogoInternet Connect", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoInternet.png"));
            table.Add("LogoVincent Medical", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoMedical.png"));
            table.Add("LogoCity Property Mgt", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoProperty.png"));
            table.Add("LogoQuest Student Loans", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoStudentLoan.png"));
            table.Add("LogoFiduciary Investments", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoFiduciary.png"));
            table.Add("LogoCentury Mortgage", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoCentury.png"));
            table.Add("LogoIRS", base.CBmp(typeFromAssembly, "Images.LogosAndCards.LogoIRS.jpg"));
            return table;
        }

        public override Resource CreateResource()
        {
            ResourceManager manager = new ResourceManager("KMI.Sim.Sim", Assembly.GetAssembly(typeof(SimFactory)));
            ResourceManager manager2 = new ResourceManager("KMI.VBPF1Lib.App", Assembly.GetAssembly(typeof(AppFactory)));
            return new Resource(new ResourceManager[] { manager, manager2 });
        }

        public override SimSettings CreateSimSettings()
        {
            return new AppSimSettings();
        }

        public override SimState CreateSimState(SimSettings simSettings, bool multiplayer)
        {
            return new AppSimState(simSettings, multiplayer);
        }

        public override SimStateAdapter CreateSimStateAdapter()
        {
            return new AppStateAdapter();
        }

        public override View[] CreateViews()
        {
            View[] viewArray = new View[2];
            viewArray[0] = new CityView("Outside", S.Resources.GetImage("City"));
            viewArray[0].ClearDrawSelectedOnMouseMove = true;
            viewArray[1] = new InsideView("Home", S.Resources.GetImage("HomeBack"));
            return viewArray;
        }

        public void PalettizeAndInsert(SortedList table, Bitmap b, string gender, string pose, string orient, string frame)
        {
            ColorPalette palette = b.Palette;
            for (int i = 240; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(0, palette.Entries[i]);
            }
            b.Palette = palette;
            table.Add(gender + pose + orient + frame, b);
        }

        public void PalettizeFlipAndInsert(SortedList table, Bitmap b, string gender, string pose, string orient, string frame)
        {
            Bitmap bitmap = (Bitmap) b.Clone();
            ColorPalette palette = b.Palette;
            for (int i = 240; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(0, palette.Entries[i]);
            }
            b.Palette = palette;
            table.Add(gender + pose + orient + frame, b);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
            table.Add(gender + pose + orient.Substring(0, 1) + "E" + frame, bitmap);
        }
    }
}

