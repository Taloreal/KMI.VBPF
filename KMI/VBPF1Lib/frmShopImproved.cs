using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KMI.VBPF1Lib
{
    public partial class frmShopImproved : Form
    {

        private bool Car = false;

        public int ActiveIndex { get; private set; } = -1;



        private List<PurchasableItem> PurchasableItems = new List<PurchasableItem>();
        public List<List<PurchasableItem>> Categorized = new List<List<PurchasableItem>>();


        public frmShopImproved()
        {
            InitializeComponent();
        }


        private void frmShop_Load(object sender, EventArgs e) {

        }

        private void btnOK_Click(object sender, EventArgs e) {

        }

        private void btnHelp_Click(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {

        }
    }
}
