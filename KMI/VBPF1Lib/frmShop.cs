using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using KMI.Utility;

namespace KMI.VBPF1Lib {

    public class frmShop : Form {

        private bool Car = false;

        public int ActiveIndex = -1;

        private double QuantityPrice = 0.0;

        private string sellerName;

        public List<string> SimilarGroups;


        public List<List<PurchasableItem>> Categorized;

        private Panel panel2;
        private Panel panListings2;

        private Button btnOK;
        private Button btnHelp;
        private Button btnCancel;

        private ArrayList PurchasableItems;

        private FlowLayoutPanel panListings;

        public QuantityListing QuantListing;

        private Container components = null;


        public frmShop(string sellerName, ArrayList purchasableItems, bool car) {
            DateTime start = DateTime.Now;
            this.SimilarGroups = new List<string>();
            this.Categorized = new List<List<PurchasableItem>>();
            this.InitializeComponent();
            this.sellerName = sellerName;
            this.Car = car;
            int num = 0;
            List<PurchasableItem> availableItems = new List<PurchasableItem>(
                (PurchasableItem[]) purchasableItems.ToArray(typeof(PurchasableItem)));
            this.AddTypes(ref availableItems, out this.PurchasableItems);
            int shop = ShopReference.Sellers.FindIndex(s => s == sellerName);
            if (shop != -1) { 
                QuantityPrice = GetPrice(shop);
                QuantListing = new QuantityListing();
                QuantListing.SetQuantityType((QuantityType)shop, GetQuantityPurchasable(shop));
            }
            this.CreateCategoryListings(ref num);
            foreach (PurchasableItem item in this.PurchasableItems) {
                ItemListing listing = new ItemListing(item) { 
                    Top = num * base.Height
                };
                if ((num % 2) == 1) { listing.BackColor = Color.LightGray; }
                this.panListings.Controls.Add(listing);
                num++;
            }
            if (shop != -1) {
                this.AddQuantityListing(shop, ShopReference.Images[shop].
                    Substring(0, ShopReference.Images[shop].Length - 1));
            }
            DateTime end = DateTime.Now;
            TimeSpan span = end - start;
        }

        public void AddQuantityListing(int ListingIndex, string RemoveName) {
            this.ActiveIndex = ListingIndex;
            this.panListings.Location = new Point(0, 105);
            this.panListings.Size = new Size(520, 229);
            this.Controls.Add(QuantListing);
            QuantListing.Location = new Point(0, 0);
            QuantListing.Visible = true;
            for (int i = 0; i != this.panListings.Controls.Count; i++) {
                if (((ItemListing) this.panListings.Controls[i]).purchasableItem.ImageName.Contains(RemoveName)) {
                    this.panListings.Controls.RemoveAt(i);
                    i--;
                }
            }
        }

        public void AddTypes(ref List<PurchasableItem> AvailableItems, out ArrayList LeftOvers) {
            LeftOvers = new ArrayList();
            while (AvailableItems.Count > 0) {
                if (ShopReference.Images.Contains(AvailableItems[0].ImageName)) {
                    LeftOvers.Add(AvailableItems[0]);
                    AvailableItems.RemoveAt(0);
                    continue;
                }
                int count = 0;
                string removal = AvailableItems[0].ImageName.Substring(0, AvailableItems[0].ImageName.Length - 1);
                PurchasableItem leftOver = new PurchasableItem();
                this.RemoveAllWith(ref AvailableItems, removal, out count, out leftOver);
                if (count > 1) { this.SimilarGroups.Add(removal); }
                else { LeftOvers.Add(leftOver); }
            }
        }

        private void btnHelp_Click(object sender, EventArgs e) {
            KMIHelp.OpenHelp(A.Resources.GetString("Shop For Goods"));
        }

        private void btnOK_Click(object sender, EventArgs e) {
            ArrayList items = new ArrayList();
            string called = "";
            float cost = 0f;
            if (ActiveIndex != -1 && QuantListing.chkBuy.Checked) {
                items.Add(this.Purchase(out cost, out called).Name);
            }
            foreach (ItemListing listing in this.panListings.Controls) {
                if (listing.chkBuy.Checked) {
                    items.Add(listing.purchasableItem.Name);
                    called = called + listing.labName.Text + ", ";
                    cost += listing.purchasableItem.Price;
                }
            }
            if (items.Count > 0) {
                if (!this.CheckSimilarPurchase(items)) {
                    called = Utilities.FormatCommaSeries(called);
                    Bill bill = A.Adapter.CreateBill(this.sellerName, "", cost, null);
                    Form form = null;
                    if (this.Car) { form = new frmPayForCar(bill, items); }
                    else { form = new frmPayBy(bill, items); }
                    form.ShowDialog(this);
                    base.Close();
                }
            }
            else { MessageBox.Show(A.Resources.GetString("Please select one or more items to buy."), A.Resources.GetString("Input Required")); }
        }

        private void button2_Click(object sender, EventArgs e) {
            base.Close();
        }

