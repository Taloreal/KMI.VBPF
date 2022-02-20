namespace KMI.Sim
{
    using System;

    [Serializable]
    public class UserAdminSettings
    {
        protected int clientDrawStepPeriod = 50;
        protected string defaultDirectory = null;
        protected string language;
        protected int multiplayerBasePort = 0x4ecc;
        protected int multiplayerPortCount = 10;
        protected bool noSound = false;
        protected int p = 11;
        protected bool passwordsForMultiplayer = false;
        protected string proxyAddress;
        protected string proxyBypassList;

        public string GetP()
        {
            switch (this.p)
            {
                case 7:
                    return "JackRabbit";

                case 9:
                    return "SuperStore";

                case 11:
                    return "LearnFast";

                case 0x17:
                    return "CustomerIsKing";

                case 0x27:
                    return "LuckyLearners";

                case 0x2f:
                    return "CoolSchool";
            }
            return "XGHYMZ";
        }

        public int ClientDrawStepPeriod
        {
            get
            {
                return this.clientDrawStepPeriod;
            }
            set
            {
                this.clientDrawStepPeriod = value;
            }
        }

        public string DefaultDirectory
        {
            get
            {
                return this.defaultDirectory;
            }
            set
            {
                this.defaultDirectory = value;
            }
        }

        public int MultiplayerBasePort
        {
            get
            {
                return this.multiplayerBasePort;
            }
            set
            {
                this.multiplayerBasePort = value;
            }
        }

        public int MultiplayerPortCount
        {
            get
            {
                return this.multiplayerPortCount;
            }
            set
            {
                this.multiplayerPortCount = value;
            }
        }

        public bool NoSound
        {
            get
            {
                return this.noSound;
            }
            set
            {
                this.noSound = value;
            }
        }

        public int P
        {
            get
            {
                return this.p;
            }
            set
            {
                this.p = value;
            }
        }

        public bool PasswordsForMultiplayer
        {
            get
            {
                return this.passwordsForMultiplayer;
            }
            set
            {
                this.passwordsForMultiplayer = value;
            }
        }

        public string ProxyAddress
        {
            get
            {
                return this.proxyAddress;
            }
            set
            {
                this.proxyAddress = value;
            }
        }

        public string ProxyBypassList
        {
            get
            {
                return this.proxyBypassList;
            }
            set
            {
                this.proxyBypassList = value;
            }
        }
    }
}

