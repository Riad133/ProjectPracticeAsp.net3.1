using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using DLL.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class DepartmentInsertRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class DepartmentInsertRequestValidator : AbstractValidator<DepartmentInsertRequest>
    {
        private IServiceProvider _serviceProvider;

        public DepartmentInsertRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotEmpty().NotNull().MinimumLength(4).MustAsync(NameExist).WithMessage("Name already exist");
            RuleFor(x => x.Code).NotEmpty().NotNull().MinimumLength(3).MaximumLength(12).MustAsync(CodeExists).WithMessage("Code already exist");
            
        }

        private async Task<bool> CodeExists(string code, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return !await departmentService.IsCodeExists(code);
        }

        private async Task<bool> NameExist(string name, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            var departmentService = _serviceProvider.GetRequiredService<IDepartmentService>();
            return !await departmentService.IsNameExists(name);
        }
    }
}