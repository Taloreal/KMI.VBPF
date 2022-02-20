using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class CategoryListing : ItemListing {

        private int WorkingIndex = 0;

        private ComboBox Choices;
        private IContainer components = null;

        private List<ItemListing> ControlSettings = new List<ItemListing>();
        private List<PurchasableItem> CategoryItems = new List<PurchasableItem>();

        public CategoryListing(List<PurchasableItem> PIs) {
            PIs.Reverse();
            this.InitializeComponent();
            if (PIs.Count != 0) {
                this.CategoryItems = PIs;
                for (int i = 0; i != PIs.Count; i++) {
                    this.ControlSettings.Add(new ItemListing(PIs[i]));
                    int num2 = i + 1;
                    this.Choices.Items.Add("Type: " + num2.ToString());
                }
                base.purchasableItem = PIs[0];
                this.Choices.SelectedIndex = 0;
                this.UpdateInterface();
            }
        }

        public void BeginUpdate() {
            Choices.BeginUpdate();
        }

        public void EndUpdate() {
            Choices.EndUpdate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            this.WorkingIndex = this.Choices.SelectedIndex;
            base.purchasableItem = this.CategoryItems[this.WorkingIndex];
            this.UpdateInterface();
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent() {
            this.Choices = new ComboBox();
            base.panel2.SuspendLayout();
            base.SuspendLayout();
            base.panel2.Controls.Add(this.Choices);
            base.panel2.Controls.SetChildIndex(base.chkBuy, 0);
            base.panel2.Controls.SetChildIndex(this.Choices, 0);
            this.Choices.FormattingEnabled = true;
            this.Choices.Location = new Point(8, 0x4b);
            this.Choices.Name = "Choices";
            this.Choices.Size = new Size(0x5d, 0x15);
            this.Choices.TabIndex = 4;
            this.Choices.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "CategoryListing";
            base.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void UpdateInterface() {
            base.labDescription.Text = base.purchasableItem.Description;
            base.labImage.Image = A.Resources.GetImage(base.purchasableItem.ImageName);
            base.labName.Text = base.purchasableItem.FriendlyName;
            base.labPrice.Text = Utilities.FC(base.purchasableItem.Price, A.Instance.CurrencyConversion);
            if (base.purchasableItem.saleDiscount > 0f) {
                base.labPrice.ForeColor = Color.Red;
                base.labOnSale.Visible = true;
            }
        }
    }
}

