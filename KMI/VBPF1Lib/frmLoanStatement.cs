namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;

    public class frmLoanStatement : frmBankStatement
    {
        public frmLoanStatement()
        {
            this.Text = A.Resources.GetString("Loan Statements");
        }

        protected override SortedList GetAccounts()
        {
            return A.Adapter.GetInstallmentLoans(A.MainForm.CurrentEntityID);
        }
    }
}

