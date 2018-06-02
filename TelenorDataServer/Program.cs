using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
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
            //svc.ShowSftpFiles();
            try
            {
                svc.SyncFiles();
                svc.ExtractFiles();
                svc.SaveDataAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Logger.Log("Exception:" + e.Message);
                Logger.Log(e.ToString());
            }
            Logger.WriteLogAsync().GetAwaiter().GetResult();
            Console.WriteLine("Finish");
        }
    }
}
