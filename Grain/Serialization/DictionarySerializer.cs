using System.Collections.Generic;

namespace Grain.Serialization
{
    public static partial class DictionarySerializer
    {
        /// <summary>
        /// Splits a string into a Dictionary of KeyValuePairs.  Expects the string to have KeyValuePairs separated by commas 
        /// and Keys and Value separated by an equal sign.
        /// </summary>
        /// <param name="input">string: a string of KeyValuePairs that are separated by two different deliminators</param>
        /// <returns>IDictionary of type string, string: the string cast into a generic Dictionary</returns>
        public static IDictionary<string, string> ToDictionary(this string input)
        {
            return input.ToDictionary(',', '=');
        }

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
    }
}
