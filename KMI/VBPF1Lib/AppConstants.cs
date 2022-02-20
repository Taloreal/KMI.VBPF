namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.Reflection;

    public class AppConstants
    {
        public const int BackEndLoadYears = 5;
        public const int BadCheckFee = 0x23;
        public const int BillingDay = 0x1c;
        public static int BuildingTypeCount = 20;
        public const float CarLoanHiRate = 0.07f;
        public const float CarLoanLoRate = 0.03f;
        public const int CarLoanTerm = 0x24;
        public static Hashtable CarryAnchorPoints;
        public static MapV2 ClassMap;
        public const int ClosingCosts = 0x1004;
        public const float CreditCardHiRate = 0.08f;
        public const float CreditCardLoRate = 0.04f;
        public const int CreditHistoryMonths = 0x24;
        public const int CreditScoreEnd = 400;
        public const int CreditScoreWarn = 500;
        public const int DanceFrames = 0x1b;
        public const int DaysToRehireIfFired = 180;
        public const int DaysToRehireIfQuit = 90;
        public const int DefaultSimulatedTimePerStep = 0x4e20;
        public const int DeptStores = 3;
        public const int EatFrames = 10;
        public const string FinanceChargeDescription = "Finance Charges";
        public const int FoodExpiresAfterWeeks = 4;
        public const int Frames = 9;
        public static string[] FundCategories = new string[] { "U.S. Stocks", "Intl Stocks", "Bonds", "Money Markets" };
        public const int GenericPalettes = 6;
        public const int HairBegin = 0xc0;
        public const int HairEnd = 0xef;
        public static int[] HealthFactorApplyForwardDays;
        public static string[] HealthFactorNames;
        public static float[] HealthFactorNeededPerDay;
        public static int[] HealthFactoryHistoryDays;
        public static MapV2 HomeMap;
        public const int IrateFrames = 30;
        public const int JumpingJackFrames = 9;
        public const int MaxAbsencesPerMonth = 4;
        public const int MaxCustomers = 20;
        public const int MaxDaysCheckInMail = 14;
        public const int MaxDaysTillBreakdown = 0x16d;
        public const float MaxFICAPerYear = 5840.4f;
        public const int MaxLatesPerMonth = 4;
        public const float MaxRent = 1200f;
        public const int MaxStudents = 8;
        public const float MinCarDownPayment = 0.1f;
        public const float MinCreditCardPayment = 0.01f;
        public const int MinCreditScoreCarLoan = 510;
        public const int MinCreditScoreCreditCard = 0;
        public const int MinCreditScoreMortgage = 610;
        public const int MinCreditScoreStudentLoan = 0;
        public const int MinDaysCheckInMail = 2;
        public const int MinDaysTillBreakdown = 120;
        public const float MinMortgageDownPayment = 0f;
        public const float MinRent = 200f;
        public const float MortgageHiRate = 0.11f;
        public const float MortgageLoRate = 0.03f;
        public static float[] MPGs = new float[] { 25f, 29f, 21f, 16f, 11f, 15f };
        public const int Palettes = 0x12;
        public const int PantsBegin = 0x30;
        public const int PantsEnd = 0x5f;
        public const string PaymentDescription = "Payment-Thank You!";
        public const int PeriodsPerDay = 0x30;
        public const float RealEstateCommission = 0.05f;
        public const float ReimbursementPerStep = 0.005f;
        public const int ShirtBegin = 0x60;
        public const int ShirtEnd = 0x8f;
        public const int ShoesBegin = 0x90;
        public const int ShoesEnd = 0xbf;
        public const int SkinBegin = 0;
        public const int SkinEnd = 0x2f;
        public const float StudentLoanHiRate = 0.06f;
        public const float StudentLoanLoRate = 0.01f;
        public const int StudentLoanTerm = 60;
        public const int TakeOrderFrames = 0x13;
        public const int TeacherFrames = 0x1c;
        public const int TypeFrames = 9;
        public static MapV2 Work0Map;
        public static MapV2 Work1Map;

        static AppConstants()
        {
            Assembly assembly = typeof(AppConstants).Assembly;
            HomeMap = new MapV2();
            HomeMap.load(assembly, "KMI.VBPF1Lib.Data.HomeMap.xml");
            Work0Map = new MapV2();
            Work0Map.load(assembly, "KMI.VBPF1Lib.Data.Work0Map.xml");
            Work1Map = new MapV2();
            Work1Map.load(assembly, "KMI.VBPF1Lib.Data.Work1Map.xml");
            ClassMap = new MapV2();
            ClassMap.load(assembly, "KMI.VBPF1Lib.Data.ClassMap.xml");
            HealthFactorNames = new string[] { "Nutrition", "Sleep", "Exercise", "Relaxation", "Social Life" };
            HealthFactoryHistoryDays = new int[] { 14, 7, 7, 7, 1 };
            HealthFactorNeededPerDay = new float[] { 2f, 16f, 2f, 2f, 0f };
            HealthFactorApplyForwardDays = new int[] { 1, 2, 2, 1, 1 };
        }

        public enum HealthFactors
        {
            Eat,
            Sleep,
            Exercise,
            Relaxation,
            SocialLife
        }
    }
}

