using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Grain.Extensions;

namespace Grain.Serialization
{
    public static class XMLSerializer
    {
        /// <summary>
        /// Returns an XML string that represents the current object 
        /// 
        /// The DataContractSerializer is ~10% faster than the XMLSerializer, but provides less control 
        /// in the XML structure.  As an example, attributes are not supported and all members are represented as elements.
        /// The class should be decorated with DataContract and DataMember attributes to be serialized with the DataContractSerializer.
        /// 
        /// DataContractSerializer Serializes only the properties of a class that are decorated with DataMember attributes.
        /// It also has better built in support for generics
        /// </summary>
        /// <param name="item">the object to be serialized</param>
        /// <param name="type">The DataContract type</param>
        /// <returns>An XML string that represents the current object</returns>
        public static string ToXml<T>(this T item)
        {
            Type _type = typeof(T);
            return item.ToXml(_type);
        }

        /// <summary>
        /// Returns an XML string that represents the current object 
        /// 
        /// The DataContractSerializer is ~10% faster than the XMLSerializer, but provides less control 
        /// in the XML structure.  As an example, attributes are not supported and all members are represented as elements.
        /// The class should be decorated with DataContract and DataMember attributes to be serialized with the DataContractSerializer.
        /// 
        /// DataContractSerializer Serializes only the properties of a class that are decorated with DataMember attributes.
        /// It also has better built in support for generics
        /// </summary>
        /// <param name="item">the object to be serialized</param>
        /// <param name="type">The DataContract type</param>
        /// <returns>An XML string that represents the current object</returns>
        public static string ToXml(this object item, Type type)
        {
            if (type == typeof(ExpandoObject) || type == typeof(Object))
            {
                //dynamic _obj = item.ToExpando();
                //return DynamicToXML(_obj).ToString();
                return ToXml(((IDictionary<string, object>)item), typeof(IDictionary<string, object>));
            }

            var _settings = new XmlWriterSettings { 
                Indent = true,
                OmitXmlDeclaration = true
            };
            var _sb = new StringBuilder();

            using (var writer = XmlWriter.Create(_sb, _settings))
            {
                var _serializer = new DataContractSerializer(type);
                _serializer.WriteObject(writer, item);
            }

            return _sb.ToString();
        }

        /// <summary>
        /// Serialize an object into an XML string, using the XMLSerializer.
        /// 
        /// The XMLSerializer is ~10% slower than the DataContractSerializer, but provides greater control 
        /// in the XML structure, using data annotations such as XElement, XAttribute and XmlIgnore.
        /// 
        /// XMLSerializer Serializes all properties of a class, unless they are decorated with the XmlIgnore attribute.
        /// </summary>
        /// <typeparam name="T">Type: the type of object to serialize</typeparam>
        /// <param name="item">The object to serialize</param>
        /// <param name="useDataContractSerializer">
        /// bool: if true, a DataContractSerializer is used to serialize the XML, 
        /// otherwise an XmlSerializer is used.
        /// </param>/// 
        /// <returns></returns>
        public static string ToXml<T>(this T item, bool useDataContractSerializer)
        {
            if (useDataContractSerializer)
                return item.ToXml();

            return item.ToXml("Root");
        }

        /// <summary>
        /// Serialize an object into an XML string, using the XMLSerializer.
        /// 
        /// The XMLSerializer is ~10% slower than the DataContractSerializer, but provides greater control 
        /// in the XML structure, using data annotations such as XElement, XAttribute and XmlIgnore.
        /// 
        /// XMLSerializer Serializes all properties of a class, unless they are decorated with the XmlIgnore attribute.
        /// </summary>
        /// <typeparam name="T">Type: the type of object to serialize</typeparam>
        /// <param name="item">The object to serialize</param>
        /// <param name="rootName">string: the name of the root element to be used if the object is of type ExpandoObject or Object</param>
        /// <returns></returns>
        public static string ToXml<T>(this T item, string rootName)
        {
            return item.ToXml(typeof(T), rootName);
        }

