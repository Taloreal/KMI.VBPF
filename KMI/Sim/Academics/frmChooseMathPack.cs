namespace KMI.Sim.Academics
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class frmChooseMathPack : Form
    {
        private Button btnOK;
        private Container components = null;
        private Label label1;
        private Label label2;
        private ListBox lstPaks;
        private string[] pageBankNames;
        private Panel panel1;

        public frmChooseMathPack()
        {
            this.InitializeComponent();
            this.pageBankNames = Directory.GetDirectories(Application.StartupPath + Path.DirectorySeparatorChar.ToString() + "MathPaks");
            foreach (string str in this.pageBankNames)
            {
                int num2 = str.LastIndexOf(Path.DirectorySeparatorChar);
                string item = str.Substring(num2 + 1, (str.Length - num2) - 1);
                this.lstPaks.Items.Add(item);
            }
            if (this.lstPaks.Items.Count == 0)
            {
                MessageBox.Show("No math paks found. Cannot continue.", "Missing Math Paks");
                Application.Exit();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.lstPaks.SelectedIndex == -1)
            {
                MessageBox.Show("Please choose a Math Pak", "Input Required");
            }
            else
            {
                AcademicGod.PageBankPath = this.pageBankNames[this.lstPaks.SelectedIndex];
                base.Close();
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

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.btnOK = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.lstPaks = new ListBox();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lstPaks);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x124, 0x14c);
            this.panel1.TabIndex = 0;
            this.btnOK.Location = new Point(0x60, 0x120);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x68, 0x18);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label1.Font = new Font("Microsoft Sans Serif", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(40, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xd0, 0x20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select a Topic:";
            this.label2.Location = new Point(0x20, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x80, 0x10);
            this.label2.TabIndex = 2;
            this.label2.Text = "Installed Math Paks:";
            this.lstPaks.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.lstPaks.ItemHeight = 0x10;
            this.lstPaks.Location = new Point(0x20, 0x60);
            this.lstPaks.Name = "lstPaks";
            this.lstPaks.Size = new Size(0xd8, 0xa4);
            this.lstPaks.TabIndex = 3;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x14c);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "frmChooseMathPack";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmChooseMathPack";
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

