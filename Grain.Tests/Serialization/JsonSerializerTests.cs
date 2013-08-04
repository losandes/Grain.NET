using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Serialization;
using Grain.Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Grain.Tests.Models.TestModels;
using System.Dynamic;

namespace Grain.Tests.Serialization
{
    [TestClass]
    public class JsonSerializerTests
    {
        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ToJsonTest()
        {
            string _expected = "{\"Name\":\"Foo\",\"Description\":\"Foo Bar\",\"Id\":1,\"TimeCreated\":\"2012-12-04T14:50:06.4559307-05:00\"}";
            string _actual = TestModelInstances.SimpleModel.ToJson();
            Assert.AreEqual(_expected, _actual);

            _expected = "{\"StringCollection\":[\"Foo\",\"Bar\"],\"Dictionary\":{\"Foo\":\"Bar\",\"Hello\":\"World\"},\"Children\":[{\"Name\":\"Foo\",\"Description\":\"Foo Bar\",\"Id\":3,\"TimeCreated\":\"2012-12-04T14:50:06.4559307-05:00\"}],\"Name\":\"Hello\",\"Description\":\"Hello Bar\",\"Id\":2,\"TimeCreated\":\"2012-12-04T14:50:06.4559307-05:00\"}";
            _actual = TestModelInstances.ModelWithCollections.ToJson();
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ToBsonTest()
        {
            string _expected = "49-00-00-00-02-4E-61-6D-65-00-04-00-00-00-46-6F-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-08-00-00-00-46-6F-6F-20-42-61-72-00-10-49-64-00-01-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00";
            byte[] _actualBytes = TestModelInstances.SimpleModel.ToBson();
            string _actual = BitConverter.ToString(_actualBytes);
            Assert.AreEqual(_expected, _actual);

            _expected = "04-01-00-00-04-53-74-72-69-6E-67-43-6F-6C-6C-65-63-74-69-6F-6E-00-1B-00-00-00-02-30-00-04-00-00-00-46-6F-6F-00-02-31-00-04-00-00-00-42-61-72-00-00-03-44-69-63-74-69-6F-6E-61-72-79-00-23-00-00-00-02-46-6F-6F-00-04-00-00-00-42-61-72-00-02-48-65-6C-6C-6F-00-06-00-00-00-57-6F-72-6C-64-00-00-04-43-68-69-6C-64-72-65-6E-00-51-00-00-00-03-30-00-49-00-00-00-02-4E-61-6D-65-00-04-00-00-00-46-6F-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-08-00-00-00-46-6F-6F-20-42-61-72-00-10-49-64-00-03-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00-00-02-4E-61-6D-65-00-06-00-00-00-48-65-6C-6C-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-0A-00-00-00-48-65-6C-6C-6F-20-42-61-72-00-10-49-64-00-02-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00";
            _actualBytes = TestModelInstances.ModelWithCollections.ToBson();
            _actual = BitConverter.ToString(_actualBytes);
            Assert.AreEqual(_expected, _actual);

            _expected = "0C-01-00-00-03-30-00-04-01-00-00-04-53-74-72-69-6E-67-43-6F-6C-6C-65-63-74-69-6F-6E-00-1B-00-00-00-02-30-00-04-00-00-00-46-6F-6F-00-02-31-00-04-00-00-00-42-61-72-00-00-03-44-69-63-74-69-6F-6E-61-72-79-00-23-00-00-00-02-46-6F-6F-00-04-00-00-00-42-61-72-00-02-48-65-6C-6C-6F-00-06-00-00-00-57-6F-72-6C-64-00-00-04-43-68-69-6C-64-72-65-6E-00-51-00-00-00-03-30-00-49-00-00-00-02-4E-61-6D-65-00-04-00-00-00-46-6F-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-08-00-00-00-46-6F-6F-20-42-61-72-00-10-49-64-00-03-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00-00-02-4E-61-6D-65-00-06-00-00-00-48-65-6C-6C-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-0A-00-00-00-48-65-6C-6C-6F-20-42-61-72-00-10-49-64-00-02-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00-00";
            _actualBytes = new List<TestModelWithCollections> { TestModelInstances.ModelWithCollections }.ToBson();
            _actual = BitConverter.ToString(_actualBytes);
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void FromJsonTest()
        {
            string _json = TestModelInstances.SimpleModel.ToJson();
            TestModel _actual = _json.FromJson<TestModel>();
            Assert.AreEqual(TestModelInstances.SimpleModel.Name, _actual.Name);
            Assert.AreEqual(TestModelInstances.SimpleModel.Description, _actual.Description);
            Assert.AreEqual(TestModelInstances.SimpleModel.Id, _actual.Id);
            Assert.AreEqual(TestModelInstances.SimpleModel.TimeCreated.Day, _actual.TimeCreated.Day);

            _json = TestModelInstances.ModelWithCollections.ToJson();
            TestModelWithCollections _actual2 = _json.FromJson<TestModelWithCollections>();
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Name, _actual2.Name);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Description, _actual2.Description);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Id, _actual2.Id);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.TimeCreated.Day, _actual2.TimeCreated.Day);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.StringCollection.First(), _actual2.StringCollection.First());
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Dictionary.First().Key, _actual2.Dictionary.First().Key);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Children.First().Name, _actual2.Children.First().Name);

            _json = new List<TestModelWithCollections> { TestModelInstances.ModelWithCollections }.ToJson();
            IEnumerable<TestModelWithCollections> _actual3 = _json.FromJson<IEnumerable<TestModelWithCollections>>();
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Name, _actual3.First().Name);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Description, _actual3.First().Description);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Id, _actual3.First().Id);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.TimeCreated.Day, _actual3.First().TimeCreated.Day);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.StringCollection.First(), _actual3.First().StringCollection.First());
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Dictionary.First().Key, _actual3.First().Dictionary.First().Key);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Children.First().Name, _actual3.First().Children.First().Name);
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void FromBsonTest()
        {
            byte[] _json = TestModelInstances.SimpleModel.ToBson();
            TestModel _actual = _json.FromBson<TestModel>();
            Assert.AreEqual(TestModelInstances.SimpleModel.Name, _actual.Name);
            Assert.AreEqual(TestModelInstances.SimpleModel.Description, _actual.Description);
            Assert.AreEqual(TestModelInstances.SimpleModel.Id, _actual.Id);
            Assert.AreEqual(TestModelInstances.SimpleModel.TimeCreated.Day, _actual.TimeCreated.Day);

            _json = TestModelInstances.ModelWithCollections.ToBson();
            TestModelWithCollections _actual2 = _json.FromBson<TestModelWithCollections>();
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Name, _actual2.Name);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Description, _actual2.Description);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Id, _actual2.Id);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.TimeCreated.Day, _actual2.TimeCreated.Day);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.StringCollection.First(), _actual2.StringCollection.First());
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Dictionary.First().Key, _actual2.Dictionary.First().Key);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Children.First().Name, _actual2.Children.First().Name);

            _json = new List<TestModelWithCollections> { TestModelInstances.ModelWithCollections }.ToBson();
            IEnumerable<TestModelWithCollections> _actual3 = _json.FromBson<IEnumerable<TestModelWithCollections>>();
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Name, _actual3.First().Name);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Description, _actual3.First().Description);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Id, _actual3.First().Id);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.TimeCreated.Day, _actual3.First().TimeCreated.Day);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.StringCollection.First(), _actual3.First().StringCollection.First());
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Dictionary.First().Key, _actual3.First().Dictionary.First().Key);
            Assert.AreEqual(TestModelInstances.ModelWithCollections.Children.First().Name, _actual3.First().Children.First().Name);

            //_expected = "0C-01-00-00-03-30-00-04-01-00-00-04-53-74-72-69-6E-67-43-6F-6C-6C-65-63-74-69-6F-6E-00-1B-00-00-00-02-30-00-04-00-00-00-46-6F-6F-00-02-31-00-04-00-00-00-42-61-72-00-00-03-44-69-63-74-69-6F-6E-61-72-79-00-23-00-00-00-02-46-6F-6F-00-04-00-00-00-42-61-72-00-02-48-65-6C-6C-6F-00-06-00-00-00-57-6F-72-6C-64-00-00-04-43-68-69-6C-64-72-65-6E-00-51-00-00-00-03-30-00-49-00-00-00-02-4E-61-6D-65-00-04-00-00-00-46-6F-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-08-00-00-00-46-6F-6F-20-42-61-72-00-10-49-64-00-03-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00-00-02-4E-61-6D-65-00-06-00-00-00-48-65-6C-6C-6F-00-02-44-65-73-63-72-69-70-74-69-6F-6E-00-0A-00-00-00-48-65-6C-6C-6F-20-42-61-72-00-10-49-64-00-02-00-00-00-09-54-69-6D-65-43-72-65-61-74-65-64-00-77-DB-77-67-3B-01-00-00-00-00";
            //_actualBytes = new List<TestModelWithCollections> { TestModelInstances.ModelWithCollections }.ToBson();
            //_actual = BitConverter.ToString(_actualBytes);
            //Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void ExpandoJsonSerializationTest()
        {
            dynamic _expected = new ExpandoObject();
            _expected.Foo = "Bar";
            _expected.Number = 42;
            _expected.Date = new DateTime(2013, 8, 3);

            string _expectedJson = "{\"Foo\":\"Bar\",\"Number\":42,\"Date\":\"2013-08-03T00:00:00\"}";
            string _actualJson = Grain.Serialization.JsonSerializer.ToJson(_expected);
            Assert.AreEqual(_expectedJson, _actualJson);

            dynamic _actual = _actualJson.FromJson<ExpandoObject>();
            Assert.AreEqual(_expected.Foo, _actual.Foo);
            Assert.AreEqual(_expected.Number, _actual.Number);
            Assert.AreEqual(_expected.Date, _actual.Date);
        }
    }
}
