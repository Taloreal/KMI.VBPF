using System;
using System.Collections;
using System.Drawing;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    [Serializable]
    public class InstallmentLoan : BankAccount {

        public DateTime BeginBilling;
        protected static Font fontB = new Font("Arial", 8f, FontStyle.Bold);
        protected static Font fontSB = new Font("Arial", 7f, FontStyle.Bold);
        protected static Font headerFont = new Font("Arial", 12f, FontStyle.Italic | FontStyle.Bold);
        public int Months = -1;
        protected float originalBalance;
        protected DateTime originationDate;
        protected float payment;
        protected int term;
        protected static Font titleFont = new Font("Arial", 12f, FontStyle.Bold);
        public ArrayList UnpaidAmounts = new ArrayList();

        public InstallmentLoan(DateTime date, float amount, float interestRate, int term)
        {
            this.originalBalance = amount;
            this.originationDate = date;
            base.interestRate = interestRate;
            this.term = term;
            this.ReComputePayment();
        }

        public override int DaysPastDue(DateTime now)
        {
            int num = this.UnpaidAmounts.Count - 1;
            int num2 = 0;
            while ((num >= 0) && (((float) this.UnpaidAmounts[num]) > 0.01))
            {
                num2++;
                num--;
            }
            if (num2 == 0)
            {
                return 0;
            }
            return (1 + ((num2 - 1) * 30));
        }

        public override void EndMonth()
        {
            if (A.State.Now > this.BeginBilling)
            {
                base.Transactions.Add(new Transaction((base.EndingBalance() * base.interestRate) / 12f, Transaction.TranType.Credit, "Interest", A.State.Now.AddDays(-1.0)));
                this.Months++;
                float num = 0f;
                ArrayList list = base.TransactionsForMonth(A.State.Year, A.State.Month);
                if (this.Months > 0)
                {
                    foreach (Transaction transaction in list)
                    {
                        if (transaction.Type == Transaction.TranType.Debit)
                        {
                            num += transaction.Amount;
                        }
                    }
                    this.UnpaidAmounts.Add(Math.Max((float) 0f, (float) (this.Payment - num)));
                    if (num > this.Payment)
                    {
                        num -= this.Payment;
                        for (int i = 0; i < this.UnpaidAmounts.Count; i++)
                        {
                            if (((float) this.UnpaidAmounts[i]) > 0f)
                            {
                                float num3 = Math.Min(num, (float) this.UnpaidAmounts[i]);
                                this.UnpaidAmounts[i] = ((float) this.UnpaidAmounts[i]) - num3;
                                num -= num3;
                            }
                            if (num < 0.01)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        public int MissedPayments()
        {
            int num = 0;
            int num2 = 0;
            while ((((this.UnpaidAmounts.Count - num) - 1) >= 0) && (num < 0x24))
            {
                if (((float) this.UnpaidAmounts[(this.UnpaidAmounts.Count - num) - 1]) > 0f)
                {
                    num2++;
                }
                num++;
            }
            return num2;
        }

        public int OnTimePayments()
        {
            int num = 0;
            int num2 = 0;
            while ((((this.UnpaidAmounts.Count - num) - 1) >= 0) && (num < 0x24))
            {
                if (((float) this.UnpaidAmounts[(this.UnpaidAmounts.Count - num) - 1]) == 0f)
                {
                    num2++;
                }
                num++;
            }
            return num2;
        }

        public override float PastDueAmount(DateTime now)
        {
            float num = 0f;
            foreach (float num2 in this.UnpaidAmounts)
            {
                num += num2;
            }
            return num;
        }

        public int PaymentsRemaining(int year, int month)
        {
            float num = base.EndingBalance(year, month);
            int num2 = 0;
            while ((num2 < this.Term) && (num > 0f))
            {
                num += ((num * base.InterestRate) / 12f) - this.Payment;
                num2++;
            }
            return num2;
        }

        public override void PrintPage(int page, Graphics g, int year, int month, int Pages, int TransactionsPerPage)
        {
            int margin = base.margin;
            int num2 = 180;
            int num3 = 310;
            int num4 = 15;
            int num5 = 50;
            int num6 = month + 1;
            int num7 = year;
            if (num6 == 13)
            {
                num6 = 1;
                num7 = year + 1;
            }
            DateTime now = new DateTime(year, month, 0x1c);
            DateTime time2 = new DateTime(num7, num6, 0x1c);
            if (this.BeginBilling.AddDays(30.0) > time2)
            {
                time2 = this.BeginBilling.AddDays(30.0);
            }
            StringFormat format = new StringFormat(BankAccount.sfc) {
                LineAlignment = StringAlignment.Far
            };
            ArrayList list = base.TransactionsForMonth(year, month);
            g.DrawImageUnscaled(A.Resources.GetImage("Logo" + base.BankName), 2, 4);
            margin += 20;
            g.DrawString(A.Resources.GetString("Installment Loan"), titleFont, BankAccount.brush, (float) num2, (float) margin);
            margin += 20;
            g.DrawString(A.Resources.GetString("Monthly Statement"), titleFont, BankAccount.brush, (float) num2, (float) margin);
            margin += 40;
            g.DrawString(A.Resources.GetString("Payment Information"), headerFont, BankAccount.brush, (float) base.margin, (float) margin);
            margin += 20;
            g.DrawLine(BankAccount.pen, base.margin, margin, num3, margin);
            margin += 10;
            g.DrawString(A.Resources.GetString("Statement Date:"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(new DateTime(year, month, 0x1c).ToShortDateString(), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            g.DrawString(A.Resources.GetString("Payment Due Date:"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(time2.ToShortDateString(), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 2 * num4;
            g.DrawString(A.Resources.GetString("Current Payment Due"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(Utilities.FC(this.Payment, 2, A.Instance.CurrencyConversion), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            g.DrawString(A.Resources.GetString("Past Due Payments"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(Utilities.FC(this.PastDueAmount(now), 2, A.Instance.CurrencyConversion), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            g.DrawString(A.Resources.GetString("Total Amount Due"), fontB, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(Utilities.FC(this.Payment + this.PastDueAmount(now), 2, A.Instance.CurrencyConversion), fontB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 2 * num4;
            g.DrawString(A.Resources.GetString("Payoff Summary"), fontB, BankAccount.brush, (float) base.margin, (float) margin);
            margin += num4;
            g.DrawString(A.Resources.GetString("Payoff Amount"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(Utilities.FC(base.EndingBalance(year, month), 2, A.Instance.CurrencyConversion), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            object[] args = new object[] { Utilities.FC(-99f, 2, A.Instance.CurrencyConversion) };
            g.DrawString(A.Resources.GetString("Payoff Good Through", args), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(time2.ToShortDateString(), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 0x19;
            g.DrawString(A.Resources.GetString("Account Information"), headerFont, BankAccount.brush, (float) base.margin, (float) margin);
            margin += 20;
            g.DrawLine(BankAccount.pen, base.margin, margin, num3, margin);
            margin += 10;
            g.DrawString(A.Resources.GetString("Account Number"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(base.AccountNumber.ToString(), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += num4;
            g.DrawString(A.Resources.GetString("Payments Remaining"), BankAccount.font, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(this.PaymentsRemaining(year, month).ToString(), BankAccount.font, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 0x19;
            g.DrawString(A.Resources.GetString("Activity Since Last Statement"), headerFont, BankAccount.brush, (float) base.margin, (float) margin);
            margin += 20;
            g.DrawLine(BankAccount.pen, base.margin, margin, num3, margin);
            margin += 10;
            g.DrawString(A.Resources.GetString("Date"), fontSB, BankAccount.brush, (float) base.margin, (float) margin);
            g.DrawString(A.Resources.GetString("Description"), fontSB, BankAccount.brush, (float) (num5 + 5), (float) margin);
            g.DrawString(A.Resources.GetString("Amount"), fontSB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
            margin += 2 * num4;
            foreach (Transaction transaction in list)
            {
                g.DrawString(transaction.Date.ToShortDateString(), fontSB, BankAccount.brush, (float) base.margin, (float) margin);
                g.DrawString(transaction.Description, fontSB, BankAccount.brush, (float) (num5 + 5), (float) margin);
                int num10 = 1;
                if (transaction.Type == Transaction.TranType.Debit)
                {
                    num10 = -1;
                }
                g.DrawString(Utilities.FC(num10 * transaction.Amount, 2, A.Instance.CurrencyConversion), fontSB, BankAccount.brush, (float) num3, (float) margin, BankAccount.sfr);
                margin += num4;
            }
            base.StampStatus(g, year, month);
        }

        public void ReComputePayment()
        {
            int term = this.term;
            float num2 = 100000f;
            float num3 = 1f;
            float num4 = 500f;
            if (this.originalBalance == 0f) { this.payment = 0f; }
            else {
                while (Math.Abs((float) (num3 - num2)) > 0.01f) {
                    float ogbal = this.originalBalance;
                    for (int i = 0; i < term; i++) {
                        ogbal = (ogbal * (1f + (base.interestRate / 12f))) - num4;
                    }
                    if (ogbal < 0f) { num2 = num4; }
                    else { num3 = num4; }
                    num4 = (num3 + num2) / 2f;
                }
                this.payment = (float) Math.Round((double) num2, 2);
            }
        }

        public override string AccountTypeFriendlyName
        {
            get
            {
                return A.Resources.GetString("Loan");
            }
        }

        public float OriginalBalance
        {
            get
            {
                return this.originalBalance;
            }
            set
            {
                this.originalBalance = value;
            }
        }

        public DateTime OriginationDate
        {
            get
            {
                return this.originationDate;
            }
        }

        public float Payment
        {
            get
            {
                return this.payment;
            }
        }

        public int Term
        {
            get
            {
                return this.term;
            }
        }
    }
}

