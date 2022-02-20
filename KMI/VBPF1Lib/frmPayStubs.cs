namespace KMI.VBPF1Lib
{
    using KMI.Sim;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class frmPayStubs : frmDrawnReport
    {
        private static Brush brush = new SolidBrush(Color.Black);
        private Button btnBack;
        private Button btnNext;
        private ComboBox cboYear;
        private Container components = null;
        private int currentIndex = 0;
        private static Font font = new Font("Arial", 8f);
        private Input input;
        private Label label1;
        private Label label3;
        private RadioButton optShow0;
        private RadioButton optShow1;
        private RadioButton optShow2;
        private ArrayList selectedForms;

        public frmPayStubs()
        {
            this.InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.currentIndex--;
            base.picReport.Refresh();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.currentIndex++;
            base.picReport.Refresh();
        }

        private void cboYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.optShow_CheckedChanged(sender, e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void DrawReportVirtual(Graphics g)
        {
            if (this.selectedForms.Count == 0)
            {
                object[] args = new object[] { this.cboYear.SelectedItem.ToString() };
                g.DrawString(A.Resources.GetString("No Forms Available for {0}", args), font, brush, (float) 30f, (float) 30f);
            }
            else
            {
                ((ITaxForm) this.selectedForms[this.currentIndex]).Print(g);
            }
            this.SynchBackNext();
        }

        protected override void GetDataVirtual()
        {
            this.input = A.Adapter.GetTaxInfo(A.MainForm.CurrentEntityID);
            if (this.cboYear.Items.Count == 0)
            {
                for (int i = this.input.BeginYear; i <= this.input.EndYear; i++)
                {
                    this.cboYear.Items.Add(i);
                }
                this.cboYear.SelectedIndex = this.cboYear.Items.Count - 1;
            }
        }

        private void InitializeComponent()
        {
            this.optShow0 = new RadioButton();
            this.optShow1 = new RadioButton();
            this.cboYear = new ComboBox();
            this.label1 = new Label();
            this.label3 = new Label();
            this.btnBack = new Button();
            this.btnNext = new Button();
            this.optShow2 = new RadioButton();
            base.pnlBottom.SuspendLayout();
            base.pnlBottom.Controls.Add(this.optShow2);
            base.pnlBottom.Controls.Add(this.btnNext);
            base.pnlBottom.Controls.Add(this.btnBack);
            base.pnlBottom.Controls.Add(this.label3);
            base.pnlBottom.Controls.Add(this.label1);
            base.pnlBottom.Controls.Add(this.cboYear);
            base.pnlBottom.Controls.Add(this.optShow0);
            base.pnlBottom.Controls.Add(this.optShow1);
            base.pnlBottom.Location = new Point(0, 0x152);
            base.pnlBottom.Name = "pnlBottom";
            base.pnlBottom.Size = new Size(0x21e, 0x58);
            base.pnlBottom.Controls.SetChildIndex(this.optShow1, 0);
            base.pnlBottom.Controls.SetChildIndex(this.optShow0, 0);
            base.pnlBottom.Controls.SetChildIndex(this.cboYear, 0);
            base.pnlBottom.Controls.SetChildIndex(this.label1, 0);
            base.pnlBottom.Controls.SetChildIndex(this.label3, 0);
            base.pnlBottom.Controls.SetChildIndex(this.btnBack, 0);
            base.pnlBottom.Controls.SetChildIndex(this.btnNext, 0);
            base.pnlBottom.Controls.SetChildIndex(this.optShow2, 0);
            base.picReport.BackColor = Color.White;
            base.picReport.Name = "picReport";
            base.picReport.Size = new Size(0x1fc, 0x134);
            this.optShow0.Checked = true;
            this.optShow0.Location = new Point(0x1c, 0x18);
            this.optShow0.Name = "optShow0";
            this.optShow0.Size = new Size(0x58, 0x10);
            this.optShow0.TabIndex = 0;
            this.optShow0.TabStop = true;
            this.optShow0.Text = "Pay Stubs";
            this.optShow0.CheckedChanged += new EventHandler(this.optShow_CheckedChanged);
            this.optShow1.Location = new Point(0x1c, 0x2c);
            this.optShow1.Name = "optShow1";
            this.optShow1.Size = new Size(0x58, 0x10);
            this.optShow1.TabIndex = 1;
            this.optShow1.Text = "W-2s";
            this.optShow1.CheckedChanged += new EventHandler(this.optShow_CheckedChanged);
            this.cboYear.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboYear.Location = new Point(120, 0x24);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new Size(80, 0x15);
            this.cboYear.TabIndex = 2;
            this.cboYear.SelectedIndexChanged += new EventHandler(this.cboYear_SelectedIndexChanged);
            this.label1.Location = new Point(120, 20);
            this.label1.Name = "label1";
            this.label1.Size = new Size(80, 0x10);
            this.label1.TabIndex = 3;
            this.label1.Text = "Year";
            this.label3.Location = new Point(0x10, 8);
            this.label3.Name = "label3";
            this.label3.Size = new Size(60, 0x10);
            this.label3.TabIndex = 6;
            this.label3.Text = "Show";
            this.btnBack.Location = new Point(0xd8, 0x20);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new Size(0x40, 0x18);
            this.btnBack.TabIndex = 7;
            this.btnBack.Text = "<< Back";
            this.btnBack.Click += new EventHandler(this.btnBack_Click);
            this.btnNext.Location = new Point(0x120, 0x20);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new Size(0x40, 0x18);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "Next >>";
            this.btnNext.Click += new EventHandler(this.btnNext_Click);
            this.optShow2.Location = new Point(0x1c, 0x40);
            this.optShow2.Name = "optShow2";
            this.optShow2.Size = new Size(0x58, 0x10);
            this.optShow2.TabIndex = 9;
            this.optShow2.Text = "1099s";
            this.optShow2.CheckedChanged += new EventHandler(this.optShow_CheckedChanged);
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(0x21e, 0x1aa);
            this.StartPosition = FormStartPosition.CenterScreen;
            base.Name = "frmPayStubs";
            this.Text = "Pay & Tax Records";
            base.pnlBottom.ResumeLayout(false);
        }

        private void optShow_CheckedChanged(object sender, EventArgs e)
        {
            this.currentIndex = 0;
            if (this.optShow0.Checked)
            {
                this.selectedForms = this.UpdateSelectedForms(this.input.PayStubs);
                this.currentIndex = this.selectedForms.Count - 1;
            }
            if (this.optShow1.Checked)
            {
                this.selectedForms = this.UpdateSelectedForms(this.input.FW2s);
            }
            if (this.optShow2.Checked)
            {
                this.selectedForms = this.UpdateSelectedForms(this.input.F1099s);
            }
            base.picReport.Refresh();
        }

        private void SynchBackNext()
        {
            this.btnBack.Enabled = this.currentIndex > 0;
            this.btnNext.Enabled = this.currentIndex < (this.selectedForms.Count - 1);
        }

        private ArrayList UpdateSelectedForms(ArrayList forms)
        {
            ArrayList list = new ArrayList();
            foreach (ITaxForm form in forms)
            {
                if (form.Year() == ((int) this.cboYear.SelectedItem))
                {
                    list.Add(form);
                }
            }
            return list;
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct Input
        {
            public ArrayList PayStubs;
            public ArrayList FW2s;
            public ArrayList F1099s;
            public int BeginYear;
            public int EndYear;
        }
    }
}

