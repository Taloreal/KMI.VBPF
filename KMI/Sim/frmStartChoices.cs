namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmStartChoices : Form
    {
        private Button btnExit;
        private Button btnLessons;
        private Button btnMultiplayer;
        private Button btnProject;
        private Button btnSavedSims;
        private Button btnTutorial;
        private Container components = null;
        private Label label1;
        private Panel panel1;

        public frmStartChoices()
        {
            this.InitializeComponent();
            base.Size = this.panel1.Size;
            this.btnProject.Text = "\"New " + S.Instance.EntityName + "\" Project";
            if (S.Instance.VBC)
            {
                this.btnProject.Enabled = false;
                this.btnLessons.Enabled = false;
                this.btnMultiplayer.Enabled = false;
            }
            if (S.Instance.Academic)
            {
                this.btnMultiplayer.Visible = false;
                this.btnLessons.Visible = false;
                this.btnProject.Text = S.Resources.GetString("New Simulation");
                this.btnProject.Top = this.btnTutorial.Bottom + 0x20;
                this.btnSavedSims.Top = this.btnProject.Bottom + 0x20;
                this.btnExit.Top = this.btnSavedSims.Bottom + 0x20;
                base.Height -= 110;
                this.panel1.Height = base.Height;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            S.MainForm.mnuFileExit.PerformClick();
        }

        private void btnLessons_Click(object sender, EventArgs e)
        {
            S.MainForm.mnuFileOpenLesson.PerformClick();
        }

        private void btnMultiplayer_Click(object sender, EventArgs e)
        {
            frmDualChoiceDialog dialog = new frmDualChoiceDialog("Do you want to join an existing multiplayer session or start a new one for others to join?", "Join Existing Session", "Start New Session", true);
            switch (dialog.ShowDialog())
            {
                case DialogResult.Yes:
                    S.MainForm.mnuFileMultiplayerJoin.PerformClick();
                    break;

                case DialogResult.No:
                    S.MainForm.mnuFileMultiplayerStart.PerformClick();
                    break;
            }
        }

        private void btnProject_Click(object sender, EventArgs e)
        {
            S.MainForm.mnuFileNew.PerformClick();
        }

        private void btnSavedSims_Click(object sender, EventArgs e)
        {
            S.MainForm.mnuFileOpenSavedSim.PerformClick();
        }

        private void btnTutorial_Click(object sender, EventArgs e)
        {
            S.MainForm.mnuHelpTutorial.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmStartChoices_Closed(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.btnExit = new Button();
            this.btnSavedSims = new Button();
            this.btnMultiplayer = new Button();
            this.btnProject = new Button();
            this.btnLessons = new Button();
            this.btnTutorial = new Button();
            this.label1 = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnSavedSims);
            this.panel1.Controls.Add(this.btnMultiplayer);
            this.panel1.Controls.Add(this.btnProject);
            this.panel1.Controls.Add(this.btnLessons);
            this.panel1.Controls.Add(this.btnTutorial);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(280, 0x1a8);
            this.panel1.TabIndex = 0;
            this.btnExit.Location = new Point(0x20, 0x178);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new Size(0xd8, 0x18);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnSavedSims.Location = new Point(0x20, 0x130);
            this.btnSavedSims.Name = "btnSavedSims";
            this.btnSavedSims.Size = new Size(0xd8, 40);
            this.btnSavedSims.TabIndex = 5;
            this.btnSavedSims.Text = "Open a Saved Sim";
            this.btnSavedSims.Click += new EventHandler(this.btnSavedSims_Click);
            this.btnMultiplayer.Location = new Point(0x20, 0xe0);
            this.btnMultiplayer.Name = "btnMultiplayer";
            this.btnMultiplayer.Size = new Size(0xd8, 40);
            this.btnMultiplayer.TabIndex = 4;
            this.btnMultiplayer.Text = "Compete using Multiplayer";
            this.btnMultiplayer.Click += new EventHandler(this.btnMultiplayer_Click);
            this.btnProject.Location = new Point(0x20, 0xa8);
            this.btnProject.Name = "btnProject";
            this.btnProject.Size = new Size(0xd8, 40);
            this.btnProject.TabIndex = 3;
            this.btnProject.Text = "#";
            this.btnProject.Click += new EventHandler(this.btnProject_Click);
            this.btnLessons.Location = new Point(0x1f, 0x70);
            this.btnLessons.Name = "btnLessons";
            this.btnLessons.Size = new Size(0xd8, 40);
            this.btnLessons.TabIndex = 2;
            this.btnLessons.Text = "Find a Lesson";
            this.btnLessons.Click += new EventHandler(this.btnLessons_Click);
            this.btnTutorial.Location = new Point(0x20, 0x38);
            this.btnTutorial.Name = "btnTutorial";
            this.btnTutorial.Size = new Size(0xd8, 0x18);
            this.btnTutorial.TabIndex = 1;
            this.btnTutorial.Text = "Tutorial";
            this.btnTutorial.Click += new EventHandler(this.btnTutorial_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 20.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x30, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xb0, 0x20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose one:";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x158, 0x1b0);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "frmStartChoices";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmStartChoices";
            base.Closed += new EventHandler(this.frmStartChoices_Closed);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

