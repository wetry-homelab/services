using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Services
{
    public class TraefikRedisStore : ITraefikRedisStore
    {
        private readonly ConnectionMultiplexer redis;

        public TraefikRedisStore(IConfiguration configuration)
        {
            redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"));
        }

        public async Task StoreValues(List<KeyValuePair<string, string>> routes)
        {
            var db = redis.GetDatabase();

            foreach (var kvp in routes)
            {
                await db.StringSetAsync(kvp.Key, kvp.Value);
            }
        }
    }
}
