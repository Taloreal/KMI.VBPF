using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KMI.VBPF1Lib {

    public static class ShopReference {

        public static List<int> BaseQuantities = new List<int> { 
            14, 10, 20
        };

        public static List<string> Texts = new List<string> { 
            " bags of food", " gals of gas", " tokens"
        };

        public static List<string> Sellers = new List<string>() { 
            "SuperMarket, Inc.", "Gas & Repairs, Inc.", "City Bus"
        };

        public static List<string> Images = new List<string> { 
            "BagOfFood0", "GasCan0", "BusTokens0"
        };
    }
}
