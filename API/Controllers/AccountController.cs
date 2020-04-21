using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    public class AccountController : OurApplicationController
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        // GET
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {

            return Ok(await _accountService.LoginUser(request));
        }
      [HttpGet("test1")]
      [Authorize(Policy = "AtToken")]
        public ActionResult Test1()
        {
            return Ok("Test1");
        }
        
        [HttpGet("test2")]
        
        [Authorize(Roles = "customer", Policy = "AtToken")]
        
        public   ActionResult Test2()
        {
          
           
            return Ok("Test2");
        }
        [HttpPost("logout")]
        public async Task<ActionResult> LoginOut()
        {
            var tt = User;
            return Ok(await _accountService.LogOutUser(tt));
        }

    }
}