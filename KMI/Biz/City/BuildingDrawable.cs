namespace KMI.Biz.City
{
    using KMI.Sim;
    using KMI.Sim.Drawables;
    using System;
    using System.Drawing;

    [Serializable]
    public class BuildingDrawable : FlexDrawable
    {
        public int Avenue;
        public string BillboardOwnerName;
        public KMI.Biz.City.BuildingType BuildingType;
        public int Lot;
        public long OwnerID;
        public float Reach;
        public float Rent;
        public int Street;

        public BuildingDrawable(Point location, string imageName, KMI.Biz.City.BuildingType BuildingType, int avenue, int street, int lot, long ownerID, float reach, float rent, string billboardOwnerName) : base(location, imageName)
        {
            base.VerticalAlignment = FlexDrawable.VerticalAlignments.Bottom;
            this.BuildingType = BuildingType;
            this.Avenue = avenue;
            this.Street = street;
            this.Lot = lot;
            this.OwnerID = ownerID;
            this.Reach = reach;
            this.Rent = rent;
            this.BillboardOwnerName = billboardOwnerName;
            if ((this.BuildingType == KMI.Biz.City.City.BuildingTypes[0]) && (this.OwnerID > -1L))
            {
                base.clickString = " ";
                object[] args = new object[] { S.Instance.EntityName };
                base.ToolTipText = S.Resources.GetString("Click to Enter {0}", args);
            }
        }

        public override void Drawable_Click(object sender, EventArgs e)
        {
            if (this.OwnerID != -1L)
            {
                S.MainForm.OnCurrentEntityChange(this.OwnerID);
                S.MainForm.OnViewChange(S.Instance.Views[1].Name);
            }
        }
    }
}

