namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Windows.Forms;

    public class frmResearchFunds : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboSector;
        private Container components = null;
        private object[,] d = null;
        public MenuItem EnablingReference;
        private ArrayList funds;
        private KMIGraph kmiGraph1;
        private Label lab12b1;
        private Label labBack;
        private Label labChange;
        private Label label1;
        private Label label11;
        private Label label14;
        private Label label16;
        private Label label18;
        private Label label2;
        private Label label20;
        private Label label22;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label9;
        private Label labFront;
        private Label labFundName;
        private Label labNav;
        private Label labPrevious;
        private Label labTER;
        private Label labYield;
        private Label labYTD;
        private LinkLabel lnk1Year;
        private LinkLabel lnk5Year;
        private LinkLabel lnkBuy;
        private LinkLabel lnkExport;
        private ListBox lstFunds;
        private DateTime now;
        private Panel panel7;
        private Panel panel9;
        private float PrimeRate;
        private int yearsToShow = 1;

        public frmResearchFunds(object sender)
        {
            this.InitializeComponent();
            this.funds = A.Adapter.GetFunds();
            this.now = A.Adapter.Now();
            this.PrimeRate = A.Adapter.GetPrimeRate();
            this.cboSector.Items.Add(A.Resources.GetString("{All}"));
            foreach (string str in AppConstants.FundCategories)
            {
                this.cboSector.Items.Add(str);
            }
            this.cboSector.SelectedIndex = 0;
            this.kmiGraph1.TitleFontSize = 9f;
            this.Reset();
            if (sender is string)
            {
                this.lstFunds.SelectedIndex = this.lstFunds.FindStringExact((string) sender);
            }
            else
            {
                this.lstFunds.SelectedIndex = 0;
            }
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

        private void cboSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Reset();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void ExportToExcel(ArrayList lines)
        {
            MessageBox.Show("Your data will be exported to a tab-delimited text file which can be opened with Excel.", "Export Format");
            SaveFileDialog dialog = new SaveFileDialog {
                Filter = "Tab Delimited Text Files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = ".txt"
            };
            if (Simulator.Instance.UserAdminSettings.DefaultDirectory != null)
            {
                dialog.InitialDirectory = Simulator.Instance.UserAdminSettings.DefaultDirectory;
            }
            while (dialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter writer = null;
                try
                {
                    writer = new StreamWriter(dialog.FileName);
                    foreach (string str in lines)
                    {
                        writer.Write(str);
                    }
                    break;
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Could not export to file. File may be read-only or in use by another application.\r\n\r\nError details: " + exception.Message, "Could Not Export");
                }
                finally
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(frmResearchFunds));
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.panel7 = new Panel();
            this.labYield = new Label();
            this.label4 = new Label();
            this.lnkBuy = new LinkLabel();
            this.panel9 = new Panel();
            this.lstFunds = new ListBox();
            this.label5 = new Label();
            this.label7 = new Label();
            this.cboSector = new ComboBox();
            this.lnkExport = new LinkLabel();
            this.labBack = new Label();
            this.label22 = new Label();
            this.labFront = new Label();
            this.label16 = new Label();
            this.lab12b1 = new Label();
            this.label18 = new Label();
            this.labTER = new Label();
            this.label20 = new Label();
            this.label14 = new Label();
            this.labYTD = new Label();
            this.label11 = new Label();
            this.labPrevious = new Label();
            this.label9 = new Label();
            this.labChange = new Label();
            this.label6 = new Label();
            this.labNav = new Label();
            this.label2 = new Label();
            this.labFundName = new Label();
            this.lnk5Year = new LinkLabel();
            this.lnk1Year = new LinkLabel();
            this.label1 = new Label();
            this.kmiGraph1 = new KMIGraph();
            this.panel7.SuspendLayout();
            this.panel9.SuspendLayout();
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
            this.panel7.Controls.Add(this.labYield);
            this.panel7.Controls.Add(this.label4);
            this.panel7.Controls.Add(this.lnkBuy);
            this.panel7.Controls.Add(this.panel9);
            this.panel7.Controls.Add(this.labBack);
            this.panel7.Controls.Add(this.label22);
            this.panel7.Controls.Add(this.labFront);
            this.panel7.Controls.Add(this.label16);
            this.panel7.Controls.Add(this.lab12b1);
            this.panel7.Controls.Add(this.label18);
            this.panel7.Controls.Add(this.labTER);
            this.panel7.Controls.Add(this.label20);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Controls.Add(this.labYTD);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Controls.Add(this.labPrevious);
            this.panel7.Controls.Add(this.label9);
            this.panel7.Controls.Add(this.labChange);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.labNav);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.labFundName);
            this.panel7.Controls.Add(this.lnk5Year);
            this.panel7.Controls.Add(this.lnk1Year);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.kmiGraph1);
            this.panel7.Location = new Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new Size(640, 0x178);
            this.panel7.TabIndex = 8;
            this.labYield.Location = new Point(0x80, 0xd8);
            this.labYield.Name = "labYield";
            this.labYield.Size = new Size(0x38, 0x10);
            this.labYield.TabIndex = 0x25;
            this.labYield.Text = "Net Asset Value:";
            this.labYield.TextAlign = ContentAlignment.TopRight;
            this.label4.Location = new Point(0x10, 0xd8);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x58, 0x10);
            this.label4.TabIndex = 0x24;
            this.label4.Text = "Yield:";
            this.lnkBuy.Location = new Point(0x138, 0x160);
            this.lnkBuy.Name = "lnkBuy";
            this.lnkBuy.Size = new Size(80, 0x10);
            this.lnkBuy.TabIndex = 0x23;
            this.lnkBuy.TabStop = true;
            this.lnkBuy.Text = "Buy Shares";
            this.lnkBuy.TextAlign = ContentAlignment.TopCenter;
            this.lnkBuy.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkBuy_LinkClicked);
            this.panel9.BorderStyle = BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.lstFunds);
            this.panel9.Controls.Add(this.label5);
            this.panel9.Controls.Add(this.label7);
            this.panel9.Controls.Add(this.cboSector);
            this.panel9.Controls.Add(this.lnkExport);
            this.panel9.DockPadding.Bottom = 10;
            this.panel9.DockPadding.Left = 10;
            this.panel9.DockPadding.Right = 10;
            this.panel9.Location = new Point(0x1c8, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new Size(0xb8, 0x176);
            this.panel9.TabIndex = 0x22;
            this.lstFunds.Location = new Point(8, 0x58);
            this.lstFunds.Name = "lstFunds";
            this.lstFunds.Size = new Size(0xa4, 0xee);
            this.lstFunds.Sorted = true;
            this.lstFunds.TabIndex = 0;
            this.lstFunds.SelectedIndexChanged += new EventHandler(this.lstFunds_SelectedIndexChanged_1);
            this.label5.Location = new Point(8, 0x48);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x58, 0x10);
            this.label5.TabIndex = 1;
            this.label5.Text = "Funds:";
            this.label7.Location = new Point(8, 0x10);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x60, 0x10);
            this.label7.TabIndex = 2;
            this.label7.Text = "Fund Category:";
            this.cboSector.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSector.ItemHeight = 13;
            this.cboSector.Location = new Point(8, 0x20);
            this.cboSector.Name = "cboSector";
            this.cboSector.Size = new Size(0xa8, 0x15);
            this.cboSector.TabIndex = 3;
            this.cboSector.SelectedIndexChanged += new EventHandler(this.cboSector_SelectedIndexChanged);
            this.lnkExport.Location = new Point(4, 0x15c);
            this.lnkExport.Name = "lnkExport";
            this.lnkExport.Size = new Size(0xa8, 0x10);
            this.lnkExport.TabIndex = 0x24;
            this.lnkExport.TabStop = true;
            this.lnkExport.Text = "Export Price Histories to Excel";
            this.lnkExport.TextAlign = ContentAlignment.TopCenter;
            this.lnkExport.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkExport_LinkClicked);
            this.labBack.Location = new Point(0x80, 0x158);
            this.labBack.Name = "labBack";
            this.labBack.Size = new Size(0x38, 0x10);
            this.labBack.TabIndex = 0x21;
            this.labBack.Text = "Net Asset Value:";
            this.labBack.TextAlign = ContentAlignment.TopRight;
            this.label22.Location = new Point(0x10, 0x158);
            this.label22.Name = "label22";
            this.label22.Size = new Size(0x58, 0x10);
            this.label22.TabIndex = 0x20;
            this.label22.Text = "Back End Load:";
            this.labFront.Location = new Point(0x80, 320);
            this.labFront.Name = "labFront";
            this.labFront.Size = new Size(0x38, 0x10);
            this.labFront.TabIndex = 0x1f;
            this.labFront.Text = "Net Asset Value:";
            this.labFront.TextAlign = ContentAlignment.TopRight;
            this.label16.Location = new Point(0x10, 320);
            this.label16.Name = "label16";
            this.label16.Size = new Size(0x58, 0x10);
            this.label16.TabIndex = 30;
            this.label16.Text = "Front End Load:";
            this.lab12b1.Location = new Point(0x80, 0x128);
            this.lab12b1.Name = "lab12b1";
            this.lab12b1.Size = new Size(0x38, 0x10);
            this.lab12b1.TabIndex = 0x1d;
            this.lab12b1.Text = "Net Asset Value:";
            this.lab12b1.TextAlign = ContentAlignment.TopRight;
            this.label18.Location = new Point(0x10, 0x128);
            this.label18.Name = "label18";
            this.label18.Size = new Size(0x58, 0x10);
            this.label18.TabIndex = 0x1c;
            this.label18.Text = "Max 12b1 Fee:";
            this.labTER.Location = new Point(0x80, 0x110);
            this.labTER.Name = "labTER";
            this.labTER.Size = new Size(0x38, 0x10);
            this.labTER.TabIndex = 0x1b;
            this.labTER.Text = "Net Asset Value:";
            this.labTER.TextAlign = ContentAlignment.TopRight;
            this.label20.Location = new Point(0x10, 0x110);
            this.label20.Name = "label20";
            this.label20.Size = new Size(0x70, 0x10);
            this.label20.TabIndex = 0x1a;
            this.label20.Text = "Total Expense Ratio:";
            this.label14.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.label14.Location = new Point(0x10, 0xf8);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0xb0, 0x10);
            this.label14.TabIndex = 0x19;
            this.label14.Text = "Fees && Expenses:";
            this.labYTD.Location = new Point(0x80, 0xc0);
            this.labYTD.Name = "labYTD";
            this.labYTD.Size = new Size(0x38, 0x10);
            this.labYTD.TabIndex = 0x16;
            this.labYTD.Text = "Net Asset Value:";
            this.labYTD.TextAlign = ContentAlignment.TopRight;
            this.label11.Location = new Point(0x10, 0xc0);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x58, 0x10);
            this.label11.TabIndex = 0x15;
            this.label11.Text = "YTD Return:";
            this.labPrevious.Location = new Point(0x80, 0xa8);
            this.labPrevious.Name = "labPrevious";
            this.labPrevious.Size = new Size(0x38, 0x10);
            this.labPrevious.TabIndex = 20;
            this.labPrevious.Text = "Net Asset Value:";
            this.labPrevious.TextAlign = ContentAlignment.TopRight;
            this.label9.Location = new Point(0x10, 0xa8);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x58, 0x10);
            this.label9.TabIndex = 0x13;
            this.label9.Text = "Previous Close:";
            this.labChange.Location = new Point(0x80, 0x90);
            this.labChange.Name = "labChange";
            this.labChange.Size = new Size(0x38, 0x10);
            this.labChange.TabIndex = 0x12;
            this.labChange.Text = "Net Asset Value:";
            this.labChange.TextAlign = ContentAlignment.TopRight;
            this.label6.Location = new Point(0x10, 0x90);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x58, 0x10);
            this.label6.TabIndex = 0x11;
            this.label6.Text = "Change:";
            this.labNav.Location = new Point(0x80, 120);
            this.labNav.Name = "labNav";
            this.labNav.Size = new Size(0x38, 0x10);
            this.labNav.TabIndex = 0x10;
            this.labNav.Text = "Net Asset Value:";
            this.labNav.TextAlign = ContentAlignment.TopRight;
            this.label2.Location = new Point(0x10, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x10);
            this.label2.TabIndex = 15;
            this.label2.Text = "Net Asset Value:";
            this.labFundName.BackColor = Color.FromArgb(0xc0, 0xff, 0xff);
            this.labFundName.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labFundName.Location = new Point(0x10, 80);
            this.labFundName.Name = "labFundName";
            this.labFundName.Size = new Size(0x1b0, 0x18);
            this.labFundName.TabIndex = 14;
            this.labFundName.TextAlign = ContentAlignment.MiddleLeft;
            this.lnk5Year.Location = new Point(0x170, 0x148);
            this.lnk5Year.Name = "lnk5Year";
            this.lnk5Year.Size = new Size(0x18, 0x10);
            this.lnk5Year.TabIndex = 13;
            this.lnk5Year.TabStop = true;
            this.lnk5Year.Text = "5 Y";
            this.lnk5Year.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnk1Year_LinkClicked);
            this.lnk1Year.Location = new Point(0x138, 0x148);
            this.lnk1Year.Name = "lnk1Year";
            this.lnk1Year.Size = new Size(0x18, 0x10);
            this.lnk1Year.TabIndex = 12;
            this.lnk1Year.TabStop = true;
            this.lnk1Year.Text = "1 Y";
            this.lnk1Year.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnk1Year_LinkClicked);
            this.label1.Image = (Image) manager.GetObject("label1.Image");
            this.label1.Location = new Point(-8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(200, 0x48);
            this.label1.TabIndex = 11;
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 9f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.BackColor = Color.White;
            this.kmiGraph1.Data = null;
            this.kmiGraph1.DataPointLabelFontSize = 9f;
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.DockPadding.All = 10;
            this.kmiGraph1.GraphType = 1;
            this.kmiGraph1.GridLinesX = true;
            this.kmiGraph1.GridLinesY = true;
            this.kmiGraph1.Legend = false;
            this.kmiGraph1.LegendFontSize = 9f;
            this.kmiGraph1.LineWidth = 4;
            this.kmiGraph1.Location = new Point(0xd0, 0x70);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0xe8, 0xd8);
            this.kmiGraph1.TabIndex = 10;
            this.kmiGraph1.Title = "Fund Performance";
            this.kmiGraph1.TitleFontSize = 18f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = null;
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x2a2, 440);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.panel7);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmResearchFunds";
            base.ShowInTaskbar = false;
            this.Text = "Research Funds";
            this.panel7.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void lnk1Year_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender == this.lnk1Year)
            {
                this.yearsToShow = 1;
            }
            else
            {
                this.yearsToShow = 5;
            }
            this.lstFunds_SelectedIndexChanged_1(new object(), new EventArgs());
        }

        private void lnkBuy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                new frmTrade(false, true, (Fund) this.lstFunds.SelectedItem).ShowDialog(this);
                new frmMyPortfolio(false).ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void lnkExport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool flag = false;
            if (A.MainForm.DesignerMode)
            {
                flag = MessageBox.Show("Annual?", "", MessageBoxButtons.YesNo) == DialogResult.Yes;
            }
            ArrayList lines = new ArrayList();
            string str = "\t";
            string str2 = str;
            foreach (Fund fund in this.lstFunds.Items)
            {
                str2 = str2 + fund.Name + str;
            }
            str2 = str2 + Environment.NewLine;
            lines.Add(str2);
            str2 = "";
            DateTime time = this.now.AddDays(-1825.0);
            for (int i = 0; i < 0x721; i++)
            {
                str2 = str2 + time.AddDays((double) i).ToShortDateString() + str;
                foreach (Fund fund2 in this.lstFunds.Items)
                {
                    str2 = str2 + fund2.sharePrice[((fund2.sharePrice.Count - 0x721) - 1) + i] + str;
                }
                str2 = str2 + Environment.NewLine;
                if (!flag || ((i % 0x16d) == 3))
                {
                    lines.Add(str2);
                }
                str2 = "";
            }
            this.ExportToExcel(lines);
        }

        private void lstFunds_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            int num = this.yearsToShow * 0x16d;
            Fund selectedItem = (Fund) this.lstFunds.SelectedItem;
            this.labFundName.Text = selectedItem.Name;
            this.labNav.Text = selectedItem.Price.ToString("N2");
            this.labPrevious.Text = selectedItem.Previous.ToString("N2");
            float num2 = selectedItem.Price - selectedItem.Previous;
            if (num2 >= 0f)
            {
                this.labChange.Text = "+" + num2.ToString("N2");
                this.labChange.ForeColor = Color.Green;
            }
            else if (num2 < 0f)
            {
                this.labChange.Text = num2.ToString("N2");
                this.labChange.ForeColor = Color.Red;
            }
            float num3 = selectedItem.PriceOn(new DateTime(this.now.Year, 1, 1));
            this.labYTD.Text = Utilities.FP((selectedItem.Price - num3) / num3, 2);
            float dividend = 0f;
            if (selectedItem is MoneyMarketFund)
            {
                dividend = ((this.PrimeRate + ((MoneyMarketFund) selectedItem).DiffToPrime) - selectedItem.TotalExpenseRatio) * selectedItem.Price;
            }
            else
            {
                dividend = selectedItem.Dividend;
            }
            this.labYield.Text = Utilities.FP(dividend / selectedItem.Price, 2);
            this.labTER.Text = Utilities.FP(selectedItem.TotalExpenseRatio, 2);
            this.lab12b1.Text = Utilities.FP(selectedItem.Fees12B1, 2);
            this.labFront.Text = Utilities.FP(selectedItem.FrontEndLoad, 2);
            this.labBack.Text = Utilities.FP(selectedItem.BackEndLoad, 2);
            this.d = new object[2, num + 1];
            float maxValue = float.MaxValue;
            float num6 = 0.01f;
            for (int i = 0; i < num; i++)
            {
                float num9 = (float) selectedItem.sharePrice[(selectedItem.sharePrice.Count - num) + i];
                this.d[1, i + 1] = num9;
                if (num9 < maxValue)
                {
                    maxValue = num9;
                }
                if (num9 > num6)
                {
                    num6 = num9;
                }
            }
            this.kmiGraph1.LineWidth = 1;
            if (num6 == maxValue)
            {
                num6 = maxValue * 1.5f;
                maxValue = 0f;
                this.kmiGraph1.LineWidth = 2;
            }
            this.kmiGraph1.AutoScaleY = false;
            this.kmiGraph1.YMin = maxValue;
            this.kmiGraph1.YMax = num6;
            this.kmiGraph1.YTicks = 4;
            if (this.yearsToShow == 1)
            {
                this.kmiGraph1.XAxisTitle = "1 Year";
            }
            else
            {
                this.kmiGraph1.XAxisTitle = "5 Years";
            }
            this.kmiGraph1.Draw(this.d);
        }

        private void Reset()
        {
            this.lstFunds.Items.Clear();
            foreach (Fund fund in this.funds)
            {
                if ((this.cboSector.SelectedIndex == 0) || (((string) this.cboSector.SelectedItem) == fund.CategoryName))
                {
                    this.lstFunds.Items.Add(fund);
                }
            }
            if (this.lstFunds.Items.Count > 0)
            {
                this.lstFunds.SelectedIndex = 0;
            }
        }
    }
}

