using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingStore.Data;
using ShoppingStore.Models;
using ShoppingStore.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using ShoppingStore.Data.Repositories;
using AspNetCore.JsonLocalization;
using Microsoft.AspNetCore.Routing.Constraints;
using ShoppingStore.Models.LocalizedViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingStore
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<LocalizedIdentityErrorDescriber>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(50);
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddAutoMapper();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



            services.AddJsonLocalization();

            services.AddMvc()
                .AddViewLocalization(
                LanguageViewLocationExpanderFormat.Suffix,
                opt =>
                {
                    opt.ResourcesPath = "Resources";
                })
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(opts =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-GB"),
                    new CultureInfo("en-US"),
                    new CultureInfo("zh-TW"),
                };

                opts.DefaultRequestCulture = new RequestCulture("zh-TW");
                opts.SupportedCultures = supportedCultures;
                opts.SupportedUICultures = supportedCultures;
            });

            services.AddMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthentication().AddGoogle(options =>
            {
                //options.ClientId = "563822801624-28sr5rr9ngiqe7bcjeg8qmn09khr8fr8.apps.googleusercontent.com";
                //options.ClientSecret = "dbdYz3vxhOeiR4Ngf3L_0EAJ";
                options.ClientId = Configuration["Google:ClientId"];
                options.ClientSecret = Configuration["Google:ClientSecret"];
            });

            services.AddAuthentication().AddFacebook(options =>
            {
                //options.ClientId = "643933675803261";
                //options.ClientSecret = "dbb9afb0f34ebb6cfdbb45d4a6965d1b";
                options.ClientId = Configuration["Facebook:ClientId"];
                options.ClientSecret = Configuration["Facebook:ClientSecret"];
            });

            services.AddAuthentication().AddTwitter(options =>
            {
                //options.ConsumerKey = "gFs2HG3vkCYhVjF466VYVZ76o";
                //options.ConsumerSecret = "d1wQvKF8BOQ8Uy17TEpjxXY8DQPSdrmsPFBc04m8Rnsz7AL6Qi";
                options.ConsumerKey = Configuration["Twitter:ConsumerKey"];
                options.ConsumerSecret = Configuration["Twitter:ConsumerSecret"];
            });

            services.AddAuthentication().AddMicrosoftAccount(options =>
            {
                //options.ClientId = "17637aad-7a0a-4b5f-a261-87728f635945";
                //options.ClientSecret = "pyjKR44_ooapJLHUH748%&<";
                options.ClientId = Configuration["Microsoft:ClientId"];
                options.ClientSecret = Configuration["Microsoft:ClientSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            var options = app.ApplicationServices
                           .GetService<IOptions<RequestLocalizationOptions>>();

            app.UseRequestLocalization(options.Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "pagination",
                    template: "Products/Page{page}",
                    defaults: new
                    {
                        controller = "Product",
                        action = "Index"
                    });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
