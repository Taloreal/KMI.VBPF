namespace KMI.Utility {

    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Printing;
    using System.Resources;
    using System.Windows.Forms;

    public class KMIGraph : UserControl
    {
        protected bool autoScaleY;
        protected Font axisLabelFont;
        protected Font axisTitleFont;
        protected bool bDataPointLabels;
        protected bool bGridLinesX;
        protected bool bGridLinesY;
        protected bool bLegend;
        protected bool bShowXTicks;
        protected bool bShowYTicks;
        protected bool bXAxisLabelDoubleDecker;
        protected PictureBox componentPictureBox;
        protected object[,] data;
        protected Font dataPointLabelFont;
        protected float fXMax;
        protected float fXMin;
        protected float fXScale;
        protected float fXStep;
        protected float fYMax;
        protected float fYMin;
        protected float fYScale;
        protected float fYStep;
        public const int GRAPH_HISTOGRAM = 2;
        public const int GRAPH_HISTOGRAM3D = 3;
        public const int GRAPH_LINE = 1;
        public const int GRAPH_PIE = 4;
        public const int GRAPH_SCATTER = 6;
        public const int GRAPH_SCOREBOARD = 5;
        protected Graphics GraphicsObject;
        public const int LEGEND_LEFT_MARGIN = 16;
        protected RectangleF LegendArea;
        protected Font legendFont;
        protected float maxXValue;
        protected float maxYValue;
        protected float minimumYMax = 1f;
        protected int nGraphType;
        protected int nLineWidth;
        protected int nXNumLabels;
        protected int nXSteps;
        protected int nYSteps;
        protected RectangleF PlotArea;
        protected int printerMargin;
        private static ResourceManager rm = new ResourceManager("KMI.Utility.Utility", System.Reflection.Assembly.GetAssembly(typeof(KMIGraph)));
        protected StringFormat SF;
        protected StringFormat SFC;
        protected StringFormat SFVC;
        protected StringFormat SFVL;
        protected bool showPercentagesForHistograms;
        protected string sTitle;
        protected string sXAxisTitle;
        protected string sXLabelFormat;
        protected string sYAxisTitle;
        protected string sYLabelFormat;
        protected RectangleF TitleArea;
        protected Font titleFont;
        protected RectangleF XAxisArea;
        protected RectangleF XAxisLabelArea;
        protected bool xAxisLabels;
        protected RectangleF XAxisTitleArea;
        protected RectangleF YAxisArea;
        protected RectangleF YAxisLabelArea;
        protected RectangleF YAxisTitleArea;

        public KMIGraph() {
            this.InitializeComponent();
            this.SetDefaultFonts();
            this.componentPictureBox.Location = new Point(0, 0);
            this.componentPictureBox.Size = base.Size;
            this.nGraphType = 1;
            this.nLineWidth = 4;
            this.bLegend = true;
            this.bDataPointLabels = true;
            this.printerMargin = 100;
            this.autoScaleY = true;
            this.xAxisLabels = true;
            this.bShowXTicks = true;
            this.bShowYTicks = true;
            this.SF = new StringFormat();
            this.SFC = new StringFormat();
            this.SFC.Alignment = StringAlignment.Center;
            this.SFVC = new StringFormat(StringFormatFlags.DirectionVertical);
            this.SFVC.Alignment = StringAlignment.Center;
            this.SFVL = new StringFormat(StringFormatFlags.DirectionVertical);
            this.SFVL.Alignment = StringAlignment.Near;
        }

        protected void BorderArea(RectangleF a, Color c)
        {
            this.GraphicsObject.DrawRectangle(new Pen(c), a.Left, a.Top, a.Width, a.Height);
        }

        public void Draw(object[,] d)
        {
            this.data = d;
            if (((this.GraphType == 2) || (this.GraphType == 3)) && this.ShowPercentagesForHistograms)
            {
                int num;
                int num2;
                float[] numArray = new float[this.data.GetUpperBound(0)];
                for (num = 1; num <= this.data.GetUpperBound(0); num++)
                {
                    numArray[num - 1] = 0f;
                    num2 = 1;
                    while (num2 <= this.data.GetUpperBound(1))
                    {
                        numArray[num - 1] += Convert.ToSingle(this.data[num, num2]);
                        num2++;
                    }
                }
                for (num = 1; num <= this.data.GetUpperBound(0); num++)
                {
                    for (num2 = 1; num2 <= this.data.GetUpperBound(1); num2++)
                    {
                        if (this.GraphType == 3)
                        {
                            this.data[num, num2] = Convert.ToSingle(this.data[num, num2]) / Math.Max(1f, numArray[0]);
                        }
                        else
                        {
                            this.data[num, num2] = Convert.ToSingle(this.data[num, num2]) / Math.Max(1f, numArray[num - 1]);
                        }
                    }
                }
            }
            this.componentPictureBox.Refresh();
        }

        protected void DrawAxes()
        {
            Pen pen = new Pen(Color.Black, 1f);
            this.GraphicsObject.DrawLine(pen, this.YAxisArea.Left + this.YAxisArea.Width, this.YAxisArea.Top, this.YAxisArea.Left + this.YAxisArea.Width, this.YAxisArea.Top + this.YAxisArea.Height);
            this.GraphicsObject.DrawLine(pen, this.XAxisArea.Left, this.XAxisArea.Top, this.XAxisArea.Left + this.XAxisArea.Width, this.XAxisArea.Top);
        }

        protected void DrawAxisLabels()
        {
            string str;
            float num2;
            int num3;
            SizeF ef;
            for (num3 = 0; num3 <= this.nYSteps; num3++)
            {
                num2 = this.YAxisArea.Top + ((num3 * this.fYStep) * this.fYScale);
                if (this.bShowYTicks)
                {
                    this.GraphicsObject.DrawLine(new Pen(Color.Black), this.YAxisArea.Left, num2, this.YAxisArea.Left + (this.YAxisArea.Width * 2f), num2);
                }
                if (this.GridLinesY)
                {
                    this.GraphicsObject.DrawLine(new Pen(Color.Gray), this.PlotArea.Left, num2, this.PlotArea.Left + this.PlotArea.Width, num2);
                }
                str = this.FormatYLabel(this.fYMax - (num3 * this.fYStep));
                ef = this.GraphicsObject.MeasureString(str, this.axisLabelFont);
                this.GraphicsObject.DrawString(str, this.axisLabelFont, new SolidBrush(Color.Black), new PointF((this.YAxisLabelArea.Left + (this.YAxisLabelArea.Width * 0.95f)) - ef.Width, num2 - (ef.Height / 2f)));
            }
            for (num3 = 1; num3 <= (this.nXSteps + 1); num3++)
            {
                float num = this.XAxisArea.Left + (((num3 - 1) * this.fXStep) * this.fXScale);
                if (this.bShowXTicks)
                {
                    this.GraphicsObject.DrawLine(new Pen(Color.Black), num, this.XAxisArea.Top - this.XAxisArea.Height, num, this.XAxisArea.Top + this.XAxisArea.Height);
                }
                if (this.bGridLinesX)
                {
                    this.GraphicsObject.DrawLine(new Pen(Color.Gray), num, this.PlotArea.Top, num, this.PlotArea.Top + this.PlotArea.Height);
                }
                if (((((int)(1f + ((num3 - 1) * this.fXStep))) <= this.data.GetUpperBound(1)) && ((this.fYMin >= 0.0) || (num3 > 1))) && (this.nGraphType != 6))
                {
                    str = this.FormatXLabel(this.data[0, 1 + ((num3 - 1) * ((int)this.fXStep))]);
                    ef = this.GraphicsObject.MeasureString(str, this.axisLabelFont);
                    if (this.nGraphType == 1)
                    {
                        num -= ef.Width / 2f;
                    }
                    else if (this.bXAxisLabelDoubleDecker)
                    {
                        num += ((0.5f * this.fXStep) * this.fXScale) - (this.GraphicsObject.MeasureString("X", this.axisLabelFont).Height / 2f);
                    }
                    else
                    {
                        num += ((0.5f * this.fXStep) * this.fXScale) - (ef.Width / 2f);
                    }
                    num2 = this.XAxisLabelArea.Top + (0.15f * ef.Height);
                    if (this.bXAxisLabelDoubleDecker)
                    {
                        this.GraphicsObject.DrawString(str, this.axisLabelFont, new SolidBrush(Color.Black), num, num2, this.SFVL);
                    }
                    else if ((num >= 0.0) && ((num + ef.Width) <= this.componentPictureBox.Width))
                    {
                        this.GraphicsObject.DrawString(str, this.axisLabelFont, new SolidBrush(Color.Black), num, num2);
                    }
                }
                else if (this.nGraphType == 6)
                {
                    str = ((((float)(num3 - 1)) / ((float)this.nXSteps)) * this.fXMax).ToString();
                    ef = this.GraphicsObject.MeasureString(str, this.axisLabelFont);
                    num -= ef.Width / 2f;
                    num2 = this.XAxisLabelArea.Top + (0.15f * ef.Height);
                    if (this.bXAxisLabelDoubleDecker && ((num3 % 2) == 0))
                    {
                        num2 += 1.15f * this.GraphicsObject.MeasureString("X", this.axisLabelFont).Height;
                    }
                    this.GraphicsObject.DrawString(str, this.axisLabelFont, new SolidBrush(Color.Black), num, num2);
                }
            }
        }

        protected void DrawAxisTitles()
        {
            if (this.sYAxisTitle != null)
            {
                this.GraphicsObject.DrawString(this.sYAxisTitle, this.axisTitleFont, new SolidBrush(Color.Black), this.YAxisTitleArea, this.SFVC);
            }
            if (this.sXAxisTitle != null)
            {
                this.GraphicsObject.DrawString(this.sXAxisTitle, this.axisTitleFont, new SolidBrush(Color.Black), this.XAxisTitleArea, this.SFC);
            }
        }

        protected void DrawData()
        {
            int num5;
            int num6;
            string str;
            SizeF ef;
            RectangleF ef2;
            float num = this.PlotArea.Width / ((float)this.nXSteps);
            float num2 = 6f;
            float num3 = 0f;
            float num4 = 0f;
            switch (this.nGraphType)
            {
                case 1:
                    for (num5 = 1; num5 <= this.data.GetUpperBound(0); num5++)
                    {
                        num6 = 1;
                        while (num6 <= this.data.GetUpperBound(1))
                        {
                            if (this.data[num5, num6] != null)
                            {
                                if (num6 == 1)
                                {
                                    num3 = this.PlotArea.Left + ((num6 - 1) * this.fXScale);
                                    num4 = this.PlotArea.Top + ((this.fYMax - Convert.ToSingle(this.data[num5, num6])) * this.fYScale);
                                }
                                else
                                {
                                    this.GraphicsObject.DrawLine(new Pen(this.LineColor(num5), (float)this.nLineWidth), num3, num4, this.PlotArea.Left + ((num6 - 1) * this.fXScale), this.PlotArea.Top + ((this.fYMax - Convert.ToSingle(this.data[num5, num6])) * this.fYScale));
                                    num3 = this.PlotArea.Left + ((num6 - 1) * this.fXScale);
                                    num4 = this.PlotArea.Top + ((this.fYMax - Convert.ToSingle(this.data[num5, num6])) * this.fYScale);
                                }
                            }
                            num6++;
                        }
                    }
                    break;

                case 2:
                    {
                        float width = (num - (2f * num2)) / ((float)this.data.GetUpperBound(0));
                        num5 = 1;
                        while (num5 <= this.data.GetUpperBound(0))
                        {
                            num6 = 1;
                            while (num6 <= this.data.GetUpperBound(1))
                            {
                                this.GraphicsObject.FillRectangle(new SolidBrush(this.LineColor(num5)), ((this.PlotArea.Left + ((num6 - 1) * num)) + num2) + ((num5 - 1) * width), (this.PlotArea.Top + this.PlotArea.Height) - (Convert.ToSingle(this.data[num5, num6]) * this.fYScale), width, Convert.ToSingle(this.data[num5, num6]) * this.fYScale);
                                if (this.bDataPointLabels)
                                {
                                    str = this.FormatYLabel(this.data[num5, num6]);
                                    ef = this.GraphicsObject.MeasureString(str, this.dataPointLabelFont);
                                    float introduced30 = Math.Max(width, ef.Width);
                                    ef2 = new RectangleF(((this.PlotArea.Left + ((num6 - 1) * num)) + num2) + ((num5 - 1) * width), ((this.PlotArea.Top + this.PlotArea.Height) - (Convert.ToSingle(this.data[num5, num6]) * this.fYScale)) - ef.Height, introduced30, ef.Height);
                                    this.GraphicsObject.DrawString(str, this.dataPointLabelFont, new SolidBrush(Color.Black), ef2, this.SFC);
                                }
                                num6++;
                            }
                            num5++;
                        }
                        break;
                    }
                case 3:
                    if (this.data.GetUpperBound(0) <= 2)
                    {
                        num5 = 1;
                        while (num5 <= this.data.GetUpperBound(0))
                        {
                            num6 = 1;
                            while (num6 <= this.data.GetUpperBound(1))
                            {
                                this.GraphicsObject.FillRectangle(new SolidBrush(this.LineColor(num5)), (float)((this.PlotArea.Left + ((num6 - 1) * num)) + (num5 * num2)), (float)((this.PlotArea.Top + this.PlotArea.Height) - (Convert.ToSingle(this.data[num5, num6]) * this.fYScale)), (float)(num - ((2 * num5) * num2)), (float)(Convert.ToSingle(this.data[num5, num6]) * this.fYScale));
                                num6++;
                            }
                            num5++;
                        }
                        if (this.bDataPointLabels)
                        {
                            for (num5 = 1; num5 <= this.data.GetUpperBound(0); num5++)
                            {
                                for (num6 = 1; num6 <= this.data.GetUpperBound(1); num6++)
                                {
                                    str = this.FormatYLabel(this.data[num5, num6]);
                                    ef = this.GraphicsObject.MeasureString(str, this.dataPointLabelFont);
                                    float introduced31 = Math.Max(ef.Width, num - ((2 * num5) * num2));
                                    ef2 = new RectangleF((this.PlotArea.Left + ((num6 - 1) * num)) + (num5 * num2), ((this.PlotArea.Top + this.PlotArea.Height) - (Convert.ToSingle(this.data[num5, num6]) * this.fYScale)) - ef.Height, introduced31, ef.Height);
                                    this.GraphicsObject.DrawString(str, this.dataPointLabelFont, new SolidBrush(Color.Black), ef2, this.SFC);
                                }
                            }
                        }
                        break;
                    }
                    this.GraphicsObject.DrawString("Histrogram3D cannot graph more than 2 sets of data.", new Font("Microsoft Sans Serif", 14f, FontStyle.Bold), new SolidBrush(Color.Red), this.PlotArea, this.SFC);
                    break;

                case 4:
                    {
                        num2 = (this.PlotArea.Width - this.PlotArea.Height) / 2f;
                        float num8 = 0f;
                        float startAngle = 0f;
                        num5 = 1;
                        while (num5 <= this.data.GetUpperBound(0))
                        {
                            num8 += Convert.ToSingle(this.data[num5, this.data.GetUpperBound(1)]);
                            num5++;
                        }
                        for (num5 = 1; num5 <= this.data.GetUpperBound(0); num5++)
                        {
                            float sweepAngle = (Convert.ToSingle(this.data[num5, this.data.GetUpperBound(1)]) / num8) * 360f;
                            this.GraphicsObject.FillPie(new SolidBrush(this.LineColor(num5)), this.PlotArea.Left + num2, this.PlotArea.Top, this.PlotArea.Height, this.PlotArea.Height, startAngle, sweepAngle);
                            startAngle += sweepAngle;
                        }
                        break;
                    }
                case 5:
                    {
                        float num11 = this.PlotArea.Height / ((float)this.nXSteps);
                        float height = num11 - 8f;
                        float num13 = (this.PlotArea.Width - (1.05f * Math.Max(this.GraphicsObject.MeasureString(this.FormatYLabel(this.fYMin), this.dataPointLabelFont).Width, this.GraphicsObject.MeasureString(this.FormatYLabel(this.fYMax), this.dataPointLabelFont).Width))) - 4f;
                        for (num5 = 1; num5 <= this.data.GetUpperBound(0); num5++)
                        {
                            float num17;
                            Color darkBlue;
                            float objValue = Convert.ToSingle(this.data[num5, this.data.GetUpperBound(1)]);
                            float y = (this.PlotArea.Top + ((num5 - 1) * num11)) + 4f;
                            float num16 = Math.Abs((float)((objValue / (this.fYMax - this.fYMin)) * num13));
                            if (objValue >= 0f)
                            {
                                darkBlue = Color.DarkBlue;
                                num17 = this.PlotArea.Left - ((this.fYMin / (this.fYMax - this.fYMin)) * num13);
                            }
                            else
                            {
                                darkBlue = Color.Red;
                                num17 = this.PlotArea.Left + (((objValue - this.fYMin) / (this.fYMax - this.fYMin)) * num13);
                            }
                            this.GraphicsObject.FillRectangle(new SolidBrush(darkBlue), num17, y, num16, height);
                            if (this.bDataPointLabels)
                            {
                                str = this.FormatYLabel(objValue);
                                ef = this.GraphicsObject.MeasureString(str, this.dataPointLabelFont);
                                this.GraphicsObject.DrawString(str, this.dataPointLabelFont, new SolidBrush(Color.Black), (float)((num17 + num16) + 4f), (float)(y + ((height - ef.Height) / 2f)));
                            }
                            string text = this.data[num5, 0].ToString();
                            SizeF ef3 = this.GraphicsObject.MeasureString(text, this.legendFont);
                            this.GraphicsObject.DrawString(text, this.legendFont, new SolidBrush(Color.Black), this.YAxisLabelArea.Left, y + ((height - ef3.Height) / 2f));
                        }
                        break;
                    }
                case 6:
                    this.FindMaxValues(this.data);
                    for (num5 = 1; num5 <= this.data.GetUpperBound(1); num5++)
                    {
                        float num21;
                        float num20 = num21 = 2f * this.nLineWidth;
                        float x = this.PlotArea.Left + ((Convert.ToSingle(this.data[0, num5]) / this.maxXValue) * this.PlotArea.Width);
                        float num19 = (this.PlotArea.Top + this.PlotArea.Height) - ((Convert.ToSingle(this.data[1, num5]) / this.maxYValue) * this.PlotArea.Height);
                        x -= num20 / 2f;
                        num19 -= num21 / 2f;
                        this.GraphicsObject.FillEllipse(new SolidBrush(Color.Yellow), x, num19, num20, num21);
                        this.GraphicsObject.DrawEllipse(new Pen(Color.Black, 1f), x, num19, num20, num21);
                    }
                    break;
            }
        }

        protected void DrawLegend()
        {
            float num = this.LegendArea.Height / ((float)(this.data.GetUpperBound(0) + 1));
            float size = this.legendFont.Size;
            for (int i = 1; i <= this.data.GetUpperBound(0); i++)
            {
                if (this.bLegend && (this.nGraphType != 6))
                {
                    if (this.nGraphType == 1)
                    {
                        this.GraphicsObject.DrawLine(new Pen(this.LineColor(i), (float)(this.nLineWidth + 2)), (float)(this.LegendArea.Left + 16f), (float)((this.LegendArea.Top + ((i - 0.5f) * num)) + (size / 2f)), (float)((this.LegendArea.Left + 16f) + size), (float)(((this.LegendArea.Top + ((i - 0.5f) * num)) + (size / 2f)) - size));
                    }
                    else
                    {
                        this.GraphicsObject.FillRectangle(new SolidBrush(this.LineColor(i)), this.LegendArea.Left + 16f, ((this.LegendArea.Top + ((i - 0.5f) * num)) + (size / 2f)) - size, size, size);
                    }
                    string text = this.data[i, 0].ToString();
                    SizeF ef = this.GraphicsObject.MeasureString(text, this.legendFont);
                    this.GraphicsObject.DrawString(text, this.legendFont, new SolidBrush(this.LineColor(i)), (float)((this.LegendArea.Left + 16f) + (size * 2f)), (float)((this.LegendArea.Top + ((i - 0.5f) * num)) - (ef.Height / 2f)));
                }
            }
        }

        protected void DrawTitle()
        {
            if (this.sTitle != null)
            {
                this.GraphicsObject.DrawString(this.sTitle, this.titleFont, new SolidBrush(Color.Black), this.TitleArea, this.SFC);
            }
        }

        protected void FindMaxValues(object[,] data)
        {
            if (this.nGraphType == 6)
            {
                this.maxXValue = Convert.ToSingle(data[0, 1]);
                this.maxYValue = Convert.ToSingle(data[1, 1]);
                for (int i = 2; i <= data.GetUpperBound(1); i++)
                {
                    if (Convert.ToSingle(data[0, i]) > this.maxXValue)
                    {
                        this.maxXValue = Convert.ToSingle(data[0, i]);
                    }
                    if (Convert.ToSingle(data[1, i]) > this.maxYValue)
                    {
                        this.maxYValue = Convert.ToSingle(data[1, i]);
                    }
                }
            }
        }

        protected string FormatXLabel(object objValue)
        {
            if (objValue is DateTime)
            {
                if ((this.sXLabelFormat == null) || (this.sXLabelFormat == ""))
                {
                    return string.Format("{0:dd MMM yy}", objValue);
                }
                return string.Format(this.sXLabelFormat, objValue);
            }
            if (objValue != null)
            {
                return objValue.ToString();
            }
            return "";
        }

        protected string FormatYLabel(object objValue)
        {
            if ((this.sYLabelFormat == null) || (this.sYLabelFormat == ""))
            {
                if (((this.GraphType == 2) || (this.GraphType == 3)) && this.ShowPercentagesForHistograms)
                {
                    return string.Format("{0:P0}", objValue);
                }
                if ((this.fYMax < 99f) && (this.fYMin > -99f))
                {
                    return string.Format("{0:C2}", objValue);
                }
                return string.Format("{0:C0}", objValue);
            }
            return string.Format(this.sYLabelFormat, objValue);
        }

        private void Graph_Paint(object sender, PaintEventArgs e)
        {
            if (((this.data == null) || ((this.GraphType == 1) && (this.data.GetUpperBound(1) < 2))) || ((this.GraphType != 1) && (this.data.GetUpperBound(1) < 1)))
            {
                e.Graphics.DrawString(rm.GetString("Not enough data to display graph at this time."), this.titleFont, new SolidBrush(Color.DarkGray), base.ClientRectangle);
            }
            else
            {
                try
                {
                    this.GraphicsObject = e.Graphics;
                    this.RenderToGraphics();
                }
                catch (Exception exception)
                {
                    throw new Exception("In KMIGraph.GraphPaint, " + exception.Message);
                }
            }
        }

        private void Graph_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                Utilities.ResetFPU();
                float sx = 1f;
                this.GraphicsObject = e.Graphics;
                this.GraphicsObject.SmoothingMode = SmoothingMode.HighQuality;
                if (!e.PageSettings.Landscape)
                {
                    sx = (e.PageSettings.Bounds.Width - (2f * this.printerMargin)) / ((float)base.Width);
                }
                else
                {
                    sx = (e.PageSettings.Bounds.Height - (2f * this.printerMargin)) / ((float)base.Height);
                }
                this.GraphicsObject.ScaleTransform(sx, sx);
                float dx = ((e.PageSettings.Bounds.Width - (base.Width * sx)) / 2f) / sx;
                float dy = ((e.PageSettings.Bounds.Height - (base.Height * sx)) / 2f) / sx;
                this.GraphicsObject.TranslateTransform(dx, dy);
                this.RenderToGraphics();
            }
            catch (Exception exception)
            {
                throw new Exception("In KMIGraph.PrintPage, " + exception.Message);
            }
        }

        private void InitializeComponent()
        {
            this.componentPictureBox = new PictureBox();
            base.SuspendLayout();
            this.componentPictureBox.Location = new Point(0xb0, 0x38);
            this.componentPictureBox.Name = "componentPictureBox";
            this.componentPictureBox.TabIndex = 0;
            this.componentPictureBox.TabStop = false;
            this.componentPictureBox.Paint += new PaintEventHandler(this.Graph_Paint);
            base.Controls.AddRange(new Control[] { this.componentPictureBox });
            base.Name = "KMIGraph";
            base.Size = new Size(0x158, 0xd0);
            base.SizeChanged += new EventHandler(this.KMIGraph_SizeChanged);
            base.ResumeLayout(false);
        }

        private void KMIGraph_SizeChanged(object sender, EventArgs e)
        {
            this.componentPictureBox.Location = new Point(0, 0);
            this.componentPictureBox.Size = base.Size;
            this.componentPictureBox.Refresh();
        }

        protected Color LineColor(int i)
        {
            Color blue = Color.Blue;
            switch ((i % 10))
            {
                case 0:
                    blue = Color.DarkSalmon;
                    break;

                case 1:
                    blue = Color.Blue;
                    break;

                case 2:
                    blue = Color.DarkGreen;
                    break;

                case 3:
                    blue = Color.Red;
                    break;

                case 4:
                    blue = Color.DarkGoldenrod;
                    break;

                case 5:
                    blue = Color.Magenta;
                    break;

                case 6:
                    blue = Color.Brown;
                    break;

                case 7:
                    blue = Color.DarkOrange;
                    break;

                case 8:
                    blue = Color.Black;
                    break;

                case 9:
                    blue = Color.DarkGray;
                    break;
            }
            if (this.nGraphType == 3)
            {
                if (i == 1)
                {
                    blue = Color.FromArgb(0x88, 0x88, 0x88);
                }
                if (i == 2)
                {
                    blue = Color.DarkBlue;
                }
            }
            return blue;
        }

        protected float LongestLegendLabelWidth(Graphics g)
        {
            float width = 0f;
            for (int i = 1; i <= this.data.GetUpperBound(0); i++)
            {
                if ((this.data[i, 0] != null) && (g.MeasureString(this.data[i, 0].ToString(), this.legendFont).Width > width))
                {
                    width = g.MeasureString(this.data[i, 0].ToString(), this.legendFont).Width;
                }
            }
            return width;
        }

        protected float LongestXAxisLabelWidth(Graphics g)
        {
            float width = 0f;
            for (int i = 1; i <= this.data.GetUpperBound(1); i++)
            {
                if ((this.data[0, i] != null) && (g.MeasureString(this.data[0, i].ToString(), this.axisLabelFont).Width > width))
                {
                    width = g.MeasureString(this.data[0, i].ToString(), this.axisLabelFont).Width;
                }
            }
            return width;
        }

        public void PrintGraph()
        {
            if (((this.data == null) || ((this.GraphType == 1) && (this.data.GetUpperBound(1) < 2))) || ((this.GraphType != 1) && (this.data.GetUpperBound(1) < 1)))
            {
                string caption = rm.GetString("No Data");
                MessageBox.Show(rm.GetString("Not enough data to display graph at this time."), caption);
            }
            else
            {
                Utilities.PrintWithExceptionHandling(this.Title, new PrintPageEventHandler(this.Graph_PrintPage));
            }
        }

        public void PrintPreviewGraph()
        {
            if (((this.data == null) || ((this.GraphType == 1) && (this.data.GetUpperBound(1) < 2))) || ((this.GraphType != 1) && (this.data.GetUpperBound(1) < 1)))
            {
                string caption = rm.GetString("No Data");
                MessageBox.Show(rm.GetString("Not enough data to display graph at this time."), caption);
            }
            else
            {
                PrintPreviewDialog dialog = new PrintPreviewDialog();
                PrintDocument document = new PrintDocument
                {
                    DocumentName = this.Title
                };
                document.PrintPage += new PrintPageEventHandler(this.Graph_PrintPage);
                dialog.Document = document;
                dialog.ShowDialog();
            }
        }

        protected void RenderToGraphics()
        {
            if (this.nGraphType == 6)
            {
                this.sXAxisTitle = (string)this.data[0, 0];
                this.sYAxisTitle = (string)this.data[1, 0];
            }
            this.SetScale();
            this.SetAreas();
            this.DrawTitle();
            this.DrawLegend();
            if ((this.nGraphType != 4) && (this.nGraphType != 5))
            {
                this.DrawAxisTitles();
            }
            this.DrawData();
            if ((this.nGraphType != 4) && (this.nGraphType != 5))
            {
                this.DrawAxes();
                this.DrawAxisLabels();
            }
        }

        protected void SetAreas()
        {
            Graphics graphicsObject = this.GraphicsObject;
            this.TitleArea = new RectangleF(0f, 0f, 0f, 0f);
            this.LegendArea = new RectangleF(0f, 0f, 0f, 0f);
            this.XAxisTitleArea = new RectangleF(0f, 0f, 0f, 0f);
            this.YAxisTitleArea = new RectangleF(0f, 0f, 0f, 0f);
            this.XAxisLabelArea = new RectangleF(0f, 0f, 0f, 0f);
            this.YAxisLabelArea = new RectangleF(0f, 0f, 0f, 0f);
            this.XAxisArea = new RectangleF(0f, 0f, 0f, 0f);
            this.YAxisArea = new RectangleF(0f, 0f, 0f, 0f);
            this.PlotArea = new RectangleF(0f, 0f, 0f, 0f);
            if (this.sTitle != null)
            {
                this.TitleArea.Height = 1.8f * graphicsObject.MeasureString(this.Title, this.titleFont).Height;
                this.TitleArea.Width = this.componentPictureBox.Width - 1;
            }
            else
            {
                this.TitleArea.Height = graphicsObject.MeasureString("X", this.axisLabelFont).Height;
            }
            if (this.bLegend)
            {
                this.LegendArea.Width = ((this.LongestLegendLabelWidth(graphicsObject) * 1.25f) + (2f * graphicsObject.MeasureString("X", this.legendFont).Height)) + 16f;
                if (this.LegendArea.Width < (this.LongestXAxisLabelWidth(graphicsObject) / 2f))
                {
                    this.LegendArea.Width = this.LongestXAxisLabelWidth(graphicsObject) / 2f;
                }
                this.LegendArea.Location = new PointF((this.componentPictureBox.Width - 1) - this.LegendArea.Width, this.TitleArea.Height);
            }
            else
            {
                this.LegendArea.Width = this.LongestXAxisLabelWidth(graphicsObject) / 2f;
                this.LegendArea.Location = new PointF((this.componentPictureBox.Width - 1) - this.LegendArea.Width, 0f);
            }
            if (this.sYAxisTitle != null)
            {
                this.YAxisTitleArea.Width = 2f * this.GraphicsObject.MeasureString(this.sYAxisTitle, this.axisTitleFont).Height;
            }
            if (this.sXAxisTitle != null)
            {
                this.XAxisTitleArea.Height = 1.5f * this.GraphicsObject.MeasureString(this.sXAxisTitle, this.axisTitleFont).Height;
            }
            else
            {
                this.XAxisTitleArea.Height = 0.5f * this.GraphicsObject.MeasureString("X", this.axisLabelFont).Height;
            }
            if (this.GraphType != 5)
            {
                this.YAxisLabelArea.Width = 1.1f * Math.Max(this.GraphicsObject.MeasureString(this.FormatYLabel(this.fYMin), this.axisLabelFont).Width, this.GraphicsObject.MeasureString(this.FormatYLabel(this.fYMax), this.axisLabelFont).Width);
            }
            else
            {
                this.YAxisLabelArea.Width = this.LongestLegendLabelWidth(graphicsObject);
            }
            this.YAxisArea.Width = 4f;
            this.XAxisArea.Height = 4f;
            this.YAxisTitleArea.Location = new PointF(0f, this.TitleArea.Height);
            this.YAxisLabelArea.Location = new PointF(this.YAxisTitleArea.Width, this.TitleArea.Height);
            this.YAxisArea.Location = new PointF(this.YAxisLabelArea.Left + this.YAxisLabelArea.Width, this.TitleArea.Height);
            this.PlotArea.Location = new PointF(this.YAxisArea.Left + this.YAxisArea.Width, this.TitleArea.Height);
            this.PlotArea.Width = this.LegendArea.Left - this.PlotArea.Left;
            this.fXScale = this.PlotArea.Width / (this.nXSteps * this.fXStep);
            this.bXAxisLabelDoubleDecker = false;
            float num = 0f;
            for (int i = 1; i <= this.data.GetUpperBound(1); i++)
            {
                float width = graphicsObject.MeasureString(this.FormatXLabel(this.data[0, i]), this.axisLabelFont).Width;
                if (width > num)
                {
                    num = width;
                }
                if (width > (this.fXScale * this.fXStep))
                {
                    this.bXAxisLabelDoubleDecker = true;
                }
            }
            if (this.bXAxisLabelDoubleDecker)
            {
                this.XAxisLabelArea.Height = num + graphicsObject.MeasureString("X", this.axisLabelFont).Width;
            }
            else
            {
                this.XAxisLabelArea.Height = 1.5f * graphicsObject.MeasureString("X", this.axisLabelFont).Height;
            }
            if ((!this.xAxisLabels || (this.GraphType == 4)) || (this.GraphType == 5))
            {
                this.XAxisLabelArea.Height = 0f;
            }
            this.XAxisTitleArea.Location = new PointF(this.PlotArea.Left, (this.componentPictureBox.Height - 1) - this.XAxisTitleArea.Height);
            if (this.fYMin >= 0.0)
            {
                this.XAxisLabelArea.Location = new PointF(this.PlotArea.Left, this.XAxisTitleArea.Top - this.XAxisLabelArea.Height);
                this.XAxisArea.Location = new PointF(this.PlotArea.Left, this.XAxisLabelArea.Top - this.XAxisArea.Height);
                this.PlotArea.Height = this.XAxisArea.Top - this.PlotArea.Top;
                this.fYScale = this.PlotArea.Height / (this.fYMax - this.fYMin);
            }
            else
            {
                this.PlotArea.Height = this.XAxisTitleArea.Top - this.PlotArea.Top;
                this.fYScale = this.PlotArea.Height / (this.fYMax - this.fYMin);
                this.XAxisArea.Location = new PointF(this.PlotArea.Left, this.PlotArea.Top + (this.fYScale * this.fYMax));
                this.XAxisLabelArea.Location = new PointF(this.PlotArea.Left, this.XAxisArea.Top + this.XAxisArea.Height);
            }
            this.XAxisTitleArea.Width = this.PlotArea.Width;
            this.XAxisLabelArea.Width = this.PlotArea.Width;
            this.XAxisArea.Width = this.PlotArea.Width;
            this.YAxisTitleArea.Height = this.PlotArea.Height;
            this.YAxisLabelArea.Height = this.PlotArea.Height;
            this.YAxisArea.Height = this.PlotArea.Height;
            this.LegendArea.Height = this.XAxisTitleArea.Top - this.LegendArea.Top;
            this.fXScale = this.PlotArea.Width / (this.nXSteps * this.fXStep);
            this.fYScale = this.PlotArea.Height / (this.fYMax - this.fYMin);
        }

        protected void SetDefaultFonts()
        {
            this.titleFont = new Font("Microsoft Sans Serif", 18f);
            this.legendFont = new Font("Microsoft Sans Serif", 9f);
            this.axisTitleFont = new Font("Microsoft Sans Serif", 9f);
            this.axisLabelFont = new Font("Microsoft Sans Serif", 9f);
            this.dataPointLabelFont = new Font("Microsoft Sans Serif", 9f);
        }

        protected void SetScale()
        {
            int upperBound;
            if (this.nGraphType == 1)
            {
                upperBound = this.data.GetUpperBound(1);
                this.nXSteps = Math.Min(5, upperBound - 1);
                this.fXStep = ((upperBound - 2) / this.nXSteps) + 1;
                if ((1.0 + (this.fXStep * (this.nXSteps - 1))) >= upperBound)
                {
                    this.nXSteps--;
                }
                if ((1.0 + (this.fXStep * (this.nXSteps - 1))) >= upperBound)
                {
                    this.nXSteps--;
                }
            }
            else
            {
                if (this.GraphType == 5)
                {
                    this.nXSteps = this.data.GetUpperBound(0);
                }
                else
                {
                    this.nXSteps = this.data.GetUpperBound(1);
                }
                this.fXStep = 1f;
            }
            if (this.autoScaleY && (this.nGraphType != 6))
            {
                this.fYMax = this.minimumYMax;
                this.fYMin = 0f;
                for (upperBound = 1; upperBound <= this.data.GetUpperBound(0); upperBound++)
                {
                    for (int i = 1; i <= this.data.GetUpperBound(1); i++)
                    {
                        if (Convert.ToSingle(this.data[upperBound, i]) > this.fYMax)
                        {
                            this.fYMax = Convert.ToSingle(this.data[upperBound, i]);
                        }
                        if (Convert.ToSingle(this.data[upperBound, i]) < this.fYMin)
                        {
                            this.fYMin = Convert.ToSingle(this.data[upperBound, i]);
                        }
                    }
                }
            }
            if (this.nGraphType == 6)
            {
                this.fXMax = Convert.ToSingle(this.data[0, 1]);
                this.fYMax = Convert.ToSingle(this.data[1, 1]);
                this.fYMin = Convert.ToSingle(this.data[1, 1]);
                for (upperBound = 2; upperBound <= this.data.GetUpperBound(1); upperBound++)
                {
                    if (Convert.ToSingle(this.data[0, upperBound]) > this.maxXValue)
                    {
                        this.maxXValue = Convert.ToSingle(this.data[0, upperBound]);
                    }
                    if (Convert.ToSingle(this.data[1, upperBound]) > this.fYMax)
                    {
                        this.fYMax = Convert.ToSingle(this.data[1, upperBound]);
                    }
                    if (Convert.ToSingle(this.data[1, upperBound]) < this.fYMin)
                    {
                        this.fYMin = Convert.ToSingle(this.data[1, upperBound]);
                    }
                }
            }
            if (this.nGraphType != 5)
            {
                if (this.autoScaleY)
                {
                    this.fYStep = (float)Math.Pow(10.0, (double)((int)Math.Log((double)(this.fYMax - this.fYMin), 10.0)));
                    if (((this.fYMax - this.fYMin) / this.fYStep) > 6f)
                    {
                        this.fYStep *= 2f;
                    }
                    else
                    {
                        if (((this.fYMax - this.fYMin) / this.fYStep) < 4f)
                        {
                            this.fYStep /= 2f;
                        }
                        if (((this.fYMax - this.fYMin) / this.fYStep) < 4f)
                        {
                            this.fYStep /= 2f;
                        }
                    }
                    if (((int)(this.fYMin / this.fYStep)) != (this.fYMin / this.fYStep))
                    {
                        if (this.fYMin >= 0f)
                        {
                            this.fYMin = ((int)(this.fYMin / this.fYStep)) * this.fYStep;
                        }
                        else
                        {
                            this.fYMin = (((int)(this.fYMin / this.fYStep)) - 1) * this.fYStep;
                        }
                    }
                    if (((int)(this.fYMax / this.fYStep)) != (this.fYMax / this.fYStep))
                    {
                        this.fYMax = (((int)(this.fYMax / this.fYStep)) + 1) * this.fYStep;
                    }
                    this.nYSteps = (int)((this.fYMax - this.fYMin) / this.fYStep);
                }
                else
                {
                    this.fYStep = (this.fYMax - this.fYMin) / ((float)this.nYSteps);
                }
            }
        }

        public bool AutoScaleY
        {
            get
            {
                return this.autoScaleY;
            }
            set
            {
                this.autoScaleY = value;
            }
        }

        public float AxisLabelFontSize
        {
            get
            {
                return this.axisLabelFont.Size;
            }
            set
            {
                this.axisLabelFont = new Font("Microsoft Sans Serif", value);
            }
        }

        public float AxisTitleFontSize
        {
            get
            {
                return this.axisTitleFont.Size;
            }
            set
            {
                this.axisTitleFont = new Font("Microsoft Sans Serif", value);
            }
        }

        public object[,] Data
        {
            get
            {
                return this.data;
            }
            set
            {
                this.data = value;
            }
        }

        public float DataPointLabelFontSize
        {
            get
            {
                return this.dataPointLabelFont.Size;
            }
            set
            {
                this.dataPointLabelFont = new Font("Microsoft Sans Serif", value);
            }
        }

        public bool DataPointLabels
        {
            get
            {
                return this.bDataPointLabels;
            }
            set
            {
                this.bDataPointLabels = value;
            }
        }

        public int GraphType
        {
            get
            {
                return this.nGraphType;
            }
            set
            {
                this.nGraphType = value;
            }
        }

        public bool GridLinesX
        {
            get
            {
                return this.bGridLinesX;
            }
            set
            {
                this.bGridLinesX = value;
            }
        }

        public bool GridLinesY
        {
            get
            {
                return this.bGridLinesY;
            }
            set
            {
                this.bGridLinesY = value;
            }
        }

        public bool Legend
        {
            get
            {
                return this.bLegend;
            }
            set
            {
                this.bLegend = value;
            }
        }

        public float LegendFontSize
        {
            get
            {
                return this.legendFont.Size;
            }
            set
            {
                this.legendFont = new Font("Microsoft Sans Serif", value);
            }
        }

        public int LineWidth
        {
            get
            {
                return this.nLineWidth;
            }
            set
            {
                this.nLineWidth = value;
            }
        }

        public float MinimumYMax
        {
            get
            {
                return this.minimumYMax;
            }
            set
            {
                this.minimumYMax = value;
            }
        }

        public int PrinterMargin
        {
            get
            {
                return this.printerMargin;
            }
            set
            {
                this.printerMargin = value;
            }
        }

        public bool ShowPercentagesForHistograms
        {
            get
            {
                return this.showPercentagesForHistograms;
            }
            set
            {
                this.showPercentagesForHistograms = value;
            }
        }

        public bool ShowXTicks
        {
            get
            {
                return this.bShowXTicks;
            }
            set
            {
                this.bShowXTicks = value;
            }
        }

        public bool ShowYTicks
        {
            get
            {
                return this.bShowYTicks;
            }
            set
            {
                this.bShowYTicks = value;
            }
        }

        public string Title
        {
            get
            {
                return this.sTitle;
            }
            set
            {
                this.sTitle = value;
            }
        }

        public float TitleFontSize
        {
            get
            {
                return this.titleFont.Size;
            }
            set
            {
                this.titleFont = new Font("Microsoft Sans Serif", value);
            }
        }

        public bool XAxisLabels
        {
            get
            {
                return this.xAxisLabels;
            }
            set
            {
                this.xAxisLabels = value;
            }
        }

        public string XAxisTitle
        {
            get
            {
                return this.sXAxisTitle;
            }
            set
            {
                this.sXAxisTitle = value;
            }
        }

        public string XLabelFormat
        {
            get
            {
                return this.sXLabelFormat;
            }
            set
            {
                this.sXLabelFormat = value;
            }
        }

        public string YAxisTitle
        {
            get
            {
                return this.sYAxisTitle;
            }
            set
            {
                this.sYAxisTitle = value;
            }
        }

        public string YLabelFormat
        {
            get
            {
                return this.sYLabelFormat;
            }
            set
            {
                this.sYLabelFormat = value;
            }
        }

        public float YMax
        {
            get
            {
                return this.fYMax;
            }
            set
            {
                this.fYMax = value;
            }
        }

        public float YMin
        {
            get
            {
                return this.fYMin;
            }
            set
            {
                this.fYMin = value;
            }
        }

        public int YTicks
        {
            get
            {
                return (this.nYSteps + 1);
            }
            set
            {
                this.nYSteps = value - 1;
            }
        }
    }
}
