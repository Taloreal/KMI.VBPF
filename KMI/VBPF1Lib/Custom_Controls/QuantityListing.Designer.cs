using System;

namespace KMI.VBPF1Lib
{
    partial class QuantityListing
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labImage = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labName = new System.Windows.Forms.Label();
            this.labDescription = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Amount = new System.Windows.Forms.TextBox();
            this.labOnSale = new System.Windows.Forms.Label();
            this.chkBuy = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.labPrice = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(168, 104);
            this.panel1.TabIndex = 1;
            // 
            // labImage
            // 
            this.labImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labImage.Location = new System.Drawing.Point(0, 0);
            this.labImage.Name = "labImage";
            this.labImage.Size = new System.Drawing.Size(168, 104);
            this.labImage.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labName);
            this.panel3.Controls.Add(this.labDescription);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(168, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(15);
            this.panel3.Size = new System.Drawing.Size(205, 104);
            this.panel3.TabIndex = 4;
            // 
            // labName
            // 
            this.labName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labName.Location = new System.Drawing.Point(8, 0);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(208, 28);
            this.labName.TabIndex = 1;
            this.labName.Text = "Purchase ";
            // 
            // labDescription
            // 
            this.labDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDescription.Location = new System.Drawing.Point(8, 36);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new System.Drawing.Size(197, 56);
            this.labDescription.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Amount);
            this.panel2.Controls.Add(this.labOnSale);
            this.panel2.Controls.Add(this.chkBuy);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.labPrice);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(373, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(15);
            this.panel2.Size = new System.Drawing.Size(123, 104);
            this.panel2.TabIndex = 3;
            // 
            // Amount
            // 
            this.Amount.Location = new System.Drawing.Point(51, 58);
            this.Amount.Name = "Amount";
            this.Amount.Size = new System.Drawing.Size(47, 20);
            this.Amount.TabIndex = 4;
            this.Amount.Text = "1";
            this.Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Amount.TextChanged += new System.EventHandler(this.Amount_TextChanged);
            // 
            // labOnSale
            // 
            this.labOnSale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labOnSale.ForeColor = System.Drawing.Color.Red;
            this.labOnSale.Location = new System.Drawing.Point(12, 39);
            this.labOnSale.Name = "labOnSale";
            this.labOnSale.Size = new System.Drawing.Size(74, 16);
            this.labOnSale.TabIndex = 3;
            this.labOnSale.Text = "On Sale!";
            this.labOnSale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labOnSale.Visible = false;
            // 
            // chkBuy
            // 
            this.chkBuy.Location = new System.Drawing.Point(82, 81);
            this.chkBuy.Name = "chkBuy";
            this.chkBuy.Size = new System.Drawing.Size(16, 20);
            this.chkBuy.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 16);
            this.label4.TabIndex = 1;
            this.label4.Text = "Buy";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labPrice
            // 
            this.labPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labPrice.Location = new System.Drawing.Point(0, 13);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new System.Drawing.Size(120, 21);
            this.labPrice.TabIndex = 0;
            this.labPrice.Text = "$/";
            this.labPrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // QuantityListing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "QuantityListing";
            this.Size = new System.Drawing.Size(496, 104);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label labImage;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Label labName;
        public System.Windows.Forms.Label labDescription;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label labOnSale;
        public System.Windows.Forms.CheckBox chkBuy;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox Amount;
        public System.Windows.Forms.Label labPrice;
    }
}
