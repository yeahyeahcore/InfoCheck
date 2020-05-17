using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor;
using Newtonsoft.Json;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = Path.Combine(Environment.CurrentDirectory, "info.json");
            AssemblyRead read = new AssemblyRead();
            Temperature temp = new Temperature();
            File.WriteAllText(fileName, JsonConvert.SerializeObject(temp));
            var res = JsonConvert.DeserializeObject(File.ReadAllText(fileName));
            Console.WriteLine(res);
        }
    }
}
