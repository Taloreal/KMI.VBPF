namespace KMI.VBPF1Lib
{
    using KMI.Utility;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class frmJobApplication : Form
    {
        private Button btnCancel;
        private Button btnHelp;
        private Button btnOK;
        private CheckBox chkCar;
        private ComboBox comboBox1;
        private ComboBox comboBox10;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private ComboBox comboBox7;
        private ComboBox comboBox8;
        private ComboBox comboBox9;
        private Container components = null;
        public JobApplication JobApp = new JobApplication();
        private Label label1;
        private Label label14;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private Panel panEducation;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panWork;
        private Panel panWorkMonths;
        private TextBox txtName;

        public frmJobApplication()
        {
            this.InitializeComponent();
            ArrayList offerings = A.Adapter.GetOfferings();
            foreach (Offering offering in offerings)
            {
                if (offering is Course)
                {
                    foreach (ComboBox box in this.panEducation.Controls)
                    {
                        if (box.FindStringExact(offering.ToString()) < 0)
                        {
                            box.Items.Add(offering);
                        }
                    }
                }
                if (offering is Job)
                {
                    foreach (ComboBox box2 in this.panWork.Controls)
                    {
                        if (box2.FindStringExact(offering.ToString()) < 0)
                        {
                            box2.Items.Add(offering);
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(A.Resources.GetString("Applying For A Job"));
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.JobApp.Name = this.txtName.Text;
            foreach (ComboBox box in this.panEducation.Controls)
            {
                if ((box.SelectedIndex > -1) && !this.JobApp.ReportedClassNames.Contains(box.SelectedItem.ToString()))
                {
                    this.JobApp.ReportedClassNames.Add(box.SelectedItem.ToString());
                }
            }
            int num = 0;
            foreach (ComboBox box2 in this.panWork.Controls)
            {
                if ((box2.SelectedIndex > -1) && !this.JobApp.ReportedJobNamesAndMonths.ContainsKey(box2.SelectedItem.ToString()))
                {
                    this.JobApp.ReportedJobNamesAndMonths.Add(box2.SelectedItem.ToString(), (int) ((NumericUpDown) this.panWorkMonths.Controls[num]).Value);
                }
                num++;
            }
            this.JobApp.Car = this.chkCar.Checked;
            base.DialogResult = DialogResult.OK;
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

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.panel3 = new Panel();
            this.label14 = new Label();
            this.panWorkMonths = new Panel();
            this.numericUpDown4 = new NumericUpDown();
            this.numericUpDown3 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.numericUpDown1 = new NumericUpDown();
            this.label9 = new Label();
            this.label8 = new Label();
            this.panWork = new Panel();
            this.comboBox7 = new ComboBox();
            this.comboBox8 = new ComboBox();
            this.comboBox9 = new ComboBox();
            this.comboBox10 = new ComboBox();
            this.panel2 = new Panel();
            this.panEducation = new Panel();
            this.comboBox4 = new ComboBox();
            this.comboBox3 = new ComboBox();
            this.comboBox2 = new ComboBox();
            this.comboBox1 = new ComboBox();
            this.label7 = new Label();
            this.label6 = new Label();
            this.label5 = new Label();
            this.label4 = new Label();
            this.label3 = new Label();
            this.chkCar = new CheckBox();
            this.txtName = new TextBox();
            this.label2 = new Label();
            this.label1 = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnHelp = new Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panWorkMonths.SuspendLayout();
            this.numericUpDown4.BeginInit();
            this.numericUpDown3.BeginInit();
            this.numericUpDown2.BeginInit();
            this.numericUpDown1.BeginInit();
            this.panWork.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panEducation.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = Color.White;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.chkCar);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x178, 0x19c);
            this.panel1.TabIndex = 0;
            this.panel3.BorderStyle = BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.panWorkMonths);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.panWork);
            this.panel3.Location = new Point(0x10, 0xe0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(0x158, 0x9c);
            this.panel3.TabIndex = 6;
            this.label14.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label14.Location = new Point(8, 4);
            this.label14.Name = "label14";
            this.label14.Size = new Size(0x4c, 0x10);
            this.label14.TabIndex = 0;
            this.label14.Text = "Work History";
            this.panWorkMonths.Controls.Add(this.numericUpDown4);
            this.panWorkMonths.Controls.Add(this.numericUpDown3);
            this.panWorkMonths.Controls.Add(this.numericUpDown2);
            this.panWorkMonths.Controls.Add(this.numericUpDown1);
            this.panWorkMonths.Location = new Point(0x110, 0x20);
            this.panWorkMonths.Name = "panWorkMonths";
            this.panWorkMonths.Size = new Size(0x40, 0x74);
            this.panWorkMonths.TabIndex = 9;
            this.numericUpDown4.Location = new Point(8, 0x58);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new Size(0x2c, 20);
            this.numericUpDown4.TabIndex = 3;
            this.numericUpDown4.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown3.Location = new Point(8, 60);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x2c, 20);
            this.numericUpDown3.TabIndex = 2;
            this.numericUpDown3.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown2.Location = new Point(8, 0x20);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x2c, 20);
            this.numericUpDown2.TabIndex = 1;
            this.numericUpDown2.TextAlign = HorizontalAlignment.Right;
            this.numericUpDown1.Location = new Point(8, 4);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x2c, 20);
            this.numericUpDown1.TabIndex = 0;
            this.numericUpDown1.TextAlign = HorizontalAlignment.Right;
            this.label9.Location = new Point(0x114, 0);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x34, 0x20);
            this.label9.TabIndex = 8;
            this.label9.Text = "Months at Job";
            this.label9.TextAlign = ContentAlignment.TopCenter;
            this.label8.Location = new Point(0x68, 12);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x44, 0x10);
            this.label8.TabIndex = 7;
            this.label8.Text = "Job Title";
            this.label8.TextAlign = ContentAlignment.TopCenter;
            this.panWork.Controls.Add(this.comboBox7);
            this.panWork.Controls.Add(this.comboBox8);
            this.panWork.Controls.Add(this.comboBox9);
            this.panWork.Controls.Add(this.comboBox10);
            this.panWork.Location = new Point(0x10, 0x20);
            this.panWork.Name = "panWork";
            this.panWork.Size = new Size(240, 0x70);
            this.panWork.TabIndex = 6;
            this.comboBox7.Location = new Point(4, 0x54);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new Size(0xe8, 0x15);
            this.comboBox7.TabIndex = 7;
            this.comboBox8.Location = new Point(4, 60);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new Size(0xe8, 0x15);
            this.comboBox8.TabIndex = 6;
            this.comboBox9.Location = new Point(4, 0x20);
            this.comboBox9.Name = "comboBox9";
            this.comboBox9.Size = new Size(0xe8, 0x15);
            this.comboBox9.TabIndex = 5;
            this.comboBox10.Location = new Point(4, 4);
            this.comboBox10.Name = "comboBox10";
            this.comboBox10.Size = new Size(0xe8, 0x15);
            this.comboBox10.TabIndex = 4;
            this.panel2.BorderStyle = BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.panEducation);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new Point(0x10, 0x44);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(0x158, 0x90);
            this.panel2.TabIndex = 5;
            this.panEducation.Controls.Add(this.comboBox4);
            this.panEducation.Controls.Add(this.comboBox3);
            this.panEducation.Controls.Add(this.comboBox2);
            this.panEducation.Controls.Add(this.comboBox1);
            this.panEducation.Location = new Point(0x40, 4);
            this.panEducation.Name = "panEducation";
            this.panEducation.Size = new Size(0x114, 0x84);
            this.panEducation.TabIndex = 6;
            this.comboBox4.Location = new Point(8, 0x68);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new Size(0x100, 0x15);
            this.comboBox4.TabIndex = 7;
            this.comboBox3.Location = new Point(8, 0x4c);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new Size(0x100, 0x15);
            this.comboBox3.TabIndex = 6;
            this.comboBox2.Location = new Point(8, 0x30);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new Size(0x100, 0x15);
            this.comboBox2.TabIndex = 5;
            this.comboBox1.Location = new Point(8, 20);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new Size(0x100, 0x15);
            this.comboBox1.TabIndex = 4;
            this.label7.Location = new Point(0x10, 0x70);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x38, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "Course 4:";
            this.label6.Location = new Point(0x10, 0x54);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x38, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "Course 3:";
            this.label5.Location = new Point(0x10, 0x38);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x38, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Course 2:";
            this.label4.Location = new Point(0x10, 0x1c);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x38, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Course 1:";
            this.label3.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label3.Location = new Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x84, 0x10);
            this.label3.TabIndex = 0;
            this.label3.Text = "Education";
            this.chkCar.Location = new Point(0x38, 0x184);
            this.chkCar.Name = "chkCar";
            this.chkCar.Size = new Size(0x110, 0x10);
            this.chkCar.TabIndex = 3;
            this.chkCar.Text = "Check if you have access to an automobile";
            this.txtName.Location = new Point(0x4c, 0x24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new Size(180, 20);
            this.txtName.TabIndex = 2;
            this.txtName.Text = "";
            this.label2.Font = new Font("Microsoft Sans Serif", 8f);
            this.label2.Location = new Point(0x1c, 40);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x24, 0x10);
            this.label2.TabIndex = 1;
            this.label2.Text = "Name:";
            this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label1.Location = new Point(0x70, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x98, 0x10);
            this.label1.TabIndex = 0;
            this.label1.Text = "Application for Employment";
            this.btnOK.Location = new Point(0x2c, 0x1bc);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x5c, 0x18);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new Point(0xa4, 0x1bc);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(0x5c, 0x18);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnHelp.Location = new Point(0x11c, 0x1bc);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x5c, 0x18);
            this.btnHelp.TabIndex = 3;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            base.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.CancelButton = this.btnCancel;
            base.ClientSize = new Size(400, 0x1e2);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.btnCancel);
            base.Controls.Add(this.btnOK);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "frmJobApplication";
            base.ShowInTaskbar = false;
            this.Text = "Apply for Job";
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panWorkMonths.ResumeLayout(false);
            this.numericUpDown4.EndInit();
            this.numericUpDown3.EndInit();
            this.numericUpDown2.EndInit();
            this.numericUpDown1.EndInit();
            this.panWork.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panEducation.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

