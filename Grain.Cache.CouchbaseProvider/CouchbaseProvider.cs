﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using Grain.Serialization;

namespace Grain.Cache.CouchbaseProvider
{
    public class CouchbaseProvider : ICacheProvider
    {
        protected ICouchbaseClient _client { get; set; }

        public CouchbaseProvider(ICouchbaseClient client) 
        {
            _client = client;
        }

        public virtual string GetKeyByGroup(string key, string group)
        {
            if (group == null)
                return key;

            return group + key;
        }

        public virtual bool Set<T>(string key, T data, ICacheProfile settings)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            if (data == null)
                return false;

            if (settings == null)
                return Set<T>(key, data);

            return _client.StoreJson(StoreMode.Set, GetKeyByGroup(key, settings.Group), data, settings.ExpiresIn);
        }

        public virtual bool Set<T>(string key, T data, string group = "", TimeSpan? expiresIn = null)
        {
            if (String.IsNullOrWhiteSpace(key) || data == null)
                return false;

            if (expiresIn.HasValue)
                return _client.StoreJson(StoreMode.Set, GetKeyByGroup(key, group), data, expiresIn.Value);    

            return _client.StoreJson(StoreMode.Set, GetKeyByGroup(key, group), data);
        }

        public virtual object Get(string key, string group)
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            if (String.IsNullOrWhiteSpace(group))
                return Get(key);

            return _client.Get(GetKeyByGroup(key, group));
        }

        public virtual object Get(string key, string group = "", TimeSpan? expiresIn = null) 
        {
            if (String.IsNullOrWhiteSpace(key))
                return null;

            if (expiresIn.HasValue)
                return _client.Get(GetKeyByGroup(key, group), DateTime.UtcNow.Add(expiresIn.Value));

            return _client.Get(GetKeyByGroup(key, group));
        }

        public virtual T Get<T>(string key, string group) where T : class
        {
            if (String.IsNullOrWhiteSpace(key))
                return default(T);

            if (String.IsNullOrWhiteSpace(group))
                return Get<T>(key);

            return _client.GetJson<T>(GetKeyByGroup(key, group));
        }

        public virtual T Get<T>(string key, string group = "", TimeSpan? expiresIn = null) where T : class
        {
            if (String.IsNullOrWhiteSpace(key))
                return default(T);

            if (expiresIn.HasValue) { 
                var _result = this.Get(key, group, expiresIn);
                return _result != null ? _result.ToString().FromJson<T>() : default(T);
            }

            return _client.GetJson<T>(GetKeyByGroup(key, group));
        }

        public virtual bool Remove(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            return _client.Remove(key);
        }

        public virtual bool Remove(string key, string group)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            return _client.Remove(GetKeyByGroup(key, group));
        }

        public virtual bool RemoveGroup(string group)
        {
            throw new NotImplementedException();
        }

        public virtual bool Exists(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            return _client.KeyExists(key);
        }

        public virtual bool Exists(string key, string group)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            return _client.KeyExists(GetKeyByGroup(key, group));
        }

        public virtual bool Touch(string key, TimeSpan expiresIn)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            _client.Touch(key, expiresIn);
            return true;
        }

        public virtual bool Touch(string key, string group, TimeSpan expiresIn)
        {
            if (String.IsNullOrWhiteSpace(key))
                return false;

            _client.Touch(GetKeyByGroup(key, group), expiresIn);
            return true;
        }

        public virtual bool FlushAll()
        {
            _client.FlushAll();
            return true;
        }
    }
}