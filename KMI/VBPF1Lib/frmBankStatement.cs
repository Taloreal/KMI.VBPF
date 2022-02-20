namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class frmBankStatement : Form
    {
        protected SortedList bankAccounts;
        private Button btnHelp;
        private Button btnMonthBack;
        private Button btnMonthNext;
        private Button btnPageBack;
        private Button btnPageNext;
        private Button btnPrint;
        private Container components = null;
        protected BankAccount currentAccount;
        protected int currentMonth;
        protected int currentYear;
        protected PrintDocument d = new PrintDocument();
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labNoAccounts;
        private ListBox lstAccount;
        protected int maxMonth;
        protected int maxYear;
        protected int page = 0;
        protected PictureBox picStatement;
        protected int printerPage = 0;
        protected int TransactionsPerPage = 0x1b;
        protected ArrayList transThisMonth;

        public frmBankStatement()
        {
            this.InitializeComponent();
            this.bankAccounts = this.GetAccounts();
            this.maxMonth = A.MainForm.Now.Month;
            this.maxYear = A.MainForm.Now.Year;
            if (this.maxMonth == 1)
            {
                this.maxMonth = 12;
                this.maxYear--;
            }
            else
            {
                this.maxMonth--;
            }
            this.currentMonth = this.maxMonth;
            this.currentYear = this.maxYear;
            foreach (BankAccount account in this.bankAccounts.Values)
            {
                this.lstAccount.Items.Add(account);
            }
            if (this.lstAccount.Items.Count > 0)
            {
                this.lstAccount.SelectedIndex = 0;
                this.picStatement.Visible = true;
            }
            this.btnPrint.Enabled = this.currentAccount != null;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnMonthBack_Click(object sender, EventArgs e)
        {
            if (this.currentMonth == 1)
            {
                this.currentMonth = 12;
                this.currentYear--;
            }
            else
            {
                this.currentMonth--;
            }
            this.picStatement.Refresh();
        }

        private void btnMonthForward_Click(object sender, EventArgs e)
        {
            if (this.currentMonth == 12)
            {
                this.currentMonth = 1;
                this.currentYear++;
            }
            else
            {
                this.currentMonth++;
            }
            this.picStatement.Refresh();
        }

        private void btnPageBack_Click(object sender, EventArgs e)
        {
            this.page--;
            this.picStatement.Refresh();
        }

        private void btnPageNext_Click(object sender, EventArgs e)
        {
            this.page++;
            this.picStatement.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.printerPage = 0;
            Utilities.PrintWithExceptionHandling(this.Text, new PrintPageEventHandler(this.Report_PrintPage));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected virtual SortedList GetAccounts()
        {
            return A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
        }

        private void InitializeComponent()
        {
            this.btnPrint = new Button();
            this.picStatement = new PictureBox();
            this.btnPageNext = new Button();
            this.btnPageBack = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.btnMonthBack = new Button();
            this.btnMonthNext = new Button();
            this.lstAccount = new ListBox();
            this.label3 = new Label();
            this.labNoAccounts = new Label();
            this.btnHelp = new Button();
            ((ISupportInitialize) this.picStatement).BeginInit();
            base.SuspendLayout();
            this.btnPrint.Location = new Point(0x180, 0x180);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x48, 0x18);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.button1_Click);
            this.picStatement.BackColor = Color.White;
            this.picStatement.BorderStyle = BorderStyle.FixedSingle;
            this.picStatement.Location = new Point(0, 0);
            this.picStatement.Name = "picStatement";
            this.picStatement.Size = new Size(360, 0x1d0);
            this.picStatement.TabIndex = 2;
            this.picStatement.TabStop = false;
            this.picStatement.Visible = false;
            this.picStatement.Paint += new PaintEventHandler(this.picStatement_Paint);
            this.btnPageNext.Location = new Point(0x1a8, 320);
            this.btnPageNext.Name = "btnPageNext";
            this.btnPageNext.Size = new Size(0x20, 0x18);
            this.btnPageNext.TabIndex = 3;
            this.btnPageNext.Text = ">>";
            this.btnPageNext.Click += new EventHandler(this.btnPageNext_Click);
            this.btnPageBack.Location = new Point(0x180, 320);
            this.btnPageBack.Name = "btnPageBack";
            this.btnPageBack.Size = new Size(0x20, 0x18);
            this.btnPageBack.TabIndex = 4;
            this.btnPageBack.Text = "<<";
            this.btnPageBack.Click += new EventHandler(this.btnPageBack_Click);
            this.label1.Location = new Point(0x180, 0x128);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x48, 0x10);
            this.label1.TabIndex = 5;
            this.label1.Text = "Page:";
            this.label2.Location = new Point(0x180, 200);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x10);
            this.label2.TabIndex = 8;
            this.label2.Text = "Month:";
            this.btnMonthBack.Location = new Point(0x180, 0xe0);
            this.btnMonthBack.Name = "btnMonthBack";
            this.btnMonthBack.Size = new Size(0x20, 0x18);
            this.btnMonthBack.TabIndex = 7;
            this.btnMonthBack.Text = "<<";
            this.btnMonthBack.Click += new EventHandler(this.btnMonthBack_Click);
            this.btnMonthNext.Location = new Point(0x1a8, 0xe0);
            this.btnMonthNext.Name = "btnMonthNext";
            this.btnMonthNext.Size = new Size(0x20, 0x18);
            this.btnMonthNext.TabIndex = 6;
            this.btnMonthNext.Text = ">>";
            this.btnMonthNext.Click += new EventHandler(this.btnMonthForward_Click);
            this.lstAccount.HorizontalScrollbar = true;
            this.lstAccount.Location = new Point(0x178, 40);
            this.lstAccount.Name = "lstAccount";
            this.lstAccount.Size = new Size(0x58, 0x79);
            this.lstAccount.TabIndex = 9;
            this.lstAccount.SelectedIndexChanged += new EventHandler(this.lstAccount_SelectedIndexChanged);
            this.label3.Location = new Point(0x178, 0x18);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x58, 0x10);
            this.label3.TabIndex = 10;
            this.label3.Text = "Account:";
            this.labNoAccounts.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labNoAccounts.Location = new Point(80, 0xc0);
            this.labNoAccounts.Name = "labNoAccounts";
            this.labNoAccounts.Size = new Size(200, 0x20);
            this.labNoAccounts.TabIndex = 11;
            this.labNoAccounts.Text = "No Accounts Open";
            this.btnHelp.Location = new Point(0x180, 0x1a8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x48, 0x18);
            this.btnHelp.TabIndex = 12;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1e2, 0x1d0);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.lstAccount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.btnMonthBack);
            base.Controls.Add(this.btnMonthNext);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnPageBack);
            base.Controls.Add(this.btnPageNext);
            base.Controls.Add(this.picStatement);
            base.Controls.Add(this.btnPrint);
            base.Controls.Add(this.labNoAccounts);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "frmBankStatement";
            base.ShowInTaskbar = false;
            this.Text = "Bank Statements";
            ((ISupportInitialize) this.picStatement).EndInit();
            base.ResumeLayout(false);
        }

        private void lstAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.currentAccount = (BankAccount) this.lstAccount.SelectedItem;
            this.picStatement.Refresh();
        }

        private void picStatement_Paint(object sender, PaintEventArgs e)
        {
            this.transThisMonth = this.currentAccount.TransactionsForMonth(this.currentYear, this.currentMonth);
            this.btnPageBack.Enabled = this.page > 0;
            this.btnPageNext.Enabled = this.page < (this.Pages - 1);
            this.btnMonthBack.Enabled = (this.currentYear > this.currentAccount.DateOpened.Year) || ((this.currentMonth > this.currentAccount.DateOpened.Month) && (this.currentYear == this.currentAccount.DateOpened.Year));
            this.btnMonthNext.Enabled = (this.currentYear < A.MainForm.Now.Year) || (this.currentMonth < A.MainForm.Now.Month);
            if (!A.MainForm.DesignerMode)
            {
                this.btnMonthNext.Enabled = (this.currentYear < this.maxYear) || (this.currentMonth < this.maxMonth);
            }
            this.PrintPage(this.page, e.Graphics);
        }

        protected virtual bool PrintPage(int page, Graphics g)
        {
            this.currentAccount.PrintPage(page, g, this.currentYear, this.currentMonth, this.Pages, this.TransactionsPerPage);
            return (page < (this.Pages - 1));
        }

        protected void Report_PrintPage(object sender, PrintPageEventArgs e)
        {
            Utilities.ResetFPU();
            e.Graphics.ScaleTransform(2f, 2f);
            e.HasMorePages = this.PrintPage(this.printerPage, e.Graphics);
            this.printerPage++;
        }

        protected int Pages
        {
            get
            {
                if (this.currentAccount == null)
                {
                    return 0;
                }
                return (int) Math.Max(1.0, Math.Ceiling((double) (((float) this.transThisMonth.Count) / ((float) this.TransactionsPerPage))));
            }
        }
    }
}

