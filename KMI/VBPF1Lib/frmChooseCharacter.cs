namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmChooseCharacter : Form
    {
        private Button btnOK;
        private Container components = null;
        private int index = -1;
        private Label label1;
        private TextBox labName;
        private bool okToClose = false;
        private Panel panPalettes;

        public frmChooseCharacter(string name)
        {
            this.InitializeComponent();
            this.labName.Text = name;
            if (name != "")
            {
                this.labName.Enabled = false;
            }
            for (int i = 0; i < 12; i++)
            {
                Label label = new Label {
                    Location = new Point(((i % 6) * 0x4d) + 20, (i / 6) * 110),
                    Image = A.Resources.GetImage("Palette" + (i + 6)),
                    Size = A.Resources.GetImage("Palette" + (i + 6)).Size
                };
                label.Click += new EventHandler(this.Palette_Click);
                label.Tag = i + 6;
                this.panPalettes.Controls.Add(label);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.labName.Text == "")
            {
                MessageBox.Show(A.Resources.GetString("You must enter a name for yourself. Please try again."), A.Resources.GetString("Input Required"));
            }
            else if (this.index == -1)
            {
                MessageBox.Show(A.Resources.GetString("You must select an image for yourself. Please click a person."), A.Resources.GetString("Input Required"));
            }
            else
            {
                try
                {
                    Person.GenderType male = Person.GenderType.Male;
                    if (this.index < 12)
                    {
                        male = Person.GenderType.Female;
                    }
                    long entityID = A.Adapter.AddEntity(A.Instance.ThisPlayerName, this.labName.Text, male, this.index);
                    A.MainForm.OnCurrentEntityChange(entityID);
                    this.okToClose = true;
                    base.Close();
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception, this);
                }
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

        private void frmChooseCharacter_Closing(object sender, CancelEventArgs e)
        {
            if (!this.okToClose)
            {
                e.Cancel = true;
            }
        }

        private void frmChooseCharacter_Load(object sender, EventArgs e)
        {
            this.labName.Focus();
        }

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.label1 = new Label();
            this.labName = new TextBox();
            this.panPalettes = new Panel();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0xcc, 0x138);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x70, 0x1c);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label1.Location = new Point(140, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x4c, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Full Name:";
            this.labName.Location = new Point(0xd0, 0x18);
            this.labName.Name = "labName";
            this.labName.Size = new Size(0xa4, 20);
            this.labName.TabIndex = 2;
            this.labName.Text = "";
            this.panPalettes.Location = new Point(4, 0x40);
            this.panPalettes.Name = "panPalettes";
            this.panPalettes.Size = new Size(0x200, 0xe0);
            this.panPalettes.TabIndex = 3;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x20c, 0x166);
            base.Controls.Add(this.panPalettes);
            base.Controls.Add(this.labName);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmChooseCharacter";
            base.ShowInTaskbar = false;
            this.Text = "Choose Yourself";
            base.Closing += new CancelEventHandler(this.frmChooseCharacter_Closing);
            base.Load += new EventHandler(this.frmChooseCharacter_Load);
            base.ResumeLayout(false);
        }

        private void Palette_Click(object sender, EventArgs e)
        {
            foreach (Label label2 in this.panPalettes.Controls)
            {
                label2.BorderStyle = BorderStyle.None;
            }
            Label label = (Label) sender;
            label.BorderStyle = BorderStyle.FixedSingle;
            this.index = (int) label.Tag;
        }
    }
}

