using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class ItemListing : UserControl {

        public CheckBox chkBuy;
        protected Container components;
        protected Label labDescription;
        protected Label label4;
        protected Label labImage;
        public Label labName;
        protected Label labOnSale;
        protected Label labPrice;
        public Panel panel1;
        public Panel panel2;
        public Panel panel3;
        public PurchasableItem purchasableItem;

        public ItemListing() {
            this.components = null;
            this.InitializeComponent();
        }

        public ItemListing(PurchasableItem p) {
            this.components = null;
            this.InitializeComponent();
            this.labDescription.Text = p.Description;
            this.labImage.Image = A.Resources.GetImage(p.ImageName);
            this.labName.Text = p.FriendlyName;
            this.labPrice.Text = Utilities.FC(p.Price, A.Instance.CurrencyConversion);
            if (p.saleDiscount > 0f) {
                this.labPrice.ForeColor = Color.Red;
                this.labOnSale.Visible = true;
            }
            this.purchasableItem = p;
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.panel1 = new Panel();
            this.labImage = new Label();
            this.panel2 = new Panel();
            this.labOnSale = new Label();
            this.chkBuy = new CheckBox();
            this.label4 = new Label();
            this.labPrice = new Label();
            this.panel3 = new Panel();
            this.labName = new Label();
            this.labDescription = new Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.labImage);
            this.panel1.Dock = DockStyle.Left;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0xa8, 0x68);
            this.panel1.TabIndex = 0;
            this.labImage.Dock = DockStyle.Fill;
            this.labImage.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labImage.Location = new Point(0, 0);
            this.labImage.Name = "labImage";
            this.labImage.Size = new Size(0xa8, 0x68);
            this.labImage.TabIndex = 0;
            this.panel2.Controls.Add(this.labOnSale);
            this.panel2.Controls.Add(this.chkBuy);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.labPrice);
            this.panel2.Dock = DockStyle.Right;
            this.panel2.Location = new Point(0x188, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new Padding(15);
            this.panel2.Size = new Size(0x68, 0x68);
            this.panel2.TabIndex = 1;
            this.labOnSale.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labOnSale.ForeColor = Color.Red;
            this.labOnSale.Location = new Point(12, 0x24);
            this.labOnSale.Name = "labOnSale";
            this.labOnSale.Size = new Size(0x4a, 0x10);
            this.labOnSale.TabIndex = 3;
            this.labOnSale.Text = "On Sale!";
            this.labOnSale.TextAlign = ContentAlignment.MiddleRight;
            this.labOnSale.Visible = false;
            this.chkBuy.Location = new Point(0x48, 0x38);
            this.chkBuy.Name = "chkBuy";
            this.chkBuy.Size = new Size(0x10, 20);
            this.chkBuy.TabIndex = 2;
            this.label4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Italic | FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label4.Location = new Point(12, 0x38);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x34, 0x10);
            this.label4.TabIndex = 1;
            this.label4.Text = "Buy It!";
            this.label4.TextAlign = ContentAlignment.MiddleLeft;
            this.labPrice.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labPrice.Location = new Point(4, 15);
            this.labPrice.Name = "labPrice";
            this.labPrice.Size = new Size(0x54, 0x10);
            this.labPrice.TabIndex = 0;
            this.labPrice.TextAlign = ContentAlignment.MiddleRight;
            this.panel3.Controls.Add(this.labName);
            this.panel3.Controls.Add(this.labDescription);
            this.panel3.Dock = DockStyle.Fill;
            this.panel3.Location = new Point(0xa8, 0);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new Padding(15);
            this.panel3.Size = new Size(0xe0, 0x68);
            this.panel3.TabIndex = 2;
            this.labName.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labName.Location = new Point(8, 4);
            this.labName.Name = "labName";
            this.labName.Size = new Size(0xd0, 0x1c);
            this.labName.TabIndex = 1;
            this.labDescription.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.labDescription.Location = new Point(8, 0x24);
            this.labDescription.Name = "labDescription";
            this.labDescription.Size = new Size(0xcc, 0x38);
            this.labDescription.TabIndex = 0;
            base.Controls.Add(this.panel3);
            base.Controls.Add(this.panel2);
            base.Controls.Add(this.panel1);
            base.Margin = new Padding(0);
            base.Name = "ItemListing";
            base.Size = new Size(0x1f0, 0x68);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            base.ResumeLayout(false);
        }
    }
}

