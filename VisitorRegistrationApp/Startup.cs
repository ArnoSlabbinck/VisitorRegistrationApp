using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using VisitorRegistrationApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using VisitorRegistrationApp.Data.Repository;
using Model;
using BL.Services;
using DAL.Repositories;
using BLL.Validators;
using FluentValidation;
using VisitorRegistrationApp.Data.Entities;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using VisitorRegistrationApp.Data.Helper;

namespace VisitorRegistrationApp
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
            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews(options => options.Filters.Add(new AuthorizeFilter()));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddScoped<IVisitorService, VisitorService>();

            services.AddScoped<IEmployeeService, EmployeeService>();

            services.AddScoped<IImageRespository, ImageRepository>();

            services.AddScoped<ICompanyService, CompanyService>();

            services.AddScoped<ICompanyRespository, CompanyRepository>();

            services.AddScoped<IEmployeeRespository, EmployeeRepository>();

            services.AddScoped<IVisitorRepository, VisitorRespository>();

            services.AddScoped<IValidator<Company>, CompanyValidator>();

            services.AddScoped<IValidator<Employee>, EmployeeValidator>();

            services.AddScoped<IValidator<ApplicationUser>, VisitorValidator>();

            services.AddScoped<IValidator<Image>, ImageValidator>();

            services.AddTransient<IEmailSender, EmailSender>();




            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsFactory>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                         // services.AddDefaultIdentity<IdentityUser>()
                         .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>()
                         .AddEntityFrameworkStores<ApplicationDbContext>()
                         .AddDefaultTokenProviders();



            services.AddSession();

            services.AddMemoryCache();




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
            DefaultFilesOptions options = new DefaultFilesOptions();
            options.DefaultFileNames.Clear();
            options.DefaultFileNames.Add("Register.cshtml");
            app.UseDefaultFiles(options);
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();
        }
    }
}
