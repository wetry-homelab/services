using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITraefikRedisStore
    {
        Task StoreValues(List<KeyValuePair<string, string>> routes);
    }
}
