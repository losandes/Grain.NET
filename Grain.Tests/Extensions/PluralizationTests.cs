using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class PluralizationTests
    {
        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void PluralizeTest()
        {
            string _expected = "Statuses";
            string _actual = "Status".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "Bicycles";
            _actual = "Bicycle".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "BonFires";
            _actual = "BonFire".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "HasAwesomeBonFires";
            _actual = "HasAwesomeBonFire".Pluralize();
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void SingularizeTest()
        {
            string _expected = "Status";
            string _actual = "Statuses".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "Bicycle";
            _actual = "Bicycles".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "BonFire";
            _actual = "BonFires".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "HasAwesomeBonFire";
            _actual = "HasAwesomeBonFires".Singularize();
            Assert.AreEqual(_expected, _actual);
        }
    }
}
