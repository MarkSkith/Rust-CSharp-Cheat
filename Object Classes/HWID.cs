using Microsoft.VisualBasic.Devices;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace Impure.Object_Classes
{
    class HWID
    {
        private static string GetHash(string value)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            return GetHexString(sec.ComputeHash(bytes));
        }

        private static string GetHexString(IList<byte> bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Count; i++)
            {
                byte b = bt[i];
                int n = b;
                int n1 = n & 15;
                int n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n2.ToString(CultureInfo.InvariantCulture);
                if (n1 > 9)
                    s += ((char)(n1 - 10 + 'B')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n1.ToString(CultureInfo.InvariantCulture);
            }
            return s;
        }

        public static string INFO_cpuInfo = "";
        public static string INFO_volumeSerial = "";
        public static string INFO_TotalPhysicalMemory = "";
        public static string INFO_OSFullName = "";
        public static string INFO_Screen_DeviceName = "";
        public static string INFO_Height = "";
        public static string INFO_Width = "";
        public static string INFO_PrimaryBusType = "";
        public static string INFO_Product = "";
        public static string INFO_User = "";
        public static string INFO_PCName = "";

        public static string Get()
        {
            //MessageBox.Show("CPU");
            Computer myComputer = new Computer();
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + "C" + @":""");
            ManagementObjectCollection moc = mc.GetInstances();


            foreach (ManagementObject mo in moc)
            {
                try
                {
                    if (cpuInfo == "")
                    {
                        cpuInfo = mo.Properties["processorID"].Value.ToString();
                        break;
                    }
                }
                catch
                {

                }
            }

            //MessageBox.Show(cpuInfo);
            //MessageBox.Show("HDD");

            dsk.Get();
            string volumeSerial = dsk["VolumeSerialNumber"].ToString();
            //MessageBox.Show(volumeSerial);
            INFO_cpuInfo = cpuInfo;
            INFO_volumeSerial = volumeSerial;
            INFO_TotalPhysicalMemory = myComputer.Info.TotalPhysicalMemory.ToString();
            //MessageBox.Show(INFO_TotalPhysicalMemory);
            INFO_OSFullName = myComputer.Info.OSFullName;
            //MessageBox.Show(INFO_OSFullName);
            INFO_Screen_DeviceName = myComputer.Screen.DeviceName;
            INFO_Screen_DeviceName = INFO_Screen_DeviceName.Replace("\\\\.\\", "");
            //MessageBox.Show(INFO_Screen_DeviceName);
            INFO_Height = myComputer.Screen.Bounds.Height.ToString();
            //MessageBox.Show(INFO_Height);
            INFO_Width = myComputer.Screen.Bounds.Width.ToString();
            //MessageBox.Show(INFO_Width);
            INFO_PrimaryBusType = PrimaryBusType;
            //MessageBox.Show(INFO_PrimaryBusType);
            INFO_Product = Product;
            //MessageBox.Show(INFO_Product);
            INFO_PCName = myComputer.Name;
            //MessageBox.Show(INFO_PCName);
            INFO_User = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            INFO_User = INFO_User.Replace(INFO_PCName + "\\", "");
            //MessageBox.Show(INFO_User);

            return GetHash(INFO_cpuInfo + INFO_volumeSerial + INFO_TotalPhysicalMemory + INFO_OSFullName + INFO_Screen_DeviceName + INFO_Height + INFO_Width + INFO_PrimaryBusType + INFO_Product);
        }





        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");

        static public string Availability
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return GetAvailability(int.Parse(queryObj["Availability"].ToString()));
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public bool HostingBoard
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (queryObj["HostingBoard"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        static public string InstallDate
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return ConvertToDateTime(queryObj["InstallDate"].ToString());
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string Manufacturer
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Manufacturer"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string Model
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Model"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string PartNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["PartNumber"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string PNPDeviceID
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["PNPDeviceID"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string PrimaryBusType
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["PrimaryBusType"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string Product
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Product"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public bool Removable
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (queryObj["Removable"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        static public bool Replaceable
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        if (queryObj["Replaceable"].ToString() == "True")
                            return true;
                        else
                            return false;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        static public string RevisionNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["RevisionNumber"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string SecondaryBusType
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["SecondaryBusType"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string SerialNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["SerialNumber"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string Status
        {
            get
            {
                try
                {
                    foreach (ManagementObject querObj in baseboardSearcher.Get())
                    {
                        return querObj["Status"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string SystemName
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["SystemName"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        static public string Version
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Version"].ToString();
                    }
                    return "";
                }
                catch
                {
                    return "";
                }
            }
        }

        private static string GetAvailability(int availability)
        {
            switch (availability)
            {
                case 1: return "Other";
                case 2: return "Unknown";
                case 3: return "Running or Full Power";
                case 4: return "Warning";
                case 5: return "In Test";
                case 6: return "Not Applicable";
                case 7: return "Power Off";
                case 8: return "Off Line";
                case 9: return "Off Duty";
                case 10: return "Degraded";
                case 11: return "Not Installed";
                case 12: return "Install Error";
                case 13: return "Power Save - Unknown";
                case 14: return "Power Save - Low Power Mode";
                case 15: return "Power Save - Standby";
                case 16: return "Power Cycle";
                case 17: return "Power Save - Warning";
                default: return "Unknown";
            }
        }

        private static string ConvertToDateTime(string unconvertedTime)
        {
            string convertedTime = "";
            int year = int.Parse(unconvertedTime.Substring(0, 4));
            int month = int.Parse(unconvertedTime.Substring(4, 2));
            int date = int.Parse(unconvertedTime.Substring(6, 2));
            int hours = int.Parse(unconvertedTime.Substring(8, 2));
            int minutes = int.Parse(unconvertedTime.Substring(10, 2));
            int seconds = int.Parse(unconvertedTime.Substring(12, 2));
            string meridian = "AM";
            if (hours > 12)
            {
                hours -= 12;
                meridian = "PM";
            }
            convertedTime = date.ToString() + "/" + month.ToString() + "/" + year.ToString() + " " +
            hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString() + " " + meridian;
            return convertedTime;
        }
    }
}