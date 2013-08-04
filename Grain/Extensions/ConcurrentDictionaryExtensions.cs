using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grain.Attributes;

namespace Grain.Extensions
{
    public static partial class ConcurrentDictionaryExtensions
    {
        /// <summary>
        ///     Adds a key/value pair to the System.Collections.Concurrent.ConcurrentDictionary<TKey,TValue>
        ///     if the key does not already exist, using Lazy initializing, to avoid duplicate values from 
        ///     concurrent sources
        /// </summary>
        /// <typeparam name="T">Type: the typeof key</typeparam>
        /// <typeparam name="V">Type: the typeof value</typeparam>
        /// <param name="dictionary">ConcurrentDictionary</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">Func: The function used to generate a new value for an absent key</param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key
        ///     if the key is already in the dictionary, or the new value if the key was
        ///     not in the dictionary.
        /// </returns>
        /// <example>
        ///     ConcurrentDictionaryExtensions.GetOrAddLazy(_myDictionary, 7, (i) => i.ToString()); // uses this
        ///     _myDictionary.GetOrAddLazy<int, Lazy<string>, string>(6, (i) => i.ToString());      // uses this
        ///     _myDictionary.GetOrAdd(5, (i) => i.ToString());                                     // uses existing
        /// </example>
        [Cite(Link = "http://codereview.stackexchange.com/questions/2025/extension-methods-to-make-concurrentdictionary-getoradd-and-addorupdate-thread-s")]
        public static V GetOrAddLazy<T, V>(this ConcurrentDictionary<T, Lazy<V>> dictionary, T key, Func<T, V> valueFactory)
        {
            Lazy<V> lazy = dictionary.GetOrAdd(key, new Lazy<V>(() => valueFactory(key)));
            return lazy.Value;
        }

        /// <summary>
        /// Adds a key/value pair to the System.Collections.Concurrent.ConcurrentDictionary<TKey,TValue> 
        /// if the key does not already exist, or updates a key/value pair in the System.Collections.Concurrent.ConcurrentDictionary<TKey,TValue>
        /// if the key already exists.  Uses Lazy initializing, to avoid duplicate values from concurrent sources.
        /// </summary>
        /// <typeparam name="T">Type: the typeof key</typeparam>
        /// <typeparam name="V">Type: the typeof value</typeparam>
        /// <param name="dictionary">ConcurrentDictionary</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="addValueFactory">Func: The function used to generate a new value for an absent key</param>
        /// <param name="updateValueFactory">
        ///     Func: The function used to generate a new value for an existing key based on the
        ///     key's existing value
        /// </param>
        /// <returns>
        ///     The value for the key. This will be either the existing value for the key
        ///     if the key is already in the dictionary, or the new value if the key was
        ///     not in the dictionary.
        /// </returns>
        [Cite(Link = "http://codereview.stackexchange.com/questions/2025/extension-methods-to-make-concurrentdictionary-getoradd-and-addorupdate-thread-s")]
        public static V AddOrUpdateLazy<T, V>(this ConcurrentDictionary<T, Lazy<V>> dictionary, T key, Func<T, V> addValueFactory, Func<T, V, V> updateValueFactory)
        {
            Lazy<V> lazy = dictionary.AddOrUpdate(key,
                new Lazy<V>(() => addValueFactory(key)),
                (k, oldValue) => new Lazy<V>(() => updateValueFactory(k, oldValue.Value)));
            return lazy.Value;
        }
    }
}
