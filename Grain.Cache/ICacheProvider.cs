using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public interface ICacheProvider
    {
        string GetKeyByGroup(string key, string group);
        bool Set<T>(string key, T data, ICacheProfile settings);
        bool Set<T>(string key, T data, string group = "", TimeSpan? expiresIn = null);
        object Get(string key, string group = "", TimeSpan? expiresIn = null);
        T Get<T>(string key, string group = "", TimeSpan? expiresIn = null) where T : class;
        bool Remove(string key);
        bool Remove(string key, string group);
        bool RemoveGroup(string group);
        //bool RemoveBy<I,O>(Func<I, O> expression);
        bool Exists(string key);
        bool Exists(string key, string group);
        bool Touch(string key, TimeSpan expiresIn);
        bool Touch(string key, string group, TimeSpan expiresIn);
        bool FlushAll();
    }
}
