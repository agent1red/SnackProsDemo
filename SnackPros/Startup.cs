using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SnackPros.DataAccess;
using SnackPros.DataAccess.Data.Repository.IRepository;
using SnackPros.DataAccess.Data.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using SnackPros.Utility;
using Stripe;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace SnackPros
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            //added path to access denied view 
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // added session 
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //Added stripe configuration
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));

            services.AddMvc(options => options.EnableEndpointRouting = false)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //Added Facebook login service 
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = "347586786484061";
                facebookOptions.AppSecret = "6e883594436e6e2f8fb4a81b8644da52";
            });

            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                options.ClientId = "cb4c5e36-7bf4-4cd9-915f-657a7b7cd136";
                options.ClientSecret = "i4uY12I8soVVosUuG0~b4H26y.~0oT.1zh";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // added usesession 
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc();
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];
        }
    }
}
