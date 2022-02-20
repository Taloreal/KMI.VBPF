namespace KMI.Sim
{
    using System;
    using System.Collections;

    [Serializable]
    public class ComplaintBuffer
    {
        protected Hashtable complaints = new Hashtable();
        protected Hashtable messageTables = new Hashtable();
        protected Entity parent;

        public ComplaintBuffer(Entity parent)
        {
            this.parent = parent;
        }

        public void AddComplaint(string from, string complaintKey)
        {
            int num = 0;
            string key = from + complaintKey;
            if (this.complaints.ContainsKey(key))
            {
                num = ((int) this.complaints[key]) + 1;
                this.complaints[key] = num;
            }
            else
            {
                num = 1;
                this.complaints.Add(key, num);
            }
            MessageTable table = (MessageTable) this.messageTables[key];
            if (table != null)
            {
                for (int i = 0; i < table.levels.Length; i++)
                {
                    if (num == table.levels[i])
                    {
                        this.parent.Player.SendMessage(string.Format(table.messages[i], num), from, table.colors[i]);
                        break;
                    }
                }
            }
        }

        public void AddMessageTable(string from, string complaintKey, int[] levels, string[] messages)
        {
            NotificationColor[] notificationColors = new NotificationColor[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                notificationColors[i] = NotificationColor.Yellow;
            }
            this.AddMessageTable(from, complaintKey, levels, messages, notificationColors);
        }

        public void AddMessageTable(string from, string complaintKey, int[] levels, string message)
        {
            string[] messages = new string[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                messages[i] = message;
            }
            this.AddMessageTable(from, complaintKey, levels, messages);
        }

        public void AddMessageTable(string from, string complaintKey, int[] levels, string[] messages, NotificationColor[] notificationColors)
        {
            string key = from + complaintKey;
            if (levels.Length != messages.Length)
            {
                throw new Exception("Length of Level, Messages, and NotificationColors do not match in ComplaintBuffer.SetMessages.");
            }
            this.ClearMessageTable(from, complaintKey);
            this.messageTables.Add(key, new MessageTable(levels, messages, notificationColors));
        }

        public void AddMessageTable(string from, string complaintKey, int[] levels, string message, NotificationColor[] notificationColors)
        {
            string[] messages = new string[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                messages[i] = message;
            }
            this.AddMessageTable(from, complaintKey, levels, messages, notificationColors);
        }

        public void Clear(string from, string complaintKey)
        {
            string key = from + complaintKey;
            this.complaints.Remove(key);
        }

        public void ClearMessageTable(string from, string complaintKey)
        {
            string key = from + complaintKey;
            this.messageTables.Remove(key);
        }

        public int Count(string from, string complaintKey)
        {
            string key = from + complaintKey;
            if (this.complaints.ContainsKey(key))
            {
                return (int) this.complaints[key];
            }
            return 0;
        }

        [Serializable]
        private class MessageTable
        {
            public NotificationColor[] colors;
            public int[] levels;
            public string[] messages;

            public MessageTable(int[] levels, string[] messages, NotificationColor[] colors)
            {
                this.levels = levels;
                this.messages = messages;
                this.colors = colors;
            }
        }
    }
}

