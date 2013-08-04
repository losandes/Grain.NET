using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Grain.Dynamic;
using Grain.Extensions;
using System.Dynamic;

namespace Grain.Tests.Models.TestModels
{
    [DataContract]
    public partial class UrlDefinition
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 5)]
        public string Description { get; set; }

        [DataMember(Order = 10)]
        public string Url { get; set; }

        [DataMember(Order = 20)]
        public UrlInfoDefinition UrlInfo { get; set; }
    }

    [DataContract]
    public partial class UrlInfoDefinition
    {
        [DataMember(Order = 1)]
        public DateTime TimeCreated { get; set; }

        [DataMember(Order = 2)]
        public int CreatorId { get; set; }
    }

    public enum SerializationType 
    { 
        Xml = 1,
        Json = 2
    }

    public partial class ExtendableObject 
    {
        public string ValueString
        {
            get
            {
                if (_valueInitialized)
                {
                    if (this.SerializationType == SerializationType.Xml)
                    {
                        return this.Value._XmlValue;
                    }
                    else
                    {
                        return this.Value._JsonValue;
                    }
                }
                else { return null; }
            }
            set
            {
                this.ValueInit(value);
                _valueInitialized = true;       // flip _valueInitialized here, so we don't have to everywhere else
            }
        } // a string that, when set prior to initialization, can be used to fill the Value object when it is constructed
        protected dynamic _value;               // the value object will be instantiated lazily
        public dynamic Value                    // Make the Value SOLID and ensure that it is never null
        {
            get
            {
                if (!_valueInitialized)         // make sure a value exists, in case we are creating a new object. 
                    ValueInit();                // this is also fired when a user sets to a property of Value. So 
                return _value;                  // initialization and construction is lazy, controlled and minimized
            }
            protected set                       // protect Value creation to enforce the UrlDefinition contract.
            {
                _value = value;
                _valueInitialized = true;       // flip _valueInitialized here, so we don't have to everywhere else
            }
        }

        /// <summary>
        /// Checks whether or not a dynamic Value exists without invoking it's constructor
        /// </summary>
        /// <returns>true if the Value object was constructed, otherwise false</returns>
        public virtual bool ValueIsInitialized { get { return _valueInitialized; } }
        protected bool _valueInitialized = false;

        /// <summary>
        /// The type of serialization object to use: Xml or Json
        /// </summary>
        protected virtual SerializationType SerializationType { get { throw new NotImplementedException(); } }
        
        /// <summary>
        /// The type of object to use for the XML/JSON DataContract
        /// </summary>
        protected virtual Type ContractType { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Runs the first time the Value object is accessed.  Initializes the Value, to make sure that it is never null.
        /// Uses ExpandoJsonObject by default, unless the SerializationType is Xml, then it uses ExpandoXmlObject.
        /// </summary>
        protected virtual void ValueInit()
        {
            this.ValueInit(this.ValueString);
        }
        
        /// <summary>
        /// Runs the first time the Value object is accessed.  Initializes the Value, to make sure that it is never null.
        /// Uses ExpandoJsonObject by default, unless the SerializationType is Xml, then it uses ExpandoXmlObject.
        /// </summary>
        protected virtual void ValueInit(string valueString)
        {
            if (SerializationType == SerializationType.Xml)
            {
                if (valueString.IsNotEmptyOrWhiteSpace())
                {
                    Value = new ExpandoXmlObject(valueString, ContractType);
                }
                else
                {
                    Value = new ExpandoXmlObject(ContractType);
                }
            }
            else 
            {
                if (valueString.IsNotEmptyOrWhiteSpace())
                {
                    Value = new ExpandoJsonObject(valueString, ContractType);
                }
                else
                {
                    Value = new ExpandoJsonObject(ContractType);
                }            
            }
        }  
    }

    public partial class ExtendableObjectForJson : ExtendableObject
    {
        protected override SerializationType SerializationType { get { return SerializationType.Json; } }
        protected override Type ContractType { get { return typeof(ExpandoObject); } }
    }

    public partial class UrlObjectForXml : ExtendableObject
    {
        public string Name { get { return Value.Name; } set { Value.Name = value; } }
        public string Description { get { return Value.Description; } set { Value.Description = value; } }
        public string Url { get { return Value.Url; } set { Value.Url = value; } }
        public UrlInfoDefinition UrlInfo { get { return Value.UrlInfo; } set { Value.UrlInfo = value; } }

        protected override SerializationType SerializationType { get { return SerializationType.Xml; } }
        protected override Type ContractType { get { return typeof(UrlDefinition); } }
    }

    public partial class UrlObjectForJson : ExtendableObject
    {
        public string Name { get { return Value.Name; } set { Value.Name = value; } }
        public string Description { get { return Value.Description; } set { Value.Description = value; } }
        public string Url { get { return Value.Url; } set { Value.Url = value; } }
        public UrlInfoDefinition UrlInfo { get { return Value.UrlInfo; } set { Value.UrlInfo = value; } }

        protected override SerializationType SerializationType { get { return SerializationType.Json; } }
        protected override Type ContractType { get { return typeof(UrlDefinition); } }
    }
}
