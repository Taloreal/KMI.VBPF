namespace KMI.Sim.Academics
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmPage : Form
    {
        private Button btnPrint;
        private Button btnQuestions;
        private Container components = null;
        public bool okToClose = false;
        private Page page;
        private Panel panel1;
        private Panel panel2;

        public frmPage(Page page)
        {
            try
            {
                this.InitializeComponent();
            }
            catch (COMException)
            {
            }
            this.page = page;
        }

        private void btnQuestions_Click(object sender, EventArgs e)
        {
            new frmQuestions(frmQuestions.Modes.Quiz, this.page.Questions).ShowDialog(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmPage_Closing(object sender, CancelEventArgs e)
        {
            if (!this.okToClose)
            {
                e.Cancel = true;
            }
        }

        private void frmPage_Load(object sender, EventArgs e)
        {
            object obj2 = new object();
            object obj3 = AcademicGod.PageBankPath + Path.DirectorySeparatorChar.ToString() + this.page.BodyURL;
        }

        private void InitializeComponent()
        {
            ResourceManager manager = new ResourceManager(typeof(frmPage));
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.btnQuestions = new Button();
            this.btnPrint = new Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = DockStyle.Bottom;
            this.panel1.Location = new Point(0, 0x1ee);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x318, 40);
            this.panel1.TabIndex = 1;
            this.panel2.Controls.Add(this.btnQuestions);
            this.panel2.Controls.Add(this.btnPrint);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x1d0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x148, 40);
            this.panel2.TabIndex = 2;
            this.btnQuestions.Location = new Point(0x68, 8);
            this.btnQuestions.Name = "btnQuestions";
            this.btnQuestions.Size = new Size(0x70, 0x18);
            this.btnQuestions.TabIndex = 1;
            this.btnQuestions.Text = "Answer Questions";
            this.btnQuestions.Click += new EventHandler(this.btnQuestions_Click);
            this.btnPrint.Location = new Point(0x10, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x40, 0x18);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x318, 0x216);
            base.Controls.Add(this.panel1);
            this.MinimumSize = new Size(0x148, 160);
            base.Name = "frmPage";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Concept";
            base.Closing += new CancelEventHandler(this.frmPage_Closing);
            base.Load += new EventHandler(this.frmPage_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

