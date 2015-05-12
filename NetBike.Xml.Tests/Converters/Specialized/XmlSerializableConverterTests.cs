namespace NetBike.Xml.Tests.Converters.Specialized
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using NetBike.Xml.Converters.Specialized;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;

    [TestFixture]
    public class XmlSerializableConverterTests
    {
        [Test]
        public void CanReadTest()
        {
            var converter = new XmlSerializableConverter();
            Assert.IsTrue(converter.CanRead(typeof(TestClass)));
            Assert.IsFalse(converter.CanRead(typeof(XmlSerializableConverter)));
        }

        [Test]
        public void CanWriteTest()
        {
            var converter = new XmlSerializableConverter();
            Assert.IsTrue(converter.CanWrite(typeof(TestClass)));
            Assert.IsFalse(converter.CanWrite(typeof(XmlSerializableConverter)));
        }

        [Test]
        public void WriteXmlTest()
        {
            var converter = new XmlSerializableConverter();
            var value = new TestClass
            {
                Id = 1,
                Name = "hello"
            };

            var actual = converter.ToXml(value);
            var expected = "<xml id=\"1\"><name>hello</name></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadXmlTest()
        {
            var converter = new XmlSerializableConverter();
            var xml = "<xml id=\"1\"><name>hello</name></xml>";

            var expected = new TestClass
            {
                Id = 1,
                Name = "hello"
            };

            var actual = converter.ParseXml<TestClass>(xml);

            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        public class TestClass : IXmlSerializable
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                Id = int.Parse(reader.GetAttribute("id"));

                reader.ReadStartElement();
                Name = reader.ReadElementString("name");
                reader.ReadEndElement();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteAttributeString("id", Id.ToString());
                writer.WriteElementString("name", Name);
            }
        }
    }
}
