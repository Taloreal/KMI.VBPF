
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

namespace KMI.VBPF1Lib {

    public class RecurringPaymentControl : UserControl {

        public TextBox Amount;
        private Container components = null;
        public ComboBox Day;
        public long PayeeAccountNumber;
        public Label PayeeName;

        public RecurringPaymentControl() {
            this.InitializeComponent();
            for (int i = 0; i < 0x1c; i++) {
                this.Day.Items.Add(i + 1);
            }
            this.Day.SelectedIndex = 0;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.PayeeName = new Label();
            this.Amount = new TextBox();
            this.Day = new ComboBox();
            base.SuspendLayout();
            this.PayeeName.AutoSize = true;
            this.PayeeName.Location = new Point(8, 0);
            this.PayeeName.Name = "PayeeName";
            this.PayeeName.Size = new Size(0, 0x10);
            this.PayeeName.TabIndex = 0;
            this.Amount.Location = new Point(0xe0, 0);
            this.Amount.Name = "Amount";
            this.Amount.Size = new Size(0x58, 20);
            this.Amount.TabIndex = 1;
            this.Amount.Text = "";
            this.Amount.TextAlign = HorizontalAlignment.Right;
            this.Day.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Day.Location = new Point(0x17e, 0);
            this.Day.Name = "Day";
            this.Day.Size = new Size(40, 0x15);
            this.Day.TabIndex = 2;
            base.Controls.Add(this.Day);
            base.Controls.Add(this.Amount);
            base.Controls.Add(this.PayeeName);
            base.Name = "RecurringPayment";
            base.Size = new Size(0x1a8, 0x18);
            base.ResumeLayout(false);
        }

        public RecurringPayment RecurringPayment {
            get {
                if (this.Amount.Text != "") {
                    float num = float.Parse(this.Amount.Text);
                    if (num > 0f) {
                        return new KMI.VBPF1Lib.RecurringPayment { PayeeName = this.PayeeName.Text, Amount = num, PayeeAccountNumber = this.PayeeAccountNumber, Day = (int) this.Day.SelectedItem };
                    }
                }
                return null;
            }
            set {
                this.Amount.Text = value.Amount.ToString("N2");
                this.Day.SelectedIndex = value.Day - 1;
            }
        }
    }
}

