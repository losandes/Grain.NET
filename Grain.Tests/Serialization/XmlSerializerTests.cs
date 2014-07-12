using System;
using Grain.Tests.Models.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Serialization;
using System.Linq;
using System.Dynamic;
using System.Collections.Generic;

namespace Grain.Tests.Serialization
{
    [TestClass]
    public class XmlSerializerTests
    {
        #region Setup

        string _testModelXml = "<TestModel xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Grain.Tests.Models.TestModels\">\r\n  <Description>Foo Bar</Description>\r\n  <Id>1</Id>\r\n  <Name>Foo</Name>\r\n  <TimeCreated>2012-12-04T14:50:06.4559307Z</TimeCreated>\r\n</TestModel>";

        string _testModelWithCollection = "<TestModelWithCollections xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.datacontract.org/2004/07/Grain.Tests.Models.TestModels\">\r\n  <Description>Hello Bar</Description>\r\n  <Id>2</Id>\r\n  <Name>Hello</Name>\r\n  <TimeCreated>2012-12-04T14:50:06.4559307Z</TimeCreated>\r\n  <Children>\r\n    <TestModel>\r\n      <Description>Foo Bar</Description>\r\n      <Id>3</Id>\r\n      <Name>Foo</Name>\r\n      <TimeCreated>2012-12-04T14:50:06.4559307Z</TimeCreated>\r\n    </TestModel>\r\n  </Children>\r\n  <Dictionary xmlns:d2p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">\r\n    <d2p1:KeyValueOfstringstring>\r\n      <d2p1:Key>Foo</d2p1:Key>\r\n      <d2p1:Value>Bar</d2p1:Value>\r\n    </d2p1:KeyValueOfstringstring>\r\n    <d2p1:KeyValueOfstringstring>\r\n      <d2p1:Key>Hello</d2p1:Key>\r\n      <d2p1:Value>World</d2p1:Value>\r\n    </d2p1:KeyValueOfstringstring>\r\n  </Dictionary>\r\n  <StringCollection xmlns:d2p1=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">\r\n    <d2p1:string>Foo</d2p1:string>\r\n    <d2p1:string>Bar</d2p1:string>\r\n  </StringCollection>\r\n</TestModelWithCollections>";

        #endregion Setup

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ToXmlTest()
        {
            string _expected = _testModelXml;
            string _actual = TestModelInstances.SimpleModel.ToXml();
            Assert.AreEqual(_expected, _actual);

            _expected = _testModelWithCollection;
            _actual = TestModelInstances.ModelWithCollections.ToXml();
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void FromXmlTest()
        {
            var _actual = _testModelXml.FromXml<TestModel>();
            Assert.AreEqual(_actual.Id, 1);
            Assert.AreEqual(_actual.Name, "Foo");

            var _actualCol = _testModelWithCollection.FromXml<TestModelWithCollections>();
            Assert.AreEqual(_actualCol.Id, 2);
            Assert.AreEqual(_actualCol.Name, "Hello");
            Assert.AreEqual(_actualCol.Children.First().Name, "Foo");
            Assert.AreEqual(_actualCol.Children.First().Description, "Foo Bar");
            Assert.AreEqual(_actualCol.Dictionary.First().Key, "Foo");
            Assert.AreEqual(_actualCol.Dictionary.First().Value, "Bar");
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ToXmlTest_NoContract()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void FromXmlTest_NoContract()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void DynamicToXmlTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void DynamicFromXmlTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ExpandoXmlSerializationTest()
        {
            dynamic _expected = new ExpandoObject();
            _expected.Foo = "Bar";
            _expected.Number = 42;
            _expected.Date = new DateTime(2013, 8, 3);

            string _expectedXml = "<ArrayOfKeyValueOfstringanyType xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/Arrays\">\r\n  <KeyValueOfstringanyType>\r\n    <Key>Foo</Key>\r\n    <Value xmlns:d3p1=\"http://www.w3.org/2001/XMLSchema\" i:type=\"d3p1:string\">Bar</Value>\r\n  </KeyValueOfstringanyType>\r\n  <KeyValueOfstringanyType>\r\n    <Key>Number</Key>\r\n    <Value xmlns:d3p1=\"http://www.w3.org/2001/XMLSchema\" i:type=\"d3p1:int\">42</Value>\r\n  </KeyValueOfstringanyType>\r\n  <KeyValueOfstringanyType>\r\n    <Key>Date</Key>\r\n    <Value xmlns:d3p1=\"http://www.w3.org/2001/XMLSchema\" i:type=\"d3p1:dateTime\">2013-08-03T00:00:00</Value>\r\n  </KeyValueOfstringanyType>\r\n</ArrayOfKeyValueOfstringanyType>";
            string _actualXml = Grain.Serialization.XMLSerializer.ToXml(_expected); //Grain.Serialization.XMLSerializer.ToXml(_expected);
            Assert.AreEqual(_expectedXml, _actualXml);

            //dynamic _actual = _actualXml.FromXml<IDictionary<string, object>>().ToExpando();
            dynamic _actual = _actualXml.FromXml<ExpandoObject>().ToExpando();
            object _actualAsObject = (object)_actual;
            Assert.IsNotNull(_actualAsObject);
            Assert.AreEqual(_expected.Foo, _actual.Foo);
            Assert.AreEqual(_expected.Number, _actual.Number);
            Assert.AreEqual(_expected.Date, _actual.Date);
        }
    }
}
