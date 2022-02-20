using System;
using KMI.VBPF1Lib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KMI.VBPF1 {

    public partial class FrmSplash : Form {

        private bool Exit = false;
        protected bool vbc = false;
        protected bool demo = false;
        protected bool academic = false;

        private char Modifier = 'M';

        public string[] Args;

        private readonly Dictionary<char, Action<char>> StartUp_Type = new Dictionary<char, Action<char>>();

        public FrmSplash() {
            InitializeComponent();
            Settings.SetValue<int>("Launch", 40);
        }

        private void FrmSplash_Load(object sender, EventArgs e) {
            this.StartUp_Type.Add('M', new Action<char>(Normal_StartUp));
            this.StartUp_Type.Add('D', new Action<char>(Designer_StartUp));
        }

        private void FrmSplash_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) { Exit = true; }
            if (this.StartUp_Type.ContainsKey((char)((ushort)e.KeyValue))) {
                this.Modifier = (char)e.KeyValue;
            }
        }

        public void Normal_StartUp(char start_Param) {
            frmMain main = new frmMain(this.Args, this.demo, this.vbc, this.academic);
            this.Hide();
            main.Show();
        }

        public void Designer_StartUp(char start_Param) {
            frmMain main = new frmMain(this.Args, this.demo, this.vbc, this.academic) {
                DesignerMode = true
            };
            this.Hide();
            main.Show();
        }

        private void StartTimer_Tick(object sender, EventArgs e) {
            Settings.GetValue<int>("Launch", out int countDown);
            Settings.SetValue<int>("Launch", countDown - 1);
            if (countDown <= 0) {
                StartTimer.Stop();
                if (Exit) { Application.Exit(); }
                else {
                    if (!StartUp_Type.ContainsKey(this.Modifier)) {
                        Normal_StartUp('M');
                        return;
                    }
                    this.StartUp_Type[this.Modifier](this.Modifier);
                }
            }
        }
    }
}
