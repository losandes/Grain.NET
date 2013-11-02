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
        bool Set<T>(string key, T data);
        bool Set<T>(string key, T data, ICacheProfile settings);
        object Get(string key);
        object Get(string key, string group);
        T Get<T>(string key) where T : class;
        T Get<T>(string key, string group) where T : class;
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
