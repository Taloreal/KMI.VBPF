namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;

    public class frmInvestmentStatement : frmBankStatement
    {
        public frmInvestmentStatement()
        {
            this.Text = A.Resources.GetString("Investment Statements");
        }

        protected override SortedList GetAccounts()
        {
            return A.Adapter.GetInvestmentAccounts(A.MainForm.CurrentEntityID, false);
        }
    }
}

