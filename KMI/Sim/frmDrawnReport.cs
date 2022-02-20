namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Printing;
    using System.Windows.Forms;

    public class frmDrawnReport : Form
    {
        private Button btnClose;
        private Button btnHelp;
        private Button btnPrint;
        private Container components = null;
        public MenuItem EnablingReference;
        protected PictureBox picReport;
        protected Panel pnlBottom;
        private Panel pnlButtons;
        private Panel pnlMain;
        protected string studentName;

        public frmDrawnReport()
        {
            this.InitializeComponent();
        }

        protected virtual void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected virtual void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        protected virtual void btnPrint_Click(object sender, EventArgs e)
        {
            this.studentName = "";
            frmInputString str = new frmInputString(S.Resources.GetString("Student Name"), S.Resources.GetString("Enter your name to help identify your printout on a shared printer:"), this.studentName);
            str.ShowDialog(this);
            this.studentName = str.Response;
            Utilities.PrintWithExceptionHandling("", new PrintPageEventHandler(this.Report_PrintPage));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void DrawReport(Graphics g)
        {
            try
            {
                this.DrawReportVirtual(g);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        protected virtual void DrawReportVirtual(Graphics g)
        {
        }

        protected virtual void EntityChangedHandler(object sender, EventArgs e)
        {
            if ((this.EnablingReference != null) && !this.EnablingReference.Enabled)
            {
                base.Close();
            }
            else if (this.GetData())
            {
                this.picReport.Refresh();
            }
        }

        protected virtual void frmReport_Closed(object sender, EventArgs e)
        {
            S.MainForm.NewWeek -= new EventHandler(this.NewWeekHandler);
            S.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        protected virtual void frmReport_Load(object sender, EventArgs e)
        {
            if ((this.EnablingReference == null) && !base.DesignMode)
            {
                throw new Exception("Enabling reference not set in " + this.Text);
            }
            if (S.MainForm != null)
            {
                S.MainForm.NewWeek += new EventHandler(this.NewWeekHandler);
                S.MainForm.EntityChanged += new EventHandler(this.EntityChangedHandler);
            }
            if (this.GetData())
            {
                this.picReport.Refresh();
            }
        }

        protected bool GetData()
        {
            try
            {
                this.GetDataVirtual();
                return true;
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
            return false;
        }

        protected virtual void GetDataVirtual()
        {
        }

        private void InitializeComponent()
        {
            this.picReport = new PictureBox();
            this.pnlMain = new Panel();
            this.pnlBottom = new Panel();
            this.pnlButtons = new Panel();
            this.btnHelp = new Button();
            this.btnClose = new Button();
            this.btnPrint = new Button();
            this.pnlMain.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            base.SuspendLayout();
            this.picReport.Location = new Point(0x10, 0x10);
            this.picReport.Name = "picReport";
            this.picReport.Size = new Size(0x198, 280);
            this.picReport.TabIndex = 0;
            this.picReport.TabStop = false;
            this.picReport.Paint += new PaintEventHandler(this.picReport_Paint);
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.picReport);
            this.pnlMain.Dock = DockStyle.Fill;
            this.pnlMain.Location = new Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new Size(440, 0x158);
            this.pnlMain.TabIndex = 0;
            this.pnlBottom.Controls.Add(this.pnlButtons);
            this.pnlBottom.Dock = DockStyle.Bottom;
            this.pnlBottom.Location = new Point(0, 0x130);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new Size(440, 40);
            this.pnlBottom.TabIndex = 1;
            this.pnlButtons.Controls.Add(this.btnHelp);
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnPrint);
            this.pnlButtons.Dock = DockStyle.Right;
            this.pnlButtons.Location = new Point(0x108, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new Size(0xb0, 40);
            this.pnlButtons.TabIndex = 0;
            this.btnHelp.Location = new Point(120, 8);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x17);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Location = new Point(0x40, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x30, 0x17);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnPrint.Location = new Point(8, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x30, 0x17);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            base.AcceptButton = this.btnClose;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnClose;
            base.ClientSize = new Size(440, 0x158);
            base.Controls.Add(this.pnlBottom);
            base.Controls.Add(this.pnlMain);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "frmDrawnReport";
            base.ShowInTaskbar = false;
            this.Text = "frmDrawnReport";
            base.Load += new EventHandler(this.frmReport_Load);
            base.Closed += new EventHandler(this.frmReport_Closed);
            this.pnlMain.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        protected virtual void NewWeekHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.picReport.Refresh();
            }
        }

        protected virtual void picReport_Paint(object sender, PaintEventArgs e)
        {
            this.DrawReport(e.Graphics);
        }

        protected virtual void Report_PrintPage(object sender, PrintPageEventArgs e)
        {
            Utilities.ResetFPU();
            if (this.studentName.Length > 0)
            {
                Font font = new Font("Arial", 10f);
                Brush brush = new SolidBrush(Color.Black);
                object[] args = new object[] { this.studentName };
                e.Graphics.DrawString(S.Resources.GetString("This report belongs to: {0}", args), font, brush, (float) 0f, (float) 0f);
                e.Graphics.TranslateTransform(0f, 2f * e.Graphics.MeasureString(this.studentName, font).Height);
            }
            this.DrawReport(e.Graphics);
        }
    }
}

