using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class DateTimeExtensionsTests
    {
        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void IsBeforeNowTest()
        {
            DateTime _inThePast = DateTime.Now - new TimeSpan(365,0,0,0);
            Assert.IsTrue(_inThePast.IsInThePast());
            Assert.IsTrue(_inThePast.IsInThePast(new TimeSpan(364,0,0,0)));
            Assert.IsFalse(_inThePast.IsInThePast(new TimeSpan(366, 0, 0, 0)));

            _inThePast = DateTime.Now + new TimeSpan(365, 0, 0, 0);
            Assert.IsFalse(_inThePast.IsInThePast());
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void IsAfterNowTest()
        {
            DateTime _inTheFuture = DateTime.Now + new TimeSpan(365, 0, 0, 0);
            Assert.IsTrue(_inTheFuture.IsInTheFuture());
            Assert.IsTrue(_inTheFuture.IsInTheFuture(new TimeSpan(364, 0, 0, 0)));
            Assert.IsFalse(_inTheFuture.IsInTheFuture(new TimeSpan(366, 0, 0, 0)));

            _inTheFuture = DateTime.Now - new TimeSpan(365, 0, 0, 0);
            Assert.IsFalse(_inTheFuture.IsInTheFuture());
        }
    }
}
