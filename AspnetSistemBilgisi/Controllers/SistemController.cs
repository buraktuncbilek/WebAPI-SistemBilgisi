using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspnetSistemBilgisi.Controllers
{
    public class SistemController : ApiController
    {
        public async Task<object> Get()
        {
            var a = new SistemDto();
            var pc = new Microsoft.VisualBasic.Devices.ComputerInfo();
            a.RamBosta = ConvertBytesToMegabytes(pc.AvailablePhysicalMemory);
            a.RamToplam = ConvertBytesToMegabytes(pc.TotalPhysicalMemory);
            a.RamKullanilan = a.RamToplam - a.RamBosta;

            a.OS = pc.OSFullName;
            a.OsPlatform = pc.OSPlatform;
            a.OsVersion = pc.OSVersion;
            a.x64 = IntPtr.Size == 8;

            a.OsUiCulture = pc.InstalledUICulture.Name;
            a.OsUiCultureName = pc.InstalledUICulture.DisplayName;

            a.BilgisayarAdi = Environment.MachineName;
            a.Domain = Environment.UserDomainName;
            a.KullaniciAdi = Environment.UserName;
            
            var calisma = TimeSpan.FromMilliseconds(Environment.TickCount);
            a.CalismaSuresi = calisma.ToString("g");

            a.UygulamaKlasoru = AppDomain.CurrentDomain.BaseDirectory;
            a.UygulamaDiski = Path.GetPathRoot(a.UygulamaKlasoru);

            var di = System.IO.DriveInfo.GetDrives().FirstOrDefault(x => x.RootDirectory.Name == a.UygulamaDiski);
            if (!(di is null))
            {
                a.UygulamaDiskiBosAlan = ConvertMBtoGB((ulong) di.AvailableFreeSpace);
                a.UygulamaDiskiToplamAlan = ConvertBytesToMegabytes((ulong) di.TotalSize) / 1024f;
                a.UygulamaDiskiKullanilanAlan = a.UygulamaDiskiToplamAlan - a.UygulamaDiskiKullanilanAlan;
                a.UygulamaDiskiFormat = di.DriveFormat;
                a.UygulamaDiskiAdi = di.VolumeLabel;
            }

            List<string> işlemciler = new List<string>();
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                işlemciler.Add(item["Name"].ToString());
            }

            a.CPU = işlemciler.ToArray();
            a.CpuSayisi = a.CPU.Length;

            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            a.CpuCekirdekSayisi = coreCount;

            a.CpuThreadSayisi = Environment.ProcessorCount;



            return Content(HttpStatusCode.OK, a);
        }

        public class SistemDto
        {
            public double RamToplam { get; set; }
            public double RamKullanilan { get; set; }
            public double RamBosta { get; set; }

            public string OS { get; set; }
            public bool x64 { get; set; } = false;
            public string OsPlatform { get; set; }
            public string OsVersion { get; set; }
            public string OsUiCulture { get; set; }
            public string OsUiCultureName { get; set; }

            public string BilgisayarAdi { get; set; }
            public string Domain { get; set; }
            public string KullaniciAdi { get; set; }

            public string CalismaSuresi { get; set; }

            public string UygulamaKlasoru { get; set; }
            public string UygulamaDiski { get; set; }
            public double UygulamaDiskiBosAlan { get; set; }
            public double UygulamaDiskiToplamAlan { get; set; }
            public double UygulamaDiskiKullanilanAlan { get; set; }
            public string UygulamaDiskiFormat { get; set; }
            public string UygulamaDiskiAdi { get; set; }


            public string[] CPU { get; set; }
            public int CpuSayisi { get; set; }
            public int CpuCekirdekSayisi { get; set; }
            public int CpuThreadSayisi { get; set; }
        }

        double ConvertBytesToMegabytes(ulong bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        double ConvertMBtoGB(ulong MB)
        {
            return (MB / 1024f);
        }
    }
}
