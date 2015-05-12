namespace NetBike.Xml.Tests.Converters.Basics
{
    using System;
    using System.Collections.Generic;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Basics;
    using NUnit.Framework;

    [TestFixture]
    public class XmlTimeSpanConverterTests : XmlBasicConverterBaseTests<TimeSpan>
    {
        protected override IXmlConverter GetConverter()
        {
            return new XmlTimeSpanConverter();
        }

        protected override IEnumerable<BasicSample> GetSamples()
        {
            return new BasicSample[]
            {
                new BasicSample("PT0S", TimeSpan.Zero),
                new BasicSample("PT2M34S", new TimeSpan(0, 2, 34)),
            };
        }
    }
}