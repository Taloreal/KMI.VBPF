namespace KMI.Sim
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    [Serializable]
    public class Player
    {
        protected string playerName;
        protected KMI.Sim.PlayerType playerType;
        protected ArrayList surveys = new ArrayList();

        public Player(string playerName, KMI.Sim.PlayerType playerType)
        {
            this.playerName = playerName;
            this.playerType = playerType;
        }

        public void SendMessage(string message, string from, NotificationColor notificationColor)
        {
            if (!S.Instance.BlockMessage(message) && ((this.playerType != KMI.Sim.PlayerType.AI) || S.MainForm.DesignerMode))
            {
                S.Adapter.FirePlayerMessageEvent(new PlayerMessage(this.PlayerName, message, from, S.State.Now, notificationColor));
            }
        }

        public void SendModalMessage(ModalMessage modalMessage)
        {
            if (this.playerType != KMI.Sim.PlayerType.AI)
            {
                S.Adapter.FireModalMessageEvent(modalMessage);
            }
        }

        public void SendModalMessage(string message, string title, MessageBoxIcon icon)
        {
            if (this.playerType != KMI.Sim.PlayerType.AI)
            {
                S.Adapter.FireModalMessageEvent(new ModalMessage(this.PlayerName, message, title, icon));
            }
        }

        public void SendPeriodicMessage(string message, string from, NotificationColor notificationColor, float daysBetweenMessages)
        {
            string key = message + this.PlayerName + from;
            if (S.Instance.PeriodicMessageTable.ContainsKey(key))
            {
                DateTime time = (DateTime) S.Instance.PeriodicMessageTable[key];
                TimeSpan span = (TimeSpan) (S.State.Now - time);
                if ((span.TotalSeconds / 86400.0) < daysBetweenMessages)
                {
                    return;
                }
                S.Instance.PeriodicMessageTable[key] = S.State.Now;
            }
            else
            {
                S.Instance.PeriodicMessageTable.Add(key, S.State.Now);
            }
            this.SendMessage(message, from, notificationColor);
        }

        public string PlayerName
        {
            get
            {
                return this.playerName;
            }
        }

        public KMI.Sim.PlayerType PlayerType
        {
            get
            {
                return this.playerType;
            }
        }

        public ArrayList Surveys
        {
            get
            {
                return this.surveys;
            }
        }
    }
}

