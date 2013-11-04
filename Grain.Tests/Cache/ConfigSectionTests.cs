using System;
using Grain.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Grain.Tests.Cache
{
    [TestClass]
    public class ConfigSectionTests
    {
        [TestMethod]
        public void GetProfileTest()
        {
            var _profile = CacheProfileManager.GetProfile("FiveMinutes");
            Assert.AreEqual(_profile.Name, "FiveMinutes");
            Assert.AreEqual(_profile.Description, "5 minute cache");
            Assert.AreEqual(_profile.Group, "timed");
            Assert.AreEqual(_profile.ExpiresIn, new TimeSpan(0, 5, 0));
        }
    }
}