        protected bool CheckSimilarPurchase(ArrayList items) {
            ArrayList list = new ArrayList();
            foreach (string str in items) {
                if (!str.StartsWith("Art") && !str.StartsWith("Platter")) {
                    string item = str.Substring(0, str.Length - 1);
                    if (list.Contains(item)) {
                        string str3 = "apartment";
                        if (item == "Car") { str3 = "garage"; }
                        object[] args = new object[] { item, str3 };
                        MessageBox.Show(A.Resources.GetString("You are trying to purchase more than one {0}. There is room for only one in your {1}. Please modify your purchases.", args));
                        return true;
                    }
                    list.Add(item);
                }
            }
            return false;
        }

        public void CreateCategoryListings(ref int num) {
            foreach (List<PurchasableItem> list in this.Categorized) {
                CategoryListing listing = new CategoryListing();
                //CatListing listing = new CatListing();
                listing.Top = num * base.Height;
                listing.AddPurchasableItems(list);
                listing.BeginUpdate();
                if ((num % 2) == 1) { listing.BackColor = Color.LightGray; }
                this.panListings.Controls.Add(listing);
                num++;
            }
        }

        protected override void Dispose(bool disposing) {
            if (disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public double GetPrice(int shop) {
            double price = 0.0;
            foreach (PurchasableItem item in this.PurchasableItems) {
                if (item.ImageName == ShopReference.Images[shop]) {
                    price = Math.Round((double) (item.Price / ((float) 
                        ShopReference.BaseQuantities[shop])), 2);
                }
            }
            return price;
        }

        public PurchasableItem GetQuantityPurchasable(int shop) {
            foreach (PurchasableItem item in PurchasableItems) {
                if (item.ImageName == ShopReference.Images[shop]) {
                    return item;
                }
            }
            return null;
        }

        private void InitializeComponent() {
            this.panel2 = new Panel();
            this.btnHelp = new Button();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.panListings2 = new Panel();
            this.panListings = new FlowLayoutPanel();
            this.panel2.SuspendLayout();
            base.SuspendLayout();
            this.panel2.Controls.Add(this.btnHelp);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnOK);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 0x14e);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(520, 60);
            this.panel2.TabIndex = 1;
            this.btnHelp.Location = new Point(0x164, 20);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new Size(0x60, 0x18);
            this.btnHelp.TabIndex = 2;
            this.btnHelp.Text = "Help";
            this.btnHelp.Click += new EventHandler(this.btnHelp_Click);
            this.btnCancel.Location = new Point(0xd4, 20);
            this.btnCancel.Name = "button2";
            this.btnCancel.Size = new Size(0x60, 0x18);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new EventHandler(this.button2_Click);
            this.btnOK.Location = new Point(0x1c, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(0x74, 0x2c);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Checkout && Pay";
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            this.panListings2.AutoScroll = true;
            this.panListings2.Location = new Point(0, 0xcf);
            this.panListings2.Margin = new Padding(0);
            this.panListings2.Name = "panListings2";
            this.panListings2.Size = new Size(520, 0x7f);
            this.panListings2.TabIndex = 3;
            this.panListings.AutoScroll = true;
            this.panListings.Location = new Point(0, 0);
            this.panListings.Margin = new Padding(0);
            this.panListings.Name = "panListings";
            this.panListings.Size = new Size(520, 0x14b);
            this.panListings.TabIndex = 2;
            this.AutoScaleBaseSize = new Size(5, 13);
            base.ClientSize = new Size(520, 394);
            base.Controls.Add(this.panListings);
            base.Controls.Add(this.panListings2);
            base.Controls.Add(this.panel2);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += FrmShop_Load;
            base.Name = "frmShop";
            base.ShowInTaskbar = false;
            this.Text = "Shop";
            this.panel2.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private void FrmShop_Load(object sender, EventArgs e) {
            foreach (Control c in panListings.Controls) {
                if (c is CatListing) { 
                    CatListing cat = c as CatListing;
                    cat.EndUpdate();
                }
            }
        }

        public PurchasableItem Purchase(out float Cost, out string called) {
            PurchasableItem item = new PurchasableItem();
            int num = Convert.ToInt32(QuantListing.Amount.Text);
            item.Name = num.ToString() + ShopReference.Texts[this.ActiveIndex];
            called = num.ToString() + ShopReference.Texts[this.ActiveIndex] + ", ";
            item.Price = (float) (this.QuantityPrice * num);
            Cost = (float) (this.QuantityPrice * num);
            return item;
        }

        public void RemoveAllWith(ref List<PurchasableItem> List, string Removal, out int Count, out PurchasableItem LeftOver) {
            LeftOver = new PurchasableItem();
            List<int> list = new List<int>();
            List<PurchasableItem> item = new List<PurchasableItem>();
            for (int i = 0; i != List.Count; i++) {
                if (List[i].ImageName.Contains(Removal)) { 
                    list.Add(i); 
                }
            }
            list.Reverse();
            for (int j = 0; j != list.Count; j++) {
                item.Add(List[list[j]]);
                List.RemoveAt(list[j]);
            }
            if (item.Count > 1) { 
                this.Categorized.Add(item); 
            }
            else { 
                LeftOver = item[0]; 
            }
            Count = list.Count;
        }
    }
}

