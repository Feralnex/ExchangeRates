using DataAccess;
using ExchangeRates.Middlewares;
using ExchangeRates.Options;
using ExchangeRates.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Networking.Text;
using System.Collections.Generic;
using System.Globalization;

namespace ExchangeRates
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
            IConfigurationSection endPoints = Configuration.GetSection("EndPoints");

            services.AddSingleton<IDownloader, JsonDownloader>();
            services.AddDbContext<ExchangeRatesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<RequestLocalizationCookiesMiddleware>();
            services.Configure<EndPoints>(endPoints);

            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });
            services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("pl-PL")
                };
                options.DefaultRequestCulture = new RequestCulture("pl-PL");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.FallBackToParentUICultures = true;
                options.RequestCultureProviders.Remove(typeof(AcceptLanguageHeaderRequestCultureProvider));
            });

            services
                .AddRazorPages()
                .AddViewLocalization();
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
            app.UseRouting();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseRequestLocalizationCookies();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
