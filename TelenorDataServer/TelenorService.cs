using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TelenorDataServer.Impoter;
using TelenorDataServer.Models;

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
        const string ArchivePath = "/home/mytos/archive";

        public string Host { get; }
        public string UserName { get; }

        public void SyncFiles()
        {
            Logger.Log("Start sync files");
            if (!Directory.Exists(DirName))
            {
                Directory.CreateDirectory(DirName);
            }
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("mytos_rsa_key")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(RemotePath);
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar") && item.Name.CompareTo(LatestLocalFileName) == 1)
                    {
                        using (var stream = File.OpenWrite(DirName + "/" + item.Name))
                        {
                            sftp.DownloadFile(item.FullName, stream);
                            sftp.RenameFile(item.FullName, ArchivePath + "/" + item.Name);
                            Logger.Log(item.Name + " 100%");
                        }
                    }
                }
            }
            Logger.Log("End of sync");
        }

        public void ExtractFiles()
        {
            var dir = new DirectoryInfo(DirName);
            var files = dir.GetFiles();
            foreach (var item in files)
            {
                if (item.Name.StartsWith("mytos") && item.Name.EndsWith(".tar"))
                {
                    var dirName = Path.GetFileNameWithoutExtension(item.Name);
                    if (!Directory.Exists(dirName))
                    {
                        Directory.CreateDirectory(DirName + "/" + dirName);
                        CommandLine.Bush($"tar -xvf {DirName}/{item.Name} -C {DirName}/{dirName}");
                        Logger.Log($"Extract {item.Name} 100%");
                    }
                }
            }
        }

        private string LatestLocalFileName
        {
            get
            {
                var names = new List<string>();
                string latestName = Directory.GetFiles(DirName).OrderBy(f => f).LastOrDefault();
                if (string.IsNullOrEmpty(latestName))
                {
                    return string.Empty;
                }
                return Path.GetFileName(latestName);
            }
        }

        public void ShowArchiveFileNames()
        {
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("mytos_rsa_key")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(ArchivePath);
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar"))
                    {
                        Console.WriteLine(item.Name);
                    }
                }
            }
        }

        public async Task SaveDataAsync()
        {
            string[] dirs = Directory.GetDirectories(DirName);
            var db = new MongoDbContext();
            var collection = db.GetCollection<SupportLog>("support_log");
            foreach (var dir in dirs)
            {
                string dirName = Path.GetFileName(dir);
                string fileDate = dirName.Substring(5);
                var cursor = await collection.FindAsync(f => f.FileDate == fileDate);
                var logs = (await cursor.ToListAsync()).ToList();
                if (!logs.Any())
                {
                    Logger.Log($"Start import {dirName}/active_sim_card_details.csv");
                    string path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/active_sim_card_details.{fileDate}.csv";
                    var simCardImporter = new ActiveSimCardDetailImpoter(fileDate);
                    await simCardImporter.Import(path);
                    Logger.Log($"End import {dirName}/active_sim_card_details.csv");

                    Logger.Log($"Start import {dirName}/cpa_call_details.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/cpa_call_details.{fileDate}.csv";
                    var cpaDetailsImporter = new CpaCallDetailImporter(fileDate);
                    await cpaDetailsImporter.Import(path);
                    Logger.Log($"End import {dirName}/cpa_call_details.csv");

                    Logger.Log($"Start import {dirName}/customer_account_structure.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/customer_account_structure.{fileDate}.csv";
                    var casImporter = new CustomerAccountStructureImporter(fileDate);
                    await casImporter.Import(path);
                    Logger.Log($"End import {dirName}/customer_account_structure.csv");

                    Logger.Log($"Start import {dirName}/invoice_details.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/invoice_details.{fileDate}.csv";
                    var invoiceImpoter = new InvoiceDetailImporter(fileDate);
                    await invoiceImpoter.Import(path);
                    Logger.Log($"End import {dirName}/invoice_details.csv");
                }
            }
        }
    }
}
