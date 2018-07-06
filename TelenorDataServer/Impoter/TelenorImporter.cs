using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TelenorDataServer.Models;
using MongoDB.Driver;
using System.Linq;

namespace TelenorDataServer.Impoter
{
    abstract class TelenorImporter<T> where T : class, new()
    {
        public TelenorImporter(string fileDate)
        {
            if (fileDate != null && fileDate.Length == 8)
            {
                FileDate = fileDate;
            }
            else
            {
                throw new ArgumentException("fileDate format must be yyyyMMdd");
            }
        }

        protected abstract string CollectionName { get; }
        protected string FileDate { get; }

        int actualLines = 0;

        public async Task Import(string path)
        {
            if (File.Exists(path))
            {
                var lines = await File.ReadAllLinesAsync(path, Encoding.GetEncoding("ISO8859-1"));
                List<T> list = new List<T>();
                for (int i = 1; i < lines.Length; i++)
                {
                    try
                    {
                        list.Add(Fetch(lines[i]));
                        actualLines++;
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Logger.Log(e.Message);
                        Logger.Log("Ingore Error Data: " + lines[i]);
                    }
                }
                var db = new MongoDbContext();
                var collection = db.GetCollection<T>(CollectionName);
                await OnImportingAsync(list);
                if (list.Any())
                {
                    await collection.InsertManyAsync(list);
                }
                await OnImportedAsync(list);
            }
            else
            {
                Logger.Log("file does not exists path:" + path);
            }
        }

        protected abstract T Fetch(string data);

        protected async virtual Task OnImportingAsync(List<T> data)
        {
            var db = new MongoDbContext();
            var collection = db.GetCollection<SupportLog>("support_log");
            await collection.InsertOneAsync(new SupportLog
            {
                FileDate = FileDate,
                StartTime = DateTime.Now,
                SourceLine = data.Count,
                FileName = CollectionName
            });
        }

        protected async Task OnImportedAsync(List<T> data)
        {
            var db = new MongoDbContext();
            var collection = db.GetCollection<SupportLog>("support_log");
            var update = Builders<SupportLog>.Update
                .Set("end_time", DateTime.Now)
                .Set("insert_lines", actualLines);
            await collection.FindOneAndUpdateAsync(l => l.FileDate == FileDate && l.FileName == CollectionName, update);
        }
    }
}
