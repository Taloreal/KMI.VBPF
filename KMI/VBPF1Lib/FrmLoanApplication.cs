namespace KMI.VBPF1Lib
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class FrmLoanApplication : Form
    {
        private IContainer components = null;
        private Label lbScore;

        public FrmLoanApplication()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void FrmLoanApplication_Load(object sender, EventArgs e)
        {
            this.lbScore.Text = this.lbScore.Text + A.Adapter.GetCreditScore(A.MainForm.CurrentEntityID).Score;
        }

        private void InitializeComponent()
        {
            this.lbScore = new Label();
            base.SuspendLayout();
            this.lbScore.AutoSize = true;
            this.lbScore.Location = new Point(12, 9);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new Size(0x6c, 13);
            this.lbScore.TabIndex = 0;
            this.lbScore.Text = "Current Credit Score: ";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x11c, 0x106);
            base.ControlBox = false;
            base.Controls.Add(this.lbScore);
            base.Name = "FrmLoanApplication";
            this.Text = "Loan Application";
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}

