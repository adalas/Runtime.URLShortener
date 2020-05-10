using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Runtime.URLShortener.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Runtime.URLShortener.Infrastructure.Config;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Runtime.URLShortener.Infrastructure.Data;
using Runtime.URLShortener.Infrastructure.Logging;

namespace Runtime.URLShortener.FunctionalTests.Web
{
    public class WebTestFixture:WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

   
        }
    }
}