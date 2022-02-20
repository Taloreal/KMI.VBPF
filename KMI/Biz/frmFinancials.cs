namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Windows.Forms;

    public class frmFinancials : Form
    {
        protected ArrayList accountList;
        private Button btnDetail;
        private Button btnExport;
        private Button btnPrint;
        private CheckBox chkShowGrid;
        private Button cmdClose;
        private Button cmdHelp;
        private IContainer components;
        private int currentWeek;
        protected bool detail = false;
        protected MenuItem enablingReference;
        protected GeneralLedger.Frequency frequency = GeneralLedger.Frequency.Weekly;
        private GeneralLedger GL;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox grpData;
        private HScrollBar hScroll;
        private KMIGraph kmiGraph1;
        protected bool loaded;
        private RadioButton optAnnually;
        private RadioButton optBalanceSheet;
        private RadioButton optDollars;
        private RadioButton optGraph;
        private RadioButton optIncomeStatement;
        private RadioButton optQuarterly;
        private RadioButton optTable;
        private RadioButton optUnits;
        private RadioButton optWeekly;
        private Panel panCanvas;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private PictureBox picCanvas;
        protected float rowHeight = 0f;
        private string statementName;
        private ToolTip toolTip1;
        private VScrollBar vScroll;

        public frmFinancials()
        {
            this.InitializeComponent();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            this.detail = !this.detail;
            if (this.detail)
            {
                this.btnDetail.Text = "<< Less Detail";
            }
            else
            {
                this.btnDetail.Text = "More Detail >>";
            }
            this.AccountList = this.GL.AccountList(this.statementName, this.detail);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.GL.PrintToFile(this.AccountList, this.statementName + " for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.currentWeek);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.optTable.Checked)
            {
                this.GL.PrintToPrinter(this.AccountList, this.statementName + " for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.frequency, this.optUnits.Checked);
            }
            else
            {
                this.kmiGraph1.PrintGraph();
            }
        }

        private void chkShowGrid_CheckedChanged(object sender, EventArgs e)
        {
            this.kmiGraph1.GridLinesY = this.chkShowGrid.Checked;
            this.UpdateForm();
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void EntityChangedHandler(object sender, EventArgs e)
        {
            if ((this.enablingReference != null) && !this.enablingReference.Enabled)
            {
                base.Close();
            }
            else if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        private void frmFinancials_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        private void frmFinancials_Load(object sender, EventArgs e)
        {
            if (this.EnablingReference == null)
            {
                throw new Exception("Enabling reference not set in " + this.Text);
            }
            S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged += new EventHandler(this.EntityChangedHandler);
            if (this.GetData())
            {
                this.kmiGraph1.Location = new Point(0, 0);
                this.picCanvas.Location = new Point(0, 0);
                this.optTable.Checked = true;
                this.optDollars.Checked = true;
                this.optWeekly.Checked = true;
                this.optIncomeStatement.Checked = true;
                this.optTableGraph_Click(new object(), new EventArgs());
                this.optDollarsUnits_Click(new object(), new EventArgs());
                this.optFrequency_Click(new object(), new EventArgs());
                this.optStatement_Click(new object(), new EventArgs());
                if (!S.MainForm.IsWin98)
                {
                    this.toolTip1.SetToolTip(this.btnPrint, "Print");
                    this.toolTip1.SetToolTip(this.btnExport, "Export to Excel");
                }
                this.panCanvas_Resize(new object(), new EventArgs());
                this.loaded = true;
                this.UpdateForm();
            }
        }

        protected bool GetData()
        {
            try
            {
                this.currentWeek = S.Adapter.getCurrentWeek();
                this.GL = S.Adapter.GetGL(S.MainForm.CurrentEntityID);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
                return false;
            }
            return true;
        }

        private void hScroll_Scroll(object sender, ScrollEventArgs e)
        {
            this.UpdateForm();
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager = new ResourceManager(typeof(frmFinancials));
            this.cmdClose = new Button();
            this.cmdHelp = new Button();
            this.panel6 = new Panel();
            this.panCanvas = new Panel();
            this.kmiGraph1 = new KMIGraph();
            this.vScroll = new VScrollBar();
            this.hScroll = new HScrollBar();
            this.picCanvas = new PictureBox();
            this.panel4 = new Panel();
            this.btnDetail = new Button();
            this.groupBox2 = new GroupBox();
            this.optUnits = new RadioButton();
            this.optDollars = new RadioButton();
            this.groupBox3 = new GroupBox();
            this.optGraph = new RadioButton();
            this.optTable = new RadioButton();
            this.chkShowGrid = new CheckBox();
            this.groupBox1 = new GroupBox();
            this.optAnnually = new RadioButton();
            this.optQuarterly = new RadioButton();
            this.optWeekly = new RadioButton();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.grpData = new GroupBox();
            this.optBalanceSheet = new RadioButton();
            this.optIncomeStatement = new RadioButton();
            this.panel5 = new Panel();
            this.toolTip1 = new ToolTip(this.components);
            this.panel6.SuspendLayout();
            this.panCanvas.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpData.SuspendLayout();
            this.panel5.SuspendLayout();
            base.SuspendLayout();
            this.cmdClose.Location = new Point(0x10, 0x18);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new Size(0x48, 0x18);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Text = "Close";
            this.cmdClose.Click += new EventHandler(this.cmdClose_Click);
            this.cmdHelp.Location = new Point(0x10, 0x38);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new Size(0x48, 0x18);
            this.cmdHelp.TabIndex = 8;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.Click += new EventHandler(this.cmdHelp_Click);
            this.panel6.Controls.Add(this.panCanvas);
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Dock = DockStyle.Fill;
            this.panel6.Location = new Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x278, 0x1a9);
            this.panel6.TabIndex = 8;
            this.panel6.Resize += new EventHandler(this.panCanvas_Resize);
            this.panCanvas.Controls.Add(this.kmiGraph1);
            this.panCanvas.Controls.Add(this.vScroll);
            this.panCanvas.Controls.Add(this.hScroll);
            this.panCanvas.Controls.Add(this.picCanvas);
            this.panCanvas.Dock = DockStyle.Fill;
            this.panCanvas.Location = new Point(0, 0);
            this.panCanvas.Name = "panCanvas";
            this.panCanvas.Size = new Size(0x278, 0x149);
            this.panCanvas.TabIndex = 10;
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
            this.kmiGraph1.LegendFontSize = 9f;
            this.kmiGraph1.LineWidth = 3;
            this.kmiGraph1.Location = new Point(0x18, 0x10);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0xa8, 0x60);
            this.kmiGraph1.TabIndex = 6;
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
            this.vScroll.LargeChange = 1;
            this.vScroll.Location = new Point(240, 0x58);
            this.vScroll.Maximum = 0;
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new Size(0x10, 0x48);
            this.vScroll.TabIndex = 2;
            this.vScroll.Scroll += new ScrollEventHandler(this.vScroll_Scroll);
            this.hScroll.Location = new Point(0x90, 0xb0);
            this.hScroll.Maximum = 0x19;
            this.hScroll.Name = "hScroll";
            this.hScroll.Size = new Size(0x58, 0x10);
            this.hScroll.TabIndex = 1;
            this.hScroll.Scroll += new ScrollEventHandler(this.hScroll_Scroll);
            this.picCanvas.BackColor = Color.White;
            this.picCanvas.Location = new Point(0x90, 0x60);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new Size(0x58, 0x48);
            this.picCanvas.TabIndex = 0;
            this.picCanvas.TabStop = false;
            this.picCanvas.Paint += new PaintEventHandler(this.picCanvas_Paint);
            this.picCanvas.MouseUp += new MouseEventHandler(this.picCanvas_MouseUp);
            this.picCanvas.MouseMove += new MouseEventHandler(this.picCanvas_MouseMove);
            this.panel4.BorderStyle = BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.btnDetail);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.groupBox3);
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Controls.Add(this.btnExport);
            this.panel4.Controls.Add(this.btnPrint);
            this.panel4.Controls.Add(this.grpData);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = DockStyle.Bottom;
            this.panel4.Location = new Point(0, 0x149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x278, 0x60);
            this.panel4.TabIndex = 0;
            this.btnDetail.Location = new Point(8, 0x40);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new Size(0x90, 0x18);
            this.btnDetail.TabIndex = 6;
            this.btnDetail.Text = "More Detail >>";
            this.btnDetail.Click += new EventHandler(this.btnDetail_Click);
            this.groupBox2.Controls.Add(this.optUnits);
            this.groupBox2.Controls.Add(this.optDollars);
            this.groupBox2.Location = new Point(0x170, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x58, 80);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Units";
            this.optUnits.Location = new Point(8, 40);
            this.optUnits.Name = "optUnits";
            this.optUnits.Size = new Size(0x48, 0x10);
            this.optUnits.TabIndex = 1;
            this.optUnits.Text = "Percents";
            this.optUnits.Click += new EventHandler(this.optDollarsUnits_Click);
            this.optDollars.Location = new Point(8, 0x18);
            this.optDollars.Name = "optDollars";
            this.optDollars.Size = new Size(0x48, 0x10);
            this.optDollars.TabIndex = 0;
            this.optDollars.Text = "Dollars";
            this.optDollars.Click += new EventHandler(this.optDollarsUnits_Click);
            this.groupBox3.Controls.Add(this.optGraph);
            this.groupBox3.Controls.Add(this.optTable);
            this.groupBox3.Controls.Add(this.chkShowGrid);
            this.groupBox3.Location = new Point(0x108, 7);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new Size(0x60, 80);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Show As";
            this.optGraph.Location = new Point(0x10, 40);
            this.optGraph.Name = "optGraph";
            this.optGraph.Size = new Size(0x48, 0x10);
            this.optGraph.TabIndex = 1;
            this.optGraph.Text = "Graph";
            this.optGraph.Click += new EventHandler(this.optTableGraph_Click);
            this.optTable.Location = new Point(0x10, 0x18);
            this.optTable.Name = "optTable";
            this.optTable.Size = new Size(0x48, 0x10);
            this.optTable.TabIndex = 0;
            this.optTable.Text = "Table";
            this.optTable.Click += new EventHandler(this.optTableGraph_Click);
            this.chkShowGrid.Location = new Point(40, 0x38);
            this.chkShowGrid.Name = "chkShowGrid";
            this.chkShowGrid.Size = new Size(0x30, 0x10);
            this.chkShowGrid.TabIndex = 2;
            this.chkShowGrid.Text = "Grid";
            this.chkShowGrid.CheckedChanged += new EventHandler(this.chkShowGrid_CheckedChanged);
            this.groupBox1.Controls.Add(this.optAnnually);
            this.groupBox1.Controls.Add(this.optQuarterly);
            this.groupBox1.Controls.Add(this.optWeekly);
            this.groupBox1.Location = new Point(160, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x60, 80);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Frequency";
            this.optAnnually.Location = new Point(0x10, 0x38);
            this.optAnnually.Name = "optAnnually";
            this.optAnnually.Size = new Size(0x48, 0x10);
            this.optAnnually.TabIndex = 2;
            this.optAnnually.Text = "Annually";
            this.optAnnually.Click += new EventHandler(this.optFrequency_Click);
            this.optQuarterly.Location = new Point(0x10, 40);
            this.optQuarterly.Name = "optQuarterly";
            this.optQuarterly.Size = new Size(0x48, 0x10);
            this.optQuarterly.TabIndex = 1;
            this.optQuarterly.Text = "Quarterly";
            this.optQuarterly.Click += new EventHandler(this.optFrequency_Click);
            this.optWeekly.Location = new Point(0x10, 0x18);
            this.optWeekly.Name = "optWeekly";
            this.optWeekly.Size = new Size(0x48, 0x10);
            this.optWeekly.TabIndex = 0;
            this.optWeekly.Text = "Weekly";
            this.optWeekly.Click += new EventHandler(this.optFrequency_Click);
            this.btnExport.Image = (Image) manager.GetObject("btnExport.Image");
            this.btnExport.Location = new Point(0x1d8, 0x38);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(0x20, 0x20);
            this.btnExport.TabIndex = 5;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnPrint.Image = (Image) manager.GetObject("btnPrint.Image");
            this.btnPrint.Location = new Point(0x1d8, 0x10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x20, 0x20);
            this.btnPrint.TabIndex = 4;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.grpData.Controls.Add(this.optBalanceSheet);
            this.grpData.Controls.Add(this.optIncomeStatement);
            this.grpData.Location = new Point(8, 8);
            this.grpData.Name = "grpData";
            this.grpData.Size = new Size(0x90, 0x34);
            this.grpData.TabIndex = 0;
            this.grpData.TabStop = false;
            this.grpData.Text = "Statement";
            this.optBalanceSheet.Location = new Point(0x10, 0x20);
            this.optBalanceSheet.Name = "optBalanceSheet";
            this.optBalanceSheet.Size = new Size(120, 0x10);
            this.optBalanceSheet.TabIndex = 1;
            this.optBalanceSheet.Text = "Balance Sheet";
            this.optBalanceSheet.Click += new EventHandler(this.optStatement_Click);
            this.optIncomeStatement.Location = new Point(0x10, 0x10);
            this.optIncomeStatement.Name = "optIncomeStatement";
            this.optIncomeStatement.Size = new Size(120, 0x10);
            this.optIncomeStatement.TabIndex = 0;
            this.optIncomeStatement.Text = "Income Statement";
            this.optIncomeStatement.Click += new EventHandler(this.optStatement_Click);
            this.panel5.Controls.Add(this.cmdClose);
            this.panel5.Controls.Add(this.cmdHelp);
            this.panel5.Dock = DockStyle.Right;
            this.panel5.Location = new Point(0x20e, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x68, 0x5e);
            this.panel5.TabIndex = 7;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x278, 0x1a9);
            base.Controls.Add(this.panel6);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new Size(640, 300);
            base.Name = "frmFinancials";
            base.ShowInTaskbar = false;
            this.Text = "Financials";
            base.Load += new EventHandler(this.frmFinancials_Load);
            base.Closed += new EventHandler(this.frmFinancials_Closed);
            this.panel6.ResumeLayout(false);
            this.panCanvas.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.grpData.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.UpdateForm();
                this.hScroll.Value = this.hScroll.Maximum - (this.hScroll.LargeChange - 1);
                this.panCanvas.Refresh();
            }
        }

        private void optDollarsUnits_Click(object sender, EventArgs e)
        {
            this.UpdateForm();
        }

        private void optFrequency_Click(object sender, EventArgs e)
        {
            if (this.optWeekly.Checked)
            {
                this.frequency = GeneralLedger.Frequency.Weekly;
            }
            if (this.optQuarterly.Checked)
            {
                this.frequency = GeneralLedger.Frequency.Quarterly;
            }
            if (this.optAnnually.Checked)
            {
                this.frequency = GeneralLedger.Frequency.Annually;
            }
            this.UpdateForm();
        }

        private void optStatement_Click(object sender, EventArgs e)
        {
            if (this.optIncomeStatement.Checked)
            {
                this.statementName = this.optIncomeStatement.Text;
            }
            if (this.optBalanceSheet.Checked)
            {
                this.statementName = this.optBalanceSheet.Text;
            }
            if (this.optGraph.Checked)
            {
                this.AccountList = this.GL.AccountListForGraphing(this.statementName);
            }
            else
            {
                this.AccountList = this.GL.AccountList(this.statementName, this.detail);
            }
        }

        private void optTableGraph_Click(object sender, EventArgs e)
        {
            this.chkShowGrid.Enabled = this.optGraph.Checked;
            this.kmiGraph1.Visible = this.optGraph.Checked;
            this.picCanvas.Visible = this.optTable.Checked;
            this.btnDetail.Enabled = this.optTable.Checked;
            this.optStatement_Click(new object(), new EventArgs());
        }

        private void panCanvas_Resize(object sender, EventArgs e)
        {
            this.kmiGraph1.Size = this.panCanvas.Size;
            this.picCanvas.Width = this.panCanvas.Width - this.vScroll.Width;
            this.picCanvas.Height = this.panCanvas.Height - this.hScroll.Height;
            this.hScroll.Width = this.panCanvas.Width - this.vScroll.Width;
            this.hScroll.Location = new Point(0, this.picCanvas.Height);
            this.vScroll.Height = this.panCanvas.Height - this.hScroll.Height;
            this.vScroll.Location = new Point(this.picCanvas.Width, 0);
            this.UpdateForm();
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.rowHeight == 0f)
            {
                this.rowHeight = this.GL.RowHeight(this.picCanvas.CreateGraphics());
            }
            int num = ((int) ((((float) (e.Y - 40)) / this.rowHeight) - 1f)) + this.vScroll.Value;
            if ((num < this.AccountList.Count) && (num >= 0))
            {
                this.picCanvas.Cursor = Cursors.Hand;
                if (!S.MainForm.IsWin98)
                {
                    this.toolTip1.SetToolTip(this.picCanvas, "Show Definition");
                }
            }
            else
            {
                this.picCanvas.Cursor = Cursors.Default;
                if (!S.MainForm.IsWin98)
                {
                    this.toolTip1.SetToolTip(this.picCanvas, "");
                }
            }
        }

        private void picCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.rowHeight == 0f)
            {
                this.rowHeight = this.GL.RowHeight(this.picCanvas.CreateGraphics());
            }
            int num = ((int) ((((float) (e.Y - 40)) / this.rowHeight) - 1f)) + this.vScroll.Value;
            if ((num < this.AccountList.Count) && (num >= 0))
            {
                KMIHelp.OpenDefinitions(((GeneralLedger.Account) this.AccountList[num]).Name);
            }
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            int num = Math.Min(this.currentWeek, GeneralLedger.WeeksOfFinancialHistory - 1) / GeneralLedger.WeeksPerPeriod[(int) this.frequency];
            int count = this.AccountList.Count;
            int num3 = this.GL.MaxPrintingColumns(this.picCanvas.ClientSize.Width, this.AccountList, e.Graphics);
            int num4 = this.GL.MaxPrintingRows(this.picCanvas.ClientSize.Height, e.Graphics);
            int num5 = (GeneralLedger.WeeksOfFinancialHistory - 1) / GeneralLedger.WeeksPerPeriod[(int) this.frequency];
            int num6 = this.currentWeek / GeneralLedger.WeeksPerPeriod[(int) this.frequency];
            this.hScroll.Minimum = Math.Max(0, num6 - num5);
            this.hScroll.Maximum = this.hScroll.Minimum + Math.Max(0, (num - num3) + (this.hScroll.LargeChange - 1));
            this.vScroll.Maximum = Math.Max(0, (count - num4) + (this.vScroll.LargeChange - 1));
            int endPeriod = Math.Min((int) ((this.hScroll.Value + num3) - 1), (int) ((this.hScroll.Minimum + num) - 1));
            int endRow = Math.Min((int) ((this.vScroll.Value + num4) - 1), (int) (count - 1));
            this.GL.PrintToScreen(this.AccountList, this.statementName + " for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.vScroll.Value, endRow, this.hScroll.Value, endPeriod, this.frequency, this.optUnits.Checked, e.Graphics);
        }

        private void UpdateForm()
        {
            try
            {
                if (this.loaded)
                {
                    if (this.optTable.Checked)
                    {
                        this.picCanvas.Refresh();
                    }
                    else
                    {
                        this.GL.Graph(this.AccountList, this.statementName + " for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), false, this.optUnits.Checked, this.currentWeek, this.kmiGraph1);
                    }
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void vScroll_Scroll(object sender, ScrollEventArgs e)
        {
            this.UpdateForm();
        }

        protected ArrayList AccountList
        {
            get
            {
                return this.accountList;
            }
            set
            {
                this.accountList = value;
                this.vScroll.Value = 0;
                this.hScroll.Value = this.hScroll.Minimum;
                this.UpdateForm();
            }
        }

        public MenuItem EnablingReference
        {
            get
            {
                return this.enablingReference;
            }
            set
            {
                this.enablingReference = value;
            }
        }
    }
}

