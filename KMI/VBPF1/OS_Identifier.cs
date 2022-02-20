namespace KMI.VBPF1
{
    using System;
    using System.Management;
    using System.Text.RegularExpressions;

    public class OS_Identifier
    {
        public int getOSArchitectureLegacy()
        {
            string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return ((string.IsNullOrEmpty(environmentVariable) || (string.Compare(environmentVariable, 0, "x86", 0, 3, true) == 0)) ? 0x20 : 0x40);
        }

        public string getOSInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem");
            string str = "";
            int num = 0;
            try
            {
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    object propertyValue = obj2.GetPropertyValue("Caption");
                    if (propertyValue != null)
                    {
                        string str2 = Regex.Replace(propertyValue.ToString(), "[^A-Za-z0-9 ]", "");
                        if (str2.StartsWith("Microsoft"))
                        {
                            str2 = str2.Substring(9);
                        }
                        if (str2.Trim().StartsWith("Windows"))
                        {
                            str2 = str2.Trim().Substring(7);
                        }
                        str = str2.Trim();
                        if (!string.IsNullOrEmpty(str))
                        {
                            object obj5 = null;
                            try
                            {
                                obj5 = obj2.GetPropertyValue("ServicePackMajorVersion");
                                if ((obj5 != null) && (obj5.ToString() != "0"))
                                {
                                    str = str + " Service Pack " + obj5.ToString();
                                }
                                else
                                {
                                    str = str + this.getOSServicePackLegacy();
                                }
                            }
                            catch (Exception)
                            {
                                str = str + this.getOSServicePackLegacy();
                            }
                        }
                        object obj4 = null;
                        try
                        {
                            obj4 = obj2.GetPropertyValue("OSArchitecture");
                            if (obj4 != null)
                            {
                                num = obj4.ToString().Contains("64") ? 0x40 : 0x20;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            if (str == "")
            {
                str = this.getOSLegacy();
            }
            if (num == 0)
            {
                num = this.getOSArchitectureLegacy();
            }
            return (str + " " + num.ToString() + "-bit");
        }

        public string getOSLegacy()
        {
            OperatingSystem oSVersion = Environment.OSVersion;
            Version version = oSVersion.Version;
            string str = "";
            if (oSVersion.Platform == PlatformID.Win32Windows)
            {
                switch (version.Minor)
                {
                    case 0:
                        str = "95";
                        break;

                    case 10:
                        if (version.Revision.ToString() == "2222A")
                        {
                            str = "98SE";
                        }
                        else
                        {
                            str = "98";
                        }
                        break;

                    case 90:
                        str = "Me";
                        break;
                }
            }
            else if (oSVersion.Platform == PlatformID.Win32NT)
            {
                switch (version.Major)
                {
                    case 3:
                        str = "NT 3.51";
                        break;

                    case 4:
                        str = "NT 4.0";
                        break;

                    case 5:
                        if (version.Minor != 0)
                        {
                            str = "XP";
                            break;
                        }
                        str = "2000";
                        break;

                    case 6:
                        if (version.Minor != 0)
                        {
                            str = "7";
                            break;
                        }
                        str = "Vista";
                        break;
                }
            }
            if (str != "")
            {
                str = str + this.getOSServicePackLegacy();
            }
            return str;
        }

        public string getOSServicePackLegacy()
        {
            string servicePack = Environment.OSVersion.ServicePack;
            if (((servicePack != null) && (servicePack.ToString() != "")) && (servicePack.ToString() != " "))
            {
                return (" " + servicePack.ToString());
            }
            return "";
        }
    }
}

