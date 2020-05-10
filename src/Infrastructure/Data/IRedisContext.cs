using StackExchange.Redis;

namespace Runtime.URLShortener.Infrastructure.Data {

    public interface IRedisContext {
        ConnectionMultiplexer Connection { get;  }
        IDatabase RedisDatabase { get ; }

    }
}