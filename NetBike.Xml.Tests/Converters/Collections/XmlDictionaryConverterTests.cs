namespace NetBike.Xml.Tests.Converters.Collections
{
    using System.Collections;
    using System.Collections.Generic;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Collections;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;

    [TestFixture]
    public class XmlDictionaryConverterTests
    {
        [Test]
        public void CanReadTest()
        {
            var converter = new XmlDictionaryConverter();
            Assert.IsTrue(converter.CanRead(typeof(Dictionary<string, object>)));
            Assert.IsTrue(converter.CanRead(typeof(Dictionary<int, int>)));
            Assert.IsFalse(converter.CanRead(typeof(object)));
            Assert.IsFalse(converter.CanRead(typeof(IDictionary)));
            Assert.IsFalse(converter.CanRead(typeof(Dictionary<,>)));
        }

        [Test]
        public void CanWriteTest()
        {
            var converter = new XmlDictionaryConverter();
            Assert.IsTrue(converter.CanWrite(typeof(Dictionary<string, object>)));
            Assert.IsTrue(converter.CanWrite(typeof(Dictionary<int, int>)));
            Assert.IsFalse(converter.CanWrite(typeof(object)));
            Assert.IsFalse(converter.CanWrite(typeof(IDictionary)));
            Assert.IsFalse(converter.CanWrite(typeof(Dictionary<,>)));
        }

        [Test]
        public void WriteDictionaryTest()
        {
            var converter = new XmlDictionaryConverter();

            var value = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            var expected = "<xml><keyValuePair><key>1</key><value>one</value></keyValuePair><keyValuePair><key>2</key><value>two</value></keyValuePair></xml>";
            var actual = converter.ToXml(value.GetType(), value);

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadDictionaryTest()
        {
            var converter = new XmlDictionaryConverter();

            var value = "<xml><keyValuePair><key>1</key><value>one</value></keyValuePair><keyValuePair><key>2</key><value>two</value></keyValuePair></xml>";

            var expected = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            var actual = converter.ParseXml(expected.GetType(), value);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteDictionaryWithCustomContractTest()
        {
            var converter = new XmlDictionaryConverter();

            var value = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            var expected = "<xml><item><key>1</key><value>one</value></item><item><key>2</key><value>two</value></item></xml>";
            var actual = converter.ToXml(value.GetType(), value, contract: GetCustomContract());

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadDictionaryWithCustomContractTest()
        {
            var converter = new XmlDictionaryConverter();

            var value = "<xml><item><key>1</key><value>one</value></item><item><key>2</key><value>two</value></item></xml>";

            var expected = new Dictionary<int, string>
            {
                { 1, "one" },
                { 2, "two" }
            };

            var actual = converter.ParseXml(expected.GetType(), value, contract: GetCustomContract());

            Assert.AreEqual(expected, actual);
        }

        private static XmlObjectContract GetCustomContract()
        {
            return new XmlObjectContract(
                typeof(Dictionary<int, string>),
                new XmlName("test"),
                item: new XmlItem(typeof(KeyValuePair<int, string>), new XmlName("item")));
        }
    }
}
