namespace KMI.Sim
{
    using System;

    [Serializable]
    public class SimSettings
    {
        protected string blockMessagesContaining = "";
        protected int level = 0;
        protected bool levelManagementOn = true;
        protected byte[] pdfAssignment;
        protected int randomSeed = -1;
        protected bool reservedBool1;
        protected bool reservedBool2;
        protected bool reservedBool3;
        protected float reservedFloat1;
        protected float reservedFloat2;
        protected float reservedFloat3;
        protected bool seasonality = true;
        protected DateTime startDate = new DateTime(0x7d8, 1, 5, 0x17, 0x3b, 0x3b);
        protected DateTime stopDate;
        protected int studentOrg = 0;

        public virtual bool AllowInstructorToEdit(string propertyName)
        {
            return false;
        }

        public string BlockMessagesContaining
        {
            get
            {
                return this.blockMessagesContaining;
            }
            set
            {
                this.blockMessagesContaining = value;
            }
        }

        public virtual int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public bool LevelManagementOn
        {
            get
            {
                return this.levelManagementOn;
            }
            set
            {
                this.levelManagementOn = value;
            }
        }

        public byte[] PdfAssignment
        {
            get
            {
                return this.pdfAssignment;
            }
            set
            {
                this.pdfAssignment = value;
            }
        }

        public int RandomSeed
        {
            get
            {
                return this.randomSeed;
            }
            set
            {
                this.randomSeed = value;
            }
        }

        public bool ReservedBool1
        {
            get
            {
                return this.reservedBool1;
            }
            set
            {
                this.reservedBool1 = value;
            }
        }

        public bool ReservedBool2
        {
            get
            {
                return this.reservedBool2;
            }
            set
            {
                this.reservedBool2 = value;
            }
        }

        public bool ReservedBool3
        {
            get
            {
                return this.reservedBool3;
            }
            set
            {
                this.reservedBool3 = value;
            }
        }

        public float ReservedFloat1
        {
            get
            {
                return this.reservedFloat1;
            }
            set
            {
                this.reservedFloat1 = value;
            }
        }

        public float ReservedFloat2
        {
            get
            {
                return this.reservedFloat2;
            }
            set
            {
                this.reservedFloat2 = value;
            }
        }

        public float ReservedFloat3
        {
            get
            {
                return this.reservedFloat3;
            }
            set
            {
                this.reservedFloat3 = value;
            }
        }

        public bool Seasonality
        {
            get
            {
                return this.seasonality;
            }
            set
            {
                this.seasonality = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = value;
            }
        }

        public DateTime StopDate
        {
            get
            {
                return this.stopDate;
            }
            set
            {
                this.stopDate = value;
            }
        }

        public int StudentOrg
        {
            get
            {
                return this.studentOrg;
            }
            set
            {
                this.studentOrg = value;
            }
        }
    }
}

