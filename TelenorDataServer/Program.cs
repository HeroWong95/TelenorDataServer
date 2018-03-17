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
            //svc.ExtractFiles();
            //Console.WriteLine("running");
            //File.Create(DateTime.Now.ToString("yyyy-MM-dd.txt"))
            svc.SyncFiles();
            Console.ReadKey();
        }
    }
}
