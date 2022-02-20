using System;
using KMI.Sim;

namespace KMI.VBPF1Lib {

    public class A {

        public static Simulator Instance => Simulator.Instance;

        public static frmMain MainForm => (frmMain)frmMainBase.Instance;

        public static Resource Resources => Simulator.Instance.Resource;

        public static AppStateAdapter Adapter => 
            (AppStateAdapter)Simulator.Instance.SimStateAdapter;

        public static AppSimSettings Settings => 
            (AppSimSettings)Simulator.Instance.SimState.SimSettings;

        public static AppSimState State => 
            (AppSimState)Simulator.Instance.SimState;

    }
}

