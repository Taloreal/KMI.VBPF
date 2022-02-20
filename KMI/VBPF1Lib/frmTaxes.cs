using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Printing;

using KMI.Sim;
using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmTaxes : Form, IConstrainedForm {

        private ComboBox cboOldReturns;
        private Container components = null;
        private BankAccount fedTaxAccount;
        private Button btnClose, btnFile;
        private Button btnHelp, btnPrint;
        private TextBox t0, t1, t2, t3, t4;
        private TextBox t5, t6, t7, t8;
        private Label Instructions;
        private Label l0, l1, l2;
        private Label l3, l4, l5;
        private Label TaxYear, Year;
        private Label ss1, ss2;
        private int lastYear;
        public Mode mode;
        private Panel panel1, panMain, panReport;
        private string studentName;
        private ArrayList taxReturns;
        private LinkLabel TaxTable;

        protected TaxReturn currentReturn;

        public frmTaxes(Mode mode) {
            this.InitializeComponent();
            this.mode = mode;
            this.fedTaxAccount = A.Adapter.GetFedTaxAccount(A.MainForm.CurrentEntityID);
            if (mode == Mode.Past)
            {
                this.lastYear = A.Adapter.GetLastYear();
                this.taxReturns = A.Adapter.GetTaxes(A.MainForm.CurrentEntityID);
                if (this.taxReturns.Count == 0)
                {
                    throw new SimApplicationException("You have no past tax returns at this time.");
                }
                this.taxReturns.Reverse();
                foreach (TaxReturn return2 in this.taxReturns)
                {
                    this.cboOldReturns.Items.Add(return2);
                }
                this.cboOldReturns.SelectedIndex = 0;
                this.btnFile.Visible = false;
                this.Instructions.Visible = false;
            }
            if (mode == Mode.Current)
            {
                this.lastYear = A.Adapter.TaxYearDue(A.MainForm.CurrentEntityID);
                if (this.lastYear != -1)
                {
                    if (!A.Adapter.UseAccountant())
                    {
                        this.CurrentReturn = A.Adapter.GetNewF1040EZ(A.MainForm.CurrentEntityID, this.lastYear);
                    }
                    else
                    {
                        this.CurrentReturn = A.Adapter.GetNewAccountantsReport(A.MainForm.CurrentEntityID, this.lastYear);
                        this.Instructions.Visible = false;
                        MessageBox.Show("Since you are past Level 1 or in an investing lesson or special competition, your tax return has been prepared for you by a tax professional. When you click File Tax Return, the tax return prepared by the professional will be sent to the IRS.", "Tax Return");
                    }
                    this.TaxYear.Visible = false;
                    this.cboOldReturns.Visible = false;
                }
            }
            this.panMain.BackgroundImage = A.Resources.GetImage("1040EZ");
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (this.currentReturn is F1040EZ)
            {
                foreach (Control control in this.panMain.Controls)
                {
                    for (int i = 0; i < this.panMain.Controls.Count; i++)
                    {
                        if (control.Name == ("t" + i))
                        {
                            try
                            {
                                if (control.Text.Length == 0)
                                {
                                    this.currentReturn.Values[i] = 0;
                                }
                                else
                                {
                                    this.currentReturn.Values[i] = int.Parse(control.Text);
                                }
                            }
                            catch
                            {
                                MessageBox.Show(A.Resources.GetString("Incorrect entry. Please retry. Remember to use the whole dollar method."), A.Resources.GetString("Please Retry"));
                                ((TextBox) control).SelectAll();
                                control.Focus();
                                return;
                            }
                        }
                    }
                }
                TextBox[] boxArray1 = new TextBox[] { this.t0, this.t2, this.t4, this.t6 };
                foreach (TextBox box in boxArray1)
                {
                    if (box.Text.Length == 0)
                    {
                        MessageBox.Show(A.Resources.GetString("Information is required on lines 1, 4, 6 and 11 of Form 1040EZ. If the correct amount is zero, enter the numeral 0."), A.Resources.GetString("More Information Required"));
                        return;
                    }
                }
                if ((this.t7.Text.Length == 0) && (this.t8.Text.Length == 0))
                {
                    MessageBox.Show(A.Resources.GetString("You must make an entry on line 12a or line 13 of Form 1040EZ. If the correct amount is zero, enter the numeral 0."), A.Resources.GetString("More Information Required"));
                    return;
                }
            }
            try
            {
                float num3 = this.currentReturn.Values[7];
                float amount = this.currentReturn.Values[8];
                if ((num3 != 0f) && !(amount == 0f))
                {
                    MessageBox.Show(A.Resources.GetString("You cannot have a refund and payment due. Please recheck your calculations."), A.Resources.GetString("Please Retry"));
                }
                else if ((num3 < 0f) || (amount < 0f))
                {
                    MessageBox.Show(A.Resources.GetString("Your refund or payment must be greater than or equal to 0. Please recheck your calculations."), A.Resources.GetString("Please Retry"));
                }
                else
                {
                    if (amount > 0f)
                    {
                        frmPayBy by = new frmPayBy(A.Adapter.CreateBill(A.Resources.GetString("IRS"), "Income Taxes for " + this.lastYear, amount, this.fedTaxAccount));
                        if (by.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }
                    }
                    A.Adapter.SetTaxes(A.MainForm.CurrentEntityID, this.currentReturn);
                    MessageBox.Show(A.Resources.GetString("Your return has been filed!"), A.Resources.GetString("Success"));
                    base.Close();
                }
            }
            catch (Exception exception)
            {
                frmExceptionHandler.Handle(exception, this);
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            KMIHelp.OpenHelp(this.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.studentName = "";
            frmInputString str = new frmInputString(S.Resources.GetString("Student Name"), S.Resources.GetString("Enter your name to help identify your printout on a shared printer:"), this.studentName);
            str.ShowDialog(this);
            this.studentName = str.Response;
            Utilities.PrintWithExceptionHandling(this.Text, new PrintPageEventHandler(this.PrintPage));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        public string CanUse()
        {
            if ((this.mode == Mode.Past) && (this.taxReturns.Count == 0))
            {
                return A.Resources.GetString("You have no past tax returns.");
            }
            if ((this.mode == Mode.Current) && (A.Adapter.TaxYearDue(A.MainForm.CurrentEntityID) == -1))
            {
                return A.Resources.GetString("You do not have any tax returns that can be filed at this time.");
            }
            return "";
        }

        private void cboOldReturns_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentReturn = (TaxReturn) this.cboOldReturns.SelectedItem;
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
            this.panMain = new Panel();
            this.l1 = new Label();
            this.ss2 = new Label();
            this.ss1 = new Label();
            this.TaxTable = new LinkLabel();
            this.l5 = new Label();
            this.l4 = new Label();
            this.t8 = new TextBox();
            this.t7 = new TextBox();
            this.t6 = new TextBox();
            this.t5 = new TextBox();
            this.t3 = new TextBox();
            this.t4 = new TextBox();
            this.t2 = new TextBox();
            this.l3 = new Label();
            this.l2 = new Label();
            this.l0 = new Label();
            this.Year = new Label();
            this.t1 = new TextBox();
            this.t0 = new TextBox();
            this.panReport = new Panel();
            this.btnFile = new Button();
            this.cboOldReturns = new ComboBox();
            this.btnClose = new Button();
            this.TaxYear = new Label();
            this.Instructions = new Label();
            this.btnHelp = new Button();
            this.btnPrint = new Button();
            this.panel1.SuspendLayout();
            this.panMain.SuspendLayout();
            base.SuspendLayout();
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = SystemColors.Window;
            this.panel1.Controls.Add(this.panMain);
            this.panel1.Controls.Add(this.panReport);
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x250, 0x1a0);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new PaintEventHandler(this.panel1_Paint);
            this.panMain.Controls.Add(this.l1);
            this.panMain.Controls.Add(this.ss2);
            this.panMain.Controls.Add(this.ss1);
            this.panMain.Controls.Add(this.TaxTable);
            this.panMain.Controls.Add(this.l5);
            this.panMain.Controls.Add(this.l4);
            this.panMain.Controls.Add(this.t8);
            this.panMain.Controls.Add(this.t7);
            this.panMain.Controls.Add(this.t6);
            this.panMain.Controls.Add(this.t5);
            this.panMain.Controls.Add(this.t3);
            this.panMain.Controls.Add(this.t4);
            this.panMain.Controls.Add(this.t2);
            this.panMain.Controls.Add(this.l3);
            this.panMain.Controls.Add(this.l2);
            this.panMain.Controls.Add(this.l0);
            this.panMain.Controls.Add(this.Year);
            this.panMain.Controls.Add(this.t1);
            this.panMain.Controls.Add(this.t0);
            this.panMain.Location = new Point(0, 0);
            this.panMain.Name = "panMain";
            this.panMain.Size = new Size(0x238, 0x29d);
            this.panMain.TabIndex = 0;
            this.l1.Location = new Point(0x1f8, 0x35);
            this.l1.Name = "l1";
            this.l1.Size = new Size(0x20, 12);
            this.l1.TabIndex = 0x18;
            this.l1.Text = "9999";
            this.ss2.Location = new Point(480, 0x35);
            this.ss2.Name = "ss2";
            this.ss2.Size = new Size(0x18, 12);
            this.ss2.TabIndex = 0x17;
            this.ss2.Text = "XX";
            this.ss1.Location = new Point(0x1c0, 0x35);
            this.ss1.Name = "ss1";
            this.ss1.Size = new Size(0x18, 12);
            this.ss1.TabIndex = 0x16;
            this.ss1.Text = "XXX";
            this.TaxTable.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.TaxTable.Location = new Point(0x198, 0x1c0);
            this.TaxTable.Name = "TaxTable";
            this.TaxTable.Size = new Size(0x30, 14);
            this.TaxTable.TabIndex = 0x15;
            this.TaxTable.TabStop = true;
            this.TaxTable.Text = "TaxTable";
            this.TaxTable.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkTaxTable_LinkClicked);
            this.l5.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.l5.Location = new Point(270, 630);
            this.l5.Name = "l5";
            this.l5.Size = new Size(50, 13);
            this.l5.TabIndex = 0x13;
            this.l5.Text = "label5";
            this.l4.Font = new Font("Monotype Corsiva", 9f, FontStyle.Italic, GraphicsUnit.Point, 0);
            this.l4.Location = new Point(0x58, 630);
            this.l4.Name = "l4";
            this.l4.Size = new Size(0xb0, 13);
            this.l4.TabIndex = 0x12;
            this.l4.Text = "label4";
            this.t8.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t8.BorderStyle = BorderStyle.None;
            this.t8.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t8.Location = new Point(0x1c8, 0x222);
            this.t8.Name = "t8";
            this.t8.Size = new Size(0x48, 11);
            this.t8.TabIndex = 0x11;
            this.t8.Text = "";
            this.t8.TextAlign = HorizontalAlignment.Right;
            this.t7.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t7.BorderStyle = BorderStyle.None;
            this.t7.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t7.Location = new Point(0x1c8, 0x1d9);
            this.t7.Name = "t7";
            this.t7.Size = new Size(0x48, 11);
            this.t7.TabIndex = 0x10;
            this.t7.Text = "";
            this.t7.TextAlign = HorizontalAlignment.Right;
            this.t6.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t6.BorderStyle = BorderStyle.None;
            this.t6.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t6.Location = new Point(0x1c8, 0x1c0);
            this.t6.Name = "t6";
            this.t6.Size = new Size(0x48, 11);
            this.t6.TabIndex = 15;
            this.t6.Text = "";
            this.t6.TextAlign = HorizontalAlignment.Right;
            this.t5.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t5.BorderStyle = BorderStyle.None;
            this.t5.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t5.Location = new Point(0x1c8, 0x162);
            this.t5.Name = "t5";
            this.t5.Size = new Size(0x48, 11);
            this.t5.TabIndex = 14;
            this.t5.Text = "";
            this.t5.TextAlign = HorizontalAlignment.Right;
            this.t3.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t3.BorderStyle = BorderStyle.None;
            this.t3.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t3.Location = new Point(0x1c8, 0x13c);
            this.t3.Name = "t3";
            this.t3.Size = new Size(0x48, 11);
            this.t3.TabIndex = 13;
            this.t3.Text = "";
            this.t3.TextAlign = HorizontalAlignment.Right;
            this.t3.TextChanged += new EventHandler(this.textBox3_TextChanged);
            this.t4.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t4.BorderStyle = BorderStyle.None;
            this.t4.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t4.Location = new Point(0x1c8, 0x152);
            this.t4.Name = "t4";
            this.t4.Size = new Size(0x48, 11);
            this.t4.TabIndex = 12;
            this.t4.Text = "";
            this.t4.TextAlign = HorizontalAlignment.Right;
            this.t2.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t2.BorderStyle = BorderStyle.None;
            this.t2.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t2.Location = new Point(0x1c8, 0x100);
            this.t2.Name = "t2";
            this.t2.Size = new Size(0x48, 11);
            this.t2.TabIndex = 11;
            this.t2.Text = "";
            this.t2.TextAlign = HorizontalAlignment.Right;
            this.l3.Location = new Point(0x70, 0x87);
            this.l3.Name = "l3";
            this.l3.Size = new Size(0xe0, 13);
            this.l3.TabIndex = 10;
            this.l3.Text = "label3";
            this.l2.Location = new Point(0x70, 0x66);
            this.l2.Name = "l2";
            this.l2.Size = new Size(0xe0, 13);
            this.l2.TabIndex = 9;
            this.l2.Text = "label2";
            this.l0.Location = new Point(0xf8, 0x35);
            this.l0.Name = "l0";
            this.l0.Size = new Size(0xb0, 13);
            this.l0.TabIndex = 8;
            this.l0.Text = "label1";
            this.Year.Font = new Font("Franklin Gothic Medium", 14.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.Year.Location = new Point(0x130, 0x10);
            this.Year.Name = "Year";
            this.Year.Size = new Size(0x58, 0x18);
            this.Year.TabIndex = 7;
            this.Year.Text = "2008";
            this.t1.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t1.BorderStyle = BorderStyle.None;
            this.t1.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t1.Location = new Point(0x1c8, 0xd0);
            this.t1.Name = "t1";
            this.t1.Size = new Size(0x48, 11);
            this.t1.TabIndex = 4;
            this.t1.Text = "";
            this.t1.TextAlign = HorizontalAlignment.Right;
            this.t0.BackColor = Color.FromArgb(0xe0, 0xe0, 0xe0);
            this.t0.BorderStyle = BorderStyle.None;
            this.t0.Font = new Font("Arial", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.t0.Location = new Point(0x1c8, 0xb8);
            this.t0.Name = "t0";
            this.t0.Size = new Size(0x48, 11);
            this.t0.TabIndex = 3;
            this.t0.Text = "";
            this.t0.TextAlign = HorizontalAlignment.Right;
            this.panReport.Location = new Point(0x18, 8);
            this.panReport.Name = "panReport";
            this.panReport.Size = new Size(0x1fc, 400);
            this.panReport.TabIndex = 8;
            this.panReport.Paint += new PaintEventHandler(this.panReport_Paint);
            this.btnFile.Location = new Point(200, 0x1b0);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new Size(0x80, 0x20);
            this.btnFile.TabIndex = 1;
            this.btnFile.Text = "File Tax Return";
            this.btnFile.Click += new EventHandler(this.btnFile_Click);
            this.cboOldReturns.Location = new Point(0x38, 440);
            this.cboOldReturns.Name = "cboOldReturns";
            this.cboOldReturns.Size = new Size(0x70, 0x15);
            this.cboOldReturns.TabIndex = 4;
            this.cboOldReturns.SelectedIndexChanged += new EventHandler(this.cboOldReturns_SelectedIndexChanged);
            this.btnClose.Location = new Point(0x1b0, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new Size(0x40, 0x18);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new EventHandler(this.button1_Click);
            this.TaxYear.Location = new Point(0x38, 0x1a8);
            this.TaxYear.Name = "TaxYear";
            this.TaxYear.Size = new Size(0x38, 0x10);
            this.TaxYear.TabIndex = 6;
            this.TaxYear.Text = "Tax Year:";
            this.Instructions.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.Instructions.ForeColor = Color.FromArgb(0x40, 0x40, 0x40);
            this.Instructions.Location = new Point(0x10, 0x1a8);
            this.Instructions.Name = "Instructions";
            this.Instructions.Size = new Size(0xa8, 40);
            this.Instructions.TabIndex = 7;
            this.Instructions.Text = "Instructions: Fill in the shaded boxes using information from your W2 forms and 1099-Int forms.";
            this.Instructions.TextAlign = ContentAlignment.MiddleLeft;
            this.btnHelp.Location = new Point(0x200, 440);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x40, 0x18);
            this.btnHelp.TabIndex = 8;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnPrint.Location = new Point(0x160, 440);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(0x40, 0x18);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x250, 0x1de);
            base.Controls.Add(this.btnPrint);
            base.Controls.Add(this.btnHelp);
            base.Controls.Add(this.Instructions);
            base.Controls.Add(this.TaxYear);
            base.Controls.Add(this.btnClose);
            base.Controls.Add(this.cboOldReturns);
            base.Controls.Add(this.btnFile);
            base.Controls.Add(this.panel1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmTaxes";
            base.ShowInTaskbar = false;
            this.Text = "Taxes";
            this.panel1.ResumeLayout(false);
            this.panMain.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void linkTaxTable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmTaxTable { Owner = this }.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void panReport_Paint(object sender, PaintEventArgs e)
        {
            this.PrintAccountantsReport(e.Graphics, 0);
        }

        protected void PrintAccountantsReport(Graphics g, int y)
        {
            string[] c = new string[] { "Courier New", "Lucida Console" };
            ArrayList list = new ArrayList(c);
            Font font = null;
            int num = 0;
            while (num < list.Count)
            {
                font = new Font((string) list[num], 12f);
                num++;
                if (list.Contains(font.FontFamily.Name))
                {
                    break;
                }
            }
            AccountantsReport currentReturn = (AccountantsReport) this.currentReturn;
            g.DrawString(currentReturn.Report, font, new SolidBrush(Color.Black), new Rectangle(this.panReport.Bounds.X, this.panReport.Bounds.Y + y, this.panReport.Width, this.panReport.Height));
        }

        protected void PrintPage(object sender, PrintPageEventArgs e)
        {
            Utilities.ResetFPU();
            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(brush, 1f);
            StringFormat format = new StringFormat {
                Alignment = StringAlignment.Far
            };
            int y = 10;
            if (this.studentName.Length > 0)
            {
                object[] args = new object[] { this.studentName };
                e.Graphics.DrawString(A.Resources.GetString("This report belongs to: {0}", args), this.Font, brush, 0f, (float) y);
            }
            y += 30;
            if (this.currentReturn is F1040EZ)
            {
                y += 30;
                e.Graphics.ScaleTransform(1.1f, 1.1f);
                e.Graphics.DrawImageUnscaled(A.Resources.GetImage("1040EZ"), 0, y);
                int num2 = 0;
                foreach (Control control in this.panMain.Controls)
                {
                    if (((control is TextBox) || (control is Label)) && (control.Text != ""))
                    {
                        Rectangle layoutRectangle = new Rectangle(control.Left + 10, (int) (1.04 * (y + control.Top)), control.Width - 1, control.Height);
                        StringFormat format2 = new StringFormat();
                        if (!(control is Label))
                        {
                            format2 = format;
                        }
                        e.Graphics.DrawString(control.Text, control.Font, brush, layoutRectangle, format2);
                        num2++;
                    }
                }
            }
            else
            {
                this.PrintAccountantsReport(e.Graphics, y);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        protected TaxReturn CurrentReturn
        {
            set
            {
                this.currentReturn = value;
                if (value is F1040EZ)
                {
                    this.panReport.Visible = false;
                    this.panMain.Visible = true;
                    this.Year.Text = value.ToString();
                    foreach (Control control in this.panMain.Controls)
                    {
                        for (int i = 0; i < this.panMain.Controls.Count; i++)
                        {
                            if (control.Name == ("t" + i))
                            {
                                if (value.Values[i] > 0)
                                {
                                    control.Text = value.Values[i].ToString();
                                }
                                else
                                {
                                    control.Text = "";
                                }
                                if (this.mode == Mode.Past)
                                {
                                    control.BackColor = Color.White;
                                    control.Enabled = false;
                                }
                            }
                            if (control.Name == ("l" + i))
                            {
                                control.Text = value.Lines[i];
                            }
                        }
                    }
                }
                else
                {
                    this.panMain.Visible = false;
                    this.panReport.Visible = true;
                    this.panReport.Refresh();
                }
            }
        }

        public enum Mode
        {
            Past,
            Current
        }
    }
}

