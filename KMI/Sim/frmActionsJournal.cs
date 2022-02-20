namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmActionsJournal : Form
    {
        private Button btnClose;
        private Button btnHelp;
        private Button btnPrint;
        private Container components = null;
        protected MenuItem enablingReference;
        private Input input;
        protected ArrayList journals;
        private KMIGraph kmiGraph1;
        protected ArrayList mergedEntries;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panGraph;
        private int printEntry;
        private int printPage;
        private Splitter splitter1;
        protected DateTime startDate;
        private string studentName;
        private TextBox txtEntries;
        protected ArrayList useEntries;

        public frmActionsJournal(bool showGraph)
        {
            this.InitializeComponent();
            this.panGraph.Visible = showGraph;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(S.Resources.GetString("Actions Journal"));
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.studentName = "";
            frmInputString str = new frmInputString(S.Resources.GetString("Student Name"), S.Resources.GetString("Enter your name to help identify your printout on a shared printer:"), this.studentName);
            str.ShowDialog(this);
            this.studentName = str.Response;
            this.printEntry = 0;
            this.printPage = 1;
            Utilities.PrintWithExceptionHandling("", new PrintPageEventHandler(this.Journal_PrintPage));
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
            if (!this.enablingReference.Enabled)
            {
                base.Close();
            }
            else if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        private void frmActionsJournal_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        private void frmActionsJournal_Load(object sender, EventArgs e)
        {
            if (this.EnablingReference == null)
            {
                throw new Exception("Enabling reference not set in " + this.Text);
            }
            S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged += new EventHandler(this.EntityChangedHandler);
            if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        protected bool GetData()
        {
            try
            {
                if (!S.Instance.Client && S.Instance.Multiplayer)
                {
                    this.input = S.Adapter.getActionsJournal(-1L);
                }
                else
                {
                    this.input = S.Adapter.getActionsJournal(S.MainForm.CurrentEntityID);
                }
                this.journals = this.input.Journals;
                if (this.journals.Count == 1)
                {
                    this.Text = "Actions Journal for " + S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID);
                }
                this.startDate = this.input.StartDate;
                this.LoadNumericDataSeriesNames();
                this.mergedEntries = new ArrayList();
                foreach (Journal journal in this.journals)
                {
                    foreach (JournalEntry entry in journal.Entries)
                    {
                        for (int i = this.mergedEntries.Count - 1; i >= -1; i--)
                        {
                            if (i == -1)
                            {
                                this.mergedEntries.Insert(0, entry);
                                break;
                            }
                            if (entry.TimeStamp >= ((JournalEntry) this.mergedEntries[i]).TimeStamp)
                            {
                                this.mergedEntries.Insert(i + 1, entry);
                                break;
                            }
                        }
                    }
                }
                this.useEntries = this.mergedEntries;
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.kmiGraph1 = new KMIGraph();
            this.panel1 = new Panel();
            this.btnPrint = new Button();
            this.panel2 = new Panel();
            this.btnHelp = new Button();
            this.btnClose = new Button();
            this.panel3 = new Panel();
            this.splitter1 = new Splitter();
            this.txtEntries = new TextBox();
            this.panGraph = new Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panGraph.SuspendLayout();
            base.SuspendLayout();
            this.kmiGraph1.AutoScaleY = true;
            this.kmiGraph1.AxisLabelFontSize = 9f;
            this.kmiGraph1.AxisTitleFontSize = 9f;
            this.kmiGraph1.BackColor = SystemColors.Control;
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
            this.kmiGraph1.Location = new Point(8, 8);
            this.kmiGraph1.Name = "kmiGraph1";
            this.kmiGraph1.PrinterMargin = 100;
            this.kmiGraph1.ShowXTicks = true;
            this.kmiGraph1.ShowYTicks = true;
            this.kmiGraph1.Size = new Size(0x218, 0x88);
            this.kmiGraph1.TabIndex = 0;
            this.kmiGraph1.Title = null;
            this.kmiGraph1.TitleFontSize = 18f;
            this.kmiGraph1.XAxisLabels = true;
            this.kmiGraph1.XAxisTitle = null;
            this.kmiGraph1.XLabelFormat = null;
            this.kmiGraph1.YAxisTitle = null;
            this.kmiGraph1.YLabelFormat = "";
            this.kmiGraph1.YMax = 0f;
            this.kmiGraph1.YMin = 0f;
            this.kmiGraph1.YTicks = 1;
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x17e);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x228, 40);
            this.panel1.TabIndex = 7;
            this.btnPrint.Location = new Point(0x18, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x60, 0x18);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print Journal";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x138, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(240, 40);
            this.panel2.TabIndex = 1;
            this.btnHelp.Location = new Point(0x80, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x68, 0x18);
            this.btnHelp.TabIndex = 1;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Location = new Point(0x10, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x68, 0x18);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.txtEntries);
            this.panel3.Controls.Add(this.panGraph);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x228, 0x1a6);
            this.panel3.TabIndex = 8;
            this.splitter1.BackColor = SystemColors.ControlDark;
            this.splitter1.Dock = DockStyle.Bottom;
            this.splitter1.Location = new Point(0, 0xe4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(0x228, 2);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            this.txtEntries.BackColor = Color.White;
            this.txtEntries.Dock = DockStyle.Fill;
            this.txtEntries.Location = new Point(0, 0);
            this.txtEntries.Multiline = true;
            this.txtEntries.Name = "txtEntries";
            this.txtEntries.ReadOnly = true;
            this.txtEntries.ScrollBars = ScrollBars.Both;
            this.txtEntries.Size = new Size(0x228, 230);
            this.txtEntries.TabIndex = 0;
            this.txtEntries.Text = "textBox1";
            this.txtEntries.WordWrap = false;
            this.panGraph.Controls.Add(this.kmiGraph1);
            this.panGraph.Dock = DockStyle.Bottom;
            this.panGraph.DockPadding.All = 8;
            this.panGraph.Location = new Point(0, 230);
            this.panGraph.Name = "panGraph";
            this.panGraph.Size = new Size(0x228, 0x98);
            this.panGraph.TabIndex = 6;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x228, 0x1a6);
            base.Controls.Add(this.panel3);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(0x180, 0x158);
            base.Name = "frmActionsJournal";
            base.ShowInTaskbar = false;
            this.Text = "Actions Journal";
            base.Load += new EventHandler(this.frmActionsJournal_Load);
            base.Closed += new EventHandler(this.frmActionsJournal_Closed);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panGraph.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void Journal_PrintPage(object sender, PrintPageEventArgs e)
        {
            string str;
            Utilities.ResetFPU();
            Font font = new Font("Arial", 12f);
            Brush brush = new SolidBrush(Color.Black);
            StringFormat format = new StringFormat {
                Trimming = StringTrimming.Word
            };
            StringFormat format2 = new StringFormat {
                Alignment = StringAlignment.Center
            };
            string str2 = "\r\n";
            string[] strArray = new string[] { "A", "B", "C", "D", "E", "F", "G" };
            Graphics graphics = e.Graphics;
            RectangleF layoutRectangle = new RectangleF((float) e.MarginBounds.Left, (float) e.MarginBounds.Top, (float) e.MarginBounds.Width, (float) e.MarginBounds.Height);
            if (this.printPage == 1)
            {
                str = this.Text + S.Resources.GetString(" -  Submitted by ") + this.studentName;
            }
            else
            {
                str = this.Text + " (continued)";
            }
            str = str + str2 + str2;
            graphics.DrawString(str, font, brush, (float) e.MarginBounds.Left, layoutRectangle.Top, format);
            layoutRectangle.Y += graphics.MeasureString(str, font, (int) layoutRectangle.Width, format).Height;
            layoutRectangle.Height -= graphics.MeasureString(str, font, (int) layoutRectangle.Width, format).Height;
            while (this.printEntry < this.useEntries.Count)
            {
                JournalEntry entry = (JournalEntry) this.useEntries[this.printEntry];
                string text = string.Format("{0:dd MMM yy}", entry.TimeStamp) + ": ";
                if (this.journals.Count != 1)
                {
                    text = text + entry.EntityName + " - ";
                }
                text = text + entry.Description + str2;
                SizeF ef3 = graphics.MeasureString(text, font, (int) layoutRectangle.Width, format);
                if (ef3.Height >= layoutRectangle.Height)
                {
                    break;
                }
                graphics.DrawString(text, font, brush, layoutRectangle, format);
                layoutRectangle.Y += ef3.Height;
                layoutRectangle.Height -= ef3.Height;
                this.printEntry++;
            }
            this.printPage++;
            e.HasMorePages = this.printEntry < this.useEntries.Count;
        }

        protected virtual void LoadNumericDataSeriesNames()
        {
        }

        protected void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.UpdateForm();
            }
        }

        protected void ScrollToEnd()
        {
            this.txtEntries.Focus();
            this.txtEntries.Select(Math.Max(0, this.txtEntries.TextLength - 2), 0);
            this.txtEntries.ScrollToCaret();
        }

        private void UpdateActions()
        {
            this.txtEntries.Text = "";
            foreach (JournalEntry entry in this.useEntries)
            {
                this.txtEntries.Text = this.txtEntries.Text + string.Format("{0:dd MMM yy}", entry.TimeStamp) + ": ";
                if (this.journals.Count != 1)
                {
                    this.txtEntries.Text = this.txtEntries.Text + entry.EntityName + " - ";
                }
                this.txtEntries.Text = this.txtEntries.Text + entry.Description + "\r\n";
            }
            this.ScrollToEnd();
        }

        protected void UpdateForm()
        {
            try
            {
                this.UpdateActions();
                this.UpdateGraph();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        protected virtual void UpdateGraph()
        {
            if (this.panGraph.Visible)
            {
                Journal journal = (Journal) this.journals[0];
                if (this.journals.Count == 1)
                {
                    this.kmiGraph1.Title = Journal.JournalSeriesName;
                    this.kmiGraph1.Legend = false;
                }
                else
                {
                    this.kmiGraph1.Title = Journal.ScoreSeriesName;
                    this.kmiGraph1.Legend = true;
                }
                this.kmiGraph1.TitleFontSize = 8f;
                this.kmiGraph1.AxisLabelFontSize = 8f;
                this.kmiGraph1.GraphType = 1;
                int periods = 0;
                foreach (Journal journal2 in this.journals)
                {
                    if (journal2.Periods > periods)
                    {
                        periods = journal2.Periods;
                    }
                }
                object[,] d = new object[this.journals.Count + 1, periods + 1];
                for (int i = 0; i < d.GetUpperBound(1); i++)
                {
                    d[0, i + 1] = this.startDate.AddDays((double) (i * journal.DaysPerPeriod));
                }
                int num2 = 0;
                foreach (Journal journal3 in this.journals)
                {
                    d[num2 + 1, 0] = journal3.EntityName;
                    ArrayList list = journal3.NumericDataSeries(this.kmiGraph1.Title);
                    for (int j = 0; j < list.Count; j++)
                    {
                        d[num2 + 1, j + 1] = list[j];
                    }
                    num2++;
                }
                this.kmiGraph1.Draw(d);
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
            public ArrayList Journals;
            public DateTime StartDate;
            public DateTime EndDate;
        }
    }
}

