using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor;
using Newtonsoft.Json;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics.Contracts;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            AssemblyRead read = new AssemblyRead();
            Temperature temperature = new Temperature();
            var temp = new Temperatures
            {
                cpu = new Cpu
                {
                    temp = temperature.temperaturecpu(),
                    clock = temperature.clockcpu()
                },

                memory = new Memory
                {
                    load = temperature.memorylload(),
                    used = temperature.memoryused(),
                    available = temperature.memoryavaible()
                },

                hdd = new Hdd
                {
                    temp = temperature.temperaturehdd()
                },

                gpu = new Gpu
                {
                    load = temperature.loadggpu(),
                    memory_used = temperature.usedgpu(),
                    memory_free = temperature.freegpu()
                },
                pc = temperature.compname(),
                mac_address = temperature.GetSystemMACID(),
                ip_address = temperature.GetLocalIpAddress(),
                cpu_name = temperature.namecpu(),
                hdd_name = temperature.namehdd(),
                gpu_name = temperature.namegpu()
            };

            string fileName = Path.Combine(Environment.CurrentDirectory, "info.json");
            File.WriteAllText(fileName, JsonConvert.SerializeObject(temp));
            var res = JsonConvert.DeserializeObject(File.ReadAllText(fileName));
            Console.WriteLine(res);
        }
    }
}
