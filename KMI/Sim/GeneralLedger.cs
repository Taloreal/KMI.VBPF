namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.IO;
    using System.Windows.Forms;

    [Serializable]
    public class GeneralLedger
    {
        protected static Font accountFont = new Font(new FontFamily("Arial"), 10f, FontStyle.Bold);
        protected ArrayList accounts = new ArrayList();
        public static bool AutomaticallyRollForwardStockAccounts = true;
        protected static Brush brush = new SolidBrush(Color.Black);
        protected static Font dateFont = new Font(new FontFamily("Arial"), 10f, FontStyle.Bold);
        public static bool DisplayCOGS = true;
        public static bool DisplayGrossMargin = true;
        public static bool FirstLevelSubaccountsWhenUndetailed = true;
        protected const int Left_Margin = 10;
        protected const string LINEFEED = "\r\n";
        protected int myCurrentWeek = 0;
        protected ArrayList printAccountList;
        protected Frequency printFrequency;
        protected int printLine;
        protected bool printPercentages;
        protected int printPeriod;
        [NonSerialized]
        protected int printPeriodsLeft;
        protected int printRow;
        protected string printTitle;
        protected string[] productNames;
        protected static StringFormat sfr = new StringFormat();
        private DateTime simStartDate;
        protected const int SubAccount_Indent = 10;
        protected static Font subAccountFont = new Font(new FontFamily("Arial"), 10f);
        public static bool SupressAllZeroLines = false;
        protected const string TAB = "\t";
        protected static Font titleFont = new Font(new FontFamily("Arial"), 12f, FontStyle.Bold);
        public const int Top_Margin = 40;
        public static int WeeksOfFinancialHistory = 0xd0;
        public static int WeeksOfProductHistory = 0x34;
        public static int[] WeeksPerPeriod = new int[] { 1, 13, 0x34 };

        public GeneralLedger(string[] productNames)
        {
            this.productNames = productNames;
            sfr.Alignment = StringAlignment.Far;
            this.simStartDate = S.Settings.StartDate;
        }

        public float AccountBalance(string name)
        {
            return this.AccountBalance(name, S.State.CurrentWeek, Frequency.Weekly);
        }

        public float AccountBalance(string name, int period)
        {
            return this.AccountBalance(name, period, Frequency.Weekly);
        }

        public float AccountBalance(string name, int period, Frequency freq)
        {
            Account account = this.GetAccount(name);
            if (freq > Frequency.Weekly)
            {
                int num = ((period + 1) * WeeksPerPeriod[(int) freq]) - 1;
                if ((account.Type == AccountType.Liability) || (account.Type == AccountType.Asset))
                {
                    return this.AccountBalance(name, num, Frequency.Weekly);
                }
                float num2 = 0f;
                for (int i = (num - WeeksPerPeriod[(int) freq]) + 1; i <= num; i++)
                {
                    num2 += this.AccountBalance(name, i, Frequency.Weekly);
                }
                return num2;
            }
            this.CheckForNewWeek(period);
            if (account.Type == AccountType.GrossMargin)
            {
                return (this.AccountBalance("Revenue", period, freq) - this.AccountBalance("COGS", period, freq));
            }
            if (account.Type == AccountType.Profit)
            {
                float num5 = 0f;
                foreach (Account account2 in this.accounts)
                {
                    if ((account2.Parent == null) && (account2.Type == AccountType.Expense))
                    {
                        num5 += account2.GetBalance(this.FinancialWeekIndex(period));
                    }
                }
                return (this.AccountBalance("Gross Margin", period, freq) - num5);
            }
            if (account.Type == AccountType.Product)
            {
                return account.GetBalance(this.ProductWeekIndex(period));
            }
            return account.GetBalance(this.FinancialWeekIndex(period));
        }

        public ArrayList AccountList(string statement, bool detail)
        {
            ArrayList list = new ArrayList();
            if (statement == Simulator.Instance.Resource.GetString("Income Statement"))
            {
                foreach (Account account2 in this.accounts)
                {
                    if (account2.Type == AccountType.Revenue)
                    {
                        list.Add(account2);
                        if (detail)
                        {
                            list.AddRange(account2.AllSubAccounts);
                        }
                        else if (FirstLevelSubaccountsWhenUndetailed)
                        {
                            list.AddRange(account2.SubAccounts);
                        }
                    }
                }
                if (DisplayCOGS)
                {
                    foreach (Account account3 in this.accounts)
                    {
                        if (account3.Type == AccountType.COGS)
                        {
                            list.Add(account3);
                            if (detail)
                            {
                                list.AddRange(account3.AllSubAccounts);
                            }
                            else if (FirstLevelSubaccountsWhenUndetailed)
                            {
                                list.AddRange(account3.SubAccounts);
                            }
                        }
                    }
                }
                if (DisplayGrossMargin)
                {
                    list.Add(this.GetAccount("Gross Margin"));
                }
                foreach (Account account4 in this.accounts)
                {
                    if (account4.Type == AccountType.Expense)
                    {
                        list.Add(account4);
                        if (detail)
                        {
                            list.AddRange(account4.AllSubAccounts);
                        }
                        else if (FirstLevelSubaccountsWhenUndetailed)
                        {
                            list.AddRange(account4.SubAccounts);
                        }
                    }
                }
                list.Add(this.GetAccount("Profit"));
                return list;
            }
            if (!(statement == Simulator.Instance.Resource.GetString("Balance Sheet")))
            {
                throw new Exception("Statement name incorrect.");
            }
            Account account = this.GetAccount("Assets");
            list.Add(account);
            if (detail)
            {
                list.AddRange(account.AllSubAccounts);
            }
            else if (FirstLevelSubaccountsWhenUndetailed)
            {
                list.AddRange(account.SubAccounts);
            }
            account = this.GetAccount("Liabilities Plus Equity");
            list.Add(account);
            if (detail)
            {
                list.AddRange(account.AllSubAccounts);
                return list;
            }
            if (FirstLevelSubaccountsWhenUndetailed)
            {
                list.AddRange(account.SubAccounts);
            }
            return list;
        }

        public ArrayList AccountList(ArrayList productDataSeriesNames, ArrayList productNames, bool units)
        {
            ArrayList list = new ArrayList();
            string str = "";
            if (units)
            {
                str = " (units)";
            }
            foreach (string str2 in productDataSeriesNames)
            {
                foreach (string str3 in productNames)
                {
                    list.Add(this.GetAccount(str2 + " - " + str3 + str));
                }
            }
            return list;
        }

        public ArrayList AccountListForGraphing(string statement)
        {
            ArrayList list = new ArrayList();
            if (statement == Simulator.Instance.Resource.GetString("Income Statement"))
            {
                list.Add(this.GetAccount("Revenue"));
                if (DisplayGrossMargin)
                {
                    list.Add(this.GetAccount("Gross Margin"));
                }
                list.Add(this.GetAccount("Profit"));
                return list;
            }
            if (statement != Simulator.Instance.Resource.GetString("Balance Sheet"))
            {
                throw new Exception("Statement name incorrect.");
            }
            list.Add(this.GetAccount("Assets"));
            list.Add(this.GetAccount("Liabilities"));
            list.Add(this.GetAccount("Equity"));
            return list;
        }

        protected string AccountsAsText(ArrayList accountList, string title, int currentPeriod)
        {
            this.printLine = 0;
            int weeksOfFinancialHistory = WeeksOfFinancialHistory;
            if ((accountList.Count > 0) && (((Account) accountList[0]).Type == AccountType.Product))
            {
                weeksOfFinancialHistory = WeeksOfProductHistory;
            }
            int num2 = Math.Max(0, currentPeriod - (weeksOfFinancialHistory - 1));
            string str = title + "\r\n\r\n\t";
            int period = num2;
            while (period < currentPeriod)
            {
                str = str + string.Format("{0:dd MMM yy}", this.EndingDateOfPeriod(period, Frequency.Weekly)) + "\t";
                period++;
            }
            foreach (Account account in accountList)
            {
                if ((!SupressAllZeroLines || (account.Level <= 0)) || !account.IsAllZero())
                {
                    str = str + "\r\n";
                    str = str + "".PadRight(account.Level * 4);
                    str = str + account.Name + "\t";
                    for (period = num2; period < currentPeriod; period++)
                    {
                        str = str + string.Format("{0:###,###,##0}", this.AccountBalance(account.Name, period, Frequency.Weekly)) + "\t";
                    }
                }
            }
            return str;
        }

        public void AddAccount(string name, AccountType type)
        {
            if ((type != AccountType.Product) && (type != AccountType.OtherProduct))
            {
                this.accounts.Add(new Account(name, type));
            }
            else
            {
                this.accounts.Add(new Account(name + " - Total", type));
                this.accounts.Add(new Account(name + " - Total (units)", type));
                foreach (string str in this.productNames)
                {
                    new Account(name + " - " + str, type, this.GetAccount(name + " - Total"));
                    new Account(name + " - " + str + " (units)", type, this.GetAccount(name + " - Total (units)"));
                }
            }
        }

        public void AddAccount(string name, AccountType type, string parentName)
        {
            if ((type == AccountType.Product) || (type == AccountType.OtherProduct))
            {
                throw new Exception("Sub accounts are automatically added for accounts of type Product. Do not add them explicitly.");
            }
            new Account(name, type, this.GetAccount(parentName));
        }

        protected ArrayList AllAccounts()
        {
            ArrayList list = new ArrayList();
            foreach (Account account in this.accounts)
            {
                list.Add(account);
                foreach (Account account2 in account.AllSubAccounts)
                {
                    list.Add(account2);
                }
            }
            return list;
        }

        protected void CarryForwardOrClearBalances(int wk)
        {
            float amount = this.AccountBalance("Profit", wk - 1, Frequency.Weekly);
            foreach (Account account in this.AllAccounts())
            {
                if (account.SubAccounts.Count == 0)
                {
                    if (account.Type == AccountType.Product)
                    {
                        account.Clear(this.ProductWeekIndex(wk));
                    }
                    else
                    {
                        account.Clear(this.FinancialWeekIndex(wk));
                    }
                    if (account.Type == AccountType.Liability)
                    {
                        if (account.Name == "Retained Earnings")
                        {
                            account.Credit(this.FinancialWeekIndex(wk - 1), amount);
                        }
                        account.Credit(this.FinancialWeekIndex(wk), account.GetBalance(this.FinancialWeekIndex(wk - 1)));
                    }
                    else if (account.Type == AccountType.Asset)
                    {
                        account.Debit(this.FinancialWeekIndex(wk), account.GetBalance(this.FinancialWeekIndex(wk - 1)));
                    }
                    else if ((account.Type == AccountType.Product) && account.Name.StartsWith("Inventory"))
                    {
                        account.Debit(this.ProductWeekIndex(wk), account.GetBalance(this.ProductWeekIndex(wk - 1)));
                    }
                }
            }
        }

        protected void CheckForNewWeek(int week)
        {
            if (AutomaticallyRollForwardStockAccounts && (this.myCurrentWeek < week))
            {
                for (int i = this.myCurrentWeek + 1; i <= week; i++)
                {
                    this.CarryForwardOrClearBalances(i);
                }
                this.myCurrentWeek = week;
            }
        }

        public GeneralLedger Clone()
        {
            return (GeneralLedger) Utilities.DeepCopyBySerialization(this);
        }

        public void ConsolidateWith(GeneralLedger GL)
        {
            int currentWeek = S.State.CurrentWeek;
            foreach (Account account in this.accounts)
            {
                if (account.SubAccounts.Count == 0)
                {
                    for (int i = Math.Max(0, currentWeek - WeeksOfFinancialHistory); i < currentWeek; i++)
                    {
                        float amount = GL.AccountBalance(account.Name, i, Frequency.Weekly);
                        if ((account.Type == AccountType.Liability) | (account.Type == AccountType.Revenue))
                        {
                            amount = -amount;
                        }
                        account.Debit(i, amount);
                    }
                }
            }
        }

        public DateTime EndingDateOfPeriod(int period, Frequency freq)
        {
            int num = ((period + 1) * WeeksPerPeriod[(int) freq]) - 1;
            return this.simStartDate.AddDays((double) (num * 7));
        }

        protected int FinancialWeekIndex(int week)
        {
            return (week % WeeksOfFinancialHistory);
        }

        private string FrequencyName(Frequency freq)
        {
            switch (freq)
            {
                case Frequency.Weekly:
                    return Simulator.Instance.Resource.GetString("Weekly");

                case Frequency.Quarterly:
                    return Simulator.Instance.Resource.GetString("Quarterly");

                case Frequency.Annually:
                    return Simulator.Instance.Resource.GetString("Annual");
            }
            throw new Exception("Bad frequency passed into Frequency name");
        }

        protected Account GetAccount(string name)
        {
            foreach (Account account in this.accounts)
            {
                if (account.GetAccount(name) != null)
                {
                    return account.GetAccount(name);
                }
            }
            throw new Exception("Account " + name + " does not exist.");
        }

        private void GL_PrintPage(object sender, PrintPageEventArgs e)
        {
            Utilities.ResetFPU();
            Graphics g = e.Graphics;
            int num = this.MaxPrintingColumns(e.MarginBounds.Width, this.printAccountList, g);
            int endPeriod = Math.Min((int) ((this.printPeriod + num) - 1), (int) ((this.printPeriod + this.printPeriodsLeft) - 1));
            int num3 = this.MaxPrintingRows(e.MarginBounds.Height, g);
            int count = this.printAccountList.Count;
            int endRow = Math.Min((int) ((this.printRow + num3) - 1), (int) (count - 1));
            this.PrintAccountList(this.printAccountList, this.printTitle, this.printRow, endRow, this.printPeriod, endPeriod, this.printFrequency, this.printPercentages, g);
            if (endPeriod < ((this.printPeriod + this.printPeriodsLeft) - 1))
            {
                this.printPeriod += num;
                this.printPeriodsLeft -= num;
                e.HasMorePages = true;
            }
            else if (endRow < (count - 1))
            {
                this.printPeriod = 0;
                this.printRow = endRow + 1;
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }
        }

        public void Graph(ArrayList accountList, string title, bool units, bool percentages, int currentPeriod, KMIGraph graph)
        {
            Simulator instance = Simulator.Instance;
            object[,] data = new object[0, 0];
            graph.GraphType = 1;
            graph.Title = title;
            graph.XAxisTitle = "Week Ending";
            graph.YAxisTitle = null;
            graph.YLabelFormat = null;
            int weeksOfFinancialHistory = WeeksOfFinancialHistory;
            if ((accountList.Count > 0) && (((Account) accountList[0]).Type == AccountType.Product))
            {
                weeksOfFinancialHistory = WeeksOfProductHistory;
            }
            int beginPeriod = Math.Max(0, currentPeriod - (weeksOfFinancialHistory - 1));
            if (percentages)
            {
                if (accountList.Count > 0)
                {
                    if ((((Account) accountList[0]).Type == AccountType.Asset) || (((Account) accountList[0]).Type == AccountType.Liability))
                    {
                        graph.YAxisTitle = "% of Assets";
                    }
                    else
                    {
                        graph.YAxisTitle = "% of Revenue";
                    }
                }
                graph.YLabelFormat = "{0:##0%}";
            }
            int line = 0;
            data = new object[accountList.Count + 1, (currentPeriod - beginPeriod) + 1];
            foreach (Account account in accountList)
            {
                this.InsertIntoGraphDataAccountBalances(account, data, ref line, percentages, currentPeriod, beginPeriod);
            }
            if (units)
            {
                graph.YLabelFormat = "{0:###,###,##0}";
            }
            for (int i = beginPeriod; i < currentPeriod; i++)
            {
                data[0, (i - beginPeriod) + 1] = this.EndingDateOfPeriod(i, Frequency.Weekly);
            }
            graph.Draw(data);
        }

        protected void InsertIntoGraphDataAccountBalances(Account account, object[,] data, ref int line, bool percentages, int currentPeriod, int beginPeriod)
        {
            line++;
            data[line, 0] = S.Resources.GetString(account.Name);
            for (int i = beginPeriod; i < currentPeriod; i++)
            {
                float num2;
                if (!percentages)
                {
                    num2 = this.AccountBalance(account.Name, i, Frequency.Weekly);
                    data[line, (i - beginPeriod) + 1] = num2;
                }
                else
                {
                    float num3;
                    if ((account.Type == AccountType.Liability) || (account.Type == AccountType.Asset))
                    {
                        num3 = this.AccountBalance("Assets", i, Frequency.Weekly);
                    }
                    else
                    {
                        num3 = this.AccountBalance("Revenue", i, Frequency.Weekly);
                    }
                    num2 = this.AccountBalance(account.Name, i, Frequency.Weekly) / Math.Max(1f, num3);
                    data[line, (i - beginPeriod) + 1] = num2;
                }
            }
        }

        protected string LongestAccountName(ArrayList accountList)
        {
            string name = "";
            foreach (Account account in accountList)
            {
                if (account.Name.Length > name.Length)
                {
                    name = account.Name;
                }
            }
            return name;
        }

        public int MaxPrintingColumns(int overallWidth, ArrayList accountList, Graphics g)
        {
            int width = (int) g.MeasureString(this.LongestAccountName(accountList) + "XX", accountFont).Width;
            int num2 = (int) g.MeasureString("X99,999,999", accountFont).Width;
            return (((overallWidth - width) - 20) / num2);
        }

        public int MaxPrintingRows(int overallHeight, Graphics g)
        {
            int height = (int) g.MeasureString("XX", titleFont).Height;
            int num2 = (int) g.MeasureString("XX", dateFont).Height;
            int num3 = (int) g.MeasureString("XX", accountFont).Height;
            return (((overallHeight - num3) - 40) / num3);
        }

        public void Post(string debitAccountName, float amount, string creditAccountName)
        {
            this.Post(debitAccountName, amount, creditAccountName, Simulator.Instance.SimState.CurrentWeek);
        }

        public void Post(string debitAccountName, float amount, string creditAccountName, int week)
        {
            this.CheckForNewWeek(week);
            this.GetAccount(debitAccountName).Debit(this.FinancialWeekIndex(week), amount);
            this.GetAccount(creditAccountName).Credit(this.FinancialWeekIndex(week), amount);
        }

        public void Post(string debitAccountName, float amount, string creditAccountName, string productName, int units, string[] productDebitAccountNames, string[] productCreditAccountNames)
        {
            this.Post(debitAccountName, amount, creditAccountName, productName, units, productDebitAccountNames, productCreditAccountNames, Simulator.Instance.SimState.CurrentWeek);
        }

        public void Post(string debitAccountName, float amount, string creditAccountName, string productName, int units, string[] productDebitAccountNames, string[] productCreditAccountNames, int week)
        {
            this.CheckForNewWeek(week);
            this.Post(debitAccountName, amount, creditAccountName, this.FinancialWeekIndex(week));
            foreach (string str in productDebitAccountNames)
            {
                this.GetAccount(str + " - " + productName).Debit(this.ProductWeekIndex(week), amount);
                this.GetAccount(str + " - " + productName + " (units)").Debit(this.ProductWeekIndex(week), (float) units);
            }
            foreach (string str2 in productCreditAccountNames)
            {
                this.GetAccount(str2 + " - " + productName).Credit(this.ProductWeekIndex(week), amount);
                this.GetAccount(str2 + " - " + productName + " (units)").Credit(this.ProductWeekIndex(week), (float) units);
            }
        }

        public void PostNonFinancial(string debitAccountName, float amount)
        {
            this.PostNonFinancial(debitAccountName, amount, Simulator.Instance.SimState.CurrentWeek);
        }

        public void PostNonFinancial(string debitAccountName, float amount, int week)
        {
            Account account = this.GetAccount(debitAccountName);
            if ((account.Type != AccountType.Other) && (account.Type != AccountType.OtherProduct))
            {
                throw new Exception("Double entry accounting violation.");
            }
            this.CheckForNewWeek(week);
            account.Debit(this.FinancialWeekIndex(week), amount);
        }

        public void PostNonFinancial(string debitAccountName, float amount, string productName)
        {
            this.PostNonFinancial(debitAccountName, amount, productName, Simulator.Instance.SimState.CurrentWeek);
        }

        public void PostNonFinancial(string debitAccountName, float amount, string productName, int week)
        {
            this.PostNonFinancial(debitAccountName + " - " + productName, amount, week);
        }

        protected virtual void PrintAccountBalance(Account account, int beginPeriod, int endPeriod, Frequency freq, bool percentages, int firstColWidth, Graphics g)
        {
            Font accountFont;
            float height = g.MeasureString("X", GeneralLedger.accountFont).Height;
            int width = (int) g.MeasureString("X99,999,999", GeneralLedger.accountFont).Width;
            if (account.Level == 0)
            {
                accountFont = GeneralLedger.accountFont;
            }
            else
            {
                accountFont = subAccountFont;
            }
            g.DrawString(S.Resources.GetString(account.Name), accountFont, brush, (float) (10 + (account.Level * 10)), 40f + (this.printLine * height));
            for (int i = beginPeriod; i <= endPeriod; i++)
            {
                float num4;
                if (!percentages)
                {
                    num4 = this.AccountBalance(account.Name, i, freq);
                    g.DrawString(string.Format("{0:###,###,##0}", num4), accountFont, brush, (float) ((10 + firstColWidth) + (((i - beginPeriod) + 1) * width)), 40f + (this.printLine * height), sfr);
                }
                else
                {
                    float num5;
                    if ((account.Type == AccountType.Liability) || (account.Type == AccountType.Asset))
                    {
                        num5 = this.AccountBalance("Assets", i, freq);
                    }
                    else
                    {
                        num5 = this.AccountBalance("Revenue", i, freq);
                    }
                    num4 = this.AccountBalance(account.Name, i, freq) / Math.Max(1f, num5);
                    g.DrawString(string.Format("{0:##0%}", num4), accountFont, brush, (float) ((10 + firstColWidth) + (((i - beginPeriod) + 1) * width)), 40f + (this.printLine * height), sfr);
                }
            }
        }

        protected virtual void PrintAccountList(ArrayList accountList, string title, int beginRow, int endRow, int beginPeriod, int endPeriod, Frequency freq, bool percentages, Graphics g)
        {
            g.Clear(Color.White);
            this.printLine = 0;
            int width = (int) g.MeasureString(this.LongestAccountName(accountList) + "X", accountFont).Width;
            int num2 = (int) g.MeasureString("X99,999,999", accountFont).Width;
            g.DrawString(title, titleFont, brush, (float) 10f, (float) 10f);
            for (int i = beginPeriod; i <= endPeriod; i++)
            {
                g.DrawString(string.Format("{0:dd MMM yy}", this.EndingDateOfPeriod(i, freq)), dateFont, brush, (float) ((10 + width) + (((i - beginPeriod) + 1) * num2)), 40f, sfr);
            }
            this.printLine++;
            for (int j = beginRow; j <= endRow; j++)
            {
                Account account = (Account) accountList[j];
                if ((!SupressAllZeroLines || (account.Level <= 0)) || !account.IsAllZero())
                {
                    this.PrintAccountBalance(account, beginPeriod, endPeriod, freq, percentages, width, g);
                    this.printLine++;
                }
            }
        }

        public void PrintToFile(ArrayList accountList, string title, int currentPeriod)
        {
            MessageBox.Show("Your data will be exported to a tab-delimited text file which can be opened with Excel.", "Export Format");
            SaveFileDialog dialog = new SaveFileDialog {
                Filter = "Tab Delimited Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = ".txt"
            };
            if (Simulator.Instance.UserAdminSettings.DefaultDirectory != null)
            {
                dialog.InitialDirectory = Simulator.Instance.UserAdminSettings.DefaultDirectory;
            }
            while (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(dialog.FileName);
                    writer.Write(this.AccountsAsText(accountList, title, currentPeriod));
                    break;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Could not export to file. File may be read-only or in use by another application.\r\n\r\nError details: " + exception.Message, "Could Not Export");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
        }

        public void PrintToPrinter(ArrayList accountList, string title, Frequency freq, bool percentages)
        {
            this.printAccountList = accountList;
            this.printFrequency = freq;
            this.printPercentages = percentages;
            this.printTitle = title;
            this.printRow = 0;
            int num = this.myCurrentWeek / WeeksPerPeriod[(int) this.printFrequency];
            this.printPeriodsLeft = Math.Min(this.myCurrentWeek, WeeksOfFinancialHistory - 1) / WeeksPerPeriod[(int) this.printFrequency];
            if ((accountList.Count > 0) && (((Account) accountList[0]).Type == AccountType.Product))
            {
                this.printPeriodsLeft = Math.Min(this.myCurrentWeek, WeeksOfProductHistory - 1) / WeeksPerPeriod[(int) this.printFrequency];
            }
            this.printPeriod = num - this.printPeriodsLeft;
            Utilities.PrintWithExceptionHandling("", new PrintPageEventHandler(this.GL_PrintPage));
        }

        public void PrintToScreen(ArrayList accountList, string title, int beginRow, int endRow, int beginPeriod, int endPeriod, Frequency freq, bool percentages, Graphics g)
        {
            this.PrintAccountList(accountList, title, beginRow, endRow, beginPeriod, endPeriod, freq, percentages, g);
        }

        protected int ProductWeekIndex(int week)
        {
            return (week % WeeksOfProductHistory);
        }

        public float RowHeight(Graphics g)
        {
            return g.MeasureString("X", accountFont).Height;
        }

        public ArrayList ProductAccountBaseNames
        {
            get
            {
                ArrayList list = new ArrayList();
                foreach (Account account in this.accounts)
                {
                    if ((account.Type == AccountType.Product) && account.Name.EndsWith(" - Total"))
                    {
                        list.Add(account.Name.Substring(0, account.Name.Length - " - Total".Length));
                    }
                }
                return list;
            }
        }

        public string[] ProductNames
        {
            get
            {
                return this.productNames;
            }
        }

        [Serializable]
        public class Account
        {
            protected float[] balance;
            protected string name;
            protected GeneralLedger.Account parent;
            protected ArrayList subAccounts;
            protected GeneralLedger.AccountType type;

            public Account(string name, GeneralLedger.AccountType type)
            {
                this.subAccounts = new ArrayList();
                this.name = name;
                this.type = type;
                if (type != GeneralLedger.AccountType.Product)
                {
                    this.balance = new float[GeneralLedger.WeeksOfFinancialHistory];
                }
                else
                {
                    this.balance = new float[GeneralLedger.WeeksOfProductHistory];
                }
            }

            public Account(string name, GeneralLedger.AccountType type, GeneralLedger.Account parent)
            {
                this.subAccounts = new ArrayList();
                this.name = name;
                this.type = type;
                this.parent = parent;
                parent.SubAccounts.Add(this);
                if (type != GeneralLedger.AccountType.Product)
                {
                    this.balance = new float[GeneralLedger.WeeksOfFinancialHistory];
                }
                else
                {
                    this.balance = new float[GeneralLedger.WeeksOfProductHistory];
                }
            }

            public void Clear(int week)
            {
                this.balance[week] = 0f;
            }

            public void Credit(int weekIndex, float amount)
            {
                this.Debit(weekIndex, -amount);
            }

            public void Debit(int weekIndex, float amount)
            {
                if (this.SubAccounts.Count > 0)
                {
                    throw new Exception("Attempt to set aggregate account balance not allowed.");
                }
                if ((this.Type == GeneralLedger.AccountType.Liability) | (this.Type == GeneralLedger.AccountType.Revenue))
                {
                    this.balance[weekIndex] -= amount;
                }
                else
                {
                    this.balance[weekIndex] += amount;
                }
            }

            public GeneralLedger.Account GetAccount(string name)
            {
                if (this.name == name)
                {
                    return this;
                }
                if (this.SubAccounts.Count > 0)
                {
                    foreach (GeneralLedger.Account account2 in this.SubAccounts)
                    {
                        if (account2.GetAccount(name) != null)
                        {
                            return account2.GetAccount(name);
                        }
                    }
                }
                return null;
            }

            public float GetBalance(int weekIndex)
            {
                float num = 0f;
                if (this.SubAccounts.Count == 0)
                {
                    return this.balance[weekIndex];
                }
                foreach (GeneralLedger.Account account in this.SubAccounts)
                {
                    num += account.GetBalance(weekIndex);
                }
                return num;
            }

            public bool IsAllZero()
            {
                foreach (float num2 in this.balance)
                {
                    if (!(num2 == 0f))
                    {
                        return false;
                    }
                }
                return true;
            }

            public void RenameSpecialForVBR3(string newName)
            {
                this.name = newName;
            }

            public ArrayList AllSubAccounts
            {
                get
                {
                    ArrayList list = new ArrayList();
                    foreach (GeneralLedger.Account account in this.subAccounts)
                    {
                        list.Add(account);
                        list.AddRange(account.AllSubAccounts);
                    }
                    return list;
                }
            }

            public int Level
            {
                get
                {
                    GeneralLedger.Account parent = this;
                    int num = 0;
                    while (parent.Parent != null)
                    {
                        parent = parent.Parent;
                        num++;
                    }
                    return num;
                }
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
            }

            public GeneralLedger.Account Parent
            {
                get
                {
                    return this.parent;
                }
                set
                {
                    this.parent = value;
                }
            }

            public ArrayList SubAccounts
            {
                get
                {
                    return this.subAccounts;
                }
            }

            public GeneralLedger.AccountType Type
            {
                get
                {
                    return this.type;
                }
            }
        }

        public enum AccountType
        {
            Revenue,
            Expense,
            COGS,
            Asset,
            Liability,
            GrossMargin,
            Profit,
            Product,
            Other,
            OtherProduct
        }

        public enum Frequency
        {
            Weekly,
            Quarterly,
            Annually
        }
    }
}

