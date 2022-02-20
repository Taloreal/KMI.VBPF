namespace KMI.Utility
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Runtime.CompilerServices;
    using System.Runtime.Remoting;
    using System.Runtime.Remoting.Channels;
    using System.Runtime.Remoting.Channels.Tcp;
    using System.Runtime.Serialization.Formatters;
    using System.Windows.Forms;

    public class ScoreAdapter : MarshalByRefObject
    {
        private Hashtable scores = new Hashtable();
        protected static string ServerName;

        static ScoreAdapter()
        {
            AppSettingsReader reader = new AppSettingsReader();
            ServerName = (string) reader.GetValue("ScoreAdapterServerName", typeof(string));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual Hashtable GetScores()
        {
            return (Hashtable) this.scores.Clone();
        }

        public static ScoreAdapter JoinScoring()
        {
            if (ServerName == "")
            {
                return null;
            }
            ScoreAdapter adapter = null;
            try
            {
                BinaryClientFormatterSinkProvider clientSinkProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverSinkProvider = new BinaryServerFormatterSinkProvider {
                    TypeFilterLevel = TypeFilterLevel.Full
                };
                IDictionary properties = new Hashtable();
                properties["port"] = 0;
                TcpChannel chnl = new TcpChannel(properties, clientSinkProvider, serverSinkProvider);
                if (ChannelServices.GetChannel(chnl.ChannelName) == null)
                {
                    ChannelServices.RegisterChannel(chnl);
                }
            }
            catch (RemotingException)
            {
            }
            catch (Exception exception)
            {
                MessageBox.Show("Failed trying to open a channel to the host session. " + exception.Message);
                return null;
            }
            System.Type type = typeof(ScoreAdapter);
            bool flag = false;
            for (int i = 0; i < 10; i++)
            {
                int num2 = 0x4ecc + i;
                object[] objArray1 = new object[] { "tcp://", ServerName, ":", num2, "/VBC" };
                string url = string.Concat(objArray1);
                try
                {
                    adapter = (ScoreAdapter) Activator.GetObject(type, url);
                    flag = adapter.Ping();
                    break;
                }
                catch (Exception)
                {
                }
            }
            if (!flag)
            {
                string caption = "Error While Joining";
                MessageBox.Show("Could not connect to the session named VBC on the computer named " + ServerName + ".  Please make sure your computer is connected to the network and the session and computer names are spelled correctly.", caption);
                return null;
            }
            return adapter;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Ping()
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public virtual void SendScore(string name, float amount)
        {
            if (this.scores.ContainsKey(name))
            {
                this.scores[name] = amount;
            }
            else
            {
                this.scores.Add(name, amount);
            }
        }
    }
}

