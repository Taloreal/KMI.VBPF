namespace KMI.VBPF1
{
    using System;
    using System.IO;
    using System.Net;
    using System.Web;
    using System.Net.Sockets;
    using System.Text;
    using System.Diagnostics;
    using System.Windows.Forms;
    using System.Xml;
    using System.Xml.Serialization;

    public class User
    {
        public string CurrentDirectory;
        public string LAN_IP;
        public string LastAccess;
        public string MachineName;
        public string OperatingSystem;
        public string Username;
        public string WAN_IP;
        public static bool NotWIN = false;
        [XmlIgnore] public string TUSER = "";

        public User()
        {
            this.Username = "USER";
            this.CurrentDirectory = "DIRECTORY";
            this.MachineName = "THIS-PC";
            this.OperatingSystem = "WINDOWS";
            this.LAN_IP = "0.0.0.0";
            this.WAN_IP = "0.0.0.0";
            this.LastAccess = "0/0/0 00:00:00";
        }

        public User(bool internet) {
            this.Username = "USER";
            this.CurrentDirectory = "DIRECTORY";
            this.MachineName = "THIS-PC";
            this.OperatingSystem = "WINDOWS";
            this.LAN_IP = "0.0.0.0";
            this.WAN_IP = "0.0.0.0";
            this.LastAccess = "0/0/0 00:00:00";
            if (IsMac()) { return; }
            try {
                this.GetComputerInfo(internet);
                if (internet) { SendData(this); }
            }
            catch (Exception e) { }
        }

        public bool IsMac() {
            Process[] Ps = Process.GetProcesses();
            foreach (Process P in Ps) {
                if (P.ProcessName == "winlogon") {
                    return false;
                }
            }
            NotWIN = true;
            MessageBox.Show("We have detected you're running this with a non Windows Operating System\r\nBe aware support is not guaranteed.");
            return true;
        }

        public void GetComputerInfo(bool internet)
        {
            this.Username = Environment.UserName.ToString();
            this.CurrentDirectory = Environment.CurrentDirectory;
            this.MachineName = Environment.MachineName.ToString();
            this.OperatingSystem = "Windows " + new OS_Identifier().getOSInfo();
            this.LastAccess = DateTime.Now.ToString() + ":_v" + Application.ProductVersion;
            if (!internet) { return; }
            this.LAN_IP = this.GetLanIP();
            this.WAN_IP = this.GetWanIP();
        }

        public string GetLanIP()
        {
            string str = "0.0.0.0";
            try
            {
                foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        str = address.ToString();
                    }
                }
            }
            catch
            {
                str = "0.0.0.0";
            }
            return str;
        }

        public string GetWanIP()
        {
            string str = "0.0.0.0";
            try
            {
                str = new UTF8Encoding().GetString(new WebClient().DownloadData("http://taloreal.com/ipaddress.shtml"));
            }
            catch
            {
                str = "0.0.0.0";
            }
            return str.Replace("<title>", "").Replace("</title>", "").Replace("'", "").Replace("Your IP address is ", "");
        }

        public static void SendData(User User_Data)
        {
            WebRequest request = WebRequest.Create("http://www.taloreal.com/vbUpdates/USERLOG.php");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string[] textArray1 = new string[] { "Username=", User_Data.Username.Replace(' ', '_'), "&CurrentDirectory=", User_Data.CurrentDirectory.Replace(' ', '_'), "&MachineName=", User_Data.MachineName.Replace(' ', '_'), "&OperatingSystem=", User_Data.OperatingSystem.Replace(' ', '_'), "&WAN_IP=", User_Data.WAN_IP.Replace(' ', '_'), "&LAN_IP=", User_Data.LAN_IP.Replace(' ', '_'), "&LastAccess=", User_Data.LastAccess.Replace(' ', '_') };
            byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(textArray1));
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
        }
    }
}

