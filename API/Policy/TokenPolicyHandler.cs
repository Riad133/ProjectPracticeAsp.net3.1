using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;

namespace API.Policy
{
    public class TokenPolicyHandler:  AuthorizationHandler<TokenPolicy>
    {
        private readonly IDistributedCache _cache;

        public TokenPolicyHandler(IDistributedCache cache)
        {
            _cache = cache;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TokenPolicy requirement)
        {
            var userid = context.User.Claims.FirstOrDefault(x => x.Type == "userid");
            if (userid == null)
            {
                throw  new UnauthorizedAccessException();
            }

            var accessTokenKey = userid.ToString() + "_accesstoken";
          

            var cacheToken = _cache.GetString(accessTokenKey);
       
            if (cacheToken == null)
            {
                throw  new UnauthorizedAccessException();
            }
            else
            {
                 context.Succeed(requirement);
            }
            return  Task.CompletedTask;
        }
    }
}