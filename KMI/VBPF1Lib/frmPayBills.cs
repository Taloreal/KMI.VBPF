using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmPayBills : Form {

        private SortedList bills;
        private Button btnAccept;
        private Button btnBack;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnNext;
        private Container components = null;
        private int currentIndex;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label labNoOffers;
        private Label labPage;
        private Panel panBills;
        private PictureBox picBill;
        protected int TransactionsPerPage = 0x18;
        private NumericUpDown updPage;

        public frmPayBills() {
            this.InitializeComponent();
            this.RefreshData();
            this.currentIndex = 0;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                new frmPayBy(this.currentBill).ShowDialog(this);
                A.MainForm.UpdateView();
                this.RefreshData();
                this.picBill.Refresh();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
            if (this.bills.Count == 0)
            {
                base.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.currentIndex--;
            this.picBill.Refresh();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.currentIndex++;
            this.picBill.Refresh();
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
            this.btnAccept = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.btnBack = new Button();
            this.btnNext = new Button();
            this.picBill = new PictureBox();
            this.labNoOffers = new Label();
            this.label2 = new Label();
            this.panBills = new Panel();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.labPage = new Label();
            this.updPage = new NumericUpDown();
            ((ISupportInitialize) this.picBill).BeginInit();
            this.panBills.SuspendLayout();
            this.updPage.BeginInit();
            base.SuspendLayout();
            this.btnAccept.Location = new Point(0x188, 0x58);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new Size(0x60, 0x18);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "Pay";
            this.btnAccept.Click += new EventHandler(this.btnAccept_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x188, 0xb8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x188, 0xe8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnBack.Location = new Point(120, 0x210);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(0x30, 0x18);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "<<";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnNext.Location = new Point(0xb8, 0x210);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(0x30, 0x18);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = ">>";
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.picBill.BackColor = Color.White;
            this.picBill.BorderStyle = BorderStyle.FixedSingle;
            this.picBill.Location = new Point(0x10, 20);
            this.picBill.Name = "picBill";
            this.picBill.Size = new Size(0x151, 0x1e4);
            this.picBill.TabIndex = 6;
            this.picBill.TabStop = false;
            this.picBill.Visible = false;
            this.picBill.Paint += new PaintEventHandler(this.picBill_Paint);
            this.labNoOffers.Font = new Font("Microsoft Sans Serif", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labNoOffers.ForeColor = Color.Gray;
            this.labNoOffers.Location = new Point(0x18, 0x88);
            this.labNoOffers.Name = "labNoOffers";
            this.labNoOffers.Size = new Size(0x108, 120);
            this.labNoOffers.TabIndex = 7;
            this.labNoOffers.Text = "There are no more bills.";
            this.label2.BackColor = Color.White;
            this.label2.BorderStyle = BorderStyle.FixedSingle;
            this.label2.Location = new Point(20, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x151, 0x1e4);
            this.label2.TabIndex = 8;
            this.label2.Visible = false;
            this.panBills.Controls.Add(this.picBill);
            this.panBills.Controls.Add(this.label2);
            this.panBills.Controls.Add(this.label3);
            this.panBills.Controls.Add(this.label4);
            this.panBills.Controls.Add(this.label5);
            this.panBills.Location = new Point(0, 8);
            this.panBills.Name = "panBills";
            this.panBills.Size = new Size(0x170, 0x1f8);
            this.panBills.TabIndex = 9;
            this.label3.BackColor = Color.White;
            this.label3.BorderStyle = BorderStyle.FixedSingle;
            this.label3.Location = new Point(0x18, 12);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x151, 0x1e4);
            this.label3.TabIndex = 9;
            this.label3.Visible = false;
            this.label4.BackColor = Color.White;
            this.label4.BorderStyle = BorderStyle.FixedSingle;
            this.label4.Location = new Point(0x1c, 8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x151, 0x1e4);
            this.label4.TabIndex = 10;
            this.label4.Visible = false;
            this.label5.BackColor = Color.White;
            this.label5.BorderStyle = BorderStyle.FixedSingle;
            this.label5.Location = new Point(0x20, 4);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x151, 0x1e4);
            this.label5.TabIndex = 11;
            this.label5.Visible = false;
            this.labPage.Location = new Point(0x188, 0x18);
            this.labPage.Name = "labPage";
            this.labPage.Size = new Size(40, 0x10);
            this.labPage.TabIndex = 10;
            this.labPage.Text = "Page:";
            this.updPage.Location = new Point(0x188, 40);
            int[] bits = new int[4];
            bits[0] = 1;
            this.updPage.Minimum = new decimal(bits);
            this.updPage.Name = "updPage";
            this.updPage.Size = new Size(40, 20);
            this.updPage.TabIndex = 11;
            this.updPage.TextAlign = HorizontalAlignment.Right;
            int[] numArray2 = new int[4];
            numArray2[0] = 1;
            this.updPage.Value = new decimal(numArray2);
            this.updPage.ValueChanged += new EventHandler(this.updPage_ValueChanged);
            base.AcceptButton = this.btnAccept;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x202, 560);
            base.Controls.Add(this.updPage);
            base.Controls.Add(this.labPage);
            base.Controls.Add(this.panBills);
            base.Controls.Add(this.btnNext);
            base.Controls.Add(this.btnBack);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnAccept);
            base.Controls.Add(this.labNoOffers);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmPayBills";
            base.ShowInTaskbar = false;
            this.Text = "Pay Bills";
            ((ISupportInitialize) this.picBill).EndInit();
            this.panBills.ResumeLayout(false);
            this.updPage.EndInit();
            base.ResumeLayout(false);
        }

        private void picBill_Paint(object sender, PaintEventArgs e)
        {
            if (((this.bills.Count > 0) && (this.currentIndex >= 0)) && (this.currentIndex < this.bills.Count))
            {
                if ((this.currentBill.Account is CreditCardAccount) || (this.currentBill.Account is InstallmentLoan))
                {
                    int count = this.currentBill.Account.TransactionsForMonth(this.currentBill.Date.Year, this.currentBill.Date.Month).Count;
                    int num2 = (int) Math.Max(1.0, Math.Ceiling((double) (((float) count) / ((float) this.TransactionsPerPage))));
                    this.updPage.Value = Math.Min(this.updPage.Value, num2);
                    this.updPage.Maximum = num2;
                    this.updPage.Visible = true;
                    this.labPage.Visible = true;
                    this.currentBill.Account.PrintPage(((int) this.updPage.Value) - 1, e.Graphics, this.currentBill.Date.Year, this.currentBill.Date.Month, num2, this.TransactionsPerPage);
                }
                else
                {
                    this.currentBill.PrintPage(e.Graphics);
                    this.updPage.Visible = false;
                    this.labPage.Visible = false;
                }
            }
            else if (this.bills.Count == 0)
            {
                this.btnAccept.Enabled = false;
            }
            this.btnBack.Enabled = this.currentIndex > 0;
            this.btnNext.Enabled = this.currentIndex < (this.bills.Count - 1);
        }

        protected void RefreshData()
        {
            this.bills = A.Adapter.GetBills(A.MainForm.CurrentEntityID);
            this.currentIndex = Math.Min(this.currentIndex, this.bills.Count - 1);
            this.panBills.Visible = this.bills.Count > 0;
            this.btnAccept.Enabled = this.bills.Count > 0;
            for (int i = 0; i < this.panBills.Controls.Count; i++)
            {
                this.panBills.Controls[i].Visible = i < this.bills.Count;
            }
        }

        private void updPage_ValueChanged(object sender, EventArgs e)
        {
            this.picBill.Refresh();
        }

        public Bill currentBill
        {
            get
            {
                return (Bill) this.bills.GetByIndex(this.currentIndex);
            }
        }
    }
}

