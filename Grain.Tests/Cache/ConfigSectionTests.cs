using System;
using Grain.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;

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

            var _neverExpiresProfile = CacheProfileManager.GetProfile("NeverExpires");
            Assert.AreEqual(_neverExpiresProfile.Name, "NeverExpires");
            Assert.AreEqual(_neverExpiresProfile.Description, "never expires");
            Assert.IsTrue(_neverExpiresProfile.Group.IsEmptyOrWhiteSpace());
            Assert.IsTrue(_neverExpiresProfile.ExpiresIn.HasValue == false);
        }
    }
}
