namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;

    [Serializable]
    public class AppSimSettings : SimSettings
    {
        protected bool actionsJournalEnabledForNonOwner = false;
        protected bool actionsJournalEnabledForOwner = true;
        protected bool aIParties = true;
        protected bool alwaysUseTaxAccountant = false;
        protected bool apartmentsForRentEnabledForNonOwner = false;
        protected bool apartmentsForRentEnabledForOwner = true;
        protected bool autofillCheckRegister = true;
        protected bool automobileEnabledForNonOwner = false;
        protected bool automobileEnabledForOwner = true;
        protected bool autoPayRentElectric = false;
        protected bool bankingEnabledForNonOwner = false;
        protected bool bankingEnabledForOwner = true;
        protected bool bankStatementsEnabledForNonOwner = false;
        protected bool bankStatementsEnabledForOwner = true;
        protected DateTime breakInDate = DateTime.MinValue;
        protected bool buyBusTokensEnabledForNonOwner = false;
        protected bool buyBusTokensEnabledForOwner = true;
        protected bool carAccidents = true;
        protected bool changeMethodOfPaymentEnabledForNonOwner = false;
        protected bool changeMethodOfPaymentEnabledForOwner = true;
        protected bool changeRetirementContributionEnabledForNonOwner = false;
        protected bool changeRetirementContributionEnabledForOwner = true;
        protected bool changeWitholdingEnabledForNonOwner = false;
        protected bool changeWitholdingEnabledForOwner = true;
        protected bool checkRegisterEnabledForNonOwner = false;
        protected bool checkRegisterEnabledForOwner = true;
        protected bool closeSomeBusinesses = false;
        protected bool condosForSaleEnabledForNonOwner = false;
        protected bool condosForSaleEnabledForOwner = false;
        protected bool creditCardsEnabledForNonOwner = false;
        protected bool creditCardsEnabledForOwner = true;
        protected bool creditCardStatementsEnabledForNonOwner = false;
        protected bool creditCardStatementsEnabledForOwner = true;
        protected bool creditReportEnabledForNonOwner = false;
        protected bool creditReportEnabledForOwner = true;
        protected bool creditScoreEnabledForNonOwner = false;
        protected bool creditScoreEnabledForOwner = true;
        protected bool disableFW4 = false;
        protected DateTime dislocatedShoulderDate = DateTime.MinValue;
        protected bool educationEnabledForNonOwner = false;
        protected bool educationEnabledForOwner = true;
        protected bool endOnLowCreditScore = true;
        protected int expectedMultiplayerPlayers = 10;
        protected bool fireForAbsencesLateness = true;
        protected bool healthAccidents = true;
        protected bool healthcareEnabledForNonOwner = false;
        protected bool healthcareEnabledForOwner = false;
        protected bool healthEnabledForNonOwner = false;
        protected bool healthEnabledForOwner = true;
        public int healthFactorsToConsider = 4;
        protected bool healthInsuranceForFastFoodJobs = false;
        protected bool homeownersEnabledForNonOwner = false;
        protected bool homeownersEnabledForOwner = false;
        protected bool includeOtherLiabilities = false;
        protected float inflationRate = 0.03f;
        public float initialCash = 5000f;
        public float initialHealth = 0.8f;
        protected bool internetAccessEnabledForNonOwner = false;
        protected bool internetAccessEnabledForOwner = false;
        protected bool investmentStatementsEnabledForNonOwner = false;
        protected bool investmentStatementsEnabledForOwner = true;
        protected bool loanStatementsEnabledForNonOwner = false;
        protected bool loanStatementsEnabledForOwner = true;
        protected bool onlineBankingEnabledForNonOwner = false;
        protected bool onlineBankingEnabledForOwner = false;
        protected bool pastTaxReturnsEnabledForNonOwner = false;
        protected bool pastTaxReturnsEnabledForOwner = true;
        protected bool payBillsEnabledForNonOwner = false;
        protected bool payBillsEnabledForOwner = true;
        protected bool payTaxRecordsEnabledForNonOwner = false;
        protected bool payTaxRecordsEnabledForOwner = true;
        protected bool rentersEnabledForNonOwner = false;
        protected bool rentersEnabledForOwner = true;
        protected bool researchFundsEnabledForNonOwner = false;
        protected bool researchFundsEnabledForOwner = false;
        protected bool resumeEnabledForNonOwner = false;
        protected bool resumeEnabledForOwner = true;
        protected bool retirementStatementsEnabledForNonOwner = false;
        protected bool retirementStatementsEnabledForOwner = true;
        protected bool robberies = false;
        protected bool sales = true;
        protected bool scheduleEnabledForNonOwner = false;
        protected bool scheduleEnabledForOwner = true;
        protected bool scheduleReadOnly = false;
        protected bool sellCarEnabledForNonOwner = false;
        protected bool sellCarEnabledForOwner = true;
        protected bool shopForCarEnabledForNonOwner = false;
        protected bool shopForCarEnabledForOwner = true;
        protected bool shopForFoodEnabledForNonOwner = false;
        protected bool shopForFoodEnabledForOwner = true;
        protected bool shopForGasRepairsEnabledForNonOwner = false;
        protected bool shopForGasRepairsEnabledForOwner = true;
        protected bool shopForGoodsEnabledForNonOwner = false;
        protected bool shopForGoodsEnabledForOwner = true;
        protected bool sickness = true;
        protected bool snapshotEnabledForNonOwner = false;
        protected bool snapshotEnabledForOwner = true;
        protected string surprises = "Health|Car Accident|Car Breakdown|Robbery|Layoff";
        protected bool taxesEnabledForNonOwner = false;
        protected bool taxesEnabledForOwner = true;
        protected bool transportationEnabledForNonOwner = false;
        protected bool transportationEnabledForOwner = true;
        protected bool turboSpeed = false;
        protected bool viewPortfolioEnabledForNonOwner = false;
        protected bool viewPortfolioEnabledForOwner = false;
        protected bool viewRetirementPortfolioEnabledForNonOwner = false;
        protected bool viewRetirementPortfolioEnabledForOwner = false;
        protected bool wealthEnabledForNonOwner = false;
        protected bool wealthEnabledForOwner = true;
        protected bool workEnabledForNonOwner = false;
        protected bool workEnabledForOwner = true;

        public AppSimSettings()
        {
            base.StartDate = new DateTime(0x7da, 1, 2, 0x17, 0x3b, 0x3b);
        }

        public override bool AllowInstructorToEdit(string propertyName)
        {
            return ((propertyName == A.Resources.GetString("AutofillCheckRegister")) || base.AllowInstructorToEdit(propertyName));
        }

        public bool ActionsJournalEnabledForNonOwner
        {
            get
            {
                return this.actionsJournalEnabledForNonOwner;
            }
            set
            {
                this.actionsJournalEnabledForNonOwner = value;
            }
        }

        public bool ActionsJournalEnabledForOwner
        {
            get
            {
                return this.actionsJournalEnabledForOwner;
            }
            set
            {
                this.actionsJournalEnabledForOwner = value;
            }
        }

        public bool AIParties
        {
            get
            {
                return this.aIParties;
            }
            set
            {
                this.aIParties = value;
            }
        }

        public bool AlwaysUseTaxAccountant
        {
            get
            {
                return this.alwaysUseTaxAccountant;
            }
            set
            {
                this.alwaysUseTaxAccountant = value;
            }
        }

        public bool ApartmentsForRentEnabledForNonOwner
        {
            get
            {
                return this.apartmentsForRentEnabledForNonOwner;
            }
            set
            {
                this.apartmentsForRentEnabledForNonOwner = value;
            }
        }

        public bool ApartmentsForRentEnabledForOwner
        {
            get
            {
                return this.apartmentsForRentEnabledForOwner;
            }
            set
            {
                this.apartmentsForRentEnabledForOwner = value;
            }
        }

        public bool AutofillCheckRegister
        {
            get
            {
                return this.autofillCheckRegister;
            }
            set
            {
                this.autofillCheckRegister = value;
            }
        }

        public bool AutomobileEnabledForNonOwner
        {
            get
            {
                return this.automobileEnabledForNonOwner;
            }
            set
            {
                this.automobileEnabledForNonOwner = value;
            }
        }

        public bool AutomobileEnabledForOwner
        {
            get
            {
                return this.automobileEnabledForOwner;
            }
            set
            {
                this.automobileEnabledForOwner = value;
            }
        }

        public bool AutoPayRentElectric
        {
            get
            {
                return this.autoPayRentElectric;
            }
            set
            {
                this.autoPayRentElectric = value;
                if (value)
                {
                    AppEntity entity = (AppEntity) A.State.Entity[A.MainForm.CurrentEntityID];
                    if ((entity != null) && (((CheckingAccount) entity.BankAccounts.GetByIndex(0)).RecurringPayments.Count == 0))
                    {
                        ((CheckingAccount) entity.BankAccounts.GetByIndex(0)).RecurringPayments.Clear();
                        RecurringPayment payment = new RecurringPayment {
                            Amount = entity.Dwelling.Rent,
                            Day = 2,
                            PayeeAccountNumber = ((BankAccount) entity.MerchantAccounts["City Property Mgt"]).AccountNumber,
                            PayeeName = ((BankAccount) entity.MerchantAccounts["City Property Mgt"]).BankName
                        };
                        ((CheckingAccount) entity.BankAccounts.GetByIndex(0)).RecurringPayments.Add(payment);
                        payment = new RecurringPayment {
                            Amount = A.State.ElectricityCost,
                            Day = 2,
                            PayeeAccountNumber = ((BankAccount) entity.MerchantAccounts["NRG Electric"]).AccountNumber,
                            PayeeName = ((BankAccount) entity.MerchantAccounts["NRG Electric"]).BankName
                        };
                        ((CheckingAccount) entity.BankAccounts.GetByIndex(0)).RecurringPayments.Add(payment);
                    }
                }
            }
        }

        public bool BankingEnabledForNonOwner
        {
            get
            {
                return this.bankingEnabledForNonOwner;
            }
            set
            {
                this.bankingEnabledForNonOwner = value;
            }
        }

        public bool BankingEnabledForOwner
        {
            get
            {
                return this.bankingEnabledForOwner;
            }
            set
            {
                this.bankingEnabledForOwner = value;
            }
        }

        public bool BankStatementsEnabledForNonOwner
        {
            get
            {
                return this.bankStatementsEnabledForNonOwner;
            }
            set
            {
                this.bankStatementsEnabledForNonOwner = value;
            }
        }

        public bool BankStatementsEnabledForOwner
        {
            get
            {
                return this.bankStatementsEnabledForOwner;
            }
            set
            {
                this.bankStatementsEnabledForOwner = value;
            }
        }

        public DateTime BreakInDate
        {
            get
            {
                return this.breakInDate;
            }
            set
            {
                this.breakInDate = value;
            }
        }

        public bool BuyBusTokensEnabledForNonOwner
        {
            get
            {
                return this.buyBusTokensEnabledForNonOwner;
            }
            set
            {
                this.buyBusTokensEnabledForNonOwner = value;
            }
        }

        public bool BuyBusTokensEnabledForOwner
        {
            get
            {
                return this.buyBusTokensEnabledForOwner;
            }
            set
            {
                this.buyBusTokensEnabledForOwner = value;
            }
        }

        public bool ChangeMethodOfPaymentEnabledForNonOwner
        {
            get
            {
                return this.changeMethodOfPaymentEnabledForNonOwner;
            }
            set
            {
                this.changeMethodOfPaymentEnabledForNonOwner = value;
            }
        }

        public bool ChangeMethodOfPaymentEnabledForOwner
        {
            get
            {
                return this.changeMethodOfPaymentEnabledForOwner;
            }
            set
            {
                this.changeMethodOfPaymentEnabledForOwner = value;
            }
        }

        public bool ChangeRetirementContributionEnabledForNonOwner
        {
            get
            {
                return this.changeRetirementContributionEnabledForNonOwner;
            }
            set
            {
                this.changeRetirementContributionEnabledForNonOwner = value;
            }
        }

        public bool ChangeRetirementContributionEnabledForOwner
        {
            get
            {
                return this.changeRetirementContributionEnabledForOwner;
            }
            set
            {
                this.changeRetirementContributionEnabledForOwner = value;
            }
        }

        public bool ChangeWitholdingEnabledForNonOwner
        {
            get
            {
                return this.changeWitholdingEnabledForNonOwner;
            }
            set
            {
                this.changeWitholdingEnabledForNonOwner = value;
            }
        }

        public bool ChangeWitholdingEnabledForOwner
        {
            get
            {
                return this.changeWitholdingEnabledForOwner;
            }
            set
            {
                this.changeWitholdingEnabledForOwner = value;
            }
        }

        public bool CheckRegisterEnabledForNonOwner
        {
            get
            {
                return this.checkRegisterEnabledForNonOwner;
            }
            set
            {
                this.checkRegisterEnabledForNonOwner = value;
            }
        }

        public bool CheckRegisterEnabledForOwner
        {
            get
            {
                return this.checkRegisterEnabledForOwner;
            }
            set
            {
                this.checkRegisterEnabledForOwner = value;
            }
        }

        public bool CloseSomeBusinesses
        {
            get
            {
                return this.closeSomeBusinesses;
            }
            set
            {
                this.closeSomeBusinesses = value;
            }
        }

        public bool CondosForSaleEnabledForNonOwner
        {
            get
            {
                return this.condosForSaleEnabledForNonOwner;
            }
            set
            {
                this.condosForSaleEnabledForNonOwner = value;
            }
        }

        public bool CondosForSaleEnabledForOwner
        {
            get
            {
                return this.condosForSaleEnabledForOwner;
            }
            set
            {
                this.condosForSaleEnabledForOwner = value;
                if (value)
                {
                    A.State.City.SetupCondoOfferings();
                }
            }
        }

        public bool CreditCardsEnabledForNonOwner
        {
            get
            {
                return this.creditCardsEnabledForNonOwner;
            }
            set
            {
                this.creditCardsEnabledForNonOwner = value;
            }
        }

        public bool CreditCardsEnabledForOwner
        {
            get
            {
                return this.creditCardsEnabledForOwner;
            }
            set
            {
                this.creditCardsEnabledForOwner = value;
            }
        }

        public bool CreditCardStatementsEnabledForNonOwner
        {
            get
            {
                return this.creditCardStatementsEnabledForNonOwner;
            }
            set
            {
                this.creditCardStatementsEnabledForNonOwner = value;
            }
        }

        public bool CreditCardStatementsEnabledForOwner
        {
            get
            {
                return this.creditCardStatementsEnabledForOwner;
            }
            set
            {
                this.creditCardStatementsEnabledForOwner = value;
            }
        }

        public bool CreditReportEnabledForNonOwner
        {
            get
            {
                return this.creditReportEnabledForNonOwner;
            }
            set
            {
                this.creditReportEnabledForNonOwner = value;
            }
        }

        public bool CreditReportEnabledForOwner
        {
            get
            {
                return this.creditReportEnabledForOwner;
            }
            set
            {
                this.creditReportEnabledForOwner = value;
            }
        }

        public bool CreditScoreEnabledForNonOwner
        {
            get
            {
                return this.creditScoreEnabledForNonOwner;
            }
            set
            {
                this.creditScoreEnabledForNonOwner = value;
            }
        }

        public bool CreditScoreEnabledForOwner
        {
            get
            {
                return this.creditScoreEnabledForOwner;
            }
            set
            {
                this.creditScoreEnabledForOwner = value;
            }
        }

        public bool DisableFW4
        {
            get
            {
                return this.disableFW4;
            }
            set
            {
                this.disableFW4 = value;
            }
        }

        public DateTime DislocatedShoulderDate
        {
            get
            {
                return this.dislocatedShoulderDate;
            }
            set
            {
                this.dislocatedShoulderDate = value;
            }
        }

        public bool EducationEnabledForNonOwner
        {
            get
            {
                return this.educationEnabledForNonOwner;
            }
            set
            {
                this.educationEnabledForNonOwner = value;
            }
        }

        public bool EducationEnabledForOwner
        {
            get
            {
                return this.educationEnabledForOwner;
            }
            set
            {
                this.educationEnabledForOwner = value;
            }
        }

        public bool EndOnLowCreditScore
        {
            get
            {
                return this.endOnLowCreditScore;
            }
            set
            {
                this.endOnLowCreditScore = value;
            }
        }

        public int ExpectedMultiplayerPlayers
        {
            get
            {
                return this.expectedMultiplayerPlayers;
            }
            set
            {
                this.expectedMultiplayerPlayers = value;
            }
        }

        public bool FireForAbsencesLateness
        {
            get
            {
                return this.fireForAbsencesLateness;
            }
            set
            {
                this.fireForAbsencesLateness = value;
            }
        }

        public bool HealthcareEnabledForNonOwner
        {
            get
            {
                return this.healthcareEnabledForNonOwner;
            }
            set
            {
                this.healthcareEnabledForNonOwner = value;
            }
        }

        public bool HealthcareEnabledForOwner
        {
            get
            {
                return this.healthcareEnabledForOwner;
            }
            set
            {
                this.healthcareEnabledForOwner = value;
            }
        }

        public bool HealthEnabledForNonOwner
        {
            get
            {
                return this.healthEnabledForNonOwner;
            }
            set
            {
                this.healthEnabledForNonOwner = value;
            }
        }

        public bool HealthEnabledForOwner
        {
            get
            {
                return this.healthEnabledForOwner;
            }
            set
            {
                this.healthEnabledForOwner = value;
            }
        }

        public int HealthFactorsToConsider
        {
            get
            {
                return this.healthFactorsToConsider;
            }
            set
            {
                this.healthFactorsToConsider = value;
            }
        }

        public bool HealthInsuranceForFastFoodJobs
        {
            get
            {
                return this.healthInsuranceForFastFoodJobs;
            }
            set
            {
                this.healthInsuranceForFastFoodJobs = value;
            }
        }

        public bool HomeownersEnabledForNonOwner
        {
            get
            {
                return this.homeownersEnabledForNonOwner;
            }
            set
            {
                this.homeownersEnabledForNonOwner = value;
            }
        }

        public bool HomeownersEnabledForOwner
        {
            get
            {
                return this.homeownersEnabledForOwner;
            }
            set
            {
                this.homeownersEnabledForOwner = value;
            }
        }

        public bool IncludeOtherLiabilities
        {
            get
            {
                return this.includeOtherLiabilities;
            }
            set
            {
                this.includeOtherLiabilities = value;
            }
        }

        public float InflationRate
        {
            get
            {
                return this.inflationRate;
            }
            set
            {
                this.inflationRate = value;
            }
        }

        public float InitialCash
        {
            get
            {
                return this.initialCash;
            }
            set
            {
                this.initialCash = value;
            }
        }

        public float InitialHealth
        {
            get
            {
                return this.initialHealth;
            }
            set
            {
                this.initialHealth = value;
            }
        }

        public bool InternetAccessEnabledForNonOwner
        {
            get
            {
                return this.internetAccessEnabledForNonOwner;
            }
            set
            {
                this.internetAccessEnabledForNonOwner = value;
            }
        }

        public bool InternetAccessEnabledForOwner
        {
            get
            {
                return this.internetAccessEnabledForOwner;
            }
            set
            {
                this.internetAccessEnabledForOwner = value;
            }
        }

        public bool InvestmentStatementsEnabledForNonOwner
        {
            get
            {
                return this.investmentStatementsEnabledForNonOwner;
            }
            set
            {
                this.investmentStatementsEnabledForNonOwner = value;
            }
        }

        public bool InvestmentStatementsEnabledForOwner
        {
            get
            {
                return this.investmentStatementsEnabledForOwner;
            }
            set
            {
                this.investmentStatementsEnabledForOwner = value;
            }
        }

        public override int Level
        {
            get
            {
                return base.Level;
            }
            set
            {
                base.Level = value;
                if (value == 2)
                {
                    A.State.City.AddHealthInsurance(0.2f);
                    A.State.City.Add401Ks(0.2f);
                    this.HealthFactorsToConsider = 5;
                    this.ResearchFundsEnabledForOwner = true;
                    this.ViewPortfolioEnabledForOwner = true;
                    this.ViewRetirementPortfolioEnabledForOwner = true;
                    this.OnlineBankingEnabledForOwner = true;
                    this.InternetAccessEnabledForOwner = true;
                    this.HealthcareEnabledForOwner = true;
                }
                if (value == 3)
                {
                    this.CondosForSaleEnabledForOwner = true;
                    this.HomeownersEnabledForOwner = true;
                }
            }
        }

        public bool LoanStatementsEnabledForNonOwner
        {
            get
            {
                return this.loanStatementsEnabledForNonOwner;
            }
            set
            {
                this.loanStatementsEnabledForNonOwner = value;
            }
        }

        public bool LoanStatementsEnabledForOwner
        {
            get
            {
                return this.loanStatementsEnabledForOwner;
            }
            set
            {
                this.loanStatementsEnabledForOwner = value;
            }
        }

        public bool OnlineBankingEnabledForNonOwner
        {
            get
            {
                return this.onlineBankingEnabledForNonOwner;
            }
            set
            {
                this.onlineBankingEnabledForNonOwner = value;
            }
        }

        public bool OnlineBankingEnabledForOwner
        {
            get
            {
                return this.onlineBankingEnabledForOwner;
            }
            set
            {
                this.onlineBankingEnabledForOwner = value;
            }
        }

        public bool PastTaxReturnsEnabledForNonOwner
        {
            get
            {
                return this.pastTaxReturnsEnabledForNonOwner;
            }
            set
            {
                this.pastTaxReturnsEnabledForNonOwner = value;
            }
        }

        public bool PastTaxReturnsEnabledForOwner
        {
            get
            {
                return this.pastTaxReturnsEnabledForOwner;
            }
            set
            {
                this.pastTaxReturnsEnabledForOwner = value;
            }
        }

        public bool PayBillsEnabledForNonOwner
        {
            get
            {
                return this.payBillsEnabledForNonOwner;
            }
            set
            {
                this.payBillsEnabledForNonOwner = value;
            }
        }

        public bool PayBillsEnabledForOwner
        {
            get
            {
                return this.payBillsEnabledForOwner;
            }
            set
            {
                this.payBillsEnabledForOwner = value;
            }
        }

        public bool PayTaxRecordsEnabledForNonOwner
        {
            get
            {
                return this.payTaxRecordsEnabledForNonOwner;
            }
            set
            {
                this.payTaxRecordsEnabledForNonOwner = value;
            }
        }

        public bool PayTaxRecordsEnabledForOwner
        {
            get
            {
                return this.payTaxRecordsEnabledForOwner;
            }
            set
            {
                this.payTaxRecordsEnabledForOwner = value;
            }
        }

        public bool RentersEnabledForNonOwner
        {
            get
            {
                return this.rentersEnabledForNonOwner;
            }
            set
            {
                this.rentersEnabledForNonOwner = value;
            }
        }

        public bool RentersEnabledForOwner
        {
            get
            {
                return this.rentersEnabledForOwner;
            }
            set
            {
                this.rentersEnabledForOwner = value;
            }
        }

        public bool ResearchFundsEnabledForNonOwner
        {
            get
            {
                return this.researchFundsEnabledForNonOwner;
            }
            set
            {
                this.researchFundsEnabledForNonOwner = value;
            }
        }

        public bool ResearchFundsEnabledForOwner
        {
            get
            {
                return this.researchFundsEnabledForOwner;
            }
            set
            {
                this.researchFundsEnabledForOwner = value;
            }
        }

        public bool ResumeEnabledForNonOwner
        {
            get
            {
                return this.resumeEnabledForNonOwner;
            }
            set
            {
                this.resumeEnabledForNonOwner = value;
            }
        }

        public bool ResumeEnabledForOwner
        {
            get
            {
                return this.resumeEnabledForOwner;
            }
            set
            {
                this.resumeEnabledForOwner = value;
            }
        }

        public bool RetirementStatementsEnabledForNonOwner
        {
            get
            {
                return this.retirementStatementsEnabledForNonOwner;
            }
            set
            {
                this.retirementStatementsEnabledForNonOwner = value;
            }
        }

        public bool RetirementStatementsEnabledForOwner
        {
            get
            {
                return this.retirementStatementsEnabledForOwner;
            }
            set
            {
                this.retirementStatementsEnabledForOwner = value;
            }
        }

        public bool Sales
        {
            get
            {
                return this.sales;
            }
            set
            {
                this.sales = value;
            }
        }

        public bool ScheduleEnabledForNonOwner
        {
            get
            {
                return this.scheduleEnabledForNonOwner;
            }
            set
            {
                this.scheduleEnabledForNonOwner = value;
            }
        }

        public bool ScheduleEnabledForOwner
        {
            get
            {
                return this.scheduleEnabledForOwner;
            }
            set
            {
                this.scheduleEnabledForOwner = value;
            }
        }

        public bool ScheduleReadOnly
        {
            get
            {
                return this.scheduleReadOnly;
            }
            set
            {
                this.scheduleReadOnly = value;
            }
        }

        public bool SellCarEnabledForNonOwner
        {
            get
            {
                return this.sellCarEnabledForNonOwner;
            }
            set
            {
                this.sellCarEnabledForNonOwner = value;
            }
        }

        public bool SellCarEnabledForOwner
        {
            get
            {
                return this.sellCarEnabledForOwner;
            }
            set
            {
                this.sellCarEnabledForOwner = value;
            }
        }

        public bool ShopForCarEnabledForNonOwner
        {
            get
            {
                return this.shopForCarEnabledForNonOwner;
            }
            set
            {
                this.shopForCarEnabledForNonOwner = value;
            }
        }

        public bool ShopForCarEnabledForOwner
        {
            get
            {
                return this.shopForCarEnabledForOwner;
            }
            set
            {
                this.shopForCarEnabledForOwner = value;
            }
        }

        public bool ShopForFoodEnabledForNonOwner
        {
            get
            {
                return this.shopForFoodEnabledForNonOwner;
            }
            set
            {
                this.shopForFoodEnabledForNonOwner = value;
            }
        }

        public bool ShopForFoodEnabledForOwner
        {
            get
            {
                return this.shopForFoodEnabledForOwner;
            }
            set
            {
                this.shopForFoodEnabledForOwner = value;
            }
        }

        public bool ShopForGasRepairsEnabledForNonOwner
        {
            get
            {
                return this.shopForGasRepairsEnabledForNonOwner;
            }
            set
            {
                this.shopForGasRepairsEnabledForNonOwner = value;
            }
        }

        public bool ShopForGasRepairsEnabledForOwner
        {
            get
            {
                return this.shopForGasRepairsEnabledForOwner;
            }
            set
            {
                this.shopForGasRepairsEnabledForOwner = value;
            }
        }

        public bool ShopForGoodsEnabledForNonOwner
        {
            get
            {
                return this.shopForGoodsEnabledForNonOwner;
            }
            set
            {
                this.shopForGoodsEnabledForNonOwner = value;
            }
        }

        public bool ShopForGoodsEnabledForOwner
        {
            get
            {
                return this.shopForGoodsEnabledForOwner;
            }
            set
            {
                this.shopForGoodsEnabledForOwner = value;
            }
        }

        public bool Sickness
        {
            get
            {
                return this.sickness;
            }
            set
            {
                this.sickness = value;
            }
        }

        public bool SnapshotEnabledForNonOwner
        {
            get
            {
                return this.snapshotEnabledForNonOwner;
            }
            set
            {
                this.snapshotEnabledForNonOwner = value;
            }
        }

        public bool SnapshotEnabledForOwner
        {
            get
            {
                return this.snapshotEnabledForOwner;
            }
            set
            {
                this.snapshotEnabledForOwner = value;
            }
        }

        public string Surprises
        {
            get
            {
                return this.surprises;
            }
            set
            {
                this.surprises = value;
            }
        }

        public bool TaxesEnabledForNonOwner
        {
            get
            {
                return this.taxesEnabledForNonOwner;
            }
            set
            {
                this.taxesEnabledForNonOwner = value;
            }
        }

        public bool TaxesEnabledForOwner
        {
            get
            {
                return this.taxesEnabledForOwner;
            }
            set
            {
                this.taxesEnabledForOwner = value;
            }
        }

        public bool TransportationEnabledForNonOwner
        {
            get
            {
                return this.transportationEnabledForNonOwner;
            }
            set
            {
                this.transportationEnabledForNonOwner = value;
            }
        }

        public bool TransportationEnabledForOwner
        {
            get
            {
                return this.transportationEnabledForOwner;
            }
            set
            {
                this.transportationEnabledForOwner = value;
            }
        }

        public bool TurboSpeed
        {
            get
            {
                return this.turboSpeed;
            }
            set
            {
                this.turboSpeed = value;
                if (!value)
                {
                    A.State.SimulatedTimePerStep = 0x4e20;
                }
                else
                {
                    A.State.SimulatedTimePerStep = 0xdbba0;
                }
            }
        }

        public bool ViewPortfolioEnabledForNonOwner
        {
            get
            {
                return this.viewPortfolioEnabledForNonOwner;
            }
            set
            {
                this.viewPortfolioEnabledForNonOwner = value;
            }
        }

        public bool ViewPortfolioEnabledForOwner
        {
            get
            {
                return this.viewPortfolioEnabledForOwner;
            }
            set
            {
                this.viewPortfolioEnabledForOwner = value;
            }
        }

        public bool ViewRetirementPortfolioEnabledForNonOwner
        {
            get
            {
                return this.viewRetirementPortfolioEnabledForNonOwner;
            }
            set
            {
                this.viewRetirementPortfolioEnabledForNonOwner = value;
            }
        }

        public bool ViewRetirementPortfolioEnabledForOwner
        {
            get
            {
                return this.viewRetirementPortfolioEnabledForOwner;
            }
            set
            {
                this.viewRetirementPortfolioEnabledForOwner = value;
            }
        }

        public bool WealthEnabledForNonOwner
        {
            get
            {
                return this.wealthEnabledForNonOwner;
            }
            set
            {
                this.wealthEnabledForNonOwner = value;
            }
        }

        public bool WealthEnabledForOwner
        {
            get
            {
                return this.wealthEnabledForOwner;
            }
            set
            {
                this.wealthEnabledForOwner = value;
            }
        }

        public bool WorkEnabledForNonOwner
        {
            get
            {
                return this.workEnabledForNonOwner;
            }
            set
            {
                this.workEnabledForNonOwner = value;
            }
        }

        public bool WorkEnabledForOwner
        {
            get
            {
                return this.workEnabledForOwner;
            }
            set
            {
                this.workEnabledForOwner = value;
            }
        }
    }
}

