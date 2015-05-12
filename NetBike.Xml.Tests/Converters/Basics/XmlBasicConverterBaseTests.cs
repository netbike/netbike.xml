namespace NetBike.Xml.Tests.Converters.Basics
{
    using System.Collections.Generic;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;

    public abstract class XmlBasicConverterBaseTests<TValue>
    {
        public IEnumerable<BasicSample> Samples
        {
            get { return GetSamples(); }
        }

        [Test]
        public void CanReadTest()
        {
            Assert.IsTrue(GetConverter().CanRead(typeof(TValue)));
        }

        [Test]
        public void CanWriteTest()
        {
            Assert.IsTrue(GetConverter().CanWrite(typeof(TValue)));
        }

        [Test, TestCaseSource("Samples")]
        public void WriteElementTest(BasicSample sample)
        {
            var expected = string.Format("<xml>{0}</xml>", sample.StringValue);
            var actual = GetConverter().ToXml(sample.Value);
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test, TestCaseSource("Samples")]
        public void WriteAttributeTest(BasicSample sample)
        {
            var expected = string.Format("<xml value=\"{0}\" />", sample.StringValue);
            var actual = GetConverter().ToXml(sample.Value, member: GetAttributeMember());
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test, TestCaseSource("Samples")]
        public void ReadElementTest(BasicSample sample)
        {
            var xml = string.Format("<xml>{0}</xml>", sample.StringValue);
            var actual = GetConverter().ParseXml<TValue>(xml);
            Assert.AreEqual(sample.Value, actual);
        }

        [Test, TestCaseSource("Samples")]
        public void ReadAttributeTest(BasicSample sample)
        {
            var xml = string.Format("<xml value=\"{0}\" />", sample.StringValue);
            var actual = GetConverter().ParseXml<TValue>(xml, member: GetAttributeMember());
            Assert.AreEqual(sample.Value, actual);
        }

        protected abstract IXmlConverter GetConverter();

        protected abstract IEnumerable<BasicSample> GetSamples();

        private static XmlMember GetAttributeMember()
        {
            return new XmlMember(typeof(TValue), new XmlName("value"), XmlMappingType.Attribute);
        }
    }
}