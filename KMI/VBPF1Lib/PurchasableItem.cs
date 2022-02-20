namespace KMI.VBPF1Lib
{
    using System;

    [Serializable]
    public class PurchasableItem
    {
        protected string description;
        protected string friendlyName;
        protected string imageName;
        protected string name;
        protected float price;
        public float saleDiscount;

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string FriendlyName
        {
            get
            {
                return this.friendlyName;
            }
            set
            {
                this.friendlyName = value;
            }
        }

        public string ImageName
        {
            get
            {
                return this.imageName;
            }
            set
            {
                this.imageName = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public float Price
        {
            get
            {
                return (this.price * (1f - this.saleDiscount));
            }
            set
            {
                this.price = value;
            }
        }
    }
}

