using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> işlemciler = new List<string>();
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                Console.WriteLine("işlemci: {0} ", item["Name"]);
                işlemciler.Add(item["Name"].ToString());
            }

            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            Console.WriteLine("Number Of Cores: {0}", coreCount);

            Console.WriteLine("Number Of Logical Processors - thread sayısı -: {0}", Environment.ProcessorCount);


            Console.ReadLine();
        }
    }
}
