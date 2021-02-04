using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using testad.AuthorizationAttributes;
using Microsoft.AspNetCore.Server.IISIntegration;
using testad.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace testad
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
            services.AddControllersWithViews();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("usermsa", policy => policy.Requirements.Add(new GroupRequirement("CN=UserMSA")));
                options.AddPolicy("bossmsa", policy => policy.Requirements.Add(new GroupRequirement("CN=BossDepartmentMSA")));
                options.AddPolicy("writermsa", policy => policy.Requirements.Add(new GroupRequirement("CN=WriterMSA")));
                options.AddPolicy("adminmsa", policy => policy.Requirements.Add(new GroupRequirement("CN=AdminMSA")));
            });

            services.AddDbContext<MvcRequestContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MvcRequestContext")));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("WorksTest", policy => policy.Requirements.Add(new GroupRequirement("CN=IT מעהוכ")));
            });
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IAuthorizationHandler, GroupHandler>();
            services.AddAuthentication(IISDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Requests}/{action=Index}/{id?}");
            });
        }
    }
}
