using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HappyDog.Infrastructure.Extensions;

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
                if (!Directory.Exists("SftpFiles"))
                {
                    Directory.CreateDirectory("SftpFiles");
                }
                var dir = new DirectoryInfo("SftpFiles");
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar"))
                    {
                        var localFiles = dir.GetFiles();
                        if (!localFiles.Any(f => f.Name == item.Name))
                        {
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

        public void Decompress()
        {
            foreach (var item in DownloadedFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(item);
                ("mkdir SftpFiles/" + fileName).Bash();
                ("tar -xvf SftpFiles/" + fileName + ".tar -C SftpFiles/" + fileName).Bash();
            }
            Console.WriteLine("Decompress Successful");
        }
    }
}
