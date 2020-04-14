using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BLL.Request;
using BLL.ViewModel;
using DLL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utility.Exceptions;

namespace BLL.Services
{
    public interface IAccountService
    {
        Task<string> LoginUser(LoginRequest request);
        Task Test(ClaimsPrincipal tt);

    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public  async Task<string> LoginUser(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw  new MyAppException("User not found");
            }

            var matchUser = await _userManager.CheckPasswordAsync(user, request.Password);
            if (matchUser == false)
            {
                throw  new MyAppException("User name or password not match");
            }

            return await GenerateJSONWebToken(user);
        }

        public Task   Test(ClaimsPrincipal tt)
        {
            var userId = tt.Claims.FirstOrDefault(x => x.Type == "Sub")?.Value;
            var role = tt.FindFirst(ClaimTypes.Role)?.Value; 
            var username = tt.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            var email = tt.FindFirst(ClaimTypes.Email)?.Value;
           throw new Exception();
        }

        private async Task<string> GenerateJSONWebToken(AppUser userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userRole = (await _userManager.GetRolesAsync(userInfo)).FirstOrDefault();
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub,userInfo.Id.ToString()),
               new Claim(CustomClaimName.UserName,userInfo.UserName),
               new Claim(CustomClaimName.Email,userInfo.Email),
               new Claim(ClaimTypes.Role,userRole)
            };
       
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],  
                _configuration["Jwt:Issuer"],  
                claims,  
                expires: DateTime.Now.AddMinutes(120),  
                signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
    }
}