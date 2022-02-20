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

    public class frmProductReport : Form
    {
        protected ArrayList accountList;
        private Button btnCheckAll;
        private Button btnExport;
        private Button btnPrint;
        private Button btnUncheckAll;
        private CheckBox chkData;
        private CheckBox chkHalf1;
        private CheckBox chkHalf2;
        private CheckBox chkProduct;
        private CheckBox chkShowGrid;
        private Button cmdClose;
        private Button cmdHelp;
        private IContainer components;
        private int currentWeek;
        protected ArrayList dataSeriesNames = new ArrayList();
        protected MenuItem enablingReference;
        protected GeneralLedger.Frequency frequency = GeneralLedger.Frequency.Weekly;
        private GeneralLedger GL;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox grpData;
        private HScrollBar hScroll;
        private KMIGraph kmiGraph1;
        protected bool loaded;
        private RadioButton optDollars;
        private RadioButton optGraph;
        private RadioButton optTable;
        private RadioButton optUnits;
        private Panel panCanvas;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Panel panel5;
        private Panel panel6;
        private Panel panProduct;
        private PictureBox picCanvas;
        protected ArrayList productNames = new ArrayList();
        protected int rowHeight = 0;
        protected bool suppressUpdates = false;
        private ToolTip toolTip1;
        protected bool units;
        private VScrollBar vScroll;

        public frmProductReport()
        {
            this.InitializeComponent();
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.panProduct.Controls)
            {
                box.Checked = true;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.GL.PrintToFile(this.AccountList, "Product Report for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.currentWeek);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (this.optTable.Checked)
            {
                this.GL.PrintToPrinter(this.AccountList, "Product Report for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), GeneralLedger.Frequency.Weekly, false);
            }
            else
            {
                this.kmiGraph1.PrintGraph();
            }
        }

        private void btnUncheckAll_Click(object sender, EventArgs e)
        {
            foreach (CheckBox box in this.panProduct.Controls)
            {
                box.Checked = false;
            }
        }

        protected void CheckHalf(int half, bool check)
        {
            int num;
            if (check)
            {
                this.chkProduct.Checked = false;
                if (half == 0)
                {
                    this.chkHalf2.Checked = false;
                }
                if (half == 1)
                {
                    this.chkHalf1.Checked = false;
                }
            }
            this.suppressUpdates = true;
            if (half == 0)
            {
                for (num = 0; num < (this.panProduct.Controls.Count / 2); num++)
                {
                    ((CheckBox) this.panProduct.Controls[num]).Checked = check;
                }
            }
            else
            {
                for (num = this.panProduct.Controls.Count / 2; num < this.panProduct.Controls.Count; num++)
                {
                    ((CheckBox) this.panProduct.Controls[num]).Checked = check;
                }
            }
            this.suppressUpdates = false;
            this.UpdateForm();
        }

        private void chkData_CheckedChanged(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            foreach (CheckBox box in this.grpData.Controls)
            {
                if (box.Checked)
                {
                    list.Add(box.Text);
                }
            }
            this.DataSeriesNames = list;
            this.AccountList = this.GL.AccountList(this.DataSeriesNames, this.ProductNames, this.Units);
        }

        private void chkHalf1_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckHalf(0, this.chkHalf1.Checked);
        }

        private void chkHalf2_CheckedChanged(object sender, EventArgs e)
        {
            this.CheckHalf(1, this.chkHalf2.Checked);
        }

        private void chkProduct_CheckedChanged(object sender, EventArgs e)
        {
            ArrayList list = new ArrayList();
            if (this.chkProduct.Checked)
            {
                list.Add("Total");
            }
            foreach (CheckBox box in this.panProduct.Controls)
            {
                if (box.Checked)
                {
                    list.Add(box.Text);
                }
            }
            this.ProductNames = list;
            this.AccountList = this.GL.AccountList(this.DataSeriesNames, this.ProductNames, this.Units);
        }

        private void chkProduct_Click(object sender, EventArgs e)
        {
            if (this.chkProduct.Checked)
            {
                this.chkHalf2.Checked = false;
                this.chkHalf1.Checked = false;
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

        private void frmSales_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        private void frmSales_Load(object sender, EventArgs e)
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
                this.optGraph.Checked = true;
                this.optDollars.Checked = true;
                this.optTableGraph_Click(new object(), new EventArgs());
                this.optDollarsUnits_Click(new object(), new EventArgs());
                if (!S.MainForm.IsWin98)
                {
                    this.toolTip1.SetToolTip(this.btnPrint, "Print");
                    this.toolTip1.SetToolTip(this.btnExport, "Export to Excel");
                }
                object[] args = new object[] { 1, this.GL.ProductNames.Length / 2 };
                this.chkHalf1.Text = S.Resources.GetString("Products {0}-{1}", args);
                object[] objArray2 = new object[] { (this.GL.ProductNames.Length / 2) + 1, this.GL.ProductNames.Length };
                this.chkHalf2.Text = S.Resources.GetString("Products {0}-{1}", objArray2);
                int num = 0;
                foreach (string str in this.GL.ProductNames)
                {
                    CheckBox box = new CheckBox {
                        Size = this.chkProduct.Size,
                        Left = this.chkProduct.Left,
                        Top = num * 13,
                        Font = this.panProduct.Font
                    };
                    this.panProduct.Controls.Add(box);
                    box.Text = str;
                    box.CheckedChanged += new EventHandler(this.chkProduct_CheckedChanged);
                    num++;
                }
                ArrayList productAccountBaseNames = this.GL.ProductAccountBaseNames;
                this.grpData.Controls.Clear();
                for (int i = 0; i < productAccountBaseNames.Count; i++)
                {
                    CheckBox box2 = new CheckBox {
                        Size = this.chkData.Size,
                        Left = this.chkData.Left + ((i / 3) * this.chkData.Width),
                        Top = ((i % 3) * 0x12) + this.chkData.Top
                    };
                    this.grpData.Controls.Add(box2);
                    box2.Text = (string) productAccountBaseNames[i];
                    box2.CheckedChanged += new EventHandler(this.chkData_CheckedChanged);
                }
                ((CheckBox) this.grpData.Controls[0]).Checked = true;
                this.chkProduct.Checked = true;
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
            ResourceManager manager = new ResourceManager(typeof(frmProductReport));
            this.cmdClose = new Button();
            this.cmdHelp = new Button();
            this.panel6 = new Panel();
            this.panCanvas = new Panel();
            this.kmiGraph1 = new KMIGraph();
            this.vScroll = new VScrollBar();
            this.hScroll = new HScrollBar();
            this.picCanvas = new PictureBox();
            this.panel2 = new Panel();
            this.panProduct = new Panel();
            this.panel3 = new Panel();
            this.chkProduct = new CheckBox();
            this.chkHalf2 = new CheckBox();
            this.chkHalf1 = new CheckBox();
            this.btnUncheckAll = new Button();
            this.btnCheckAll = new Button();
            this.panel4 = new Panel();
            this.groupBox1 = new GroupBox();
            this.optUnits = new RadioButton();
            this.optDollars = new RadioButton();
            this.btnExport = new Button();
            this.btnPrint = new Button();
            this.groupBox2 = new GroupBox();
            this.optGraph = new RadioButton();
            this.optTable = new RadioButton();
            this.chkShowGrid = new CheckBox();
            this.grpData = new GroupBox();
            this.chkData = new CheckBox();
            this.panel5 = new Panel();
            this.toolTip1 = new ToolTip(this.components);
            this.panel6.SuspendLayout();
            this.panCanvas.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpData.SuspendLayout();
            this.panel5.SuspendLayout();
            base.SuspendLayout();
            this.cmdClose.Location = new Point(0x10, 0x18);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new Size(0x48, 0x18);
            this.cmdClose.TabIndex = 0;
            this.cmdClose.Text = "Close";
            this.cmdClose.Click += new EventHandler(this.cmdClose_Click);
            this.cmdHelp.Location = new Point(0x10, 0x38);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new Size(0x48, 0x18);
            this.cmdHelp.TabIndex = 1;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.Click += new EventHandler(this.cmdHelp_Click);
            this.panel6.Controls.Add(this.panCanvas);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Controls.Add(this.panel4);
            this.panel6.Dock = DockStyle.Fill;
            this.panel6.Location = new Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new Size(0x278, 0x1be);
            this.panel6.TabIndex = 8;
            this.panel6.Resize += new EventHandler(this.panCanvas_Resize);
            this.panCanvas.Controls.Add(this.kmiGraph1);
            this.panCanvas.Controls.Add(this.vScroll);
            this.panCanvas.Controls.Add(this.hScroll);
            this.panCanvas.Controls.Add(this.picCanvas);
            this.panCanvas.Dock = DockStyle.Fill;
            this.panCanvas.Location = new Point(0x90, 0);
            this.panCanvas.Name = "panCanvas";
            this.panCanvas.Size = new Size(0x1e8, 350);
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
            this.vScroll.Location = new Point(240, 0x58);
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new Size(0x10, 0x48);
            this.vScroll.TabIndex = 2;
            this.vScroll.Scroll += new ScrollEventHandler(this.vScroll_Scroll);
            this.hScroll.Location = new Point(0x90, 0xb0);
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
            this.panel2.BorderStyle = BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panProduct);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = DockStyle.Left;
            this.panel2.Location = new Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x90, 350);
            this.panel2.TabIndex = 9;
            this.panProduct.AutoScroll = true;
            this.panProduct.Dock = DockStyle.Fill;
            this.panProduct.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.panProduct.Location = new Point(0, 0x40);
            this.panProduct.Name = "panProduct";
            this.panProduct.Size = new Size(0x8e, 0x11c);
            this.panProduct.TabIndex = 1;
            this.panProduct.Paint += new PaintEventHandler(this.panProduct_Paint);
            this.panel3.BackColor = SystemColors.Control;
            this.panel3.Controls.Add(this.chkProduct);
            this.panel3.Controls.Add(this.chkHalf2);
            this.panel3.Controls.Add(this.chkHalf1);
            this.panel3.Controls.Add(this.btnUncheckAll);
            this.panel3.Controls.Add(this.btnCheckAll);
            this.panel3.Dock = DockStyle.Top;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x8e, 0x40);
            this.panel3.TabIndex = 0;
            this.chkProduct.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.chkProduct.Location = new Point(8, 8);
            this.chkProduct.Name = "chkProduct";
            this.chkProduct.Size = new Size(0x70, 0x10);
            this.chkProduct.TabIndex = 9;
            this.chkProduct.Text = "Total";
            this.chkProduct.Click += new EventHandler(this.chkProduct_Click);
            this.chkProduct.CheckedChanged += new EventHandler(this.chkProduct_CheckedChanged);
            this.chkHalf2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.chkHalf2.Location = new Point(8, 0x2c);
            this.chkHalf2.Name = "chkHalf2";
            this.chkHalf2.Size = new Size(0x70, 0x10);
            this.chkHalf2.TabIndex = 6;
            this.chkHalf2.Tag = "10";
            this.chkHalf2.Text = "Products Half 2";
            this.chkHalf2.CheckedChanged += new EventHandler(this.chkHalf2_CheckedChanged);
            this.chkHalf1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.chkHalf1.Location = new Point(8, 0x1c);
            this.chkHalf1.Name = "chkHalf1";
            this.chkHalf1.Size = new Size(0x70, 0x10);
            this.chkHalf1.TabIndex = 5;
            this.chkHalf1.Tag = "0";
            this.chkHalf1.Text = "Products Half 1";
            this.chkHalf1.CheckedChanged += new EventHandler(this.chkHalf1_CheckedChanged);
            this.btnUncheckAll.Location = new Point(0xc0, 0x20);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new Size(0x48, 0x18);
            this.btnUncheckAll.TabIndex = 4;
            this.btnUncheckAll.Text = "Uncheck All";
            this.btnUncheckAll.Visible = false;
            this.btnUncheckAll.Click += new EventHandler(this.btnUncheckAll_Click);
            this.btnCheckAll.Location = new Point(0xc0, 0x20);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new Size(0x48, 0x18);
            this.btnCheckAll.TabIndex = 3;
            this.btnCheckAll.Text = "Check All";
            this.btnCheckAll.Visible = false;
            this.btnCheckAll.Click += new EventHandler(this.btnCheckAll_Click);
            this.panel4.BorderStyle = BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Controls.Add(this.btnExport);
            this.panel4.Controls.Add(this.btnPrint);
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Controls.Add(this.grpData);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = DockStyle.Bottom;
            this.panel4.Location = new Point(0, 350);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(0x278, 0x60);
            this.panel4.TabIndex = 0;
            this.groupBox1.Controls.Add(this.optUnits);
            this.groupBox1.Controls.Add(this.optDollars);
            this.groupBox1.Location = new Point(0x188, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x58, 80);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Units";
            this.optUnits.Location = new Point(8, 40);
            this.optUnits.Name = "optUnits";
            this.optUnits.Size = new Size(0x38, 0x10);
            this.optUnits.TabIndex = 1;
            this.optUnits.Text = "Units";
            this.optUnits.Click += new EventHandler(this.optDollarsUnits_Click);
            this.optDollars.Location = new Point(8, 0x18);
            this.optDollars.Name = "optDollars";
            this.optDollars.Size = new Size(0x40, 0x10);
            this.optDollars.TabIndex = 0;
            this.optDollars.Text = "Dollars";
            this.optDollars.Click += new EventHandler(this.optDollarsUnits_Click);
            this.btnExport.Image = (Image) manager.GetObject("btnExport.Image");
            this.btnExport.Location = new Point(0x1e8, 0x38);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(0x20, 0x20);
            this.btnExport.TabIndex = 4;
            this.btnExport.Click += new EventHandler(this.btnExport_Click);
            this.btnPrint.Image = (Image) manager.GetObject("btnPrint.Image");
            this.btnPrint.Location = new Point(0x1e8, 0x10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x20, 0x20);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.groupBox2.Controls.Add(this.optGraph);
            this.groupBox2.Controls.Add(this.optTable);
            this.groupBox2.Controls.Add(this.chkShowGrid);
            this.groupBox2.Location = new Point(0x120, 8);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x60, 80);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Show As";
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
            this.grpData.Controls.Add(this.chkData);
            this.grpData.Location = new Point(8, 8);
            this.grpData.Name = "grpData";
            this.grpData.Size = new Size(0x110, 80);
            this.grpData.TabIndex = 0;
            this.grpData.TabStop = false;
            this.grpData.Text = "Data";
            this.chkData.Location = new Point(8, 20);
            this.chkData.Name = "chkData";
            this.chkData.Size = new Size(0x80, 0x10);
            this.chkData.TabIndex = 1;
            this.chkData.Text = "Sales";
            this.panel5.Controls.Add(this.cmdClose);
            this.panel5.Controls.Add(this.cmdHelp);
            this.panel5.Dock = DockStyle.Right;
            this.panel5.Location = new Point(0x20e, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new Size(0x68, 0x5e);
            this.panel5.TabIndex = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x278, 0x1be);
            base.Controls.Add(this.panel6);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new Size(640, 300);
            base.Name = "frmProductReport";
            base.ShowInTaskbar = false;
            this.Text = "Products";
            base.Load += new EventHandler(this.frmSales_Load);
            base.Closed += new EventHandler(this.frmSales_Closed);
            this.panel6.ResumeLayout(false);
            this.panCanvas.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
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
            this.Units = this.optUnits.Checked;
        }

        private void optTableGraph_Click(object sender, EventArgs e)
        {
            this.chkShowGrid.Enabled = this.optGraph.Checked;
            this.kmiGraph1.Visible = this.optGraph.Checked;
            this.picCanvas.Visible = this.optTable.Checked;
            this.UpdateForm();
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

        private void panProduct_Paint(object sender, PaintEventArgs e)
        {
        }

        private void picCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.rowHeight == 0)
            {
                this.rowHeight = (int) this.GL.RowHeight(this.picCanvas.CreateGraphics());
            }
            int num = ((e.Y - 40) / this.rowHeight) - 1;
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
            if (this.rowHeight == 0)
            {
                this.rowHeight = (int) this.GL.RowHeight(this.picCanvas.CreateGraphics());
            }
            int num = ((e.Y - 40) / this.rowHeight) - 1;
            if ((num < this.AccountList.Count) && (num >= 0))
            {
                string name = ((GeneralLedger.Account) this.AccountList[num]).Name;
                KMIHelp.OpenDefinitions(name.Substring(0, name.IndexOf(" -")));
            }
        }

        private void picCanvas_Paint(object sender, PaintEventArgs e)
        {
            int num = Math.Min(this.currentWeek, GeneralLedger.WeeksOfProductHistory - 1) / GeneralLedger.WeeksPerPeriod[(int) this.frequency];
            int count = this.AccountList.Count;
            int num3 = this.GL.MaxPrintingColumns(this.picCanvas.ClientSize.Width, this.AccountList, e.Graphics);
            int num4 = this.GL.MaxPrintingRows(this.picCanvas.ClientSize.Height, e.Graphics);
            this.hScroll.Minimum = Math.Max(0, this.currentWeek - (GeneralLedger.WeeksOfProductHistory - 1));
            this.hScroll.Maximum = this.hScroll.Minimum + Math.Max(0, (num - num3) + (this.hScroll.LargeChange - 1));
            this.vScroll.Maximum = Math.Max(0, (count - num4) + (this.vScroll.LargeChange - 1));
            int endPeriod = Math.Min((int) ((this.hScroll.Value + num3) - 1), (int) ((this.hScroll.Minimum + num) - 1));
            int endRow = Math.Min((int) ((this.vScroll.Value + num4) - 1), (int) (count - 1));
            this.GL.PrintToScreen(this.AccountList, "Product Report for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.vScroll.Value, endRow, this.hScroll.Value, endPeriod, GeneralLedger.Frequency.Weekly, false, e.Graphics);
        }

        private void UpdateForm()
        {
            try
            {
                if (this.loaded && !this.suppressUpdates)
                {
                    if (this.optTable.Checked)
                    {
                        this.picCanvas.Refresh();
                    }
                    else
                    {
                        this.GL.Graph(this.AccountList, "Product Report for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID), this.units, false, this.currentWeek, this.kmiGraph1);
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

        protected ArrayList DataSeriesNames
        {
            get
            {
                return this.dataSeriesNames;
            }
            set
            {
                this.dataSeriesNames = value;
                this.AccountList = this.GL.AccountList(this.dataSeriesNames, this.productNames, this.units);
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

        protected ArrayList ProductNames
        {
            get
            {
                return this.productNames;
            }
            set
            {
                this.productNames = value;
                this.AccountList = this.GL.AccountList(this.dataSeriesNames, this.productNames, this.units);
            }
        }

        protected bool Units
        {
            get
            {
                return this.units;
            }
            set
            {
                this.units = value;
                this.AccountList = this.GL.AccountList(this.dataSeriesNames, this.productNames, this.units);
            }
        }
    }
}

