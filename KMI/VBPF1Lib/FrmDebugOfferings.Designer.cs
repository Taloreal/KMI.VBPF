
namespace KMI.VBPF1Lib
{
    partial class FrmDebugOfferings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDebugOfferings));
            this.CB_Offerings = new System.Windows.Forms.ComboBox();
            this.labBuildType = new System.Windows.Forms.Label();
            this.labOfferList = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // CB_Offerings
            // 
            this.CB_Offerings.FormattingEnabled = true;
            this.CB_Offerings.Location = new System.Drawing.Point(82, 35);
            this.CB_Offerings.Name = "CB_Offerings";
            this.CB_Offerings.Size = new System.Drawing.Size(159, 21);
            this.CB_Offerings.TabIndex = 0;
            // 
            // labBuildType
            // 
            this.labBuildType.AutoSize = true;
            this.labBuildType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labBuildType.Location = new System.Drawing.Point(12, 9);
            this.labBuildType.Name = "labBuildType";
            this.labBuildType.Size = new System.Drawing.Size(97, 16);
            this.labBuildType.TabIndex = 1;
            this.labBuildType.Text = "Building Type: ";
            // 
            // labOfferList
            // 
            this.labOfferList.AutoSize = true;
            this.labOfferList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labOfferList.Location = new System.Drawing.Point(12, 36);
            this.labOfferList.Name = "labOfferList";
            this.labOfferList.Size = new System.Drawing.Size(64, 16);
            this.labOfferList.TabIndex = 2;
            this.labOfferList.Text = "Offerings:";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 193);
            this.panel1.TabIndex = 3;
            // 
            // FrmDebugOfferings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 267);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labOfferList);
            this.Controls.Add(this.labBuildType);
            this.Controls.Add(this.CB_Offerings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDebugOfferings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debug Offerings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CB_Offerings;
        private System.Windows.Forms.Label labBuildType;
        private System.Windows.Forms.Label labOfferList;
        private System.Windows.Forms.Panel panel1;
    }
}