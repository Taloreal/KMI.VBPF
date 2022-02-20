namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frm401K : Form
    {
        private Label AllocatedAmount;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Container components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Input input;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labMatch;
        private FlowLayoutPanel panFunds;
        private long taskID;
        private NumericUpDown updPercent;

        public frm401K(long taskID)
        {
            this.InitializeComponent();
            ArrayList funds = A.Adapter.GetFunds();
            this.input = A.Adapter.Get401K(A.MainForm.CurrentEntityID, taskID);
            this.taskID = taskID;
            this.updPercent.Value = ((decimal) this.input.PercentWitheld) * 100M;
            if (this.updPercent.Value != decimal.Zero)
            {
                this.AllocatedAmount.Text = "Allocated amount : 100%";
            }
            for (int i = 0; i != funds.Count; i++)
            {
                Fund fund = (Fund) funds[i];
                AllocationControl control = new AllocationControl {
                    Top = i * base.Height,
                    Tag = fund
                };
                control.updPct.Value = ((decimal) this.input.Allocations[i]) * 100M;
                control.updPct.TextChanged += new EventHandler(this.ValueChanged);
                control.labFundName.Text = fund.Name;
                if ((i % 2) == 1)
                {
                    control.BackColor = Color.LightGray;
                }
                this.panFunds.Controls.Add(control);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("View Retirement Portfolio"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            decimal total = new decimal();
            float[] allocations = this.CountInvestments(out total);
            if ((this.updPercent.Value > decimal.Zero) && (total != 100M))
            {
                string str = "less";
                if (total > 100M)
                {
                    str = "more";
                }
                object[] args = new object[] { str };
                MessageBox.Show(A.Resources.GetString("Your investment allocation percentages add to {0} than 100%. Please try again.", args), "Try Again");
            }
            else
            {
                A.Adapter.Set401K(A.MainForm.CurrentEntityID, this.taskID, ((float) this.updPercent.Value) / 100f, allocations);
                base.Close();
            }
        }

        public float[] CountInvestments(out decimal Total)
        {
            Total = new decimal();
            int num = 0;
            float[] numArray = new float[this.panFunds.Controls.Count];
            foreach (AllocationControl control in this.panFunds.Controls)
            {
                Total += control.updPct.Value;
                numArray[num++] = ((float) control.updPct.Value) / 100f;
            }
            return numArray;
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
            this.label3 = new Label();
            this.groupBox1 = new GroupBox();
            this.updPercent = new NumericUpDown();
            this.labMatch = new Label();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.groupBox2 = new GroupBox();
            this.panFunds = new FlowLayoutPanel();
            this.label2 = new Label();
            this.AllocatedAmount = new Label();
            this.groupBox1.SuspendLayout();
            this.updPercent.BeginInit();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label1.Location = new Point(20, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x90, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Percent of Pay to Withhold:";
            this.label3.Location = new Point(20, 0x38);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x88, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Company Match:";
            this.groupBox1.Controls.Add(this.updPercent);
            this.groupBox1.Controls.Add(this.labMatch);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new Point(20, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x10c, 0x5c);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Contribution Rate";
            this.updPercent.Location = new Point(0xac, 0x18);
            this.updPercent.Name = "updPercent";
            this.updPercent.Size = new Size(0x2c, 20);
            this.updPercent.TabIndex = 4;
            this.updPercent.TextAlign = HorizontalAlignment.Right;
            this.updPercent.ValueChanged += new EventHandler(this.updPercent_ValueChanged);
            this.labMatch.Location = new Point(0xa8, 0x38);
            this.labMatch.Name = "labMatch";
            this.labMatch.Size = new Size(40, 0x10);
            this.labMatch.TabIndex = 3;
            this.labMatch.Text = "0%";
            this.labMatch.TextAlign = ContentAlignment.TopRight;
            this.btnHelp.Location = new Point(0xd4, 300);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x48, 0x18);
            this.btnHelp.TabIndex = 0x19;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.Location = new Point(0x7a, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x48, 0x18);
            this.btnCancel.TabIndex = 0x18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnOK.Location = new Point(0x18, 300);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x48, 0x18);
            this.btnOK.TabIndex = 0x17;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.groupBox2.Controls.Add(this.panFunds);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new Point(20, 0x70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x10c, 0x9c);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Investment Allocation";
            this.panFunds.AutoScroll = true;
            this.panFunds.Location = new Point(4, 0x20);
            this.panFunds.Name = "panFunds";
            this.panFunds.Size = new Size(260, 120);
            this.panFunds.TabIndex = 1;
            this.label2.Location = new Point(0x34, 0x10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x90, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Must add to 100 percent.";
            this.label2.TextAlign = ContentAlignment.TopCenter;
            this.AllocatedAmount.Location = new Point(12, 0x10f);
            this.AllocatedAmount.Name = "AllocatedAmount";
            this.AllocatedAmount.Size = new Size(0x90, 20);
            this.AllocatedAmount.TabIndex = 2;
            this.AllocatedAmount.Text = "Allocated amount : 0%";
            this.AllocatedAmount.TextAlign = ContentAlignment.TopCenter;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x134, 0x150);
            base.Controls.Add(this.AllocatedAmount);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.groupBox2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frm401K";
            base.ShowInTaskbar = false;
            this.Text = "401K Retirement Savings Elections";
            this.groupBox1.ResumeLayout(false);
            this.updPercent.EndInit();
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void updPercent_ValueChanged(object sender, EventArgs e)
        {
            this.labMatch.Text = Utilities.FP((float) Math.Min(this.updPercent.Value / 100M, (decimal) this.input.CompanyMatch));
        }

        public void ValueChanged(object sender, EventArgs e)
        {
            decimal total = new decimal();
            this.CountInvestments(out total);
            this.AllocatedAmount.Text = "Allocated amount : " + total.ToString();
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public float[] Allocations;
            public float PercentWitheld;
            public float CompanyMatch;
        }
    }
}

