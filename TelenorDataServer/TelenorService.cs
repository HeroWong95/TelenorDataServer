﻿using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelenorDataServer.Impoter;
using TelenorDataServer.Models;

namespace TelenorDataServer
{
    public class TelenorService
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
            Regex reg = new Regex(@"^mytos\d{8}.tar$");
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("mytos_rsa_key")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(RemotePath);
                var localDir = new DirectoryInfo(DirName);
                var localFiles = localDir.GetFiles();
                foreach (var item in files)
                {
                    if (reg.IsMatch(item.Name)
                        && localFiles.All(f => f.Name != item.Name)
                        && item.Length < 157286400)
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
                    if (!Directory.Exists(DirName + "/" + dirName))
                    {
                        Directory.CreateDirectory(DirName + "/" + dirName);
                        CommandLine.Bush($"tar -xvf {DirName}/{item.Name} -C {DirName}/{dirName}");
                        Logger.Log($"Extract {item.Name} 100%");
                    }
                }
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
                        Console.WriteLine(item.Name + "\t" + item.Length);
                    }
                }
            }
        }

        public void ShowSftpFiles()
        {
            using (var sftp = new SftpClient(Host, UserName, new PrivateKeyFile("mytos_rsa_key")))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(RemotePath);
                foreach (var item in files)
                {
                    if (item.Name.EndsWith(".tar"))
                    {
                        Console.WriteLine(item.Name + "\t" + item.Length);
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
                string path = null;
                if (!logs.Any(l => l.FileName == "active_sim_card_details"))
                {
                    Logger.Log($"Start import {dirName}/active_sim_card_details.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/active_sim_card_details.{fileDate}.csv";
                    var simCardImporter = new ActiveSimCardDetailImpoter(fileDate);
                    await simCardImporter.Import(path);
                    Logger.Log($"End import {dirName}/active_sim_card_details.csv");
                }
                if (!logs.Any(l => l.FileName == "cpa_call_details"))
                {
                    Logger.Log($"Start import {dirName}/cpa_call_details.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/cpa_call_details.{fileDate}.csv";
                    var cpaDetailsImporter = new CpaCallDetailImporter(fileDate);
                    await cpaDetailsImporter.Import(path);
                    Logger.Log($"End import {dirName}/cpa_call_details.csv");
                }
                if (!logs.Any(l => l.FileName == "customer_account_structure"))
                {
                    Logger.Log($"Start import {dirName}/customer_account_structure.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/customer_account_structure.{fileDate}.csv";
                    var casImporter = new CustomerAccountStructureImporter(fileDate);
                    await casImporter.Import(path);
                    Logger.Log($"End import {dirName}/customer_account_structure.csv");
                }
                if (!logs.Any(l => l.FileName == "invoice_details"))
                {
                    Logger.Log($"Start import {dirName}/invoice_details.csv");
                    path = $"{DirName}/{dirName}/data/dwm1/pm/MYTOS/data/invoice_details.{fileDate}.csv";
                    var invoiceImpoter = new InvoiceDetailImporter(fileDate);
                    await invoiceImpoter.Import(path);
                    Logger.Log($"End import {dirName}/invoice_details.csv");
                }
            }
        }

        public async Task DeleteDataAsync()
        {
            Logger.Log("delete data for GDPR");

            var db = new MongoDbContext();
            string fileDate = DateTime.Now.AddYears(-2).ToString("yyyyMMdd");

            var filter1 = Builders<SupportLog>.Filter.Lte(l => l.FileDate, fileDate);
            Logger.Log("start delete support_log where file date <= " + fileDate);
            var csp = db.GetCollection<SupportLog>("support_log");
            await csp.DeleteManyAsync(filter1);

            var filter2 = Builders<ActiveSimCardDetail>.Filter.Lte(l => l.FileDate, fileDate);
            Logger.Log("start delete active_sim_card_details where file date <= " + fileDate);
            var casd = db.GetCollection<ActiveSimCardDetail>("active_sim_card_details");
            await casd.DeleteManyAsync(filter2);

            var filter3 = Builders<CpaCallDetail>.Filter.Lte(l => l.FileDate, fileDate);
            Logger.Log("start delete cpa_call_details where file date <= " + fileDate);
            var cccd = db.GetCollection<CpaCallDetail>("cpa_call_details");
            await cccd.DeleteManyAsync(filter3);

            var filter4 = Builders<CustomerAccountStructure>.Filter.Lte(l => l.FileDate, fileDate);
            Logger.Log("start delete customer_account_structure where file date <= " + fileDate);
            var ccas = db.GetCollection<CustomerAccountStructure>("customer_account_structure");
            await ccas.DeleteManyAsync(filter4);

            var filter5 = Builders<InvoiceDetail>.Filter.Lte(l => l.FileDate, fileDate);
            Logger.Log("start delete invoice_details where file date <= " + fileDate);
            var cid = db.GetCollection<InvoiceDetail>("invoice_details");
            await cid.DeleteManyAsync(filter5);
        }
    }
}
