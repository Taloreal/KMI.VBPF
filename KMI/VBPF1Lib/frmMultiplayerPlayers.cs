namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMultiplayerPlayers : Form
    {
        private Button btnOK;
        private Container components = null;
        private Label label1;
        public int NumPlayers;
        private NumericUpDown updPlayers;

        public frmMultiplayerPlayers()
        {
            this.InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
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

        private void frmMultiplayerPlayers_Closed(object sender, EventArgs e)
        {
            this.NumPlayers = (int) this.updPlayers.Value;
        }

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.label1 = new Label();
            this.updPlayers = new NumericUpDown();
            this.updPlayers.BeginInit();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x48, 0x74);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x88, 0x18);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Continue";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label1.Location = new Point(0x30, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xc0, 0x20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Approximately how many students will participate in this session?";
            this.label1.TextAlign = ContentAlignment.TopCenter;
            this.updPlayers.Location = new Point(0x70, 0x4c);
            int[] bits = new int[4];
            bits[0] = 10;
            this.updPlayers.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.updPlayers.Minimum = new decimal(bits);
            this.updPlayers.Name = "updPlayers";
            this.updPlayers.Size = new Size(60, 20);
            this.updPlayers.TabIndex = 2;
            this.updPlayers.TextAlign = HorizontalAlignment.Right;
            bits = new int[4];
            bits[0] = 5;
            this.updPlayers.Value = new decimal(bits);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(280, 160);
            base.Controls.Add(this.updPlayers);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmMultiplayerPlayers";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Students in Session";
            base.Closed += new EventHandler(this.frmMultiplayerPlayers_Closed);
            this.updPlayers.EndInit();
            base.ResumeLayout(false);
        }
    }
}

