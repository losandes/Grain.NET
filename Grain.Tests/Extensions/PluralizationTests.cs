using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;
using Grain.Repositories;
using System.Collections.Generic;

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

            _expected = "Quizzes";
            _actual = "Quiz".Pluralize();
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

            _expected = "Quiz";
            _actual = "Quizzes".Singularize();
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

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void PluralizationRepoRunTimeExtensionsTest()
        {
            PluralizationRepository.Instance.AddPluralizationOverride("andy", "andes");
            PluralizationRepository.Instance.AddPluralizationOverrides(new Dictionary<string,string> { {"fourtytwo", "theanswer"}, {"jackierobinson", "numberfourtytwo"} });

            var _expected = "andes";
            var _actual = "andy".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "andy";
            _actual = "andes".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "fourtytwo";
            _actual = "theanswer".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "theanswer";
            _actual = "fourtytwo".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "jackierobinson";
            _actual = "numberfourtytwo".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "numberfourtytwo";
            _actual = "jackierobinson".Pluralize();
            Assert.AreEqual(_expected, _actual);

            PluralizationRepository.Instance.RemovePluralizationOverride("andy");
            PluralizationRepository.Instance.RemovePluralizationOverrides(new Dictionary<string, string> { { "fourtytwo", "theanswer" }, { "jackierobinson", "numberfourtytwo" } });

            _expected = "andies";
            _actual = "andy".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "andy";
            _actual = "andies".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "fourtytwo";
            _actual = "fourtytwoes".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "fourtytwoes";
            _actual = "fourtytwo".Pluralize();
            Assert.AreEqual(_expected, _actual);

            _expected = "jackierobinson";
            _actual = "jackierobinsons".Singularize();
            Assert.AreEqual(_expected, _actual);

            _expected = "jackierobinsons";
            _actual = "jackierobinson".Pluralize();
            Assert.AreEqual(_expected, _actual);
        }
    }
}
