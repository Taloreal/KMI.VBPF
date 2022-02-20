namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class MessageControl : UserControl
    {
        private Container components;
        private Label labDate;
        private Label labFrom;
        private Label labFromID;
        private Label labImage;
        private Label labMessageText;
        protected const int MIN_MESSAGE_TEXT_HEIGHT = 0x20;
        protected const int PADDING = 4;
        protected bool resizing;
        public static StringFormat SFTW;

        public MessageControl()
        {
            this.components = null;
            this.resizing = false;
            this.InitializeComponent();
        }

        public MessageControl(PlayerMessage message)
        {
            string str;
            this.components = null;
            this.resizing = false;
            this.InitializeComponent();
            this.Init();
            this.labMessageText.Text = message.Message;
            this.labFrom.Text = message.From;
            if (this.labFrom.Text.Equals(""))
            {
                this.labFromID.Visible = false;
            }
            else
            {
                this.labFromID.Visible = true;
            }
            if (message.Date.Hour >= 12)
            {
                str = message.Date.ToString("M/d/yy - h:mm") + " PM";
            }
            else
            {
                str = message.Date.ToString("M/d/yy - h:mm") + " AM";
            }
            this.labDate.Text = str;
            this.labImage.Image = new Bitmap(base.GetType(), "Images.Warning" + message.NotificationColor.ToString() + ".gif");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void Init()
        {
            if (SFTW == null)
            {
                SFTW = new StringFormat();
                SFTW.Trimming = StringTrimming.Word;
            }
            base.SuspendLayout();
            this.labMessageText.Height = 0x20;
            this.labImage.Height = 0x20;
            this.labImage.Left = 4;
            this.labMessageText.Left = this.labImage.Left + this.labImage.Width;
            this.labFrom.Left = this.labImage.Left + this.labImage.Width;
            this.labFromID.Left = 4;
            this.labFromID.Top = 4;
            this.labFrom.Top = 4;
            this.labDate.Top = 4;
            this.MessageControl_Resize(this, new EventArgs());
            base.ResumeLayout();
        }

        private void InitializeComponent()
        {
            this.labMessageText = new Label();
            this.labImage = new Label();
            this.labFromID = new Label();
            this.labFrom = new Label();
            this.labDate = new Label();
            base.SuspendLayout();
            this.labMessageText.Location = new Point(40, 0x10);
            this.labMessageText.Name = "labMessageText";
            this.labMessageText.Size = new Size(0xd8, 0x20);
            this.labMessageText.TabIndex = 0;
            this.labMessageText.Text = "#message text";
            this.labMessageText.TextChanged += new EventHandler(this.labMessageText_TextChanged);
            this.labImage.Location = new Point(0, 0x10);
            this.labImage.Name = "labImage";
            this.labImage.Size = new Size(40, 0x20);
            this.labImage.TabIndex = 1;
            this.labFromID.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labFromID.Location = new Point(0, 0);
            this.labFromID.Name = "labFromID";
            this.labFromID.Size = new Size(40, 0x10);
            this.labFromID.TabIndex = 2;
            this.labFromID.Text = "From:";
            this.labFromID.TextAlign = ContentAlignment.MiddleLeft;
            this.labFrom.Location = new Point(40, 0);
            this.labFrom.Name = "labFrom";
            this.labFrom.Size = new Size(0x68, 0x10);
            this.labFrom.TabIndex = 3;
            this.labFrom.Text = "#from";
            this.labFrom.TextAlign = ContentAlignment.MiddleLeft;
            this.labDate.Location = new Point(0x90, 0);
            this.labDate.Name = "labDate";
            this.labDate.Size = new Size(0x60, 0x10);
            this.labDate.TabIndex = 4;
            this.labDate.Text = "#date";
            this.labDate.TextAlign = ContentAlignment.MiddleRight;
            base.Controls.Add(this.labDate);
            base.Controls.Add(this.labFrom);
            base.Controls.Add(this.labFromID);
            base.Controls.Add(this.labImage);
            base.Controls.Add(this.labMessageText);
            base.Name = "MessageControl";
            base.Size = new Size(240, 0x30);
            base.Resize += new EventHandler(this.MessageControl_Resize);
            base.ResumeLayout(false);
        }

        private void labMessageText_TextChanged(object sender, EventArgs e)
        {
            this.labMessageText.Height = (int) this.labMessageText.CreateGraphics().MeasureString(this.labMessageText.Text, this.labMessageText.Font, this.labMessageText.Width, SFTW).Height;
            base.Height = this.labMessageText.Height;
        }

        private void MessageControl_Resize(object sender, EventArgs e)
        {
            base.SuspendLayout();
            this.labMessageText.Width = (base.Width - this.labImage.Width) - 8;
            this.labFrom.Width = this.labMessageText.Width / 2;
            this.labDate.Left = this.labFrom.Left + this.labFrom.Width;
            this.labDate.Width = this.labFrom.Width;
            int height = (int) this.labMessageText.CreateGraphics().MeasureString(this.labMessageText.Text, this.labMessageText.Font, this.labMessageText.Width, SFTW).Height;
            height = Math.Max(height, 0x20);
            if (this.labImage.Image != null)
            {
                height = Math.Max(this.labImage.Height, height);
            }
            this.labMessageText.Height = height;
            this.labImage.Height = height;
            int num2 = (int) this.labFrom.CreateGraphics().MeasureString(this.labFrom.Text, this.labFrom.Font, this.labFrom.Width, SFTW).Height;
            int num3 = (int) this.labDate.CreateGraphics().MeasureString(this.labDate.Text, this.labDate.Font, this.labDate.Width, SFTW).Height;
            height = Math.Max(num2, num3);
            this.labFromID.Height = height;
            this.labFrom.Height = height;
            this.labDate.Height = height;
            this.labImage.Top = height + 4;
            this.labMessageText.Top = height + 4;
            base.Height = (this.labFromID.Height + this.labMessageText.Height) + 8;
            base.ResumeLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            if (!this.resizing)
            {
                this.resizing = true;
                base.OnResize(e);
            }
            this.resizing = false;
        }
    }
}

