namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmDailyRoutine : Form
    {
        private LinkLabel AddEat;
        private LinkLabel AddRelaxation;
        private LinkLabel AddSleep;
        private LinkLabel AddWorkout;
        private Button btnChangeTravel;
        private Button btnClose;
        private Button btnHelp;
        private Button btnHostAParty;
        private CheckedListBox chkEvents;
        private Container components = null;
        private LinkLabel CopyWeekdays;
        private LinkLabel CopyWeekends;
        private DailyRoutine[] dailyRoutines;
        private const int hrSpacing = 7;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label13;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabel2;
        private LinkLabel linkLabel3;
        private LinkLabel linkLabel4;
        private Panel panel2;
        private Panel[] panMains;
        private Panel panMainWD;
        private Panel panMainWE;
        private Panel panSpecialEvents;
        private AppSimSettings simSettings;

        public frmDailyRoutine()
        {
            this.InitializeComponent();
            this.DrawSchedule(this.panMainWD);
            this.DrawSchedule(this.panMainWE);
            this.panMains = new Panel[] { this.panMainWD, this.panMainWE };
            this.simSettings = (AppSimSettings) A.Adapter.getSimSettings();
            this.panSpecialEvents.Visible = this.simSettings.HealthFactorsToConsider > 4;
            if (this.simSettings.ScheduleReadOnly)
            {
                foreach (Control control in base.Controls)
                {
                    if (control is LinkLabel)
                    {
                        control.Enabled = false;
                    }
                }
            }
            this.UpdateForm();
            this.btnChangeTravel.Enabled = A.MainForm.mnuActionsLivingTransportation.Enabled;
        }

        private void AddEat_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.AddTask(new Eat(), bool.Parse((string) ((Control) sender).Tag));
        }

        private void AddRelaxation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.AddTask(new Relax(), bool.Parse((string) ((Control) sender).Tag));
        }

        private void AddSleep_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.AddTask(new Sleep(), bool.Parse((string) ((Control) sender).Tag));
        }

        private void AddTask(Task task, bool weekend)
        {
            try
            {
                new frmChangeTask(task, false, weekend).ShowDialog(this);
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        public void AddTasks(int i)
        {
            IEnumerator enumerator = this.dailyRoutines[i].Tasks.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Task current = (Task) enumerator.Current;
                Label label = new Label {
                    Location = new Point(-1, ((0x2b + (current.StartPeriod * 7)) - this.panMains[i].Top) + 4),
                    AutoSize = false,
                    Tag = current
                };
                this.panMains[i].Controls.Add(label);
                label.Size = new Size(this.panMains[i].Width - 12, current.Duration * 7);
                if (current.StartPeriod > current.EndPeriod)
                {
                    label.Size = new Size(this.panMains[i].Width - 12, (0x30 - current.StartPeriod) * 7);
                    Label label2 = new Label {
                        Tag = label.Tag,
                        AutoSize = false,
                        Location = new Point(-1, (0x2b - this.panMains[i].Top) + 4)
                    };
                    this.panMains[i].Controls.Add(label2);
                    label2.Size = new Size(this.panMains[i].Width - 12, current.EndPeriod * 7);
                }
            }
        }

        private void AddWorkout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.AddTask(new Exercise(), bool.Parse((string) ((Control) sender).Tag));
        }

        private void btnChangeTravel_Click(object sender, EventArgs e)
        {
            A.MainForm.mnuActionsLivingTransportation.PerformClick();
            this.UpdateForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnHostAParty_Click(object sender, EventArgs e)
        {
            new frmChooseEventTime().ShowDialog(this);
            this.UpdateForm();
        }

        public bool CanCopy(Task T, int To)
        {
            bool flag = true;
            if (T.Building.BuildingType.Name != "Apartments")
            {
                flag = false;
            }
            if (!this.dailyRoutines[To].CheckConflicts(T))
            {
                flag = false;
            }
            if (((T.CategoryName() != A.Resources.GetString("Sleep")) && (T.CategoryName() != A.Resources.GetString("Exercise"))) && ((T.CategoryName() != A.Resources.GetString("Relax")) && (T.CategoryName() != A.Resources.GetString("Eat"))))
            {
                flag = false;
            }
            return flag;
        }

        private void chkEvents_SelectedValueChanged(object sender, EventArgs e)
        {
            ArrayList eventIDs = new ArrayList();
            foreach (Task task in this.chkEvents.CheckedItems)
            {
                eventIDs.Add(task.ID);
            }
            A.Adapter.SetOneTimeEvents(A.MainForm.CurrentEntityID, eventIDs);
            this.UpdateForm();
        }

        public void Copy(int From)
        {
            this.dailyRoutines = A.Adapter.GetDailyRoutines(A.MainForm.CurrentEntityID);
            this.Transition(From);
            this.UpdateForm();
        }

        private void CopyWeekdays_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Copy(0);
        }

        private void CopyWeekends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Copy(1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected void DrawSchedule(Panel p)
        {
            for (int i = 0; i < 0x30; i++)
            {
                Label label = new Label();
                DateTime time = new DateTime(1, 1, 1);
                label.Text = time.AddHours((double) (((float) i) / 2f)).ToShortTimeString();
                label.Location = new Point(p.Left - 0x30, 0x2d + (i * 7));
                label.Font = new Font("Arial", 7f);
                label.Size = new Size(0x2e, 0x10);
                label.TextAlign = ContentAlignment.TopRight;
                label.ForeColor = Color.FromArgb(110, 110, 110);
                Label label2 = new Label {
                    BackColor = Color.LightGray
                };
                if ((i % 2) == 0)
                {
                    label2.BackColor = Color.DarkGray;
                }
                label2.Location = new Point(0, (label.Top - p.Top) + 2);
                label2.Size = new Size(p.Width, 1);
                p.Controls.Add(label2);
                label2.Enabled = false;
                if ((i % 2) == 0)
                {
                    base.Controls.Add(label);
                }
            }
        }

        public Task GenerateCopy(Task T)
        {
            List<Task> list = new List<Task>();
            if (T.CategoryName() == A.Resources.GetString("Sleep"))
            {
                list.Add(new Sleep());
            }
            if (T.CategoryName() == A.Resources.GetString("Exercise"))
            {
                list.Add(new Exercise());
            }
            if (T.CategoryName() == A.Resources.GetString("Relax"))
            {
                list.Add(new Relax());
            }
            if (T.CategoryName() == A.Resources.GetString("Eat"))
            {
                list.Add(new Eat());
            }
            list[0].Weekend = !T.Weekend;
            list[0].StartPeriod = T.StartPeriod;
            list[0].EndPeriod = T.EndPeriod;
            list[0].Building = T.Building;
            return list[0];
        }

        private void InitializeComponent()
        {
            this.panMainWD = new Panel();
            this.btnClose = new Button();
            this.label1 = new Label();
            this.AddWorkout = new LinkLabel();
            this.AddRelaxation = new LinkLabel();
            this.AddSleep = new LinkLabel();
            this.AddEat = new LinkLabel();
            this.label2 = new Label();
            this.label3 = new Label();
            this.btnChangeTravel = new Button();
            this.panel2 = new Panel();
            this.btnHelp = new Button();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label6 = new Label();
            this.linkLabel1 = new LinkLabel();
            this.linkLabel2 = new LinkLabel();
            this.linkLabel3 = new LinkLabel();
            this.linkLabel4 = new LinkLabel();
            this.label7 = new Label();
            this.panMainWE = new Panel();
            this.label8 = new Label();
            this.label9 = new Label();
            this.label10 = new Label();
            this.btnHostAParty = new Button();
            this.label11 = new Label();
            this.chkEvents = new CheckedListBox();
            this.label12 = new Label();
            this.label13 = new Label();
            this.panSpecialEvents = new Panel();
            this.CopyWeekdays = new LinkLabel();
            this.CopyWeekends = new LinkLabel();
            this.panel2.SuspendLayout();
            this.panSpecialEvents.SuspendLayout();
            base.SuspendLayout();
            this.panMainWD.BackColor = Color.White;
            this.panMainWD.BorderStyle = BorderStyle.FixedSingle;
            this.panMainWD.Location = new Point(0x40, 0x30);
            this.panMainWD.Name = "panMainWD";
            this.panMainWD.Size = new Size(0x48, 0x150);
            this.panMainWD.TabIndex = 0;
            this.btnClose.Location = new Point(0x1d0, 0x10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x48, 0x18);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.btnClose_Click);
            this.label1.Location = new Point(0x98, 0x38);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x30, 0x20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Add time to:";
            this.AddWorkout.Location = new Point(0x98, 0x98);
            this.AddWorkout.Name = "AddWorkout";
            this.AddWorkout.Size = new Size(0x38, 0x10);
            this.AddWorkout.TabIndex = 5;
            this.AddWorkout.TabStop = true;
            this.AddWorkout.Tag = "false";
            this.AddWorkout.Text = "Exercise";
            this.AddWorkout.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddWorkout_LinkClicked);
            this.AddRelaxation.Location = new Point(0x98, 0x80);
            this.AddRelaxation.Name = "AddRelaxation";
            this.AddRelaxation.Size = new Size(0x38, 0x10);
            this.AddRelaxation.TabIndex = 6;
            this.AddRelaxation.TabStop = true;
            this.AddRelaxation.Tag = "false";
            this.AddRelaxation.Text = "Relax";
            this.AddRelaxation.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddRelaxation_LinkClicked);
            this.AddSleep.Location = new Point(0x98, 0xb0);
            this.AddSleep.Name = "AddSleep";
            this.AddSleep.Size = new Size(0x38, 0x10);
            this.AddSleep.TabIndex = 7;
            this.AddSleep.TabStop = true;
            this.AddSleep.Tag = "false";
            this.AddSleep.Text = "Sleep";
            this.AddSleep.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddSleep_LinkClicked);
            this.AddEat.Location = new Point(0x98, 0x68);
            this.AddEat.Name = "AddEat";
            this.AddEat.Size = new Size(0x38, 0x10);
            this.AddEat.TabIndex = 11;
            this.AddEat.TabStop = true;
            this.AddEat.Tag = "false";
            this.AddEat.Text = "Eat";
            this.AddEat.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddEat_LinkClicked);
            this.label2.BackColor = Color.Violet;
            this.label2.BorderStyle = BorderStyle.FixedSingle;
            this.label2.Location = new Point(200, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x18);
            this.label2.TabIndex = 12;
            this.label2.Text = "Est. Travel Time";
            this.label2.TextAlign = ContentAlignment.MiddleCenter;
            this.label3.Location = new Point(8, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x98, 0x20);
            this.label3.TabIndex = 13;
            this.label3.Text = "To edit or delete an existing activity, click it.";
            this.label3.TextAlign = ContentAlignment.BottomCenter;
            this.btnChangeTravel.Location = new Point(0x138, 0x10);
            this.btnChangeTravel.Name = "btnChangeTravel";
            this.btnChangeTravel.Size = new Size(0x80, 0x18);
            this.btnChangeTravel.TabIndex = 14;
            this.btnChangeTravel.Text = "Change Travel Mode";
            this.btnChangeTravel.Click += new EventHandler(this.btnChangeTravel_Click);
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.btnChangeTravel);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Location = new Point(0, 400);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x278, 0x38);
            this.panel2.TabIndex = 0x15;
            this.btnHelp.Location = new Point(0x228, 0x10);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x40, 0x18);
            this.btnHelp.TabIndex = 15;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label5.BackColor = SystemColors.ControlDarkDark;
            this.label5.Location = new Point(0xd0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new Size(1, 400);
            this.label5.TabIndex = 0x16;
            this.label4.BackColor = SystemColors.ControlDarkDark;
            this.label4.Location = new Point(0, 400);
            this.label4.Name = "label4";
            this.label4.Size = new Size(640, 1);
            this.label4.TabIndex = 0x17;
            this.label4.Click += new EventHandler(this.label4_Click);
            this.label6.BackColor = SystemColors.ControlDarkDark;
            this.label6.Location = new Point(0x1a8, 0);
            this.label6.Name = "label6";
            this.label6.Size = new Size(1, 400);
            this.label6.TabIndex = 30;
            this.linkLabel1.Location = new Point(0x170, 0x68);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new Size(0x38, 0x10);
            this.linkLabel1.TabIndex = 0x1d;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Tag = "true";
            this.linkLabel1.Text = "Eat";
            this.linkLabel1.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddEat_LinkClicked);
            this.linkLabel2.Location = new Point(0x170, 0xb0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new Size(0x38, 0x10);
            this.linkLabel2.TabIndex = 0x1c;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Tag = "true";
            this.linkLabel2.Text = "Sleep";
            this.linkLabel2.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddSleep_LinkClicked);
            this.linkLabel3.Location = new Point(0x170, 0x80);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new Size(0x38, 0x10);
            this.linkLabel3.TabIndex = 0x1b;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Tag = "true";
            this.linkLabel3.Text = "Relax";
            this.linkLabel3.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddRelaxation_LinkClicked);
            this.linkLabel4.Location = new Point(0x170, 0x98);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new Size(0x38, 0x10);
            this.linkLabel4.TabIndex = 0x1a;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Tag = "true";
            this.linkLabel4.Text = "Exercise";
            this.linkLabel4.LinkClicked += new LinkLabelLinkClickedEventHandler(this.AddWorkout_LinkClicked);
            this.label7.Location = new Point(0x170, 0x38);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x30, 0x20);
            this.label7.TabIndex = 0x19;
            this.label7.Text = "Add time to:";
            this.panMainWE.BackColor = Color.White;
            this.panMainWE.BorderStyle = BorderStyle.FixedSingle;
            this.panMainWE.Location = new Point(280, 0x30);
            this.panMainWE.Name = "panMainWE";
            this.panMainWE.Size = new Size(0x48, 0x150);
            this.panMainWE.TabIndex = 0x18;
            this.label8.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label8.ForeColor = SystemColors.ControlDarkDark;
            this.label8.Location = new Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0xb8, 0x18);
            this.label8.TabIndex = 0x1f;
            this.label8.Text = "Weekday";
            this.label8.TextAlign = ContentAlignment.TopCenter;
            this.label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label9.ForeColor = SystemColors.ControlDarkDark;
            this.label9.Location = new Point(0xe0, 8);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0xb8, 0x18);
            this.label9.TabIndex = 0x20;
            this.label9.Text = "Weekend";
            this.label9.TextAlign = ContentAlignment.TopCenter;
            this.label10.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label10.ForeColor = SystemColors.ControlDarkDark;
            this.label10.Location = new Point(8, 8);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0xb8, 0x18);
            this.label10.TabIndex = 0x21;
            this.label10.Text = "Special Events";
            this.label10.TextAlign = ContentAlignment.TopCenter;
            this.btnHostAParty.Location = new Point(0x10, 40);
            this.btnHostAParty.Name = "btnHostAParty";
            this.btnHostAParty.Size = new Size(160, 0x18);
            this.btnHostAParty.TabIndex = 0x22;
            this.btnHostAParty.Text = "Host a Party";
            this.btnHostAParty.Click += new EventHandler(this.btnHostAParty_Click);
            this.label11.Location = new Point(0x10, 80);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x70, 0x10);
            this.label11.TabIndex = 0x23;
            this.label11.Text = "Attend Events:";
            this.chkEvents.CheckOnClick = true;
            this.chkEvents.HorizontalScrollbar = true;
            this.chkEvents.Location = new Point(0x10, 0x68);
            this.chkEvents.Name = "chkEvents";
            this.chkEvents.Size = new Size(160, 0xc7);
            this.chkEvents.TabIndex = 0x24;
            this.chkEvents.SelectedValueChanged += new EventHandler(this.chkEvents_SelectedValueChanged);
            this.label12.Location = new Point(0x20, 0x138);
            this.label12.Name = "label12";
            this.label12.Size = new Size(0x80, 0x10);
            this.label12.TabIndex = 0x25;
            this.label12.Text = "Check the box to attend.";
            this.label13.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.label13.Location = new Point(0x11, 0x148);
            this.label13.Name = "label13";
            this.label13.Size = new Size(0xa8, 0x40);
            this.label13.TabIndex = 0x26;
            this.label13.Text = "If the special event begins during a scheduled activity, you will complete that activity first then go to the event. If the event has ended by that time, you will not get credit for attending.";
            this.panSpecialEvents.Controls.Add(this.label10);
            this.panSpecialEvents.Controls.Add(this.btnHostAParty);
            this.panSpecialEvents.Controls.Add(this.label11);
            this.panSpecialEvents.Controls.Add(this.chkEvents);
            this.panSpecialEvents.Controls.Add(this.label12);
            this.panSpecialEvents.Controls.Add(this.label13);
            this.panSpecialEvents.Location = new Point(0x1a8, 0);
            this.panSpecialEvents.Name = "panSpecialEvents";
            this.panSpecialEvents.Size = new Size(200, 400);
            this.panSpecialEvents.TabIndex = 0x27;
            this.CopyWeekdays.Location = new Point(0x8e, 0x15f);
            this.CopyWeekdays.Name = "CopyWeekdays";
            this.CopyWeekdays.Size = new Size(60, 0x21);
            this.CopyWeekdays.TabIndex = 40;
            this.CopyWeekdays.TabStop = true;
            this.CopyWeekdays.Tag = "false";
            this.CopyWeekdays.Text = "Copy To Weekend";
            this.CopyWeekdays.TextAlign = ContentAlignment.TopCenter;
            this.CopyWeekdays.LinkClicked += new LinkLabelLinkClickedEventHandler(this.CopyWeekdays_LinkClicked);
            this.CopyWeekends.Location = new Point(0xd7, 0x15f);
            this.CopyWeekends.Name = "CopyWeekends";
            this.CopyWeekends.Size = new Size(60, 0x21);
            this.CopyWeekends.TabIndex = 0x29;
            this.CopyWeekends.TabStop = true;
            this.CopyWeekends.Tag = "false";
            this.CopyWeekends.Text = "Copy To Weekday";
            this.CopyWeekends.TextAlign = ContentAlignment.TopCenter;
            this.CopyWeekends.LinkClicked += new LinkLabelLinkClickedEventHandler(this.CopyWeekends_LinkClicked);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x27a, 0x1c3);
            base.Controls.Add(this.CopyWeekends);
            base.Controls.Add(this.CopyWeekdays);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.linkLabel1);
            base.Controls.Add(this.linkLabel2);
            base.Controls.Add(this.linkLabel3);
            base.Controls.Add(this.linkLabel4);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.panMainWE);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.AddEat);
            base.Controls.Add(this.AddSleep);
            base.Controls.Add(this.AddRelaxation);
            base.Controls.Add(this.AddWorkout);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.panMainWD);
            base.Controls.Add(this.panSpecialEvents);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmDailyRoutine";
            base.ShowInTaskbar = false;
            this.Text = "Schedule";
            this.panel2.ResumeLayout(false);
            this.panSpecialEvents.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        public void RemoveOld(int i)
        {
            ArrayList list = new ArrayList(this.panMains[i].Controls);
            foreach (Control control in list)
            {
                if (control.Tag != null)
                {
                    this.panMains[i].Controls.Remove(control);
                }
            }
        }

        private void Task_Click(object sender, EventArgs e)
        {
            if (this.simSettings.ScheduleReadOnly)
            {
                MessageBox.Show("You can view but not change your schedule in this lesson.", "Changes Disabled");
            }
            else
            {
                try
                {
                    Control control = (Control) sender;
                    Task tag = (Task) control.Tag;
                    new frmChangeTask(tag, true, tag.Weekend).ShowDialog(this);
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                }
            }
        }

        public void TaskStyling(int i)
        {
            foreach (Control control in this.panMains[i].Controls)
            {
                if (control.Tag != null)
                {
                    Task tag = (Task) control.Tag;
                    Label label = (Label) control;
                    label.BackColor = tag.GetColor();
                    label.BorderStyle = BorderStyle.FixedSingle;
                    label.BringToFront();
                    if (tag is TravelTask)
                    {
                        label.Width = 12;
                        label.Left = this.panMains[i].Width - 12;
                    }
                    else
                    {
                        label.Click += new EventHandler(this.Task_Click);
                        label.Cursor = Cursors.Hand;
                        label.TextAlign = ContentAlignment.MiddleCenter;
                        label.Text = tag.CategoryName();
                    }
                }
            }
        }

        public void Transition(int From)
        {
            DailyRoutine routine = this.dailyRoutines[From];
            List<Task> list = new List<Task>();
            foreach (Task task in routine.Tasks.Values)
            {
                if (this.CanCopy(task, (From + 1) % 2))
                {
                    A.Adapter.AddTask(A.MainForm.CurrentEntityID, this.GenerateCopy(task));
                }
            }
        }

        public void UpdateForm()
        {
            this.dailyRoutines = A.Adapter.GetDailyRoutines(A.MainForm.CurrentEntityID);
            for (int i = 0; i < 2; i++)
            {
                this.RemoveOld(i);
                this.AddTasks(i);
                this.TaskStyling(i);
            }
            this.UpdateParties();
        }

        public void UpdateParties()
        {
            SortedList oneTimeEventsInvitedTo = A.Adapter.GetOneTimeEventsInvitedTo(A.MainForm.CurrentEntityID);
            SortedList oneTimeEventsAttending = A.Adapter.GetOneTimeEventsAttending(A.MainForm.CurrentEntityID);
            int index = 0;
            this.chkEvents.Items.Clear();
            foreach (OneTimeEvent event2 in oneTimeEventsInvitedTo.Values)
            {
                this.chkEvents.Items.Add(event2);
                if (oneTimeEventsAttending.ContainsKey(event2.Key))
                {
                    this.chkEvents.SetItemChecked(index, true);
                }
                index++;
            }
        }
    }
}

