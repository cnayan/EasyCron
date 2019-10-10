using EasyCron.Api.DbContextes;
using EasyCron.Api.Implements;
using EasyCron.Api.Interfaces;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Bayantu.Evos.Services.CronJob.Api
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddHttpClient("http_client")
                .ConfigurePrimaryHttpMessageHandler(
                    c => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true
                    });

            var connStr = Configuration["ConnectionString"];
            services.AddHangfire(config =>
            {
                config.UseStorage(new MySqlStorage(connStr));
            }
            );

            services.AddDbContext<CronJobDbContext>(_ => _.UseMySql(connStr));
            services.AddDbContext<HangfireDbContext>(_ => _.UseMySql(connStr));

            services.AddTransient<ICronJobService, CronJobService>();
            services.AddSingleton<ICronTranslator,CronTranslator>();
            services.AddTransient<IJobWorker, JobWorker>();

            services.AddMvc(_ =>
            {
                _.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = Environment.ProcessorCount * 10
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() }
            });

            app.UseMvc();
        }

        public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
            }
        }
    }
}
