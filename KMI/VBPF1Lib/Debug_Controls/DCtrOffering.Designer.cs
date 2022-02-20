
namespace KMI.VBPF1Lib.Debug_Controls
{
    partial class DCtrOffering
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labOfferList = new System.Windows.Forms.Label();
            this.Btn_Delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labOfferList
            // 
            this.labOfferList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labOfferList.Location = new System.Drawing.Point(3, 0);
            this.labOfferList.Name = "labOfferList";
            this.labOfferList.Size = new System.Drawing.Size(221, 54);
            this.labOfferList.TabIndex = 3;
            this.labOfferList.Text = "Offering Type:";
            this.labOfferList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Btn_Delete
            // 
            this.Btn_Delete.Location = new System.Drawing.Point(3, 165);
            this.Btn_Delete.Name = "Btn_Delete";
            this.Btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.Btn_Delete.TabIndex = 4;
            this.Btn_Delete.Text = "Delete";
            this.Btn_Delete.UseVisualStyleBackColor = true;
            // 
            // DCtrOffering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Btn_Delete);
            this.Controls.Add(this.labOfferList);
            this.Name = "DCtrOffering";
            this.Size = new System.Drawing.Size(227, 191);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labOfferList;
        private System.Windows.Forms.Button Btn_Delete;
    }
}
