using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        protected string DirName => "mytos" + FileDate;
        protected string FileDate { get; }

        public async Task Import(string path)
        {
            var lines = await File.ReadAllLinesAsync(path, Encoding.GetEncoding("ISO8859-1"));
            List<T> list = new List<T>();
            for (int i = 1; i < lines.Length; i++)
            {
                list.Add(Fetch(lines[i]));
            }
            var db = new MongoDbContext();
            var collection = db.GetCollection<T>(CollectionName);
            await collection.InsertManyAsync(list);
        }

        protected abstract T Fetch(string data);
    }
}
