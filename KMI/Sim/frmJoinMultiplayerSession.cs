namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Net;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Tcp;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Security;
    using System.Windows.Forms;

    public class frmJoinMultiplayerSession : Form
    {
        private Button cmdCancel;
        private Button cmdHelp;
        private Button cmdOK;
        private Container components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labPassword;
        private Label labPasswordHelp;
        private Label labWait;
        public string MultiplayerRoleName = "";
        private Panel panBottom;
        protected KMI.Sim.Player player;
        protected SimStateAdapter remoteAdapter;
        private TextBox txtComputerName;
        private TextBox txtPassword;
        private TextBox txtSessionName;
        private TextBox txtTeamName;

        public frmJoinMultiplayerSession()
        {
            this.InitializeComponent();
            this.txtTeamName.MaxLength = 12;
            if (S.Instance.UserAdminSettings.PasswordsForMultiplayer)
            {
                this.labPassword.Visible = true;
                this.txtPassword.Visible = true;
                this.labPasswordHelp.Visible = true;
                base.Height += 0x38;
            }
        }

        private void CleanupForCancel()
        {
            base.DialogResult = DialogResult.Cancel;
            base.Close();
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
            this.labWait.Visible = true;
            this.labWait.Refresh();
            if (((this.txtComputerName.Text == "") || (this.txtSessionName.Text == "")) || (this.txtTeamName.Text == ""))
            {
                MessageBox.Show("Please fill in all fields.");
                if (this.txtComputerName.Text == "")
                {
                    this.txtComputerName.Focus();
                }
                else if (this.txtSessionName.Text == "")
                {
                    this.txtSessionName.Focus();
                }
                else
                {
                    this.txtTeamName.Focus();
                }
            }
            else
            {
                Exception exception;
                string str;
                string str2;
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    BinaryClientFormatterSinkProvider clientSinkProvider = new BinaryClientFormatterSinkProvider();
                    BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider {
                        TypeFilterLevel = TypeFilterLevel.Full
                    };
                    IDictionary properties = new Hashtable();
                    properties["port"] = 0;
                    TcpChannel chnl = new TcpChannel(properties, clientSinkProvider, serverSinkProvider);
                    if (ChannelServices.GetChannel(chnl.ChannelName) == null)
                    {
                        ChannelServices.RegisterChannel(chnl);
                    }
                }
                catch (RemotingException)
                {
                }
                catch (SecurityException exception3)
                {
                    MessageBox.Show("Your application was not granted permission to open a channel to a host session. " + exception3.Message);
                    this.CleanupForCancel();
                    return;
                }
                catch (Exception exception4)
                {
                    exception = exception4;
                    MessageBox.Show("Failed trying to open a channel to the host session. " + exception.Message);
                    this.CleanupForCancel();
                    return;
                }
                Exception exception2 = null;
                try
                {
                    IPHostEntry hostByName = Dns.GetHostByName(this.txtComputerName.Text);
                }
                catch (Exception exception5)
                {
                    exception = exception5;
                    exception2 = exception;
                }
                System.Type type = Simulator.Instance.SimFactory.CreateSimStateAdapter().GetType();
                this.remoteAdapter = null;
                bool flag4 = false;
                for (int i = 0; i < S.Instance.UserAdminSettings.MultiplayerPortCount; i++)
                {
                    int num2 = S.Instance.UserAdminSettings.MultiplayerBasePort + i;
                    object[] objArray1 = new object[] { "tcp://", this.txtComputerName.Text, ":", num2, "/", this.txtSessionName.Text };
                    string url = string.Concat(objArray1);
                    try
                    {
                        this.remoteAdapter = (SimStateAdapter) Activator.GetObject(type, url);
                        flag4 = this.remoteAdapter.Ping();
                        break;
                    }
                    catch (MemberAccessException exception6)
                    {
                        MessageBox.Show("This member was invoked with a late-binding mechanism. " + exception6.Message);
                        this.CleanupForCancel();
                        return;
                    }
                    catch (Exception)
                    {
                    }
                }
                if (!flag4)
                {
                    str = "Error While Joining";
                    string[] textArray1 = new string[] { "Could not connect to the session named ", this.txtSessionName.Text, " on the computer named ", this.txtComputerName.Text, ".  Please make sure your computer is connected to the network and the session and computer names are spelled correctly." };
                    str2 = string.Concat(textArray1);
                    MessageBox.Show(this, str2, str);
                    this.labWait.Visible = false;
                    this.Cursor = Cursors.Default;
                }
                else if (this.remoteAdapter.GetSimulatorID() == S.Instance.GUID)
                {
                    str = "Error While Joining";
                    str2 = "You have just tried to connect this host session to itself.  If you would like to join this host session from from a client session on this computer, you need to join from a new " + Application.ProductName + " simulation.";
                    MessageBox.Show(this, str2, str);
                    this.labWait.Visible = false;
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    try
                    {
                        KMI.Sim.Player player = this.remoteAdapter.CreateClientPlayer(this.txtTeamName.Text, this.txtPassword.Text);
                        this.player = player;
                        this.txtTeamName.Text = player.PlayerName;
                        if (this.remoteAdapter.RoleBasedMultiplayer())
                        {
                            base.Hide();
                            frmMultiplayerRole role = new frmMultiplayerRole();
                            role.ShowDialog(this);
                            this.MultiplayerRoleName = role.RoleName;
                        }
                        base.DialogResult = DialogResult.OK;
                        base.Close();
                    }
                    catch (BadTeamPasswordException)
                    {
                        object[] args = new object[] { 3 };
                        MessageBox.Show(S.Resources.GetString("Password incorrect. Please try again. Note: Passwords must be at least {0} characters long.", args), S.Resources.GetString("Password Incorrect"));
                        this.Cursor = Cursors.Default;
                    }
                    catch (OverMultiplayerLimitException)
                    {
                        MessageBox.Show("The host session is full.  No other players may join.");
                        this.CleanupForCancel();
                    }
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
            this.txtTeamName = new TextBox();
            this.txtComputerName = new TextBox();
            this.label3 = new Label();
            this.label1 = new Label();
            this.txtSessionName = new TextBox();
            this.labWait = new Label();
            this.panBottom = new Panel();
            this.txtPassword = new TextBox();
            this.labPassword = new Label();
            this.labPasswordHelp = new Label();
            this.panBottom.SuspendLayout();
            base.SuspendLayout();
            this.cmdOK.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.cmdOK.Location = new Point(40, 0x30);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new Size(0x40, 0x18);
            this.cmdOK.TabIndex = 6;
            this.cmdOK.Text = "OK";
            this.cmdOK.Click += new EventHandler(this.cmdOK_Click);
            this.cmdCancel.DialogResult = DialogResult.Cancel;
            this.cmdCancel.Location = new Point(120, 0x30);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new Size(0x38, 0x18);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.Click += new EventHandler(this.cmdCancel_Click);
            this.cmdHelp.Location = new Point(0xc0, 0x30);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new Size(0x38, 0x18);
            this.cmdHelp.TabIndex = 8;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.Click += new EventHandler(this.cmdHelp_Click);
            this.label2.Location = new Point(0x38, 120);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x88, 0x10);
            this.label2.TabIndex = 4;
            this.label2.Text = "Enter Team Name:";
            this.txtTeamName.Location = new Point(0x38, 0x88);
            this.txtTeamName.MaxLength = 12;
            this.txtTeamName.Name = "txtTeamName";
            this.txtTeamName.Size = new Size(0xa8, 20);
            this.txtTeamName.TabIndex = 5;
            this.txtTeamName.Text = "";
            this.txtTeamName.Validating += new CancelEventHandler(this.txtTeamName_Validating);
            this.txtComputerName.Location = new Point(0x38, 40);
            this.txtComputerName.Name = "txtComputerName";
            this.txtComputerName.Size = new Size(0xa8, 20);
            this.txtComputerName.TabIndex = 1;
            this.txtComputerName.Text = "";
            this.txtComputerName.Validating += new CancelEventHandler(this.txtComputerName_Validating);
            this.label3.Location = new Point(0x38, 0x18);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xb8, 0x10);
            this.label3.TabIndex = 0;
            this.label3.Text = "Enter Host Computer Name:";
            this.label1.Location = new Point(0x38, 0x48);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xa8, 0x10);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter Session Name";
            this.txtSessionName.Location = new Point(0x38, 0x58);
            this.txtSessionName.Name = "txtSessionName";
            this.txtSessionName.Size = new Size(0xa8, 20);
            this.txtSessionName.TabIndex = 3;
            this.txtSessionName.Text = "";
            this.txtSessionName.Validating += new CancelEventHandler(this.txtSessionName_Validating);
            this.labWait.Location = new Point(0x10, 0);
            this.labWait.Name = "labWait";
            this.labWait.Size = new Size(0x100, 40);
            this.labWait.TabIndex = 9;
            this.labWait.Text = "Attempting to find session...  Please wait. This could take a minute or more.";
            this.labWait.TextAlign = ContentAlignment.MiddleCenter;
            this.labWait.Visible = false;
            this.panBottom.Controls.Add(this.cmdOK);
            this.panBottom.Controls.Add(this.labWait);
            this.panBottom.Controls.Add(this.cmdHelp);
            this.panBottom.Controls.Add(this.cmdCancel);
            this.panBottom.Dock = DockStyle.Bottom;
            this.panBottom.Location = new Point(0, 0xb6);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new Size(0x124, 80);
            this.panBottom.TabIndex = 10;
            this.txtPassword.Location = new Point(0x38, 0xb8);
            this.txtPassword.MaxLength = 12;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(0x80, 20);
            this.txtPassword.TabIndex = 12;
            this.txtPassword.Text = "";
            this.txtPassword.Visible = false;
            this.labPassword.Location = new Point(0x38, 0xa8);
            this.labPassword.Name = "labPassword";
            this.labPassword.Size = new Size(0x88, 0x10);
            this.labPassword.TabIndex = 11;
            this.labPassword.Text = "Enter Team Password:";
            this.labPassword.Visible = false;
            this.labPasswordHelp.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Underline, GraphicsUnit.Point, 0);
            this.labPasswordHelp.ForeColor = Color.Blue;
            this.labPasswordHelp.Location = new Point(0xc0, 0xb8);
            this.labPasswordHelp.Name = "labPasswordHelp";
            this.labPasswordHelp.Size = new Size(40, 0x18);
            this.labPasswordHelp.TabIndex = 13;
            this.labPasswordHelp.Text = "Explain";
            this.labPasswordHelp.TextAlign = ContentAlignment.MiddleLeft;
            this.labPasswordHelp.Visible = false;
            this.labPasswordHelp.Click += new EventHandler(this.labPasswordHelp_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x124, 0x106);
            base.Controls.Add(this.labPasswordHelp);
            base.Controls.Add(this.txtPassword);
            base.Controls.Add(this.labPassword);
            base.Controls.Add(this.panBottom);
            base.Controls.Add(this.txtSessionName);
            base.Controls.Add(this.txtComputerName);
            base.Controls.Add(this.txtTeamName);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.label2);
            base.Name = "frmJoinMultiplayerSession";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Join Multiplayer Session";
            this.panBottom.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void labPasswordHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(S.Resources.GetString("If this is the first time you are joining a multiplayer session under this team name, type any password to set your team password. If you are rejoining the multiplayer session, type the password you previously entered for your team name."), S.Resources.GetString("Multiplayer Passwords"));
        }

        private void txtComputerName_Validating(object sender, CancelEventArgs e)
        {
        }

        private void txtSessionName_Validating(object sender, CancelEventArgs e)
        {
        }

        private void txtTeamName_Validating(object sender, CancelEventArgs e)
        {
        }

        public KMI.Sim.Player Player
        {
            get
            {
                return this.player;
            }
        }

        public SimStateAdapter RemoteAdapter
        {
            get
            {
                return this.remoteAdapter;
            }
        }

        public string SessionName
        {
            get
            {
                return this.txtSessionName.Text;
            }
        }

        public string TeamName
        {
            get
            {
                return this.txtTeamName.Text;
            }
        }

        [Serializable]
        public class BadTeamPasswordException : SimApplicationException
        {
            public BadTeamPasswordException()
            {
            }

            public BadTeamPasswordException(string messageStringResource) : base(messageStringResource)
            {
            }

            public BadTeamPasswordException(SerializationInfo info, StreamingContext context)
            {
            }
        }

        [Serializable]
        public class OverMultiplayerLimitException : SimApplicationException
        {
            public OverMultiplayerLimitException()
            {
            }

            public OverMultiplayerLimitException(string messageStringResource) : base(messageStringResource)
            {
            }

            public OverMultiplayerLimitException(SerializationInfo info, StreamingContext context)
            {
            }
        }
    }
}

