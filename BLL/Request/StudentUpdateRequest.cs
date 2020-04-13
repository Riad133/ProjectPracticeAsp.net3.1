using System;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services;
using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace BLL.Request
{
    public class StudentUpdateRequest
    {
        public string Name { get; set; }
        public string RollNo { get; set; }
        public string Email { get; set; }
    }

    public class StudentUpdateRequestValidator : AbstractValidator<StudentUpdateRequest>
    {
        private readonly IServiceProvider _serviceProvider;


        public StudentUpdateRequestValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.RollNo).NotEmpty().NotNull();
        }
       
    }
}