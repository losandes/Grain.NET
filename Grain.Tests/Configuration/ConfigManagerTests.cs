using System;
using Grain.Configuration;
using Grain.Extensions;
using Grain.Serialization;
using Grain.Tests.Models.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grain.Tests.Configuration
{
    [TestClass]
    public class ConfigManagerTests
    {
        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void GetConnectionStringTests()
        {
            var _expected = "Data Source=.;Initial Catalog=UnitTests;Integrated Security=True;";
            var _actual = ConfigManager.GetConnectionString("UnitTests");
            Assert.AreEqual(_expected, _actual);

            _actual = ConfigManager.TryGetConnectionString("UnitTests");
            Assert.AreEqual(_expected, _actual);

            _actual = ConfigManager.TryGetConnectionString("NOTUnitTests", "FooBar");
            Assert.AreEqual("FooBar", _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueTest()
        {
            var _actual = ConfigManager.TryGetValue("mockStringSetting", "HelloWorld!");
            Assert.AreEqual("HelloWorld!", _actual);

            _actual = ConfigManager.TryGetValue("missingMockStringSetting", "FooBar");
            Assert.AreEqual("FooBar", _actual);

            _actual = ConfigManager.TryGetValue("mockStringSetting", "FooBar");
            Assert.AreEqual("HelloWorld!", _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueFromBinaryAsTest()
        {
            var _actual = ConfigManager.TryGetValueFromBinaryAs<string>("mockStringSetting", "HelloWorld!");
            Assert.AreEqual("HelloWorld!", _actual);

            var _int = ConfigManager.TryGetValueFromBinaryAs<int>("mockIntSetting", 42);
            Assert.AreEqual(42, _int);

            var _long = ConfigManager.TryGetValueFromBinaryAs<long>("mockIntSetting", 42);
            Assert.AreEqual(42, _long);

            var _decimal = ConfigManager.TryGetValueFromBinaryAs<decimal>("missingMockSetting", 42.42M);
            Assert.AreEqual(42.42M, _decimal);

            var _bool = ConfigManager.TryGetValueFromBinaryAs<bool>("missingMockSetting", true);
            Assert.AreEqual(true, _bool);

            var _date = ConfigManager.TryGetValueFromBinaryAs<DateTime>("missingMockSetting", new DateTime(2013, 4, 2, 11, 42, 36));
            Assert.AreEqual(2013, _date.Year);
            Assert.AreEqual(4, _date.Month);
            Assert.AreEqual(2, _date.Day);
            Assert.AreEqual(11, _date.Hour);
            Assert.AreEqual(42, _date.Minute);
            Assert.AreEqual(36, _date.Second);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueAsSerializedTest()
        {
            var _expected = new TestModel
            {
                Id = 1,
                Name = "foo",
                Description = "bar",
                TimeCreated = DateTime.Now
            };
            var _actual = ConfigManager.TryGetValueAs<TestModel>("mockJsonSetting",SerializationTypes.Json, _expected);
            AssertTestModelsAreEqual(_expected, _actual);

            _actual = ConfigManager.TryGetValueAs<TestModel>("mockJsonSetting", SerializationTypes.Xml, _expected);
            AssertTestModelsAreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueAsFactoryTest()
        {
            var _expected = new TestModel
            {
                Id = 1,
                Name = "foo",
                Description = "bar",
                TimeCreated = DateTime.Now
            };
            var _actual = ConfigManager.TryGetValueAs<TestModel>("mockJsonSetting", _expected, r => r.FromJson<TestModel>(), r => r.ToJson<TestModel>());
            AssertTestModelsAreEqual(_expected, _actual);
        }

        private void AssertTestModelsAreEqual(TestModel expected, TestModel actual) 
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.TimeCreated, actual.TimeCreated);        
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueAsIntTest()
        {
            var _actual = ConfigManager.TryGetValueAsInt("mockIntSetting", 42);
            Assert.AreEqual(42, _actual);

            _actual = ConfigManager.TryGetValueAsInt("missingmockIntSetting", 44);
            Assert.AreEqual(44, _actual);

            _actual = ConfigManager.TryGetValueAsInt("mockIntSetting", 44);
            Assert.AreEqual(42, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueAsBoolTest()
        {
            var _actual = ConfigManager.TryGetValueAsBool("mockBoolSetting", true);
            Assert.AreEqual(true, _actual);

            _actual = ConfigManager.TryGetValueAsBool("missingmockBoolSetting", true);
            Assert.AreEqual(true, _actual);

            _actual = ConfigManager.TryGetValueAsBool("mockBoolSetting", false);
            Assert.AreEqual(true, _actual);
        }

        
        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void TryGetValueAsDateTimeTest()
        {
            var _actual = ConfigManager.TryGetValueAsDateTime("mockDateTimeSetting", new DateTime(2013, 4, 2, 11, 42, 36));
            Assert.AreEqual(2013, _actual.Year);
            Assert.AreEqual(4, _actual.Month);
            Assert.AreEqual(2, _actual.Day);
            Assert.AreEqual(11, _actual.Hour);
            Assert.AreEqual(42, _actual.Minute);
            Assert.AreEqual(36, _actual.Second);

            _actual = ConfigManager.TryGetValueAsDateTime("missingmockDateTimeSetting", new DateTime(2013, 4, 2, 11, 42, 36));
            Assert.AreEqual(2013, _actual.Year);
            Assert.AreEqual(4, _actual.Month);
            Assert.AreEqual(2, _actual.Day);
            Assert.AreEqual(11, _actual.Hour);
            Assert.AreEqual(42, _actual.Minute);
            Assert.AreEqual(36, _actual.Second);

            _actual = ConfigManager.TryGetValueAsDateTime("mockDateTimeSetting", new DateTime(2013, 2, 4, 11, 42, 36));
            Assert.AreEqual(2013, _actual.Year);
            Assert.AreEqual(4, _actual.Month);
            Assert.AreEqual(2, _actual.Day);
            Assert.AreEqual(11, _actual.Hour);
            Assert.AreEqual(42, _actual.Minute);
            Assert.AreEqual(36, _actual.Second);
        }

        [TestMethod]
        [TestCategory("Grain.Configuration")]
        public void GetSectionTest()
        {
            string _expected = "helloWorld";
            var _section = ConfigManager.GetSection<TestSection>("testSection");
            Assert.AreEqual(_expected, _section.TestValue);

            _section = ConfigManager.TryGetSection<TestSection>("testSection", new TestSection { TestValue = _expected });
            Assert.AreEqual(_expected, _section.TestValue);

            _expected = "foobar";
            _section = ConfigManager.TryGetSection<TestSection>("NOTtestSection", new TestSection { TestValue = _expected });
            Assert.AreEqual(_expected, _section.TestValue);
        }
    }
}
