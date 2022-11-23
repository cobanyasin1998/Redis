using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;
        public IDatabase db { get; set; }
        private ConnectionMultiplexer _redis;

        public RedisService(IConfiguration configuration)
        {
           

            _redisHost = configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }
        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString);


        }
        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db);
        }
    }
}
