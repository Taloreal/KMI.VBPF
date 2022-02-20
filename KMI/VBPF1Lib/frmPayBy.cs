using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using KMI.Sim;

namespace KMI.VBPF1Lib {

    public class frmPayBy : Form {

        private Bill bill;
        private Button btnCancel;
        private Button btnCash;
        private Button btnCheck;
        private Button btnCreditCard;
        private Button btnDebitCard;
        private Container components;
        private ArrayList shoppingList;

        public frmPayBy(Bill bill) {
            this.components = null;
            this.InitializeComponent();
            this.bill = bill;
        }

        public frmPayBy(Bill bill, ArrayList shoppingList)
        {
            this.components = null;
            this.InitializeComponent();
            this.bill = bill;
            this.shoppingList = shoppingList;
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            try
            {
                frmPayInCash cash = new frmPayInCash();
                cash.updAmount.Value = Math.Min(Math.Max(0M, (decimal)this.bill.Amount), cash.updAmount.Maximum);
                if ((this.bill.Account != null) && !(this.bill.Account is InstallmentLoan))
                {
                    cash.updAmount.Value = Math.Max(decimal.Zero, (decimal) this.bill.Account.EndingBalance());
                }
                if (this.shoppingList != null)
                {
                    cash.updAmount.Enabled = false;
                }
                if ((this.shoppingList != null) || (cash.ShowDialog(this) == DialogResult.OK))
                {
                    this.bill.Amount = (float) cash.updAmount.Value;
                    A.Adapter.PayByCash(A.MainForm.CurrentEntityID, this.bill, (float) cash.updAmount.Value);
                    if (this.shoppingList != null)
                    {
                        A.Adapter.Purchase(A.MainForm.CurrentEntityID, this.shoppingList);
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            this.PayByCard(true);
        }

        private void btnDebitCard_Click(object sender, EventArgs e)
        {
            this.PayByCard(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.shoppingList != null)
            {
                MessageBox.Show(A.Resources.GetString("Personal checks are not accepted for merchandise purchases at this store."), A.Resources.GetString("Please Retry"));
            }
            else
            {
                try
                {
                    frmCheckbook checkbook = new frmCheckbook();
                    checkbook.FillIn(this.bill);
                    if (checkbook.ShowDialog(this) == DialogResult.OK)
                    {
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnCheck = new Button();
            this.btnCreditCard = new Button();
            this.btnDebitCard = new Button();
            this.btnCash = new Button();
            this.btnCancel = new Button();
            base.SuspendLayout();
            this.btnCheck.Location = new Point(40, 0x1c);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new Size(0x80, 0x1c);
            this.btnCheck.TabIndex = 0;
            this.btnCheck.Text = "Check";
            this.btnCheck.Click += new EventHandler(this.button1_Click);
            this.btnCreditCard.Location = new Point(40, 0x48);
            this.btnCreditCard.Name = "btnCreditCard";
            this.btnCreditCard.Size = new Size(0x80, 0x1c);
            this.btnCreditCard.TabIndex = 1;
            this.btnCreditCard.Text = "Credit Card";
            this.btnCreditCard.Click += new EventHandler(this.btnCreditCard_Click);
            this.btnDebitCard.Location = new Point(40, 120);
            this.btnDebitCard.Name = "btnDebitCard";
            this.btnDebitCard.Size = new Size(0x80, 0x1c);
            this.btnDebitCard.TabIndex = 2;
            this.btnDebitCard.Text = "Debit Card";
            this.btnDebitCard.Click += new EventHandler(this.btnDebitCard_Click);
            this.btnCash.Location = new Point(40, 0xa4);
            this.btnCash.Name = "btnCash";
            this.btnCash.Size = new Size(0x80, 0x1c);
            this.btnCash.TabIndex = 3;
            this.btnCash.Text = "Cash";
            this.btnCash.Click += new EventHandler(this.btnCash_Click);
            this.btnCancel.Location = new Point(0x88, 220);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x40, 0x18);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.button5_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xd4, 0xfe);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnCash);
            base.Controls.Add(this.btnDebitCard);
            base.Controls.Add(this.btnCreditCard);
            base.Controls.Add(this.btnCheck);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmPayBy";
            base.ShowInTaskbar = false;
            this.Text = "Pay By";
            base.ResumeLayout(false);
        }

        private void PayByCard(bool credit)
        {
            try
            {
                frmSelectCard card = new frmSelectCard(credit);
                card.updAmount.Value = Math.Min(Math.Max(0M, (decimal)this.bill.Amount), card.updAmount.Maximum);
                if (!(((this.bill.Account == null) || (this.bill.Account is InstallmentLoan)) || (this.bill.Account is CreditCardAccount)))
                {
                    card.updAmount.Value = Math.Max(0M, (decimal)this.bill.Account.EndingBalance());
                }
                if (this.shoppingList != null)
                {
                    card.updAmount.Enabled = false;
                }
                card.ShowDialog(this);
                if (card.Card != null)
                {
                    this.bill.Amount = (float) card.updAmount.Value;
                    A.Adapter.PayByCard(A.MainForm.CurrentEntityID, this.bill, (float) card.updAmount.Value, card.Card.AccountNumber, credit);
                    if (this.shoppingList != null)
                    {
                        A.Adapter.Purchase(A.MainForm.CurrentEntityID, this.shoppingList);
                    }
                    base.DialogResult = DialogResult.OK;
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }
    }
}

