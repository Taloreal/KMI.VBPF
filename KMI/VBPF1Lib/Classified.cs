namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class Classified : UserControl
    {
        private Container components = null;
        public Label labText;
        public RadioButton opt;
        public static StringFormat SFTW;

        public Classified()
        {
            this.InitializeComponent();
            if (SFTW == null)
            {
                SFTW = new StringFormat();
                SFTW.Trimming = StringTrimming.Word;
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
            this.labText = new Label();
            this.opt = new RadioButton();
            base.SuspendLayout();
            this.labText.Font = new Font("Microsoft Sans Serif", 6.75f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labText.Location = new Point(0x20, 8);
            this.labText.Name = "labText";
            this.labText.Size = new Size(0x98, 0x20);
            this.labText.TabIndex = 0;
            this.labText.Text = "labText";
            this.labText.TextChanged += new EventHandler(this.labText_TextChanged);
            this.opt.Location = new Point(8, 0x10);
            this.opt.Name = "opt";
            this.opt.Size = new Size(0x10, 0x10);
            this.opt.TabIndex = 1;
            this.opt.Text = "radioButton1";
            this.BackColor = Color.White;
            base.Controls.Add(this.opt);
            base.Controls.Add(this.labText);
            base.Name = "Classified";
            base.Size = new Size(190, 0x30);
            base.ResumeLayout(false);
        }

        private void labText_TextChanged(object sender, EventArgs e)
        {
            this.labText.Height = (int) this.labText.CreateGraphics().MeasureString(this.labText.Text, this.labText.Font, this.labText.Width, SFTW).Height;
            base.Height = this.labText.Height + 8;
            this.opt.Top = ((base.Height + 10) - this.opt.Height) / 2;
        }
    }
}

