namespace KMI.Sim
{
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.Remoting;
    using System.Text;
    using System.Windows.Forms;

    public class frmExceptionHandler : Form
    {
        protected static bool alreadyHandling = false;
        private Label cdKeyLabel;
        private Container components = null;
        private Button doneButton;
        private TextBox errorMessageTextBox;
        private TextBox errorTextTextBox;
        public static bool ORIGINAL_REPORT = false;
        private Button reportButton;
        public static string ReportURL = "http://www.knowledgematters.com/reports/bugs.php";
        public static string SupportEmail = "naate@taloreal.com";
        public static string SupportPhone = "1-315-398-4070";
        private string Title = "report.txt";
        private TextBox txtSchool;

        protected frmExceptionHandler()
        {
            this.InitializeComponent();
        }

        public void DeleteReportContents(object sender, EventArgs EAs)
        {
            System.IO.File.Delete(this.Title);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void ExceptionHandler_Load(object sender, EventArgs e)
        {
        }

        private void frmExceptionHandler_Closing(object sender, CancelEventArgs e)
        {
            Application.Exit();
        }

        private static string GenerateInformation(Exception e)
        {
            string str = "";
            StackTrace trace = null;
            StackFrame frame = null;
            if (e == null)
            {
                trace = new StackTrace(true);
            }
            else
            {
                trace = new StackTrace(e, true);
            }
            if (trace.FrameCount > 0)
            {
                frame = trace.GetFrame(0);
            }
            OperatingSystem oSVersion = Environment.OSVersion;
            if (oSVersion != null)
            {
                str = str + "OSPlatform: " + oSVersion.Platform.ToString() + "\r\n";
                if (oSVersion.Version != null)
                {
                    str = str + "OSVersion: " + oSVersion.Version.ToString() + "\r\n";
                }
            }
            if (e != null)
            {
                object obj2;
                if ((S.Instance != null) && (S.State != null))
                {
                    string[] textArray1 = new string[] { str, "Multiplayer: ", S.Instance.Multiplayer.ToString(), "\r\nSimStateID: ", S.State.GUID.ToString(), "\r\n" };
                    str = string.Concat(textArray1);
                }
                if ((e.Source != null) && (e.Source.Length > 0))
                {
                    str = str + "Module: " + e.Source + "\r\n";
                }
                if ((frame.GetFileName() != null) && (frame.GetFileName().Length > 0))
                {
                    str = str + "File: " + frame.GetFileName() + "\r\n";
                }
                if ((e.GetType().Name != null) && (e.GetType().Name.Length > 0))
                {
                    str = str + "Exception: " + e.GetType().Name + "\r\n";
                }
                if ((e.Message != null) && (e.Message.Length > 0))
                {
                    str = str + "Reason: " + e.Message + "\r\n";
                }
                if (e.TargetSite != null)
                {
                    str = str + "Target: " + e.TargetSite.ToString() + "\r\n";
                }
                if (frame.GetFileLineNumber() > 0)
                {
                    obj2 = str;
                    object[] objArray1 = new object[] { obj2, "Line: ", frame.GetFileLineNumber(), "\r\n" };
                    str = string.Concat(objArray1);
                }
                if (frame.GetFileColumnNumber() > 0)
                {
                    obj2 = str;
                    object[] objArray2 = new object[] { obj2, "Column: ", frame.GetFileColumnNumber(), "\r\n" };
                    str = string.Concat(objArray2);
                }
            }
            if (e != null)
            {
                return (str + "Stack Trace: " + e.StackTrace + "\r\n\r\n");
            }
            return (str + "Stack Trace: " + trace.ToString() + "\r\n\r\n");
        }

        public string[] GetComputerInfo()
        {
            return new string[] { ("Username: " + Environment.UserName.ToString()), ("Current Directory: " + Environment.CurrentDirectory), ("Computer Name: " + Environment.MachineName.ToString()), ("Operating System: Windows " + new OS_Identifier().getOSInfo()), ("LAN IP: " + this.GetLanIP()), ("WAN IP: " + this.GetWanIP()) };
        }

        public string GetLanIP()
        {
            string str = "0.0.0.0";
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    str = address.ToString();
                }
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

        public static void Handle(Exception e)
        {
            Handle(e, null);
        }

        public static void Handle(Exception e, Form errantForm)
        {
            if (!alreadyHandling)
            {
                alreadyHandling = true;
                if (e is SocketException)
                {
                    HandleRemoteError();
                }
                else if (e is RemotingException)
                {
                    HandleRemoteError();
                }
                else if (e is EntityNotFoundException)
                {
                    object[] args = new object[] { S.Instance.EntityName };
                    string caption = S.Resources.GetString("{0} No Longer Exists", args);
                    MessageBox.Show(e.Message, caption);
                    S.MainForm.UpdateView();
                    alreadyHandling = false;
                }
                else if (e is SimApplicationException)
                {
                    MessageBox.Show(e.Message, Application.ProductName);
                    alreadyHandling = false;
                }
                else
                {
                    frmExceptionHandler handler2 = new frmExceptionHandler {
                        Text = S.Resources.GetString("An unexpected error has occurred.")
                    };
                    string[] textArray1 = new string[5];
                    textArray1[0] = S.Resources.GetString("The simulation has encountered an unanticipated error from which it cannot recover.");
                    textArray1[1] = "\r\n\r\n";
                    textArray1[2] = S.Resources.GetString("If you would like to report this error please use the 'Report' button below.");
                    textArray1[3] = "\r\n\r\n";
                    object[] objArray2 = new object[] { SupportEmail };
                    textArray1[4] = S.Resources.GetString("Alternatively, you can send an e-mail to {0}.  Please include the text from the box below as part of your message.", objArray2);
                    handler2.MessageText = string.Concat(textArray1);
                    handler2.ErrorText = GenerateInformation(e);
                    frmExceptionHandler handler = handler2;
                    handler.reportButton.Visible = true;
                    handler.doneButton.Text = S.Resources.GetString("Close");
                    handler.ShowDialog();
                }
                if (errantForm != null)
                {
                    errantForm.Close();
                }
            }
        }

        protected static void HandleRemoteError()
        {
            if (S.Instance.SimTimeRunning)
            {
                S.MainForm.mnuOptionsGoStop.PerformClick();
            }
            MessageBox.Show(S.Resources.GetString("Apparently you have been disconnected from the host session. Either the network connection has failed or the host session is no longer running.  If the host is still running, you might be able to reconnect by doing a Multiplayer Join from the File menu."), S.Resources.GetString("Disconnected From Host"));
            new frmStartChoices().ShowDialog(S.MainForm);
            alreadyHandling = false;
        }

        public static bool HandleToLog(Exception e)
        {
            Exception exception;
            try
            {
                if (!EventLog.SourceExists(Application.ProductName) || !EventLog.Exists(Application.ProductName))
                {
                    try
                    {
                        EventLog.CreateEventSource(Application.ProductName, Application.ProductName);
                    }
                    catch (Exception exception2)
                    {
                        exception = exception2;
                        Handle(exception);
                        return false;
                    }
                }
                EventLog.WriteEntry(Application.ProductName, GenerateInformation(e), EventLogEntryType.Error, 0, 0);
            }
            catch (Exception exception3)
            {
                exception = exception3;
                Handle(exception);
                return false;
            }
            return true;
        }

        private void InitializeComponent()
        {
            this.errorTextTextBox = new TextBox();
            this.doneButton = new Button();
            this.reportButton = new Button();
            this.errorMessageTextBox = new TextBox();
            this.txtSchool = new TextBox();
            this.cdKeyLabel = new Label();
            base.SuspendLayout();
            this.errorTextTextBox.AcceptsReturn = true;
            this.errorTextTextBox.AcceptsTab = true;
            this.errorTextTextBox.Location = new Point(0x10, 0xb8);
            this.errorTextTextBox.Multiline = true;
            this.errorTextTextBox.Name = "errorTextTextBox";
            this.errorTextTextBox.ReadOnly = true;
            this.errorTextTextBox.ScrollBars = ScrollBars.Vertical;
            this.errorTextTextBox.Size = new Size(0x1c0, 0x70);
            this.errorTextTextBox.TabIndex = 3;
            this.errorTextTextBox.Text = "#error text";
            this.doneButton.Location = new Point(0x180, 0x138);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new Size(80, 0x18);
            this.doneButton.TabIndex = 5;
            this.doneButton.Text = "#text";
            this.doneButton.Click += new EventHandler(this.doneButton_Click);
            this.reportButton.Location = new Point(0x120, 0x138);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new Size(80, 0x18);
            this.reportButton.TabIndex = 4;
            this.reportButton.Text = "Report";
            this.reportButton.Visible = false;
            this.reportButton.Click += new EventHandler(this.reportButton_Click);
            this.errorMessageTextBox.AcceptsReturn = true;
            this.errorMessageTextBox.AcceptsTab = true;
            this.errorMessageTextBox.Location = new Point(0x10, 0x38);
            this.errorMessageTextBox.Multiline = true;
            this.errorMessageTextBox.Name = "errorMessageTextBox";
            this.errorMessageTextBox.ReadOnly = true;
            this.errorMessageTextBox.ScrollBars = ScrollBars.Vertical;
            this.errorMessageTextBox.Size = new Size(0x1c0, 0x70);
            this.errorMessageTextBox.TabIndex = 2;
            this.errorMessageTextBox.Text = "#error message";
            this.txtSchool.Location = new Point(0x60, 0x10);
            this.txtSchool.MaxLength = 0x80;
            this.txtSchool.Name = "txtSchool";
            this.txtSchool.Size = new Size(0x170, 20);
            this.txtSchool.TabIndex = 1;
            this.cdKeyLabel.Location = new Point(0, 9);
            this.cdKeyLabel.Name = "cdKeyLabel";
            this.cdKeyLabel.Size = new Size(90, 0x25);
            this.cdKeyLabel.TabIndex = 0;
            this.cdKeyLabel.Text = "Email Address or\r\nText Phone#:";
            this.cdKeyLabel.TextAlign = ContentAlignment.MiddleRight;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1e2, 0x160);
            base.Controls.Add(this.txtSchool);
            base.Controls.Add(this.errorMessageTextBox);
            base.Controls.Add(this.errorTextTextBox);
            base.Controls.Add(this.cdKeyLabel);
            base.Controls.Add(this.reportButton);
            base.Controls.Add(this.doneButton);
            base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            base.Name = "frmExceptionHandler";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ExceptionHandler";
            base.Closing += new CancelEventHandler(this.frmExceptionHandler_Closing);
            base.Load += new EventHandler(this.ExceptionHandler_Load);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            if (this.txtSchool.Text.Length == 0)
            {
                MessageBox.Show(S.Resources.GetString("Please fill in the school name field."));
                this.txtSchool.Focus();
            }
            else
            {
                try
                {
                    if (ORIGINAL_REPORT)
                    {
                        WebRequest request = WebRequest.Create(ReportURL);
                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        string s = "";
                        string[] textArray1 = new string[] { s, "school=", Utilities.URLEncode(this.txtSchool.Text), "&product=", Utilities.URLEncode(Application.ProductName), "&version=", Utilities.URLEncode(Application.ProductVersion), "&error_text=", Utilities.URLEncode(this.errorTextTextBox.Text) };
                        s = string.Concat(textArray1);
                        byte[] bytes = Encoding.ASCII.GetBytes(s);
                        request.ContentLength = bytes.Length;
                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                        string str2 = reader.ReadToEnd();
                        reader.Close();
                        if (str2 == "ReportSuccess")
                        {
                            object[] args = new object[] { SupportEmail, SupportPhone };
                            MessageBox.Show(S.Resources.GetString("ReportSuccess", args));
                            base.Close();
                        }
                        else
                        {
                            object[] objArray2 = new object[] { ReportURL, SupportEmail, SupportPhone };
                            MessageBox.Show(S.Resources.GetString("ReportFailure", objArray2));
                        }
                    }
                    else
                    {
                        try
                        {
                            string[] computerInfo = this.GetComputerInfo();
                            this.Title = "VBPF Report " + DateTime.Now.ToString().Replace("/", ",").Replace(":", ",") + ".txt";
                            TextWriter writer = new StreamWriter(this.Title);
                            string str3 = this.errorMessageTextBox.Text + "\r\n\r\n-----\r\n\r\n" + this.errorTextTextBox.Text;
                            foreach (string str4 in computerInfo)
                            {
                                writer.WriteLine(str4);
                            }
                            writer.WriteLine();
                            writer.WriteLine();
                            writer.Write(str3);
                            writer.WriteLine();
                            writer.WriteLine();
                            writer.WriteLine("Contact Info: " + this.txtSchool.Text);
                            writer.Close();
                            WebClient client = new WebClient();
                            client.Headers.Add("Content-Type", "binary/octet-stream");
                            client.UploadFileCompleted += new UploadFileCompletedEventHandler(this.DeleteReportContents);
                            client.UploadFileAsync(new Uri("http://www.taloreal.com/vbUpdates/Reports/UploadFile.php"), "POST", this.Title);
                            MessageBox.Show("Feedback sent.  Every bug report helps, Thank You.");
                        }
                        catch
                        {
                            System.IO.File.Delete(this.Title);
                            MessageBox.Show("We're sorry but your message could not be sent...");
                        }
                    }
                }
                catch
                {
                    object[] objArray3 = new object[] { ReportURL, SupportEmail, SupportPhone };
                    MessageBox.Show(S.Resources.GetString("ReportFailure", objArray3));
                }
            }
        }

        public string ErrorText
        {
            get
            {
                return this.errorTextTextBox.Text;
            }
            set
            {
                this.errorTextTextBox.Text = value;
            }
        }

        public string MessageText
        {
            get
            {
                return this.errorMessageTextBox.Text;
            }
            set
            {
                this.errorMessageTextBox.Text = value;
            }
        }
    }
}

