namespace KMI.Sim
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmMessages : Form
    {
        protected bool alternateBackground = false;
        protected const int AVAILABLE_WIDTH = 30;
        private IContainer components;
        public List<frmMainBase> Controller = new List<frmMainBase>();
        public static int MAX_MESSAGES = 20;

        public frmMessages()
        {
            this.InitializeComponent();
        }

        public void AddMessage(PlayerMessage message)
        {
            if ((message.NotificationColor == NotificationColor.Red) || (message.NotificationColor == NotificationColor.Yellow))
            {
                base.Visible = true;
            }
            if (message.NotificationColor == NotificationColor.Red)
            {
                ((frmMainBase) base.Owner).mnuOptionsGoStop_Click(null, null);
            }
            if (((message.NotificationColor == NotificationColor.Yellow) && !(message.To == "All Players")) && (S.Instance.SimState.SpeedIndex > 1))
            {
                ((frmMainBase) base.Owner).mnuOptionsSlower_Click(null, null);
            }
            if (base.Controls.Count == MAX_MESSAGES)
            {
                base.Controls.RemoveAt(0);
            }
            MessageControl control = new MessageControl(message);
            if (this.alternateBackground)
            {
                control.BackColor = Color.Gainsboro;
            }
            this.alternateBackground = !this.alternateBackground;
            base.SuspendLayout();
            base.Controls.Add(control);
            base.ResumeLayout();
            this.frmMessages_Resize(this, new EventArgs());
            base.ScrollControlIntoView(base.Controls[base.Controls.Count - 1]);
        }

        public void Clear()
        {
            base.SuspendLayout();
            base.Controls.Clear();
            base.ResumeLayout();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmMessages_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            S.MainForm.HideMessageWindow();
        }

        private void frmMessages_Resize(object sender, EventArgs e)
        {
            base.SuspendLayout();
            for (int i = 0; i < base.Controls.Count; i++)
            {
                MessageControl control = (MessageControl) base.Controls[i];
                control.Width = base.Width - 30;
                if (i == 0)
                {
                    control.Location = new Point(base.AutoScrollPosition.X, base.AutoScrollPosition.Y);
                }
                else
                {
                    control.Location = new Point(base.AutoScrollPosition.X, base.Controls[i - 1].Location.Y + base.Controls[i - 1].Height);
                }
            }
            base.ResumeLayout();
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            this.AutoScroll = true;
            base.ClientSize = new Size(0x120, 0xa6);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Location = new Point(0, 0x1388);
            this.MinimumSize = new Size(200, 160);
            base.Name = "frmMessages";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "Message Center";
            base.Closing += new CancelEventHandler(this.frmMessages_Closing);
            base.Resize += new EventHandler(this.frmMessages_Resize);
            base.ResumeLayout(false);
        }
    }
}

