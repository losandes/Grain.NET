using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Grain.Serialization
{
    public static partial class DictionarySerializer
    {
        /// <summary>
        /// Splits a string into a Dictionary of KeyValuePairs.
        /// </summary>
        /// <param name="input">string: a string of KeyValuePairs that are separated by two different deliminators</param>
        /// <param name="pairDeliminator">char: the deliminator that separates KeyValuePairs from each other</param>
        /// <param name="keyValueDeliminator">char: the deliminator that separates the Key from the Value in each pair</param>
        /// <example>
        /// type:text,class:color_blue text_center,name:my_input_field
        /// </example>
        /// <returns>IDictionary of type string, string: the string cast into a generic Dictionary, or null if the string was empty or malformed</returns>
        public static IDictionary<string, string> ToDictionary(this string input, char pairDeliminator, char keyValueDeliminator)
        {
            Dictionary<string, string> output = new Dictionary<string,string> { };

            try
            {
                string[] _keyValuePairs = input.Split(pairDeliminator);
                string[] _keyValuePair;

                foreach (string p in _keyValuePairs)
                {
                    _keyValuePair = p.Split(keyValueDeliminator);
                    output.Add(_keyValuePair[0].Trim(), _keyValuePair[1].Trim());
                }

                return output;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a dictionary representation of a JSON object, where all property names
        /// are entered as keys in the dictionary, and all property values
        /// are either primitives (int, string), lists, or dictionaries.
        /// JSON object may contain nested objects and lists to any depth.
        /// </summary>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionaryFromJson(this string jsonContent)
        {
            var result = jsonContent.FromJson<Dictionary<string, object>>();
            if (result == null) result = new Dictionary<string, object>();

            foreach (var key in result.Keys.ToList())
            {
                result[key] = ConvertJsonObject(result[key]);
            }

            return result;
        }

        private static object ConvertJsonObject(Object obj)
        {
            var value = obj;
            if (value is JProperty)
                value = ((JProperty)value).Value;

            if (value is JContainer)
                return ((JContainer)value).ToDictionary();
            else if (value is JArray)
                return ((JArray)value).ToObjectList();
            else if (value is JValue)
                return ((JValue)value).Value;
            else
                return value;
        }

        private static Dictionary<string, object> ToDictionary(this JContainer container)
        {
            var result = new Dictionary<string, object>();
            if (container == null) return result;

            foreach (var item in container)
            {
                if (item is JProperty)
                {
                    var prop = item as JProperty;
                    result[prop.Name] = ConvertJsonObject(prop);
                }
            }

            return result;
        }

        private static List<object> ToObjectList(this JArray array)
        {
            var result = new List<object>();
            if (array == null) return result;

            foreach (var item in array)
            {
                result.Add(ConvertJsonObject(item));
            }

            return result;
        }
    }
}
