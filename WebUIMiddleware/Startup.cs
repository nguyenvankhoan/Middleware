using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebUIMiddleware
{
    public class Startup
    {
        public delegate void SimpleDelegate(Logger<Startup> logger);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Run(MyMiddlewareRun);
            //app.Use(MyMiddlewareUseThenNext);
            //app.Map("/map",MyMiddlewareMapTest);
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMyCustomMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMyCustomMiddleware();

        }


        private Task MyMiddlewareRun(HttpContext httpContext)
        {
            return httpContext.Response.WriteAsync("This is my Middleware 1");
        }

        private Task MyMiddlewareUseThenNext(HttpContext httpContext, Func<Task> next)
        {
            httpContext.Response.WriteAsync("This is my Middleware Use Then Next 1234324");
            return next();
        }

        private Task MyMiddlewareRun2(HttpContext httpContext)
        {
            return httpContext.Response.WriteAsync("This is my Middleware run 2");
        }

        private static void MyMiddlewareMapTest(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("This is my Middleware running by Map");
            });
        }
  
    }
}
