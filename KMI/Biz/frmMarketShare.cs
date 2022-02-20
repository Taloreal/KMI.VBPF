namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMarketShare : Form
    {
        private Button btnClose;
        private Button btnHelp;
        private Container components = null;
        protected object[,] graphData;
        private GroupBox grpShowAs;
        private Label label1;
        private KMIGraph marketGraph;
        private RadioButton optLineGraph;
        private RadioButton optPieGraph;
        private Panel panel1;
        private Panel panel2;

        public frmMarketShare()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
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

        private void frmMarketShare_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
        }

        private void frmMarketShare_Load(object sender, EventArgs e)
        {
            S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
            this.optLineGraph.Checked = true;
            this.optPieGraph.Checked = false;
            this.marketGraph.YLabelFormat = "{0:0%}";
            if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        protected bool GetData()
        {
            try
            {
                this.graphData = ((BizStateAdapter) S.Adapter).GetMarketShare();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.marketGraph = new KMIGraph();
            this.grpShowAs = new GroupBox();
            this.optPieGraph = new RadioButton();
            this.optLineGraph = new RadioButton();
            this.btnClose = new Button();
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.btnHelp = new Button();
            this.label1 = new Label();
            this.grpShowAs.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.marketGraph.AutoScaleY = true;
            this.marketGraph.AxisLabelFontSize = 9f;
            this.marketGraph.AxisTitleFontSize = 9f;
            this.marketGraph.Data = null;
            this.marketGraph.DataPointLabelFontSize = 9f;
            this.marketGraph.DataPointLabels = true;
            this.marketGraph.Dock = DockStyle.Fill;
            this.marketGraph.GraphType = 1;
            this.marketGraph.GridLinesX = false;
            this.marketGraph.GridLinesY = false;
            this.marketGraph.Legend = true;
            this.marketGraph.LegendFontSize = 9f;
            this.marketGraph.LineWidth = 3;
            this.marketGraph.Location = new Point(0, 0);
            this.marketGraph.MinimumYMax = 1f;
            this.marketGraph.Name = "marketGraph";
            this.marketGraph.PrinterMargin = 100;
            this.marketGraph.ShowPercentagesForHistograms = false;
            this.marketGraph.ShowXTicks = true;
            this.marketGraph.ShowYTicks = true;
            this.marketGraph.Size = new Size(0x180, 0xc6);
            this.marketGraph.TabIndex = 0;
            this.marketGraph.Title = "Market Share Report";
            this.marketGraph.TitleFontSize = 18f;
            this.marketGraph.XAxisLabels = true;
            this.marketGraph.XAxisTitle = null;
            this.marketGraph.XLabelFormat = null;
            this.marketGraph.YAxisTitle = null;
            this.marketGraph.YLabelFormat = null;
            this.marketGraph.YMax = 0f;
            this.marketGraph.YMin = 0f;
            this.marketGraph.YTicks = 1;
            this.grpShowAs.Controls.Add(this.optPieGraph);
            this.grpShowAs.Controls.Add(this.optLineGraph);
            this.grpShowAs.Location = new Point(0x10, 8);
            this.grpShowAs.Name = "grpShowAs";
            this.grpShowAs.Size = new Size(0xe0, 0x30);
            this.grpShowAs.TabIndex = 1;
            this.grpShowAs.TabStop = false;
            this.grpShowAs.Text = "Show As";
            this.optPieGraph.Location = new Point(120, 0x10);
            this.optPieGraph.Name = "optPieGraph";
            this.optPieGraph.Size = new Size(0x58, 0x18);
            this.optPieGraph.TabIndex = 1;
            this.optPieGraph.Text = "&Pie Graph";
            this.optPieGraph.Click += new EventHandler(this.optPieGraph_Click);
            this.optLineGraph.Location = new Point(0x10, 0x10);
            this.optLineGraph.Name = "optLineGraph";
            this.optLineGraph.Size = new Size(0x58, 0x18);
            this.optLineGraph.TabIndex = 0;
            this.optLineGraph.Text = "&Line Graph";
            this.optLineGraph.Click += new EventHandler(this.optLineGraph_Click);
            this.btnClose.Location = new Point(0x18, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x60, 0x17);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.grpShowAs);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xc6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x180, 0x48);
            this.panel1.TabIndex = 0;
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0xf8, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x88, 0x48);
            this.panel2.TabIndex = 0;
            this.btnHelp.Location = new Point(0x18, 40);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "&Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.ForeColor = SystemColors.ControlDark;
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x170, 0xc0);
            this.label1.TabIndex = 4;
            this.label1.Text = "No businesses exist now";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x180, 270);
            base.Controls.Add(this.marketGraph);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new Size(0x188, 0x128);
            base.Name = "frmMarketShare";
            base.ShowInTaskbar = false;
            this.Text = "Market Share";
            base.Load += new EventHandler(this.frmMarketShare_Load);
            base.Closed += new EventHandler(this.frmMarketShare_Closed);
            this.grpShowAs.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        private void optLineGraph_Click(object sender, EventArgs e)
        {
            this.UpdateForm();
        }

        private void optPieGraph_Click(object sender, EventArgs e)
        {
            this.UpdateForm();
        }

        protected void UpdateForm()
        {
            try
            {
                if (this.graphData == null)
                {
                    this.marketGraph.Visible = false;
                }
                else
                {
                    this.marketGraph.Visible = true;
                    if (this.optLineGraph.Checked)
                    {
                        this.marketGraph.GraphType = 1;
                        this.marketGraph.Title = "Share of Revenue -- Last " + this.graphData.GetUpperBound(1) + " Weeks";
                    }
                    else
                    {
                        this.marketGraph.GraphType = 4;
                        this.marketGraph.Title = "Share of Revenue -- Last Week";
                    }
                    this.marketGraph.Draw(this.graphData);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }
    }
}

