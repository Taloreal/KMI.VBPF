using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using KMI.Biz.City;
using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmMain : frmMainBase
    {
        //private IContainer components;
        private readonly IContainer components;
        private ToolBarButton HELP;
        //private ResourceManager manager;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private MenuItem menuItem4;
        private MenuItem menuItem5;
        private MenuItem menuItem50;
        private MenuItem menuItem51;
        private MenuItem menuItem52;
        private MenuItem menuItem53;
        private MenuItem menuItem54;
        private MenuItem menuItem56;
        private MenuItem menuItem6;
        private MenuItem menuItem7;
        private MenuItem menuItem8;
        private MenuItem menuItem9;
        private MenuItem mnuActionsCredit;
        public MenuItem mnuActionsCreditCreditCards;
        public MenuItem mnuActionsCreditForGoods;
        public MenuItem mnuActionsCreditInternet;
        public MenuItem mnuActionsCreditSellCar;
        public MenuItem mnuActionsCreditShopForCar;
        public MenuItem mnuActionsCreditShopForFood;
        public MenuItem mnuActionsCreditShopForGas;
        private MenuItem mnuActionsIncome;
        public MenuItem mnuActionsIncome401K;
        public MenuItem mnuActionsIncomeEducation;
        public MenuItem mnuActionsIncomePayment;
        public MenuItem mnuActionsIncomeTaxes;
        public MenuItem mnuActionsIncomeWitholding;
        public MenuItem mnuActionsIncomeWork;
        public MenuItem mnuActionsInsurance;
        public MenuItem mnuActionsInsuranceAuto;
        public MenuItem mnuActionsInsuranceHealth;
        public MenuItem mnuActionsInsuranceHomeowners;
        public MenuItem mnuActionsInsuranceRenters;
        private MenuItem mnuActionsInvesting;
        public MenuItem mnuActionsInvestingMyPortfolio;
        public MenuItem mnuActionsInvestingResearchFunds;
        public MenuItem mnuActionsInvestingRetirement;
        private MenuItem mnuActionsLiving;
        private MenuItem mnuActionsLivingCondo;
        public MenuItem mnuActionsLivingHousing;
        public MenuItem mnuActionsLivingTimeMgt;
        public MenuItem mnuActionsLivingTransportation;
        private MenuItem mnuActionsMM;
        public MenuItem mnuActionsMMBanking;
        public MenuItem mnuActionsMMBills;
        public MenuItem mnuActionsMMOnlineBanking;
        public MenuItem mnuActionsShopBusTokens;
        private MenuItem mnuLoanStatements;
        private MenuItem mnuOptionsLegend;
        private MenuItem mnuOptionsProvideFood;
        private MenuItem mnuReportsActionsJournal;
        private MenuItem mnuReportsCheckbook;
        public MenuItem mnuReportsCreditReport;
        private MenuItem mnuReportsCreditScore;
        private MenuItem mnuReportsHealth;
        private MenuItem mnuReportsInvestmentStatements;
        private MenuItem mnuReportsPayAndTaxRecords;
        private MenuItem mnuReportsPersonalBalanceSheet;
        public MenuItem mnuReportsResume;
        private MenuItem mnuReportsRetirementStatements;
        public MenuItem mnuReportsSnapshot;
        //private string Password;
        private readonly string Password;
        private ComponentResourceManager resources;
        private ToolBarButton scoreboardButton;
        private ToolBarButton toolBarButton1;
        private ToolBarButton toolBarButton10;
        private ToolBarButton toolBarButton11;
        private ToolBarButton toolBarButton12;
        private ToolBarButton toolBarButton13;
        private ToolBarButton toolBarButton14;
        private ToolBarButton toolBarButton15;
        private ToolBarButton toolBarButton16;
        private ToolBarButton toolBarButton17;
        private ToolBarButton toolBarButton2;
        private ToolBarButton toolBarButton3;
        private ToolBarButton toolBarButton7;
        private ToolBarButton toolBarButton8;
        private ToolBarButton toolBarButton9;
        private string typed;

        public frmMain() {
            this.Password = "deadless";
            this.typed    = "        ";
            //Settings.SetUp(true);
            this.components = null;
            this.LoadBaseImages();
            this.InitializeComponent();
            Settings.GetValue<bool>("NO_BGM", out bool flag);
            base.mnuOptionsBackgroundMusic.Checked = !flag;
            base.backgroundMusicTimer.Enabled = !flag;
        }

        public frmMain(string[] args, bool demo, bool vbc, bool academic) : base(args, demo, vbc, academic) {
            this.Password = "deadless";
            this.typed    = "        ";
            //Settings.SetUp(true);
            this.components = null;
            this.LoadBaseImages();
            this.InitializeComponent();
            this.Init();
            Settings.GetValue<bool>("NO_BGM", out bool flag);
            base.backgroundMusicTimer.Enabled = !flag;
            base.backgroundMusicTimer.Stop();
            base.mnuOptionsBackgroundMusic.Checked = !flag;
        }

        protected override void ConstructSimulator()
        {
            Simulator.InitSimulator(new AppFactory());
        }

        public bool Disabled(MenuItem m)
        {
            if (!m.Enabled)
            {
                object[] args = new object[] { Utilities.NoEllipsis(m.Text) };
                MessageBox.Show(A.Resources.GetString("The action, {0}, is not currently enabled.", args), A.Resources.GetString("Action Not Enabled"));
            }
            return !m.Enabled;
        }

        protected override void Dispose(bool disposing)
        {
            base.StopMusic();
            Simulator.Instance.simEngine.stopEngine = true;
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void DoSimUpgrade()
        {
            ((AppSimState) S.State).GetPurchasables();
        }

        public override void EnableDisable()
        {
            base.EnableDisable();
            try
            {
                if (!A.Adapter.HasEntity(A.Instance.ThisPlayerName) && !A.Instance.Host)
                {
                    this.mnuActionsLivingHousing.Enabled = ((AppSimSettings) A.Adapter.getSimSettings()).ApartmentsForRentEnabledForOwner;
                    if (this.mnuActionsLivingHousing.Enabled)
                    {
                        this.mnuActionsLiving.Enabled = true;
                    }
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e) {
            if (e.KeyChar == '+' && S.Instance.SimState.SpeedIndex < (S.State.Speeds.Length - 1)) {
                ChangeSpeed(1);
                return;
            }
            else if (e.KeyChar == '-' && S.Instance.SimState.SpeedIndex > 0) {
                ChangeSpeed(-1);
                return;
            }
            this.typed = this.typed.Substring(1) + e.KeyChar.ToString();
            if (this.typed == this.Password) {
                Surprise.Enabled = !Surprise.Enabled;
                FoodEntry.FoodExpires = !FoodEntry.FoodExpires;
                FoodEntry.FoodConsumes = !FoodEntry.FoodConsumes;
                MessageBox.Show(
                    "Surprises allowed: " + Surprise.Enabled.ToString() + "\r\n" +
                    "Food Expires: " + FoodEntry.FoodExpires.ToString() + "\r\n" +
                    "Food Consumes: " + FoodEntry.FoodConsumes.ToString());
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                base.mnuView.Visible = false;
            }
            catch (Exception err) 
            {
                MessageBox.Show(err.Message);
            }
        }

        private void frmReportsCreditReport_Click(object sender, EventArgs e)
        {
            try
            {
                frmCreditReport frm = new frmCreditReport {
                    EnablingReference = this.mnuReportsCreditReport
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public override int GetVBCStudentOrgCode()
        {
            return 4;
        }

        public static void HandleError(Exception ex)
        {
            frmExceptionHandler.Handle(ex);
        }

        public void Init()
        {
            KMIHelp.LocalPath = Application.StartupPath + @"\Help\index.htm";
            KMIHelp.RemotePath = "http://help.knowledgematters.com/vbpf1/Help/index.htm";
            S.Instance.DataFileTypeExtension = "VBPF1";
            base.ScoreboardButton = this.scoreboardButton;
            S.Instance.EntityName = A.Resources.GetString("Career");
            KMI.Biz.City.City.BuildingTypes = (BuildingType[]) TableReader.Read(base.GetType().Assembly, typeof(BuildingType), "KMI.VBPF1Lib.Data.BuildingTypes.txt");
            for (int i = 0; i < KMI.Biz.City.City.BuildingTypes.Length; i++)
            {
                KMI.Biz.City.City.BuildingTypes[i].Height = A.Resources.GetImage("Building" + i).Height;
            }
            base.EntityCriticalResourceNamePanel.Text = A.Resources.GetString("Net Worth");
            base.CurrentEntityNamePanel.Text = "";
            S.Instance.SafeViewsForNoEntity.Add(S.Instance.Views[1].Name);
            Journal.JournalSeriesName = A.Resources.GetString("Net Worth");
            Journal.ScoreSeriesName = A.Resources.GetString("Net Worth");
            Journal.JournalNumericDataSeriesNames = new string[] { "Net Worth" };
            base.CurrentEntityPanel.MinWidth = 0;
            base.CurrentEntityPanel.Width = 0;
            this.mnuOptionsProvideFood.Visible = A.MainForm.DesignerMode;
            base.menuMessagesSep.Visible = false;
            S.Instance.DemoDuration = 180;
            base.tlbMain.labLogo.Image = A.Resources.GetImage("HRLogo");
            base.tlbMain.labLogo.Width = base.tlbMain.labLogo.Image.Width;
        }

        private void InitializeComponent()
        {
            this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton2 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton3 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton7 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton8 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton10 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton11 = new System.Windows.Forms.ToolBarButton();
            this.mnuReportsActionsJournal = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.scoreboardButton = new System.Windows.Forms.ToolBarButton();
            this.mnuReportsPersonalBalanceSheet = new System.Windows.Forms.MenuItem();
            this.mnuReportsCreditScore = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncome = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncomeWork = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncomeEducation = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncomeTaxes = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncomeWitholding = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncomePayment = new System.Windows.Forms.MenuItem();
            this.mnuActionsIncome401K = new System.Windows.Forms.MenuItem();
            this.mnuActionsMM = new System.Windows.Forms.MenuItem();
            this.mnuActionsMMBills = new System.Windows.Forms.MenuItem();
            this.mnuActionsMMBanking = new System.Windows.Forms.MenuItem();
            this.mnuActionsMMOnlineBanking = new System.Windows.Forms.MenuItem();
            this.mnuActionsInsurance = new System.Windows.Forms.MenuItem();
            this.mnuActionsInsuranceHealth = new System.Windows.Forms.MenuItem();
            this.mnuActionsInsuranceRenters = new System.Windows.Forms.MenuItem();
            this.mnuActionsInsuranceHomeowners = new System.Windows.Forms.MenuItem();
            this.mnuActionsInsuranceAuto = new System.Windows.Forms.MenuItem();
            this.mnuActionsInvesting = new System.Windows.Forms.MenuItem();
            this.mnuActionsInvestingResearchFunds = new System.Windows.Forms.MenuItem();
            this.mnuActionsInvestingMyPortfolio = new System.Windows.Forms.MenuItem();
            this.mnuActionsInvestingRetirement = new System.Windows.Forms.MenuItem();
            this.mnuActionsLiving = new System.Windows.Forms.MenuItem();
            this.mnuActionsLivingHousing = new System.Windows.Forms.MenuItem();
            this.mnuActionsLivingTransportation = new System.Windows.Forms.MenuItem();
            this.mnuActionsLivingTimeMgt = new System.Windows.Forms.MenuItem();
            this.mnuActionsLivingCondo = new System.Windows.Forms.MenuItem();
            this.mnuActionsCredit = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditCreditCards = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditShopForFood = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditForGoods = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditShopForCar = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditSellCar = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditShopForGas = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.mnuActionsShopBusTokens = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.mnuActionsCreditInternet = new System.Windows.Forms.MenuItem();
            this.mnuReportsHealth = new System.Windows.Forms.MenuItem();
            this.menuItem50 = new System.Windows.Forms.MenuItem();
            this.menuItem51 = new System.Windows.Forms.MenuItem();
            this.menuItem52 = new System.Windows.Forms.MenuItem();
            this.menuItem53 = new System.Windows.Forms.MenuItem();
            this.menuItem54 = new System.Windows.Forms.MenuItem();
            this.mnuReportsInvestmentStatements = new System.Windows.Forms.MenuItem();
            this.menuItem56 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.toolBarButton9 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton13 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton12 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton14 = new System.Windows.Forms.ToolBarButton();
            this.mnuLoanStatements = new System.Windows.Forms.MenuItem();
            this.mnuReportsPayAndTaxRecords = new System.Windows.Forms.MenuItem();
            this.mnuReportsCheckbook = new System.Windows.Forms.MenuItem();
            this.mnuReportsResume = new System.Windows.Forms.MenuItem();
            this.mnuReportsSnapshot = new System.Windows.Forms.MenuItem();
            this.toolBarButton15 = new System.Windows.Forms.ToolBarButton();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.mnuReportsRetirementStatements = new System.Windows.Forms.MenuItem();
            this.mnuReportsCreditReport = new System.Windows.Forms.MenuItem();
            this.toolBarButton16 = new System.Windows.Forms.ToolBarButton();
            this.toolBarButton17 = new System.Windows.Forms.ToolBarButton();
            this.mnuOptionsLegend = new System.Windows.Forms.MenuItem();
            this.mnuOptionsProvideFood = new System.Windows.Forms.MenuItem();
            this.HELP = new System.Windows.Forms.ToolBarButton();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DayOfWeekPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourceNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourcePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewMessagesPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpacerPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // CurrentEntityNamePanel
            // 
            this.CurrentEntityNamePanel.Text = "";
            this.CurrentEntityNamePanel.Width = 10;
            // 
            // mnuActions
            // 
            this.mnuActions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsLiving,
            this.mnuActionsIncome,
            this.mnuActionsMM,
            this.mnuActionsCredit,
            this.mnuActionsInsurance,
            this.mnuActionsInvesting});
            // 
            // mnuOptions
            // 
            this.mnuOptions.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOptionsLegend,
            this.mnuOptionsProvideFood});
            // 
            // mnuOptionsIA
            // 
            this.mnuOptionsIA.Index = 15;
            // 
            // mnuOptionsShowMessages
            // 
            this.mnuOptionsShowMessages.Index = 7;
            // 
            // mnuReports
            // 
            this.mnuReports.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuReportsSnapshot,
            this.mnuReportsPersonalBalanceSheet,
            this.mnuReportsHealth,
            this.menuItem9,
            this.mnuReportsResume,
            this.menuItem4,
            this.mnuReportsCreditScore,
            this.mnuReportsCreditReport,
            this.menuItem53,
            this.menuItem51,
            this.menuItem52,
            this.mnuLoanStatements,
            this.mnuReportsInvestmentStatements,
            this.mnuReportsRetirementStatements,
            this.menuItem56,
            this.mnuReportsCheckbook,
            this.menuItem54,
            this.mnuReportsPayAndTaxRecords,
            this.menuItem50,
            this.menuItem2,
            this.mnuReportsActionsJournal});
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(209, 25);
            this.picMain.Size = new System.Drawing.Size(374, 211);
            // 
            // SpacerPanel
            // 
            this.SpacerPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
            this.SpacerPanel.Width = 266;
            // 
            // staMain
            // 
            this.staMain.Location = new System.Drawing.Point(0, 315);
            this.staMain.Size = new System.Drawing.Size(792, 20);
            // 
            // tlbMain
            // 
            this.tlbMain.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButton1,
            this.toolBarButton2,
            this.toolBarButton3,
            this.toolBarButton7,
            this.toolBarButton8,
            this.toolBarButton14,
            this.toolBarButton12,
            this.toolBarButton15,
            this.toolBarButton9,
            this.toolBarButton13,
            this.toolBarButton10,
            this.toolBarButton17,
            this.toolBarButton16,
            this.toolBarButton11,
            this.scoreboardButton,
            this.HELP});
            this.tlbMain.Size = new System.Drawing.Size(792, 53);
            // 
            // toolBarButton1
            // 
            this.toolBarButton1.ImageIndex = 0;
            this.toolBarButton1.Name = "toolBarButton1";
            this.toolBarButton1.Text = "Go";
            // 
            // toolBarButton2
            // 
            this.toolBarButton2.ImageIndex = 2;
            this.toolBarButton2.Name = "toolBarButton2";
            this.toolBarButton2.Text = "Faster";
            // 
            // toolBarButton3
            // 
            this.toolBarButton3.ImageIndex = 3;
            this.toolBarButton3.Name = "toolBarButton3";
            this.toolBarButton3.Text = "Slower";
            // 
            // toolBarButton7
            // 
            this.toolBarButton7.Name = "toolBarButton7";
            this.toolBarButton7.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton8
            // 
            this.toolBarButton8.ImageIndex = 4;
            this.toolBarButton8.Name = "toolBarButton8";
            this.toolBarButton8.Text = "Vital Signs";
            this.toolBarButton8.Visible = false;
            // 
            // toolBarButton10
            // 
            this.toolBarButton10.Name = "toolBarButton10";
            this.toolBarButton10.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton11
            // 
            this.toolBarButton11.ImageIndex = 4;
            this.toolBarButton11.Name = "toolBarButton11";
            this.toolBarButton11.Text = "Assignment";
            // 
            // mnuReportsActionsJournal
            // 
            this.mnuReportsActionsJournal.Index = 20;
            this.mnuReportsActionsJournal.Text = "&Actions Journal...";
            this.mnuReportsActionsJournal.Click += new System.EventHandler(this.mnuReportsActionsJournal_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 19;
            this.menuItem2.Text = "-";
            // 
            // scoreboardButton
            // 
            this.scoreboardButton.ImageIndex = 5;
            this.scoreboardButton.Name = "scoreboardButton";
            this.scoreboardButton.Text = "Scoreboard";
            // 
            // mnuReportsPersonalBalanceSheet
            // 
            this.mnuReportsPersonalBalanceSheet.Index = 1;
            this.mnuReportsPersonalBalanceSheet.Text = "Wealth...";
            this.mnuReportsPersonalBalanceSheet.Click += new System.EventHandler(this.mnuReportsWealth_Click);
            // 
            // mnuReportsCreditScore
            // 
            this.mnuReportsCreditScore.Index = 6;
            this.mnuReportsCreditScore.Text = "Credit Score...";
            this.mnuReportsCreditScore.Click += new System.EventHandler(this.mnuReportsCreditScore_Click);
            // 
            // mnuActionsIncome
            // 
            this.mnuActionsIncome.Index = 1;
            this.mnuActionsIncome.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsIncomeWork,
            this.mnuActionsIncomeEducation,
            this.mnuActionsIncomeTaxes,
            this.menuItem8,
            this.mnuActionsIncomeWitholding,
            this.mnuActionsIncomePayment,
            this.mnuActionsIncome401K});
            this.mnuActionsIncome.Text = "Income";
            // 
            // mnuActionsIncomeWork
            // 
            this.mnuActionsIncomeWork.Index = 0;
            this.mnuActionsIncomeWork.Text = "Work";
            this.mnuActionsIncomeWork.Click += new System.EventHandler(this.mnuActionsIncomeWork_Click);
            // 
            // mnuActionsIncomeEducation
            // 
            this.mnuActionsIncomeEducation.Index = 1;
            this.mnuActionsIncomeEducation.Text = "Education";
            this.mnuActionsIncomeEducation.Click += new System.EventHandler(this.mnuActionsIncomeEducation_Click);
            // 
            // mnuActionsIncomeTaxes
            // 
            this.mnuActionsIncomeTaxes.Index = 2;
            this.mnuActionsIncomeTaxes.Text = "Taxes";
            this.mnuActionsIncomeTaxes.Click += new System.EventHandler(this.mnuActionsTaxes_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 3;
            this.menuItem8.Text = "-";
            // 
            // mnuActionsIncomeWitholding
            // 
            this.mnuActionsIncomeWitholding.Index = 4;
            this.mnuActionsIncomeWitholding.Text = "Change Witholding";
            this.mnuActionsIncomeWitholding.Click += new System.EventHandler(this.mnuActionsIncomeWitholding_Click);
            // 
            // mnuActionsIncomePayment
            // 
            this.mnuActionsIncomePayment.Index = 5;
            this.mnuActionsIncomePayment.Text = "Change Method Of Payment";
            this.mnuActionsIncomePayment.Click += new System.EventHandler(this.mnuActionsIncomePayment_Click);
            // 
            // mnuActionsIncome401K
            // 
            this.mnuActionsIncome401K.Index = 6;
            this.mnuActionsIncome401K.Text = "Change Retirement Contribution";
            this.mnuActionsIncome401K.Click += new System.EventHandler(this.mnuActionsIncome401K_Click);
            // 
            // mnuActionsMM
            // 
            this.mnuActionsMM.Index = 2;
            this.mnuActionsMM.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsMMBills,
            this.mnuActionsMMBanking,
            this.mnuActionsMMOnlineBanking});
            this.mnuActionsMM.Text = "Money Management";
            // 
            // mnuActionsMMBills
            // 
            this.mnuActionsMMBills.Index = 0;
            this.mnuActionsMMBills.Text = "Pay Bills";
            this.mnuActionsMMBills.Click += new System.EventHandler(this.mnuActionsMMBillPaying_Click);
            // 
            // mnuActionsMMBanking
            // 
            this.mnuActionsMMBanking.Index = 1;
            this.mnuActionsMMBanking.Text = "Banking";
            this.mnuActionsMMBanking.Click += new System.EventHandler(this.mnuActionsMMBanking_Click);
            // 
            // mnuActionsMMOnlineBanking
            // 
            this.mnuActionsMMOnlineBanking.Index = 2;
            this.mnuActionsMMOnlineBanking.Text = "Online Banking";
            this.mnuActionsMMOnlineBanking.Click += new System.EventHandler(this.mnuActionsOnlineBanking_Click);
            // 
            // mnuActionsInsurance
            // 
            this.mnuActionsInsurance.Index = 4;
            this.mnuActionsInsurance.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsInsuranceHealth,
            this.mnuActionsInsuranceRenters,
            this.mnuActionsInsuranceHomeowners,
            this.mnuActionsInsuranceAuto});
            this.mnuActionsInsurance.Text = "Insurance";
            // 
            // mnuActionsInsuranceHealth
            // 
            this.mnuActionsInsuranceHealth.Index = 0;
            this.mnuActionsInsuranceHealth.Text = "Healthcare";
            this.mnuActionsInsuranceHealth.Click += new System.EventHandler(this.mnuActionsInsuranceHealth_Click);
            // 
            // mnuActionsInsuranceRenters
            // 
            this.mnuActionsInsuranceRenters.Index = 1;
            this.mnuActionsInsuranceRenters.Text = "Renters";
            this.mnuActionsInsuranceRenters.Click += new System.EventHandler(this.mnuActionsInsuranceRenters_Click);
            // 
            // mnuActionsInsuranceHomeowners
            // 
            this.mnuActionsInsuranceHomeowners.Index = 2;
            this.mnuActionsInsuranceHomeowners.Text = "Homeowners";
            this.mnuActionsInsuranceHomeowners.Click += new System.EventHandler(this.mnuActionsInsuranceHomeowners_Click);
            // 
            // mnuActionsInsuranceAuto
            // 
            this.mnuActionsInsuranceAuto.Index = 3;
            this.mnuActionsInsuranceAuto.Text = "Automobile";
            this.mnuActionsInsuranceAuto.Click += new System.EventHandler(this.mnuActionsAutoInsurance_Click);
            // 
            // mnuActionsInvesting
            // 
            this.mnuActionsInvesting.Index = 5;
            this.mnuActionsInvesting.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsInvestingResearchFunds,
            this.mnuActionsInvestingMyPortfolio,
            this.mnuActionsInvestingRetirement});
            this.mnuActionsInvesting.Text = "Investing";
            // 
            // mnuActionsInvestingResearchFunds
            // 
            this.mnuActionsInvestingResearchFunds.Index = 0;
            this.mnuActionsInvestingResearchFunds.Text = "Research Funds...";
            this.mnuActionsInvestingResearchFunds.Click += new System.EventHandler(this.mnuActionsInvestingResearchFunds_Click);
            // 
            // mnuActionsInvestingMyPortfolio
            // 
            this.mnuActionsInvestingMyPortfolio.Index = 1;
            this.mnuActionsInvestingMyPortfolio.Text = "View Portfolio...";
            this.mnuActionsInvestingMyPortfolio.Click += new System.EventHandler(this.mnuActionsInvestingMyPortfolio_Click);
            // 
            // mnuActionsInvestingRetirement
            // 
            this.mnuActionsInvestingRetirement.Index = 2;
            this.mnuActionsInvestingRetirement.Text = "View Retirement Portfolio...";
            this.mnuActionsInvestingRetirement.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // mnuActionsLiving
            // 
            this.mnuActionsLiving.Index = 0;
            this.mnuActionsLiving.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsLivingHousing,
            this.mnuActionsLivingTransportation,
            this.mnuActionsLivingTimeMgt,
            this.mnuActionsLivingCondo});
            this.mnuActionsLiving.Text = "Living";
            // 
            // mnuActionsLivingHousing
            // 
            this.mnuActionsLivingHousing.Index = 0;
            this.mnuActionsLivingHousing.Text = "Apartments For Rent";
            this.mnuActionsLivingHousing.Click += new System.EventHandler(this.mnuActionsLivingHousing_Click);
            // 
            // mnuActionsLivingTransportation
            // 
            this.mnuActionsLivingTransportation.Index = 1;
            this.mnuActionsLivingTransportation.Text = "Transportation";
            this.mnuActionsLivingTransportation.Click += new System.EventHandler(this.mnuActionsLivingTransportation_Click);
            // 
            // mnuActionsLivingTimeMgt
            // 
            this.mnuActionsLivingTimeMgt.Index = 2;
            this.mnuActionsLivingTimeMgt.Text = "Schedule";
            this.mnuActionsLivingTimeMgt.Click += new System.EventHandler(this.mnuActionsLivingTimeMgt_Click);
            // 
            // mnuActionsLivingCondo
            // 
            this.mnuActionsLivingCondo.Index = 3;
            this.mnuActionsLivingCondo.Text = "Condos For Sale";
            this.mnuActionsLivingCondo.Click += new System.EventHandler(this.mnuActionsLivingCondo_Click);
            // 
            // mnuActionsCredit
            // 
            this.mnuActionsCredit.Index = 3;
            this.mnuActionsCredit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuActionsCreditCreditCards,
            this.menuItem1,
            this.mnuActionsCreditShopForFood,
            this.mnuActionsCreditForGoods,
            this.menuItem5,
            this.mnuActionsCreditShopForCar,
            this.mnuActionsCreditSellCar,
            this.mnuActionsCreditShopForGas,
            this.menuItem7,
            this.mnuActionsShopBusTokens,
            this.menuItem6,
            this.mnuActionsCreditInternet});
            this.mnuActionsCredit.Text = "Spending, Credit && Debt";
            // 
            // mnuActionsCreditCreditCards
            // 
            this.mnuActionsCreditCreditCards.Index = 0;
            this.mnuActionsCreditCreditCards.Text = "Credit Cards";
            this.mnuActionsCreditCreditCards.Click += new System.EventHandler(this.mnuActionsCreditCreditCards_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Enabled = false;
            this.menuItem1.Index = 1;
            this.menuItem1.Text = "-";
            // 
            // mnuActionsCreditShopForFood
            // 
            this.mnuActionsCreditShopForFood.Index = 2;
            this.mnuActionsCreditShopForFood.Text = "Shop For Food";
            this.mnuActionsCreditShopForFood.Click += new System.EventHandler(this.mnuActionsCreditShopForFood_Click);
            // 
            // mnuActionsCreditForGoods
            // 
            this.mnuActionsCreditForGoods.Index = 3;
            this.mnuActionsCreditForGoods.Text = "Shop For Goods";
            this.mnuActionsCreditForGoods.Click += new System.EventHandler(this.mnuActionsCreditForGoods_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Enabled = false;
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "-";
            // 
            // mnuActionsCreditShopForCar
            // 
            this.mnuActionsCreditShopForCar.Index = 5;
            this.mnuActionsCreditShopForCar.Text = "Shop For Car";
            this.mnuActionsCreditShopForCar.Click += new System.EventHandler(this.mnuActionsCreditShopForCar_Click);
            // 
            // mnuActionsCreditSellCar
            // 
            this.mnuActionsCreditSellCar.Index = 6;
            this.mnuActionsCreditSellCar.Text = "Sell Car";
            this.mnuActionsCreditSellCar.Click += new System.EventHandler(this.mnuActionsCreditSellCar_Click);
            // 
            // mnuActionsCreditShopForGas
            // 
            this.mnuActionsCreditShopForGas.Index = 7;
            this.mnuActionsCreditShopForGas.Text = "Shop For Gas && Repairs";
            this.mnuActionsCreditShopForGas.Click += new System.EventHandler(this.mnuActionsCreditShopForGas_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 8;
            this.menuItem7.Text = "-";
            // 
            // mnuActionsShopBusTokens
            // 
            this.mnuActionsShopBusTokens.Index = 9;
            this.mnuActionsShopBusTokens.Text = "Buy Bus Tokens";
            this.mnuActionsShopBusTokens.Click += new System.EventHandler(this.mnuActionsShopBusTokens_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Enabled = false;
            this.menuItem6.Index = 10;
            this.menuItem6.Text = "-";
            // 
            // mnuActionsCreditInternet
            // 
            this.mnuActionsCreditInternet.Index = 11;
            this.mnuActionsCreditInternet.Text = "Internet Access";
            this.mnuActionsCreditInternet.Click += new System.EventHandler(this.mnuActionsCreditInternet_Click);
            // 
            // mnuReportsHealth
            // 
            this.mnuReportsHealth.Index = 2;
            this.mnuReportsHealth.Text = "Health...";
            this.mnuReportsHealth.Click += new System.EventHandler(this.mnuReportsHealth_Click);
            // 
            // menuItem50
            // 
            this.menuItem50.Index = 18;
            this.menuItem50.Text = "Past Tax Returns...";
            this.menuItem50.Click += new System.EventHandler(this.mnuReportsTaxes_Click);
            // 
            // menuItem51
            // 
            this.menuItem51.Index = 9;
            this.menuItem51.Text = "Bank Statements...";
            this.menuItem51.Click += new System.EventHandler(this.mnuReportsBankStatements_Click);
            // 
            // menuItem52
            // 
            this.menuItem52.Index = 10;
            this.menuItem52.Text = "Credit Card Statements...";
            this.menuItem52.Click += new System.EventHandler(this.mnuReportsCreditCardStatements_Click);
            // 
            // menuItem53
            // 
            this.menuItem53.Index = 8;
            this.menuItem53.Text = "-";
            // 
            // menuItem54
            // 
            this.menuItem54.Index = 16;
            this.menuItem54.Text = "-";
            // 
            // mnuReportsInvestmentStatements
            // 
            this.mnuReportsInvestmentStatements.Index = 12;
            this.mnuReportsInvestmentStatements.Text = "Investment Statements...";
            this.mnuReportsInvestmentStatements.Click += new System.EventHandler(this.mnuReportsInvestmentStatements_Click);
            // 
            // menuItem56
            // 
            this.menuItem56.Index = 14;
            this.menuItem56.Text = "-";
            this.menuItem56.Visible = false;
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 5;
            this.menuItem4.Text = "-";
            // 
            // toolBarButton9
            // 
            this.toolBarButton9.ImageIndex = 6;
            this.toolBarButton9.Name = "toolBarButton9";
            this.toolBarButton9.Text = "Wealth";
            // 
            // toolBarButton13
            // 
            this.toolBarButton13.ImageIndex = 7;
            this.toolBarButton13.Name = "toolBarButton13";
            this.toolBarButton13.Text = "Health";
            // 
            // toolBarButton12
            // 
            this.toolBarButton12.Name = "toolBarButton12";
            this.toolBarButton12.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton14
            // 
            this.toolBarButton14.ImageIndex = 8;
            this.toolBarButton14.Name = "toolBarButton14";
            this.toolBarButton14.Text = "Schedule";
            // 
            // mnuLoanStatements
            // 
            this.mnuLoanStatements.Index = 11;
            this.mnuLoanStatements.Text = "Loan Statements...";
            this.mnuLoanStatements.Click += new System.EventHandler(this.mnuLoanStatements_Click);
            // 
            // mnuReportsPayAndTaxRecords
            // 
            this.mnuReportsPayAndTaxRecords.Index = 17;
            this.mnuReportsPayAndTaxRecords.Text = "Pay && Tax Records...";
            this.mnuReportsPayAndTaxRecords.Click += new System.EventHandler(this.mnuReportsPayAndTaxRecords_Click);
            // 
            // mnuReportsCheckbook
            // 
            this.mnuReportsCheckbook.Index = 15;
            this.mnuReportsCheckbook.Text = "Check Register...";
            this.mnuReportsCheckbook.Click += new System.EventHandler(this.mnuReportsCheckbook_Click);
            // 
            // mnuReportsResume
            // 
            this.mnuReportsResume.Index = 4;
            this.mnuReportsResume.Text = "Resume...";
            this.mnuReportsResume.Click += new System.EventHandler(this.mnuReportsResume_Click);
            // 
            // mnuReportsSnapshot
            // 
            this.mnuReportsSnapshot.Index = 0;
            this.mnuReportsSnapshot.Text = "Snapshot...";
            this.mnuReportsSnapshot.Click += new System.EventHandler(this.mnuReportsSnapshot_Click);
            // 
            // toolBarButton15
            // 
            this.toolBarButton15.ImageIndex = 10;
            this.toolBarButton15.Name = "toolBarButton15";
            this.toolBarButton15.Text = "Snapshot";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 3;
            this.menuItem9.Text = "-";
            // 
            // mnuReportsRetirementStatements
            // 
            this.mnuReportsRetirementStatements.Index = 13;
            this.mnuReportsRetirementStatements.Text = "Retirement Statements...";
            this.mnuReportsRetirementStatements.Click += new System.EventHandler(this.mnuReportsRetirementStatements_Click);
            // 
            // mnuReportsCreditReport
            // 
            this.mnuReportsCreditReport.Index = 7;
            this.mnuReportsCreditReport.Text = "Credit Report...";
            this.mnuReportsCreditReport.Click += new System.EventHandler(this.frmReportsCreditReport_Click);
            // 
            // toolBarButton16
            // 
            this.toolBarButton16.Name = "toolBarButton16";
            this.toolBarButton16.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // toolBarButton17
            // 
            this.toolBarButton17.ImageIndex = 9;
            this.toolBarButton17.Name = "toolBarButton17";
            this.toolBarButton17.Text = "Legend";
            // 
            // mnuOptionsLegend
            // 
            this.mnuOptionsLegend.Index = 17;
            this.mnuOptionsLegend.Text = "Legend...";
            this.mnuOptionsLegend.Click += new System.EventHandler(this.mnuOptionsLegend_Click);
            // 
            // mnuOptionsProvideFood
            // 
            this.mnuOptionsProvideFood.Index = 18;
            this.mnuOptionsProvideFood.Text = "Provide Food";
            this.mnuOptionsProvideFood.Visible = false;
            this.mnuOptionsProvideFood.Click += new System.EventHandler(this.mnuOptionsProvideFood_Click);
            // 
            // HELP
            // 
            this.HELP.Name = "HELP";
            this.HELP.Text = "Support";
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(792, 335);
            this.Name = "frmMain";
            this.Text = "";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMain_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityNamePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentEntityPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DatePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DayOfWeekPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourceNamePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EntityCriticalResourcePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NewMessagesPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpacerPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimePanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void LoadBaseImages()
        {
            this.resources = new ComponentResourceManager(typeof(frmMain));
            base.Icon = (Icon) this.resources.GetObject("$this.Icon");
            base.ilsMainToolBar.ImageStream = (ImageListStreamer) this.resources.GetObject("ilsMainToolBar.ImageStream");
            for (int i = 0; i != 11; i++) {
                base.ilsMainToolBar.Images.SetKeyName(i, "");
                Image img = base.ilsMainToolBar.Images[i];
                img.Save("ToolBar" + i + ".bmp");
            }
        }

        protected override void LoadStateHook()
        {
            base.LoadStateHook();
            A.Instance.Views[1].ViewerOptions = A.State.ViewerOptions1;
            A.State.UpdateCarAndGasData();
            foreach (AppEntity entity in A.State.Entity.Values)
            {
                if (entity.Reserved == null)
                {
                    entity.SetUpReserved();
                }
            }
            A.State.God.fixIR();
            if (A.State.RunToDate == DateTime.MinValue)
            {
                A.State.RunToDate = DateTime.MaxValue;
            }
            if (A.Settings.BlockMessagesContaining == null)
            {
                A.Settings.BlockMessagesContaining = "";
            }
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            try
            {
                frmMyPortfolio frm = new frmMyPortfolio(true) {
                    EnablingReference = this.mnuActionsInvestingMyPortfolio
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void MessageBoxWithIcon(string message, Bitmap icon)
        {
            new frmMessageBoxWithIcon(message, icon).ShowDialog(this);
        }

        private void mnuActionsAutoInsurance_Click(object sender, EventArgs e)
        {
            try
            {
                new frmAutoInsurance().ShowDialog();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsCreditCreditCards_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To get a credit card or close a credit account, switch to the City view, mouse-over the banks, and click for a menu of options.", A.Resources.GetImage("Building8"));
        }

        private void mnuActionsCreditForGoods_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To shop for goods, click one or more of the department stores and compare prices.", A.Resources.GetImage("Building12"));
        }

        private void mnuActionsCreditInternet_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To subscribe or unsubscribe to Internet access,  a credit account, mouse-over the internet access provider, and click for a menu of options.", A.Resources.GetImage("Building14"));
        }

        private void mnuActionsCreditSellCar_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show(A.Adapter.GetCarPrice(A.MainForm.CurrentEntityID), A.Resources.GetString("Confirm Sale"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    A.Adapter.SellCar(A.MainForm.CurrentEntityID);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsCreditShopForCar_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList carShop = A.Adapter.GetCarShop(A.MainForm.CurrentEntityID);
                new frmShop(A.Resources.GetString("Taranti Auto"), carShop, true).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsCreditShopForFood_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList shopFood = A.Adapter.GetShopFood(A.MainForm.CurrentEntityID);
                new frmShop(A.Resources.GetString("SuperMarket, Inc."), shopFood, false).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsCreditShopForGas_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList shopAutoRepair = A.Adapter.GetShopAutoRepair(A.MainForm.CurrentEntityID);
                new frmShop(A.Resources.GetString("Gas & Repairs, Inc."), shopAutoRepair, false).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsIncome401K_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change your retirement contribution, click on the Schedule button, click the job, then click 401K.", "Change 401K Contribution");
        }

        private void mnuActionsIncomeEducation_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To find courses, switch to the City view, mouse-over the university buildings, and click to enroll.", A.Resources.GetImage("Building5"));
        }

        private void mnuActionsIncomePayment_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change your method of payment, click on the Schedule button, click the job, then click Payment.", "Change Method of Payment");
        }

        private void mnuActionsIncomeWitholding_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To change your witholding, click on the Schedule button, click the job, then click Witholding.", "Change Witholding");
        }

        private void mnuActionsIncomeWork_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To find jobs, switch to the City view, mouse-over possible workplaces, and click to apply.", A.Resources.GetImage("Building2"));
        }

        private void mnuActionsInsuranceHealth_Click(object sender, EventArgs e)
        {
            try
            {
                new frmHealthInsurance().ShowDialog();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsInsuranceHomeowners_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To change your homeowner's insurance on a condo you own, switch to the City view and click on the condo.", A.Resources.GetImage("Building1"));
        }

        private void mnuActionsInsuranceRenters_Click(object sender, EventArgs e)
        {
            try
            {
                new frmRentersInsurance().ShowDialog();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsInvestingMyPortfolio_Click(object sender, EventArgs e)
        {
            try
            {
                frmMyPortfolio frm = new frmMyPortfolio(false) {
                    EnablingReference = this.mnuActionsInvestingMyPortfolio
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void mnuActionsInvestingResearchFunds_Click(object sender, EventArgs e)
        {
            try
            {
                frmResearchFunds frm = new frmResearchFunds(sender) {
                    EnablingReference = this.mnuActionsInvestingResearchFunds
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsLivingCondo_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To select housing, switch to the City view, mouse-over the apartment/condo buildings, and click for info on a buying condo.", A.Resources.GetImage("Building1"));
        }

        private void mnuActionsLivingHousing_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("To select housing, switch to the City view, mouse-over the apartment buildings, and click to rent.", A.Resources.GetImage("Building1"));
        }

        private void mnuActionsLivingTimeMgt_Click(object sender, EventArgs e)
        {
            Form errantForm = null;
            try
            {
                errantForm = new frmDailyRoutine();
                errantForm.ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuActionsLivingTransportation_Click(object sender, EventArgs e)
        {
            frmTransportation errantForm = null;
            try
            {
                errantForm = new frmTransportation();
                errantForm.ShowDialog();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuActionsMMBanking_Click(object sender, EventArgs e)
        {
            base.OnViewChange(A.Instance.Views[0].Name);
            this.MessageBoxWithIcon("For all banking functions, switch to the City view, mouse-over the banks, and click for a menu of options.", A.Resources.GetImage("Building8"));
        }

        private void mnuActionsMMBillPaying_Click(object sender, EventArgs e)
        {
            try
            {
                new frmPayBills().ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsOnlineBanking_Click(object sender, EventArgs e)
        {
            frmOnlineBanking errantForm = null;
            try
            {
                errantForm = new frmOnlineBanking();
                errantForm.ShowDialog();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuActionsShopBusTokens_Click(object sender, EventArgs e)
        {
            try
            {
                ArrayList shopBusTokens = A.Adapter.GetShopBusTokens(A.MainForm.CurrentEntityID);
                new frmShop(A.Resources.GetString("City Bus"), shopBusTokens, false).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuActionsTaxes_Click(object sender, EventArgs e)
        {
            frmTaxes errantForm = null;
            try
            {
                errantForm = new frmTaxes(frmTaxes.Mode.Current);
                string text = errantForm.CanUse();
                if (text == "")
                {
                    errantForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show(text, A.Resources.GetString("Taxes"));
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuLoanStatements_Click(object sender, EventArgs e)
        {
            try
            {
                new frmLoanStatement().ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public override void mnuOptionsBackgroundMusic_Click(object sender, EventArgs e)
        {
            base.mnuOptionsBackgroundMusic_Click(sender, e);
            Settings.SetValue<bool>("NO_BGM", !base.mnuOptionsBackgroundMusic.Checked);
        }

        protected override void mnuOptionsIAProvideCash_Click(object sender, EventArgs e)
        {
            frmPassword password = new frmPassword(S.Instance.UserAdminSettings.GetP());
            if (base.DesignerMode || (password.ShowDialog(this) == DialogResult.OK))
            {
                try
                {
                    new frmAppProvideCash().ShowDialog(this);
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }

        private void mnuOptionsLegend_Click(object sender, EventArgs e)
        {
            try
            {
                frmLegend legend = new frmLegend();
                legend.Location = new Point((base.Bounds.Right - legend.Width) - 6, base.Bounds.Top + 100);
                base.ShowNonModalForm(legend);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuOptionsProvideFood_Click(object sender, EventArgs e) {
            ((AppEntity)A.State.Entity[A.MainForm.currentEntityID]).Food.Add(
                new FoodEntry(5000, A.State.Now, 28, false));
        }

        private void mnuReportsActionsJournal_Click(object sender, EventArgs e)
        {
            frmActionsJournal frm = new frmActionsJournal(true) {
                EnablingReference = this.mnuReportsActionsJournal
            };
            base.ShowNonModalForm(frm);
        }

        private void mnuReportsBankStatements_Click(object sender, EventArgs e)
        {
            try
            {
                new frmBankStatement().ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsCheckbook_Click(object sender, EventArgs e)
        {
            Form errantForm = null;
            try
            {
                errantForm = new frmCheckbook();
                errantForm.ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuReportsCreditCardStatements_Click(object sender, EventArgs e)
        {
            try
            {
                new frmCreditCardStatement().ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsCreditScore_Click(object sender, EventArgs e)
        {
            try
            {
                frmCreditScore frm = new frmCreditScore {
                    EnablingReference = this.mnuReportsCreditScore
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsHealth_Click(object sender, EventArgs e)
        {
            try
            {
                frmHealth2 frm = new frmHealth2 {
                    EnablingReference = this.mnuReportsHealth
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsInvestmentStatements_Click(object sender, EventArgs e)
        {
            try
            {
                new frmInvestmentStatement().ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsPayAndTaxRecords_Click(object sender, EventArgs e)
        {
            try
            {
                frmPayStubs frm = new frmPayStubs {
                    EnablingReference = this.mnuReportsPayAndTaxRecords
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsResume_Click(object sender, EventArgs e)
        {
            try
            {
                frmResume frm = new frmResume {
                    EnablingReference = this.mnuReportsResume
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsRetirementStatements_Click(object sender, EventArgs e)
        {
            try
            {
                new frmRetirementStatement().ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsSnapshot_Click(object sender, EventArgs e)
        {
            try
            {
                frmSnapshot frm = new frmSnapshot {
                    EnablingReference = this.mnuReportsSnapshot
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void mnuReportsTaxes_Click(object sender, EventArgs e)
        {
            frmTaxes errantForm = null;
            try
            {
                errantForm = new frmTaxes(frmTaxes.Mode.Past);
                string text = errantForm.CanUse();
                if (text == "")
                {
                    errantForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show(text, A.Resources.GetString("Taxes"));
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, errantForm);
            }
        }

        private void mnuReportsWealth_Click(object sender, EventArgs e)
        {
            try
            {
                frmPersonalBalanceSheet frm = new frmPersonalBalanceSheet {
                    EnablingReference = this.mnuReportsPersonalBalanceSheet
                };
                base.ShowNonModalForm(frm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        protected override void OnStateChangedHook()
        {
            base.OnStateChangedHook();
            A.Instance.Views[0].ViewerOptions = new object[] { 0, 0, PointF.Empty, A.Instance.ThisPlayerName };
        }

        public override void OpenSavedSim(string filepath)
        {
            base.OpenSavedSim(filepath);
            this.DoSimUpgrade();
        }
    }
}

