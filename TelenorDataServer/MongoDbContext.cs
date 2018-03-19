using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace TelenorDataServer
{
    public class MongoDbContext
    {
        string _connStr;

        public MongoDbContext()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false);
            var configuration = builder.Build();
            _connStr = configuration["AppSettings:MongoDb"];
        }

        public IMongoCollection<T> GetCollection<T>(string collection) where T : class, new()
        {
            var client = new MongoClient(_connStr);
            var db = client.GetDatabase("mytos");
            return db.GetCollection<T>(collection);
        }
    }
}
