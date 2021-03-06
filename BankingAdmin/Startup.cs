using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;


using BankingLib.Data;
using BankingLib.Models;
using BankingAdmin.Models.Manager;
using BankingAdmin.Models.Repository;
using BankingAdmin.Models.Json;

namespace BankingAdmin
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
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
                options.JsonSerializerOptions.Converters.Add(new BankingDateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new EnumConverter<BillPayPeriod>());
                options.JsonSerializerOptions.Converters.Add(new EnumConverter<BillPayStatus>());
                options.JsonSerializerOptions.Converters.Add(new EnumConverter<TransactionType>());
            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.AddDbContext<BankingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(nameof(BankingContext)));
                options.UseLazyLoadingProxies();
            });
            services.AddScoped<IAsyncRepository<Customer, int>, CustomerManager>();
            services.AddScoped<IAsyncSearchRepository<Transaction, TransactionQuery>, TransactionManager>();
            services.AddScoped<IAsyncRepository<Login, string>, LoginManager>();
            services.AddScoped<IAsyncRepository<BillPay, int>, BillPayManager>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
