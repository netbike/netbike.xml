namespace NetBike.Xml.Tests.Converters
{
    using System;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NetBike.Xml.Converters;
    using NUnit.Framework;

    [TestFixture]
    public class XmlEnumConverterTests
    {
        private enum SimpleEnum
        {
            One,
            Two,
            Three
        }

        [Flags]
        private enum FlagEnum
        {
            Bit1 = 1,
            Bit2 = 2,
            Bit3 = 4
        }

        [Test]
        public void WriteEnumTest()
        {
            var value = SimpleEnum.One;
            var converter = new XmlEnumConverter();
            var actual = converter.ToXml(value);
            var expected = "<xml>one</xml>";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteEnumWithCustomContractTest()
        {
            var value = SimpleEnum.One;
            var converter = new XmlEnumConverter();
            var actual = converter.ToXml(value, contract: GetCustomContract());
            var expected = "<xml>1</xml>";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteFlagEnumTest()
        {
            var value = FlagEnum.Bit1 | FlagEnum.Bit2;
            var converter = new XmlEnumConverter();
            var actual = converter.ToXml(value);
            var expected = "<xml>bit1 bit2</xml>";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteFlagEnumWithCustomSeparatorTest()
        {
            var value = FlagEnum.Bit1 | FlagEnum.Bit2;
            var converter = new XmlEnumConverter(',');
            var actual = converter.ToXml(value);
            var expected = "<xml>bit1,bit2</xml>";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteUnknownValueTest()
        {
            var value = (SimpleEnum)4;
            var converter = new XmlEnumConverter();
            Assert.Throws<FormatException>(() => converter.ToXml(value));
        }

        [Test]
        public void WriteUnknownFlagValueTest()
        {
            var value = (FlagEnum)10;
            var converter = new XmlEnumConverter();
            Assert.Throws<FormatException>(() => converter.ToXml(value));
        }

        [Test]
        public void ReadEnumTest()
        {
            var converter = new XmlEnumConverter();
            var xml = "<xml>two</xml>";
            var actual = converter.ParseXml<SimpleEnum>(xml);
            var expected = SimpleEnum.Two;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadEnumWithCustomContractTest()
        {
            var converter = new XmlEnumConverter();
            var xml = "<xml>big_number</xml>";
            var actual = converter.ParseXml<SimpleEnum>(xml, contract: GetCustomContract());
            var expected = SimpleEnum.Three;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFlagEnumTest()
        {
            var converter = new XmlEnumConverter();
            var xml = "<xml>bit2 bit3</xml>";
            var actual = converter.ParseXml<FlagEnum>(xml);
            var expected = FlagEnum.Bit2 | FlagEnum.Bit3;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadEnumWithWhitespacesTest()
        {
            var converter = new XmlEnumConverter();
            var xml = "<xml> three </xml>";
            var actual = converter.ParseXml<SimpleEnum>(xml);
            var expected = SimpleEnum.Three;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFlagEnumWithCustomSeparatorTest()
        {
            var converter = new XmlEnumConverter(',');
            var xml = "<xml>bit1, bit3,bit2</xml>";
            var actual = converter.ParseXml<FlagEnum>(xml);
            var expected = FlagEnum.Bit1 | FlagEnum.Bit3 | FlagEnum.Bit2;
            Assert.AreEqual(expected, actual);
        }

        private static XmlEnumContract GetCustomContract()
        {
            return new XmlEnumContractBuilder<SimpleEnum>()
                .SetName("test")
                .SetItem(SimpleEnum.One, "1")
                .SetItem(SimpleEnum.Two, "2")
                .SetItem(SimpleEnum.Three, "big_number")
                .Build();
        }
    }
}