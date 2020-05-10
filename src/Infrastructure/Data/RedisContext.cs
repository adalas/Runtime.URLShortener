using StackExchange.Redis;

namespace Runtime.URLShortener.Infrastructure.Data
{

    public class RedisContext:IRedisContext
    {
      
        public ConnectionMultiplexer Connection {get; private set;}
        public IDatabase RedisDatabase {get { return Connection.GetDatabase();}}
        public RedisContext(string connectionString)
        {
            Connection  = ConnectionMultiplexer.Connect(connectionString);
        }
    }
}