namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmChooseEventTime : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Button button1;
        private Button button2;
        private MonthCalendar Cal;
        private ListBox cboEnd;
        private ListBox cboStart;
        private Container components = null;
        private bool DatesBusy = false;
        private CheckBox friCheck;
        private TextBox GotoDate;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button Jump_bt;
        private TextBox jumpToDate_TB;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private CheckBox monCheck;
        private static DateTime Remember_Date = new DateTime(1, 1, 1);
        private static int[] Remember_Times = new int[2];
        private CheckBox saturCheck;
        private CheckBox sunCheck;
        private CheckBox thursCheck;
        private CheckBox tuesCheck;
        private CheckBox wednesCheck;

        public frmChooseEventTime()
        {
            this.InitializeComponent();
            for (int i = 0; i < 0x30; i++)
            {
                DateTime time = new DateTime(1, 1, 1);
                this.cboStart.Items.Add(time.AddHours((double) (((float) i) / 2f)).ToShortTimeString());
                time = new DateTime(1, 1, 1);
                this.cboEnd.Items.Add(time.AddHours((double) (((float) i) / 2f)).ToShortTimeString());
            }
            this.Cal.TodayDate = A.MainForm.Now.AddDays(1.0);
            this.Cal.SetDate(this.Cal.TodayDate);
            this.Cal.MinDate = this.Cal.TodayDate;
            if (Remember_Date.CompareTo(this.Cal.MinDate) < 0)
            {
                Remember_Date = this.Cal.MinDate;
            }
            else
            {
                this.Cal.SelectionStart = Remember_Date;
            }
            this.GetAllowedDays();
            this.Update_Date();
            this.Recall_Times();
        }

        private bool Allowed(DateTime day)
        {
            foreach (Control control in base.Controls)
            {
                if ((control is CheckBox) && ((control as CheckBox).Text == day.DayOfWeek.ToString()))
                {
                    return (control as CheckBox).Checked;
                }
            }
            return false;
        }

        private void AllowedDaysChanged(object sender, EventArgs e)
        {
            string[] textArray1 = new string[] { this.monCheck.Checked ? "1" : "0", this.tuesCheck.Checked ? "1" : "0", this.wednesCheck.Checked ? "1" : "0", this.thursCheck.Checked ? "1" : "0", this.friCheck.Checked ? "1" : "0", this.saturCheck.Checked ? "1" : "0", this.sunCheck.Checked ? "1" : "0" };
            string str = string.Concat(textArray1);
            if (str == "0000000")
            {
                ((CheckBox) sender).Checked = true;
            }
            else
            {
                Settings.SetValue<string>("AllowedDays", str);
                this.GoToAllowedDate();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Socializing"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Save_Times();
            try
            {
                if ((this.cboStart.SelectedIndex == -1) || (this.cboEnd.SelectedIndex == -1))
                {
                    MessageBox.Show(A.Resources.GetString("You must selected a starting and ending time. Please try again."), A.Resources.GetString("Input Required"));
                }
                else if (this.cboStart.SelectedIndex == this.cboEnd.SelectedIndex)
                {
                    MessageBox.Show(A.Resources.GetString("Events must last at least one-half hour. Please try again."), A.Resources.GetString("Input Required"));
                }
                else
                {
                    A.Adapter.SetParty(A.MainForm.CurrentEntityID, this.Cal.SelectionStart, this.cboStart.SelectedIndex, this.cboEnd.SelectedIndex);
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime time;
            if (this.IsValidDate(this.GotoDate.Text, out time))
            {
                this.Cal.SetDate(time);
            }
            this.GoToAllowedDate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cal.SelectionStart = this.Cal.MinDate;
            Remember_Date = this.Cal.MinDate;
            this.GoToAllowedDate();
            this.Update_Date();
        }

        private void Cal_DateChanged(object sender, DateRangeEventArgs e)
        {
            this.GoToAllowedDate();
        }

        private void Cal_DateSelected(object sender, DateRangeEventArgs e)
        {
            this.Update_Date();
            Settings.SetValue<DateTime>("RDate", this.Cal.SelectionStart);
        }

        private bool CheckDateExceptions(int[] DateInfo, out DateTime SET)
        {
            SET = new DateTime(1, 1, 1);
            if (DateInfo.Length != 3)
            {
                return false;
            }
            for (int i = 0; i != 3; i++)
            {
                if (DateInfo[i] == 0)
                {
                    return false;
                }
            }
            DateTime time = new DateTime(DateInfo[2], DateInfo[0], DateInfo[1]);
            int num = 0;
            if (DateTime.IsLeapYear(DateInfo[2]))
            {
                num++;
            }
            int[] numArray1 = new int[] { 0x1f, 0, 0x1f, 30, 0x1f, 30, 0x1f, 0x1f, 30, 0x1f, 30, 0x1f };
            numArray1[1] = 0x1c + num;
            int[] numArray = numArray1;
            if ((DateInfo[0] > 12) || (DateInfo[0] == 0))
            {
                return false;
            }
            if (DateInfo[1] > numArray[DateInfo[0]])
            {
                return false;
            }
            if (time.CompareTo(this.Cal.MinDate) < 0)
            {
                return false;
            }
            SET = time;
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetAllowedDays()
        {
            string str;
            this.DatesBusy = true;
            if (!Settings.GetValue<string>("AllowedDays", out str))
            {
                str = "1111111";
            }
            char ch = str[0];
            this.monCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[1];
            this.tuesCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[2];
            this.wednesCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[3];
            this.thursCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[4];
            this.friCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[5];
            this.saturCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            ch = str[6];
            this.sunCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ch.ToString()));
            this.DatesBusy = false;
        }

        private void GoToAllowedDate()
        {
            while (!this.Allowed(this.Cal.SelectionStart))
            {
                this.Cal.SelectionStart = this.Cal.SelectionStart.AddDays(1.0);
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new GroupBox();
            this.cboEnd = new ListBox();
            this.cboStart = new ListBox();
            this.label1 = new Label();
            this.label2 = new Label();
            this.groupBox2 = new GroupBox();
            this.Jump_bt = new Button();
            this.label5 = new Label();
            this.jumpToDate_TB = new TextBox();
            this.button2 = new Button();
            this.button1 = new Button();
            this.label3 = new Label();
            this.GotoDate = new TextBox();
            this.Cal = new MonthCalendar();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.label4 = new Label();
            this.monCheck = new CheckBox();
            this.tuesCheck = new CheckBox();
            this.wednesCheck = new CheckBox();
            this.thursCheck = new CheckBox();
            this.friCheck = new CheckBox();
            this.saturCheck = new CheckBox();
            this.sunCheck = new CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Controls.Add(this.cboEnd);
            this.groupBox1.Controls.Add(this.cboStart);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(0xf4, 0x10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd0, 0xc0);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time";
            this.cboEnd.Location = new Point(120, 0x20);
            this.cboEnd.Name = "cboEnd";
            this.cboEnd.Size = new Size(0x48, 0x93);
            this.cboEnd.TabIndex = 4;
            this.cboStart.Location = new Point(0x10, 0x20);
            this.cboStart.Name = "cboStart";
            this.cboStart.Size = new Size(0x48, 0x93);
            this.cboStart.TabIndex = 3;
            this.label1.Location = new Point(0x10, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x10);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Time:";
            this.label2.Location = new Point(120, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x48, 0x18);
            this.label2.TabIndex = 2;
            this.label2.Text = "End Time:";
            this.groupBox2.Controls.Add(this.Jump_bt);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.jumpToDate_TB);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.GotoDate);
            this.groupBox2.Controls.Add(this.Cal);
            this.groupBox2.Location = new Point(0x10, 0x10);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(210, 0xf7);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Date";
            this.Jump_bt.Location = new Point(0x7e, 0xd3);
            this.Jump_bt.Name = "Jump_bt";
            this.Jump_bt.Size = new Size(0x2e, 0x18);
            this.Jump_bt.TabIndex = 0x11;
            this.Jump_bt.Text = "Jump";
            this.Jump_bt.Click += new EventHandler(this.Jump_bt_Click);
            this.label5.Location = new Point(1, 0xd9);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x2f, 0x1b);
            this.label5.TabIndex = 0x10;
            this.label5.Text = "Jump # days";
            this.jumpToDate_TB.Location = new Point(0x36, 0xd6);
            this.jumpToDate_TB.Name = "jumpToDate_TB";
            this.jumpToDate_TB.Size = new Size(0x42, 20);
            this.jumpToDate_TB.TabIndex = 15;
            this.button2.Location = new Point(0x7e, 0xb9);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x2e, 0x18);
            this.button2.TabIndex = 14;
            this.button2.Text = "Now";
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button1.Location = new Point(0xac, 0xb9);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x26, 0x18);
            this.button1.TabIndex = 13;
            this.button1.Text = "Go";
            this.button1.Click += new EventHandler(this.button1_Click);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(1, 0xbf);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x24, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Go to:";
            this.GotoDate.Location = new Point(0x36, 0xbc);
            this.GotoDate.Name = "GotoDate";
            this.GotoDate.Size = new Size(0x42, 20);
            this.GotoDate.TabIndex = 1;
            this.Cal.Location = new Point(0x10, 20);
            this.Cal.MaxSelectionCount = 1;
            this.Cal.Name = "Cal";
            this.Cal.ShowToday = false;
            this.Cal.ShowTodayCircle = false;
            this.Cal.TabIndex = 0;
            this.Cal.DateChanged += new DateRangeEventHandler(this.Cal_DateChanged);
            this.Cal.DateSelected += new DateRangeEventHandler(this.Cal_DateSelected);
            this.btnHelp.Location = new Point(310, 0x139);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 12;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.Location = new Point(190, 0x139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Close";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(70, 0x139);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x18);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "Set Party Date";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(12, 0x10d);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x3d, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Party Days:";
            this.monCheck.AutoSize = true;
            this.monCheck.Checked = true;
            this.monCheck.CheckState = CheckState.Checked;
            this.monCheck.Location = new Point(0x4f, 0x10d);
            this.monCheck.Name = "monCheck";
            this.monCheck.Size = new Size(0x40, 0x11);
            this.monCheck.TabIndex = 14;
            this.monCheck.Text = "Monday";
            this.monCheck.UseVisualStyleBackColor = true;
            this.monCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.tuesCheck.AutoSize = true;
            this.tuesCheck.Checked = true;
            this.tuesCheck.CheckState = CheckState.Checked;
            this.tuesCheck.Location = new Point(0x95, 0x10d);
            this.tuesCheck.Name = "tuesCheck";
            this.tuesCheck.Size = new Size(0x43, 0x11);
            this.tuesCheck.TabIndex = 15;
            this.tuesCheck.Text = "Tuesday";
            this.tuesCheck.UseVisualStyleBackColor = true;
            this.tuesCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.wednesCheck.AutoSize = true;
            this.wednesCheck.Checked = true;
            this.wednesCheck.CheckState = CheckState.Checked;
            this.wednesCheck.Location = new Point(0xde, 0x10d);
            this.wednesCheck.Name = "wednesCheck";
            this.wednesCheck.Size = new Size(0x53, 0x11);
            this.wednesCheck.TabIndex = 0x10;
            this.wednesCheck.Text = "Wednesday";
            this.wednesCheck.UseVisualStyleBackColor = true;
            this.wednesCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.thursCheck.AutoSize = true;
            this.thursCheck.Checked = true;
            this.thursCheck.CheckState = CheckState.Checked;
            this.thursCheck.Location = new Point(310, 0x10d);
            this.thursCheck.Name = "thursCheck";
            this.thursCheck.Size = new Size(70, 0x11);
            this.thursCheck.TabIndex = 0x11;
            this.thursCheck.Text = "Thursday";
            this.thursCheck.UseVisualStyleBackColor = true;
            this.thursCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.friCheck.AutoSize = true;
            this.friCheck.Checked = true;
            this.friCheck.CheckState = CheckState.Checked;
            this.friCheck.Location = new Point(0x4f, 290);
            this.friCheck.Name = "friCheck";
            this.friCheck.Size = new Size(0x36, 0x11);
            this.friCheck.TabIndex = 0x12;
            this.friCheck.Text = "Friday";
            this.friCheck.UseVisualStyleBackColor = true;
            this.friCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.saturCheck.AutoSize = true;
            this.saturCheck.Checked = true;
            this.saturCheck.CheckState = CheckState.Checked;
            this.saturCheck.Location = new Point(0x95, 290);
            this.saturCheck.Name = "saturCheck";
            this.saturCheck.Size = new Size(0x44, 0x11);
            this.saturCheck.TabIndex = 0x13;
            this.saturCheck.Text = "Saturday";
            this.saturCheck.UseVisualStyleBackColor = true;
            this.saturCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.sunCheck.AutoSize = true;
            this.sunCheck.Checked = true;
            this.sunCheck.CheckState = CheckState.Checked;
            this.sunCheck.Location = new Point(0xde, 290);
            this.sunCheck.Name = "sunCheck";
            this.sunCheck.Size = new Size(0x3e, 0x11);
            this.sunCheck.TabIndex = 20;
            this.sunCheck.Text = "Sunday";
            this.sunCheck.UseVisualStyleBackColor = true;
            this.sunCheck.CheckedChanged += new EventHandler(this.AllowedDaysChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x1d8, 0x157);
            base.Controls.Add(this.sunCheck);
            base.Controls.Add(this.saturCheck);
            base.Controls.Add(this.friCheck);
            base.Controls.Add(this.thursCheck);
            base.Controls.Add(this.wednesCheck);
            base.Controls.Add(this.tuesCheck);
            base.Controls.Add(this.monCheck);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmChooseEventTime";
            base.ShowInTaskbar = false;
            this.Text = "Plan Your Party";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private bool IsValidDate(string TEXT, out DateTime DATE)
        {
            DATE = new DateTime(1, 1, 1);
            try
            {
                char[] separator = new char[] { '/' };
                string[] strArray = TEXT.Split(separator);
                int[] dateInfo = new int[strArray.Length];
                for (int i = 0; i != 3; i++)
                {
                    dateInfo[i] = Convert.ToInt32(strArray[i]);
                }
                return this.CheckDateExceptions(dateInfo, out DATE);
            }
            catch
            {
                return false;
            }
        }

        private void Jump_bt_Click(object sender, EventArgs e)
        {
            if (!this.DatesBusy)
            {
                int result = 0;
                if (int.TryParse(this.jumpToDate_TB.Text, out result) || (result <= 0))
                {
                    this.Cal.SelectionStart = this.Cal.SelectionStart.AddDays((double) result);
                    Settings.SetValue<int>("JumpDays", result);
                    this.GoToAllowedDate();
                }
            }
        }

        private void Recall_Times()
        {
            int num;
            int result = -1;
            DateTime time = new DateTime(1, 1, 1);
            int num3 = 0;
            if (Settings.GetValue<int>("JumpDays", out num3))
            {
                this.jumpToDate_TB.Text = num3.ToString();
            }
            Settings.GetValue<int>("AllowedTime0", out num);
            Settings.GetValue<int>("AllowedTime1", out result);
            Settings.GetValue<DateTime>("RDate", out time);
            this.cboStart.SelectedIndex = num;
            this.cboEnd.SelectedIndex = result;
            this.Cal.SelectionStart = (this.Cal.MinDate < time) ? time : this.Cal.MinDate;
            this.GoToAllowedDate();
        }

        private void Save_Times()
        {
            Settings.SetValue<int>("AllowedTime0", this.cboStart.SelectedIndex);
            Settings.SetValue<int>("AllowedTime1", this.cboEnd.SelectedIndex);
        }

        private void Update_Date()
        {
            int[] numArray = new int[] { this.Cal.SelectionStart.Year, this.Cal.SelectionStart.Month, this.Cal.SelectionStart.Day };
            object[] objArray1 = new object[] { numArray[1], "/", numArray[2], "/", numArray[0] };
            this.GotoDate.Text = string.Concat(objArray1);
        }
    }
}

