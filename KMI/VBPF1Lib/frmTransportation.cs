using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmTransportation : Form {

        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private Container components = null;
        private GroupBox grpMain;
        private Label label1;
        private Label label2;
        private Label label3;
        private Panel panMain;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;

        public frmTransportation() {
            this.InitializeComponent();
            int transportation = A.Adapter.GetTransportation(A.MainForm.CurrentEntityID);
            if (transportation > -1) {
                ((RadioButton) this.panMain.Controls[transportation]).Checked = true;
                this.btnCancel.Enabled = true;
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int index = -1;
                for (int i = 0; i < this.panMain.Controls.Count; i++)
                {
                    if (((RadioButton) this.panMain.Controls[i]).Checked)
                    {
                        index = i;
                    }
                }
                if (index == -1)
                {
                    MessageBox.Show("You must select a mode of transportation.", "Input Required");
                }
                else
                {
                    A.Adapter.SetTransportation(A.MainForm.CurrentEntityID, index);
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void frmTransportation_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = !this.btnOK.Enabled;
        }

        private void InitializeComponent()
        {
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.btnOK = new Button();
            this.radioButton1 = new RadioButton();
            this.grpMain = new GroupBox();
            this.panMain = new Panel();
            this.radioButton2 = new RadioButton();
            this.radioButton3 = new RadioButton();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.grpMain.SuspendLayout();
            this.panMain.SuspendLayout();
            base.SuspendLayout();
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new Point(0x98, 0xf4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(70, 0x18);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.button3_Click);
            this.btnHelp.Location = new Point(0xf4, 0xf4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x44, 0x18);
            this.btnHelp.TabIndex = 10;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnOK.Enabled = false;
            this.btnOK.Location = new Point(0x40, 0xf4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x44, 0x18);
            this.btnOK.TabIndex = 9;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.radioButton1.Location = new Point(8, 4);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x30, 0x1c);
            this.radioButton1.TabIndex = 14;
            this.radioButton1.Text = "Foot";
            this.radioButton1.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.grpMain.Controls.Add(this.panMain);
            this.grpMain.Controls.Add(this.label3);
            this.grpMain.Controls.Add(this.label2);
            this.grpMain.Controls.Add(this.label1);
            this.grpMain.Location = new Point(0x10, 0x10);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new Size(0x15c, 0xd0);
            this.grpMain.TabIndex = 13;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "Travel By";
            this.panMain.Controls.Add(this.radioButton1);
            this.panMain.Controls.Add(this.radioButton2);
            this.panMain.Controls.Add(this.radioButton3);
            this.panMain.Location = new Point(0x18, 0x10);
            this.panMain.Name = "panMain";
            this.panMain.Size = new Size(0x40, 0xa4);
            this.panMain.TabIndex = 0x12;
            this.radioButton2.Location = new Point(8, 60);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x30, 0x1c);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.Text = "Bus";
            this.radioButton2.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.radioButton3.Location = new Point(8, 0x74);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x30, 0x1c);
            this.radioButton3.TabIndex = 12;
            this.radioButton3.Text = "Car";
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton1_CheckedChanged);
            this.label3.Location = new Point(0x5c, 0x7c);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0xec, 0x34);
            this.label3.TabIndex = 0x11;
            this.label3.Text = "You must lease or buy a car. Other expenses include insurance, gas, and repairs. If your car is out of gas or broken down, you will automatically walk instead.";
            this.label3.TextAlign = ContentAlignment.MiddleLeft;
            this.label2.Location = new Point(0x5c, 0x44);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0xec, 0x2c);
            this.label2.TabIndex = 0x10;
            this.label2.Text = "Bus tokens are $1 per ride and can be purchased at any bus stop. If walking is faster, you will automatically walk instead.";
            this.label2.TextAlign = ContentAlignment.MiddleLeft;
            this.label1.Location = new Point(0x5c, 0x10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0xec, 0x24);
            this.label1.TabIndex = 15;
            this.label1.Text = "This option is free.";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(380, 0x11a);
            base.Controls.Add(this.grpMain);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnOK);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmTransportation";
            base.ShowInTaskbar = false;
            this.Text = "Transportation";
            base.Closing += new CancelEventHandler(this.frmTransportation_Closing);
            this.grpMain.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.btnOK.Enabled = true;
        }
    }
}

