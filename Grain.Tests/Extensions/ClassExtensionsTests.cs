using System;
using System.Collections.Generic;
using System.Diagnostics;
using Grain.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Grain.Tests.Models.TestModels;
using System.Reflection;

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class ClassExtensionsTests
    {
        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void GetTypesInNamespaceTest()
        {
            var _types = Assembly.GetExecutingAssembly().FindTypesInNamespace("Grain.Tests.Extensions");
            Assert.IsNotNull(_types.FirstOrDefault(t => t.Name == "ClassExtensionsTests"));
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void GetInstanceOfTest()
        {
            TestModel _actual = ClassExtensions.GetInstanceOf<TestModel>();
            Assert.IsInstanceOfType(_actual, typeof(TestModel));

            TestModel _expected = new TestModel { 
                Id = 1,
                Name = "Foo",
                Description = "HelloWorld!"
            };
            _actual = ClassExtensions.GetInstanceOf<TestModel>(new Type[] { typeof(int), typeof(string), typeof(string) }
                , new object[]{ 1, "Foo", "HelloWorld!" });
            Assert.AreEqual(_expected.Id, _actual.Id);
            Assert.AreEqual(_expected.Name, _actual.Name);
            Assert.AreEqual(_expected.Description, _actual.Description);
        }

        //[TestMethod]
        public void GetInstanceOfPerformanceTest()
        {
            var steps = new List<long>();
            var watch = new Stopwatch();

            for (int i = 0; i < 100; i++)
            {
                watch.Reset();
                watch.Start();
                TestModel _actual = ClassExtensions.GetInstanceOf<TestModel>();
                _actual.Id = 1;
                watch.Stop();
                Console.WriteLine(watch.ElapsedMilliseconds.ToString());
                if (i != 0)
                    steps.Add(watch.ElapsedMilliseconds);
            }
            var _avg = steps.Average();
            string _avgString = _avg.ToString();
            Console.WriteLine("Average: " + _avgString);
            //100:    0 with invoke
            //1000:   0 with invoke
            //10000:  0.00060006000600060011 with invoke
            //100000: 0.000050000500005000053 with invoke

            //100:    0.0606060606060606 with GetInstance()
            //1000:   0.00600600600600601 with GetInstance()
            //10000:  0.0036003600360036 with GetInstance()
            //100000: 0.003090030900309 with GetInstance()
        }
    }
}
