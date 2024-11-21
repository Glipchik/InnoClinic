using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Offices.Data.Entities;

namespace Offices.Data.Providers
{
    public class MongoDbContext<T> where T : BaseEntity
    {
        public readonly IMongoCollection<T> Entities;

        public MongoDbContext(IConfiguration configuration) 
        {
            var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
            var database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);

            Entities = database.GetCollection<T>(typeof(T).Name);
        }
    }
}
