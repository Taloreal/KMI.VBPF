using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace KMI.VBPF1Lib {

    public class CheckControl : UserControl {

        private Bill bill;
        private Check check;
        private Container components = null;
        public Label labAccountNumber;
        private Label labAmountWords;
        public Label labBankName;
        public Label labCheckNumber;
        public Label labCheckNumberBottom;
        public Label label2;
        public Label label22;
        public Label label24;
        public Label label3;
        public Label label30;
        public Label label31;
        public Label label32;
        public Label label33;
        public Label label34;
        public Label label35;
        public Label label37;
        public Label label4;
        public Label label5;
        public Label label6;
        public Label labelBankNumber;
        private Label labMonthDay;
        public Label labPayee;
        public Label labPayor;
        public Label labSignature;
        private Label labYear;
        private Panel panel1;
        public TextBox txtAmount;
        public TextBox txtMemo;

        public CheckControl() {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.panel1 = new Panel();
            this.txtAmount = new TextBox();
            this.labCheckNumber = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.label6 = new Label();
            this.labCheckNumberBottom = new Label();
            this.labBankName = new Label();
            this.labAccountNumber = new Label();
            this.labelBankNumber = new Label();
            this.label22 = new Label();
            this.label24 = new Label();
            this.label30 = new Label();
            this.label31 = new Label();
            this.label32 = new Label();
            this.label33 = new Label();
            this.label34 = new Label();
            this.label35 = new Label();
            this.label37 = new Label();
            this.labPayor = new Label();
            this.labSignature = new Label();
            this.labAmountWords = new Label();
            this.labPayee = new Label();
            this.labYear = new Label();
            this.labMonthDay = new Label();
            this.txtMemo = new TextBox();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtAmount);
            this.panel1.Controls.Add(this.labCheckNumber);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.labCheckNumberBottom);
            this.panel1.Controls.Add(this.labBankName);
            this.panel1.Controls.Add(this.labAccountNumber);
            this.panel1.Controls.Add(this.labelBankNumber);
            this.panel1.Controls.Add(this.label22);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Controls.Add(this.label31);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.label34);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.label37);
            this.panel1.Controls.Add(this.labPayor);
            this.panel1.Controls.Add(this.labSignature);
            this.panel1.Controls.Add(this.labAmountWords);
            this.panel1.Controls.Add(this.labPayee);
            this.panel1.Controls.Add(this.labYear);
            this.panel1.Controls.Add(this.labMonthDay);
            this.panel1.Controls.Add(this.txtMemo);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x22c, 200);
            this.panel1.TabIndex = 0;
            this.txtAmount.BackColor = Color.LightCyan;
            this.txtAmount.BorderStyle = BorderStyle.None;
            this.txtAmount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.txtAmount.ForeColor = SystemColors.ControlText;
            this.txtAmount.Location = new Point(420, 0x44);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new Size(120, 13);
            this.txtAmount.TabIndex = 0x51;
            this.txtAmount.Text = "";
            this.txtAmount.TextAlign = HorizontalAlignment.Center;
            this.txtAmount.TextChanged += new EventHandler(this.txtAmount_TextChanged);
            this.labCheckNumber.BackColor = Color.LightCyan;
            this.labCheckNumber.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labCheckNumber.Location = new Point(0x1c8, 8);
            this.labCheckNumber.Name = "labCheckNumber";
            this.labCheckNumber.Size = new Size(80, 0x10);
            this.labCheckNumber.TabIndex = 0x4d;
            this.labCheckNumber.Text = "108";
            this.labCheckNumber.TextAlign = ContentAlignment.TopRight;
            this.label2.BackColor = Color.LightCyan;
            this.label2.BorderStyle = BorderStyle.FixedSingle;
            this.label2.Location = new Point(0x30, 0xa4);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x92, 1);
            this.label2.TabIndex = 0x4c;
            this.label2.Text = "label2";
            this.label3.BackColor = Color.LightCyan;
            this.label3.Location = new Point(0x1e4, 100);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x40, 0x10);
            this.label3.TabIndex = 0x4b;
            this.label3.Text = "DOLLARS";
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.LightCyan;
            this.label4.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(0x58, 0x84);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x48, 14);
            this.label4.TabIndex = 0x4a;
            this.label4.Text = "Springfield, USA";
            this.label5.BackColor = Color.LightCyan;
            this.label5.BorderStyle = BorderStyle.FixedSingle;
            this.label5.Location = new Point(0x1c4, 0x30);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x1a, 1);
            this.label5.TabIndex = 0x49;
            this.label5.Text = "label5";
            this.label6.BackColor = Color.LightCyan;
            this.label6.BorderStyle = BorderStyle.FixedSingle;
            this.label6.Location = new Point(8, 0x70);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x1c8, 1);
            this.label6.TabIndex = 0x48;
            this.label6.Text = "label6";
            this.labCheckNumberBottom.BackColor = Color.LightCyan;
            this.labCheckNumberBottom.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labCheckNumberBottom.Location = new Point(0xe8, 0xb0);
            this.labCheckNumberBottom.Name = "labCheckNumberBottom";
            this.labCheckNumberBottom.Size = new Size(0x2c, 0x10);
            this.labCheckNumberBottom.TabIndex = 0x47;
            this.labCheckNumberBottom.Text = "108";
            this.labBankName.AutoSize = true;
            this.labBankName.BackColor = Color.LightCyan;
            this.labBankName.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labBankName.Location = new Point(0x58, 120);
            this.labBankName.Name = "labBankName";
            this.labBankName.Size = new Size(0x3d, 14);
            this.labBankName.TabIndex = 70;
            this.labBankName.Text = "Olympic Bank";
            this.labAccountNumber.BackColor = Color.LightCyan;
            this.labAccountNumber.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labAccountNumber.Location = new Point(120, 0xb0);
            this.labAccountNumber.Name = "labAccountNumber";
            this.labAccountNumber.Size = new Size(0x66, 0x10);
            this.labAccountNumber.TabIndex = 0x45;
            this.labAccountNumber.Text = "1505 303079";
            this.labelBankNumber.BackColor = Color.LightCyan;
            this.labelBankNumber.Font = new Font("Courier New", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labelBankNumber.Location = new Point(8, 0xb0);
            this.labelBankNumber.Name = "labelBankNumber";
            this.labelBankNumber.Size = new Size(0x66, 0x10);
            this.labelBankNumber.TabIndex = 0x44;
            this.labelBankNumber.Text = "123456789";
            this.label22.AutoSize = true;
            this.label22.BackColor = Color.LightCyan;
            this.label22.Location = new Point(8, 0x94);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x27, 0x10);
            this.label22.TabIndex = 0x43;
            this.label22.Text = "MEMO";
            this.label24.BackColor = Color.LightCyan;
            this.label24.Location = new Point(0x1ac, 0x20);
            this.label24.Name = "label24";
            this.label24.Size = new Size(0x18, 0x10);
            this.label24.TabIndex = 0x42;
            this.label24.Text = "20";
            this.label30.BackColor = Color.LightCyan;
            this.label30.BorderStyle = BorderStyle.FixedSingle;
            this.label30.Location = new Point(0x134, 0x30);
            this.label30.Name = "label30";
            this.label30.Size = new Size(0x74, 1);
            this.label30.TabIndex = 0x41;
            this.label30.Text = "label30";
            this.label31.BackColor = Color.LightCyan;
            this.label31.BorderStyle = BorderStyle.FixedSingle;
            this.label31.Location = new Point(420, 0x58);
            this.label31.Name = "label31";
            this.label31.Size = new Size(0x74, 1);
            this.label31.TabIndex = 0x40;
            this.label31.Text = "label31";
            this.label32.BackColor = Color.LightCyan;
            this.label32.BorderStyle = BorderStyle.FixedSingle;
            this.label32.Location = new Point(0x164, 0xa8);
            this.label32.Name = "label32";
            this.label32.Size = new Size(180, 1);
            this.label32.TabIndex = 0x3f;
            this.label32.Text = "label32";
            this.label33.BackColor = Color.LightCyan;
            this.label33.Location = new Point(0x194, 0x48);
            this.label33.Name = "label33";
            this.label33.Size = new Size(0x10, 0x10);
            this.label33.TabIndex = 0x3e;
            this.label33.Text = "$";
            this.label34.BackColor = Color.LightCyan;
            this.label34.BorderStyle = BorderStyle.FixedSingle;
            this.label34.Location = new Point(0x58, 0x58);
            this.label34.Name = "label34";
            this.label34.Size = new Size(300, 1);
            this.label34.TabIndex = 0x3d;
            this.label34.Text = "label34";
            this.label35.BackColor = Color.LightCyan;
            this.label35.Location = new Point(8, 0x40);
            this.label35.Name = "label35";
            this.label35.Size = new Size(80, 0x18);
            this.label35.TabIndex = 0x3b;
            this.label35.Text = "PAY TO THE ORDER OF";
            this.label37.AutoSize = true;
            this.label37.BackColor = Color.LightCyan;
            this.label37.Location = new Point(8, 0x1c);
            this.label37.Name = "label37";
            this.label37.Size = new Size(0x6d, 0x10);
            this.label37.TabIndex = 0x39;
            this.label37.Text = "SPRINGFIELD, USA";
            this.labPayor.AutoSize = true;
            this.labPayor.BackColor = Color.LightCyan;
            this.labPayor.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPayor.Location = new Point(8, 8);
            this.labPayor.Name = "labPayor";
            this.labPayor.Size = new Size(0x57, 0x11);
            this.labPayor.TabIndex = 0x38;
            this.labPayor.Text = "JOHN Q. DOE";
            this.labSignature.Font = new Font("Monotype Corsiva", 12f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.labSignature.ForeColor = Color.FromArgb(0, 0, 0xc0);
            this.labSignature.Location = new Point(0x16c, 0x98);
            this.labSignature.Name = "labSignature";
            this.labSignature.Size = new Size(0xa4, 20);
            this.labSignature.TabIndex = 0x59;
            this.labSignature.Text = "label11";
            this.labAmountWords.Location = new Point(0x10, 0x60);
            this.labAmountWords.Name = "labAmountWords";
            this.labAmountWords.Size = new Size(440, 20);
            this.labAmountWords.TabIndex = 0x58;
            this.labAmountWords.Text = "label10";
            this.labPayee.Location = new Point(0x58, 0x48);
            this.labPayee.Name = "labPayee";
            this.labPayee.Size = new Size(0x124, 20);
            this.labPayee.TabIndex = 0x57;
            this.labPayee.Text = "label9";
            this.labYear.Font = new Font("Verdana", 9.75f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labYear.ForeColor = Color.Gray;
            this.labYear.Location = new Point(0x1c4, 0x20);
            this.labYear.Name = "labYear";
            this.labYear.Size = new Size(0x1c, 20);
            this.labYear.TabIndex = 0x56;
            this.labYear.Text = "08";
            this.labMonthDay.Location = new Point(0x138, 0x20);
            this.labMonthDay.Name = "labMonthDay";
            this.labMonthDay.Size = new Size(0x6c, 20);
            this.labMonthDay.TabIndex = 0x55;
            this.labMonthDay.Text = "label1";
            this.txtMemo.BackColor = Color.LightCyan;
            this.txtMemo.BorderStyle = BorderStyle.None;
            this.txtMemo.Location = new Point(0x34, 0x94);
            this.txtMemo.Name = "txtMemo";
            this.txtMemo.Size = new Size(0xc0, 13);
            this.txtMemo.TabIndex = 0x52;
            this.txtMemo.Text = "";
            this.BackColor = Color.LightCyan;
            base.Controls.Add(this.panel1);
            base.Name = "CheckControl";
            base.Size = new Size(0x22c, 200);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected string NumeralsToWords(float x) {
            string str;
            int num = (int) Math.Floor((double) x);
            int i = num / 0xf4240;
            int num3 = (num % 0xf4240) / 0x3e8;
            int num4 = num % 0x3e8;
            int num5 = (int) Math.Round((double) ((x - num) * 100f));
            string str2 = "";
            if (i > 0) {
                str = str2;
                string[] textArray1 = new string[] { str, this.NumeralsToWordsHelper(i), " ", A.Resources.GetString("million"), " " };
                str2 = string.Concat(textArray1);
            }
            if (num3 > 0) {
                str = str2;
                string[] textArray2 = new string[] { str, this.NumeralsToWordsHelper(num3), " ", A.Resources.GetString("thousand"), " " };
                str2 = string.Concat(textArray2);
            }
            str = str2 + this.NumeralsToWordsHelper(num4) + " ";
            string[] textArray3 = new string[] { str, A.Resources.GetString("and"), " ", num5.ToString(), "/100" };
            str2 = string.Concat(textArray3);
            if (str2.Length > 1) {
                str2 = str2.Substring(0, 1).ToUpper() + str2.Substring(1);
            }
            return str2;
        }

        protected string NumeralsToWordsHelper(int i) {
            int index = i / 100;
            int num2 = (i % 100) / 10;
            int num3 = i % 10;
            string[] strArray = new string[] { "", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            string[] strArray2 = new string[] { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            string[] strArray3 = new string[] { "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
            string str = "";
            if (index > 0) {
                string str2 = str;
                string[] textArray4 = new string[] { str2, strArray[index], " ", A.Resources.GetString("hundred"), " " };
                str = string.Concat(textArray4);
            }
            if (((i % 100) >= 11) && ((i % 100) <= 0x13)) {
                return (str + strArray3[(i % 100) - 11] + " ");
            }
            if (num2 > 0) {
                str = str + strArray2[num2] + " ";
            }
            if (num3 > 0) {
                str = str + strArray[num3] + " ";
            }
            return str;
        }

        protected void SetFonts(Font font, Color color) {
            this.txtAmount.Font = font;
            this.labPayee.Font = font;
            this.labMonthDay.Font = font;
            this.labYear.Font = font;
            this.labAmountWords.Font = font;
            this.txtMemo.Font = new Font(font.FontFamily, 8f, font.Style);
            this.txtAmount.ForeColor = color;
            this.txtMemo.ForeColor = color;
            this.labPayee.ForeColor = color;
            this.labMonthDay.ForeColor = color;
            this.labYear.ForeColor = color;
            this.labAmountWords.ForeColor = color;
            this.labSignature.ForeColor = color;
        }

        private void txtAmount_TextChanged(object sender, EventArgs e) {
            try {
                float x = float.Parse(this.txtAmount.Text);
                this.labAmountWords.Text = this.NumeralsToWords(x);
            }
            catch (Exception) { }
        }

        public string BankName {
            set {
                this.labBankName.Text = value;
            }
        }

        public Bill Bill {
            set {
                this.bill = value;
                this.txtAmount.Text = this.bill.Amount.ToString("N2");
                if (((this.bill.Account != null) && !(this.bill.Account is InstallmentLoan)) && !(this.bill.Account is CreditCardAccount)) {
                    this.txtAmount.Text = this.bill.Account.EndingBalance().ToString("N2");
                }
                this.labPayee.Text = this.bill.From;
                this.labMonthDay.Text = this.bill.Date.ToString("M");
                this.labYear.Text = this.bill.Date.ToString("yy");
                this.labAmountWords.Text = this.NumeralsToWords(this.bill.Amount);
                this.SetFonts(this.labSignature.Font, this.labSignature.ForeColor);
            }
        }

        public Check Check {
            set {
                this.check = value;
                this.txtAmount.Text = this.check.Amount.ToString("N2");
                this.labAmountWords.Text = this.NumeralsToWords(this.check.Amount);
                this.txtAmount.Enabled = false;
                this.txtMemo.Text = this.check.Memo;
                this.txtMemo.Enabled = false;
                this.labPayee.Text = this.check.Payee;
                this.labMonthDay.Text = this.check.Date.ToString("M");
                this.labYear.Text = this.check.Date.ToString("yy");
                this.labPayor.Text = this.check.Payor;
                this.labSignature.Text = this.check.Signature;
                this.labCheckNumber.Text = this.check.Number.ToString();
                this.labCheckNumberBottom.Text = this.check.Number.ToString();
                this.SetFonts(this.labYear.Font, this.labYear.ForeColor);
            }
        }

        public string Payor {
            set {
                this.labPayor.Text = value;
            }
        }
    }
}

