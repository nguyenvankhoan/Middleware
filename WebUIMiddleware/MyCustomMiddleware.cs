using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUIMiddleware
{
    public class MyCustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public MyCustomMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;

            _logger = logFactory.CreateLogger("MyCustomMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            

            try
            {
                string reqQs = httpContext.Request.QueryString + "";
                if (!string.IsNullOrEmpty(reqQs))
                {
                    httpContext.Request.Headers.Add("myvalue", reqQs);
                    //httpContext.Response.Headers.Add("myvalue", reqQs);
                }



                _logger.LogInformation("My Custom Middleware is executing..");

                await _next(httpContext); // calling next middleware
            }
            catch (Exception ex)
            {
                string mess = GetMessageFromExeption(ex);

                await HandleExceptionAsync(httpContext, ex);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception error)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                StatusCode = context.Response.StatusCode,
                Msg = "Internal Server Error, Please try again!"
            }));
        }

        private string GetMessageFromExeption(Exception ex)
        {
            string mess = ex.Message;
            if (ex.InnerException != null)
            {
                mess += " \n" + GetMessageFromExeption(ex.InnerException);
            }
            return mess;
        }
    }

}
