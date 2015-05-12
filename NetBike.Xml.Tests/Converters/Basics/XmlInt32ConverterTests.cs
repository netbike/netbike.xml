namespace NetBike.Xml.Tests.Converters.Basics
{
    using System.Collections.Generic;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Basics;
    using NUnit.Framework;

    [TestFixture]
    public class XmlInt32ConverterTests : XmlBasicConverterBaseTests<int>
    {
        protected override IXmlConverter GetConverter()
        {
            return new XmlInt32Converter();
        }

        protected override IEnumerable<BasicSample> GetSamples()
        {
            yield return new BasicSample("123", 123);
        }
    }
}