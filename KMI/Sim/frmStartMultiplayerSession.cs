namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Tcp;
    using System.Runtime.Serialization.Formatters;
    using System.Security;
    using System.Windows.Forms;

    public class frmStartMultiplayerSession : Form
    {
        public CheckBox chkRequireRoles;
        private Button cmdCancel;
        private Button cmdHelp;
        private Button cmdOK;
        private Container components = null;
        private Label label2;
        private static TcpChannel serverChannel;
        private TextBox txtSessionName;

        public frmStartMultiplayerSession()
        {
            this.InitializeComponent();
            this.chkRequireRoles.Visible = S.Instance.AllowRoleBasedMultiplayer;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp("MULTIPLAYER");
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            SecurityException exception;
            BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider {
                TypeFilterLevel = TypeFilterLevel.Full
            };
            IDictionary properties = new Hashtable();
            for (int i = 0; i < S.Instance.UserAdminSettings.MultiplayerPortCount; i++)
            {
                try
                {
                    BinaryClientFormatterSinkProvider clientSinkProvider = new BinaryClientFormatterSinkProvider();
                    properties["port"] = S.Instance.UserAdminSettings.MultiplayerBasePort + i;
                    if (serverChannel == null)
                    {
                        serverChannel = new TcpChannel(properties, clientSinkProvider, serverSinkProvider);
                        if (ChannelServices.GetChannel(serverChannel.ChannelName) == null)
                        {
                            ChannelServices.RegisterChannel(serverChannel);
                        }
                    }
                    break;
                }
                catch (SecurityException exception2)
                {
                    exception = exception2;
                    MessageBox.Show("Your application was not granted permission to open a channel for a host session on this computer. " + exception.Message);
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                    return;
                }
                catch
                {
                }
            }
            if (serverChannel == null)
            {
                MessageBox.Show("Could not make a host session available from this computer.  This is most likely due to network security settings.  Please have your network administrator contact us to resolve this.");
                base.DialogResult = DialogResult.Cancel;
                base.Close();
            }
            else
            {
                try
                {
                    RemotingServices.Marshal(S.Adapter, this.txtSessionName.Text, S.Adapter.GetType());
                    Simulator.Instance.SessionName = this.txtSessionName.Text;
                }
                catch (SecurityException exception3)
                {
                    exception = exception3;
                    MessageBox.Show("Your application was not granted permission to host a session on this computer. " + exception.Message);
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
                }
                catch (Exception exception4)
                {
                    MessageBox.Show("Could not start a host session session. " + exception4.Message);
                    base.DialogResult = DialogResult.Cancel;
                    base.Close();
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

        private void InitializeComponent()
        {
            this.cmdOK = new Button();
            this.cmdCancel = new Button();
            this.cmdHelp = new Button();
            this.label2 = new Label();
            this.txtSessionName = new TextBox();
            this.chkRequireRoles = new CheckBox();
            base.SuspendLayout();
            this.cmdOK.DialogResult = DialogResult.OK;
            this.cmdOK.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdOK.Location = new Point(40, 0xb8);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new Size(0x40, 0x18);
            this.cmdOK.TabIndex = 2;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
            this.cmdCancel.CausesValidation = false;
            this.cmdCancel.DialogResult = DialogResult.Cancel;
            this.cmdCancel.Location = new Point(120, 0xb8);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(0x38, 0x18);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
            this.cmdHelp.CausesValidation = false;
            this.cmdHelp.Location = new Point(0xc0, 0xb8);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new Size(0x38, 0x18);
            this.cmdHelp.TabIndex = 4;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.Click += new EventHandler(this.cmdHelp_Click);
            this.label2.Location = new Point(0x38, 0x48);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x88, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "Enter Session Name:";
            this.txtSessionName.Location = new Point(0x38, 0x58);
            this.txtSessionName.Name = "txtSessionName";
            this.txtSessionName.Size = new Size(0xa8, 20);
            this.txtSessionName.TabIndex = 1;
            this.txtSessionName.Text = "";
            this.txtSessionName.Validating += new CancelEventHandler(this.txtSessionName_Validating);
            this.chkRequireRoles.Location = new Point(0x48, 0x80);
            this.chkRequireRoles.Name = "chkRequireRoles";
            this.chkRequireRoles.Size = new Size(0x98, 0x10);
            this.chkRequireRoles.TabIndex = 5;
            this.chkRequireRoles.Text = "Require Roles";
            base.AcceptButton = this.cmdOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.cmdCancel;
            base.ClientSize = new Size(0x124, 230);
            base.Controls.Add(this.chkRequireRoles);
            base.Controls.Add(this.txtSessionName);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.cmdHelp);
            base.Controls.Add(this.cmdCancel);
            base.Controls.Add(this.cmdOK);
            base.Name = "frmStartMultiplayerSession";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "New Multiplayer Session";
            base.ResumeLayout(false);
        }

        private void txtSessionName_Validating(object sender, CancelEventArgs e)
        {
            if (((TextBox) sender).Text == "")
            {
                MessageBox.Show("You must enter a session name. It may be any name you choose. Please retry.");
                base.ActiveControl = (Control) sender;
                e.Cancel = true;
            }
        }
    }
}

