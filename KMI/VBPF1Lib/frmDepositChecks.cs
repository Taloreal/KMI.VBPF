using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib
{

    public class frmDepositChecks : Form
    {
        private SortedList accounts;
        private Button btnAccept;
        private Button btnBack;
        private Button btnCancel;
        private Button btnHelp;
        private Button btnNext;
        private ComboBox cboAccounts;
        private SortedList checks;
        private CheckControl chkCheck;
        private Container components = null;
        private int currentIndex;
        private CheckBox DoToAll;
        private GroupBox groupBox1;
        private bool hasCheckingAccount;
        private Label labNoOffers;
        private Label labWarning;
        private RadioButton optCash;
        private RadioButton optDeposit;
        private Panel panChecks;

        public frmDepositChecks()
        {
            this.InitializeComponent();
            this.chkCheck = new CheckControl();
            this.chkCheck.Location = new Point(0x10, 0x18);
            this.panChecks.Controls.Add(this.chkCheck);
            for (int i = 0; i < 4; i++)
            {
                Label label = new Label {
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = this.chkCheck.BackColor,
                    Size = this.chkCheck.Size,
                    Location = new Point(20 + (i * 4), 20 - (i * 4))
                };
                this.panChecks.Controls.Add(label);
            }
            this.accounts = A.Adapter.GetBankAccounts(A.MainForm.CurrentEntityID);
            foreach (BankAccount account in this.accounts.Values)
            {
                this.cboAccounts.Items.Add(account);
                if (account is CheckingAccount)
                {
                    this.hasCheckingAccount = true;
                }
            }
            if (this.cboAccounts.Items.Count > 0)
            {
                this.optDeposit.Enabled = true;
                this.cboAccounts.SelectedIndex = 0;
            }
            this.labWarning.Visible = !this.hasCheckingAccount;
            this.RefreshData();
            this.currentIndex = 0;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.optCash.Checked)
                {
                    float fee = 0f;
                    if (!this.hasCheckingAccount)
                    {
                        fee = 0.1f;
                    }
                    if (this.DoToAll.Checked)
                    {
                        this.cashAll(fee);
                    }
                    else
                    {
                        A.Adapter.CashCheck(A.MainForm.CurrentEntityID, this.currentCheck, fee);
                    }
                }
                else if (this.DoToAll.Checked)
                {
                    this.depositAll();
                }
                else
                {
                    A.Adapter.DepositCheck(A.MainForm.CurrentEntityID, this.currentCheck, ((BankAccount) this.cboAccounts.SelectedItem).AccountNumber);
                }
                A.MainForm.UpdateView();
                this.RefreshData();
                if (this.checks.Count == 0)
                {
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.currentIndex--;
            this.SwitchCheck();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Deposit Funds"));
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.currentIndex++;
            this.SwitchCheck();
        }

        public void cashAll(float fee)
        {
            while (this.checks.Count > 0)
            {
                A.Adapter.CashCheck(A.MainForm.CurrentEntityID, (KMI.VBPF1Lib.Check) this.checks.GetByIndex(0), fee);
            }
        }

        public void depositAll()
        {
            while (this.checks.Count > 0)
            {
                A.Adapter.DepositCheck(A.MainForm.CurrentEntityID, (KMI.VBPF1Lib.Check) this.checks.GetByIndex(0), ((BankAccount) this.cboAccounts.SelectedItem).AccountNumber);
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
            this.btnAccept = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.btnBack = new Button();
            this.btnNext = new Button();
            this.labNoOffers = new Label();
            this.panChecks = new Panel();
            this.groupBox1 = new GroupBox();
            this.DoToAll = new CheckBox();
            this.labWarning = new Label();
            this.cboAccounts = new ComboBox();
            this.optDeposit = new RadioButton();
            this.optCash = new RadioButton();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.btnAccept.Location = new Point(0x1f8, 0x100);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new Size(0x60, 0x18);
            this.btnAccept.TabIndex = 0;
            this.btnAccept.Text = "Go";
            this.btnAccept.Click += new EventHandler(this.btnAccept_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0x1f8, 0x120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x1f8, 320);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnBack.Location = new Point(0x18, 280);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(0x30, 0x18);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "<<";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnNext.Location = new Point(0x58, 280);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(0x30, 0x18);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = ">>";
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.labNoOffers.Font = new Font("Microsoft Sans Serif", 21.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labNoOffers.ForeColor = Color.Gray;
            this.labNoOffers.Location = new Point(0xb0, 0x40);
            this.labNoOffers.Name = "labNoOffers";
            this.labNoOffers.Size = new Size(0x108, 120);
            this.labNoOffers.TabIndex = 7;
            this.labNoOffers.Text = "There are no more checks.";
            this.panChecks.Location = new Point(0, 8);
            this.panChecks.Name = "panChecks";
            this.panChecks.Size = new Size(600, 0xe8);
            this.panChecks.TabIndex = 9;
            this.groupBox1.Controls.Add(this.DoToAll);
            this.groupBox1.Controls.Add(this.labWarning);
            this.groupBox1.Controls.Add(this.cboAccounts);
            this.groupBox1.Controls.Add(this.optDeposit);
            this.groupBox1.Controls.Add(this.optCash);
            this.groupBox1.Location = new Point(160, 0xf8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(320, 0x60);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cash or Deposit";
            this.DoToAll.AutoSize = true;
            this.DoToAll.Location = new Point(6, 0x48);
            this.DoToAll.Name = "DoToAll";
            this.DoToAll.Size = new Size(0x4d, 0x11);
            this.DoToAll.TabIndex = 5;
            this.DoToAll.Text = "Apply to all";
            this.DoToAll.UseVisualStyleBackColor = true;
            this.labWarning.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labWarning.ForeColor = Color.Red;
            this.labWarning.Location = new Point(0x70, 0x12);
            this.labWarning.Name = "labWarning";
            this.labWarning.Size = new Size(200, 0x18);
            this.labWarning.TabIndex = 4;
            this.labWarning.Text = "Since you do not have a checking account in town, a 10% check cashing fee will be charged.";
            this.cboAccounts.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboAccounts.Location = new Point(90, 0x34);
            this.cboAccounts.Name = "cboAccounts";
            this.cboAccounts.Size = new Size(0xd8, 0x15);
            this.cboAccounts.TabIndex = 1;
            this.optDeposit.Enabled = false;
            this.optDeposit.Location = new Point(4, 0x2d);
            this.optDeposit.Name = "optDeposit";
            this.optDeposit.Size = new Size(80, 0x18);
            this.optDeposit.TabIndex = 3;
            this.optDeposit.Text = "Deposit to";
            this.optDeposit.CheckedChanged += new EventHandler(this.optDeposit_CheckedChanged);
            this.optCash.Checked = true;
            this.optCash.Location = new Point(6, 30);
            this.optCash.Name = "optCash";
            this.optCash.Size = new Size(0x60, 0x10);
            this.optCash.TabIndex = 2;
            this.optCash.TabStop = true;
            this.optCash.Text = "Cash Check";
            this.optCash.CheckedChanged += new EventHandler(this.optCash_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(610, 0x160);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.panChecks);
            base.Controls.Add(this.btnNext);
            base.Controls.Add(this.btnBack);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnAccept);
            base.Controls.Add(this.labNoOffers);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmDepositChecks";
            base.ShowInTaskbar = false;
            this.Text = "Cash or Deposit Checks";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            base.ResumeLayout(false);
        }

        private void optCash_CheckedChanged(object sender, EventArgs e)
        {
            if (this.optCash.Checked)
            {
                this.DoToAll.Text = "Cash all";
            }
        }

        private void optDeposit_CheckedChanged(object sender, EventArgs e)
        {
            this.cboAccounts.Enabled = this.optDeposit.Checked;
            if (this.optDeposit.Checked)
            {
                this.DoToAll.Text = "Deposit all";
            }
        }

        protected void RefreshData()
        {
            this.checks = A.Adapter.GetChecks(A.MainForm.CurrentEntityID);
            this.currentIndex = Math.Min(this.currentIndex, this.checks.Count - 1);
            this.panChecks.Visible = this.checks.Count > 0;
            this.btnAccept.Enabled = this.checks.Count > 0;
            for (int i = 0; i < this.panChecks.Controls.Count; i++)
            {
                this.panChecks.Controls[i].Visible = i < this.checks.Count;
            }
            if (this.checks.Count > 0)
            {
                this.SwitchCheck();
            }
        }

        private void SwitchCheck()
        {
            this.chkCheck.Check = this.currentCheck;
            this.btnBack.Enabled = this.currentIndex > 0;
            this.btnNext.Enabled = this.currentIndex < (this.checks.Count - 1);
        }

        public KMI.VBPF1Lib.Check currentCheck
        {
            get
            {
                return (KMI.VBPF1Lib.Check) this.checks.GetByIndex(this.currentIndex);
            }
        }
    }
}

