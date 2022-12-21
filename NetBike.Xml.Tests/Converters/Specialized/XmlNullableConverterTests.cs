namespace NetBike.Xml.Tests.Converters.Specialized
{
    using System;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters.Specialized;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;
    using XmlSerializer = NetBike.Xml.XmlSerializer;

    [TestFixture]
    public class XmlNullableConverterTests
    {
        [Test]
        public void CanReadTest()
        {
            var converter = new XmlNullableConverter();
            Assert.IsTrue(converter.CanRead(typeof(Nullable<int>)));
            Assert.IsTrue(converter.CanRead(typeof(DateTime?)));
            Assert.IsFalse(converter.CanRead(typeof(DateTime)));
            Assert.IsFalse(converter.CanRead(typeof(Nullable<>)));
        }

        [Test]
        public void CanWriteTest()
        {
            var converter = new XmlNullableConverter();
            Assert.IsTrue(converter.CanWrite(typeof(Nullable<int>)));
            Assert.IsTrue(converter.CanWrite(typeof(DateTime?)));
            Assert.IsFalse(converter.CanWrite(typeof(DateTime)));
            Assert.IsFalse(converter.CanWrite(typeof(Nullable<>)));
        }

        [Test]
        public void WriteNullableTest()
        {
            var converter = new XmlNullableConverter();
            var actual = converter.ToXml<int?>(1);
            var expected = "<xml>1</xml>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteNullableAttributeTest()
        {
            var converter = new XmlNullableConverter();
            var actual = converter.ToXml<int?>(1, member: GetAttributeMember<int?>());
            var expected = "<xml value=\"1\" />";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteNullTest()
        {
            var converter = new XmlNullableConverter();
            var actual = converter.ToXml<int?>(null);
            var expected = "<xml />";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteNullAttributeTest()
        {
            var serializer = new XmlSerializer();
            var actual = serializer.ToXml(new TestClass2());
            var expected = "<TestClass2 />";
            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void WriteNullWithNullIncludeHandlingTest()
        {
            var serializer = new XmlSerializer
            {
                Settings =
                {
                    OmitXmlDeclaration = true,
                    NullValueHandling = XmlNullValueHandling.Include
                }
            };
            var actual = serializer.ToXml(new TestClass());
            var expected = @"<TestClass xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><Value xsi:nil=""true"" /></TestClass>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadNullableTest()
        {
            var serializer = new XmlSerializer();
            var xml = "<TestClass><Value>1</Value></TestClass>";
            var actual = serializer.ParseXml<TestClass>(xml);
            int? expected = 1;
            Assert.AreEqual(expected, actual.Value);
        }

        [Test]
        public void ReadNullTest()
        {
            var serializer = new XmlSerializer();
            var xml = "<testClass></testClass>";
            var actual = serializer.ParseXml<TestClass>(xml);
            int? expected = null;
            Assert.AreEqual(expected, actual.Value);
        }

        [Test]
        public void ReadNullWithNullIncludeHandlingTest()
        {
            var serializer = new XmlSerializer();
            var xml = @"<testClass><value xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" /></testClass>";
            var actual = serializer.ParseXml<TestClass>(xml);
            int? expected = null;
            Assert.AreEqual(expected, actual.Value);
        }

        private static XmlMember GetAttributeMember<T>()
        {
            return new XmlMember(typeof(T), "value", XmlMappingType.Attribute);
        }

        public class TestClass
        {
            public int? Value { get; set; }
        }

        public class TestClass2
        {
            [XmlAttribute]
            public int? Value { get; set; }
        }
    }
}