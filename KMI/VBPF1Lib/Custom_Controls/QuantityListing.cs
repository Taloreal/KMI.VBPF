using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMI.VBPF1Lib {

    public enum QuantityType {
        FoodListing = 0,
        GasListing = 1,
        BusListing = 2,
        NaN = 4,
    }

    public partial class QuantityListing : UserControl {

        /// <summary>
        /// Associates a quantity type with the name of the image that type should use.
        /// </summary>
        private static Dictionary<QuantityType, string> QuantityImages = new Dictionary<QuantityType, string>() {
            // Quantity Type                Image Name
            { QuantityType.FoodListing,     "BagOfFood2" },
            { QuantityType.GasListing,      "GasCan2" },
            { QuantityType.BusListing,      "BusTokens2" },
        };

        /// <summary>
        /// [0] = Name
        /// [1] = Description
        /// [2] = Price Tag
        /// </summary>
        private static Dictionary<QuantityType, string[]> QuantityStrings = new Dictionary<QuantityType, string[]>() {
                                                     // Name                   Description                          Price tag
            { QuantityType.FoodListing, new string[] { "Purchase Food",       "You need atleast 2 meals per day.", "/Bag" } },
            { QuantityType.GasListing, new string[] {  "Purchase Gas",        "Buy Octane Gas.",                   "/Gal" } },
            { QuantityType.BusListing, new string[] {  "Purchase Bus Tokens", "Buy tokens to ride the bus.",       "/Token" } },
        };

        private int _ToBuy = 1;
        public int ToBuy => _ToBuy;

        private QuantityType QuantType = QuantityType.NaN;

        public QuantityListing() {
            InitializeComponent();
        }

        public void SetQuantityType(QuantityType qType, PurchasableItem refItem) {
            QuantType    = qType;
            int quantity = ShopReference.BaseQuantities[(int)qType];
            float price  = (float)Math.Round(refItem.Price / quantity, 2);
            string tag   = price.ToString();
            tag          = tag.Contains(".") ? tag : tag + ".00";
            labImage.Image      = A.Resources.GetImage(QuantityImages[qType]);
            labPrice.Text       = "$" + tag + QuantityStrings[qType][2];
            labDescription.Text = QuantityStrings[qType][1];
            labName.Text        = QuantityStrings[qType][0];
            Settings.GetValue(qType.ToString() + "Amount", out _ToBuy);
            Amount.Text         = _ToBuy.ToString();
        }

        private void Amount_TextChanged(object sender, EventArgs e) {
            if (QuantType == QuantityType.NaN) { return; }
            try {
                _ToBuy = Convert.ToInt32(Amount.Text);
                Settings.SetValue<int>(QuantType.ToString() + "Amount", _ToBuy);
                chkBuy.Checked = true;
            }
            catch { Amount.Text = _ToBuy.ToString(); }
        }
    }
}
