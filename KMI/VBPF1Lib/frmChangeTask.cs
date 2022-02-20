using System;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.ComponentModel;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmChangeTask : Form {

        private Button bb_ShiftDown;
        private Button bb_ShiftUp;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Button btnQuit;
        private ListBox cboEnd;
        private ListBox cboStart;
        private bool change;
        private Container components = null;
        private GroupBox groupBox1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labMedical;
        private Label lb_Shift;
        private LinkLabel lnk401K;
        private LinkLabel lnkPayment;
        private LinkLabel lnkWitholding;
        private ResourceManager manager;
        private Panel panWorkTask;
        private Task task;
        private ToolTip toolTip;

        public frmChangeTask(Task task, bool change, bool weekend) {
            this.InitializeComponent();
            this.task = task;
            this.change = change;
            this.task.Weekend = weekend;
            this.AddTimes();
            this.cboStart.SelectedIndex = task.StartPeriod;
            this.cboEnd.SelectedIndex = task.EndPeriod;
            this.btnQuit.Visible = change;
            ResourceManager manager = new ResourceManager(typeof(frmChangeTask));
            this.labMedical.Image = (Image) manager.GetObject("labMedical.Image");
            if ((task is WorkTask) || (task is AttendClass)) {
                this.Make_Unchangable();
                if (task is WorkTask) {
                    this.Make_WorkTask();
                }
            }
        }

        private void AddTimes()
        {
            for (int i = 0; i < 0x30; i++)
            {
                DateTime time = new DateTime(1, 1, 1);
                string item = time.AddHours((double) (((float) i) / 2f)).ToShortTimeString();
                this.cboStart.Items.Add(item);
                this.cboEnd.Items.Add(item);
            }
        }

        private void bb_ShiftDown_Click(object sender, EventArgs e)
        {
            if (this.cboStart.SelectedIndex != 0x2f)
            {
                this.cboStart.SelectedIndex++;
            }
            else
            {
                this.cboStart.SelectedIndex = 0;
            }
            if (this.cboEnd.SelectedIndex != 0x2f)
            {
                this.cboEnd.SelectedIndex++;
            }
            else
            {
                this.cboEnd.SelectedIndex = 0;
            }
        }

        private void bb_ShiftUp_Click(object sender, EventArgs e)
        {
            if (this.cboStart.SelectedIndex > 0)
            {
                this.cboStart.SelectedIndex--;
            }
            else
            {
                this.cboStart.SelectedIndex = 0x2f;
            }
            if (this.cboEnd.SelectedIndex > 0)
            {
                this.cboEnd.SelectedIndex--;
            }
            else
            {
                this.cboEnd.SelectedIndex = 0x2f;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Schedule"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.cboStart.SelectedIndex == this.cboEnd.SelectedIndex)
            {
                MessageBox.Show("Tasks must be at least one hour long. Please try again.", "Invalid Entry");
            }
            else
            {
                try
                {
                    if (this.change)
                    {
                        if (this.cboStart.Enabled)
                        {
                            A.Adapter.EditTask(A.MainForm.CurrentEntityID, this.task.ID, this.cboStart.SelectedIndex, this.cboEnd.SelectedIndex);
                        }
                    }
                    else
                    {
                        this.task.StartPeriod = this.cboStart.SelectedIndex;
                        this.task.EndPeriod = this.cboEnd.SelectedIndex;
                        A.Adapter.AddTask(A.MainForm.CurrentEntityID, this.task);
                    }
                    ((frmDailyRoutine) base.Owner).UpdateForm();
                    base.Close();
                }
                catch (Exception exception)
                {
                    frmExceptionHandler.Handle(exception);
                    this.cboStart.SelectedIndex = this.task.StartPeriod;
                    this.cboEnd.SelectedIndex = this.task.EndPeriod;
                }
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(A.Resources.GetString("Are you sure you want to quit this activity?"), A.Resources.GetString("Confirm Quit"), MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (this.task is WorkTask)
                {
                    A.Adapter.DeleteTask(A.MainForm.CurrentEntityID, this.task.ID, false, true);
                }
                else
                {
                    A.Adapter.DeleteTask(A.MainForm.CurrentEntityID, this.task.ID);
                }
                ((frmDailyRoutine) base.Owner).UpdateForm();
                base.Close();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.groupBox1 = new GroupBox();
            this.cboEnd = new ListBox();
            this.cboStart = new ListBox();
            this.btnQuit = new Button();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.label3 = new Label();
            this.lnkWitholding = new LinkLabel();
            this.lnkPayment = new LinkLabel();
            this.lnk401K = new LinkLabel();
            this.panWorkTask = new Panel();
            this.labMedical = new Label();
            this.bb_ShiftUp = new Button();
            this.bb_ShiftDown = new Button();
            this.lb_Shift = new Label();
            this.groupBox1.SuspendLayout();
            this.panWorkTask.SuspendLayout();
            base.SuspendLayout();
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
            this.groupBox1.Controls.Add(this.lb_Shift);
            this.groupBox1.Controls.Add(this.bb_ShiftDown);
            this.groupBox1.Controls.Add(this.bb_ShiftUp);
            this.groupBox1.Controls.Add(this.cboEnd);
            this.groupBox1.Controls.Add(this.cboStart);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new Point(0x10, 0x10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xd0, 0x100);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time";
            this.cboEnd.Location = new Point(0x7b, 0x20);
            this.cboEnd.Name = "cboEnd";
            this.cboEnd.Size = new Size(0x45, 0xd4);
            this.cboEnd.TabIndex = 4;
            this.cboStart.Location = new Point(0x13, 0x20);
            this.cboStart.Name = "cboStart";
            this.cboStart.Size = new Size(0x45, 0xd4);
            this.cboStart.TabIndex = 3;
            this.btnQuit.BackColor = SystemColors.Control;
            this.btnQuit.Location = new Point(0x33, 0x116);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new Size(0x84, 0x18);
            this.btnQuit.TabIndex = 0;
            this.btnQuit.Text = "Stop Doing This Activity";
            this.btnQuit.UseVisualStyleBackColor = false;
            this.btnQuit.Click += new EventHandler(this.btnQuit_Click);
            this.btnOK.Location = new Point(0x20, 0x160);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x30, 0x18);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.Location = new Point(0x60, 0x160);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x30, 0x18);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(160, 0x160);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x30, 0x18);
            this.btnHelp.TabIndex = 9;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label3.Location = new Point(2, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x30, 0x10);
            this.label3.TabIndex = 10;
            this.label3.Text = "Change:";
            this.lnkWitholding.Location = new Point(0x34, 8);
            this.lnkWitholding.Name = "lnkWitholding";
            this.lnkWitholding.Size = new Size(0x40, 0x10);
            this.lnkWitholding.TabIndex = 11;
            this.lnkWitholding.TabStop = true;
            this.lnkWitholding.Text = "Withholding";
            this.lnkWitholding.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkWitholding_LinkClicked);
            this.lnkPayment.Location = new Point(120, 8);
            this.lnkPayment.Name = "lnkPayment";
            this.lnkPayment.Size = new Size(0x38, 0x10);
            this.lnkPayment.TabIndex = 12;
            this.lnkPayment.TabStop = true;
            this.lnkPayment.Text = "Payment";
            this.lnkPayment.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnkPayment_LinkClicked);
            this.lnk401K.Location = new Point(180, 8);
            this.lnk401K.Name = "lnk401K";
            this.lnk401K.Size = new Size(0x20, 0x10);
            this.lnk401K.TabIndex = 13;
            this.lnk401K.TabStop = true;
            this.lnk401K.Text = "401K";
            this.lnk401K.LinkClicked += new LinkLabelLinkClickedEventHandler(this.lnk401K_LinkClicked);
            this.panWorkTask.Controls.Add(this.lnkWitholding);
            this.panWorkTask.Controls.Add(this.lnkPayment);
            this.panWorkTask.Controls.Add(this.lnk401K);
            this.panWorkTask.Controls.Add(this.label3);
            this.panWorkTask.Location = new Point(8, 0x138);
            this.panWorkTask.Name = "panWorkTask";
            this.panWorkTask.Size = new Size(0xe0, 0x20);
            this.panWorkTask.TabIndex = 14;
            this.panWorkTask.Visible = false;
            this.labMedical.Location = new Point(0xc0, 0x114);
            this.labMedical.Name = "labMedical";
            this.labMedical.Size = new Size(0x20, 0x20);
            this.labMedical.TabIndex = 15;
            this.labMedical.Visible = false;
            this.labMedical.Click += new EventHandler(this.labMedical_Click);
            this.bb_ShiftUp.Location = new Point(0x5f, 0x6d);
            this.bb_ShiftUp.Name = "bb_ShiftUp";
            this.bb_ShiftUp.Size = new Size(0x16, 0x17);
            this.bb_ShiftUp.TabIndex = 5;
            this.bb_ShiftUp.Text = "▲";
            this.bb_ShiftUp.UseVisualStyleBackColor = true;
            this.bb_ShiftUp.Click += new EventHandler(this.bb_ShiftUp_Click);
            this.bb_ShiftDown.Location = new Point(0x5f, 0x8a);
            this.bb_ShiftDown.Name = "bb_ShiftDown";
            this.bb_ShiftDown.Size = new Size(0x16, 0x17);
            this.bb_ShiftDown.TabIndex = 6;
            this.bb_ShiftDown.Text = "▼";
            this.bb_ShiftDown.UseVisualStyleBackColor = true;
            this.bb_ShiftDown.Click += new EventHandler(this.bb_ShiftDown_Click);
            this.lb_Shift.Location = new Point(0x58, 0x4d);
            this.lb_Shift.Name = "lb_Shift";
            this.lb_Shift.Size = new Size(0x23, 0x1d);
            this.lb_Shift.TabIndex = 7;
            this.lb_Shift.Text = "Shift 30min";
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(240, 0x17e);
            base.Controls.Add(this.labMedical);
            base.Controls.Add(this.panWorkTask);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.btnQuit);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmChangeTask";
            base.ShowInTaskbar = false;
            this.Text = "Change Activity";
            this.groupBox1.ResumeLayout(false);
            this.panWorkTask.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void labMedical_Click(object sender, EventArgs e)
        {
        }

        private void lnk401K_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frm401K(this.task.ID).ShowDialog(this);
        }

        private void lnkPayment_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmMethodOfPay(this.task.ID).ShowDialog(this);
        }

        private void lnkWitholding_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmW4(this.task.ID).ShowDialog(this);
        }

        private void Make_Unchangable()
        {
            this.bb_ShiftDown.Enabled = false;
            this.bb_ShiftUp.Enabled = false;
            this.cboStart.Enabled = false;
            this.cboEnd.Enabled = false;
        }

        private void Make_WorkTask()
        {
            this.panWorkTask.Visible = true;
            this.lnkPayment.Enabled = A.MainForm.mnuActionsIncomePayment.Enabled;
            this.lnkWitholding.Enabled = A.MainForm.mnuActionsIncomeWitholding.Enabled;
            this.lnk401K.Enabled = A.MainForm.mnuActionsIncome401K.Enabled;
            this.lnk401K.Visible = ((WorkTask) this.task).R401KMatch > -1f;
            this.labMedical.Visible = ((WorkTask) this.task).HealthInsurance != null;
            this.toolTip = new ToolTip();
            this.toolTip.InitialDelay = 0;
            this.toolTip.SetToolTip(this.labMedical, "This job offers health insurance");
        }
    }
}

