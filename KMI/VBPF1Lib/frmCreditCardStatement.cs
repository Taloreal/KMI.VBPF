namespace KMI.VBPF1Lib
{
    using System;
    using System.Collections;

    public class frmCreditCardStatement : frmBankStatement
    {
        public frmCreditCardStatement()
        {
            this.Text = A.Resources.GetString("Credit Card Statements");
            base.picStatement.Width = 0x151;
            base.TransactionsPerPage = 0x18;
        }

        protected override SortedList GetAccounts()
        {
            return A.Adapter.GetCreditCardAccounts(A.MainForm.CurrentEntityID);
        }
    }
}

