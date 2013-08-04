using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Extensions;

namespace Grain.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void RemoveSpecialCharactersTest()
        {
            string _expected = "Hello World";
            string _actual = "Hello World!@$%^&*".RemoveSpecialCharacters();
            Assert.AreEqual(_expected, _actual);

            _expected = "Hello_World";
            _actual = "Hello_World!".RemoveSpecialCharacters(true);
            Assert.AreEqual(_expected, _actual);
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void RemoveLastCharacterForwardSlashTest() 
        {
            string _base = "http://google.com";
            string _actual = _base + "/";
            Assert.AreEqual(_actual.RemoveLastCharacterForwardSlash(), _base);

            _actual = _base + "//";
            Assert.AreEqual(_actual.RemoveLastCharacterForwardSlash(), _base);

            _actual = _base + "/////";
            Assert.AreEqual(_actual.RemoveLastCharacterForwardSlash(), _base);
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void RemoveFirstCharacterForwardSlashTest()
        {
            string _base = "somepath/";
            string _actual = "/" + _base;
            Assert.AreEqual(_actual.RemoveFirstCharacterForwardSlash(), _base);

            _actual = "//" + _base;
            Assert.AreEqual(_actual.RemoveFirstCharacterForwardSlash(), _base);

            _actual = "/////" + _base;
            Assert.AreEqual(_actual.RemoveFirstCharacterForwardSlash(), _base);
        }

        [TestMethod]
        [TestCategory("Grain.Extensions")]
        public void AppendUrlTest()
        {
            string _base2 = "http://google.com";
            string _base = _base2 + "/";
            string _relativePath = "/somePath/";
            string _relativePath2 = "somePath/";

            string _expected = _base2 + _relativePath;
            string _actual = _base.AppendUrl(_relativePath);
            Assert.AreEqual(_expected, _actual);

            _actual = _base2.AppendUrl(_relativePath);
            Assert.AreEqual(_expected, _actual);

            _actual = _base.AppendUrl(_relativePath2);
            Assert.AreEqual(_expected, _actual);

            _actual = _base2.AppendUrl(_relativePath2);
            Assert.AreEqual(_expected, _actual);
        }
    }
}
