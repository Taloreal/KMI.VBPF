namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;

    public class frmRetirementStatement : frmBankStatement
    {
        public frmRetirementStatement() {
            this.Text = A.Resources.GetString("Retirement Statements");
        }

        protected override SortedList GetAccounts() {
            return A.Adapter.GetInvestmentAccounts(A.MainForm.CurrentEntityID, true);
        }
    }
}

