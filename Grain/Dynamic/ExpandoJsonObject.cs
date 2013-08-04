using System;
using System.Dynamic;
using System.Runtime.Serialization;
using Grain.Serialization;
using Grain.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Grain.Dynamic
{
    // For more info on DynamicObject classes, see: http://blogs.msdn.com/b/csharpfaq/archive/2009/10/19/dynamic-in-c-4-0-creating-wrappers-with-dynamicobject.aspx
    public class ExpandoJsonObject : DynamicObject
    {
        // All properties are prefixed with an underscore to avoid collision with JSON element names
        private Type _type;
        private string _privateJsonValue { get; set; }
        public string _JsonValue
        {
            get
            {
                //Init();
                _privateJsonValue = ((object)_Value._Data).ToJson();
                return _privateJsonValue;
            }
            set { _privateJsonValue = value; _dataInitialized = false; }
        }
        public dynamic _Value { get; private set; }
        public object _DataAsObject { get { return (object)_Value._Data; } }
        protected bool _dataInitialized = false;

        public ExpandoJsonObject()
        {
            _Value = new ExpandoObject();
            _type = typeof(ExpandoObject);
            _Value._Data = ClassExtensions.GetInstanceOf(_type);  //FormatterServices.GetUninitializedObject(type);

            if (_Value._Data == null)
            {
                _Value._Data = new ExpandoObject();
            }
        }
        
        public ExpandoJsonObject(Type type)
        {
            _Value = new ExpandoObject();
            _type = type;
            _Value._Data = ClassExtensions.GetInstanceOf(type);  //FormatterServices.GetUninitializedObject(type);

            if (_Value._Data == null) 
            {
                _Value._Data = new ExpandoObject();
            }
        }

        public ExpandoJsonObject(string jsonValue, Type type)
        {
            _Value = new ExpandoObject();
            _type = type;
            _Value._Data = ClassExtensions.GetInstanceOf(type);  //FormatterServices.GetUninitializedObject(type);

            if (_Value._Data == null)
            {
                _Value._Data = new ExpandoObject();
            }
            _privateJsonValue = jsonValue;
            InitializeDataFromJson();
        }

        /// <summary>
        /// If the data needs to be deserialized from JSON, deserializes the value of the _privateJsonValue field 
        /// into an object that is stored in the Value._Data property.
        /// </summary>
        public void InitializeDataFromJson()
        {
            if (!String.IsNullOrWhiteSpace(_privateJsonValue) && !_dataInitialized)
            {
                _Value._Data = _privateJsonValue.FromJson(_type);
                _dataInitialized = true;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // in case this is the first action taken against this object, and existing JSON has 
            // not be deserialized, run the initialization to prevent losing data
            InitializeDataFromJson();
            Type _propertyType = (_Value._Data).GetType();
            var _property = _propertyType.GetProperty(binder.Name);
            if (_property != null) 
            {
                _property.SetValue(_Value._Data, Convert.ChangeType(value, _property.PropertyType), null);
                return true;            
            }
            else if (ClassExtensions.TypeIsDerivedFrom<ExpandoObject>(_propertyType)) 
            {
                return TrySetAsDictionary(binder, value);
            }
            else if (_propertyType.ImplementsInterface(typeof(IDictionary)))
            {
                return TrySetAsDictionary(binder, value);
            }
            return false;
        }

        protected virtual bool TrySetAsDictionary(SetMemberBinder binder, object value) 
        {
            var _dict = (IDictionary<string, object>)_Value._Data;
            if (_dict.Any(d => d.Key == binder.Name))
            {
                _dict[binder.Name] = value;
            }
            else
            {
                _dict.Add(binder.Name, value);
            }
            return true;        
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            InitializeDataFromJson();
            Type _propertyType = (_Value._Data).GetType();

            if (ClassExtensions.TypeIsDerivedFrom<ExpandoObject>(_propertyType)) 
            {
                var _result = ((IDictionary<string, object>)_Value._Data).FirstOrDefault(e => e.Key == binder.Name);

                if (_result.Value != null)
                {
                    result = _result.Value;
                    return true;
                }
                result = null;
                return true;
            }
            else if (_propertyType.ImplementsInterface(typeof(IDictionary)))
            {
                var _result = ((IDictionary<string, object>)_Value._Data).FirstOrDefault(e => e.Key == binder.Name);

                if (_result.Value != null)
                {
                    result = _result.Value;
                    return true;
                }
                result = null;
                return true;
            }
            
            var _property = _propertyType.GetProperty(binder.Name);
            if (_property != null)
            {
                result = _property.GetValue(_Value._Data, null);
                return true;
            }
            result = null;
            return true;
        }
    }    
}
