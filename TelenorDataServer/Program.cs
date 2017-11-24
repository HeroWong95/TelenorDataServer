using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Threading.Tasks;

namespace TelenorDataServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var provider = new TelenorFileProvider();
            provider.DownloadFile();
            //provider.DownloadedFiles.Add("SftpFiles\\mytos20171109.tar");
            provider.DecompressAsync().GetAwaiter().GetResult();
            Console.WriteLine("Program Execution Complete");
            Console.ReadKey();
        }
    }
}
