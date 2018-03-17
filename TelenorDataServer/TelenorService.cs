using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TelenorDataServer
{
    class TelenorService
    {
        public TelenorService()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false);
            var configuration = builder.Build();
            Host = configuration["AppSettings:TelenorSftp:Host"];
            UserName = configuration["AppSettings:TelenorSftp:UserName"];
        }

        const string DirName = "TelenorFiles";
        const string RemotePath = "/home/mytos/data/from_fakturakontroll";

        public string Host { get; }
        public string UserName { get; }

        public void SyncFiles()
        {
            Logger.Log("Start sync files");
            var localFileNames = GetLocalFileNames();
            var remoteFileNames = GetRemoteFileNames();
            var downloadList = remoteFileNames
                .Where(f => f.CompareTo(localFileNames.FirstOrDefault()) == 1);
            var deleteList = localFileNames
                .Where(f => f.CompareTo(remoteFileNames.FirstOrDefault()) == -1);

            Console.WriteLine("downloadList:");
            foreach (var item in downloadList)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("deleteList:");
            foreach (var item in deleteList)
            {
                Console.WriteLine(item);
            }

            //if (downloadList.Any())
            //{
            //    DownloadFiles(downloadList);
            //}
            //if (deleteList.Any())
            //{
            //    DeleteFiles(deleteList);
            //}
            Logger.Log("End of sync");
        }

        public void ExtractFiles()
        {
            Logger.Log("Start decompress files");
            CommandLine.Bush("mkdir SftpFiles/mytos20171109test");
            CommandLine.Bush("tar -xvf SftpFiles/mytos20171109.tar -C SftpFiles/mytos20171109test");
            Logger.Log("Decompress completed");
        }

        private void DownloadFiles(IEnumerable<string> fileNames)
        {
            Logger.Log("Downloading files ...");
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("PrivateKey")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(RemotePath);
                foreach (var item in files)
                {
                    if (fileNames.Any(f => f == item.Name))
                    {
                        using (var stream = File.OpenWrite(DirName + "/" + item.Name))
                        {
                            sftp.DownloadFile(item.FullName, stream);
                            Logger.Log(item.Name + " 100%");
                        }
                    }
                }
            }
            Logger.Log("Download completed");
        }

        private void DeleteFiles(IEnumerable<string> fileNames)
        {
            Logger.Log("Delete old files ...");
            foreach (var item in fileNames)
            {
                File.Delete(DirName + "/" + item);
                Directory.Delete(DirName + "/" + Path.GetFileNameWithoutExtension(item));
            }
            Logger.Log("Delete completed");
        }

        private List<string> GetLocalFileNames()
        {
            var names = new List<string>();
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            var files = Directory.GetFiles(DirName);
            foreach (var item in files)
            {
                names.Add(Path.GetFileName(item));
            }
            return names;
        }

        private List<string> GetRemoteFileNames()
        {
            List<string> names = new List<string>();
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("PrivateKey")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(RemotePath);
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar") && item.Length < 1073741824)
                    {
                        names.Add(item.Name);
                    }
                }
            }
            return names;
        }
    }
}
