using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KMI.VBPF1Lib {

    public partial class FrmDebugOfferings : Form {

        private AppBuildingDrawable Building;

        public FrmDebugOfferings(AppBuildingDrawable building) {
            InitializeComponent();
            Building = building;
            labBuildType.Text += building.BuildingType.Name;
            UpdateComboBox();
        }

        private void UpdateComboBox() {
            CB_Offerings.Items.Clear();
            int count = 1;
            foreach (Offering offer in Building.Offerings) {
                CB_Offerings.Items.Add(count.ToString() + ": " + offer.ThingName());
                count++;
            }
            if (count < 9) { CB_Offerings.Items.Add("New Offering"); }
            CB_Offerings.SelectedIndex = 0;
        }
    }
}
