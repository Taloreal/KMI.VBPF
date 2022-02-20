namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class frmViewComments : frmDrawnReport
    {
        private Button btnBack;
        private Button btnNext;
        private int index = 0;
        private CommentLog input;
        private PrintPageEventArgs printArgs;
        private int printStartIndex = 0;

        public frmViewComments()
        {
            this.InitializeComponent();
            frmMainBase.Instance.NewDay += new EventHandler(this.NewWeekHandler);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.index--;
            this.btnBack.Enabled = false;
            this.btnNext.Enabled = false;
            if ((this.index > 1) && (this.input.Comments[this.index - 1] != null))
            {
                this.btnBack.Enabled = true;
            }
            if (this.index < (this.input.Comments.Count - 2))
            {
                this.btnNext.Enabled = true;
            }
            this.printStartIndex = 0;
            base.picReport.Refresh();
        }

        protected override void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.index++;
            this.btnBack.Enabled = false;
            this.btnNext.Enabled = false;
            if ((this.index > 1) && (this.input.Comments[this.index - 1] != null))
            {
                this.btnBack.Enabled = true;
            }
            if (this.index < (this.input.Comments.Count - 2))
            {
                this.btnNext.Enabled = true;
            }
            this.printStartIndex = 0;
            base.picReport.Refresh();
        }

        protected override void btnPrint_Click(object sender, EventArgs e)
        {
            this.printStartIndex = 0;
            base.studentName = "";
            frmInputString str = new frmInputString(S.Resources.GetString("Student Name"), S.Resources.GetString("Enter your name to help identify your printout on a shared printer:"), base.studentName);
            str.ShowDialog(this);
            base.studentName = str.Response;
            Utilities.PrintWithExceptionHandling("", new PrintPageEventHandler(this.Report_PrintPage));
        }

        protected override void DrawReportVirtual(Graphics g)
        {
            int num = 0;
            bool flag = num >= this.printStartIndex;
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Far
            };
            int num2 = 0;
            int num3 = 0;
            ArrayList comments = this.input.Comments;
            DateTime time = this.input.StartDate.AddDays((double) (this.index + this.input.FrequencyInDays));
            DateTime time2 = this.input.StartDate.AddDays((double) ((this.index + 2) * this.input.FrequencyInDays));
            num2 = 8;
            if (this.input.FrequencyInDays <= 1)
            {
                if (flag)
                {
                    object[] args = new object[] { time.AddDays(-1.0).ToString("MMMM d, yyyy") };
                    g.DrawString(Simulator.Instance.Resource.GetString("Comments for {0}", args), new Font("Arial", 10f, FontStyle.Bold), Brushes.Black, new RectangleF((float) num3, (float) num2, (float) (base.picReport.Width - 1), 32f));
                }
            }
            else if (flag)
            {
                object[] objArray2 = new object[] { time.ToString("MMMM d, yyyy"), time2.ToString("MMMM d, yyyy") };
                g.DrawString(Simulator.Instance.Resource.GetString("Comments for the period {0} - {1}", objArray2), new Font("Arial", 10f, FontStyle.Bold), Brushes.Black, new RectangleF((float) num3, (float) num2, (float) (base.picReport.Width - 1), 32f));
            }
            num2 = 40;
            Hashtable hashtable = (Hashtable) comments[this.index];
            foreach (DictionaryEntry entry in hashtable)
            {
                string s = entry.Key.ToString();
                if (s.Length > 0)
                {
                    num3 = 8;
                    if (flag)
                    {
                        g.DrawString(s, new Font("Arial", 10f, FontStyle.Bold), Brushes.Black, new RectangleF((float) num3, (float) num2, (float) (base.picReport.Width - 1), 32f));
                    }
                    num2 += 0x10;
                }
                Hashtable hashtable2 = (Hashtable) entry.Value;
                foreach (DictionaryEntry entry2 in hashtable2)
                {
                    string str2 = entry2.Key.ToString();
                    if (str2.Length > 0)
                    {
                        num3 = 0x10;
                        if (flag)
                        {
                            g.DrawString(str2, new Font("Arial", 9f, FontStyle.Bold), Brushes.Black, new RectangleF((float) num3, (float) num2, (float) (base.picReport.Width - 1), 32f));
                        }
                        num2 += 0x10;
                    }
                    Hashtable hashtable3 = (Hashtable) entry2.Value;
                    ArrayList list2 = new ArrayList();
                    foreach (DictionaryEntry entry3 in hashtable3)
                    {
                        list2.Add(entry3);
                    }
                    list2.Sort(new PairComparer());
                    foreach (DictionaryEntry entry4 in list2)
                    {
                        num3 = 0x20;
                        if (flag)
                        {
                            g.DrawString("(" + entry4.Value + ")", new Font("Arial", 8.25f), Brushes.Black, new RectangleF((float) num3, (float) num2, 40f, 30f), format);
                        }
                        num3 = 0x4b;
                        SizeF ef = g.MeasureString(entry4.Key.ToString(), new Font("Arial", 8.25f), new SizeF((float) (((base.picReport.Width - 1) - num3) - 0x10), (float) (base.picReport.Height - 1)));
                        if (flag)
                        {
                            g.DrawString(entry4.Key.ToString(), new Font("Arial", 8.25f), Brushes.Black, new RectangleF((float) num3, (float) num2, ef.Width, ef.Height));
                        }
                        num2 += 5 + ((int) ef.Height);
                        num++;
                        bool flag9 = flag;
                        flag = num >= this.printStartIndex;
                        if (flag9 != flag)
                        {
                            num2 = 0;
                        }
                        if (((this.printArgs != null) & flag) && (num2 >= (this.printArgs.PageSettings.Bounds.Height - 200)))
                        {
                            this.printStartIndex = num;
                            num2 = 0;
                            this.printArgs.HasMorePages = true;
                            return;
                        }
                    }
                    num2 += 0x10;
                }
            }
            base.picReport.Height = num2 + 0x30;
            if (this.printArgs != null)
            {
                this.printArgs.HasMorePages = false;
                this.printStartIndex = 0;
            }
        }

        protected override void frmReport_Closed(object sender, EventArgs e)
        {
            base.frmReport_Closed(sender, e);
            frmMainBase.Instance.NewDay -= new EventHandler(this.NewWeekHandler);
        }

        private void frmViewComments_Load(object sender, EventArgs e)
        {
        }

        protected override void GetDataVirtual()
        {
            this.input = ((BizStateAdapter) Simulator.Instance.SimStateAdapter).GetComments(frmMainBase.Instance.CurrentEntityID);
            this.index = this.input.Comments.Count - 2;
            this.btnNext.Enabled = false;
            this.btnBack.Enabled = false;
            if (this.input.Comments.Count > 2)
            {
                this.btnBack.Enabled = true;
            }
            this.printStartIndex = 0;
            base.picReport.Refresh();
        }

        private void InitializeComponent()
        {
            this.btnBack = new Button();
            this.btnNext = new Button();
            base.pnlBottom.SuspendLayout();
            ((ISupportInitialize) base.picReport).BeginInit();
            base.SuspendLayout();
            base.pnlBottom.Controls.Add(this.btnNext);
            base.pnlBottom.Controls.Add(this.btnBack);
            base.pnlBottom.Location = new Point(0, 0x20c);
            base.pnlBottom.Size = new Size(0x1aa, 40);
            base.pnlBottom.Controls.SetChildIndex(this.btnBack, 0);
            base.pnlBottom.Controls.SetChildIndex(this.btnNext, 0);
            base.picReport.BackColor = Color.White;
            base.picReport.Location = new Point(0, 0);
            base.picReport.Size = new Size(420, 500);
            base.picReport.Click += new EventHandler(this.picReport_Click);
            this.btnBack.Location = new Point(0x58, 8);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(0x38, 0x17);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "<< Back";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnNext.Location = new Point(0xa4, 8);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(0x38, 0x17);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1aa, 0x234);
            base.Name = "frmViewComments";
            this.Text = "Comment Log";
            base.Load += new EventHandler(this.frmViewComments_Load);
            base.Closed += new EventHandler(this.frmReport_Closed);
            base.pnlBottom.ResumeLayout(false);
            ((ISupportInitialize) base.picReport).EndInit();
            base.ResumeLayout(false);
        }

        private void picReport_Click(object sender, EventArgs e)
        {
        }

        protected override void Report_PrintPage(object sender, PrintPageEventArgs e)
        {
            this.printArgs = e;
            Utilities.ResetFPU();
            if (base.studentName.Length > 0)
            {
                Font font = new Font("Arial", 10f);
                Brush brush = new SolidBrush(Color.Black);
                object[] args = new object[] { base.studentName };
                e.Graphics.DrawString(S.Resources.GetString("This report belongs to: {0}", args), font, brush, (float) 0f, (float) 0f);
                e.Graphics.TranslateTransform(0f, 2f * e.Graphics.MeasureString(base.studentName, font).Height);
            }
            base.DrawReport(e.Graphics);
            this.printArgs = null;
        }

        protected void ReportUpdateHandler(object sender, EventArgs e)
        {
            base.GetData();
            this.printStartIndex = 0;
            base.picReport.Refresh();
        }

        private class PairComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DictionaryEntry entry = (DictionaryEntry) y;
                entry = (DictionaryEntry) x;
                return (((int) entry.Value) - ((int) entry.Value));
            }
        }
    }
}

