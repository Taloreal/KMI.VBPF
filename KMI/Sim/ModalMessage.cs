namespace KMI.Sim
{
    using System;
    using System.Windows.Forms;

    [Serializable]
    public class ModalMessage
    {
        protected MessageBoxIcon icon;
        protected string message;
        protected string title;
        protected string to;

        public ModalMessage(string to, string message, string title, MessageBoxIcon icon)
        {
            this.to = to;
            this.message = message;
            this.title = title;
            this.icon = icon;
        }

        public MessageBoxIcon Icon
        {
            get
            {
                return this.icon;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
        }

        public string To
        {
            get
            {
                return this.to;
            }
        }
    }
}

