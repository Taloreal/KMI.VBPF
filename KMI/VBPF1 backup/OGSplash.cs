using System;
using System.IO;
using System.Net;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Net.NetworkInformation;

using KMI.VBPF1Lib;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace KMI.VBPF1 {

    public class OGSplash : Form {

        private int countDown = 40;
        private IContainer components;
        //private string[] args;
        private readonly string[] args;

        private bool exit = false;
        protected bool demo = false;
        protected bool academic = false;

        private Label labCopyright;
        private Label labVersion;
        //private ComponentResourceManager manager;
        private char Modifier = 'M';
        //private Dictionary<char, Action<char>> StartUp_Type = new Dictionary<char, Action<char>>();
        private readonly Dictionary<char, Action<char>> StartUp_Type = new Dictionary<char, Action<char>>();
        private System.Windows.Forms.Timer timer1;
        protected bool vbc = false;

        public OGSplash(string[] args) {
            this.InitializeComponent();
            this.args = args;
            this.labVersion.Text += Application.ProductVersion;
            this.timer1.Start();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            frmMain.HandleError(e.Exception);
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        /*private void DrawTrialText(string message) {
            Graphics graphics = Graphics.FromImage(this.BackgroundImage);
            int alpha = 0x7a;
            Color color = Color.FromArgb(alpha, Color.White);
            Rectangle rect = new Rectangle(20, 150, 460, 100);
            Brush brush = new SolidBrush(color);
            graphics.FillRectangle(brush, rect);
            brush = new SolidBrush(Color.Red);
            StringFormat format = new StringFormat {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near
            };
            Font font = new Font("Microsoft Sans Serif", 20f);
            graphics.DrawString(message, font, brush, rect, format);
        }*/

        private void InitializeComponent() {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(OGSplash));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labVersion = new Label();
            this.labCopyright = new Label();
            base.SuspendLayout();
            this.timer1.Interval = 50;
            this.timer1.Tick += new EventHandler(this.timer1_Tick);
            this.labVersion.BackColor = Color.White;
            this.labVersion.Location = new Point(24, 48);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new Size(149, 16);
            this.labVersion.TabIndex = 1;
            this.labVersion.Text = "Version ";
            this.labCopyright.BackColor = Color.Transparent;
            this.labCopyright.Location = new Point(0x17, 0x40);
            this.labCopyright.Name = "labCopyright";
            this.labCopyright.Size = new Size(0xeb, 40);
            this.labCopyright.TabIndex = 2;
            this.labCopyright.Text = "Copyright 2008-2010 Knowledge Matters, Inc. All rights reserved worldwide.";
            this.labCopyright.TextAlign = ContentAlignment.MiddleLeft;
            this.AutoScaleBaseSize = new Size(5, 13);

            base.ClientSize = new Size(500, 320);
            base.Controls.Add(this.labCopyright);
            base.Controls.Add(this.labVersion);
            base.FormBorderStyle = FormBorderStyle.None;

            base.Name = "frmSplash";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "frmSplashAbout";
            base.Load += new EventHandler(this.frmSplash_Load);
            base.KeyDown += new KeyEventHandler(this.frmSplash_KeyDown);
            base.ResumeLayout(false);
        }

        [STAThread]
        private static void _Main(string[] args) {
            try {
                Application.ThreadException += new ThreadExceptionEventHandler(OGSplash.Application_ThreadException);
                Settings.SetUp(true);
                Application.Run(new OGSplash(args));
            }
            catch (Exception exception) {
                frmMain.HandleError(exception);
            }
        }

        #region START UP MODES

        private void frmSplash_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) { exit = true; }
            if (this.StartUp_Type.ContainsKey((char) ((ushort) e.KeyValue))) {
                this.Modifier = (char) e.KeyValue;
            }
        }

        private void timer1_Tick(object sender, EventArgs e) {
            this.countDown--;
            if (this.countDown <= 0) {
                this.timer1.Stop();
                if (this.exit) { Application.Exit(); }
                else {
                    if (!StartUp_Type.ContainsKey(this.Modifier)) {
                        Normal_StartUp('M');
                        return;
                    }
                    this.StartUp_Type[this.Modifier](this.Modifier);
                }
            }
        }

        private void frmSplash_Load(object sender, EventArgs e) {
            this.StartUp_Type.Add('M', new Action<char>(this.Normal_StartUp));
            this.StartUp_Type.Add('D', new Action<char>(this.Designer_StartUp));
        }

        public void Normal_StartUp(char Start_Param) {
            frmMain main = new frmMain(this.args, this.demo, this.vbc, this.academic);
            base.Hide();
            main.Show();
        }

        public void Designer_StartUp(char Start_Param) {
            frmMain main = new frmMain(this.args, this.demo, this.vbc, this.academic) {
                DesignerMode = true
            };
            base.Hide();
            main.Show();
        }

        #endregion
    }
}

