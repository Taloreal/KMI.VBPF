namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMultiplayerRole : Form
    {
        private Button btnOK;
        private Container components = null;
        private Label label1;
        private ListBox lstRoles;
        public string RoleName;

        public frmMultiplayerRole()
        {
            this.InitializeComponent();
            foreach (MultiplayerRole role in MultiplayerRole.Roles)
            {
                this.lstRoles.Items.Add(role.RoleName);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.lstRoles.SelectedIndex == -1)
            {
                MessageBox.Show(S.Resources.GetString("You must select a role to play in the sim."), S.Resources.GetString("Please Retry"));
            }
            else
            {
                this.RoleName = this.lstRoles.SelectedItem.ToString();
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
            this.label1 = new Label();
            this.lstRoles = new ListBox();
            this.btnOK = new Button();
            base.SuspendLayout();
            this.label1.Location = new Point(0x20, 0x18);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xe0, 0x38);
            this.label1.TabIndex = 0;
            this.label1.Text = "This multiplayer session requires you to play a specific role. Please choose a role as directed by your instructor.";
            this.lstRoles.Location = new Point(0x38, 0x70);
            this.lstRoles.Name = "lstRoles";
            this.lstRoles.Size = new Size(0xb0, 0x45);
            this.lstRoles.TabIndex = 1;
            this.btnOK.Location = new Point(0x60, 0xd8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x108);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.lstRoles);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmMultiplayerRole";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Choose Multiplayer Role";
            base.ResumeLayout(false);
        }
    }
}

