using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Runtime.URLShortener.Infrastructure.Config;
using Runtime.URLShortener.Infrastructure.Data;
using Runtime.URLShortener.Infrastructure.Logging;
using Runtime.URLShortener.Web.Services;

namespace Runtime.URLShortener.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private void ConfigureRedisService(IServiceCollection services)
        {
            string redisPassword = Configuration["RedisDB:Password"];//using user-secrets. See Docker/secrets.sh
            string redisHostname = Configuration["RedisDB:Hostname"];
            string redisPort = Configuration["RedisDB:Port"];
            string connectionString = $"{redisHostname}:{redisPort},password={redisPassword}";
          
            services.AddSingleton<IRedisContext>(provider => new RedisContext(connectionString));



        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ConfigDB>(Configuration.GetSection("ConfigDB"));
            services.Configure<ConfigApplicationLimits>(Configuration.GetSection("ConfigApplicationLimits"));
            services.AddControllersWithViews();
            ConfigureRedisService(services);
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped<IURLRepository,URLRepository>();
            services.AddScoped<IURLService,URLService>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
                    // spa.UseProxyToSpaDevelopmentServer("http://urlshortner.angular.app:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
