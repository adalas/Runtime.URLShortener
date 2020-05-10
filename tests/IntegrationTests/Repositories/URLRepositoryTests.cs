using System;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Runtime.URLShortener.Infrastructure.Data;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Microsoft.Extensions.Options;
using Runtime.URLShortener.Infrastructure.Config;
using StackExchange.Redis;
using Xunit;
using Runtime.URLShortener.UnitsTests.ApplicationCore.Domain.URLTests;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Runtime.URLShortener.ApplicationCore.Entities;

namespace Runtime.URLShortener.IntegrationTests.Repositories
{

    public class RedisConfig
    {
        public string Hostname {get; set;}
        public int Port {get; set;}
        public string Password {get; set;}
    }

    public class URLRepositoryTests {

        public IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testsettings.json", optional: false);
                    _config = builder.Build();
                }

                return _config;
            }
        }
        private IConfiguration _config;
        private readonly IURLRepository _urlRepository;
        private readonly IDatabase _database;
        public URLRepositoryTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();
            var section = Configuration.GetSection("Redis");
            services.Configure<RedisConfig>(s => {section.Bind(s);});
            services.Configure<ConfigDB>(s => {Configuration.GetSection("ConfigDB").Bind(s);});

            ServiceProvider sp = services.BuildServiceProvider();
            IOptions<RedisConfig> options = (IOptions<RedisConfig>)sp.GetService(typeof(IOptions<RedisConfig>));
            RedisConfig rc = options.Value;
            IOptions<ConfigDB> coptions = (IOptions<ConfigDB>)sp.GetService(typeof(IOptions<ConfigDB>));
            IRedisContext ctx = new RedisContext($"{rc.Hostname}:{rc.Port},password={rc.Password}");
             _urlRepository = new  URLRepository(ctx,coptions);

            _database = ctx.RedisDatabase;

        }

        [Fact]
        public async void CreateNotExists_Test() {
            _database.Execute("FLUSHDB"); //cleans db
            ShortURL surl = new ShortURL(ShortURLTest._shortURL);
            URL url = new URL(ShortURLTest._url,surl); 
            bool res = await _urlRepository.ExistsAsync(surl);
            Assert.False(res);

            await _urlRepository.AddAsync(url);
            res = await _urlRepository.ExistsAsync(surl);
            Assert.True(res);
        }
    }
}