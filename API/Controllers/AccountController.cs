using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BLL.Request;
using BLL.Response;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace API.Controllers
{
    public class AccountController : OurApplicationController
    {
        private readonly IAccountService _accountService;
        private readonly IHttpClientFactory _clientFactory;


        public AccountController(IAccountService accountService,IHttpClientFactory clientFactory)
        {
            _accountService = accountService;
            _clientFactory = clientFactory;
        }
        // GET
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {

            return Ok(await _accountService.LoginUser(request));
        } 
        [HttpPost("LoginCookei")]
        public async Task<ActionResult> LoginCookei(LoginRequest request)
        {
            var client = _clientFactory.CreateClient();

            var dict = new Dictionary<string, string>();
            // dict.Add("client_id", alreadyApplication.ApplicationId);
            // dict.Add("client_secret", alreadyApplication.ApplicationSecret);
            // dict.Add("grant_type", "client_credentials");

            var data = new LoginRequest
            {
                UserName = "teacher@gmail.com",
                Password = "Leads@1234"
            };
            var myContent = JsonConvert.SerializeObject(data);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(new Uri("https://localhost:5001/api/v1/Account/login"),
                byteContent);
            var result =
                JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
            Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions() { HttpOnly = true, Secure = false,SameSite = SameSiteMode.Strict });

            return Ok();
        }
        [HttpPost("LogOutCookei")]
        public async Task<ActionResult> LogOutCookei(LoginRequest request)
        {
            
            Response.Cookies.Append("X-Access-Token", "", new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

            return Ok();
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
        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken(string refreshToken)
        {
            
            return Ok(await _accountService.RefreshToken(refreshToken));
        }

    }
}