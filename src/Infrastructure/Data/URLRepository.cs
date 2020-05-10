using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Runtime.URLShortener.ApplicationCore.Entities;
using Runtime.URLShortener.ApplicationCore.Entities.ValueObject;
using Runtime.URLShortener.ApplicationCore.Interfaces;
using Runtime.URLShortener.ApplicationCore.Interfaces.Config;
using Runtime.URLShortener.Infrastructure.Config;
using StackExchange.Redis;

namespace Runtime.URLShortener.Infrastructure.Data {
    public class URLRepository:IURLRepository {
        private readonly IDatabase _redisDB;
        private readonly IConfigDB _config;

        public URLRepository(IRedisContext database,IOptions<ConfigDB> config)
        {
            _redisDB = database.RedisDatabase;
            _config = config.Value;

        }


        public async Task<ShortURL> AddAsync(URL url)
        {
            string key = url.Id.ToString();
            string value = url.Value;
            await _redisDB.StringSetAsync(key,value,_config.EntryTimeToLeave);
            return url.Id;
        }

        public void DeleteFireAndForget(ShortURL entity)
        {
             _redisDB.KeyDelete(entity.ToString(),flags: CommandFlags.FireAndForget);
        }

        public async Task<bool> ExistsAsync(ShortURL id)
        {
            return await _redisDB.KeyExistsAsync(id.ToString());
        }

        public async Task<URL> GetByIdAsync(ShortURL id)
        {
            string key = id.ToString();
            string value = await _redisDB.StringGetAsync(key);
            return new URL(value, id);
        }
    }
}