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

            services.AddAutoMapper();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IPhotoRepository, PhotoRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
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
