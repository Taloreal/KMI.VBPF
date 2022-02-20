namespace KMI.Utility
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    public sealed class KMIHelp
    {
        public static string LocalPath = (Application.StartupPath + @"\Help\index.htm");
        public static string RemotePath = "http://help.knowledgematters.com/vbx1/help/index.htm";
        private static ResourceManager rm = new ResourceManager("KMI.Utility.Utility", System.Reflection.Assembly.GetAssembly(typeof(KMIHelp)));


        [DllImport("shell32.dll")]
        private static extern int FindExecutable(string file, string dir, StringBuilder buffer);
        private static string GetExecutableFile(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            StringBuilder buffer = new StringBuilder(260);
            if (FindExecutable(path, "", buffer) < 0x20)
            {
                throw new BrowserNotFoundException();
            }
            return buffer.ToString();
        }

        public static string LocalFileURLEncode(string url)
        {
            url = url.Replace("%", "%25");
            url = url.Replace("^", "%5E");
            url = url.Replace("{", "%7B");
            url = url.Replace("}", "%7D");
            url = url.Replace("[", "%5B");
            url = url.Replace("]", "%5D");
            url = url.Replace("`", "%60");
            url = url.Replace(";", "%3b");
            url = url.Replace("#", "%23");
            url = url.Replace(" ", "%20");
            return url;
        }

        public static string MakeSafeForDHTML(string topic)
        {
            if (!topic.Equals(""))
            {
                topic = topic.Replace(" ", "_");
                topic = topic.Replace("!", "");
                topic = topic.Replace("@", "");
                topic = topic.Replace("#", "");
                topic = topic.Replace("$", "");
                topic = topic.Replace("%", "");
                topic = topic.Replace("^", "");
                topic = topic.Replace("&", "");
                topic = topic.Replace("*", "");
                topic = topic.Replace("(", "");
                topic = topic.Replace(")", "");
                topic = topic.Replace("=", "");
                topic = topic.Replace("+", "");
                topic = topic.Replace("~", "");
                topic = topic.Replace("`", "");
                topic = topic.Replace("|", "");
                topic = topic.Replace(@"\", "");
                topic = topic.Replace("/", "");
                topic = topic.Replace("[", "");
                topic = topic.Replace("]", "");
                topic = topic.Replace("{", "");
                topic = topic.Replace("}", "");
                topic = topic.Replace(";", "");
                topic = topic.Replace(":", "");
                topic = topic.Replace("'", "");
                topic = topic.Replace("\"", "");
                topic = topic.Replace("?", "");
                topic = topic.Replace(",", "");
                topic = topic.Replace("<", "");
                topic = topic.Replace(">", "");
                topic = topic.Replace(".", "");
                if (char.IsNumber(topic, 0))
                {
                    topic = "NuM" + topic;
                }
            }
            return topic;
        }

        public static string MakeSafeForJavaScriptStringLiteral(string str)
        {
            str = str.Replace(@"\", @"\\");
            str = str.Replace("\"", "\\\"");
            return str;
        }

        private static void OpenBrowser(string queryString)
        {
            try
            {
                string remotePath;
                try
                {
                    if (Utilities.GetWebPage(WebRequest.Create(RemotePath)) == "")
                    {
                        remotePath = LocalFileURLEncode("file:///" + LocalPath.Replace(@"\", "/"));
                    }
                    else
                    {
                        remotePath = RemotePath;
                    }
                }
                catch
                {
                    remotePath = LocalFileURLEncode("file:///" + LocalPath.Replace(@"\", "/"));
                }
                Process.Start(GetExecutableFile(LocalPath), remotePath + queryString);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(rm.GetString("Could not find help files.  Please check that the following path is valid:") + "\r\n\r\n" + LocalPath, "Knowledge Matters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (BrowserNotFoundException)
            {
                MessageBox.Show(rm.GetString("Could not find the default browser's executable file."), "Knowledge Matters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Win32Exception)
            {
                MessageBox.Show(rm.GetString("Could not start the default browser."), "Knowledge Matters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception)
            {
                MessageBox.Show(rm.GetString("Unspecified error opening help."), "Knowledge Matters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public static void OpenDefinitions()
        {
            string queryString = "?topic=DEFINITIONS";
            OpenBrowser(queryString);
        }

        public static void OpenDefinitions(string definition)
        {
            OpenBrowser("?topic=DEFINITIONS&term=" + MakeSafeForDHTML(definition));
        }

        public static void OpenHelp()
        {
            string queryString = "";
            OpenBrowser(queryString);
        }

        public static void OpenHelp(string topic)
        {
            OpenBrowser("?topic=" + MakeSafeForDHTML(topic));
        }

        public static void OpenSearch()
        {
            string queryString = "?search=true";
            OpenBrowser(queryString);
        }

        public static void OpenSearch(string query)
        {
            OpenBrowser("?search=true&query=" + Utilities.URLEncode(query));
        }

        private class BrowserNotFoundException : Exception
        {
        }
    }
}
