namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmAppProvideCash : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private ComboBox cboNames;
        private Container components = null;
        private Label labDescription;
        private Label label1;
        private Label labStore;
        private Hashtable NamesAndIds;
        private NumericUpDown updCash;

        public frmAppProvideCash()
        {
            this.InitializeComponent();
            this.NamesAndIds = A.Adapter.GetNamesAndIds();
            foreach (string str in this.NamesAndIds.Keys)
            {
                this.cboNames.Items.Add(str);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            long entityID = (long) this.NamesAndIds[this.cboNames.SelectedItem.ToString()];
            try
            {
                A.Adapter.ProvideCash(entityID, (float) this.updCash.Value);
                A.MainForm.UpdateView();
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

        private void frmAppProvideCash_Load(object sender, EventArgs e)
        {
            if (this.cboNames.Items.Count > 0)
            {
                this.cboNames.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("There aren't any people in this sim yet. Rent an apartment to create a person.", "Provide Cash");
                base.Close();
            }
        }

        private void InitializeComponent()
        {
            this.labDescription = new Label();
            this.labStore = new Label();
            this.updCash = new NumericUpDown();
            this.btnCancel = new Button();
            this.btnOk = new Button();
            this.label1 = new Label();
            this.cboNames = new ComboBox();
            this.updCash.BeginInit();
            base.SuspendLayout();
            this.labDescription.Location = new Point(0x10, 11);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new Size(0x130, 0x35);
            this.labDescription.TabIndex = 5;
            this.labDescription.Text = "This feature lets you \"bail out\" a person by providing cash.   This feature can be used by Instructors to keep students involved and encouraged.";
            this.labStore.Location = new Point(0x10, 0x48);
            this.labStore.Name = "labStore";
            this.labStore.Size = new Size(0xb8, 0x10);
            this.labStore.TabIndex = 6;
            this.labStore.Text = "Amount to give:";
            this.labStore.TextAlign = ContentAlignment.TopRight;
            int[] bits = new int[4];
            bits[0] = 0x2710;
            this.updCash.Increment = new decimal(bits);
            this.updCash.Location = new Point(200, 0x48);
            bits = new int[4];
            bits[0] = 0x5f5e100;
            this.updCash.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 0x5f5e100;
            bits[3] = -2147483648;
            this.updCash.Minimum = new decimal(bits);
            this.updCash.Name = "updCash";
            this.updCash.Size = new Size(0x58, 20);
            this.updCash.TabIndex = 7;
            this.updCash.TextAlign = HorizontalAlignment.Right;
            this.updCash.ThousandsSeparator = true;
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xb8, 0x93);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnOk.Location = new Point(0x40, 0x93);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(0x60, 0x18);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.label1.Location = new Point(0x20, 0x68);
            this.label1.Name = "label1";
            this.label1.Size = new Size(120, 0x10);
            this.label1.TabIndex = 10;
            this.label1.Text = "Person to recieve:";
            this.label1.TextAlign = ContentAlignment.TopRight;
            this.cboNames.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboNames.Location = new Point(0x98, 0x68);
            this.cboNames.Name = "cboNames";
            this.cboNames.Size = new Size(0x88, 0x15);
            this.cboNames.TabIndex = 11;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x150, 190);
            base.Controls.Add(this.cboNames);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.labDescription);
            base.Controls.Add(this.labStore);
            base.Controls.Add(this.updCash);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOk);
            base.Name = "frmAppProvideCash";
            this.Text = "Provide Cash";
            base.Load += new EventHandler(this.frmAppProvideCash_Load);
            this.updCash.EndInit();
            base.ResumeLayout(false);
        }
    }
}

