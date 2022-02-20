namespace KMI.Sim
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmProvideCash : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private Container components = null;
        private Label labDescription;
        private Label labStore;
        private NumericUpDown updCash;

        public frmProvideCash()
        {
            this.InitializeComponent();
            this.labDescription.Text = this.labDescription.Text.Replace("XXX", S.Instance.EntityName.ToLower());
            this.labStore.Text = this.labStore.Text.Replace("XXX", S.MainForm.EntityIDToName(S.MainForm.CurrentEntityID));
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                S.Adapter.ProvideCash(S.MainForm.CurrentEntityID, (float) this.updCash.Value);
                S.MainForm.UpdateView();
                base.Close();
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
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
            this.btnOk = new Button();
            this.btnCancel = new Button();
            this.updCash = new NumericUpDown();
            this.labStore = new Label();
            this.labDescription = new Label();
            this.updCash.BeginInit();
            base.SuspendLayout();
            this.btnOk.Location = new Point(0x48, 0x90);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x60, 0x18);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xc0, 0x90);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.updCash.Increment = new decimal(bits);
            this.updCash.Location = new Point(0xd0, 0x58);
            bits = new int[4];
            bits[0] = 0x5f5e100;
            this.updCash.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x5f5e100;
            bits[3] = -2147483648;
            this.updCash.Minimum = new decimal(bits);
            this.updCash.Name = "updCash";
            this.updCash.Size = new Size(0x58, 20);
            this.updCash.TabIndex = 2;
            this.updCash.TextAlign = HorizontalAlignment.Right;
            this.updCash.ThousandsSeparator = true;
            this.labStore.Location = new Point(0x18, 0x58);
            this.labStore.Name = "labStore";
            this.labStore.Size = new Size(0xb8, 0x10);
            this.labStore.TabIndex = 1;
            this.labStore.Text = "Amount to give to XXX:";
            this.labStore.TextAlign = ContentAlignment.TopRight;
            this.labDescription.Location = new Point(0x18, 8);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new Size(0x130, 0x40);
            this.labDescription.TabIndex = 0;
            this.labDescription.Text = "This feature lets you \"bail out\" the current XXX by providing cash.  The students' equity is not reduced.  This feature can be used by Instructors to keep students involved and encouraged.";
            base.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(0x160, 190);
            base.Controls.Add(this.labDescription);
            base.Controls.Add(this.labStore);
            base.Controls.Add(this.updCash);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmProvideCash";
            base.ShowInTaskbar = false;
            this.Text = "Provide Cash";
            this.updCash.EndInit();
            base.ResumeLayout(false);
        }
    }
}

