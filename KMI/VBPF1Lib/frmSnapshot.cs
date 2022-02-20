using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmSnapshot : Form {

        private static Bitmap buffer;
        private IContainer components;
        public MenuItem EnablingReference;
        private static Graphics g;
        private Input input;
        private Panel panMain;
        private ArrayList statusToolTips = new ArrayList();
        private ToolTip toolTip;
        private Timer Updater;

        public frmSnapshot() {
            this.InitializeComponent();
            A.MainForm.NewDay += new EventHandler(this.NewDayHandler);
            A.MainForm.EntityChanged += new EventHandler(this.EntityChangedHandler);
            this.toolTip = new ToolTip();
            this.toolTip.InitialDelay = 0;
            this.GetData();
        }

        protected void DrawStatusIcon(Graphics g, string image, float val, string toolTip, ref ArrayList toolTips)
        {
            this.statusToolTips.Add(toolTip);
            int x = 230 - (this.statusToolTips.Count * 0x2e);
            int y = 0;
            g.DrawImageUnscaled(A.Resources.GetImage(image), x, y);
            Color color = Color.FromArgb(0x67, 0xb6, 0x67);
            if (val < 0.1)
            {
                color = Color.FromArgb(0xcf, 0x6f, 0x6f);
            }
            else if (val < 0.66)
            {
                color = Color.FromArgb(0xc3, 0xb1, 0x6b);
            }
            Brush brush = new SolidBrush(color);
            int height = (int) Utilities.Clamp(19f * val, 1f, 19f);
            g.FillRectangle(brush, x + 11, ((y + 7) + 0x12) - height, 7, height);
        }

        protected virtual void EntityChangedHandler(object sender, EventArgs e)
        {
            if ((this.EnablingReference != null) && !this.EnablingReference.Enabled)
            {
                base.Close();
            }
            else if (this.GetData())
            {
                this.panMain.Refresh();
            }
        }

        protected void frmReport_Closed(object sender, EventArgs e)
        {
            A.MainForm.NewDay -= new EventHandler(this.NewDayHandler);
            A.MainForm.EntityChanged -= new EventHandler(this.EntityChangedHandler);
        }

        private void frmSnapshot_Load(object sender, EventArgs e)
        {
            base.Location = new Point((A.MainForm.Bounds.Right - base.Width) - 6, (A.MainForm.Bounds.Bottom - base.Height) - 20);
        }

        private void frmSnapshot_Resize(object sender, EventArgs e)
        {
            base.Location = new Point((A.MainForm.Bounds.Right - base.Width) - 6, (A.MainForm.Bounds.Bottom - base.Height) - 20);
        }

        protected bool GetData()
        {
            try
            {
                this.input = A.Adapter.GetSnapshot(A.MainForm.CurrentEntityID);
                return true;
            }
            catch (EntityNotFoundException)
            {
                base.Close();
                return false;
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.panMain = new Panel();
            this.Updater = new Timer(this.components);
            base.SuspendLayout();
            this.panMain.Location = new Point(0, 2);
            this.panMain.Name = "panMain";
            this.panMain.Size = new Size(0xe8, 0x21);
            this.panMain.TabIndex = 0;
            this.panMain.Paint += new PaintEventHandler(this.panMain_Paint);
            this.panMain.MouseMove += new MouseEventHandler(this.panMain_MouseMove);
            this.Updater.Enabled = true;
            this.Updater.Interval = 0xbb8;
            this.Updater.Tick += new EventHandler(this.Updater_Tick);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0xe8, 0x24);
            base.Controls.Add(this.panMain);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.StartPosition = FormStartPosition.Manual;
            base.Name = "frmSnapshot";
            base.ShowInTaskbar = false;
            this.Text = "Snapshot";
            base.Closed += new EventHandler(this.frmReport_Closed);
            base.Load += new EventHandler(this.frmSnapshot_Load);
            base.Resize += new EventHandler(this.frmSnapshot_Resize);
            base.ResumeLayout(false);
        }

        protected void NewDayHandler(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.panMain.Refresh();
            }
        }

        private void panMain_MouseMove(object sender, MouseEventArgs e)
        {
            int num = (this.panMain.Width - e.X) / 0x2e;
            if ((num >= 0) && (num < this.statusToolTips.Count))
            {
                this.toolTip.SetToolTip(this.panMain, (string) this.statusToolTips[num]);
            }
            else
            {
                this.toolTip.SetToolTip(this.panMain, "");
            }
        }

        private void panMain_Paint(object sender, PaintEventArgs e)
        {
            this.statusToolTips = new ArrayList();
            if (buffer == null)
            {
                buffer = new Bitmap(230, this.panMain.Height, e.Graphics);
                g = Graphics.FromImage(buffer);
            }
            g.Clear(this.BackColor);
            if (this.input.gas > -1f)
            {
                string str = "OK";
                if (this.input.carBroken)
                {
                    str = "Broken";
                }
                object[] objArray1 = new object[] { str };
                this.DrawStatusIcon(g, this.input.carImageName + str, -1f, A.Resources.GetString("Car: {0}", objArray1), ref this.statusToolTips);
                object[] objArray2 = new object[] { this.input.gas.ToString("N1") };
                this.DrawStatusIcon(g, "StatusGas", this.input.gas / 60f, A.Resources.GetString("Gas: {0} gals", objArray2), ref this.statusToolTips);
            }
            if (this.input.busTokens > -1)
            {
                object[] objArray3 = new object[] { this.input.busTokens };
                this.DrawStatusIcon(g, "StatusBusTokens", ((float) this.input.busTokens) / 60f, A.Resources.GetString("Bus Tokens: {0}", objArray3), ref this.statusToolTips);
            }
            object[] args = new object[] { this.input.food };
            this.DrawStatusIcon(g, "StatusFood", ((float) this.input.food) / 60f, A.Resources.GetString("Food: {0} meals", args), ref this.statusToolTips);
            object[] objArray5 = new object[] { Utilities.FP(this.input.health) };
            this.DrawStatusIcon(g, "StatusHealth", this.input.health, A.Resources.GetString("Health: {0}", objArray5), ref this.statusToolTips);
            int num = 0x2e * (5 - this.statusToolTips.Count);
            base.Width = 0xee - num;
            this.panMain.Left = -num;
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        private void Updater_Tick(object sender, EventArgs e)
        {
            if (this.GetData())
            {
                this.panMain.Refresh();
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public int food;
            public float health;
            public int busTokens;
            public float gas;
            public string carImageName;
            public bool carBroken;
        }
    }
}

