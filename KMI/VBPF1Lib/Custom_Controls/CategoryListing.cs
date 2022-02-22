using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    public partial class CategoryListing : UserControl {

        private int WorkingIndex = 0;
        private PurchasableItem purchasableItem;
        private List<PurchasableItem> CategoryItems = new List<PurchasableItem>();

        public CategoryListing() {
            InitializeComponent();
        }


        public void AddPurchasableItems(List<PurchasableItem> items) {
            items.Reverse();
            if (items.Count != 0) {
                CategoryItems = items;
                for (int i = 0; i != items.Count; i++) {
                    Choices.Items.Add("Type: " + (i + 1).ToString());
                }
                purchasableItem = items[0];
                Choices.SelectedIndex = 0;
                UpdateInterface();
            }
        }

        public void BeginUpdate() {
            Choices.BeginUpdate();
        }

        public void EndUpdate() {
            Choices.EndUpdate();
        }

        private void Choices_SelectedIndexChanged(object sender, EventArgs e) {
            WorkingIndex = Choices.SelectedIndex;
            purchasableItem = CategoryItems[WorkingIndex];
            UpdateInterface();
        }


        private void UpdateInterface() {
            labDescription.Text = purchasableItem.Description;
            labImage.Image = A.Resources.GetImage(purchasableItem.ImageName);
            labName.Text = purchasableItem.FriendlyName;
            labPrice.Text = Utilities.FC(purchasableItem.Price, A.Instance.CurrencyConversion);
            if (purchasableItem.saleDiscount > 0f) {
                labPrice.ForeColor = Color.Red;
                labOnSale.Visible = true;
            }
        }

    }
}
