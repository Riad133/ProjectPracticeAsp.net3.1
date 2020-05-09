using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Middleware;
using API.Policy;
using BLL;
using DLL;
using DLL.DbContext;
using DLL.Model;
using DLL.Repository;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Utility;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this part start for API documentation 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
            //add for API versioning 
            
            services.AddMvcCore();
            services.AddApiVersioning(config =>
            {
                // Specify the default API Version as 1.0
                config.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number 
                config.AssumeDefaultVersionWhenUnspecified = true;
            });
          
            //this part end for API documentation 
            services.AddDbContext<ApplicationDbContext>(services.AddControllers().AddFluentValidation().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
                                                                    options =>
                options.UseSqlServer(Configuration.GetConnectionString("myDbConnection")));
            GetAllDependency(services);
            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // JWT SetUp
            JwTConfigatration(services);
            
            // JWT SetUp End
            services.AddAuthorization(o =>
            {
                o.AddPolicy("FirstStepCompleted", policy => policy.RequireClaim("FirstStepCompleted"));
                o.AddPolicy("Authorized", policy => policy.RequireClaim("Authorized"));
            });


        }

        private void JwTConfigatration(IServiceCollection services)
        {
           services.AddAuthentication(x =>
               {
                   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })  
               .AddJwtBearer(options =>  
               {  
                   options.TokenValidationParameters = new TokenValidationParameters  
                   {  
                       ValidateIssuer = true,  
                       ValidateAudience = true,  
                       ValidateLifetime = true,  
                       ValidateIssuerSigningKey = true,  
                       ValidIssuer = Configuration["Jwt:Issuer"],  
                       ValidAudience = Configuration["Jwt:Issuer"],  
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))  
                   };  
               });
           services.AddAuthorization(options => options.AddPolicy("AtToken",Policy=> Policy.Requirements.Add(new TokenPolicy())));
        }

        private void GetAllDependency(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler,TokenPolicyHandler>();
            DLLDependency.ALLDependency(services);
            BLLDependency.AllDependency(services);
            UtilityDependency.AllDependency(services);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //this part start for API documentation 
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseSwagger();
            

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            //this part end for API documentation 
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
           
            app.UseRouting();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

     
    }
}