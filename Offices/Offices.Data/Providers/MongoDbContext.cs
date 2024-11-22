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
    public class MongoDbContext
    {
        public readonly IMongoDatabase Database;

        public MongoDbContext(IConfiguration configuration) 
        {
            var mongoClient = new MongoClient(configuration["MongoDB:ConnectionString"]);
            Database = mongoClient.GetDatabase(configuration["MongoDB:DatabaseName"]);
        }
    }
}
