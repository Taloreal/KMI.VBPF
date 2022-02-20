namespace KMI.VBPF1Lib
{
    using KMI.Biz.City;
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    [Serializable]
    public class AppBuildingDrawable : BuildingDrawable
    {
        public long BuildingID;
        public bool IsOwnersDwelling;
        public ArrayList Offerings;

        public AppBuildingDrawable(Point location, string imageName, BuildingType bldgType, int avenue, int street, int lot, long ownerID, ArrayList offerings, string clickString, bool isOwnersDwelling) : base(location, imageName, bldgType, avenue, street, lot, ownerID, 0f, 0f, "")
        {
            base.VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom;
            this.Offerings = offerings;
            base.clickString = clickString;
            base.OwnerID = ownerID;
            this.IsOwnersDwelling = isOwnersDwelling;
        }

        private void BankHandler(object sender, EventArgs e)
        {
            try
            {
                switch (((MenuItem) sender).Index)
                {
                    case 0:
                        new frmOpenBankAccount(base.Avenue, base.Street, base.Lot, true).ShowDialog(A.MainForm);
                        return;

                    case 1:
                        new frmOpenBankAccount(base.Avenue, base.Street, base.Lot, false).ShowDialog(A.MainForm);
                        return;

                    case 2:
                    {
                        CreditCardAccount creditCardOffer = A.Adapter.GetCreditCardOffer(A.MainForm.CurrentEntityID, (CreditCardAccount) this.Offerings[2]);
                        object[] args = new object[] { creditCardOffer.BankName, Utilities.FP(creditCardOffer.InterestRate), Utilities.FC(creditCardOffer.CreditLimit, A.Instance.CurrencyConversion), Utilities.FC(creditCardOffer.LatePaymentFee, A.Instance.CurrencyConversion) };
                        if (MessageBox.Show(A.Resources.GetString("We are pleased to offer you a {0} credit card at low {1} APR!. High credit limit of {2}. Late payment fee is {3}. Would you like to receive this card?", args), A.Resources.GetString("Credit Card Offer"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            A.Adapter.SetCreditCardAccount(A.MainForm.CurrentEntityID, creditCardOffer);
                            MessageBox.Show(A.Resources.GetString("Your card has been issued."), A.Resources.GetString("Congratulations!"));
                        }
                        return;
                    }
                    case 3:
                    case 4:
                    case 8:
                        return;

                    case 5:
                        MessageBox.Show(A.Resources.GetString("To deposit cash, click on the cash pile on your desk in your apartment. To deposit checks, click on the pile of checks on your desk in your apartment."), A.Resources.GetString("Deposit Funds"));
                        return;

                    case 6:
                        new frmDepositWithdrawCash(((BankAccount) this.Offerings[0]).BankName, true).ShowDialog(A.MainForm);
                        return;

                    case 7:
                        new frmTransferFunds(((BankAccount) this.Offerings[0]).BankName).ShowDialog(A.MainForm);
                        return;

                    case 9:
                        new frmCloseAccount(((BankAccount) this.Offerings[0]).BankName).ShowDialog(A.MainForm);
                        return;
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void BuyCondoHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                DwellingOffer offering = (DwellingOffer) this.Offerings[index - 1];
                frmMortgage mortgage = new frmMortgage(offering);
                if (mortgage.ShowDialog(A.MainForm) == DialogResult.OK)
                {
                    if (MessageBox.Show("Do you want to move into your new condo at this time? If you have any months left on an existing lease, you will be charged for them.", "Confirm Move", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        A.Adapter.MoveTo(A.MainForm.CurrentEntityID, offering.ID);
                    }
                    MessageBox.Show("Congratulations. You bought yourself a condo!", "Got Condo");
                }
            }
            catch (EntityNotFoundException)
            {
                MessageBox.Show(A.Resources.GetString("You do not have enough money to buy a condo. Get an apartment first."), A.Resources.GetString("Condo Purchase"));
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void CarShopHandler(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsCreditShopForCar.PerformClick();
        }

        private void CourseHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                Course course = (Course) this.Offerings[index - 1];
                new frmStudentLoan(course).ShowDialog(A.MainForm);
                A.MainForm.UpdateView();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void DeletionHandler(object sender, EventArgs e)
        {
            int index = ((MenuItem) sender).Index;
            int num2 = 0;
            foreach (Offering offering in this.Offerings)
            {
                if ((index == 0) || ((index - 1) == num2++))
                {
                    A.Adapter.DeleteOffering(offering.ID);
                }
            }
        }

        public override void Drawable_Click(object sender, EventArgs e) {
            if (A.MainForm.DesignerMode && (MessageBox.Show("Debug building?", "Designer Mode", MessageBoxButtons.YesNo) == DialogResult.Yes)) {
                FrmDebugOfferings debugger = new FrmDebugOfferings(this);
                debugger.Show();
                return;
                ContextMenu menu = new ContextMenu();
                menu.MenuItems.Add(new MenuItem("All", new EventHandler(this.DeletionHandler)));
                for (int i = 0; i < this.Offerings.Count; i++)
                {
                    int num2 = i + 1;
                    menu.MenuItems.Add(new MenuItem(num2.ToString(), new EventHandler(this.DeletionHandler)));
                }
                menu.Show(A.MainForm, base.Location);
            }
            else
            {
                ContextMenu m = new ContextMenu();
                switch (base.BuildingType.Index)
                {
                    case 0:
                        return;

                    case 5:
                        m.MenuItems.Add(A.Resources.GetString("Enter"), new EventHandler(this.EnterHandler));
                        foreach (Course course in this.Offerings)
                        {
                            object[] args = new object[] { course.Name };
                            m.MenuItems.Add(A.Resources.GetString("Enroll in {0}", args), new EventHandler(this.CourseHandler));
                        }
                        this.Enable(m, A.MainForm.mnuActionsIncomeEducation);
                        break;

                    case 6:
                        m.MenuItems.Add(A.Resources.GetString("Shop"), new EventHandler(this.ShopBusTokensHandler));
                        this.Enable(m, A.MainForm.mnuActionsShopBusTokens);
                        break;

                    case 8:
                        m.MenuItems.Add(A.Resources.GetString("Open Checking Account"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add(A.Resources.GetString("Open Savings Account"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add(A.Resources.GetString("Apply for Credit Card"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add(A.Resources.GetString("Apply for a personal loan"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add("-");
                        m.MenuItems.Add(A.Resources.GetString("Deposit Funds"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add(A.Resources.GetString("Withdraw Funds"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add(A.Resources.GetString("Transfer Funds"), new EventHandler(this.BankHandler));
                        m.MenuItems.Add("-");
                        m.MenuItems.Add(A.Resources.GetString("Close Account"), new EventHandler(this.BankHandler));
                        this.Enable(m, A.MainForm.mnuActionsMMBanking);
                        break;

                    case 9:
                        m.MenuItems.Add(A.Resources.GetString("Shop for Car"), new EventHandler(this.CarShopHandler));
                        m.MenuItems.Add(A.Resources.GetString("Sell Car"), new EventHandler(this.SellCarHandler));
                        this.Enable(m, A.MainForm.mnuActionsCreditShopForCar);
                        break;

                    case 10:
                        break;

                    case 11:
                        m.MenuItems.Add(A.Resources.GetString("Healthcare"), new EventHandler(this.InsuranceHandler));
                        m.MenuItems.Add(A.Resources.GetString("Renters"), new EventHandler(this.InsuranceHandler));
                        m.MenuItems.Add(A.Resources.GetString("Homeowners"), new EventHandler(this.InsuranceHandler));
                        m.MenuItems.Add(A.Resources.GetString("Automobile"), new EventHandler(this.InsuranceHandler));
                        this.Enable(m, A.MainForm.mnuActionsInsurance);
                        break;

                    case 12:
                        m.MenuItems.Add(A.Resources.GetString("Shop"), new EventHandler(this.ShopHandler));
                        this.Enable(m, A.MainForm.mnuActionsCreditForGoods);
                        break;

                    case 13:
                        m.MenuItems.Add(A.Resources.GetString("Shop"), new EventHandler(this.ShopFoodHandler));
                        this.Enable(m, A.MainForm.mnuActionsCreditShopForFood);
                        break;

                    case 14:
                        m.MenuItems.Add(A.Resources.GetString("Subscribe to Internet Access ($45-$55/mo)"), new EventHandler(this.InternetSubscribeHandler));
                        m.MenuItems.Add(A.Resources.GetString("Cancel Subscription"), new EventHandler(this.InternetUnSubscribeHandler));
                        this.Enable(m, A.MainForm.mnuActionsCreditInternet);
                        break;

                    case 15:
                        m.MenuItems.Add(A.Resources.GetString("Shop"), new EventHandler(this.ShopAutoRepairHandler));
                        this.Enable(m, A.MainForm.mnuActionsCreditShopForGas);
                        break;

                    case 0x10:
                        m.MenuItems.Add(A.Resources.GetString("Research Funds"), new EventHandler(this.InvestmentAccountsHandler));
                        m.MenuItems.Add(A.Resources.GetString("View Portfolio"), new EventHandler(this.InvestmentAccountsHandler));
                        m.MenuItems.Add(A.Resources.GetString("View Retirement Portfolio"), new EventHandler(this.InvestmentAccountsHandler));
                        this.Enable(m, A.MainForm.mnuActionsInvestingResearchFunds);
                        break;

                    default:
                    {
                        m.MenuItems.Add(A.Resources.GetString("Enter"), new EventHandler(this.EnterHandler));
                        int num4 = 1;
                        foreach (Offering offering in this.Offerings)
                        {
                            if (offering is Job)
                            {
                                object[] objArray2 = new object[] { num4 };
                                m.MenuItems.Add(A.Resources.GetString("Apply for Job {0}", objArray2), new EventHandler(this.JobHandler));
                                if (!offering.Taken)
                                {
                                    num4++;
                                }
                                else
                                {
                                    m.MenuItems[m.MenuItems.Count - 1].Visible = false;
                                }
                                this.Enable(m, A.MainForm.mnuActionsIncomeWork);
                            }
                            else if (offering is DwellingOffer)
                            {
                                if (((DwellingOffer) offering).Condo)
                                {
                                    if (!offering.Taken)
                                    {
                                        m.MenuItems.Add(A.Resources.GetString("Buy Condo"), new EventHandler(this.BuyCondoHandler));
                                    }
                                    else if (base.OwnerID == A.MainForm.CurrentEntityID)
                                    {
                                        if (!this.IsOwnersDwelling)
                                        {
                                            m.MenuItems.Add("Move to Condo", new EventHandler(this.MoveToCondoHandler));
                                            m.MenuItems.Add("Sell Condo", new EventHandler(this.SellCondoHandler));
                                        }
                                        m.MenuItems.Add("Change Insurance", new EventHandler(this.HomeOwnersHandler));
                                    }
                                }
                                else if (!offering.Taken)
                                {
                                    m.MenuItems.Add(A.Resources.GetString("Rent Apartment"), new EventHandler(this.DwellingHandler));
                                }
                                this.Enable(m, A.MainForm.mnuActionsLivingHousing);
                            }
                        }
                        break;
                    }
                }
                if (((((base.BuildingType.Index == 4) || (base.BuildingType.Index == 0x11)) || ((base.BuildingType.Index == 2) || (base.BuildingType.Index == 3))) || (base.BuildingType.Index == 0x12)) || (base.BuildingType.Index == 0x13))
                {
                    m.MenuItems[0].Visible = false;
                }
                if (((m.MenuItems.Count == 1) && m.MenuItems[0].Enabled) && m.MenuItems[0].Visible)
                {
                    m.MenuItems[0].PerformClick();
                }
                else
                {
                    m.Show(A.MainForm.picMain, base.Location);
                }
                KMI.Sim.View.ClearCurrentHit();
            }
        }

        public override void DrawSelected(Graphics g)
        {
            if ((base.clickString != null) && (base.clickString != ""))
            {
                Point anchor = new Point((this.location.X + (this.Size.Width / 2)) + base.offsetX, (this.location.Y + base.offsetY) + (this.Size.Height / 3));
                Font font = new Font("Arial", 8f);
                Utilities.DrawComment(g, base.clickString, anchor, Rectangle.Round(g.VisibleClipBounds), 300, font, Color.SteelBlue);
            }
        }

        private void DwellingHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                DwellingOffer offering = (DwellingOffer) this.Offerings[index - 1];
                if (!A.Adapter.HasEntity(A.Instance.ThisPlayerName))
                {
                    new frmChooseCharacter(A.Instance.ThisPlayerName).ShowDialog(A.MainForm);
                }
                string movingMessage = A.Adapter.GetMovingMessage(A.MainForm.CurrentEntityID, offering);
                if ((movingMessage == null) || (MessageBox.Show(movingMessage, "Confirm Move", MessageBoxButtons.YesNo) == DialogResult.Yes))
                {
                    A.Adapter.AddOffering(A.MainForm.CurrentEntityID, offering.ID, null);
                    MessageBox.Show("Congratulations! You got the apartment!", "Got Apartment");
                }
                A.MainForm.UpdateView();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void Enable(ContextMenu m, MenuItem mainMenuItem)
        {
            foreach (MenuItem item in m.MenuItems)
            {
                if (item.Text != A.Resources.GetString("Enter"))
                {
                    item.Enabled = mainMenuItem.Enabled;
                }
            }
        }

        private void EnterHandler(object sender, EventArgs e)
        {
            InsideView view = (InsideView) A.Instance.Views[1];
            view.ViewerOptions = new object[] { base.Avenue, base.Street, base.Lot };
            if (!S.Instance.Client)
            {
                A.State.ViewerOptions1 = view.ViewerOptions;
            }
            view.SetBackground(base.BuildingType.Index);
            A.MainForm.OnViewChange(view.Name);
        }

        private void HomeOwnersHandler(object sender, EventArgs e)
        {
            try
            {
                new frmHomeOwnersInsurance((Offering) this.Offerings[0]).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void InsuranceHandler(object sender, EventArgs e)
        {
            try
            {
                switch (((MenuItem) sender).Index)
                {
                    case 0:
                        A.MainForm.mnuActionsInsuranceHealth.PerformClick();
                        return;

                    case 1:
                        A.MainForm.mnuActionsInsuranceRenters.PerformClick();
                        return;

                    case 2:
                        A.MainForm.mnuActionsInsuranceHomeowners.PerformClick();
                        return;

                    case 3:
                        A.MainForm.mnuActionsInsuranceAuto.PerformClick();
                        return;
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void InternetSubscribeHandler(object sender, EventArgs e)
        {
            try
            {
                A.Adapter.InternetSubscribe(A.MainForm.CurrentEntityID);
                MessageBox.Show("Your internet connection is now turned on.", "Confirm Subscription");
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void InternetUnSubscribeHandler(object sender, EventArgs e)
        {
            try
            {
                A.Adapter.InternetUnSubscribe(A.MainForm.CurrentEntityID);
                MessageBox.Show("Your internet connection is now turned off.", "Confirm Cancellation");
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void InvestmentAccountsHandler(object sender, EventArgs e)
        {
            switch (((MenuItem) sender).Index)
            {
                case 0:
                    A.MainForm.mnuActionsInvestingResearchFunds.PerformClick();
                    break;

                case 1:
                    A.MainForm.mnuActionsInvestingMyPortfolio.PerformClick();
                    break;

                case 2:
                    A.MainForm.mnuActionsInvestingRetirement.PerformClick();
                    break;
            }
        }

        private void JobHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                Job job = (Job) this.Offerings[index - 1];
                frmJobApplication application = new frmJobApplication();
                if (application.ShowDialog(A.MainForm) == DialogResult.OK)
                {
                    AddOfferingInfo info = A.Adapter.AddOffering(A.MainForm.CurrentEntityID, job.ID, application.JobApp);
                    MessageBox.Show("Congratulations! You got the job!  Your employer needs a little more information so you can get paid properly...", "Got Job");
                    new frmMethodOfPay(info.TaskID).ShowDialog();
                    new frmW4(info.TaskID).ShowDialog();
                    if (info.IsFirstTravel)
                    {
                        PickTravelMode();
                    }
                    if ((((WorkTask) job.PrototypeTask).R401KMatch > -1f) && (MessageBox.Show("This job offers a 401K retirement savings plan. Would you like to participate? If you answer yes, you will be asked how you want to allocate your investments.", "401K Plan", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        new frm401K(info.TaskID).ShowDialog(A.MainForm);
                    }
                }
                A.MainForm.UpdateView();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void MoveToCondoHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                DwellingOffer offer = (DwellingOffer) this.Offerings[0];
                if (MessageBox.Show("Are you sure you want to move? If you have any months left on an existing lease, you will be charged for them.", "Confirm Move", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    A.Adapter.MoveTo(A.MainForm.CurrentEntityID, offer.ID);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public static void PickTravelMode()
        {
            MessageBox.Show(A.Resources.GetString("Since this is your first activity outside your home, you will be asked to choose a mode of transportation."), "Travel Mode");
            new frmTransportation().ShowDialog();
        }

        private void SellCarHandler(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsCreditSellCar.PerformClick();
        }

        private void SellCondoHandler(object sender, EventArgs e)
        {
            try
            {
                int index = ((MenuItem) sender).Index;
                DwellingOffer offering = (DwellingOffer) this.Offerings[0];
                if (MessageBox.Show(A.Adapter.GetCondoPrice(A.MainForm.CurrentEntityID, offering), A.Resources.GetString("Confirm Sale"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    A.Adapter.SellCondo(A.MainForm.CurrentEntityID, offering);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void ShopAutoRepairHandler(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsCreditShopForGas.PerformClick();
        }

        private void ShopBusTokensHandler(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsShopBusTokens.PerformClick();
        }

        private void ShopFoodHandler(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsCreditShopForFood.PerformClick();
        }

        private void ShopHandler(object sender, EventArgs e)
        {
            try
            {
                ArrayList shop = A.Adapter.GetShop(A.MainForm.CurrentEntityID, this.BuildingID);
                new frmShop(base.ClickString.Substring(0, base.ClickString.IndexOf(":")), shop, false).ShowDialog(A.MainForm);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct AddOfferingInfo
        {
            public bool IsFirstTravel;
            public long TaskID;
        }
    }
}

