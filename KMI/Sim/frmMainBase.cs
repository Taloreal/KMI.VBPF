namespace KMI.Sim
{
    using KMI.Sim.Academics;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using System.Windows.Forms;

    public class frmMainBase : Form
    {
        protected Bitmap backBuffer;
        protected Graphics backBufferGraphics;
        public System.Windows.Forms.Timer backgroundMusicTimer;
        protected SimSettings cacheDesignerSimSettings;
        protected SimSettings cachedSimSettings;
        protected string[] commandLineArgs;
        private IContainer components;
        private StatusBarPanel CreateMessagePanel;
        protected long currentEntityID;
        protected StatusBarPanel CurrentEntityNamePanel;
        protected StatusBarPanel CurrentEntityPanel;
        protected string currentFilePath;
        protected internal int currentMacroActionIndex;
        protected KMI.Sim.View currentView;
        protected string currentViewName;
        protected int currentWeek;
        public static DateTime DateNotSet = new DateTime(0x7d0, 1, 1);
        protected StatusBarPanel DatePanel;
        protected StatusBarPanel DayOfWeekPanel;
        protected bool designerMode;
        protected bool dirtySimState;
        protected StatusBarPanel EntityCriticalResourceNamePanel;
        protected StatusBarPanel EntityCriticalResourcePanel;
        private Hashtable entityNames;
        protected ImageList ilsMainToolBar;
        protected static frmMainBase instance;
        protected bool isWin98;
        protected bool lessonLoadedPromptSaveAs;
        private StatusBarPanel Level;
        protected internal ArrayList macroActions;
        protected internal string macroFilename;
        protected internal bool macroPlayingOn;
        protected internal bool macroRecordingOn;
        protected MainMenu mainMenu1;
        private ResourceManager manager;
        private MenuItem menuItem3;
        protected MenuItem menuMessagesSep;
        protected frmMessages messagesForm;
        protected MenuItem mnuActions;
        private MenuItem mnuFile;
        public MenuItem mnuFileExit;
        protected MenuItem mnuFileMultiplayer;
        public MenuItem mnuFileMultiplayerJoin;
        protected MenuItem mnuFileMultiplayerScoreboard;
        public MenuItem mnuFileMultiplayerStart;
        private MenuItem mnuFileMultiplayerTeamList;
        public MenuItem mnuFileNew;
        public MenuItem mnuFileOpenLesson;
        public MenuItem mnuFileOpenSavedSim;
        private MenuItem mnuFilePrintView;
        private MenuItem mnuFileSave;
        public MenuItem mnuFileSaveAs;
        private MenuItem mnuFileUploadSeparator;
        private MenuItem mnuHelp;
        private MenuItem mnuHelpAbout;
        private MenuItem mnuHelpAssignment;
        private MenuItem mnuHelpSearch;
        private MenuItem mnuHelpTopicsAndIndex;
        public MenuItem mnuHelpTutorial;
        protected MenuItem mnuOptions;
        public MenuItem mnuOptionsBackgroundMusic;
        private MenuItem mnuOptionsChangeOwner;
        private MenuItem mnuOptionsFaster;
        protected internal MenuItem mnuOptionsGoStop;
        protected MenuItem mnuOptionsIA;
        private MenuItem mnuOptionsIACustomizeYourSim;
        private MenuItem mnuOptionsIAProvideCash;
        private MenuItem mnuOptionsMacros;
        private MenuItem mnuOptionsMacrosPlayMacro;
        private MenuItem mnuOptionsMacrosRecordMacro;
        private MenuItem mnuOptionsMacrosStopPlaying;
        private MenuItem mnuOptionsMacroStopRecording;
        private MenuItem mnuOptionsRenameEntity;
        private MenuItem mnuOptionsRunTo;
        protected MenuItem mnuOptionsShowMessages;
        private MenuItem mnuOptionsSlower;
        private MenuItem mnuOptionsSoundEffects;
        private MenuItem mnuOptionsTestResults;
        private MenuItem mnuOptionsTuning;
        protected MenuItem mnuReports;
        private MenuItem mnuSep1;
        private MenuItem mnuSep3;
        private MenuItem mnuSep4;
        private MenuItem mnuSep6;
        private MenuItem mnuSep7;
        private MenuItem mnuSep8;
        private MenuItem mnuSep9385;
        protected MenuItem mnuView;
        protected StatusBarPanel NewMessagesPanel;
        protected internal DateTime nextMacroPlayTime;
        protected DateTime now;
        public PictureBox picMain;
        protected internal long playIntervalMilliseconds;
        protected internal bool playLooping;
        private Panel pnlMain;
        protected string printStudentName;
        protected ToolBarButton ScoreboardButton;
        protected StatusBarPanel SpacerPanel;
        protected internal StatusBar staMain;
        protected StatusBarPanel TimePanel;
        protected ToolbarSponsored tlbMain;
        private ToolTip viewToolTip;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler EntityChanged;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler NewDay;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler NewHour;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler NewWeek;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler NewYear;

        public frmMainBase()
        {
            this.entityNames = new Hashtable();
            this.currentEntityID = -1L;
            this.printStudentName = "";
            this.macroRecordingOn = false;
            this.macroPlayingOn = false;
            this.playLooping = true;
            this.playIntervalMilliseconds = 0x3e8L;
            this.currentMacroActionIndex = 0;
            this.macroActions = new ArrayList();
            this.InitializeComponent();
        }

        public frmMainBase(string[] args, bool demo, bool vbc, bool academic)
        {
            this.entityNames = new Hashtable();
            this.currentEntityID = -1L;
            this.printStudentName = "";
            this.macroRecordingOn = false;
            this.macroPlayingOn = false;
            this.playLooping = true;
            this.playIntervalMilliseconds = 0x3e8L;
            this.currentMacroActionIndex = 0;
            this.macroActions = new ArrayList();
            this.InitializeComponent();
            this.isWin98 = Environment.OSVersion.Platform != PlatformID.Win32NT;
            instance = this;
            this.InitRemotingConfiguration();
            if ((base.CreateGraphics().DpiX != 96f) || !(base.CreateGraphics().DpiY == 96f))
            {
                MessageBox.Show("A DPI setting other than 96 was detected. This program is designed for 96 DPI and may not function properly. To correct this problem, change the DPI setting under Display on the Control Panel.", "Warning");
            }
            Cursor.Current = Cursors.WaitCursor;
            this.commandLineArgs = args;
            this.ConstructSimulator();
            S.Instance.VBC = vbc;
            S.Instance.Demo = demo;
            S.Instance.Academic = academic;
            if (S.Instance.VBC)
            {
                int vBCStudentOrgCode = this.GetVBCStudentOrgCode();
                if (vBCStudentOrgCode > 0)
                {
                    string str = Utilities.GetWebPage(WebRequest.Create("http://vbc.knowledgematters.com/vbccommon/vbcvalidate.php?ss=" + vBCStudentOrgCode), S.Instance.UserAdminSettings.ProxyAddress, S.Instance.UserAdminSettings.ProxyBypassList);
                    if (str != "1")
                    {
                        if (str == "")
                        {
                            MessageBox.Show("This special version of " + Application.ProductName + " could not connect to the Internet to confirm that a Virtual Business Challenge is currently running. Please check your Internet connection and try again.", "No Internet Connection");
                            Application.Exit();
                        }
                        else
                        {
                            MessageBox.Show("There is no live challenge for " + Application.ProductName + " at this time.", "No Valid Virtual Business Challenge");
                            Application.Exit();
                        }
                    }
                }
            }
            this.DesignerMode = false;
            foreach (KMI.Sim.View view in S.Instance.Views)
            {
                MenuItem item = new MenuItem(S.Resources.GetString(view.Name), new EventHandler(this.mnuViewView_Click));
                this.mnuView.MenuItems.Add(item);
            }
            this.mnuView.MenuItems.Add(new MenuItem("-"));
            this.CurrentEntityNamePanel.Text = S.Resources.GetString("Current") + " " + S.Instance.EntityName + ":";
            this.mnuOptionsSoundEffects.Checked = !S.Instance.UserAdminSettings.NoSound;
            this.mnuOptionsBackgroundMusic.Checked = !S.Instance.UserAdminSettings.NoSound;
            this.mnuOptionsSoundEffects.Enabled = !S.Instance.UserAdminSettings.NoSound;
            this.mnuOptionsBackgroundMusic.Enabled = !S.Instance.UserAdminSettings.NoSound;
            this.mnuFileNew.Enabled = !S.Instance.VBC;
            this.mnuFileOpenLesson.Enabled = !S.Instance.VBC;
            this.mnuFileMultiplayerStart.Enabled = !S.Instance.VBC;
            this.mnuFileMultiplayerJoin.Enabled = !S.Instance.VBC;
            this.mnuFileOpenLesson.Visible = !S.Instance.Academic;
            this.mnuFileMultiplayer.Visible = !S.Instance.Academic;
            this.mnuOptionsTestResults.Visible = S.Instance.Academic;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
        }

        public void AbortSession()
        {
            this.dirtySimState = false;
            this.mnuFileExit_Click(null, null);
        }

        public void AddPlayerMessage(PlayerMessage message)
        {
            if (this.messagesForm != null)
            {
                this.messagesForm.AddMessage(message);
                if (!this.messagesForm.Visible && (this.NewMessagesPanel.Width != 120))
                {
                    this.NewMessagesPanel.Width = 120;
                }
            }
        }

        private void backgroundMusicTimer_Tick(object sender, EventArgs e)
        {
            this.StartMusic();
        }

        protected bool CanShowForm(Form f)
        {
            if (f is IConstrainedForm)
            {
                string text = ((IConstrainedForm) f).CanUse();
                if (text.Equals(""))
                {
                    return true;
                }
                MessageBox.Show(text, S.Resources.GetString("Action Not Allowed"));
                return false;
            }
            return true;
        }

        protected virtual void ClientJoinHook(Player p)
        {
        }

        private string ClientOrHost()
        {
            if (Simulator.Instance.Client)
            {
                return "Client";
            }
            return "Host";
        }

        protected void CloseActionForms()
        {
            foreach (Form form in base.OwnedForms)
            {
                if (form is IActionForm)
                {
                    form.Close();
                }
            }
        }

        protected void CloseOwnedForms()
        {
            foreach (Form form in base.OwnedForms)
            {
                form.Close();
            }
        }

        protected virtual void ConstructSimulator()
        {
            Simulator.InitSimulator(new SimFactory());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public virtual void EnableDisable()
        {
            MenuItem item;
            ToolBarButton button;
            this.cachedSimSettings = S.Instance.SimStateAdapter.getSimSettings();
            bool flag = false;
            if (this.CurrentEntityID != -1L)
            {
                flag = S.Instance.ThisPlayerName == S.Instance.SimStateAdapter.GetEntityPlayer(this.CurrentEntityID);
            }
            this.mnuHelpAssignment.Enabled = this.cachedSimSettings.PdfAssignment != null;
            Utilities.FindButtonEquivalent(this.tlbMain, this.mnuHelpAssignment.Text).Enabled = this.mnuHelpAssignment.Enabled;
            bool flag2 = this.CurrentEntityID == -1L;
            bool flag3 = S.Instance.Multiplayer && !S.Instance.Client;
            PropertyInfo[] properties = this.cachedSimSettings.GetType().GetProperties();
            foreach (PropertyInfo info in properties)
            {
                string str;
                if (flag)
                {
                    str = "Owner";
                }
                else
                {
                    str = "NonOwner";
                }
                int index = info.Name.IndexOf("EnabledFor" + str);
                if (index > -1)
                {
                    string s = info.Name.Substring(0, index);
                    item = Utilities.FindMenuEquivalent(this.mainMenu1, Utilities.AddSpaces(s));
                    if (item != null)
                    {
                        bool flag8 = (bool) info.GetGetMethod().Invoke(this.cachedSimSettings, new object[0]);
                        if (flag2)
                        {
                            flag8 = false;
                        }
                        else if (this.DesignerMode)
                        {
                            flag8 = true;
                        }
                        else if (flag3)
                        {
                            flag8 = this.IsReportMenuItem(item);
                        }
                        item.Enabled = flag8;
                        button = Utilities.FindButtonEquivalent(this.tlbMain, item.Text);
                        if (button != null)
                        {
                            button.Enabled = item.Enabled;
                        }
                    }
                }
            }
            if (S.Instance.MultiplayerRole != null)
            {
                string[] disableList = S.Instance.MultiplayerRole.DisableList;
                foreach (string str3 in disableList)
                {
                    item = Utilities.FindMenuEquivalent(this.mainMenu1, str3);
                    if (item != null)
                    {
                        item.Enabled = false;
                        button = Utilities.FindButtonEquivalent(this.tlbMain, item.Text);
                        if (button != null)
                        {
                            button.Enabled = item.Enabled;
                        }
                    }
                }
            }
            foreach (MenuItem item2 in this.mainMenu1.MenuItems)
            {
                foreach (MenuItem item3 in item2.MenuItems)
                {
                    if (item3.MenuItems.Count > 0)
                    {
                        item3.Enabled = false;
                        foreach (MenuItem item4 in item3.MenuItems)
                        {
                            if (item4.Text != "-")
                            {
                                item3.Enabled |= item4.Enabled;
                            }
                        }
                    }
                }
            }
            if (S.Instance.Client)
            {
                this.mnuOptionsIA.Enabled = false;
            }
            if ((this.cachedSimSettings.StudentOrg > 0) && !S.MainForm.DesignerMode)
            {
                this.mnuOptionsIA.Enabled = false;
            }
        }

        public void EnableMenuAndSubMenus(MenuItem m)
        {
            m.Enabled = true;
            foreach (MenuItem item in m.MenuItems)
            {
                this.EnableMenuAndSubMenus(item);
            }
        }

        public string EntityIDToName(long ID)
        {
            if (this.entityNames.ContainsKey(ID))
            {
                return (string) this.entityNames[ID];
            }
            return "";
        }

        public long EntityNameToID(string entityName)
        {
            foreach (long num in this.entityNames.Keys)
            {
                if (((string) this.entityNames[num]).ToUpper() == entityName.ToUpper())
                {
                    return num;
                }
            }
            return -1L;
        }

        public void ExplainNoFunctionality()
        {
            MessageBox.Show(this, "You will be receiving a technical update shortly that implements this functionality");
        }

        protected Form FindOwnedForm(System.Type type)
        {
            foreach (Form form in base.OwnedForms)
            {
                if (form.GetType() == type)
                {
                    return form;
                }
            }
            return null;
        }

        private void FireNewTimeEvents(SimStateAdapter.ViewUpdate viewUpdate)
        {
            bool flag = (this.Now != DateNotSet) && (viewUpdate.Now.Year != this.Now.Year);
            bool flag2 = (this.CurrentWeek != -1) && (viewUpdate.CurrentWeek != this.CurrentWeek);
            bool flag3 = (this.Now != DateNotSet) && (((viewUpdate.Now.Day != this.Now.Day) || (viewUpdate.Now.Month != this.Now.Month)) | flag);
            if (((this.Now != DateNotSet) && ((viewUpdate.Now.Hour != this.Now.Hour) | flag3)) && (this.NewHour != null))
            {
                this.NewHour(this, new EventArgs());
            }
            if (flag3 && (this.NewDay != null))
            {
                this.NewDay(this, new EventArgs());
            }
            if (flag2 && (this.NewWeek != null))
            {
                this.NewWeek(this, new EventArgs());
                if (this.SoundOn)
                {
                    Sound.PlaySoundFromFile(@"sounds\NewWeek.wav");
                }
            }
            if (flag && (this.NewYear != null))
            {
                this.NewYear(this, new EventArgs());
            }
        }

        private void frmMainBase_Closed(object sender, EventArgs e)
        {
            this.UnregisterClientEventHandlers();
            Application.Exit();
        }

        private void frmMainBase_Closing(object sender, CancelEventArgs e)
        {
            if (!this.QuerySave())
            {
                e.Cancel = true;
            }
        }

        private void frmMainBase_Load(object sender, EventArgs e)
        {
            this.Text = Application.ProductName;
            if (!base.DesignMode)
            {
                if (S.Instance.Academic)
                {
                    Simulator i = S.Instance;
                    i.DataFileTypeExtension = i.DataFileTypeExtension + "A";
                }
                if ((this.commandLineArgs == null) || (this.commandLineArgs.Length == 0))
                {
                    this.GetCorrectStartChoices().ShowDialog(this);
                }
                else
                {
                    this.OpenSavedSim(this.commandLineArgs[0]);
                }
            }
        }

        private void frmMainBase_Resize(object sender, EventArgs e)
        {
            int num = 0;
            if (this.picMain.Size.Height > this.pnlMain.Size.Height)
            {
                num = 0x11;
            }
            this.pnlMain.AutoScrollPosition = new Point(0, 0);
            this.picMain.Location = new Point(Math.Max(0, ((this.pnlMain.Size.Width - num) - this.picMain.Size.Width) / 2), Math.Max(0, (this.pnlMain.Size.Height - this.picMain.Size.Height) / 2));
        }

        public Form GetCorrectStartChoices()
        {
            return new frmStartChoices();
        }

        public bool GetEnablingFor(string menuCaption)
        {
            return Utilities.FindMenuEquivalent(this.mainMenu1, menuCaption).Enabled;
        }

        public virtual int GetVBCStudentOrgCode()
        {
            return 0;
        }

        public void HideMessageWindow()
        {
            this.mnuOptionsShowMessages.Checked = false;
            this.messagesForm.Hide();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuFileNew = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenLesson = new System.Windows.Forms.MenuItem();
            this.mnuFileOpenSavedSim = new System.Windows.Forms.MenuItem();
            this.mnuSep1 = new System.Windows.Forms.MenuItem();
            this.mnuFileSave = new System.Windows.Forms.MenuItem();
            this.mnuFileSaveAs = new System.Windows.Forms.MenuItem();
            this.mnuFileUploadSeparator = new System.Windows.Forms.MenuItem();
            this.mnuFileMultiplayer = new System.Windows.Forms.MenuItem();
            this.mnuFileMultiplayerJoin = new System.Windows.Forms.MenuItem();
            this.mnuFileMultiplayerStart = new System.Windows.Forms.MenuItem();
            this.mnuFileMultiplayerScoreboard = new System.Windows.Forms.MenuItem();
            this.mnuFileMultiplayerTeamList = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuFilePrintView = new System.Windows.Forms.MenuItem();
            this.mnuSep3 = new System.Windows.Forms.MenuItem();
            this.mnuFileExit = new System.Windows.Forms.MenuItem();
            this.mnuView = new System.Windows.Forms.MenuItem();
            this.mnuReports = new System.Windows.Forms.MenuItem();
            this.mnuActions = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuOptionsGoStop = new System.Windows.Forms.MenuItem();
            this.mnuOptionsFaster = new System.Windows.Forms.MenuItem();
            this.mnuOptionsSlower = new System.Windows.Forms.MenuItem();
            this.mnuSep9385 = new System.Windows.Forms.MenuItem();
            this.mnuOptionsRunTo = new System.Windows.Forms.MenuItem();
            this.menuMessagesSep = new System.Windows.Forms.MenuItem();
            this.mnuOptionsShowMessages = new System.Windows.Forms.MenuItem();
            this.mnuSep4 = new System.Windows.Forms.MenuItem();
            this.mnuOptionsBackgroundMusic = new System.Windows.Forms.MenuItem();
            this.mnuOptionsSoundEffects = new System.Windows.Forms.MenuItem();
            this.mnuSep6 = new System.Windows.Forms.MenuItem();
            this.mnuOptionsIA = new System.Windows.Forms.MenuItem();
            this.mnuOptionsIACustomizeYourSim = new System.Windows.Forms.MenuItem();
            this.mnuOptionsIAProvideCash = new System.Windows.Forms.MenuItem();
            this.mnuOptionsTuning = new System.Windows.Forms.MenuItem();
            this.mnuOptionsChangeOwner = new System.Windows.Forms.MenuItem();
            this.mnuOptionsRenameEntity = new System.Windows.Forms.MenuItem();
            this.mnuOptionsMacros = new System.Windows.Forms.MenuItem();
            this.mnuOptionsMacrosRecordMacro = new System.Windows.Forms.MenuItem();
            this.mnuOptionsMacroStopRecording = new System.Windows.Forms.MenuItem();
            this.mnuOptionsMacrosPlayMacro = new System.Windows.Forms.MenuItem();
            this.mnuOptionsMacrosStopPlaying = new System.Windows.Forms.MenuItem();
            this.mnuOptionsTestResults = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuHelpTopicsAndIndex = new System.Windows.Forms.MenuItem();
            this.mnuHelpTutorial = new System.Windows.Forms.MenuItem();
            this.mnuHelpSearch = new System.Windows.Forms.MenuItem();
            this.mnuSep7 = new System.Windows.Forms.MenuItem();
            this.mnuHelpAssignment = new System.Windows.Forms.MenuItem();
            this.mnuSep8 = new System.Windows.Forms.MenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.MenuItem();
            this.staMain = new System.Windows.Forms.StatusBar();
            this.DatePanel = new System.Windows.Forms.StatusBarPanel();
            this.DayOfWeekPanel = new System.Windows.Forms.StatusBarPanel();
            this.TimePanel = new System.Windows.Forms.StatusBarPanel();
            this.NewMessagesPanel = new System.Windows.Forms.StatusBarPanel();
            this.CreateMessagePanel = new System.Windows.Forms.StatusBarPanel();
            this.Level = new System.Windows.Forms.StatusBarPanel();
            this.SpacerPanel = new System.Windows.Forms.StatusBarPanel();
            this.CurrentEntityNamePanel = new System.Windows.Forms.StatusBarPanel();
            this.CurrentEntityPanel = new System.Windows.Forms.StatusBarPanel();
            this.EntityCriticalResourceNamePanel = new System.Windows.Forms.StatusBarPanel();
            this.EntityCriticalResourcePanel = new System.Windows.Forms.StatusBarPanel();
            this.tlbMain = new KMI.Sim.ToolbarSponsored();
            this.ilsMainToolBar = new System.Windows.Forms.ImageList(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.viewToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundMusicTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.DatePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DayOfWeekPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewMessagesPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CreateMessagePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpacerPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourceNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourcePanel)).BeginInit();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuReports,
            this.mnuActions,
            this.mnuOptions,
            this.mnuHelp});
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileNew,
            this.mnuFileOpenLesson,
            this.mnuFileOpenSavedSim,
            this.mnuSep1,
            this.mnuFileSave,
            this.mnuFileSaveAs,
            this.mnuFileUploadSeparator,
            this.mnuFileMultiplayer,
            this.menuItem3,
            this.mnuFilePrintView,
            this.mnuSep3,
            this.mnuFileExit});
            this.mnuFile.Text = "&File";
            // 
            // mnuFileNew
            // 
            this.mnuFileNew.Index = 0;
            this.mnuFileNew.Text = "&New...";
            this.mnuFileNew.Click += new System.EventHandler(this.mnuFileNew_Click);
            // 
            // mnuFileOpenLesson
            // 
            this.mnuFileOpenLesson.Index = 1;
            this.mnuFileOpenLesson.Text = "&Open Lesson...";
            this.mnuFileOpenLesson.Click += new System.EventHandler(this.mnuFileOpenLesson_Click);
            // 
            // mnuFileOpenSavedSim
            // 
            this.mnuFileOpenSavedSim.Index = 2;
            this.mnuFileOpenSavedSim.Text = "Open S&aved Sim...";
            this.mnuFileOpenSavedSim.Click += new System.EventHandler(this.mnuFileOpenSavedSim_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Index = 3;
            this.mnuSep1.Text = "-";
            // 
            // mnuFileSave
            // 
            this.mnuFileSave.Index = 4;
            this.mnuFileSave.Text = "Sa&ve";
            this.mnuFileSave.Click += new System.EventHandler(this.mnuFileSave_Click);
            // 
            // mnuFileSaveAs
            // 
            this.mnuFileSaveAs.Index = 5;
            this.mnuFileSaveAs.Text = "Sav&e As...";
            this.mnuFileSaveAs.Click += new System.EventHandler(this.mnuFileSaveAs_Click);
            // 
            // mnuFileUploadSeparator
            // 
            this.mnuFileUploadSeparator.Index = 6;
            this.mnuFileUploadSeparator.Text = "-";
            this.mnuFileUploadSeparator.Visible = false;
            // 
            // mnuFileMultiplayer
            // 
            this.mnuFileMultiplayer.Index = 7;
            this.mnuFileMultiplayer.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFileMultiplayerJoin,
            this.mnuFileMultiplayerStart,
            this.mnuFileMultiplayerScoreboard,
            this.mnuFileMultiplayerTeamList});
            this.mnuFileMultiplayer.Text = "&Multiplayer";
            // 
            // mnuFileMultiplayerJoin
            // 
            this.mnuFileMultiplayerJoin.Index = 0;
            this.mnuFileMultiplayerJoin.Text = "&Join Session";
            this.mnuFileMultiplayerJoin.Click += new System.EventHandler(this.mnuFileMultiplayerJoin_Click);
            // 
            // mnuFileMultiplayerStart
            // 
            this.mnuFileMultiplayerStart.Index = 1;
            this.mnuFileMultiplayerStart.Text = "&Start Session";
            this.mnuFileMultiplayerStart.Click += new System.EventHandler(this.mnuFileMultiplayerStart_Click);
            // 
            // mnuFileMultiplayerScoreboard
            // 
            this.mnuFileMultiplayerScoreboard.Index = 2;
            this.mnuFileMultiplayerScoreboard.Text = "S&coreboard";
            this.mnuFileMultiplayerScoreboard.Click += new System.EventHandler(this.mnuFileMultiplayerScoreboard_Click);
            // 
            // mnuFileMultiplayerTeamList
            // 
            this.mnuFileMultiplayerTeamList.Index = 3;
            this.mnuFileMultiplayerTeamList.Text = "&Team List";
            this.mnuFileMultiplayerTeamList.Click += new System.EventHandler(this.mnuFileMultiplayerTeamList_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 8;
            this.menuItem3.Text = "-";
            // 
            // mnuFilePrintView
            // 
            this.mnuFilePrintView.Index = 9;
            this.mnuFilePrintView.Text = "&Print View...";
            this.mnuFilePrintView.Click += new System.EventHandler(this.mnuFilePrintView_Click);
            // 
            // mnuSep3
            // 
            this.mnuSep3.Index = 10;
            this.mnuSep3.Text = "-";
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Index = 11;
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
            // 
            // mnuView
            // 
            this.mnuView.Index = 1;
            this.mnuView.Text = "&View";
            // 
            // mnuReports
            // 
            this.mnuReports.Index = 2;
            this.mnuReports.Text = "&Reports";
            // 
            // mnuActions
            // 
            this.mnuActions.Index = 3;
            this.mnuActions.Text = "&Actions";
            this.mnuActions.Select += new System.EventHandler(this.mnuActions_Select);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 4;
            this.mnuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptionsGoStop,
            this.mnuOptionsFaster,
            this.mnuOptionsSlower,
            this.mnuSep9385,
            this.mnuOptionsRunTo,
            this.menuMessagesSep,
            this.mnuOptionsShowMessages,
            this.mnuSep4,
            this.mnuOptionsBackgroundMusic,
            this.mnuOptionsSoundEffects,
            this.mnuSep6,
            this.mnuOptionsIA,
            this.mnuOptionsTuning,
            this.mnuOptionsChangeOwner,
            this.mnuOptionsRenameEntity,
            this.mnuOptionsMacros,
            this.mnuOptionsTestResults});
            this.mnuOptions.Text = "&Options";
            // 
            // mnuOptionsGoStop
            // 
            this.mnuOptionsGoStop.Index = 0;
            this.mnuOptionsGoStop.Text = "&Go";
            this.mnuOptionsGoStop.Click += new System.EventHandler(this.mnuOptionsGoStop_Click);
            // 
            // mnuOptionsFaster
            // 
            this.mnuOptionsFaster.Index = 1;
            this.mnuOptionsFaster.Text = "&Faster";
            this.mnuOptionsFaster.Click += new System.EventHandler(this.mnuOptionsFaster_Click);
            // 
            // mnuOptionsSlower
            // 
            this.mnuOptionsSlower.Index = 2;
            this.mnuOptionsSlower.Text = "&Slower";
            this.mnuOptionsSlower.Click += new System.EventHandler(this.mnuOptionsSlower_Click);
            // 
            // mnuSep9385
            // 
            this.mnuSep9385.Index = 3;
            this.mnuSep9385.Text = "-";
            // 
            // mnuOptionsRunTo
            // 
            this.mnuOptionsRunTo.Index = 4;
            this.mnuOptionsRunTo.Text = "Run to...";
            this.mnuOptionsRunTo.Click += new System.EventHandler(this.mnuOptionsRunTo_Click);
            // 
            // menuMessagesSep
            // 
            this.menuMessagesSep.Index = 5;
            this.menuMessagesSep.Text = "-";
            // 
            // mnuOptionsShowMessages
            // 
            this.mnuOptionsShowMessages.Checked = true;
            this.mnuOptionsShowMessages.Index = 6;
            this.mnuOptionsShowMessages.Text = "Show Messages";
            this.mnuOptionsShowMessages.Click += new System.EventHandler(this.mnuOptionsShowMessages_Click);
            // 
            // mnuSep4
            // 
            this.mnuSep4.Index = 7;
            this.mnuSep4.Text = "-";
            // 
            // mnuOptionsBackgroundMusic
            // 
            this.mnuOptionsBackgroundMusic.Index = 8;
            this.mnuOptionsBackgroundMusic.Text = "&Background Music";
            this.mnuOptionsBackgroundMusic.Click += new System.EventHandler(this.mnuOptionsBackgroundMusic_Click);
            // 
            // mnuOptionsSoundEffects
            // 
            this.mnuOptionsSoundEffects.Checked = true;
            this.mnuOptionsSoundEffects.Index = 9;
            this.mnuOptionsSoundEffects.Text = "S&ound Effects";
            this.mnuOptionsSoundEffects.Click += new System.EventHandler(this.mnuOptionsSoundEffects_Click);
            // 
            // mnuSep6
            // 
            this.mnuSep6.Index = 10;
            this.mnuSep6.Text = "-";
            // 
            // mnuOptionsIA
            // 
            this.mnuOptionsIA.Index = 11;
            this.mnuOptionsIA.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptionsIACustomizeYourSim,
            this.mnuOptionsIAProvideCash});
            this.mnuOptionsIA.Text = "&Instructor\'s Area";
            // 
            // mnuOptionsIACustomizeYourSim
            // 
            this.mnuOptionsIACustomizeYourSim.Index = 0;
            this.mnuOptionsIACustomizeYourSim.Text = "Enable/Disable &Features...";
            this.mnuOptionsIACustomizeYourSim.Click += new System.EventHandler(this.mnuOptionsIACustomizeYourSim_Click);
            // 
            // mnuOptionsIAProvideCash
            // 
            this.mnuOptionsIAProvideCash.Index = 1;
            this.mnuOptionsIAProvideCash.Text = "&Provide Cash...";
            this.mnuOptionsIAProvideCash.Click += new System.EventHandler(this.mnuOptionsIAProvideCash_Click);
            // 
            // mnuOptionsTuning
            // 
            this.mnuOptionsTuning.Index = 12;
            this.mnuOptionsTuning.Text = "Tuning";
            this.mnuOptionsTuning.Click += new System.EventHandler(this.mnuOptionsTuning_Click);
            // 
            // mnuOptionsChangeOwner
            // 
            this.mnuOptionsChangeOwner.Index = 13;
            this.mnuOptionsChangeOwner.Text = "Change Owner";
            this.mnuOptionsChangeOwner.Click += new System.EventHandler(this.mnuOptionsChangeOwner_Click);
            // 
            // mnuOptionsRenameEntity
            // 
            this.mnuOptionsRenameEntity.Index = 14;
            this.mnuOptionsRenameEntity.Text = "Rename Entity";
            this.mnuOptionsRenameEntity.Click += new System.EventHandler(this.mnuOptionsRenameEntity_Click);
            // 
            // mnuOptionsMacros
            // 
            this.mnuOptionsMacros.Index = 15;
            this.mnuOptionsMacros.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptionsMacrosRecordMacro,
            this.mnuOptionsMacroStopRecording,
            this.mnuOptionsMacrosPlayMacro,
            this.mnuOptionsMacrosStopPlaying});
            this.mnuOptionsMacros.Text = "&Macros";
            // 
            // mnuOptionsMacrosRecordMacro
            // 
            this.mnuOptionsMacrosRecordMacro.Index = 0;
            this.mnuOptionsMacrosRecordMacro.Text = "&Record Macro";
            this.mnuOptionsMacrosRecordMacro.Click += new System.EventHandler(this.mnuOptionsMacrosRecordMacro_Click);
            // 
            // mnuOptionsMacroStopRecording
            // 
            this.mnuOptionsMacroStopRecording.Enabled = false;
            this.mnuOptionsMacroStopRecording.Index = 1;
            this.mnuOptionsMacroStopRecording.Text = "&Stop Recording";
            this.mnuOptionsMacroStopRecording.Click += new System.EventHandler(this.mnuOptionsMacroStopRecording_Click);
            // 
            // mnuOptionsMacrosPlayMacro
            // 
            this.mnuOptionsMacrosPlayMacro.Index = 2;
            this.mnuOptionsMacrosPlayMacro.Text = "&Play Macro";
            this.mnuOptionsMacrosPlayMacro.Click += new System.EventHandler(this.mnuOptionsMacrosPlayMacro_Click);
            // 
            // mnuOptionsMacrosStopPlaying
            // 
            this.mnuOptionsMacrosStopPlaying.Enabled = false;
            this.mnuOptionsMacrosStopPlaying.Index = 3;
            this.mnuOptionsMacrosStopPlaying.Text = "S&top Playing";
            this.mnuOptionsMacrosStopPlaying.Click += new System.EventHandler(this.mnuOptionsMacrosStopPlaying_Click);
            // 
            // mnuOptionsTestResults
            // 
            this.mnuOptionsTestResults.Index = 16;
            this.mnuOptionsTestResults.Text = "Test Results...";
            this.mnuOptionsTestResults.Visible = false;
            this.mnuOptionsTestResults.Click += new System.EventHandler(this.mnuOptionsTestResults_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 5;
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuHelpTopicsAndIndex,
            this.mnuHelpTutorial,
            this.mnuHelpSearch,
            this.mnuSep7,
            this.mnuHelpAssignment,
            this.mnuSep8,
            this.mnuHelpAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpTopicsAndIndex
            // 
            this.mnuHelpTopicsAndIndex.Index = 0;
            this.mnuHelpTopicsAndIndex.Text = "&Topics && Index...";
            this.mnuHelpTopicsAndIndex.Click += new System.EventHandler(this.mnuHelpTopicsAndIndex_Click);
            // 
            // mnuHelpTutorial
            // 
            this.mnuHelpTutorial.Index = 1;
            this.mnuHelpTutorial.Text = "T&utorial...";
            this.mnuHelpTutorial.Click += new System.EventHandler(this.mnuHelpTutorial_Click);
            // 
            // mnuHelpSearch
            // 
            this.mnuHelpSearch.Index = 2;
            this.mnuHelpSearch.Text = "Sear&ch...";
            this.mnuHelpSearch.Click += new System.EventHandler(this.mnuHelpSearch_Click);
            // 
            // mnuSep7
            // 
            this.mnuSep7.Index = 3;
            this.mnuSep7.Text = "-";
            // 
            // mnuHelpAssignment
            // 
            this.mnuHelpAssignment.Index = 4;
            this.mnuHelpAssignment.Text = "A&ssignment...";
            this.mnuHelpAssignment.Click += new System.EventHandler(this.mnuHelpAssignment_Click);
            // 
            // mnuSep8
            // 
            this.mnuSep8.Index = 5;
            this.mnuSep8.Text = "-";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Index = 6;
            this.mnuHelpAbout.Text = "&About...";
            this.mnuHelpAbout.Click += new System.EventHandler(this.mnuHelpAbout_Click);
            // 
            // staMain
            // 
            this.staMain.Location = new System.Drawing.Point(0, 317);
            this.staMain.Name = "staMain";
            this.staMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.DatePanel,
            this.DayOfWeekPanel,
            this.TimePanel,
            this.NewMessagesPanel,
            this.CreateMessagePanel,
            this.Level,
            this.SpacerPanel,
            this.CurrentEntityNamePanel,
            this.CurrentEntityPanel,
            this.EntityCriticalResourceNamePanel,
            this.EntityCriticalResourcePanel});
            this.staMain.ShowPanels = true;
            this.staMain.Size = new System.Drawing.Size(632, 17);
            this.staMain.SizingGrip = false;
            this.staMain.TabIndex = 2;
            this.staMain.PanelClick += new System.Windows.Forms.StatusBarPanelClickEventHandler(this.staMain_PanelClick);
            // 
            // DatePanel
            // 
            this.DatePanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.DatePanel.Name = "DatePanel";
            // 
            // DayOfWeekPanel
            // 
            this.DayOfWeekPanel.Name = "DayOfWeekPanel";
            this.DayOfWeekPanel.Width = 40;
            // 
            // TimePanel
            // 
            this.TimePanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.TimePanel.Name = "TimePanel";
            this.TimePanel.Width = 60;
            // 
            // NewMessagesPanel
            // 
            this.NewMessagesPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.NewMessagesPanel.MinWidth = 0;
            this.NewMessagesPanel.Name = "NewMessagesPanel";
            this.NewMessagesPanel.Text = "New Messages";
            this.NewMessagesPanel.Width = 0;
            // 
            // CreateMessagePanel
            // 
            this.CreateMessagePanel.MinWidth = 0;
            this.CreateMessagePanel.Name = "CreateMessagePanel";
            this.CreateMessagePanel.ToolTipText = "Send Memo to Team";
            this.CreateMessagePanel.Width = 0;
            // 
            // Level
            // 
            this.Level.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.Level.Name = "Level";
            this.Level.Width = 70;
            // 
            // SpacerPanel
            // 
            this.SpacerPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.SpacerPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.SpacerPanel.Name = "SpacerPanel";
            this.SpacerPanel.Width = 94;
            // 
            // CurrentEntityNamePanel
            // 
            this.CurrentEntityNamePanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.CurrentEntityNamePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.CurrentEntityNamePanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.CurrentEntityNamePanel.Name = "CurrentEntityNamePanel";
            this.CurrentEntityNamePanel.Text = "#";
            this.CurrentEntityNamePanel.Width = 22;
            // 
            // CurrentEntityPanel
            // 
            this.CurrentEntityPanel.Name = "CurrentEntityPanel";
            // 
            // EntityCriticalResourceNamePanel
            // 
            this.EntityCriticalResourceNamePanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.EntityCriticalResourceNamePanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.EntityCriticalResourceNamePanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.EntityCriticalResourceNamePanel.Name = "EntityCriticalResourceNamePanel";
            this.EntityCriticalResourceNamePanel.Text = "Cash";
            this.EntityCriticalResourceNamePanel.Width = 46;
            // 
            // EntityCriticalResourcePanel
            // 
            this.EntityCriticalResourcePanel.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.EntityCriticalResourcePanel.Name = "EntityCriticalResourcePanel";
            // 
            // tlbMain
            // 
            this.tlbMain.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tlbMain.DropDownArrows = true;
            this.tlbMain.ImageList = this.ilsMainToolBar;
            this.tlbMain.Location = new System.Drawing.Point(0, 0);
            this.tlbMain.Name = "tlbMain";
            this.tlbMain.ShowToolTips = true;
            this.tlbMain.Size = new System.Drawing.Size(632, 42);
            this.tlbMain.TabIndex = 0;
            this.tlbMain.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tlbMain_ButtonClick);
            // 
            // ilsMainToolBar
            // 
            this.ilsMainToolBar.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.ilsMainToolBar.ImageSize = new System.Drawing.Size(24, 24);
            this.ilsMainToolBar.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.picMain);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 42);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(632, 275);
            this.pnlMain.TabIndex = 1;
            // 
            // picMain
            // 
            this.picMain.BackColor = System.Drawing.Color.White;
            this.picMain.Location = new System.Drawing.Point(86, 46);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(375, 212);
            this.picMain.TabIndex = 0;
            this.picMain.TabStop = false;
            this.picMain.Click += new System.EventHandler(this.picMain_Click);
            this.picMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picMain_Paint);
            this.picMain.DoubleClick += new System.EventHandler(this.picMain_DoubleClick);
            this.picMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseDown);
            this.picMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseMove);
            this.picMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseUp);
            // 
            // viewToolTip
            // 
            this.viewToolTip.AutomaticDelay = 200;
            // 
            // backgroundMusicTimer
            // 
            this.backgroundMusicTimer.Tick += new System.EventHandler(this.backgroundMusicTimer_Tick);
            // 
            // frmMainBase
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(632, 334);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.tlbMain);
            this.Controls.Add(this.staMain);
            this.Menu = this.mainMenu1;
            this.Name = "frmMainBase";
            this.Text = "#";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmMainBase_Closing);
            this.Closed += new System.EventHandler(this.frmMainBase_Closed);
            this.Load += new System.EventHandler(this.frmMainBase_Load);
            this.Resize += new System.EventHandler(this.frmMainBase_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.DatePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DayOfWeekPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewMessagesPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CreateMessagePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpacerPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityNamePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourceNamePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourcePanel)).EndInit();
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected void InitRemotingConfiguration()
        {
            AssemblyName name = Assembly.GetEntryAssembly().GetName();
            int startIndex = 1 + name.CodeBase.LastIndexOf("/");
            RemotingConfiguration.Configure(Application.StartupPath + @"\" + name.CodeBase.Substring(startIndex) + ".config");
        }

        public virtual bool IsActionMenuItem(string menuItemText)
        {
            MenuItem item = Utilities.FindMenuEquivalent(this.mainMenu1, menuItemText);
            if (item != null)
            {
                MenuItem parent = (MenuItem) item.Parent;
                while (parent.Parent != this.mainMenu1)
                {
                    parent = (MenuItem) parent.Parent;
                }
                return (parent == this.mnuActions);
            }
            return false;
        }

        public bool IsMenuItemEnabled(string menuItemText)
        {
            MenuItem item = Utilities.FindMenuEquivalent(this.mainMenu1, menuItemText);
            if (item == null)
            {
                throw new Exception("Can't find menu item " + menuItemText + " in IsMenuItemEnabled.");
            }
            return item.Enabled;
        }

        protected virtual bool IsReportMenuItem(MenuItem menuItem)
        {
            MenuItem parent = (MenuItem) menuItem.Parent;
            return ((parent == this.mnuReports) || (parent == this.mnuView));
        }

        protected virtual void LoadStateHook()
        {
        }

        private void mnuActions_Select(object sender, EventArgs e)
        {
            this.dirtySimState = true;
            this.CloseActionForms();
        }

        private void mnuFileExit_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            base.Close();
        }

        protected virtual void mnuFileMultiplayerJoin_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            if (this.QuerySave())
            {
                frmJoinMultiplayerSession session = new frmJoinMultiplayerSession();
                if (session.ShowDialog() != DialogResult.Cancel)
                {
                    this.ReInit();
                    S.Instance.Client = true;
                    S.Instance.SimStateAdapter = session.RemoteAdapter;
                    S.Instance.ThisPlayerName = session.TeamName;
                    S.Instance.SessionName = session.SessionName;
                    S.Instance.MultiplayerRoleName = session.MultiplayerRoleName;
                    if ((S.Instance.MultiplayerRoleName != "") && S.Instance.AllowIntraTeamMessaging)
                    {
                        this.CreateMessagePanel.Width = 60;
                    }
                    this.ClientJoinHook(session.Player);
                    this.OnStateChanged();
                    this.mnuOptionsGoStop.PerformClick();
                }
            }
        }

        private void mnuFileMultiplayerScoreboard_Click(object sender, EventArgs e)
        {
            Form form = this.FindOwnedForm(typeof(frmScoreboard));
            if (form == null)
            {
                frmScoreboard scoreboard1 = new frmScoreboard {
                    Owner = this
                };
                form = scoreboard1;
                form.Show();
            }
            else
            {
                form.Focus();
            }
        }

        protected virtual void mnuFileMultiplayerStart_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            if (this.QuerySave())
            {
                this.ReInit();
                this.currentFilePath = null;
                if (!this.DesignerMode || (this.cacheDesignerSimSettings == null))
                {
                    S.Instance.NewState(S.Instance.DefaultSimSettings, true);
                }
                else
                {
                    S.Instance.NewState(this.cacheDesignerSimSettings, true);
                }
                this.ServerStartHook();
                this.OnStateChanged();
                this.mnuOptionsGoStop.PerformClick();
            }
        }

        private void mnuFileMultiplayerTeamList_Click(object sender, EventArgs e)
        {
            string text = "";
            foreach (Player player in S.State.Player.Values)
            {
                if ((player.PlayerName != S.Instance.ThisPlayerName) && (player.PlayerType != PlayerType.AI))
                {
                    text = text + S.Resources.GetString("Team Name: ") + player.PlayerName;
                    if (S.Instance.UserAdminSettings.PasswordsForMultiplayer)
                    {
                        text = text + "     " + S.Resources.GetString("Password: ") + S.State.GetMultiplayerTeamPassword(player.PlayerName);
                    }
                    text = text + "\r\n";
                }
            }
            MessageBox.Show(text, S.Resources.GetString("Teams That Have Logged In To This Session"));
        }

        private void mnuFileNew_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            if (this.QuerySave())
            {
                if (S.Instance.Academic)
                {
                    new frmChooseMathPack().ShowDialog();
                }
                DialogResult result = new frmDualChoiceDialog("What kind of " + S.Instance.NewWhatName.ToLower() + " would you like?", "Standard " + S.Instance.NewWhatName, "Random " + S.Instance.NewWhatName, true) { Text = "New " + S.Instance.EntityName + " Project" }.ShowDialog(this);
                if (result != DialogResult.Cancel)
                {
                    SimSettings defaultSimSettings = S.Instance.DefaultSimSettings;
                    if (result == DialogResult.Yes)
                    {
                        if (S.Instance.NewStandardProjectFromFile)
                        {
                            string[] textArray1 = new string[] { Application.StartupPath, @"\Project\New ", S.Instance.EntityName, " Project.", S.Instance.DataFileTypeExtension };
                            string filepath = string.Concat(textArray1);
                            if (S.Instance.Academic)
                            {
                                object[] objArray1 = new object[] { AcademicGod.PageBankPath, Path.DirectorySeparatorChar, "Project.", S.Instance.DataFileTypeExtension };
                                filepath = string.Concat(objArray1);
                            }
                            this.OpenSavedSim(filepath);
                            this.lessonLoadedPromptSaveAs = true;
                            return;
                        }
                        defaultSimSettings.RandomSeed = 0;
                    }
                    this.ReInit();
                    this.currentFilePath = null;
                    if (!this.DesignerMode || (this.cacheDesignerSimSettings == null))
                    {
                        S.Instance.NewState(defaultSimSettings, false);
                    }
                    else
                    {
                        S.Instance.NewState(this.cacheDesignerSimSettings, false);
                    }
                    this.OnStateChanged();
                    this.mnuOptionsGoStop.PerformClick();
                }
            }
        }

        private void mnuFileOpenLesson_Click(object sender, EventArgs e)
        {
            if (S.Instance.Demo)
            {
                MessageBox.Show(this, S.Resources.GetString("Only a limited number of lessons are available in this demo edition."), S.Resources.GetString("Demo Edition"));
            }
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            if (this.QuerySave())
            {
                frmLessonsSimple simple;
                try
                {
                    simple = new frmLessonsSimple();
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("The Lessons directory is missing from the directory in which " + Application.ProductName + " was installed. Re-install " + Application.ProductName + ".");
                    return;
                }
                if (simple.ShowDialog() == DialogResult.OK)
                {
                    this.OpenSavedSim(simple.LessonFileName);
                    this.lessonLoadedPromptSaveAs = true;
                }
            }
        }

        private void mnuFileOpenSavedSim_Click(object sender, EventArgs e)
        {
            if (S.Instance.Demo)
            {
                MessageBox.Show(this, S.Resources.GetString("This feature is disabled in this demo edition."), S.Resources.GetString("Demo Edition"));
            }
            else
            {
                if (S.Instance.SimTimeRunning)
                {
                    this.mnuOptionsGoStop_Click(new object(), new EventArgs());
                }
                if (this.QuerySave())
                {
                    string str = Environment.CurrentDirectory + @"\";
                    if (Directory.Exists(Environment.CurrentDirectory + @"\saves\"))
                    {
                        str = str + @"saves\";
                    }
                    OpenFileDialog dialog2 = new OpenFileDialog {
                        InitialDirectory = str
                    };
                    string[] textArray1 = new string[] { S.Instance.DataFileTypeName, " files (*.", S.Instance.DataFileTypeExtension, ")|*.", S.Instance.DataFileTypeExtension, "|All files (*.*)|*.*" };
                    dialog2.Filter = string.Concat(textArray1);
                    dialog2.DefaultExt = S.Instance.DataFileTypeExtension;
                    OpenFileDialog dialog = dialog2;
                    if (S.Instance.UserAdminSettings.DefaultDirectory != null)
                    {
                        dialog.InitialDirectory = S.Instance.UserAdminSettings.DefaultDirectory;
                    }
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        this.OpenSavedSim(dialog.FileName);
                    }
                }
            }
        }

        private void mnuFilePrintView_Click(object sender, EventArgs e)
        {
            frmInputString str = new frmInputString(S.Resources.GetString("Student Name"), S.Resources.GetString("Enter your name to help identify your printout on a shared printer:"), this.printStudentName);
            str.ShowDialog(this);
            this.printStudentName = str.Response;
            Utilities.PrintWithExceptionHandling(this.Text, new PrintPageEventHandler(this.View_PrintPage));
        }

        private void mnuFileSave_Click(object sender, EventArgs e)
        {
            if (S.Instance.Demo)
            {
                MessageBox.Show(this, S.Resources.GetString("This feature is disabled in this demo edition."), S.Resources.GetString("Demo Edition"));
            }
            else if ((this.currentFilePath == null) | this.lessonLoadedPromptSaveAs)
            {
                this.mnuFileSaveAs_Click(new object(), new EventArgs());
            }
            else
            {
                try
                {
                    this.SaveSim(this.currentFilePath);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(S.Resources.GetString("Could not save file. File may be read-only or in use by another application.") + "\r\n\r\n" + S.Resources.GetString("Error details") + ": " + exception.Message, S.Resources.GetString("Could Not Save"));
                    this.mnuFileSaveAs_Click(new object(), new EventArgs());
                }
            }
        }

        private void mnuFileSaveAs_Click(object sender, EventArgs e)
        {
            if (S.Instance.Demo)
            {
                MessageBox.Show(this, S.Resources.GetString("This feature is disabled in this demo edition."), S.Resources.GetString("Demo Edition"));
            }
            else
            {
                bool flag2 = false;
                if (S.Instance.SimTimeRunning)
                {
                    flag2 = true;
                    this.mnuOptionsGoStop_Click(new object(), new EventArgs());
                }
                string str = Environment.CurrentDirectory + @"\";
                if (Directory.Exists(Environment.CurrentDirectory + @"\saves\"))
                {
                    str = str + @"saves\";
                }
                SaveFileDialog dialog2 = new SaveFileDialog {
                    InitialDirectory = str
                };
                string[] textArray1 = new string[] { S.Instance.DataFileTypeName, " files (*.", S.Instance.DataFileTypeExtension, ")|*.", S.Instance.DataFileTypeExtension, "|All files (*.*)|*.*" };
                dialog2.Filter = string.Concat(textArray1);
                dialog2.DefaultExt = S.Instance.DataFileTypeExtension;
                SaveFileDialog dialog = dialog2;
                if (S.Instance.UserAdminSettings.DefaultDirectory != null)
                {
                    dialog.InitialDirectory = S.Instance.UserAdminSettings.DefaultDirectory;
                }
                while (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        this.SaveSim(dialog.FileName);
                        this.lessonLoadedPromptSaveAs = false;
                        this.currentFilePath = dialog.FileName;
                        this.Text = Path.GetFileNameWithoutExtension(this.currentFilePath) + " - " + Application.ProductName;
                        if (S.Instance.SimStateAdapter.getMultiplayer())
                        {
                            string text = this.Text;
                            string[] textArray2 = new string[] { text, " Multiplayer   Role: ", this.ClientOrHost(), "   Session Name: ", Simulator.Instance.SessionName };
                            this.Text = string.Concat(textArray2);
                        }
                        break;
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(S.Resources.GetString("Could not save file. File may be read-only or in use by another application.") + "\r\n\r\n" + S.Resources.GetString("Error details") + ": " + exception.Message, S.Resources.GetString("Could Not Save"));
                    }
                }
                if (flag2)
                {
                    this.mnuOptionsGoStop_Click(new object(), new EventArgs());
                }
            }
        }

        private void mnuHelpAbout_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog(this);
        }

        private void mnuHelpAssignment_Click(object sender, EventArgs e)
        {
            byte[] buffer = S.Adapter.getPdfAssignment();
            if (Thread.CurrentThread.CurrentUICulture.Name != "")
            {
                Hashtable hashtable = (Hashtable) S.State.Reserved["LocalLanguageAssignments"];
                if (hashtable != null)
                {
                    byte[] buffer2 = (byte[]) hashtable[Thread.CurrentThread.CurrentUICulture.Name];
                    if (buffer2 != null)
                    {
                        buffer = buffer2;
                    }
                }
            }
            string tempPath = Path.GetTempPath();
            string path = null;
            int num = 100;
            int num2 = 0;
            while (num2 < num)
            {
                try
                {
                    object[] objArray1 = new object[] { tempPath, "Printable Virtual Business Assignment.pdf", num2, "q.pdf" };
                    path = string.Concat(objArray1);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    break;
                }
                catch (IOException)
                {
                }
                num2++;
            }
            if (num2 >= num)
            {
                MessageBox.Show("Too many assignment files are being viewed at one time.  Try closing some first, then try viewing this assignment again.");
            }
            else
            {
                try
                {
                    FileStream output = new FileStream(path, FileMode.Create);
                    new BinaryWriter(output).Write(buffer);
                    output.Close();
                    Process.Start(path);
                }
                catch (Win32Exception)
                {
                    MessageBox.Show("Could not find Adobe Acrobat Reader to display assignment. Either download and install Acrobat Reader from www.Adobe.com or have your instructor copy the assignment out of the Instructor's Manual.");
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Could not display assignment. Please have your instructor copy the assignment out of the Instructor's Manual.\r\n\r\nDetailed problem: " + exception.Message);
                }
            }
        }

        private void mnuHelpSearch_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenSearch();
        }

        private void mnuHelpTopicsAndIndex_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp();
        }

        private void mnuHelpTutorial_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp();
        }

        public virtual void mnuOptionsBackgroundMusic_Click(object sender, EventArgs e)
        {
            this.mnuOptionsBackgroundMusic.Checked = !this.mnuOptionsBackgroundMusic.Checked;
            if (!this.mnuOptionsBackgroundMusic.Checked)
            {
                this.StopMusic();
            }
            if (this.mnuOptionsBackgroundMusic.Checked && S.Instance.SimTimeRunning)
            {
                this.backgroundMusicTimer.Enabled = true;
                this.StartMusic();
            }
        }

        private void mnuOptionsChangeOwner_Click(object sender, EventArgs e)
        {
            frmInputString errantForm = new frmInputString("Change Owner", "Enter the player name of the new owner. Enter USER for single player user. Enter AI for a new AI player.", "");
            if (errantForm.ShowDialog() == DialogResult.OK)
            {
                string response = errantForm.Response;
                if (response.ToUpper() == "USER")
                {
                    response = "";
                }
                if (response.ToUpper() == "AI")
                {
                    response = S.Adapter.CreatePlayer(Guid.NewGuid().ToString(), PlayerType.AI).PlayerName;
                }
                try
                {
                    S.Adapter.ChangeEntityOwner(S.MainForm.CurrentEntityID, response);
                }
                catch (SimApplicationException exception)
                {
                    MessageBox.Show(exception.Message, "Couldn't change owner");
                }
                catch (Exception exception2)
                {
                    frmExceptionHandler.Handle(exception2, errantForm);
                }
            }
        }

        private void mnuOptionsFaster_Click(object sender, EventArgs e)
        {
            SimState simState = S.Instance.SimState;
            simState.SpeedIndex++;
            this.OnSpeedChange();
        }

        public void mnuOptionsGoStop_Click(object sender, EventArgs e)
        {
            if (!S.Instance.SimTimeRunning)
            {
                this.dirtySimState = true;
                S.Instance.StartSimTimeRunning();
            }
            else
            {
                S.Instance.StopSimTimeRunning();
            }
            this.SynchGoStop();
        }

        protected void mnuOptionsIACustomizeYourSim_Click(object sender, EventArgs e) {
            frmPassword password = new frmPassword(S.Instance.UserAdminSettings.GetP());
            if (this.DesignerMode || (password.ShowDialog(this) == DialogResult.OK))
            {
                frmEditSimSettings errantForm = null;
                try
                {
                    errantForm = new frmEditSimSettings();
                    errantForm.ShowDialog();
                    this.EnableDisable();
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception, errantForm);
                }
            }
        }

        protected virtual void mnuOptionsIAProvideCash_Click(object sender, EventArgs e)
        {
            frmPassword password = new frmPassword(S.Instance.UserAdminSettings.GetP());
            if (this.DesignerMode || (password.ShowDialog(this) == DialogResult.OK))
            {
                try
                {
                    new frmProvideCash().ShowDialog(this);
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }

        private void mnuOptionsLanguage_Click(object sender, EventArgs e)
        {
        }

        private void mnuOptionsMacrosPlayMacro_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.macroActions.Clear();
                FileStream serializationStream = null;
                try
                {
                    serializationStream = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    IFormatter formatter = new BinaryFormatter();
                    while (serializationStream.Position < serializationStream.Length)
                    {
                        MacroAction action = (MacroAction) formatter.Deserialize(serializationStream);
                        this.macroActions.Add(action);
                    }
                }
                catch (Exception)
                {
                    this.macroActions.Clear();
                    MessageBox.Show("Had problems deserializing from " + dialog.FileName);
                }
                finally
                {
                    if (serializationStream != null)
                    {
                        serializationStream.Close();
                    }
                }
                frmPlayMacro macro = new frmPlayMacro();
                if (macro.ShowDialog(this) == DialogResult.OK)
                {
                    this.playLooping = macro.optContinuously.Checked;
                    this.playIntervalMilliseconds = (long) macro.updInterval.Value;
                    this.mnuOptionsMacrosPlayMacro.Enabled = false;
                    this.mnuOptionsMacrosStopPlaying.Enabled = true;
                    this.macroPlayingOn = true;
                    this.mnuOptionsMacrosRecordMacro.Enabled = false;
                    this.mnuOptionsMacrosStopPlaying.Enabled = true;
                }
            }
        }

        private void mnuOptionsMacrosRecordMacro_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                this.macroFilename = dialog.FileName;
                this.mnuOptionsMacrosRecordMacro.Enabled = false;
                this.mnuOptionsMacroStopRecording.Enabled = true;
                this.mnuOptionsMacrosPlayMacro.Enabled = false;
                this.macroRecordingOn = true;
            }
        }

        private void mnuOptionsMacrosStopPlaying_Click(object sender, EventArgs e)
        {
            this.macroPlayingOn = false;
            this.macroActions = null;
            this.mnuOptionsMacrosStopPlaying.Enabled = false;
            this.mnuOptionsMacrosRecordMacro.Enabled = true;
        }

        private void mnuOptionsMacroStopRecording_Click(object sender, EventArgs e)
        {
            this.macroRecordingOn = false;
            this.mnuOptionsMacroStopRecording.Enabled = false;
            this.mnuOptionsMacrosRecordMacro.Enabled = true;
            this.mnuOptionsMacrosPlayMacro.Enabled = true;
        }

        private void mnuOptionsRenameEntity_Click(object sender, EventArgs e)
        {
            frmInputString str = new frmInputString("Rename " + S.Instance.EntityName, "Enter new name:", "");
            str.ShowDialog();
            if ((str.Response != null) && (str.Response != ""))
            {
                S.Adapter.RenameEntity(S.MainForm.CurrentEntityID, str.Response);
            }
            this.UpdateView();
        }

        private void mnuOptionsRunTo_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop.PerformClick();
            }
            new frmRunTo().ShowDialog(this);
        }

        protected virtual void mnuOptionsShowMessages_Click(object sender, EventArgs e)
        {
            if (!this.mnuOptionsShowMessages.Checked)
            {
                this.ShowMessageWindow();
            }
            else
            {
                this.HideMessageWindow();
            }
        }

        public void mnuOptionsSlower_Click(object sender, EventArgs e)
        {
            SimState simState = S.Instance.SimState;
            simState.SpeedIndex--;
            this.OnSpeedChange();
        }

        private void mnuOptionsSoundEffects_Click(object sender, EventArgs e)
        {
            this.mnuOptionsSoundEffects.Checked = !this.mnuOptionsSoundEffects.Checked;
        }

        private void mnuOptionsTestResults_Click(object sender, EventArgs e)
        {
            if (S.Instance.SimTimeRunning)
            {
                this.mnuOptionsGoStop.PerformClick();
            }
            new frmTestResults().ShowDialog(this);
        }

        protected virtual void mnuOptionsTuning_Click(object sender, EventArgs e)
        {
        }

        private void mnuViewEntity_Click(object sender, EventArgs e)
        {
            this.OnCurrentEntityChange(this.EntityNameToID(((MenuItem) sender).Text));
            this.UpdateView();
        }

        private void mnuViewView_Click(object sender, EventArgs e)
        {
            this.OnViewChange(((MenuItem) sender).Text);
            if (this.SoundOn)
            {
                Sound.PlaySoundFromFile(@"sounds\viewchange.wav");
            }
        }

        public void OnCurrentEntityChange(long entityID)
        {
            this.CurrentEntityID = entityID;
            this.CloseActionForms();
            foreach (MenuItem item2 in this.mnuView.MenuItems)
            {
                item2.Checked = item2.Text == this.EntityIDToName(this.CurrentEntityID);
            }
            this.CurrentEntityPanel.Text = this.EntityIDToName(this.CurrentEntityID);
            try
            {
                this.EnableDisable();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
            MenuItem item = Utilities.FindMenuEquivalent(this.mainMenu1, this.CurrentViewName);
            if ((item != null) && !item.Enabled)
            {
                this.OnViewChange(S.Instance.Views[0].Name);
            }
            if (this.EntityChanged != null)
            {
                this.EntityChanged(this, new EventArgs());
            }
        }

        public void ChangeSpeed(int mod) {
            S.Instance.SimState.SpeedIndex += mod;
            OnSpeedChange();
        }

        private void OnSpeedChange()
        {
            this.mnuOptionsFaster.Enabled = S.Instance.SimState.SpeedIndex < (S.State.Speeds.Length - 1);
            this.mnuOptionsSlower.Enabled = S.Instance.SimState.SpeedIndex > 0;
            this.ReenableButtons();
        }

        private void OnStateChanged()
        {
            if (!S.Instance.Client && S.Instance.Demo)
            {
                S.Settings.StopDate = S.Settings.StartDate.AddDays((double) S.Instance.DemoDuration);
            }
            this.Text = Application.ProductName;
            if (this.currentFilePath != null)
            {
                this.Text = Path.GetFileNameWithoutExtension(this.currentFilePath) + " - " + this.Text;
            }
            bool flag = S.Instance.SimStateAdapter.getMultiplayer();
            if (this.ScoreboardButton != null)
            {
                this.ScoreboardButton.Visible = flag;
            }
            this.mnuFileMultiplayerScoreboard.Enabled = flag;
            this.mnuFileMultiplayerTeamList.Visible = flag && S.Instance.Host;
            if (flag)
            {
                if (S.Instance.Client)
                {
                    this.CurrentEntityID = S.Adapter.GetAnEntityIdForPlayer(S.Instance.ThisPlayerName);
                }
                else
                {
                    frmStartMultiplayerSession session = new frmStartMultiplayerSession();
                    if (session.ShowDialog() == DialogResult.Cancel)
                    {
                        this.GetCorrectStartChoices().ShowDialog(this);
                        return;
                    }
                    S.Instance.ThisPlayerName = "";
                    S.State.RoleBasedMultiplayer = session.chkRequireRoles.Checked;
                }
                string text = this.Text;
                string[] textArray1 = new string[] { text, " Multiplayer ", this.ClientOrHost(), "   Session Name: ", Simulator.Instance.SessionName };
                this.Text = string.Concat(textArray1);
                if (S.Instance.MultiplayerRoleName != "")
                {
                    this.Text = this.Text + "   Role: " + S.Instance.MultiplayerRoleName;
                }
            }
            this.OnCurrentEntityChange(this.CurrentEntityID);
            if (!S.Instance.Client)
            {
                this.OnSpeedChange();
            }
            else
            {
                S.Instance.SetSimEngineSpeed(new SimSpeed(S.Instance.UserAdminSettings.ClientDrawStepPeriod, 1));
                this.mnuFileSave.Enabled = false;
                this.mnuFileSaveAs.Enabled = false;
                this.mnuOptionsFaster.Enabled = false;
                this.mnuOptionsSlower.Enabled = false;
                this.mnuOptionsRunTo.Enabled = false;
                this.ReenableButtons();
            }
            if (this.CurrentViewName == "")
            {
                this.CurrentViewName = S.Instance.Views[0].Name;
            }
            this.OnViewChange(this.CurrentViewName);
            if (this.mnuHelpAssignment.Enabled)
            {
                Form form = this.FindOwnedForm(typeof(frmStartChoices));
                if (form != null)
                {
                    form.Location = new Point(0, 0x7530);
                }
                if (MessageBox.Show("Do you want to view or print your assignment?", "View Assignment", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.mnuHelpAssignment.PerformClick();
                }
            }
            this.RegisterClientEventHandlers();
            this.OnStateChangedHook();
        }

        protected virtual void OnStateChangedHook()
        {
        }

        public void OnViewChange(string viewName)
        {
            this.CloseActionForms();
            KMI.Sim.View.ClearCurrentHit();
            this.CurrentViewName = viewName;
            this.picMain.Visible = false;
            this.backBuffer = new Bitmap(this.currentView.Size.Width, this.currentView.Size.Height, this.picMain.CreateGraphics());
            this.backBufferGraphics = Graphics.FromImage(this.backBuffer);
            this.picMain.Size = this.currentView.Size;
            this.frmMainBase_Resize(new object(), new EventArgs());
            this.UpdateView();
            this.picMain.Visible = true;
        }

        public virtual void OpenSavedSim(string filepath)
        {
            this.Cursor = Cursors.WaitCursor;
            this.ReInit();
            try
            {
                S.Instance.LoadState(filepath);
                this.currentViewName = S.State.SavedViewName;
                this.currentEntityID = S.State.SavedEntityID;
                S.State.SpeedIndex = S.State.SpeedIndex;
                this.LoadStateHook();
            }
            catch (Exception exception)
            {
                string[] textArray1 = new string[5];
                string[] args = new string[] { Application.ProductName, Simulator.Instance.DataFileTypeExtension };
                textArray1[0] = S.Resources.GetString("An error occurred while opening the file. Check that the file is a valid {0} (.{1}) file. If the error continues, the file may have been corrupted.", args);
                textArray1[1] = "\r\n\r\n";
                textArray1[2] = S.Resources.GetString("Error details");
                textArray1[3] = ": ";
                textArray1[4] = exception.Message;
                MessageBox.Show(string.Concat(textArray1), S.Resources.GetString("Error Opening File"));
                this.GetCorrectStartChoices().ShowDialog(this);
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            this.currentFilePath = filepath;
            if (S.Settings.StudentOrg > 0)
            {
                MessageBox.Show("You have opened a Virtual Business Challenge file. The Instructor's Area will be disabled.", "Virtual Business Challenge");
            }
            else if (S.Instance.VBC)
            {
                MessageBox.Show(S.Resources.GetString("The file you are trying to load was not built for this Edition. It can be loaded only by the classroom version."));
                this.GetCorrectStartChoices().ShowDialog(this);
            }
            this.OnStateChanged();
        }

        public virtual bool OptOutModalMessageHook(ModalMessage message)
        {
            return false;
        }

        private void picMain_Click(object sender, EventArgs e)
        {
            this.currentView.View_Click(sender, e);
        }

        private void picMain_DoubleClick(object sender, EventArgs e)
        {
            this.currentView.View_DoubleClick(sender, e);
        }

        private void picMain_MouseDown(object sender, MouseEventArgs e)
        {
            this.currentView.View_MouseDown(sender, e);
        }

        private void picMain_MouseMove(object sender, MouseEventArgs e)
        {
            this.currentView.UpdateCurrentHit(e);
            this.currentView.View_MouseMove(sender, e);
        }

        private void picMain_MouseUp(object sender, MouseEventArgs e)
        {
            this.currentView.View_MouseUp(sender, e);
        }

        private void picMain_Paint(object sender, PaintEventArgs e)
        {
            if (!base.DesignMode && (this.backBuffer != null))
            {
                e.Graphics.DrawImageUnscaled(this.backBuffer, 0, 0);
            }
        }

        public void PlayMacroAction()
        {
            if (this.macroPlayingOn)
            {
                MacroAction action;
                if (this.playLooping)
                {
                    if (this.currentMacroActionIndex >= this.macroActions.Count)
                    {
                        this.currentMacroActionIndex = 0;
                    }
                    action = (MacroAction) this.macroActions[this.currentMacroActionIndex];
                    if (this.nextMacroPlayTime < DateTime.Now)
                    {
                        action.Method.Invoke(S.Adapter, action.ArgumentValues);
                        this.nextMacroPlayTime = this.nextMacroPlayTime.AddMilliseconds((double) this.playIntervalMilliseconds);
                        this.currentMacroActionIndex++;
                    }
                }
                else if (!S.Instance.Client)
                {
                    while (this.currentMacroActionIndex < this.macroActions.Count)
                    {
                        action = (MacroAction) this.macroActions[this.currentMacroActionIndex];
                        if (action.Timestamp >= S.State.Now)
                        {
                            break;
                        }
                        action.Method.Invoke(S.Adapter, action.ArgumentValues);
                        this.currentMacroActionIndex++;
                    }
                }
            }
        }

        private bool QuerySave()
        {
            if (S.Instance.Demo)
            {
                return true;
            }
            if (!this.dirtySimState || S.Instance.Client)
            {
                return true;
            }
            switch (MessageBox.Show(S.Instance.Resource.GetString("MsgQuerySave"), Application.ProductName, MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Cancel:
                    return false;

                case DialogResult.Yes:
                    this.mnuFileSave_Click(new object(), new EventArgs());
                    return !this.dirtySimState;

                case DialogResult.No:
                    this.dirtySimState = false;
                    return true;
            }
            return false;
        }

        private void ReenableButtons()
        {
            foreach (ToolBarButton button in this.tlbMain.Buttons)
            {
                MenuItem item = Utilities.FindMenuEquivalent(this.mainMenu1, button.Text);
                if (item != null)
                {
                    button.Enabled = item.Enabled;
                }
            }
        }

        protected virtual void RegisterClientEventHandlers()
        {
            S.Adapter.PlaySoundEvent += new PlaySoundDelegate(ClientEventHandlers.Instance.PlaySoundHandler);
            S.Adapter.PlayerMessageEvent += new PlayerMessageDelegate(ClientEventHandlers.Instance.PlayerMessageHandler);
            S.Adapter.ModalMessageEvent += new ModalMessageDelegate(ClientEventHandlers.Instance.ModalMessageHandler);
        }

        private void ReInit()
        {
            this.CloseOwnedForms();
            if (S.Adapter != null)
            {
                this.UnregisterClientEventHandlers();
            }
            if ((this.DesignerMode && !S.Instance.Client) && (S.Instance.SimState != null))
            {
                this.cacheDesignerSimSettings = S.Instance.SimState.SimSettings;
            }
            S.Instance.SimState = null;
            S.Instance.PeriodicMessageTable.Clear();
            try
            {
                if ((S.Adapter != null) && !S.Instance.Client)
                {
                    RemotingServices.Disconnect(S.Adapter);
                }
            }
            catch
            {
                throw new Exception("The state of the SimStateAdapter and the Client flag are somehow out of sync.");
            }
            S.Instance.Client = false;
            S.Instance.MultiplayerRoleName = "";
            this.CreateMessagePanel.Width = 0;
            S.Instance.SessionName = "";
            S.Instance.SimStateAdapter = S.Instance.SimFactory.CreateSimStateAdapter();
            S.Instance.ThisPlayerName = "";
            this.mnuFileSave.Enabled = true;
            this.mnuFileSaveAs.Enabled = true;
            this.mnuOptionsGoStop.Enabled = true;
            this.mnuOptionsFaster.Enabled = true;
            this.mnuOptionsSlower.Enabled = true;
            this.mnuOptionsIA.Enabled = true;
            this.mnuOptionsRunTo.Enabled = true;
            this.EnableMenuAndSubMenus(this.mnuReports);
            this.EnableMenuAndSubMenus(this.mnuActions);
            this.lessonLoadedPromptSaveAs = false;
            this.currentWeek = -1;
            this.Now = DateNotSet;
            for (int i = this.mnuView.MenuItems.Count - 1; (i >= 0) && (this.mnuView.MenuItems[i].Text != "-"); i--)
            {
                this.mnuView.MenuItems.RemoveAt(i);
            }
            this.CurrentViewName = S.Instance.Views[0].Name;
            this.CurrentEntityID = -1L;
            this.NewMessagesPanel.Width = 0;
            if (S.Instance.Messages)
            {
                this.messagesForm = new frmMessages();
                this.messagesForm.Controller.Add(this);
                this.messagesForm.Owner = this;
                this.ShowMessageWindow();
            }
        }

        public void SaveMacroAction(MacroAction action)
        {
            if (this.macroRecordingOn)
            {
                FileStream serializationStream = null;
                serializationStream = new FileStream(this.macroFilename, FileMode.Append, FileAccess.Write, FileShare.None);
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(serializationStream, action);
                serializationStream.Close();
            }
        }

        private void SaveSim(string filename)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                S.State.SavedEntityID = this.CurrentEntityID;
                S.State.SavedViewName = this.CurrentViewName;
                S.Instance.SaveState(filename);
                this.dirtySimState = false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        protected virtual void ServerStartHook()
        {
        }

        public void SetDirty()
        {
            this.dirtySimState = true;
        }

        protected void ShowMessageWindow()
        {
            this.mnuOptionsShowMessages.Checked = true;
            this.messagesForm.Show();
            this.messagesForm.Location = base.PointToScreen(new Point(0, this.staMain.Top - this.messagesForm.Height));
            this.NewMessagesPanel.Width = 0;
        }

        public void ShowModalMessage(ModalMessage message)
        {
            bool flag = false;
            if (S.Instance.SimTimeRunning && !S.Instance.Multiplayer)
            {
                flag = true;
                this.mnuOptionsGoStop_Click(new object(), new EventArgs());
            }
            if (message is RunToDateReachedMessage)
            {
                this.UpdateView();
                this.SynchGoStop();
            }
            else if (message is StopDateReachedMessage)
            {
                this.UpdateView();
                new frmUpload().ShowDialog(S.MainForm);
                this.SynchGoStop();
            }
            else
            {
                if (message is ShowPageMessage)
                {
                    new frmPage(((ShowPageMessage) message).Page).ShowDialog();
                }
                else if (message is LevelEndTestMessage)
                {
                    LevelEndTestMessage m = (LevelEndTestMessage) message;
                    AcademicGod.HandleLevelEnd(m);
                    if (m.LastLevel)
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(message.Message, message.Title, MessageBoxButtons.OK, message.Icon);
                    if (message is GameOverMessage)
                    {
                        if (!S.Instance.Client)
                        {
                            this.SynchGoStop();
                        }
                        this.DirtySimState = false;
                        this.GetCorrectStartChoices().ShowDialog(this);
                        return;
                    }
                }
                if (flag)
                {
                    this.mnuOptionsGoStop_Click(new object(), new EventArgs());
                }
            }
        }

        protected void ShowNonModalForm(Form frm)
        {
            Form form = this.FindOwnedForm(frm.GetType());
            if (form == null)
            {
                frm.Owner = this;
                if (this.CanShowForm(frm))
                {
                    frm.Show();
                }
            }
            else
            {
                form.Focus();
            }
        }

        private void staMain_PanelClick(object sender, StatusBarPanelClickEventArgs e)
        {
            if (e.StatusBarPanel == this.NewMessagesPanel)
            {
                this.ShowMessageWindow();
            }
            if (e.StatusBarPanel == this.CreateMessagePanel)
            {
                new frmCreateMessage().ShowDialog(this);
            }
        }

        protected void StartMusic()
        {
            this.backgroundMusicTimer.Stop();
            this.backgroundMusicTimer.Interval = S.Instance.BackgroundMusicLength;
            Sound.PlayMidiFromFile(@"sounds\Background.Mid");
            this.backgroundMusicTimer.Start();
        }

        public void StopMusic()
        {
            Sound.StopMidi();
            this.backgroundMusicTimer.Stop();
        }

        protected internal void StopSimulation()
        {
            if (S.Instance.SimTimeRunning)
            {
                S.MainForm.mnuOptionsGoStop.PerformClick();
            }
        }

        public void SynchGoStop()
        {
            ToolBarButton button;
            if (!S.Instance.SimTimeRunning && this.mnuOptionsGoStop.Text.EndsWith(S.Instance.Resource.GetString("Stop")))
            {
                button = Utilities.FindButtonEquivalent(this.tlbMain, this.mnuOptionsGoStop.Text);
                this.mnuOptionsGoStop.Text = S.Instance.Resource.GetString("Go");
                if (button != null)
                {
                    button.Text = this.mnuOptionsGoStop.Text;
                    button.ImageIndex--;
                }
                this.StopMusic();
            }
            if (S.Instance.SimTimeRunning && this.mnuOptionsGoStop.Text.EndsWith(S.Instance.Resource.GetString("Go")))
            {
                button = Utilities.FindButtonEquivalent(this.tlbMain, this.mnuOptionsGoStop.Text);
                this.mnuOptionsGoStop.Text = S.Instance.Resource.GetString("Stop");
                if (button != null)
                {
                    button.Text = this.mnuOptionsGoStop.Text;
                    button.ImageIndex++;
                }
                if (this.MusicOn)
                {
                    this.StartMusic();
                }
            }
        }

        private void tlbMain_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {
            MenuItem item = Utilities.FindMenuEquivalent(this.mainMenu1, e.Button.Text);
            if (item == null)
            {
                throw new Exception("A main toolbar button click was unhandled. Check the toolbar buttons text property.");
            }
            item.PerformClick();
        }

        protected virtual void UnregisterClientEventHandlers()
        {
            try {
                if (S.Adapter == null) { return; }
                S.Adapter.PlaySoundEvent -= new PlaySoundDelegate(ClientEventHandlers.Instance.PlaySoundHandler);
                S.Adapter.PlayerMessageEvent -= new PlayerMessageDelegate(ClientEventHandlers.Instance.PlayerMessageHandler);
                S.Adapter.ModalMessageEvent -= new ModalMessageDelegate(ClientEventHandlers.Instance.ModalMessageHandler);
            }
            catch { }
        }

        protected virtual void UpdateStatusBar(SimStateAdapter.ViewUpdate viewUpdate)
        {
            this.Now = viewUpdate.Now;
            this.CurrentWeek = viewUpdate.CurrentWeek;
            this.EntityCriticalResourcePanel.Text = Utilities.FC(viewUpdate.Cash, S.Instance.CurrencyConversion);
            if (!S.Instance.Client && S.Settings.LevelManagementOn)
            {
                object[] args = new object[] { S.Settings.Level.ToString() };
                this.Level.Text = S.Resources.GetString("Level {0}", args);
            }
            else
            {
                this.Level.Text = "";
            }
        }

        public void UpdateView()
        {
            SimStateAdapter.ViewUpdate viewUpdate = null;
            try
            {
                viewUpdate = S.Adapter.GetViewUpdate(this.CurrentViewName, this.CurrentEntityID, this.currentView.ViewerOptions);
            }
            catch (EntityNotFoundException exception)
            {
                this.OnCurrentEntityChange(exception.ExistingEntityID);
                this.UpdateView();
                return;
            }
            catch (Exception exception2)
            {
                frmExceptionHandler.Handle(exception2);
                return;
            }
            this.FireNewTimeEvents(viewUpdate);
            this.UpdateStatusBar(viewUpdate);
            this.UpdateViewMenu(viewUpdate);
            this.backBufferGraphics.Clear(Color.White);
            this.currentView.Drawables = viewUpdate.Drawables;
            this.currentView.Draw(this.backBufferGraphics);
            this.picMain.Refresh();
        }

        protected virtual void UpdateViewMenu(SimStateAdapter.ViewUpdate viewUpdate)
        {
            this.entityNames = viewUpdate.EntityNames;
            bool flag = false;
            int index = this.mnuView.MenuItems.Count - 1;
            while ((index >= 0) && (this.mnuView.MenuItems[index].Text != "-"))
            {
                index--;
            }
            int num2 = index + 1;
            if ((this.mnuView.MenuItems.Count - num2) != viewUpdate.EntityNames.Count)
            {
                flag = true;
            }
            else
            {
                int num3 = 0;
                foreach (string str in viewUpdate.EntityNames.Values)
                {
                    if (this.mnuView.MenuItems[num2 + num3++].Text != str)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
            {
                for (index = this.mnuView.MenuItems.Count - 1; (index >= 0) && (this.mnuView.MenuItems[index].Text != "-"); index--)
                {
                    this.mnuView.MenuItems.RemoveAt(index);
                }
                foreach (string str2 in viewUpdate.EntityNames.Values)
                {
                    MenuItem item = new MenuItem(str2, new EventHandler(this.mnuViewEntity_Click));
                    this.mnuView.MenuItems.Add(item);
                    item.Checked = item.Text == this.EntityIDToName(this.CurrentEntityID);
                }
                this.CurrentEntityPanel.Text = this.EntityIDToName(this.CurrentEntityID);
                if ((this.CurrentEntityID == -1L) && (viewUpdate.EntityNames.Count > 0))
                {
                    foreach (long num4 in viewUpdate.EntityNames.Keys)
                    {
                        this.OnCurrentEntityChange(num4);
                        break;
                    }
                }
            }
        }

        private void View_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Utilities.ResetFPU();
                Font font = new Font("Arial", 14f);
                string text = "";
                if (this.printStudentName != "")
                {
                    text = text + "Student Name: " + this.printStudentName + "\r\n\r\n";
                }
                text = text + this.CurrentViewName + " View";
                if (S.Instance.EntityName != "")
                {
                    string str2 = text;
                    string[] textArray1 = new string[] { str2, "\r\nCurrent ", S.Instance.EntityName, ": ", this.EntityIDToName(this.CurrentEntityID) };
                    text = string.Concat(textArray1);
                }
                int num = (int) (e.Graphics.MeasureString(text, font).Height * 1.5f);
                Rectangle marginBounds = e.MarginBounds;
                Rectangle rectangle2 = new Rectangle(marginBounds.Left, marginBounds.Top + num, marginBounds.Width, marginBounds.Height - num);
                float sx = Math.Min((float) (((float) rectangle2.Width) / ((float) this.picMain.Width)), (float) (((float) rectangle2.Height) / ((float) this.picMain.Height)));
                float dx = rectangle2.Left + ((rectangle2.Width - (this.picMain.Width * sx)) / 2f);
                float dy = rectangle2.Top + ((rectangle2.Height - (this.picMain.Height * sx)) / 2f);
                e.Graphics.TranslateTransform(dx, dy);
                e.Graphics.ScaleTransform(sx, sx);
                e.Graphics.DrawImageUnscaled(this.backBuffer, 0, 0);
                e.Graphics.ResetTransform();
                e.Graphics.DrawString(text, font, new SolidBrush(Color.Black), (float) e.MarginBounds.Left, (float) e.MarginBounds.Top);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public Graphics BackBufferGraphics
        {
            get
            {
                return this.backBufferGraphics;
            }
        }

        public long CurrentEntityID
        {
            get
            {
                return this.currentEntityID;
            }
            set
            {
                this.currentEntityID = value;
            }
        }

        public string CurrentViewName
        {
            get
            {
                return this.currentViewName;
            }
            set
            {
                this.currentViewName = value;
                this.currentView = S.Instance.View(this.currentViewName);
            }
        }

        public int CurrentWeek
        {
            get
            {
                return this.currentWeek;
            }
            set
            {
                this.currentWeek = value;
            }
        }

        public bool DesignerMode
        {
            get
            {
                return this.designerMode;
            }
            set
            {
                this.designerMode = value;
                this.mnuOptionsTuning.Visible = this.designerMode;
                this.mnuOptionsChangeOwner.Visible = this.designerMode;
                this.mnuOptionsMacros.Visible = this.designerMode;
                this.mnuOptionsRenameEntity.Visible = this.designerMode;
            }
        }

        public bool DirtySimState
        {
            get
            {
                return this.dirtySimState;
            }
            set
            {
                this.dirtySimState = value;
            }
        }

        public static frmMainBase Instance
        {
            get
            {
                return instance;
            }
        }

        public bool IsWin98
        {
            get
            {
                return this.isWin98;
            }
        }

        public Rectangle MainWindowBounds
        {
            get
            {
                return new Rectangle(0, 0, this.picMain.Width, this.picMain.Height);
            }
        }

        public bool MusicOn
        {
            get
            {
                return this.mnuOptionsBackgroundMusic.Checked;
            }
        }

        public DateTime Now
        {
            get
            {
                return this.now;
            }
            set
            {
                this.now = value;
                if (this.now != DateNotSet)
                {
                    this.TimePanel.Text = this.now.ToShortTimeString();
                    this.DayOfWeekPanel.Text = this.now.ToString("ddd");
                    this.DatePanel.Text = this.now.ToString("MMMM dd, yyyy");
                }
            }
        }

        public bool SoundOn
        {
            get
            {
                return this.mnuOptionsSoundEffects.Checked;
            }
        }

        public ToolTip ViewToolTip
        {
            get
            {
                return this.viewToolTip;
            }
        }

        public class MenuItemEnabledNoEntities : MenuItem
        {
        }
    }
}

