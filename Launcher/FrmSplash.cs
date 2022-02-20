using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using KMI.VBPF1Lib;

namespace Launcher {

    public partial class FrmSplash : Form {

        /// <summary>
        /// Should the application exit?
        /// </summary>
        private bool Exit = false;

        /// <summary>
        /// Is this application a Virtual Business Challenge?
        /// </summary>
        protected bool VBC = false;

        /// <summary>
        /// Is this application a demo?
        /// </summary>
        protected bool Demo = false;

        /// <summary>
        /// Is this application an academic version?
        /// </summary>
        protected bool Academic = false;

        /// <summary>
        /// Determines the type of startup for the application.
        /// 
        /// N = "Normal" Mode,
        /// D = "Designer" Mode,
        /// </summary>
        private Keys Modifier = Keys.N;

        /// <summary>
        /// Counts down to application startup.
        /// </summary>
        private int LaunchCount = 40;

        public string[] Args;

        private readonly Dictionary<Keys, Func> StartUp_Type = new Dictionary<Keys, Func>();

        /// <summary>
        /// Creates a new splash screen.
        /// </summary>
        public FrmSplash() {
            InitializeComponent();
            labVersion.Text += Application.ProductVersion;
            this.StartUp_Type.Add(Keys.D, () => StartUp(true));
            this.StartUp_Type.Add(Keys.N, () => StartUp(false));
        }

        /// <summary>
        /// Modifies the start up mode.
        /// </summary>
        private void FrmSplash_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) { Exit = true; }
            Modifier = StartUp_Type.ContainsKey(e.KeyCode) ? e.KeyCode : Modifier;
        }

        /// <summary>
        /// Starts up the application.
        /// </summary>
        /// <param name="designer">Should the app be in designer mode?</param>
        public void StartUp(bool designer = false) {
            frmMain main = new frmMain(Args, Demo, VBC, Academic) { DesignerMode = designer };
            this.Hide();
            main.Show();
        }

        /// <summary>
        /// Counts down the time to launch the application.
        /// </summary>
        private void StartTimer_Tick(object sender, EventArgs e) {
            LaunchCount -= 1;
            if (LaunchCount <= 0) {
                StartTimer.Stop();
                if (Exit) { Application.Exit(); return; }
                StartUp_Type[StartUp_Type.ContainsKey(this.Modifier) ? Modifier : Keys.N]();
            }
        }
    }
}
