using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {

        public virtual string GetKeyByGroup(string key, string group)
        {
            return group + key;
        }

        public bool Set<T>(string key, T data)
        {
            throw new NotImplementedException();
        }

        public bool Set<T>(string key, T data, ICacheProfile settings)
        {
            throw new NotImplementedException();
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public object Get(string key, string group)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key, string group) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, string group)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGroup(string group)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key, string group)
        {
            throw new NotImplementedException();
        }

        public bool Touch(string key, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool Touch(string key, string group, TimeSpan expiresIn)
        {
            throw new NotImplementedException();
        }

        public bool FlushAll()
        {
            throw new NotImplementedException();
        }
    }
}
