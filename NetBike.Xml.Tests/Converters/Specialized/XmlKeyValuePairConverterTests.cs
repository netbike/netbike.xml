namespace NetBike.Xml.Tests.Converters.Specialized
{
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Specialized;
    using NUnit.Framework;

    [TestFixture]
    public class XmlKeyValuePairConverterTests : XmlKeyValuePairConverterTestsBase
    {
        protected override IXmlConverter GetConverter()
        {
            return new XmlKeyValuePairConverter();
        }
    }
}