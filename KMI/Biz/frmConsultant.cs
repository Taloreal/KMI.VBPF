namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmConsultant : Form
    {
        private Button btnClose;
        private Button btnHelp;
        private Button btnPrint;
        private CheckBox chkFullReport;
        private Container components = null;
        private Panel panel1;
        private Panel panel2;
        private Panel panReportArea;
        private PictureBox picReport;
        protected ConsultantReport report;

        public frmConsultant()
        {
            this.InitializeComponent();
            this.report = ((BizStateAdapter) Simulator.Instance.SimStateAdapter).GetConsultantReport(frmMainBase.Instance.CurrentEntityID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp("Consultant");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.report.PrintToPrinter(this.chkFullReport.Checked, false);
        }

        private void chkFullReport_Click(object sender, EventArgs e)
        {
            this.picReport.Refresh();
        }

        private void chkGrades_Click(object sender, EventArgs e)
        {
            this.picReport.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmConsultant_Load(object sender, EventArgs e)
        {
        }

        private void frmConsultant_Resize(object sender, EventArgs e)
        {
            this.picReport.Width = (this.panReportArea.ClientRectangle.Width - this.picReport.Left) - 20;
            this.picReport.Refresh();
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.btnClose = new Button();
            this.btnHelp = new Button();
            this.btnPrint = new Button();
            this.chkFullReport = new CheckBox();
            this.panReportArea = new Panel();
            this.picReport = new PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panReportArea.SuspendLayout();
            ((ISupportInitialize) this.picReport).BeginInit();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Controls.Add(this.chkFullReport);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x16e);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(520, 80);
            this.panel1.TabIndex = 1;
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x180, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x88, 80);
            this.panel2.TabIndex = 4;
            this.btnClose.Location = new Point(0x10, 0x10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x60, 0x18);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.btnHelp.Location = new Point(0x10, 0x30);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnPrint.Location = new Point(0x10, 0x10);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x48, 0x38);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.chkFullReport.Location = new Point(0xb0, 0x20);
            this.chkFullReport.Name = "chkFullReport";
            this.chkFullReport.Size = new Size(0x70, 0x10);
            this.chkFullReport.TabIndex = 0;
            this.chkFullReport.Text = "Show Full Report";
            this.chkFullReport.Click += new EventHandler(this.chkFullReport_Click);
            this.panReportArea.AutoScroll = true;
            this.panReportArea.BackColor = Color.White;
            this.panReportArea.Controls.Add(this.picReport);
            this.panReportArea.Dock = DockStyle.Fill;
            this.panReportArea.Location = new Point(0, 0);
            this.panReportArea.Name = "panReportArea";
            this.panReportArea.Size = new Size(520, 0x16e);
            this.panReportArea.TabIndex = 0;
            this.picReport.Location = new Point(0x18, 0x18);
            this.picReport.Name = "picReport";
            this.picReport.Size = new Size(0x1d8, 0x148);
            this.picReport.TabIndex = 0;
            this.picReport.TabStop = false;
            this.picReport.Paint += new PaintEventHandler(this.picReport_Paint);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(520, 0x1be);
            base.Controls.Add(this.panReportArea);
            base.Controls.Add(this.panel1);
            this.MinimumSize = new Size(0x198, 280);
            base.Name = "frmConsultant";
            base.ShowInTaskbar = false;
            this.Text = "Consultant's Report";
            base.Load += new EventHandler(this.frmConsultant_Load);
            base.Resize += new EventHandler(this.frmConsultant_Resize);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panReportArea.ResumeLayout(false);
            ((ISupportInitialize) this.picReport).EndInit();
            base.ResumeLayout(false);
        }

        private void picReport_Paint(object sender, PaintEventArgs e)
        {
            this.picReport.Height = this.report.PrintToScreen(this.picReport.Width, e.Graphics, this.chkFullReport.Checked, false);
        }
    }
}

