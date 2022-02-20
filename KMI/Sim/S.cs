using System;

namespace KMI.Sim {

    public class S {

        public static Simulator Instance => Simulator.Instance;

        public static frmMainBase MainForm => frmMainBase.Instance;

        public static Resource Resources => Simulator.Instance.Resource;

        public static SimStateAdapter Adapter => Simulator.Instance.SimStateAdapter;

        public static SimSettings Settings => Simulator.Instance.SimState.SimSettings;

        public static SimState State => Simulator.Instance.SimState;
    }
}

