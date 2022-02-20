namespace KMI.Sim
{
    using System;
    using System.Windows.Forms;

    [Serializable]
    public class PlayerMessage
    {
        protected DateTime date;
        protected string from;
        protected string message;
        protected KMI.Sim.NotificationColor notificationColor;
        protected string to;

        public PlayerMessage(string to, string message, string from, DateTime date, KMI.Sim.NotificationColor notificationColor)
        {
            this.to = to;
            this.message = message;
            this.from = from;
            this.date = date;
            this.notificationColor = notificationColor;
        }

        public static void AddTimedMessage(string message, DateTime sendTime, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            PlayerMessage playerMessage = new PlayerMessage(null, message, from, S.State.Now, notificationColor);
            S.Instance.Subscribe(new TimedMessage(playerMessage), sendTime);
        }

        public static void AddTimedMessage(string message, DateTime sendTime, string to, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            PlayerMessage playerMessage = new PlayerMessage(to, message, from, S.State.Now, notificationColor);
            S.Instance.Subscribe(new TimedMessage(playerMessage), sendTime);
        }

        public static void AddTimedMessage(string message, DateTime sendTime, TimeSpan repeatInterval, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            PlayerMessage playerMessage = new PlayerMessage(null, message, from, S.State.Now, notificationColor);
            S.Instance.Subscribe(new TimedMessage(playerMessage, repeatInterval), sendTime);
        }

        public static void AddTimedMessage(string message, DateTime sendTime, TimeSpan repeatInterval, string to, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            PlayerMessage playerMessage = new PlayerMessage(to, message, from, S.State.Now, notificationColor);
            S.Instance.Subscribe(new TimedMessage(playerMessage, repeatInterval), sendTime);
        }

        public static void Broadcast(string message, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            PlayerMessage message2 = new PlayerMessage("All Players", message, from, S.State.Now, notificationColor);
            S.Adapter.FirePlayerMessageEvent(message2);
        }

        public static void BroadcastAllBut(string playerName, string message, string from, KMI.Sim.NotificationColor notificationColor)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            foreach (Player player in S.State.Player.Values)
            {
                if (!playerName.Equals(player.PlayerName))
                {
                    player.SendMessage(message, from, notificationColor);
                }
            }
        }

        public static void BroadcastModal(string message, string title, MessageBoxIcon icon)
        {
            if (S.Instance.Client)
            {
                throw new Exception("Can only send messages from server");
            }
            ModalMessage message2 = new ModalMessage(S.Resources.GetString("All Players"), message, title, icon);
            S.Adapter.FireModalMessageEvent(message2);
        }

        public DateTime Date
        {
            get
            {
                return this.date;
            }
        }

        public string From
        {
            get
            {
                return this.from;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public KMI.Sim.NotificationColor NotificationColor
        {
            get
            {
                return this.notificationColor;
            }
        }

        public string To
        {
            get
            {
                return this.to;
            }
        }

        protected class TimedMessage : ActiveObject
        {
            protected PlayerMessage playerMessage;
            protected TimeSpan repeatInterval;

            public TimedMessage(PlayerMessage playerMessage)
            {
                this.playerMessage = playerMessage;
                this.repeatInterval = new TimeSpan(0L);
            }

            public TimedMessage(PlayerMessage playerMessage, TimeSpan repeatInterval)
            {
                this.playerMessage = playerMessage;
                this.repeatInterval = repeatInterval;
            }

            public override bool NewStep()
            {
                if (this.playerMessage.To == null)
                {
                    PlayerMessage.Broadcast(this.playerMessage.Message, this.playerMessage.From, this.playerMessage.NotificationColor);
                }
                else
                {
                    Player player = (Player) S.State.Player[this.playerMessage.To];
                    if (player != null)
                    {
                        player.SendMessage(this.playerMessage.Message, this.playerMessage.From, this.playerMessage.NotificationColor);
                    }
                }
                if (this.repeatInterval > new TimeSpan(0L))
                {
                    this.WakeupTime = S.State.Now + this.repeatInterval;
                    return true;
                }
                S.Instance.UnSubscribe(this);
                return false;
            }
        }
    }
}

