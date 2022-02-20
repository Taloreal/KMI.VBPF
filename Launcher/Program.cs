using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

using KMI.VBPF1Lib;

namespace Launcher {

    public static class Program {

        [STAThread] 
        public static void Main(string[] args) {
            try {
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmSplash() { Args = args });
            }
            catch (Exception e) { 
                frmMain.HandleError(e);
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            frmMain.HandleError(e.Exception);
        }
    }
}
