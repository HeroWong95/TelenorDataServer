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
            //try
            //{
            //    svc.SyncFiles();
            //    svc.ExtractFiles();
            //    svc.SaveDataAsync().GetAwaiter().GetResult();
            //}
            //catch (Exception e)
            //{
            //    Logger.Log("Exception:" + e.Message);
            //    Logger.Log(e.ToString());
            //}
            //Logger.WriteLogAsync().GetAwaiter().GetResult();

            //var importer = new Impoter.InvoiceDetailImporter("20181109");
            //string path = @"C:\Users\hero_\Documents\Work\ImportFiles\mytos20181109\data\dwm1\pm\MYTOS\data\invoice_details.20171109.csv";
            //importer.Import(path).GetAwaiter().GetResult();

            var db = new MongoDbContext();
            var collection = db.GetCollection<Models.SupportLog>("support_log_test");
            var update = MongoDB.Driver.Builders<Models.SupportLog>.Update
                .Set("end_time", DateTime.Now)
                .Set("insert_lines", 222);
            var log = collection.FindOneAndUpdateAsync(l => l.FileDate == "20181109" && l.FileName == "invoice_details_test", update).GetAwaiter().GetResult();
            
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
