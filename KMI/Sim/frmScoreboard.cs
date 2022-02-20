namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmScoreboard : Form
    {
        private Button btnClose;
        private Button btnHelp;
        private Button btnPrint;
        private Button btnReplay;
        private IContainer components;
        protected const int DataPointLabelFontSize = 9;
        protected const int DefaultFormHeight = 280;
        protected const int DefaultFormWidth = 0x108;
        public static int DefaultInitialScoreScale = 0xc350;
        protected const int DefaultLegendFontSize = 9;
        protected const int DefaultTitleFontSize = 14;
        private Input input;
        private KMIGraph kmiGraph1;
        private Label labEmptyMessage;
        private Panel panel1;
        private Panel panel2;
        public static bool ShowAIOwnedEntities = true;
        private int step;
        private Timer timer1;
        public static bool UpdateDaily = false;

        public frmScoreboard()
        {
            this.InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp("Scoreboard");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.kmiGraph1.Title = this.Text;
            this.kmiGraph1.PrintGraph();
            this.kmiGraph1.Title = "";
        }

        private void btnReplay_Click(object sender, EventArgs e)
        {
            if (this.btnReplay.Text == "Stop")
            {
                this.timer1.Stop();
                this.btnReplay.Text = "Replay";
                this.UpdateForm();
            }
            else
            {
                this.step = 0;
                this.btnReplay.Text = "Stop";
                this.timer1.Start();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmScoreboard_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            if (UpdateDaily)
            {
                S.MainForm.NewDay -= new EventHandler(this.NewWeekHandler);
            }
        }

        private void frmScoreboard_Load(object sender, EventArgs e)
        {
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.Legend = false;
            this.kmiGraph1.GraphType = 5;
            S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
            if (UpdateDaily)
            {
                S.MainForm.NewDay += new EventHandler(this.NewWeekHandler);
            }
            if (this.GetData())
            {
                this.Text = "Scoreboard - " + this.input.ScoreFriendlyName;
                this.SetYScale();
                this.UpdateForm();
            }
        }

        private void frmScoreboard_Resize(object sender, EventArgs e)
        {
            float num = Math.Min((float) (((float) base.Size.Width) / 264f), (float) (((float) base.Size.Height) / 280f));
            num = Math.Max(1f, num);
            this.kmiGraph1.TitleFontSize = num * 14f;
            this.kmiGraph1.DataPointLabelFontSize = num * 9f;
            this.kmiGraph1.LegendFontSize = num * 9f;
        }

        protected bool GetData()
        {
            try
            {
                this.input = S.Adapter.getScoreboard(ShowAIOwnedEntities);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.kmiGraph1 = new KMIGraph();
            this.panel1 = new Panel();
            this.btnReplay = new Button();
            this.btnPrint = new Button();
            this.panel2 = new Panel();
            this.btnHelp = new Button();
            this.btnClose = new Button();
            this.timer1 = new Timer(this.components);
            this.labEmptyMessage = new Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 9f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.BackColor = Color.White;
            this.kmiGraph1.Data = null;
            this.kmiGraph1.DataPointLabelFontSize = 9f;
            this.kmiGraph1.DataPointLabels = true;
            this.kmiGraph1.Dock = DockStyle.Fill;
            this.kmiGraph1.GraphType = 1;
            this.kmiGraph1.GridLinesX = false;
            this.kmiGraph1.GridLinesY = false;
            this.kmiGraph1.Legend = true;
            this.kmiGraph1.LegendFontSize = 9f;
            this.kmiGraph1.LineWidth = 3;
            this.kmiGraph1.Location = new Point(0, 0);
            this.kmiGraph1.MinimumYMax = 1f;
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowPercentagesForHistograms = false;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0x100, 0xde);
            this.kmiGraph1.TabIndex = 5;
            this.kmiGraph1.Title = null;
            this.kmiGraph1.TitleFontSize = 14f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = "";
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.panel1.Controls.Add(this.btnReplay);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0xde);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x100, 0x20);
            this.panel1.TabIndex = 6;
            this.btnReplay.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnReplay.Location = new Point(0x40, 8);
            this.btnReplay.Name = "btnReplay";
            this.btnReplay.Size = new Size(40, 20);
            this.btnReplay.TabIndex = 9;
            this.btnReplay.Text = "Replay";
            this.btnReplay.Click += new EventHandler(this.btnReplay_Click);
            this.btnPrint.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnPrint.Location = new Point(8, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(40, 20);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(120, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x88, 0x20);
            this.panel2.TabIndex = 0;
            this.btnHelp.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnHelp.Location = new Point(0x48, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x38, 20);
            this.btnHelp.TabIndex = 11;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.btnClose.Location = new Point(8, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x38, 20);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.timer1.Interval = 500;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.labEmptyMessage.CausesValidation = false;
            this.labEmptyMessage.Font = new Font("Microsoft Sans Serif", 18f);
            this.labEmptyMessage.ForeColor = SystemColors.ControlDark;
            this.labEmptyMessage.Location = new Point(0, 0);
            this.labEmptyMessage.Name = "labEmptyMessage";
            this.labEmptyMessage.Size = new Size(0x100, 0xd8);
            this.labEmptyMessage.TabIndex = 7;
            this.labEmptyMessage.Text = "No scores are available yet";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x100, 0xfe);
            base.Controls.Add(this.kmiGraph1);
            base.Controls.Add(this.labEmptyMessage);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new Size(200, 0xd8);
            base.Name = "frmScoreboard";
            base.ShowInTaskbar = false;
            this.Text = "Scoreboard";
            base.Resize += new EventHandler(this.frmScoreboard_Resize);
            base.Load += new EventHandler(this.frmScoreboard_Load);
            base.Closed += new EventHandler(this.frmScoreboard_Closed);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.SetYScale();
                this.UpdateForm();
            }
        }

        protected void SetYScale()
        {
            float defaultInitialScoreScale = DefaultInitialScoreScale;
            float num2 = 0f;
            for (int i = 0; i < this.input.Scores.Length; i++)
            {
                foreach (float num4 in this.input.Scores[i])
                {
                    defaultInitialScoreScale = Math.Max(num4, defaultInitialScoreScale);
                    num2 = Math.Min(num4, num2);
                }
            }
            this.kmiGraph1.YMax = defaultInitialScoreScale;
            this.kmiGraph1.YMin = num2;
            this.kmiGraph1.AutoScaleY = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.input.Scores.Length == 0)
            {
                this.btnReplay.PerformClick();
            }
            else if (this.step < this.input.Scores[0].Count)
            {
                int step = this.step;
                this.step = step + 1;
                this.UpdateForm(step);
            }
            else
            {
                this.timer1.Stop();
                this.btnReplay.Text = "Replay";
            }
        }

        protected virtual void UpdateForm()
        {
            try
            {
                if (this.input.Scores.Length == 0)
                {
                    this.kmiGraph1.Visible = false;
                }
                else
                {
                    this.kmiGraph1.Visible = true;
                    this.UpdateForm(this.input.Scores[0].Count - 1);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        protected virtual void UpdateForm(int index)
        {
            try
            {
                if (index >= 0)
                {
                    object[,] d = new object[this.input.EntityNames.Length + 1, 2];
                    for (int i = 0; i < this.input.EntityNames.Length; i++)
                    {
                        if (index >= this.input.Scores[i].Count)
                        {
                            return;
                        }
                        d[i + 1, 0] = this.input.EntityNames[i];
                        d[i + 1, 1] = (float) this.input.Scores[i][index];
                    }
                    this.kmiGraph1.Draw(d);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public string[] EntityNames;
            public ArrayList[] Scores;
            public string ScoreFriendlyName;
        }
    }
}

