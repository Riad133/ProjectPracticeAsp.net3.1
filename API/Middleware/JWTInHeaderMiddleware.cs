using System.Threading.Tasks;
using BLL.Response;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace API.Middleware
{
    public class JWTInHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTInHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var authenticationCookieName = "X-Access-Token";
            var cookie = context.Request.Cookies[authenticationCookieName];
            if (cookie != null)
            {
                context.Request.Headers.Add("Authorization", "Bearer " + cookie);
             //  context.Request.Headers.Append("Authorization", "Bearer " + cookie);
            }

            if (context.Request.Path.ToString().ToLower().Contains("/account/logout"))
            {
                
            }

            await _next.Invoke(context);
        }
    }
    public static class JwtCookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookie(this IApplicationBuilder build)
        {
            return build.UseMiddleware<JWTInHeaderMiddleware>();
        }
    }
}