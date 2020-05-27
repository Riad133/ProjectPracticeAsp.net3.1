using DLL.MongoReport.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DLL.MongoReport
{
    public class MongoDbDependency
    {
        public static  void AllDependency(IServiceCollection service)
        {
            service.AddSingleton(typeof(MongoDbContext));
            service.AddTransient<IDepartmentStudentMongoRepository, DepartmentStudentMongoRepository>();
        }
    }
}