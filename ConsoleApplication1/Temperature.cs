using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using System.Management.Instrumentation;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.IO;

namespace ConsoleApplication1
{
    class Temperature
    {
        public string ComputerName
        {
            get
            {
                return compname();
            }
        }
        public string NameCPU
        {
            get
            {
                return namecpu();
            }
        }

        public List<float?> TemperatureCPU
        {
            get
            {
                temperaturecpu();
                return tempcpu;
            }
        }

        public List<float?> ClocksCPU
        {
            get
            {
                clockcpu();
                return clockscpu;
            }
        }

        public float? MemoryLoad
        {
            get
            {
                return memorylload();
            }
        }

        public List<float?> MemoryData
        {
            get
            {
                memoryddata();
                return memorydata;
            }
        }

        public string NameHDD
        {
            get
            {
                return namehdd();
            }
        }

        public float? TemperatureHDD 
        {
            get
            {
                return temperaturehdd();
            }
        }

        public string NameGPU
        {
            get
            {
                return namegpu();
            }
        }

        public float? LoadGPU
        {
            get
            {
                return loadggpu();
            }
        }

        public List<float?> DataGPU
        {
            get
            {
                dataggpu();
                return datagpu;
            }
        }

        public string LocalIpAddress
        {
            get
            {
                return GetLocalIpAddress();
            }
        }

        public string MACaddress
        {
            get
            {
                return GetSystemMACID();
            }
        }

        Computer thisComputer;
        StreamWriter writer;
        FileStream fs;
        List<float?> tempcpu = new List<float?>();
        List<float?> clockscpu = new List<float?>();
        List<float?> memorydata = new List<float?>();
        List<float?> datagpu = new List<float?>();

        string path = Directory.GetCurrentDirectory();
        string filename = "log1.txt";

        public Temperature() 
        {
            //Запуск работы вычисления
            thisComputer = new Computer() { CPUEnabled = true, GPUEnabled = true, HDDEnabled = true, RAMEnabled = true, MainboardEnabled = true };
            thisComputer.Open();
        }

        public float? temperaturehdd()
        {
            float? temperature = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.HDD)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            temperature = (thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
            return temperature;
        }

        public string namehdd()
        {
            string name = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.HDD)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                            name = (thisComputer.Hardware[i].Name);
                    }
                }
            }
            name = (name == null) ? "error" : name;
            return name;
        }

        public string compname()
        {
            string name = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.Mainboard)
                {
                    thisComputer.Hardware[i].Update();
                    name = (thisComputer.Hardware[i].Name);
                    
                }
            }
            name = (name == null) ? "error" : name;
            return name;
        }

        public void temperaturecpu()
        {
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature && thisComputer.Hardware[i].Sensors[j].Name != "CPU Package")
                            tempcpu.Add(thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
        }

        public void memoryddata()
        {
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Data)
                            memorydata.Add(thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
            
        }

        public float? memorylload()
        {
            float? memoryload = null;
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            memoryload = (thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
            return memoryload;
        }

        public void clockcpu()
        {
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Clock && thisComputer.Hardware[i].Sensors[j].Name != "Bus Speed")
                            clockscpu.Add(thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
        }

        public string namecpu()
        {
            string name = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.CPU)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        name = (thisComputer.Hardware[i].Name);
                    }
                }
            }
            name = (name == null) ? "error" : name;
            return name;
        }

        public float? loadggpu()
        {
            float? temperature = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.GpuAti || thisComputer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        thisComputer.Hardware[i].Update();
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Load)
                            temperature = (thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
            return temperature;
        }

        public void dataggpu()
        {
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.GpuAti || thisComputer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        thisComputer.Hardware[i].Update();
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.SmallData)
                            datagpu.Add(thisComputer.Hardware[i].Sensors[j].Value);
                    }
                }
            }
        }

        public string namegpu()
        {
            string name = null;

            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.GpuAti || thisComputer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        thisComputer.Hardware[i].Update();
                        name = thisComputer.Hardware[i].Name;
                    }
                }
            }
            name = (name == null) ? "error" : name;
            return name;
        }

        public string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address.ToString();
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }

        public string GetSystemMACID()
        {
            string systemName = Dns.GetHostEntry(Dns.GetHostName()).ToString();

            try
            {
                ManagementScope theScope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
                ObjectQuery theQuery = new ObjectQuery("SELECT * FROM Win32_NetworkAdapter");
                ManagementObjectSearcher theSearcher = new ManagementObjectSearcher(theScope, theQuery);
                ManagementObjectCollection theCollectionOfResults = theSearcher.Get();

                foreach (ManagementObject theCurrentObject in theCollectionOfResults)
                {
                    if (theCurrentObject["MACAddress"] != null)
                    {
                        string macAdd = theCurrentObject["MACAddress"].ToString();
                        return macAdd.Replace(':', '-');
                    }
                }
            }

            catch (ManagementException e)
            {
                if (!Directory.Exists(path + "/dir1"))
                {
                    Directory.CreateDirectory(path + "/dir1");
                }
                fs = File.Create(path + "/dir1/" + filename);

                writer = new StreamWriter(fs);
                writer.Write(e.Message);
                writer.Close();
            }

            catch (System.UnauthorizedAccessException e)
            {
                if (!Directory.Exists(path + "/dir1"))
                {
                    Directory.CreateDirectory(path + "/dir1");
                }
                fs = File.Create(path + "/dir1/" + filename);

                writer = new StreamWriter(fs);
                writer.Write(e.Message);
                writer.Close();
            }

            return string.Empty;
        }

    }
}
