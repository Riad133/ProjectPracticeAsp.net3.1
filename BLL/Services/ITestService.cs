using System.Threading.Tasks;
using DLL.Model;
using DLL.UnitOfWork;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public interface ITestService
    {
        Task SaveData();
    }

    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;


        public TestService(IUnitOfWork unitOfWork,UserManager<AppUser> userManager,RoleManager<AppRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task SaveData()
        {
            var user = new AppUser
             {
                 UserName = "riad.nazrul@gmail.com",
                Email ="riad.nazrul@gmail.com"
             };
             var result = await _userManager.CreateAsync(user, "Leads@1234");
             if (result.Succeeded)
             {
                 var role = await _roleManager.FindByNameAsync("customer");
                 if (role == null)
                 {
                     await _roleManager.CreateAsync(new AppRole
                     {
                         Name = "customer"
                     });
                     await _userManager.AddToRoleAsync(user, "customer");
                   
                 }
             }
            return;
        }
    }
}