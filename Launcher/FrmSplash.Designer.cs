using System;
using System.Drawing;
using System.Windows.Forms;

namespace Launcher {

    partial class FrmSplash {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labCopyright = new System.Windows.Forms.Label();
            this.labVersion = new System.Windows.Forms.Label();
            this.StartTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labCopyright
            // 
            this.labCopyright.BackColor = System.Drawing.Color.Transparent;
            this.labCopyright.Location = new System.Drawing.Point(23, 64);
            this.labCopyright.Name = "labCopyright";
            this.labCopyright.Size = new System.Drawing.Size(235, 40);
            this.labCopyright.TabIndex = 2;
            this.labCopyright.Text = "Copyright 2008-2010 Knowledge Matters, Inc. All rights reserved worldwide.";
            this.labCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labVersion
            // 
            this.labVersion.BackColor = System.Drawing.Color.White;
            this.labVersion.Location = new System.Drawing.Point(24, 48);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(149, 16);
            this.labVersion.TabIndex = 1;
            this.labVersion.Text = "Version ";
            // 
            // StartTimer
            // 
            this.StartTimer.Enabled = true;
            this.StartTimer.Interval = 50;
            this.StartTimer.Tick += new System.EventHandler(this.StartTimer_Tick);
            // 
            // FrmSplash
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = global::Launcher.Properties.Resources.BackgroundImage;
            this.ClientSize = new System.Drawing.Size(500, 320);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.labCopyright);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = global::Launcher.Properties.Resources.Icon;
            this.Name = "FrmSplash";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSplashAbout";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSplash_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labCopyright;
        private System.Windows.Forms.Label labVersion;
        private Timer StartTimer;
    }
}