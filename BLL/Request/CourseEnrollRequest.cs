using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseEnrollRequest
    {
        public string studentRollNo { get; set; }
        public string CourseCode { get; set; }
    }

    public class CourseEnrollRequestValidator : AbstractValidator<CourseEnrollRequest>
    {
        private IServiceProvider _serviceProvider;

        public CourseEnrollRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.studentRollNo).NotEmpty().NotNull().MustAsync(RollNoExists).WithMessage("Name already exist");
            RuleFor(x => x.CourseCode).NotEmpty().NotNull().MustAsync(CodeExists).WithMessage("Code already exist");

        }
        
        private async Task<bool> RollNoExists(string studentRollNo, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(studentRollNo))
            {
                return false;
            }

            var StudentServiceProvider = _serviceProvider.GetRequiredService<IStudentService>();
            return !await StudentServiceProvider.IsRollNoExists(studentRollNo);
        }
        private async Task<bool> CodeExists(string code, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }
     
            var courseService = _serviceProvider.GetRequiredService<ICourseService>();
            return !await courseService.IsCodeExists(code);
        }
    }
}