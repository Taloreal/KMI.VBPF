namespace KMI.Sim {

    using ICSharpCode.SharpZipLib.GZip;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    public class Simulator
    {
        public bool Academic = false;
        public bool AllowIntraTeamMessaging = false;
        public bool AllowRoleBasedMultiplayer = false;
        public int BackgroundMusicLength = 0x180c4;
        protected bool client = false;
        private const int COMPRESSION_BUFFER_SIZE = 0x800;
        protected static bool compressSaves;
        public float CurrencyConversion = 1f;
        public string DataFileTypeExtension = "vxx";
        public string DataFileTypeName = Application.ProductName;
        public SimSettings DefaultSimSettings = new SimSettings();
        public bool Demo = false;
        public int DemoDuration = 120;
        protected Hashtable dontShowAgain = new Hashtable();
        public string EntityName = "Business";
        public static ArrayList FiringOrder;
        public DayOfWeek FirstDayOfWeek = DayOfWeek.Sunday;
        protected Guid guid = Guid.NewGuid();
        protected Hashtable imageTable;
        protected static Simulator instance;
        public bool Messages = true;
        protected string multiplayerRoleName;
        public bool NewStandardProjectFromFile = true;
        public string NewWhatName = "City";
        public Hashtable PeriodicMessageTable = new Hashtable();
        protected KMI.Sim.Resource resource;
        public ArrayList SafeViewsForNoEntity = new ArrayList();
        protected string sessionName;
        public SimEngine simEngine;
        protected KMI.Sim.SimFactory simFactory;
        protected KMI.Sim.SimState simState;
        protected KMI.Sim.SimStateAdapter simStateAdapter;
        protected string thisPlayerName = "";
        protected KMI.Sim.UserAdminSettings userAdminSettings;
        public bool VBC = false;
        protected KMI.Sim.View[] views;

        static Simulator()
        {
            TimePeriod[] c = new TimePeriod[5];
            c[0] = TimePeriod.Year;
            c[1] = TimePeriod.Week;
            c[2] = TimePeriod.Day;
            c[3] = TimePeriod.Hour;
            FiringOrder = new ArrayList(c);
        }

        public Simulator(KMI.Sim.SimFactory simFactory)
        {
            this.simFactory = simFactory;
        }

        public bool BlockMessage(string message)
        {
            if (message != null)
            {
                if ((S.Settings.BlockMessagesContaining == null) || (S.Settings.BlockMessagesContaining == ""))
                {
                    return false;
                }
                string[] strArray = S.Settings.BlockMessagesContaining.Split(new char[] { '|' });
                foreach (string str in strArray)
                {
                    if (message.IndexOf(str) > -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void DontShowAgain(string messageTitle)
        {
            if (!this.dontShowAgain.ContainsKey(messageTitle))
            {
                this.dontShowAgain.Add(messageTitle, messageTitle);
            }
        }

        public bool DoShowAgain(string messageTitle)
        {
            return !this.dontShowAgain.ContainsKey(messageTitle);
        }

        public static void InitSimulator(KMI.Sim.SimFactory simFactory)
        {
            instance = simFactory.CreateSimulator();
            Instance.resource = Instance.simFactory.CreateResource();
            Instance.simEngine = Instance.simFactory.CreateSimEngine();
            Instance.userAdminSettings = Instance.simFactory.CreateUserAdminSettings();
            Instance.resource.ImageTable = Instance.simFactory.CreateImageTable();
            Instance.resource.PageTable = Instance.simFactory.CreatePageTable();
            Instance.resource.CursorTable = Instance.simFactory.CreateCursorTable();
            Instance.views = Instance.simFactory.CreateViews();
            S.Instance.SafeViewsForNoEntity.Add(Instance.Views[0].Name);
            S.Instance.DefaultSimSettings = Instance.SimFactory.CreateSimSettings();
            compressSaves = true;
        }

        public void LoadState(string filename)
        {
            lock (S.Adapter)
            {
                Stream serializationStream = null;
                CryptoStream baseInputStream = null;
                try
                {
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                    byte[] bytes = Encoding.ASCII.GetBytes("A8C3q97w");
                    byte[] rgbIV = Encoding.ASCII.GetBytes("pK8J6Gfe");
                    ICryptoTransform transform = provider.CreateDecryptor(bytes, rgbIV);
                    IFormatter formatter = new BinaryFormatter();
                    serializationStream = new GZipInputStream(File.OpenRead(filename));
                    try
                    {
                        this.simState = (KMI.Sim.SimState)formatter.Deserialize(serializationStream);
                    }
                    catch (GZipException)
                    {
                        serializationStream.Close();
                        baseInputStream = new CryptoStream(File.OpenRead(filename), transform, CryptoStreamMode.Read);
                        serializationStream = new GZipInputStream(baseInputStream);
                        this.simState = (KMI.Sim.SimState)formatter.Deserialize(serializationStream);
                    }
                }
                finally
                {

                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                    if (baseInputStream != null)
                    {
                        baseInputStream.Close();
                    }
                }
            }
        }

        public void NewState(SimSettings simSettings, bool multiplayer)
        {
            SimSettings settings = (SimSettings)Utilities.DeepCopyBySerialization(simSettings);
            this.SimState = this.simFactory.CreateSimState(settings, multiplayer);
            this.SimState.Init();
        }

        public void SaveState(string filename)
        {
            lock (S.Adapter)
            {
                Stream serializationStream = null;
                CryptoStream stream2 = null;
                try
                {
                    Stream stream3;
                    IFormatter formatter = new BinaryFormatter();
                    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                    byte[] bytes = Encoding.ASCII.GetBytes("A8C3q97w");
                    byte[] rgbIV = Encoding.ASCII.GetBytes("pK8J6Gfe");
                    ICryptoTransform transform = provider.CreateEncryptor(bytes, rgbIV);
                    if (S.Settings.StudentOrg == 0)
                    {
                        stream3 = File.Create(filename);
                    }
                    else
                    {
                        stream3 = new CryptoStream(File.Create(filename), transform, CryptoStreamMode.Write);
                    }
                    serializationStream = new GZipOutputStream(stream3);
                    formatter.Serialize(serializationStream, this.simState);
                }
                finally
                {
                    if (stream2 != null)
                    {
                        stream2.Close();
                    }
                    else if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
            }
        }

        public void SetSimEngineSpeed(SimSpeed speed)
        {
            this.simEngine.StepPeriod = speed.StepPeriod;
            this.simEngine.Skip = speed.SkipFactor;
        }

        public void StartSimTimeRunning()
        {
            this.simEngine.ResumeThread();
        }

        public void StopSimTimeRunning()
        {
            this.simEngine.PauseThread();
        }

        public void Subscribe(ActiveObject activeObject)
        {
            this.simState.SubscribeTimedEvent(activeObject);
        }

        public void Subscribe(ActiveObject activeObject, TimePeriod timePeriod)
        {
            this.simState.SubscribeTimedEvent(activeObject, timePeriod);
        }

        public void Subscribe(ActiveObject activeObject, DateTime wakeupTime)
        {
            activeObject.WakeupTime = wakeupTime;
            if (!this.simState.eventQueue.ContainsValue(activeObject))
            {
                this.simState.eventQueue.Add(activeObject, activeObject);
            }
        }

        public void UnSubscribe(ActiveObject activeObject)
        {
            this.simState.UnSubscribeTimedEvent(activeObject);
        }

        public void UnSubscribe(ActiveObject activeObject, TimePeriod timePeriod)
        {
            this.simState.UnSubscribeTimedEvent(activeObject, timePeriod);
        }

        public KMI.Sim.View View(string name)
        {
            foreach (KMI.Sim.View view in this.Views)
            {
                if (view.Name == name)
                {
                    return view;
                }
            }
            throw new Exception("View " + name + " not found.");
        }

        public bool Client
        {
            get
            {
                return this.client;
            }
            set
            {
                this.client = value;
            }
        }

        public static bool CompressSaves
        {
            get
            {
                return compressSaves;
            }
            set
            {
                compressSaves = value;
            }
        }

        public Guid GUID
        {
            get
            {
                return this.guid;
            }
        }

        public bool Host
        {
            get
            {
                return (this.Multiplayer && !this.Client);
            }
        }

        public static Simulator Instance
        {
            get
            {
                return instance;
            }
        }

        public bool Multiplayer
        {
            get
            {
                return (S.Instance.Client || S.State.Multiplayer);
            }
        }

        public KMI.Sim.MultiplayerRole MultiplayerRole
        {
            get
            {
                if (this.Client && (this.MultiplayerRoleName != ""))
                {
                    foreach (KMI.Sim.MultiplayerRole role in KMI.Sim.MultiplayerRole.Roles)
                    {
                        if (role.RoleName == this.MultiplayerRoleName)
                        {
                            return role;
                        }
                    }
                }
                return null;
            }
        }

        public string MultiplayerRoleName
        {
            get
            {
                return this.multiplayerRoleName;
            }
            set
            {
                this.multiplayerRoleName = value;
            }
        }

        public KMI.Sim.Resource Resource
        {
            get
            {
                return this.resource;
            }
        }

        public string SessionName
        {
            get
            {
                return this.sessionName;
            }
            set
            {
                this.sessionName = value;
            }
        }

        public KMI.Sim.SimFactory SimFactory
        {
            get
            {
                return this.simFactory;
            }
        }

        public KMI.Sim.SimState SimState
        {
            get
            {
                if (this.Client)
                {
                    throw new Exception("SimState accessed from client");
                }
                return this.simState;
            }
            set
            {
                this.simState = value;
            }
        }

        public KMI.Sim.SimStateAdapter SimStateAdapter
        {
            get
            {
                return this.simStateAdapter;
            }
            set
            {
                this.simStateAdapter = value;
            }
        }

        public bool SimTimeRunning
        {
            get
            {
                return this.simEngine.Running;
            }
        }

        public string ThisPlayerName
        {
            get
            {
                return this.thisPlayerName;
            }
            set
            {
                this.thisPlayerName = value;
            }
        }

        public KMI.Sim.UserAdminSettings UserAdminSettings
        {
            get
            {
                return this.userAdminSettings;
            }
        }

        public KMI.Sim.View[] Views
        {
            get
            {
                return this.views;
            }
        }

        public enum TimePeriod
        {
            Step,
            Hour,
            Day,
            Week,
            Year
        }
    }
}