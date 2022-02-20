using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmSelectCard : Form {

        private SortedList accounts;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        public BankAccount Card;
        private Container components = null;
        private bool credit;
        private Label label2;
        private Panel panCards;
        private Panel panel1;
        public NumericUpDown updAmount;

        public frmSelectCard(bool credit) {
            this.InitializeComponent();
            this.credit = credit;
            if (credit) {
                this.accounts = A.Adapter.GetGoodCreditCardAccounts(A.MainForm.CurrentEntityID);
            }
            else {
                this.accounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            }
            foreach (BankAccount account in this.accounts.Values) {
                if ((account is CreditCardAccount) || (account is CheckingAccount)) {
                    CardControl control = new CardControl(account);
                    control.Click += new EventHandler(this.Card_Click);
                    control.Location = new Point(((control.Width + 20) * this.panCards.Controls.Count) + 20, 20);
                    this.panCards.Controls.Add(control);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            base.Close();
        }

        private void btnCancel_Click_1(object sender, EventArgs e) {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e) {
            KMIHelp.OpenHelp(A.Resources.GetString("Pay Bills"));
        }

        private void btnOK_Click(object sender, EventArgs e) {
            foreach (CardControl control in this.panCards.Controls) {
                if (control.Selected) {
                    this.Card = control.Account;
                    base.Close();
                    return;
                }
            }
            MessageBox.Show(A.Resources.GetString("Please select a card by clicking on it."), A.Resources.GetString("Input Required"));
        }

        private void Card_Click(object sender, EventArgs e) {
            foreach (CardControl control in this.panCards.Controls) {
                control.Selected = control == sender;
            }
            this.Refresh();
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmSelectCard_Load(object sender, EventArgs e) {
            if (this.panCards.Controls.Count == 0) {
                if (this.credit) {
                    MessageBox.Show(A.Resources.GetString("You do not have any credit cards. To get a credit card, click on a bank in the City View."), A.Resources.GetString("No Card"));
                }
                else {
                    MessageBox.Show(A.Resources.GetString("You do not have any debit cards. To get a debit card, click on a bank in the City View and open a checking account."), A.Resources.GetString("No Card"));
                }
                base.Close();
            }
            if (this.panCards.Controls.Count == 1) {
                ((CardControl) this.panCards.Controls[0]).Selected = true;
            }
        }

        private void InitializeComponent() {
            this.panel1 = new Panel();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.btnOK = new Button();
            this.panCards = new Panel();
            this.updAmount = new NumericUpDown();
            this.label2 = new Label();
            this.panel1.SuspendLayout();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnHelp);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xe2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x224, 40);
            this.panel1.TabIndex = 0;
            this.btnCancel.Location = new Point(0xe2, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click_1);
            this.btnHelp.Location = new Point(0x15a, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnOK.Location = new Point(0x6a, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.panCards.AutoScroll = true;
            this.panCards.Location = new Point(0, 0);
            this.panCards.Name = "panCards";
            this.panCards.Size = new Size(0x224, 0xac);
            this.panCards.TabIndex = 1;
            this.updAmount.DecimalPlaces = 2;
            this.updAmount.Location = new Point(0x100, 0xbc);
            int[] bits = new int[4];
            bits[0] = 0xf4240;
            this.updAmount.Maximum = new decimal(bits);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x60, 20);
            this.updAmount.TabIndex = 10;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.updAmount.ThousandsSeparator = true;
            bits = new int[4];
            bits[0] = 20;
            this.updAmount.Value = new decimal(bits);
            this.label2.Location = new Point(0x9c, 0xc0);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x10);
            this.label2.TabIndex = 9;
            this.label2.Text = "Amount to Pay:";
            this.label2.TextAlign = ContentAlignment.TopRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x224, 0x10a);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.panCards);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmSelectCard";
            base.ShowInTaskbar = false;
            this.Text = "Select Card";
            base.Load += new EventHandler(this.frmSelectCard_Load);
            this.panel1.ResumeLayout(false);
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }
    }
}

