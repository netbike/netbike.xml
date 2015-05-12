namespace NetBike.Xml.Tests.Converters.Specialized
{
    using System;
    using System.Collections.Generic;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NetBike.Xml.Converters;
    using NUnit.Framework;

    public abstract class XmlKeyValuePairConverterTestsBase
    {
        [Test]
        public void CanReadTest()
        {
            var converter = GetConverter();
            Assert.IsTrue(converter.CanRead(typeof(KeyValuePair<string, object>)));
            Assert.IsTrue(converter.CanRead(typeof(KeyValuePair<int, int>)));
            Assert.IsFalse(converter.CanRead(typeof(DateTime)));
            Assert.IsFalse(converter.CanRead(typeof(KeyValuePair<,>)));
        }

        [Test]
        public void CanWriteTest()
        {
            var converter = GetConverter();
            Assert.IsTrue(converter.CanWrite(typeof(KeyValuePair<string, object>)));
            Assert.IsTrue(converter.CanWrite(typeof(KeyValuePair<int, int>)));
            Assert.IsFalse(converter.CanWrite(typeof(DateTime)));
            Assert.IsFalse(converter.CanWrite(typeof(KeyValuePair<,>)));
        }

        [Test]
        public void WriteKeyValuePairTest()
        {
            var converter = GetConverter();
            var value = new KeyValuePair<int, string>(1, "item");
            var expected = "<xml><key>1</key><value>item</value></xml>";
            var actual = converter.ToXml(value.GetType(), value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadKeyValuePairTest()
        {
            var converter = GetConverter();
            var xml = "<xml><key>1</key><value>item</value></xml>";
            var actual = converter.ParseXml<KeyValuePair<int, string>>(xml);
            var expected = new KeyValuePair<int, string>(1, "item");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteKeyValuePairWithCustomContractTest()
        {
            var converter = GetConverter();
            var value = new KeyValuePair<int, string>(1, "item");
            var expected = "<xml id=\"1\">item</xml>";
            var actual = converter.ToXml(value, contract: GetCustomContract());
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadKeyValuePairWithCustomContractTest()
        {
            var converter = GetConverter();
            var xml = "<xml id=\"1\">item</xml>";
            var actual = converter.ParseXml<KeyValuePair<int, string>>(xml, contract: GetCustomContract());
            var expected = new KeyValuePair<int, string>(1, "item");

            Assert.AreEqual(expected, actual);
        }

        protected abstract IXmlConverter GetConverter();

        private static XmlContract GetCustomContract()
        {
            return new XmlObjectContractBuilder<KeyValuePair<int, string>>()
                .SetName("test")
                .SetProperty(x => x.Key, p => p.SetName("id").SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Value, p => p.SetName("value").SetMappingType(XmlMappingType.InnerText))
                .Build();
        }
    }
}