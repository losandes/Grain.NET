using System;
using System.Runtime.Serialization;
using Grain.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using Grain.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;
using Grain.Tests.Models.TestModels;

namespace Grain.Tests.Models
{
    [TestClass]
    public class ExpandoXmlObjectTests
    {
        string _expectedName = "Testing Extendo Objects";
        string _xml;

        [TestInitialize]
        public void Setup() 
        {
            _xml = @"
<UrlDefinition xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/Grain.Tests.Models.TestModels"">
    <Name>" + _expectedName + @"</Name>
    <Description>This is a test.</Description>
    <Url>http://google.com</Url>
</UrlDefinition>
";        
        }


        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlGetTest()
        {
            
            dynamic _extendo = new ExpandoXmlObject(_xml, typeof(UrlDefinition));
            string _name = _extendo.Name;
            Assert.AreEqual(_name, _expectedName);

            var _objWExt = new UrlObjectForXml();
            _objWExt.ValueString = _xml;
            _objWExt.Value.Name = _expectedName;
            Assert.AreEqual(_objWExt.Value.Name, _expectedName);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlSetTest()
        {
            // Reset a value and make sure it sticks
            dynamic _extendo = new ExpandoXmlObject(_xml, typeof(UrlDefinition));
            string _expected = "HelloWorld";
            _extendo.Name = _expected;
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, _expected);

            // Get the xml back from the object, and make sure it contains the changed value
            string _xmlResult = _extendo._XmlValue;
            UrlDefinition _result = _xmlResult.FromXml<UrlDefinition>();
            Assert.AreEqual(_result.Name, _expected);

            var _objWExt = new UrlObjectForXml();
            _objWExt.Value.Name = _expected;
            Assert.AreEqual(_objWExt.Value.Name, _expected);

            var _objWExt2 = new UrlObjectForXml();
            _objWExt2.Name = _expected;
            Assert.AreEqual(_objWExt2.Name, _expected);

            var _objWExt3 = new UrlObjectForXml { ValueString = _xml };
            _objWExt3.Description = _expected;
            Assert.AreEqual(_objWExt3.Description, _expected);
            Assert.AreEqual(_objWExt3.Name, _expectedName);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlEmptyConstructorTest()
        {
            dynamic _extendo = new ExpandoXmlObject(typeof(UrlDefinition));
            string _name = _extendo.Name;
            Assert.AreEqual(_name, null);

            _extendo.Name = "HelloWorld";
            Assert.AreEqual(_extendo.Name, "HelloWorld");

            // Get the xml back from the object, and make sure it contains the changed value
            string _xmlResult = _extendo._XmlValue;
            UrlDefinition _result = _xmlResult.FromXml<UrlDefinition>();
            Assert.AreEqual(_result.Name, "HelloWorld");
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlGenericObjectTest()
        {
            string _expected = "HelloWorld";
            dynamic _extendo = new ExpandoXmlObject(typeof(Dictionary<string, object>));
            _extendo.Name = _expected;
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, _expected);

            // Get the xml back from the object, and make sure it contains the changed value
            string _xmlResult = _extendo._XmlValue;
            dynamic _result = _xmlResult.FromXml<Dictionary<string, object>>().ToExpando();
            Assert.AreEqual(_result.Name, _expected);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlGenericNestedObjectsTest()
        {
            string _expected = "HelloWorld";
            dynamic _extendo = new ExpandoXmlObject(typeof(ExpandoObject));
            _extendo.Name = _expected;
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, _expected);

            // test a nested strongly typed object
            _extendo.NestedUrl = new UrlDefinition
            {
                Name = _expected,
                Description = "FooBar",
                Url = "http://google.com",
                UrlInfo = new UrlInfoDefinition { 
                    TimeCreated = DateTime.Now,
                    CreatorId = 5
                }
            };
            _actual = _extendo.NestedUrl.Name;
            Assert.AreEqual(_actual, _expected);
            int _creatorId = _extendo.NestedUrl.UrlInfo.CreatorId;  // test nested data
            Assert.AreEqual(_creatorId, 5);

            // test a nested object
            dynamic _nest = new ExpandoObject();
            _nest.Name = _expected;
            //_nest.Foo = "Bar";
            _extendo.Nest = _nest;
            _actual = _extendo.Nest.Name;
            Assert.AreEqual(_actual, _expected);

            string _xmlResult = _extendo._XmlValue;
            dynamic _result = _xmlResult.FromXml<Dictionary<string, object>>().ToExpando();
            Assert.AreEqual(_result.Name, _expected);
            Assert.AreEqual(_result.NestedUrl.Name, _expected);
            Assert.AreEqual(_result.Nest.Name, _expected);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoXmlDictionaryTest() 
        {
            dynamic _extendo = new ExpandoXmlObject(typeof(Dictionary<string, object>));
            _extendo.Name = "HelloWorld";
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, "HelloWorld");

            // Get the xml back from the object, and make sure it contains the changed value
            string _xmlResult = _extendo._XmlValue;
            dynamic _result = new ExpandoXmlObject(_xmlResult, typeof(Dictionary<string, object>));
            Assert.AreEqual(_result.Name, "HelloWorld");
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void EmptyPropertyTest()
        {
            UrlObjectForXml _extendo = new UrlObjectForXml();
            var _something = _extendo.Value.Something;
            Assert.IsNull(_something);
        }
    }
}