        /// <summary>
        /// Serialize an object into an XML string, using the XMLSerializer.
        /// 
        /// The XMLSerializer is ~10% slower than the DataContractSerializer, but provides greater control 
        /// in the XML structure, using data annotations such as XElement, XAttribute and XmlIgnore.
        /// 
        /// XMLSerializer Serializes all properties of a class, unless they are decorated with the XmlIgnore attribute.
        /// </summary>
        /// <param name="item">The object to serialize</param>
        /// <param name="type">Type: the type of object to serialize</param>
        /// <param name="rootName">string: the name of the root element to be used if the object is of type ExpandoObject or Object</param>
        /// <returns></returns>
        /// <remarks>
        /// Partial Credit: http://stackoverflow.com/questions/4970542/consuming-rest-web-service-in-net-mvc-3
        /// </remarks>
        public static string ToXml(this object item, Type type, string rootName)
        {
            if (type == typeof(ExpandoObject) || type == typeof(Object))
            {
                //dynamic _obj = item.ToExpando();
                //return DynamicToXML(_obj, rootName).ToString();
                return ToXml(((IDictionary<string, object>)item), typeof(IDictionary<string, object>));
            }

            //XDocument _xml;
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    XmlSerializerNamespaces _namespaces = new XmlSerializerNamespaces();
            //    _namespaces.Add("", "");
            //    XmlSerializer _serializer = new XmlSerializer(type);
            //    _serializer.Serialize(stream, item, _namespaces);
            //    stream.Close();

            //    byte[] _buffer = stream.ToArray();
            //    string _xmlAsString = new UTF8Encoding().GetString(_buffer);
            //    _xml = XDocument.Parse(_xmlAsString);
            //    _xml.Declaration = null;
            //    return _xml.ToString();
            //}
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer _serializer = new XmlSerializer(type);
                _serializer.Serialize(stream, item);
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Reconstruct an object from an XML string, using DataContractSerializer
        /// </summary>
        /// <typeparam name="T">Type: the type of object to deserialize</typeparam>
        /// <param name="xml">string: the xml to deserialize</param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml)
        {
            Type _type = typeof(T);
            if (_type == typeof(ExpandoObject) || _type == typeof(Object))
            {
                throw new ArgumentException("ExpandoObjects and Objects are serialized as typeof(IDictionary<string, object>), so the output type must be IDictionary<string, object>. You can use the ToExpando() method to then cast the result to ExpandoObject, if you wish.", "T");
            }

            using (StringReader stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    var _serializer = new DataContractSerializer(_type);
                    return (T)_serializer.ReadObject(xmlReader);
                }
            }
        }

        /// <summary>
        /// Reconstruct an object from an XML string, using DataContractSerializer
        /// </summary>
        /// <param name="xml">string: the xml to deserialize</param>
        /// <param name="type">Type: the type of object to deserialize</param>
        /// <returns></returns>
        public static object FromXml(this string xml, Type type)
        {
            if (type == typeof(ExpandoObject) || type == typeof(Object))
            {
                throw new ArgumentException("ExpandoObjects and Objects are serialized as typeof(IDictionary<string, object>), so the output type must be IDictionary<string, object>. You can use the ToExpando() method to then cast the result to ExpandoObject, if you wish.", "T");
            }

            using (StringReader stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    var _serializer = new DataContractSerializer(type);
                    return _serializer.ReadObject(xmlReader);
                }
            }
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <typeparam name="T">Type: the type of object to deserialize</typeparam>
        /// <param name="xml">string: the xml to deserialize</param>
        /// <param name="useDataContractSerializer">
        /// bool: if true, a DataContractSerializer is used to deserialize the XML, 
        /// otherwise an XmlSerializer is used.
        /// </param>
        /// <returns></returns>
        /// http://stackoverflow.com/questions/4970542/consuming-rest-web-service-in-net-mvc-3
        public static T FromXml<T>(this string xml, bool useDataContractSerializer)
        {
            if (useDataContractSerializer)
                return xml.FromXml<T>();

            Type _type = typeof(T);
            if (_type == typeof(ExpandoObject) || _type == typeof(Object))
            {
                throw new ArgumentException("ExpandoObjects and Objects are serialized as typeof(IDictionary<string, object>), so the output type must be IDictionary<string, object>. You can use the ToExpando() method to then cast the result to ExpandoObject, if you wish.", "T");
            }

            XmlSerializer _serializer = new XmlSerializer(_type);
            using (MemoryStream memoryStream = new MemoryStream(Conversions.StringToUTF8ByteArray(xml)))
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    return (T)_serializer.Deserialize(memoryStream);
                }
            }
        }
    }
}
