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
    public class Device
    {
        public string pc { get; set; }
        public string mac_address { get; set; }
        public string ip_address { get; set; }
        public string cpu { get; set; }
        public string hdd { get; set; }
        public string gpu { get; set; }
    }
    public class Cpu
    {
        public List<float?> temp { get; set; }

        public List<float?> clock { get; set; }
    }

    public class Memory
    {
        public float? load { get; set; }
        public float? used { get; set; }
        public float? available { get; set; }
    }

    public class Hdd
    {
        public float? temp { get; set; }
    }

    public class Gpu
    {
        public float? load { get; set; }
        public float? memory_used { get; set; }
        public float? memory_free { get; set; }
    }

    public class Temperatures
    {
        public object cpu { get; set; }
        public object memory { get; set; }
        public object hdd { get; set; }
        public object gpu { get; set; }
        public object device { get; set; }
    }
    public class Temperature
    {
        Computer thisComputer;
        StreamWriter writer;
        FileStream fs;
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

        public List<float?> temperaturecpu()
        {
            List<float?> tempcpu = new List<float?>();
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
            return tempcpu;
        }

        public float? memoryused()
        {
            float? memoryused = null;
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Data && thisComputer.Hardware[i].Sensors[j].Name == "Used Memory")
                            memoryused = thisComputer.Hardware[i].Sensors[j].Value;
                    }
                }
            }
            return memoryused;
        }

        public float? memoryavaible()
        {
            float? memoryavaible = null;
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.RAM)
                {
                    thisComputer.Hardware[i].Update();
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.Data && thisComputer.Hardware[i].Sensors[j].Name == "Available Memory")
                            memoryavaible = thisComputer.Hardware[i].Sensors[j].Value;
                    }
                }
            }
            return memoryavaible;
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

        public List<float?> clockcpu()
        {
            List<float?> clockscpu = new List<float?>();
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
            return clockscpu;
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

        public float? freegpu()
        {
            float? freegpu = null;
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.GpuAti || thisComputer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        thisComputer.Hardware[i].Update();
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.SmallData && thisComputer.Hardware[i].Sensors[j].Name == "GPU Memory Free")
                            freegpu = thisComputer.Hardware[i].Sensors[j].Value;
                    }
                }
            }
            return freegpu;
        }
        public float? usedgpu()
        {
            float? usedgpu = null;
            for (int i = 0; i < thisComputer.Hardware.Length; i++)
            {
                if (thisComputer.Hardware[i].HardwareType == HardwareType.GpuAti || thisComputer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < thisComputer.Hardware[i].Sensors.Length; j++)
                    {
                        thisComputer.Hardware[i].Update();
                        if (thisComputer.Hardware[i].Sensors[j].SensorType == SensorType.SmallData && thisComputer.Hardware[i].Sensors[j].Name == "GPU Memory Used")
                            usedgpu = thisComputer.Hardware[i].Sensors[j].Value;
                    }
                }
            }
            return usedgpu;
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