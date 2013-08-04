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
    public class ExpandoJsonObjectTests
    {
        string _expectedName = "Testing Extendo Objects";
        string _json;

        [TestInitialize]
        public void Setup() 
        {
            _json = @"{""Name"":""Testing Extendo Objects"",""NestedUrl"":{""Name"":""HelloWorld"",""Description"":""FooBar"",""Url"":""http://google.com"",""UrlInfo"":{""TimeCreated"":""2013-03-19T09:31:12.0149394-04:00"",""CreatorId"":5}},""Nest"":{""Name"":""HelloWorld""}}";        
        }


        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonGetTest()
        {
            dynamic _extendo = new ExpandoJsonObject(_json, typeof(UrlDefinition));
            string _name = _extendo.Name;
            Assert.AreEqual(_name, _expectedName);

            var _objWExt = new UrlObjectForJson();
            _objWExt.ValueString = _json;
            _objWExt.Value.Name = _expectedName;
            Assert.AreEqual(_objWExt.Value.Name, _expectedName);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonSetTest()
        {
            // Reset a value and make sure it sticks
            dynamic _extendo = new ExpandoJsonObject(_json, typeof(UrlDefinition));
            string _expected = "HelloWorld";
            string _expected2 = "HelloWorld2";
            _extendo.Name = _expected;
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, _expected);

            // Get the xml back from the object, and make sure it contains the changed value
            string _jsonResult = _extendo._JsonValue;
            UrlDefinition _result = _jsonResult.FromJson<UrlDefinition>();
            Assert.AreEqual(_result.Name, _expected);

            dynamic _extendo2 = new ExtendableObjectForJson();
            _extendo2.Value.Name = _expected;
            Assert.AreEqual(_extendo2.Value.Name, _expected);
            _extendo2.Value.Name = _expected2;
            Assert.AreEqual(_extendo2.Value.Name, _expected2);

            var _objWExt = new UrlObjectForJson();
            _objWExt.Value.Name = _expected;
            Assert.AreEqual(_objWExt.Value.Name, _expected);
            _objWExt.Value.Name = _expected2;
            Assert.AreEqual(_objWExt.Value.Name, _expected2);

            var _objWExt2 = new UrlObjectForJson();
            _objWExt2.Name = _expected;
            Assert.AreEqual(_objWExt2.Name, _expected);
            _objWExt2.Name = _expected2;
            Assert.AreEqual(_objWExt2.Name, _expected2);

            var _objWExt3 = new UrlObjectForJson { ValueString = _json };
            _objWExt3.Description = _expected;
            Assert.AreEqual(_objWExt3.Description, _expected);
            Assert.AreEqual(_objWExt3.Name, _expectedName);

            var _objWExt4 = new ExtendableObjectForJson();
            ((IDictionary<string, object>)_objWExt4.Value._DataAsObject)["Name"] = _expected;
            Assert.AreEqual(_objWExt4.Value.Name, _expected);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonEmptyConstructorTest()
        {
            dynamic _extendo = new ExpandoJsonObject(typeof(UrlDefinition));
            string _name = _extendo.Name;
            Assert.AreEqual(_name, null);

            _extendo.Name = "HelloWorld";
            Assert.AreEqual(_extendo.Name, "HelloWorld");

            // Get the xml back from the object, and make sure it contains the changed value
            string _jsonResult = _extendo._JsonValue;
            UrlDefinition _result = _jsonResult.FromJson<UrlDefinition>();
            Assert.AreEqual(_result.Name, "HelloWorld");
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonGenericObjectTest()
        {
            string _expected = "HelloWorld";
            dynamic _extendo = new ExpandoJsonObject(typeof(ExpandoObject));
            _extendo.Name = _expected;
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, _expected);

            // Get the xml back from the object, and make sure it contains the changed value
            string _jsonResult = _extendo._JsonValue;
            dynamic _result = _jsonResult.FromJson<ExpandoObject>();
            Assert.AreEqual(_result.Name, _expected);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonGenericNestedObjectsTest()
        {
            string _expected = "HelloWorld";
            dynamic _extendo = new ExpandoJsonObject(typeof(ExpandoObject));
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

            string _jsonResult = _extendo._JsonValue;
            dynamic _result = _jsonResult.FromJson<ExpandoObject>();
            Assert.AreEqual(_result.Name, _expected);
            Assert.AreEqual(_result.NestedUrl.Name, _expected);
            Assert.AreEqual(_result.Nest.Name, _expected);
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExpandoJsonDictionaryTest() 
        {
            dynamic _extendo = new ExpandoJsonObject(typeof(Dictionary<string, object>));
            _extendo.Name = "HelloWorld";
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, "HelloWorld");

            // Get the xml back from the object, and make sure it contains the changed value
            string _jsonResult = _extendo._JsonValue;
            dynamic _result = new ExpandoJsonObject(_jsonResult, typeof(Dictionary<string, object>));
            Assert.AreEqual(_result.Name, "HelloWorld");
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void ExtendableObjectTest() 
        {
            UrlObjectForJson _extendo = new UrlObjectForJson();
            _extendo.Name = "HelloWorld";
            string _actual = _extendo.Name;
            Assert.AreEqual(_actual, "HelloWorld");

            // Get the xml back from the object, and make sure it contains the changed value
            string _jsonResult = _extendo.Value._JsonValue;
            dynamic _result = new ExpandoJsonObject(_jsonResult, typeof(UrlDefinition));
            Assert.AreEqual(_result.Name, "HelloWorld");
        }

        [TestMethod]
        [TestCategory("Grain.Dynamic")]
        public void EmptyPropertyTest()
        {
            UrlObjectForJson _extendo = new UrlObjectForJson();
            var _something = _extendo.Value.Something;
            Assert.IsNull(_something);
        }
    }
}
