using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void ToListTest()
        {
            string _test = "hello,world,foo,bar";
            var _list = _test.ToList(',');
            Assert.IsTrue(_list.Contains("hello"));
            Assert.IsTrue(_list.Contains("world"));
            Assert.IsTrue(_list.Contains("foo"));
            Assert.IsTrue(_list.Contains("bar"));
            Assert.IsTrue(_list.Count == 4);

            _test = "hello, world, foo, bar";
            _list = _test.ToList(',');
            Assert.IsTrue(_list.Contains("hello"));
            Assert.IsTrue(_list.Contains("world"));
            Assert.IsFalse(_list.Contains(" world"));
            Assert.IsTrue(_list.Contains("foo"));
            Assert.IsTrue(_list.Contains("bar"));
            Assert.IsTrue(_list.Count == 4);

            _test = "";
            _list = _test.ToList(',');
            Assert.IsNotNull(_list);
            Assert.IsTrue(_list.Count == 0);
        }
    }
}
