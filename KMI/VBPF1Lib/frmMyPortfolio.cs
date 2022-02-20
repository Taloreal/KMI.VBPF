namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class frmMyPortfolio : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Container components = null;
        private object[,] d = null;
        public MenuItem EnablingReference;
        private KMIGraph kmiGraph1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label labFundName;
        private Label labTotal;
        private Label labTotalLabel;
        private LinkLabel lnkBuy;
        private SortedList myFunds;
        private Panel panel1;
        private Panel panel7;
        private Panel panFunds;
        private bool Retirement;

        public frmMyPortfolio(bool retirement)
        {
            this.InitializeComponent();
            this.Retirement = retirement;
            if (retirement)
            {
                this.Text = "View Retirement Portfolio";
            }
            this.lnkBuy.Visible = !retirement;
            this.RefreshData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
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
            ResourceManager manager = new ResourceManager(typeof(frmMyPortfolio));
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.panel7 = new Panel();
            this.lnkBuy = new LinkLabel();
            this.panFunds = new Panel();
            this.labTotal = new Label();
            this.labTotalLabel = new Label();
            this.kmiGraph1 = new KMIGraph();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.labFundName = new Label();
            this.label1 = new Label();
            this.panel1 = new Panel();
            this.panel7.SuspendLayout();
            this.panFunds.SuspendLayout();
            base.SuspendLayout();
            this.btnHelp.Location = new Point(0x180, 400);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 7;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.Location = new Point(0x108, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(0x90, 400);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.panel7.BackColor = Color.White;
            this.panel7.BorderStyle = BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.lnkBuy);
            this.panel7.Controls.Add(this.panFunds);
            this.panel7.Controls.Add(this.kmiGraph1);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.label3);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.labFundName);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Location = new Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(0x2b0, 0x178);
            this.panel7.TabIndex = 8;
            this.lnkBuy.Location = new Point(0x98, 0x160);
            this.lnkBuy.Name = "lnkBuy";
            this.lnkBuy.Size = new Size(0x88, 0x18);
            this.lnkBuy.TabIndex = 0x18;
            this.lnkBuy.TabStop = true;
            this.lnkBuy.Text = "Buy Shares";
            this.lnkBuy.TextAlign = ContentAlignment.TopCenter;
            this.lnkBuy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkBuy_LinkClicked);
            this.panFunds.AutoScroll = true;
            this.panFunds.Controls.Add(this.labTotal);
            this.panFunds.Controls.Add(this.labTotalLabel);
            this.panFunds.Location = new Point(8, 0x68);
            this.panFunds.Name = "panFunds";
            this.panFunds.Size = new Size(0x1a8, 240);
            this.panFunds.TabIndex = 0x17;
            this.labTotal.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labTotal.Location = new Point(0x108, 0x98);
            this.labTotal.Name = "labTotal";
            this.labTotal.Size = new Size(0x48, 0x10);
            this.labTotal.TabIndex = 0x16;
            this.labTotal.Text = "Total";
            this.labTotal.TextAlign = ContentAlignment.TopRight;
            this.labTotalLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labTotalLabel.Location = new Point(0xd8, 0x98);
            this.labTotalLabel.Name = "labTotalLabel";
            this.labTotalLabel.Size = new Size(0x38, 0x10);
            this.labTotalLabel.TabIndex = 0x15;
            this.labTotalLabel.Text = "Total";
            this.labTotalLabel.TextAlign = ContentAlignment.TopCenter;
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 9f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.Data = null;
            this.kmiGraph1.DataPointLabelFontSize = 9f;
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.GraphType = 1;
            this.kmiGraph1.GridLinesX = false;
            this.kmiGraph1.GridLinesY = false;
            this.kmiGraph1.Legend = true;
            this.kmiGraph1.LegendFontSize = 7f;
            this.kmiGraph1.LineWidth = 4;
            this.kmiGraph1.Location = new Point(440, 0x98);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(240, 0x70);
            this.kmiGraph1.TabIndex = 20;
            this.kmiGraph1.Title = null;
            this.kmiGraph1.TitleFontSize = 18f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = null;
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.label6.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.label6.Location = new Point(0x130, 0x58);
            this.label6.Name = "label6";
            this.label6.Size = new Size(40, 0x10);
            this.label6.TabIndex = 0x13;
            this.label6.Text = "Value";
            this.label5.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.label5.Location = new Point(0xc0, 0x58);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x58, 0x10);
            this.label5.TabIndex = 0x12;
            this.label5.Text = "Price && Change";
            this.label4.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.label4.Location = new Point(0x90, 0x58);
            this.label4.Name = "label4";
            this.label4.Size = new Size(40, 0x10);
            this.label4.TabIndex = 0x11;
            this.label4.Text = "Shares";
            this.label3.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.label3.Location = new Point(0x18, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x48, 0x10);
            this.label3.TabIndex = 0x10;
            this.label3.Text = "Fund Name";
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(0x1f8, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x60, 0x10);
            this.label2.TabIndex = 15;
            this.label2.Text = "Asset Allocation";
            this.label2.TextAlign = ContentAlignment.TopCenter;
            this.labFundName.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.labFundName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labFundName.Location = new Point(8, 80);
            this.labFundName.Name = "labFundName";
            this.labFundName.Size = new Size(0x1a8, 0x18);
            this.labFundName.TabIndex = 14;
            this.labFundName.TextAlign = ContentAlignment.MiddleLeft;
            this.label1.Image = (Image) manager.GetObject("label1.Image");
            this.label1.Location = new Point(-8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(200, 0x48);
            this.label1.TabIndex = 11;
            this.panel1.BackColor = SystemColors.Highlight;
            this.panel1.Location = new Point(440, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(200, 400);
            this.panel1.TabIndex = 0;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2aa, 440);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.panel7);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmMyPortfolio";
            base.ShowInTaskbar = false;
            this.Text = "View Portfolio";
            this.panel7.ResumeLayout(false);
            this.panFunds.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lnkBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                new frmTrade(this.Retirement, true, null).ShowDialog(this);
                this.RefreshData();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void RefreshData()
        {
            this.panFunds.Controls.Clear();
            this.myFunds = A.Adapter.GetInvestmentAccounts(A.MainForm.CurrentEntityID, this.Retirement);
            int num = 0;
            float amount = 0f;
            Hashtable hashtable = new Hashtable();
            foreach (InvestmentAccount account in this.myFunds.Values)
            {
                FundControl control = new FundControl(account, this.Retirement) {
                    Top = num
                };
                this.panFunds.Controls.Add(control);
                num += control.Height;
                amount += account.Value;
                string categoryName = account.Fund.CategoryName;
                if (hashtable.ContainsKey(categoryName))
                {
                    hashtable[categoryName] = ((float) hashtable[categoryName]) + account.Value;
                }
                else
                {
                    hashtable.Add(categoryName, account.Value);
                }
            }
            if (this.panFunds.Controls.Count > 0)
            {
                this.labTotalLabel.Top = num;
                this.labTotal.Top = num;
                this.labTotal.Text = Utilities.FC(amount, 2, A.Instance.CurrencyConversion);
                this.panFunds.Controls.Add(this.labTotalLabel);
                this.panFunds.Controls.Add(this.labTotal);
            }
            this.kmiGraph1.GraphType = 4;
            this.d = new object[1 + hashtable.Count, 2];
            int num3 = 0;
            foreach (string str2 in hashtable.Keys)
            {
                this.d[1 + num3, 1] = (float) hashtable[str2];
                this.d[1 + num3, 0] = str2;
                num3++;
            }
            this.kmiGraph1.Draw(this.d);
        }
    }
}

