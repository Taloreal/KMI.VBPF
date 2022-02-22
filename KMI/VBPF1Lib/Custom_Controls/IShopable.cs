using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace KMI.VBPF1Lib {

    public interface IShopable {

        bool Buying { get; }

        string ItemName { get; }

        PurchasableItem PurchaseItem { get; }

    }
}
