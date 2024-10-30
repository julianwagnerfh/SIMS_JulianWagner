using StackExchange.Redis;

namespace SIMSAPI
{
    public class RedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;

        public RedisService(string redisConnectionString)
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        }

        public IDatabase GetDatabase()
        {
            return _connectionMultiplexer.GetDatabase();
        }
    }
}
