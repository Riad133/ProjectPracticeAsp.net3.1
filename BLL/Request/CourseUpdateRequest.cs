using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class CourseUpdateRequest
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class CourseUpdateRequestValidator : AbstractValidator<CourseUpdateRequest>
    {
        private readonly IServiceProvider _serviceProvider;


        public CourseUpdateRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotEmpty().NotNull().MustAsync(NameExist).WithMessage("Name already exist");
           
        }
        private async Task<bool> NameExist(string name, CancellationToken arg2)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
     
            var courseService = _serviceProvider.GetRequiredService<ICourseService>();
            return !await courseService.IsNameExists(name);
        }
    }
}