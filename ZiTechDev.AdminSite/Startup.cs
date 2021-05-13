using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using ZiTechDev.AdminSite.ApiClientServices.Auth;
using ZiTechDev.AdminSite.ApiClientServices.User;
using ZiTechDev.Business.Requests.Activity;
using ZiTechDev.Business.Requests.Auth;
using ZiTechDev.Business.Requests.User;
using ZiTechDev.Business.Services.Activities;
using ZiTechDev.Business.Services.Auth;
using ZiTechDev.Business.Services.User;
using ZiTechDev.Business.Validations.Activity;
using ZiTechDev.Business.Validations.Auth;
using ZiTechDev.Business.Validations.User;
using ZiTechDev.Common.Constants;
using ZiTechDev.Data.Context;
using ZiTechDev.Data.Entities;

namespace ZiTechDev.AdminSite
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
            services.AddHttpClient("zitechdev", x => {
                x.BaseAddress = new Uri("https://localhost:5001/");
            });

            services.AddTransient<IAuthApiClient, AuthApiClient>();
            services.AddTransient<IUserApiClient, UserApiClient>();

            services.AddSession(x=> {
                x.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => {
                x.LoginPath = "/Auth/Login/";
                x.AccessDeniedPath = "/Auth/Forbidden/";
            }); 
            
            services.AddControllersWithViews().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<LoginValidator>());
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

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
