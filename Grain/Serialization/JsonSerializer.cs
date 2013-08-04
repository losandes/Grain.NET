using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Grain.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Grain.Serialization
{
    public static partial class JsonSerializer
    {
        /// <summary>
        /// Serialize an object into a JSON string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Serialize an object into a JSON string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        ///// <summary>
        ///// Serialize an object into a Binary JSON string (BSON)
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public static byte[] ToBson<T>(this T obj) 
        //{
        //    return obj.ToBson();
        //}

        /// <summary>
        /// Serialize an object into a Binary JSON string (BSON)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ToBson(this object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // serialize object to BSON
                using (BsonWriter writer = new BsonWriter(ms))
                {
                    new Newtonsoft.Json.JsonSerializer().Serialize(writer, obj);

                    //Console.WriteLine(BitConverter.ToString(ms.ToArray()));
                    return ms.ToArray();
                }
            }
        }
        
        /// <summary>
        /// Reconstruct an object from a JSON string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Reconstruct an object from a JSON string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object FromJson(this string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// Reconstruct an object from a Binary JSON string (BSON)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bson"></param>
        /// <returns></returns>
        public static T FromBson<T>(this byte[] bson) 
        {
            if (bson == null)
                return default(T);

            using (MemoryStream ms = new MemoryStream(bson))
            {
                Type _type = typeof(T);
                //if (typeof(T).IsSubclassOf(typeof(IEnumerable<T>)))
                if (_type.ImplementsGenericInterface(typeof(IEnumerable<>)) || _type.ImplementsInterface(typeof(IEnumerable)))
                {
                    // deserialize object from BSON
                    using (BsonReader reader = new BsonReader(ms, true, DateTimeKind.Unspecified))
                    {
                        return new Newtonsoft.Json.JsonSerializer().Deserialize<T>(reader);
                    }
                }
                else 
                {
                    // deserialize object from BSON
                    using (BsonReader reader = new BsonReader(ms))
                    {
                        return new Newtonsoft.Json.JsonSerializer().Deserialize<T>(reader);
                    }                
                }
            }
        }

        /// <summary>
        /// Reconstruct an object from a Binary JSON string (BSON)
        /// </summary>
        /// <param name="bson"></param>
        /// <returns></returns>
        public static object FromBson(this byte[] bson, Type type)
        {
            if (bson == null)
                return ClassExtensions.GetInstanceOf(type);

            using (MemoryStream ms = new MemoryStream(bson))
            {
                //if (typeof(T).IsSubclassOf(typeof(IEnumerable<T>)))
                if (type.ImplementsGenericInterface(typeof(IEnumerable<>)) || type.ImplementsInterface(typeof(IEnumerable)))
                {
                    // deserialize object from BSON
                    using (BsonReader reader = new BsonReader(ms, true, DateTimeKind.Unspecified))
                    {
                        return new Newtonsoft.Json.JsonSerializer().Deserialize(reader, type);
                    }
                }
                else
                {
                    // deserialize object from BSON
                    using (BsonReader reader = new BsonReader(ms))
                    {
                        return new Newtonsoft.Json.JsonSerializer().Deserialize(reader, type);
                    }
                }
            }
        }
    }
}
