namespace KMI.Biz
{
    using KMI.Sim;
    using KMI.Utility;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmTransferCash : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private ComboBox cboFrom;
        private ComboBox cboTo;
        private Container components = null;
        protected Input input;
        private Label labCashFrom;
        private Label labCashTo;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private NumericUpDown updAmount;

        public frmTransferCash()
        {
            this.InitializeComponent();
            this.input = ((BizStateAdapter) S.Adapter).getTransferCash(S.Instance.ThisPlayerName);
            foreach (string str in this.input.OwnedEntities)
            {
                this.cboFrom.Items.Add(str);
                this.cboTo.Items.Add(str);
            }
            if (this.input.OwnedEntities.Length > 1)
            {
                this.cboFrom.SelectedIndex = 0;
                this.cboTo.SelectedIndex = Math.Min(1, this.input.OwnedEntities.Length - 1);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string str;
            try
            {
                ((BizStateAdapter) S.Adapter).setTransferCash(S.MainForm.EntityNameToID(this.cboFrom.SelectedItem.ToString()), S.MainForm.EntityNameToID(this.cboTo.SelectedItem.ToString()), (float) this.updAmount.Value);
                base.Close();
            }
            catch (EntityNotFoundException exception)
            {
                str = S.Resources.GetString("No longer exists");
                MessageBox.Show(this, exception.Message, str);
                base.Close();
            }
            catch (SimApplicationException exception2)
            {
                str = S.Resources.GetString("Not enough cash");
                MessageBox.Show(this, exception2.Message, str);
                base.Close();
            }
            catch (Exception exception3)
            {
                frmExceptionHandler.Handle(exception3, this);
            }
        }

        private void cboFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labCashFrom.Text = Utilities.FC(this.input.CashBalances[this.cboFrom.SelectedIndex], S.Instance.CurrencyConversion);
            this.updAmount.Maximum = (decimal) this.input.CashBalances[this.cboFrom.SelectedIndex];
        }

        private void cboTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.labCashTo.Text = Utilities.FC(this.input.CashBalances[this.cboTo.SelectedIndex], S.Instance.CurrencyConversion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmTransferCash_Load(object sender, EventArgs e)
        {
            if (this.input.OwnedEntities.Length <= 1)
            {
                MessageBox.Show("You need to own at least two " + S.Instance.EntityName.ToLower() + "s to transfer cash.", "Transfer Cash");
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.updAmount = new NumericUpDown();
            this.label3 = new Label();
            this.label4 = new Label();
            this.label5 = new Label();
            this.cboFrom = new ComboBox();
            this.cboTo = new ComboBox();
            this.labCashFrom = new Label();
            this.labCashTo = new Label();
            this.updAmount.BeginInit();
            base.SuspendLayout();
            this.btnOK.Location = new Point(0x30, 0x88);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x60, 0x17);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(160, 0x88);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x17);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x110, 0x88);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x17);
            this.btnHelp.TabIndex = 12;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.label1.Location = new Point(0x18, 0x20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x58, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "From:";
            this.label2.Location = new Point(0x18, 80);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x58, 0x10);
            this.label2.TabIndex = 6;
            this.label2.Text = "Available Cash:";
            int[] bits = new int[4];
            bits[0] = 0x3e8;
            this.updAmount.Increment = new decimal(bits);
            this.updAmount.Location = new Point(0x128, 0x30);
            this.updAmount.Name = "updAmount";
            this.updAmount.Size = new Size(0x60, 20);
            this.updAmount.TabIndex = 5;
            this.updAmount.TextAlign = HorizontalAlignment.Right;
            this.label3.Location = new Point(0x128, 0x20);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x58, 0x10);
            this.label3.TabIndex = 2;
            this.label3.Text = "Amount:";
            this.label4.Location = new Point(160, 0x20);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x58, 0x10);
            this.label4.TabIndex = 1;
            this.label4.Text = "To:";
            this.label5.Location = new Point(160, 80);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x58, 0x10);
            this.label5.TabIndex = 7;
            this.label5.Text = "Available Cash:";
            this.cboFrom.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboFrom.Location = new Point(0x18, 0x30);
            this.cboFrom.Name = "cboFrom";
            this.cboFrom.Size = new Size(0x70, 0x15);
            this.cboFrom.TabIndex = 3;
            this.cboFrom.SelectedIndexChanged += new EventHandler(this.cboFrom_SelectedIndexChanged);
            this.cboTo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboTo.Location = new Point(160, 0x30);
            this.cboTo.Name = "cboTo";
            this.cboTo.Size = new Size(0x70, 0x15);
            this.cboTo.TabIndex = 4;
            this.cboTo.SelectedIndexChanged += new EventHandler(this.cboTo_SelectedIndexChanged);
            this.labCashFrom.Location = new Point(0x18, 0x60);
            this.labCashFrom.Name = "labCashFrom";
            this.labCashFrom.Size = new Size(120, 0x10);
            this.labCashFrom.TabIndex = 8;
            this.labCashFrom.Text = "#";
            this.labCashTo.Location = new Point(160, 0x60);
            this.labCashTo.Name = "labCashTo";
            this.labCashTo.Size = new Size(120, 0x10);
            this.labCashTo.TabIndex = 9;
            this.labCashTo.Text = "#";
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x1a2, 0xb8);
            base.Controls.Add(this.labCashTo);
            base.Controls.Add(this.labCashFrom);
            base.Controls.Add(this.cboTo);
            base.Controls.Add(this.cboFrom);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.updAmount);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmTransferCash";
            base.ShowInTaskbar = false;
            this.Text = "Transfer Cash";
            base.Load += new EventHandler(this.frmTransferCash_Load);
            this.updAmount.EndInit();
            base.ResumeLayout(false);
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public string[] OwnedEntities;
            public float[] CashBalances;
        }
    }
}

