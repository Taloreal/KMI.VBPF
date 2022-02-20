namespace KMI.Sim
{
    using KMI.Utility;
    using System;

    public class MultiplayerRole
    {
        public string[] DisableList;
        protected string disableListConcatenated;
        public bool receivesMessages;
        protected string roleName;
        public static MultiplayerRole[] Roles;

        public bool DisableListContains(string text)
        {
            foreach (string str in this.DisableList)
            {
                if (Utilities.NoAmpersand(Utilities.NoEllipsis(text)) == str)
                {
                    return true;
                }
            }
            return false;
        }

        public static void LoadRolesFromTable(Type type, string resource)
        {
            Roles = (MultiplayerRole[]) TableReader.Read(type.Assembly, typeof(MultiplayerRole), resource);
            foreach (MultiplayerRole role in Roles)
            {
                char[] separator = new char[] { '|' };
                role.DisableList = role.DisableListConcatenated.Split(separator);
                role.DisableListConcatenated = null;
            }
        }

        public string DisableListConcatenated
        {
            get
            {
                return this.disableListConcatenated;
            }
            set
            {
                this.disableListConcatenated = value;
            }
        }

        public bool ReceivesMessages
        {
            get
            {
                return this.receivesMessages;
            }
            set
            {
                this.receivesMessages = value;
            }
        }

        public string RoleName
        {
            get
            {
                return this.roleName;
            }
            set
            {
                this.roleName = value;
            }
        }
    }
}

