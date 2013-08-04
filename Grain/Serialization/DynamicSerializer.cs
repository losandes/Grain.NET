using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Grain.Attributes;
using Grain.Extensions;

namespace Grain.Serialization
{
    /// <summary>
    /// The DynamicSerializer class is still a Lab, even though it is moved into the main Grain library.
    /// Use with caution.
    /// </summary>
    public static class DynamicSerializer
    {
        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// </summary>
        [Cite(Link = "http://theburningmonk.com/2011/05/idictionarystring-object-to-expandoobject-extension-method/")]
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }

        /// <summary>
        /// Extension method that turns an XElement to an ExpandoObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static dynamic ToExpando(this object value)
        {
            if (value.GetType() == typeof(ExpandoObject))   // if the object is already an Expando, no processing necessary
                return value;

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        ///// <summary>
        ///// Extension method that turns an XElement to an ExpandoObject
        ///// </summary>
        //public static dynamic ToExpando(this XElement xml)
        //{
        //    return xml.DynamicFromXML();
        //}

        //public static dynamic ToExpando(this XElement node, String nodeName)
        //{
        //    dynamic _object = new ExpandoObject();
        //    _object.Name = nodeName;
        //    IDictionary<string, object> _dict = (IDictionary<string, object>)_object;

        //    IEnumerable<XElement> _properties = node.Descendants();

        //    foreach (XElement property in _properties)
        //    {
        //        // TODO: Handle nested nodes

        //        //if (property.Value.GetType() == typeof(ExpandoObject))
        //        //    xmlNode.Add(ToXML(property.Value, property.Key));

        //        //else
        //        //    if (property.Value.GetType() == typeof(List<dynamic>))
        //        //        foreach (var element in (List<dynamic>)property.Value)
        //        //            xmlNode.Add(ToXML(element, property.Key));
        //        //    else
        //        //        xmlNode.Add(new XElement(property.Key, property.Value));

        //        _dict.Add(property.Name.ToString(), property.Value);
        //    }

        //    return _dict;
        //}

        public static T FromExpando<T>(ExpandoObject source)
        {
            T _result = default(T);
            _result.FromExpando<T>(source);
            return _result;
        }

        public static void FromExpando<T>(this T destination, ExpandoObject source)
        {
            // Might as well take care of null references early.
            if (source == null)
                throw new ArgumentNullException("source");

            Dictionary<string, PropertyInfo> _propertyMap = typeof(T)
                .GetProperties()
                .ToDictionary(
                    p => p.Name.ToLower(),
                    p => p
                );

            // By iterating the KeyValuePair<string, object> of
            // source we can avoid manually searching the keys of
            // source as we see in your original code.
            foreach (var kv in source)
            {
                PropertyInfo p;
                if (_propertyMap.TryGetValue(kv.Key.ToLower(), out p))
                {
                    var _propType = p.PropertyType;
                    var _valType = kv.Value.GetType();
                    if (kv.Value == null)
                    {
                        if (!_propType.IsByRef && _propType.Name != "Nullable`1")
                        {
                            // Throw if type is a value type but not Nullable<>
                            throw new ArgumentException("not nullable");
                        }
                    }
                    else if (_propType.ImplementsInterface(typeof(IDictionary)))
                    {
                        // UNDONE
                        throw new NotImplementedException();
                    }
                    else if (_propType.ImplementsInterface(typeof(ICollection)))
                    {
                        //var _subPropType = _propType.GetGenericTypeDefinition();
                        //var _subValType = _valType.GetGenericTypeDefinition();
                        // UNDONE
                        throw new NotImplementedException();
                    }
                    else if (_valType != _propType)
                    {
                        var converter = TypeDescriptor.GetConverter(_propType);
                        if (converter.CanConvertFrom(kv.Value.GetType()))
                            p.SetValue(destination, converter.ConvertFrom(kv.Value), null);

                        else // You could make this a bit less strict but I don't recommend it.
                            throw new ArgumentException("type mismatch");
                    }
                    else
                    {
                        p.SetValue(destination, kv.Value, null);
                    }

                }
            }
        } // From Expando

        //[Cite(Link="http://geekswithblogs.net/Nettuce/archive/2012/06/02/convert-dynamic-to-type-and-convert-type-to-dynamic.aspx")]
        //public static T FromDynamic<T>(this IDictionary<string, object> dictionary)
        //{
        //    var bindings = new List<MemberBinding>();
        //    foreach (var sourceProperty in typeof(T).GetProperties().Where(x => x.CanWrite))
        //    {
        //        var key = dictionary.Keys.SingleOrDefault(x => x.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase));
        //        if (string.IsNullOrEmpty(key)) continue;
        //        var propertyValue = dictionary[key];
        //        bindings.Add(Expression.Bind(sourceProperty, Expression.Constant(propertyValue)));
        //    }
        //    Expression memberInit = Expression.MemberInit(Expression.New(typeof(T)), bindings);
        //    return Expression.Lambda<Func<T>>(memberInit).Compile().Invoke();
        //}

        //[Cite(Link="http://geekswithblogs.net/Nettuce/archive/2012/06/02/convert-dynamic-to-type-and-convert-type-to-dynamic.aspx")]
        //public static dynamic ToDynamic<T>(this T obj)
        //{
        //    IDictionary<string, object> expando = new ExpandoObject();

        //    foreach (var propertyInfo in typeof(T).GetProperties())
        //    {
        //        var propertyExpression = Expression.Property(Expression.Constant(obj), propertyInfo);
        //        var currentValue = Expression.Lambda<Func<string>>(propertyExpression).Compile().Invoke();
        //        expando.Add(propertyInfo.Name.ToLower(), currentValue);
        //    }
        //    return expando as ExpandoObject;
        //}

        #region Xml Serialization

        /////// <summary>        ///// Reconstruct an object from an XML string
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="xml"></param>
        ///// <returns></returns>
        //public static ToT FromDynamicXML<ToT>(this string xml)
        //{
        //    dynamic _result = DynamicFromXML(xml);
        //    ToT _object = DynamicSerializer.FromExpando<ToT>(_result);  //new ModelBinder().BindModel<ToT>(_result);
        //    return _object;
        //}

        /// <summary>
        /// Serializes an ExpandoObject as XML
        /// </summary>
        /// <param name="_object">dynamic: the ExpandoObject</param>
        /// <returns>XElement: the XML node</returns>
        public static XElement DynamicToXML(dynamic _object)
        {
            return DynamicToXML(_object, "Root");
        }

        /// <summary>
        /// Serializes an ExpandoObject as XML
        /// (WARNING) This method is beta and does not fully support recursion!
        /// </summary>
        /// <param name="_object">dynamic: the ExpandoObject</param>
        /// <param name="rootName">String: the name of the XML node to create</param>
        /// <returns>XElement: the XML node</returns>
        // http://blogs.msdn.com/b/csharpfaq/archive/2009/10/01/dynamic-in-c-4-0-introducing-the-expandoobject.aspx
        public static XElement DynamicToXML(dynamic _object, string rootName) //, bool useDataContractSerializer)
        {
            XElement xmlNode = new XElement(rootName);

            foreach (var property in (IDictionary<String, Object>)_object)
            {
                if (property.Value == null)
                    continue;
                Type _type = property.Value.GetType();

                if (_type.IsPrimitive || _type.Equals(typeof(string)))
                {
                    xmlNode.Add(new XElement(property.Key, property.Value));
                }
                else if (_type == typeof(ExpandoObject))
                {
                    xmlNode.Add(DynamicToXML(property.Value, property.Key));
                }
                else if (_type == typeof(List<dynamic>))
                {
                    foreach (var element in (List<dynamic>)property.Value)
                    {
                        xmlNode.Add(DynamicToXML(element, property.Key));
                    }
                }
                else if (_type.ImplementsInterface(typeof(IDictionary)))
                {
                    XElement _el = new XElement(property.Key);
                    foreach (var element in (IDictionary)property.Value)
                    {
                        _el.Add(XElement.Parse(element.ToXml("KeyValuePair")));
                        // TODO: make this recursive
                    }
                    xmlNode.Add(_el);
                }
                else if (_type.ImplementsInterface(typeof(ICollection)))
                {
                    XElement _el = new XElement(property.Key);
                    foreach (var element in (ICollection)property.Value)
                    {
                        // TODO: make this recursive
                        _el.Add(new XElement(element.GetType().Name, element));
                    }
                    xmlNode.Add(_el);
                }
                // <param name="useDataContractSerializer">When true, complex objects that are found during serialization are serialized using DataContract serialization, otherwise, a less rigid (and less reliable) serialization is attempted.</param>
                //else if (useDataContractSerializer)
                //{
                //    xmlNode.Add(new XElement(property.Key,
                //        XElement.Parse(property.Value.ToXml(_type))
                //    ));
                //}
                else
                {
                    xmlNode.Add(new XElement(property.Key,
                        XElement.Parse(property.Value.ToXml(_type, "Child")).Descendants()
                    ));
                }
            }
            return xmlNode;
        }

        /// <summary>
        /// Extension method that turns an XML string to an ExpandoObject
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static dynamic DynamicFromXML(this string xml)
        {
            if (xml.IsEmptyOrWhiteSpace()) return null;
            return DynamicFromXML(XElement.Parse(xml));
        }

        /// <summary>
        /// Extension method that turns an XElement to an ExpandoObject
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        /// Inspired by: http://www.codeproject.com/Tips/227139/Converting-XML-to-an-dynamic-object-using-ExpandoO
        public static dynamic DynamicFromXML(this XElement xml)
        {
            dynamic _result = new ExpandoObject();

            foreach (var element in xml.Elements())
            {
                var skip = false;

                //// if the type is "list", or if a single child element exists, use an extra layer of recursion to deserialize the children
                //// This appears to turn elements that have a single node into lists, in which case they are no longer accessible via the ExpandoObject
                //var _type = element.Attributes().Where(e => e.Name.LocalName.ToLower() == "type").Select(e => e.Value.ToLower()).FirstOrDefault();
                //var _numberOfChildren = element.Elements().GroupBy(n => n.Name.LocalName).Count();
                //skip = _type == "list" ? true : _numberOfChildren == 1;

                var p = _result as IDictionary<String, dynamic>;
                var values = new List<dynamic>();
                // If the current node is a container node then we want to skip adding
                // the container node itself, but instead we load the children elements
                // of the current node. If the current node has child elements then load
                // those child elements recursively
                if (skip)
                {
                    foreach (var item in element.Elements())
                    {
                        values.Add((item.HasElements) ? item.DynamicFromXML() : item.Value.Trim());
                    }
                }
                else
                {
                    values.Add((element.HasElements) ? element.DynamicFromXML() : element.Value.Trim());
                }

                // Add the object name + value or value collection to the dictionary
                p[element.Name.LocalName] = skip ? values : values.FirstOrDefault();
            }

            return _result;
        }

        #endregion Xml Serialization

        #region Helpers

        /// <summary>
        /// Used to format the form name for parent nodes in Xml (eg. People, Projects)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string XmlParentNodeName(this string nodeName)
        {
            return nodeName.RemoveSpecialCharacters().RemoveSpaces().Pluralize();
        }

        /// <summary>
        /// Used to format the form name for child nodes in Xml (eg. Person, Project)
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string XmlChildNodeName(this string nodeName)
        {
            return nodeName.RemoveSpecialCharacters().RemoveSpaces().Singularize();
        }

        #endregion Helpers
    }
}
