namespace NetBike.Xml.Tests.Converters.Collections
{
    using System;
    using System.Collections.Generic;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Collections;
    using NUnit.Framework;

    [TestFixture]
    public class XmlListConverterTests : XmlCollectionConverterBaseTests
    {
        protected override IXmlConverter CreateConverter()
        {
            return new XmlListConverter();
        }

        protected override Type GetCollectionType<T>()
        {
            return typeof(List<T>);
        }

        protected override object GetCollection<T>(IEnumerable<T> values)
        {
            return new List<T>(values);
        }
    }
}