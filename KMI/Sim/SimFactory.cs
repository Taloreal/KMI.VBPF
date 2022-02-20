namespace KMI.Sim
{
    using KMI.Sim.Surveys;
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Drawing;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    public class SimFactory
    {
        protected Bitmap CBmp(System.Type typeFromAssembly, string filename)
        {
            Bitmap image = null;
            Bitmap bitmap2 = new Bitmap(typeFromAssembly, filename);
            bitmap2.SetResolution(96f, 96f);
            if (bitmap2 == null)
            {
                throw new Exception("In SimFactory.CreateCompatibleBitmap, could not get image from filename " + filename);
            }
            image = new Bitmap(bitmap2.Width, bitmap2.Height, S.MainForm.picMain.CreateGraphics());
            Graphics.FromImage(image).DrawImageUnscaled(bitmap2, 0, 0);
            return image;
        }

        protected Cursor CCursor(System.Type typeFromAssembly, string filename)
        {
            return new Cursor(this.CBmp(typeFromAssembly, filename).GetHicon());
        }

        protected Page CPage(System.Type typeFromAssembly, string filename, int cols, int rows, int anchorX, int anchorY)
        {
            return new Page(this.CBmp(typeFromAssembly, filename), cols, rows, anchorX, anchorY);
        }

        public virtual SortedList CreateCursorTable()
        {
            return null;
        }

        public virtual Entity CreateEntity(Player player, string entityName)
        {
            return new Entity(player, entityName);
        }

        public virtual SortedList CreateImageTable()
        {
            return null;
        }

        public virtual SortedList CreatePageTable()
        {
            return null;
        }

        public virtual Player CreatePlayer(string playerName, PlayerType playerType)
        {
            return new Player(playerName, playerType);
        }

        public virtual Resource CreateResource()
        {
            ResourceManager manager = new ResourceManager("KMI.Sim.Sim", Assembly.GetAssembly(typeof(SimFactory)));
            return new Resource(new ResourceManager[] { manager });
        }

        public virtual SimEngine CreateSimEngine()
        {
            return new SimEngine();
        }

        public virtual SimSettings CreateSimSettings()
        {
            return new SimSettings();
        }

        public virtual SimState CreateSimState(SimSettings simSettings, bool multiplayer)
        {
            return new SimState(simSettings, multiplayer);
        }

        public virtual SimStateAdapter CreateSimStateAdapter()
        {
            return new SimStateAdapter();
        }

        public virtual Simulator CreateSimulator()
        {
            return new Simulator(this);
        }

        public virtual Survey CreateSurvey(long entityID, DateTime date, string[] entityNames, ArrayList surveyQuestions)
        {
            return new Survey(entityID, date, entityNames, surveyQuestions);
        }

        public virtual UserAdminSettings CreateUserAdminSettings()
        {
            UserAdminSettings settings = new UserAdminSettings();
            AppSettingsReader reader = new AppSettingsReader();
            settings.DefaultDirectory = global::Properties.Settings.Default.DefaultDirectory;
            settings.P = global::Properties.Settings.Default.P;
            settings.ProxyAddress = global::Properties.Settings.Default.ProxyAddress;
            settings.ProxyBypassList = global::Properties.Settings.Default.ProxyBypassList;
            settings.NoSound = global::Properties.Settings.Default.NoSound;
            settings.MultiplayerBasePort = global::Properties.Settings.Default.MultiplayerBasePort;
            settings.MultiplayerPortCount = global::Properties.Settings.Default.MultiplayerPortCount;
            settings.ClientDrawStepPeriod = global::Properties.Settings.Default.ClientDrawStepPeriod;
            settings.PasswordsForMultiplayer = global::Properties.Settings.Default.PasswordsForMultiplayer;
            return settings;
        }

        public virtual KMI.Sim.View[] CreateViews()
        {
            return new KMI.Sim.View[0];
        }

        protected void LoadWith8CompassPoints(SortedList table, System.Type type, string baseResourceName, string fileExtension)
        {
            this.LoadWithCompassPoints(table, type, baseResourceName, fileExtension);
            char[] separator = new char[] { '.' };
            string[] strArray = baseResourceName.Split(separator);
            string str = strArray[strArray.Length - 1];
            string[] textArray1 = new string[] { "N", "S", "E", "W" };
            foreach (string str2 in textArray1)
            {
                table.Add(str + str2, this.CBmp(type, baseResourceName + str2 + "." + fileExtension));
            }
        }

        protected void LoadWithCompassPoints(SortedList table, System.Type type, string baseResourceName, string fileExtension)
        {
            char[] separator = new char[] { '.' };
            string[] strArray = baseResourceName.Split(separator);
            string str = strArray[strArray.Length - 1];
            string[] textArray1 = new string[] { "N", "S" };
            foreach (string str2 in textArray1)
            {
                string[] textArray2 = new string[] { "E", "W" };
                foreach (string str3 in textArray2)
                {
                    string[] textArray3 = new string[] { baseResourceName, str2, str3, ".", fileExtension };
                    table.Add(str + str2 + str3, this.CBmp(type, string.Concat(textArray3)));
                }
            }
        }
    }
}

