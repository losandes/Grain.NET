using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Grain.Tests.Models.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions; 

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class EnumExtensionsTests
    {
        MockEnumFlag _mockFlags = (MockEnumFlag)23;
        MockLongEnumFlag _mockLongFlags = (MockLongEnumFlag)23;

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void AsEnumerableIntsTest()
        {
            // Ensure that we get a list of integers and the first int is not 0
            var _result = _mockFlags.AsEnumerableInts(true);
            var _resultAsList = _result.ToList();
            Assert.IsTrue(_resultAsList.Count > 0);
            Assert.IsFalse(_resultAsList[0] == 0);
            Assert.IsInstanceOfType(_resultAsList.First(), typeof(int));

            // Ensure that we get a list of integers and the first int is 0
            _result = _mockFlags.AsEnumerableInts(false);
            _resultAsList = _result.ToList();
            Assert.IsTrue(_resultAsList.Count > 0);
            Assert.IsTrue(_resultAsList[0] == 0);
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void AsEnumerableNamesTest()
        {
            // Ensure that we get a list of strings
            var _result = _mockFlags.AsEnumerableNames<MockEnumFlag>();
            var _resultAsList = _result.ToList();
            Assert.IsTrue(_resultAsList.Count > 0);
            Assert.IsInstanceOfType(_resultAsList.First(), typeof(string));
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void AsEnumerableTest()
        {
            // Ensure that we get a list of enums and that both the int and the string
            var _result = _mockFlags.AsEnumerable(true);
            var _resultAsList = _result.ToList();
            Assert.IsTrue(_resultAsList.Count > 0);
            Assert.IsFalse(Enum.GetName(typeof(MockEnumFlag), _resultAsList[0]) == Enum.GetName(typeof(MockEnumFlag), MockEnumFlag.None));
            Assert.IsFalse(((int)(object)_resultAsList[0]) == ((int)(object)MockEnumFlag.None));
            Assert.IsInstanceOfType(_resultAsList.First(), typeof(MockEnumFlag));

            _result = _result = _mockFlags.AsEnumerable(false);
            _resultAsList = _result.ToList();
            Assert.IsTrue(_resultAsList.Count > 0);
            Assert.IsTrue(Enum.GetName(typeof(MockEnumFlag), _resultAsList[0]) == Enum.GetName(typeof(MockEnumFlag), MockEnumFlag.None));
            Assert.IsTrue(((int)(object)_resultAsList[0]) == ((int)(object)MockEnumFlag.None));
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void LongAsEnumerableTest()
        {
            try
            {
                // Ensure that we get a list of enums and that both the int and the string
                var _result = _mockLongFlags.AsEnumerable(true);
                var _resultAsList = _result.ToList();
                Assert.Fail("LongAsEnumerableTest should have failed");
            }
            catch 
            {
                Assert.IsTrue(true); // LongAsEnumerableTest should have failed
            }
        }
    }
}
