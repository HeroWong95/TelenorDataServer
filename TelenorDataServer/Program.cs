using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TelenorDataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var svc = new TelenorService();
            //svc.SyncFiles();
            //svc.ExtractFiles();
            svc.SaveDataAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }
    }
}
