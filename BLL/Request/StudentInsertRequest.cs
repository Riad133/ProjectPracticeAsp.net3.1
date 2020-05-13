using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using DLL.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class StudentInsertRequest
    {
      
        public string Name { get; set; }
        public string Email { get; set; }
        public string RollNo { get; set; }
        public int DepartmentId { get; set; }
    }
    
    public class StudentInsertRequestValidator: AbstractValidator<StudentInsertRequest>
    {
        private IServiceProvider _serviceProvider;

        public StudentInsertRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull().MustAsync(EmailExists).WithMessage("email already exists");
            RuleFor(x => x.RollNo).NotEmpty().NotNull().MustAsync(RollNoExists).WithMessage("rollNo already exists");
            RuleFor(x => x.DepartmentId).NotEmpty().NotNull().MustAsync(DepartmentIdNoExists).WithMessage("Department Not exist");
        }

        private async Task<bool> EmailExists(string email, CancellationToken arg2)
                 {
                     if (string.IsNullOrWhiteSpace(email))
                     {
                         return false;
                     }
         
                     var StudentServiceProvider = _serviceProvider.GetRequiredService<IStudentService>();
                     return !await StudentServiceProvider.IsEmailExists(email);
                 }
        private async Task<bool> RollNoExists(string rollNo, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(rollNo))
            {
                return false;
            }

            var StudentServiceProvider = _serviceProvider.GetRequiredService<IStudentService>();
            return !await StudentServiceProvider.IsRollNoExists(rollNo);
        }
        private async Task<bool> DepartmentIdNoExists(int DepartmentId, CancellationToken arg2)
        {
            if (DepartmentId  ==0 )
            {
                return false;
            }

            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return await departmentService.IsDepartmentIdExists(DepartmentId);
        }
    }
}