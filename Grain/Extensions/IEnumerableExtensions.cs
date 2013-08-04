using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Grain.Extensions
{
    public static partial class IEnumerableExtensions
    {
        /// <summary>
        /// Indicates whether the specified IEnumerable object is null (Nothing) or Empty (has no values).
        /// </summary>
        /// <typeparam name="T">object: the type of IEnumerable object</typeparam>
        /// <param name="source">object: the IEnumerable object itself</param>
        /// <returns>bool: true if the object is null or empty</returns>
        /// <example>
        /// var _settings = db.Settings.Where(s => s.Default == true);
        /// bool _settingsAreEmpty = _settings.IsEmpty(); 
        /// </example>
        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null) { return true; }
            return !source.Any();
        }

        /// <summary>
        /// Indicates whether the specified string is null (Nothing) or an Empty string.
        /// </summary>
        /// <param name="source">string: the string to check</param>
        /// <returns>bool: true if the string has no value</returns>
        public static Boolean IsEmptyOrWhiteSpace(this string source)
        {
            return String.IsNullOrWhiteSpace(source);
            //return source == null || source.Trim().Length < 1;
        }

        /// <summary>
        /// Indicates whether the specified IEnumerable object is not null (Nothing) and is not Empty (it has at least one value).
        /// </summary>
        /// <typeparam name="T">object: the type of IEnumerable object</typeparam>
        /// <param name="source">object: the IEnumerable object itself</param>
        /// <returns>bool: true if the object has a value</returns>
        /// <example>
        /// var _settings = db.Settings.Where(s => s.Default == true);
        /// bool _settingsAreNotEmpty = _settings.IsNotEmpty(); 
        /// </example>
        public static Boolean IsNotEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null) { return false; }
            return source.Any();
        }

        /// <summary>
        /// Indicates whether the specified string has a value.
        /// </summary>
        /// <param name="source">string: the string to check</param>
        /// <returns>bool: true if the string has a value</returns>
        public static Boolean IsNotEmptyOrWhiteSpace(this string source)
        {
            return String.IsNullOrWhiteSpace(source) == false;
            //return source != null & source.Trim().Length > 0;
        }

        /// <summary>
        /// Get a list of objects as a derived type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsType<T>(this IEnumerable list)
        {
            // TODO: Can we use OfType, instead?
            foreach (var obj in list)
            {
                if (obj is T)
                    yield return (T)obj;
            }
        }

        /// <summary>
        /// Indicates whether the specified Guid has a value.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool GuidIsEmpty(this Guid? guid)
        {
            return guid.HasValue == false || guid.Value == Guid.Empty;
        }

        /// <summary>
        /// Indicates whether the specified Guid has a value.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool GuidIsNotEmpty(this Guid? guid)
        {
            return guid.HasValue && guid.Value != Guid.Empty;
        }

        /// <summary>
        /// Indicates whether the specified Guid has a value.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool GuidIsEmpty(this Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }

        /// <summary>
        /// Indicates whether the specified Guid has a value.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool GuidIsNotEmpty(this Guid guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        /// <summary>
        /// Converts a string into a Guid
        /// </summary>
        /// <param name="source">string: the string you wish to convert</param>
        /// <returns>Guid: the Guid</returns>
        public static Guid ToGuid(this string source)
        {
            try
            {
                if (source == null)
                {
                    return Guid.Empty;
                }
                return new Guid(source);
            }
            catch
            {
                return Guid.Empty;
            }
        }

        /// <summary>
        /// Casts a deliminated string to a List
        /// </summary>
        /// <param name="input"></param>
        /// <param name="deliminator"></param>
        /// <returns></returns>
        public static List<string> ToList(this string input, char deliminator)
        {
            return input.ToHashSet(',').ToList();
        }

        /// <summary>
        /// Casts a deliminated string to a HashSet
        /// </summary>
        /// <param name="input"></param>
        /// <param name="deliminator"></param>
        /// <returns></returns>
        public static HashSet<string> ToHashSet(this string input, char deliminator)
        {
            string[] _list = input.Split(deliminator);
            var _output = new HashSet<string> { };

            foreach (var item in _list)
            {
                var _item = item.Trim();
                if (_item.IsNotEmptyOrWhiteSpace())
                    _output.Add(_item);
            }

            return _output;
        }

        

        /// <summary>
        /// Returns a new list of values merged from the input and the other list that are passed as parameters
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="input"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static List<T> Merge<T>(this List<T> input, List<T> otherList)
        {
            foreach (T item in otherList)
            {
                input.Add(item);
            }

            return input;
        }

        /// <summary>
        /// Returns a new dictionary of values merged from the input and the other dictionaries that are passed as parameters
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="input"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        public static IDictionary<K, V> Merge<K, V>(this IDictionary<K, V> input, params IDictionary<K, V>[] others)
        {
            List<IDictionary<K, V>> dictionaries = new List<IDictionary<K, V>> { input };

            foreach (IDictionary<K, V> dict in others)
            {
                dictionaries.Add(dict);
            }

            var result = dictionaries.SelectMany(dict => dict)
                                     .ToLookup(pair => pair.Key, pair => pair.Value)
                                     .ToDictionary(group => group.Key, group => group.First());

            return result;
        }

        /// <summary>
        /// Iterates over the values of two Dictionaries to merge and override values to the left.  Html classes are treated differently, in that they are appended, rather than overridden.
        /// </summary>
        /// <param name="HtmlAttributes"></param>
        /// <param name="HtmlAttributesThatOverride"></param>
        /// <returns></returns>
        public static IDictionary<string, string> MergeHtmlAttributesLeft(this IDictionary<string, string> HtmlAttributes, IDictionary<string, string> HtmlAttributesThatOverride)
        {
            foreach (KeyValuePair<string, string> pair in HtmlAttributesThatOverride)   // Loop over the trump HtmlAttrubutes that were passed as a parameter
            {
                if (!HtmlAttributes.ContainsKey(pair.Key))                              // if the attributes doesn't exist, add it
                {
                    HtmlAttributes.Add(pair.Key, pair.Value);
                }
                else
                {
                    if (pair.Key.Trim().ToLower() == "class")                           // otherwise, if it exists and is the class attribute, append it
                    {
                        string _newValue = HtmlAttributes[pair.Key].AppendCssClass(pair.Value);
                        HtmlAttributes[pair.Key] = _newValue;
                    }
                    else                                                                // otherwise, override it
                    {
                        HtmlAttributes[pair.Key] = pair.Value;
                    }
                }
            }

            return HtmlAttributes;
        }

        /// <summary>
        /// Replaces the value for a given key to the new value that is passed as a parameter.
        /// This does not iterate over similar values - the key must be exact.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static IDictionary<string, object> RenameKey(this IDictionary<string, object> input, string oldKey, string newKey)
        {
            var _valuePair = input.FirstOrDefault(k => k.Key == oldKey);
            input.Remove(oldKey);
            input.Add(newKey, _valuePair.Value);

            return input;
        }

        /// <summary>
        /// Replaces the value for a given key to the new value that is passed as a parameter.
        /// This does not iterate over similar values - the key must be exact.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static IDictionary<K, V> Replace<K, V>(this IDictionary<K, V> input, K key, V newValue)
        {
            input.Remove(key);
            input.Add(key, newValue);

            return input;
        }

        /// <summary>
        /// Replaces the value for a given key to the new value that is passed as a parameter.
        /// This does not iterate over similar values - the key must be exact.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static IDictionary<K, V> Update<K, V>(this IDictionary<K, V> input, K key, V newValue)
        {
            return input.Replace(key, newValue);
        }

        /// <summary>
        /// Replaces the value for a given key to the new value that is passed as a parameter.
        /// This does not iterate over similar values - the key must be exact.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static FormCollection Replace(this FormCollection input, string key, string newValue)
        {
            input.Remove(key);
            input.Add(key, newValue);

            return input;
        }
    }
}
