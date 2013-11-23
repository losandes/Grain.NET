using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// Get the group key (a concatted string)
        /// </summary>
        /// <param name="key">the unqique key for the item being cached</param>
        /// <param name="group">the data group the cached item belongs to</param>
        /// <returns>a concatenated string (group + key)</returns>
        string GetKeyByGroup(string key, string group);

        /// <summary>
        /// Get the group keys (concatted strings)
        /// </summary>
        /// <param name="keys">a collection of unique keys for the cached items</param>
        /// <param name="group">the data group the cached item belongs to</param>
        /// <returns>a collection of concatenated strings (group + key)</returns>
        IEnumerable<string> GetKeysByGroup(IEnumerable<string> keys, string group);

        /// <summary>
        /// Sets data in the cache, by the given key and with the given settings.  If the item already exists in the cache, 
        /// it is overwritten.  Otherwise it is added.
        /// </summary>
        /// <typeparam name="T">the type of data being set</typeparam>
        /// <param name="key">the unique key for the item being cached</param>
        /// <param name="data">the data that is being cached</param>
        /// <param name="settings">the settings to use when caching this data</param>
        /// <returns>true if the item was cached successfully. otherwise false.</returns>
        bool Set<T>(string key, T data, ICacheProfile settings);

        /// <summary>
        /// Sets data in the cache, by the given key and with the given settings.  If the item already exists in the cache, 
        /// it is overwritten.  Otherwise it is added.
        /// </summary>
        /// <typeparam name="T">the type of data being set</typeparam>
        /// <param name="key">the unique key for the item being cached</param>
        /// <param name="data">the data that is being cached</param>
        /// <param name="group">the data group the cached item belongs to</param>
        /// <param name="expiresIn">the duration that this item will persist in the cache</param>
        /// <returns>true if the item was cached successfully. otherwise false.</returns>
        bool Set<T>(string key, T data, string group = "", TimeSpan? expiresIn = null);

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">(optional) a new duration to persist this item in the cache (i.e. touch)</param>
        /// <returns>the cached item as an object</returns>
        object Get(string key, string group = "", TimeSpan? expiresIn = null);

        /// <summary>
        /// Gets data from the cache, by the given key.
        /// </summary>
        /// <typeparam name="T">the type of the requested data (i.e. the type it will be deserialized or cast as)</typeparam>
        /// <param name="key">the unique key for the cached item</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <param name="expiresIn">(optional) a new duration to persist this item in the cache (i.e. touch) </param>
        /// <returns>the cached item as T</returns>
        T Get<T>(string key, string group = "", TimeSpan? expiresIn = null) where T : class;

        /// <summary>
        /// Gets the values for multiple keys from the cache, by the given keys
        /// </summary>
        /// <param name="keys">a collection of unique keys for the cached items</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <returns>the cached items in a Dictionary</returns>
        IDictionary<string,object> Get(IEnumerable<string> keys, string group = "");
        
        /// <summary>
        /// Gets the values for multiple keys from the cache, by the given keys
        /// </summary>
        /// <typeparam name="T">the type of the requested data (i.e. the type it will be deserialized or cast as)</typeparam>
        /// <param name="keys">a collection of unique keys for the cached items</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <returns>the cached items in a Dictionary</returns>
        IDictionary<string, T> Get<T>(IEnumerable<string> keys, string group = "") where T : class;

        /// <summary>
        /// removes an item from the cache, by the given key
        /// </summary>
        /// <param name="key">the unique key of the item being removed from the cache</param>
        /// <returns>true if the item was removed from the cache, otherwise false</returns>
        bool Remove(string key);

        /// <summary>
        /// removes an item from the cache, by the given key
        /// </summary>
        /// <param name="key">the unique key of the item being removed from the cache</param>
        /// <param name="group">(optional) the data group the cached item belongs to</param>
        /// <returns>true if the item was removed from the cache, otherwise false</returns>
        bool Remove(string key, string group);
        
        /// <summary>
        /// Removes items from the cache, by group
        /// </summary>
        /// <param name="group">the data group to remove items from</param>
        /// <returns>true if the items were removed, otherwise false</returns>
        bool RemoveGroup(string group);
        //bool RemoveBy<I,O>(Func<I, O> expression);

        /// <summary>
        /// Checks to see if given key exists in the cache
        /// </summary>
        /// <param name="key">the unique key to check for</param>
        /// <returns>true, if the key exists in the cache, otherwise false</returns>
        bool Exists(string key);

        /// <summary>
        /// Checks to see if given grouped key exists in the cache
        /// </summary>
        /// <param name="key">the unique key to check for</param>
        /// <param name="group">the data group this key belongs to</param>
        /// <returns>true, if the key exists in the cache, otherwise false</returns>
        bool Exists(string key, string group);

        /// <summary>
        /// Touches an item in the cache to change the duration that it will be persisted for
        /// </summary>
        /// <param name="key">the unique key of the item being touched</param>
        /// <param name="expiresIn">the new duration this item should be persisted for</param>
        /// <returns>true if the key was touched in the cache, otherwise false</returns>
        bool Touch(string key, TimeSpan expiresIn);


        /// <summary>
        /// Touches an item in the cache to change the duration that it will be persisted for
        /// </summary>
        /// <param name="key">the unique key of the item being touched</param>
        /// <param name="group">the data group this item belongs to in the cache</param>
        /// <param name="expiresIn">the new duration this item should be persisted for</param>
        /// <returns>true if the key was touched in the cache, otherwise false</returns>        
        bool Touch(string key, string group, TimeSpan expiresIn);

        /// <summary>
        /// Flushes / Clears all data from the cache (destructive)
        /// </summary>
        /// <returns>true if the cache was flushed, otherwise false</returns>
        bool FlushAll();
    }
}
