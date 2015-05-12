namespace NetBike.Xml.Tests.Converters.Specialized
{
    using System;
    using NetBike.Xml.Converters.Specialized;
    using NUnit.Framework;

    [TestFixture]
    public class XmlByteArrayConverterTests
    {
        [Test]
        public void WriteByteArrayTest()
        {
            var value = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var actual = new XmlByteArrayConverter().ToXml(value);
            var expected = "<xml>AQIECBAgQIA=</xml>";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteLargeByteArrayTest()
        {
            var value = GetLargeByteArray();
            var actual = new XmlByteArrayConverter().ToXml(value);
            var expected = GetLargeBase64XmlString();
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadByteArrayTest()
        {
            var value = "<xml>AQIECBAgQIA=</xml>";
            var expected = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var actual = new XmlByteArrayConverter().ParseXml<byte[]>(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadByteArrayInCDataTest()
        {
            var value = "<xml><![CDATA[AQIECBAgQIA=]]></xml>";
            var expected = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var actual = new XmlByteArrayConverter().ParseXml<byte[]>(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadByteArrayWithWhitespaceTest()
        {
            var value = "<xml>\r\nAQIECBAgQIA=   \r\n </xml>";
            var expected = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };
            var actual = new XmlByteArrayConverter().ParseXml<byte[]>(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadEmptyByteArrayTest()
        {
            var value = "<xml />";
            var expected = new byte[0];
            var actual = new XmlByteArrayConverter().ParseXml<byte[]>(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadLargeByteArrayTest()
        {
            var value = GetLargeBase64XmlString();
            var expected = GetLargeByteArray();
            var actual = new XmlByteArrayConverter().ParseXml<byte[]>(value);
            Assert.AreEqual(expected, actual);
        }

        private byte[] GetLargeByteArray()
        {
            var value = new byte[XmlByteArrayConverter.ChunkSize * 2 + 123];

            for (var i = 0; i < value.Length; i++)
            {
                value[i] = (byte)(i % 256);
            }

            return value;
        }

        private string GetLargeBase64XmlString()
        {
            return "<xml>" + Convert.ToBase64String(GetLargeByteArray()) + "</xml>";
        }
    }
}