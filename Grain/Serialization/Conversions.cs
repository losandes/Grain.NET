using System;
using System.ComponentModel;
using System.Text;

namespace Grain.Serialization
{
    public static class Conversions
    {
        /// <summary>
        /// Tries to convert an object to a given type.  If the object is binary, it uses Pollen.Serialization.JsonSerializer.FromBson(), 
        /// so the binary format is expected to be BSON.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T TryConvert<T>(object data)
        {
            if (data == null)
                return default(T);

            Type dataType = data.GetType();
            Type valueType = typeof(T);

            if (valueType == dataType || valueType.IsAssignableFrom(dataType))
                return (T)data;

            var converter = TypeDescriptor.GetConverter(valueType);
            if (converter.CanConvertFrom(dataType))
                return (T)converter.ConvertFrom(data);

            if (dataType != typeof(byte[]))
                return default(T);

            try
            {
                return ((byte[])data).FromBson<T>();
            }
            catch { }

            return default(T);
        }

        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        // http://stackoverflow.com/questions/4970542/consuming-rest-web-service-in-net-mvc-3
        public static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        // http://stackoverflow.com/questions/4970542/consuming-rest-web-service-in-net-mvc-3
        public static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    }
}
