﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Utility.Exceptions;
using Utility.Helper;

using Microsoft.Extensions.Hosting;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);


            }
            catch (Exception e)
            {
                await HandleMyAllException(e,context);
            }
            
        }

        private  async Task HandleMyAllException(Exception exception,HttpContext context)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = new ErrorResponse();
            error.StatusCode = (int)code;
            error.Message = exception.Message;
            error.DeveloperMessage=exception.Message;
            if (_env.IsDevelopment())
            {
                error.DeveloperMessage = exception.StackTrace;
            }
            else
            {
                error.Message = exception.Message;
            }
            switch (exception)
            {
                case MyAppException myAppException:
                {
                    error.StatusCode =(int) HttpStatusCode.NotFound;
                    break;
                    
                }
                case UnauthorizedAccessException unauthorizedAccessException:
                    error.StatusCode = (int)HttpStatusCode.Unauthorized;
                    error.Message = "You are not authorized";
                    break;

                default:
                {
                    break;
                }
                 
                    
            }
            var result = JsonConvert.SerializeObject(error);
            context.Response.ContentType = "applicaton/json";
            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}