using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.MongoReport.Model;
using MongoDB.Driver;

namespace DLL.MongoReport.Repositories
{
    public interface IDepartmentStudentMongoRepository
    {
        Task<DepartmentStudentReportMongo> Create(DepartmentStudentReportMongo departmentStudentReportMongo);
        Task<List<DepartmentStudentReportMongo>> GetAll(DepartmentStudentReportMongo departmentStudentReportMongo);
    }
    public class DepartmentStudentMongoRepository:IDepartmentStudentMongoRepository
    {
        private readonly MongoDbContext _context;

        public DepartmentStudentMongoRepository(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<DepartmentStudentReportMongo> Create(DepartmentStudentReportMongo departmentStudentReportMongo)
        {
            await _context.DepartmentStudentReport.InsertOneAsync(departmentStudentReportMongo);
            return departmentStudentReportMongo;
        }

        public async Task<List<DepartmentStudentReportMongo>> GetAll(DepartmentStudentReportMongo departmentStudentReportMongo)
        {
            var filterBuilder = Builders<DepartmentStudentReportMongo>.Filter;
            var filter = filterBuilder.Empty;
            var sort = Builders<DepartmentStudentReportMongo>.Sort.Descending("_id");
            return await _context.DepartmentStudentReport.Find(filter).Sort(sort).ToListAsync();
        }
    }
}