using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KMI.VBPF1Lib { 

    public class CardControl : UserControl {

        public bool Selected;
        public BankAccount Account;
        private Container components = null;

        public CardControl(BankAccount ba) {
            this.InitializeComponent();
            this.Account = ba;
        }

        private void CardControl_Load(object sender, EventArgs e) {

        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            base.Name = "CardControl";
            base.Size = new Size(0xcc, 0x81);
            base.Load += new EventHandler(this.CardControl_Load);
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Font font = new Font("Arial", 9f);
            Font font2 = new Font("Arial", 8f);
            Brush brush = new SolidBrush(Color.White);
            Brush brush2 = new SolidBrush(Color.Black);
            string str = "CCard";
            if (this.Account is CheckingAccount) {
                str = "DCard";
            }
            e.Graphics.DrawImageUnscaled(A.Resources.GetImage(str + this.Account.BankName), 2, 2);
            if (this.Selected) {
                e.Graphics.DrawRectangle(new Pen(brush2, 5f), 0, 0, base.Width, base.Height);
            }
            e.Graphics.DrawString(this.Account.OwnerName, font, brush, 20f, (float) (base.Height - 20));
            e.Graphics.DrawString("1234 5678 9012 " + this.Account.AccountNumber.ToString(), font, brush, 40f, (float) ((base.Height / 2) + 5));
            e.Graphics.DrawString("Expires 02/99", font2, brush, 60f, (float) ((base.Height / 2) + 0x1c));
        }
    }
}

