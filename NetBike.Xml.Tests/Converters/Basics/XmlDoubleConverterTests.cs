namespace NetBike.Xml.Tests.Converters.Basics
{
    using System.Collections.Generic;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Basics;
    using NUnit.Framework;

    [TestFixture]
    public class XmlDoubleConverterTests : XmlBasicConverterBaseTests<double>
    {
        protected override IXmlConverter GetConverter()
        {
            return new XmlDoubleConverter();
        }

        protected override IEnumerable<BasicSample> GetSamples()
        {
            return new BasicSample[] 
            { 
                new BasicSample("123", 123.0),
                new BasicSample("1.2345", 1.2345)
            };
        }
    }
}