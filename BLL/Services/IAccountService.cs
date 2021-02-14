using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using BLL.ViewModel;
using DLL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Helper;
using Utility.Models;

namespace BLL.Services
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginUser(LoginRequest request);
        Task Test(ClaimsPrincipal tt);

        Task<SuccessResponse> LogOutUser(ClaimsPrincipal tt);
        Task<LoginResponse> RefreshToken(string refreshToken);
    }

    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TaposRSA _taposRsa;
        private readonly IDistributedCache _cache;

        public AccountService(UserManager<AppUser> userManager, IConfiguration configuration,TaposRSA taposRsa, IDistributedCache cache)
        {
            _userManager = userManager;
            _configuration = configuration;
            _taposRsa = taposRsa;
            _cache = cache;
        }
        public  async Task<LoginResponse> LoginUser(LoginRequest request)
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

        public async Task<SuccessResponse> LogOutUser(ClaimsPrincipal tt)
        {
            var userId = tt.FindFirst(c => c.Type == "userid")?.Value;

            var accessTokenKey = userId + "_acesstoken";
            var refreshTokenKey = userId + "_refreshtoken";

            await _cache.RemoveAsync(accessTokenKey);
            await _cache.RemoveAsync(refreshTokenKey);

            return new SuccessResponse()
            {
                Message = "logout successfully",
                statusCode = 200
            };

        }

        private async Task<LoginResponse> GenerateJSONWebToken(AppUser userInfo)  
        {  
            var response =new LoginResponse();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userRole = (await _userManager.GetRolesAsync(userInfo)).FirstOrDefault();
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Sub,userInfo.Id.ToString()),
               new Claim(CustomClaimName.UserName,userInfo.UserName),
               new Claim(CustomClaimName.Email,userInfo.Email),
               new Claim(CustomClaimName.UserId,userInfo.Id.ToString()),
               new Claim(ClaimTypes.Role,userRole)
               
            };
            var time = _configuration.GetValue<int>("Jwt:AccessTokenLifeTime");
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],  
                _configuration["Jwt:Issuer"],  
                claims,  
                expires: DateTime.Now.AddMinutes(time),  
                signingCredentials: credentials);  
            
            var  refreshToken = new RefreshTokenResponse
            {
                UserId = userInfo.Id,
                Id= Guid.NewGuid().ToString()
                
                
            };
            var resData = _taposRsa.EncryptData(JsonConvert.SerializeObject(refreshToken), "v1");
            
            response.Token= new JwtSecurityTokenHandler().WriteToken(token);
            response.Expire = time * 60;
            response.RefreshToken = resData;
          //await  StoreTokenInformation(userInfo.Id, response.Token, response.RefreshToken);
            return response;

        }
        private async Task StoreTokenInformation(long userId, string accessToken, string refreshToken)
        {

            var accessTokenOptions = new DistributedCacheEntryOptions()
          .SetSlidingExpiration(TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:AccessTokenLifeTime")));

            var refreshTokenOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:RefreshTokenLifeTime")));

            var accessTokenKey = userId.ToString() + "_acesstoken";
            var refreshTokenKey = userId.ToString() + "_refreshtoken";

            await _cache.SetStringAsync(accessTokenKey, accessToken, accessTokenOptions);
            await _cache.SetStringAsync(refreshTokenKey, refreshToken, refreshTokenOptions);

        }

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            var decryptRsa = _taposRsa.Decrypt(refreshToken, "v1");
            if (decryptRsa == null)
            {
                throw new MyAppException("Refresh Token not found");
            }
            var refreshtokenObject = JsonConvert.DeserializeObject<RefreshTokenResponse>(decryptRsa);
            var refreshTokenKey = refreshtokenObject.UserId.ToString() + "_refreshtoken";
            var cacheData = await _cache.GetStringAsync(refreshTokenKey);

            if (cacheData == null)
            {
                throw new MyAppException("Refresh Token not found");
            }

            if (cacheData != refreshToken)
            {
                throw new MyAppException("Refresh Token not found");
            }
            var user = await _userManager.FindByIdAsync(refreshtokenObject.UserId.ToString());
            return await GenerateJSONWebToken(user);

        }
    }
}