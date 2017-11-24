using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TelenorDataServer
{
    class TelenorFileProvider
    {
        public TelenorFileProvider()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false);
            var configuration = builder.Build();
            Host = configuration["AppSettings:Sftp:Host"];
            UserName = configuration["AppSettings:Sftp:UserName"];
            DownloadedFiles = new List<string>();
        }


        public string Host { get; }
        public string UserName { get; }
        public List<string> DownloadedFiles { get; }

        public void DownloadFile()
        {
            Console.WriteLine("Downloading Files ...");
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("PrivateKey")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory("/");
                var dir = new DirectoryInfo("SftpFiles");
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar"))
                    {
                        var localFiles = dir.GetFiles();
                        if (!localFiles.Any(f => f.Name == item.Name))
                        {
                            //#if Debug
                            //                            Console.WriteLine("SftpFiles\\" + item.Name);
                            //                            using (var stream = File.OpenWrite("SftpFiles\\" + item.Name))
                            //#else
                            //                            Console.WriteLine("SftpFiles/" + item.Name);
                            //                            using (var stream = File.OpenWrite("SftpFiles/" + item.Name))
                            //#endif
                            //                            {
                            //                                sftp.DownloadFile(item.FullName, stream);
                            //#if Debug
                            //                                DownloadedFiles.Add("SftpFiles\\" + item.Name);
                            //#else
                            //                                DownloadedFiles.Add("SftpFiles/" + item.Name);
                            //#endif

                            Console.WriteLine("SftpFiles/" + item.Name);
                            using (var stream = File.OpenWrite("SftpFiles/" + item.Name))
                            {
                                sftp.DownloadFile(item.FullName, stream);
                                DownloadedFiles.Add("SftpFiles/" + item.Name);
                            }
                        }
                    }
                }
            }
            Console.WriteLine("Download Successful");
        }

        public async Task DecompressAsync()
        {
            foreach (var item in DownloadedFiles)
            {
                using (var stream = File.OpenRead(item))
                using (var decompressFolderStream = File.Create("SftpFiles/" + Path.GetFileNameWithoutExtension(item)))
                using (var decompressStream = new GZipStream(stream, CompressionMode.Decompress))
                {
                    await decompressStream.CopyToAsync(decompressFolderStream);
                }
            }
        }
    }
}
