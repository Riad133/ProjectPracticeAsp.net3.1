using BLL.Request;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    public class BLLDependency
    {
        public static void AllDependency(IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
                options.InstanceName = "PracticseDb";
            });
            services.AddTransient<IDepartmentService,DepartmentService>();
            
            services.AddTransient<IStudentService,StudentService>();
            services.AddTransient<ITestService,TestService>();
            services.AddTransient<IAccountService, AccountService>();
           
            AllValidationDependency(services);

        }

        private static void AllValidationDependency(IServiceCollection services)
        {
            services.AddTransient<IValidator<DepartmentInsertRequest>, DepartmentInsertRequestValidator>();
            services.AddTransient<IValidator<StudentInsertRequest>,StudentInsertRequestValidator>();
            services.AddTransient<IValidator<StudentUpdateRequest>,StudentUpdateRequestValidator>();
        }
    }
}