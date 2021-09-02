using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    public interface ICacheManager
    {
        void Set(string cacheKey, string value);
        string Get(string cacheKey);
        void Remove(string key);
    }
}
