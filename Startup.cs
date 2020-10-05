using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Reflection;

namespace DutchTreat
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddIdentity<StoreUser, IdentityRole>(cfg=>
            {
                cfg.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DutchContext>();

            services.AddDbContext<DutchContext>(config=> 
            config.UseSqlServer(_config.GetConnectionString("DutchConnectionString")));

            services.AddScoped<IDutchRepository, DutchRepository>();
            services.AddTransient<DutchSeeder>();
            services.AddTransient<INullMailService, NullMailService>();
            services.AddControllersWithViews();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMvc()
                
                .AddNewtonsoftJson(options=>
                options.SerializerSettings.ReferenceLoopHandling= ReferenceLoopHandling.Ignore);
                
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            app.UseStaticFiles();
           
            app.UseNodeModules();
            app.UseAuthentication();
           

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(cfg => 
            {
                cfg.MapControllerRoute("Fallback",
                    "{controller}/{action}/{id?}",
                    new { controller = "App", action = "Index" });
            });
        }
    }
}
