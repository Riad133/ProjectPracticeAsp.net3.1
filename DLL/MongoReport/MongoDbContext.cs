using DLL.MongoReport.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace DLL.MongoReport
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext( IConfiguration configuration)
        {
           
            var client = new MongoClient(configuration.GetValue<string>("MongoConnection:ConnectionString"));
            _database = client.GetDatabase(configuration.GetValue<string>("MongoConnection:Database"));
        }

        public IMongoCollection<DepartmentStudentReportMongo> DepartmentStudentReport =>
            _database.GetCollection<DepartmentStudentReportMongo>("DepartmentStudentReport");
        
    }
}