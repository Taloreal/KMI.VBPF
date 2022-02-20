namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmVitalSigns : Form
    {
        protected Button btnClose;
        private Container components = null;
        protected MenuItem enablingReference;
        protected Input input;
        private KMIGraph kmiGraph1;
        private KMIGraph kmiGraph2;
        private KMIGraph kmiGraph3;
        protected Label labCumProfit;
        protected Label labProfitLabel;
        public const int RECENTWEEKS = 8;

        public frmVitalSigns()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
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

        private void frmVitalSigns_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        private void frmVitalSigns_Load(object sender, EventArgs e)
        {
            if (!base.DesignMode)
            {
                S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
                S.MainForm.EntityChanged += new EventHandler(this.EntityChangedHandler);
                if (this.GetData())
                {
                    this.UpdateForm();
                }
            }
        }

        protected virtual bool GetData()
        {
            try
            {
                this.input = ((BizStateAdapter) S.Adapter).getVitalSigns(S.MainForm.CurrentEntityID);
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
            this.kmiGraph1 = new KMIGraph();
            this.labProfitLabel = new Label();
            this.labCumProfit = new Label();
            this.btnClose = new Button();
            this.kmiGraph2 = new KMIGraph();
            this.kmiGraph3 = new KMIGraph();
            base.SuspendLayout();
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 7f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.BackColor = SystemColors.Control;
            this.kmiGraph1.Data = null;
            this.kmiGraph1.DataPointLabelFontSize = 9f;
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.GraphType = 1;
            this.kmiGraph1.GridLinesX = false;
            this.kmiGraph1.GridLinesY = false;
            this.kmiGraph1.Legend = true;
            this.kmiGraph1.LegendFontSize = 9f;
            this.kmiGraph1.LineWidth = 3;
            this.kmiGraph1.Location = new Point(8, 8);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0x88, 120);
            this.kmiGraph1.TabIndex = 0;
            this.kmiGraph1.Title = null;
            this.kmiGraph1.TitleFontSize = 8f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = null;
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.labProfitLabel.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labProfitLabel.Location = new Point(0x98, 0x90);
            this.labProfitLabel.Name = "labProfitLabel";
            this.labProfitLabel.Size = new Size(0x88, 0x1c);
            this.labProfitLabel.TabIndex = 3;
            this.labProfitLabel.Text = "Cumulative Profit";
            this.labProfitLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.labCumProfit.BorderStyle = BorderStyle.Fixed3D;
            this.labCumProfit.Font = new Font("Microsoft Sans Serif", 14.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labCumProfit.Location = new Point(0x98, 0xb0);
            this.labCumProfit.Name = "labCumProfit";
            this.labCumProfit.Size = new Size(0x88, 0x20);
            this.labCumProfit.TabIndex = 4;
            this.labCumProfit.Text = "$999,999,999";
            this.labCumProfit.TextAlign = ContentAlignment.MiddleRight;
            this.btnClose.Location = new Point(0xb0, 0xe0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x58, 0x18);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.kmiGraph2.AutoScaleY = true;
            this.kmiGraph2.AxisLabelFontSize = 7f;
            this.kmiGraph2.AxisTitleFontSize = 9f;
            this.kmiGraph2.BackColor = SystemColors.Control;
            this.kmiGraph2.Data = null;
            this.kmiGraph2.DataPointLabelFontSize = 9f;
            this.kmiGraph2.DataPointLabels = true;
            this.kmiGraph2.GraphType = 1;
            this.kmiGraph2.GridLinesX = false;
            this.kmiGraph2.GridLinesY = false;
            this.kmiGraph2.Legend = true;
            this.kmiGraph2.LegendFontSize = 9f;
            this.kmiGraph2.LineWidth = 3;
            this.kmiGraph2.Location = new Point(0x98, 8);
            this.kmiGraph2.MinimumYMax = 1f;
            this.kmiGraph2.Name = "kmiGraph2";
            this.kmiGraph2.PrinterMargin = 100;
            this.kmiGraph2.ShowPercentagesForHistograms = false;
            this.kmiGraph2.ShowXTicks = true;
            this.kmiGraph2.ShowYTicks = true;
            this.kmiGraph2.Size = new Size(0x88, 120);
            this.kmiGraph2.TabIndex = 1;
            this.kmiGraph2.Title = null;
            this.kmiGraph2.TitleFontSize = 8f;
            this.kmiGraph2.XAxisLabels = true;
            this.kmiGraph2.XAxisTitle = null;
            this.kmiGraph2.XLabelFormat = null;
            this.kmiGraph2.YAxisTitle = null;
            this.kmiGraph2.YLabelFormat = null;
            this.kmiGraph2.YMax = 0f;
            this.kmiGraph2.YMin = 0f;
            this.kmiGraph2.YTicks = 1;
            this.kmiGraph3.AutoScaleY = true;
            this.kmiGraph3.AxisLabelFontSize = 7f;
            this.kmiGraph3.AxisTitleFontSize = 9f;
            this.kmiGraph3.BackColor = SystemColors.Control;
            this.kmiGraph3.Data = null;
            this.kmiGraph3.DataPointLabelFontSize = 9f;
            this.kmiGraph3.DataPointLabels = true;
            this.kmiGraph3.GraphType = 1;
            this.kmiGraph3.GridLinesX = false;
            this.kmiGraph3.GridLinesY = false;
            this.kmiGraph3.Legend = true;
            this.kmiGraph3.LegendFontSize = 9f;
            this.kmiGraph3.LineWidth = 3;
            this.kmiGraph3.Location = new Point(8, 0x88);
            this.kmiGraph3.MinimumYMax = 1f;
            this.kmiGraph3.Name = "kmiGraph3";
            this.kmiGraph3.PrinterMargin = 100;
            this.kmiGraph3.ShowPercentagesForHistograms = false;
            this.kmiGraph3.ShowXTicks = true;
            this.kmiGraph3.ShowYTicks = true;
            this.kmiGraph3.Size = new Size(0x88, 120);
            this.kmiGraph3.TabIndex = 2;
            this.kmiGraph3.Title = null;
            this.kmiGraph3.TitleFontSize = 8f;
            this.kmiGraph3.XAxisLabels = true;
            this.kmiGraph3.XAxisTitle = null;
            this.kmiGraph3.XLabelFormat = null;
            this.kmiGraph3.YAxisTitle = null;
            this.kmiGraph3.YLabelFormat = null;
            this.kmiGraph3.YMax = 0f;
            this.kmiGraph3.YMin = 0f;
            this.kmiGraph3.YTicks = 1;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x128, 0x108);
            base.Controls.Add(this.kmiGraph3);
            base.Controls.Add(this.kmiGraph2);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.labCumProfit);
            base.Controls.Add(this.kmiGraph1);
            base.Controls.Add(this.labProfitLabel);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "frmVitalSigns";
            base.ShowInTaskbar = false;
            this.Text = "Vital Signs - Last 8 Weeks";
            base.Load += new EventHandler(this.frmVitalSigns_Load);
            base.Closed += new EventHandler(this.frmVitalSigns_Closed);
            base.ResumeLayout(false);
        }

        protected virtual void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        protected virtual void UpdateForm()
        {
            try
            {
                int num;
                this.labProfitLabel.Text = "Cumulative Profit";
                if (this.input.MultipleEntities)
                {
                    this.labProfitLabel.Text = this.labProfitLabel.Text + " for All Your " + S.Instance.EntityName + "s";
                }
                this.labCumProfit.Text = Utilities.FC(this.input.CumProfit, S.Instance.CurrencyConversion);
                if (this.input.CumProfit < 0f)
                {
                    this.labCumProfit.ForeColor = Color.Red;
                }
                else
                {
                    this.labCumProfit.ForeColor = Color.Black;
                }
                this.kmiGraph1.Title = "Revenue";
                this.kmiGraph1.Legend = false;
                this.kmiGraph1.XAxisLabels = false;
                object[,] d = new object[2, 9];
                d[1, 0] = "";
                for (num = 0; num < this.input.Sales.Length; num++)
                {
                    d[1, num + 1] = this.input.Sales[num];
                }
                this.kmiGraph1.Draw(d);
                this.kmiGraph2.Title = "Profit";
                this.kmiGraph2.Legend = false;
                this.kmiGraph2.XAxisLabels = false;
                object[,] objArray2 = new object[2, 9];
                objArray2[1, 0] = "";
                for (num = 0; num < this.input.Profit.Length; num++)
                {
                    objArray2[1, num + 1] = this.input.Profit[num];
                }
                this.kmiGraph2.Draw(objArray2);
                this.kmiGraph3.Title = "Customers";
                this.kmiGraph3.Legend = false;
                this.kmiGraph3.XAxisLabels = false;
                this.kmiGraph3.YLabelFormat = "{0:N0}";
                this.kmiGraph3.MinimumYMax = 5f;
                object[,] objArray3 = new object[2, 9];
                objArray3[1, 0] = "";
                for (num = 0; num < this.input.Customers.Length; num++)
                {
                    objArray3[1, num + 1] = this.input.Customers[num];
                }
                this.kmiGraph3.Draw(objArray3);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
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

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public float[] Sales;
            public float[] Profit;
            public int[] Customers;
            public float CumProfit;
            public bool MultipleEntities;
        }
    }
}

