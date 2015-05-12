namespace NetBike.Xml.Tests.Converters.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Collections;

    public class XmlArrayConverterTests : XmlCollectionConverterBaseTests
    {
        protected override IXmlConverter CreateConverter()
        {
            return new XmlArrayConverter();
        }

        protected override Type GetCollectionType<TItem>()
        {
            return typeof(TItem[]);
        }

        protected override object GetCollection<T>(IEnumerable<T> values)
        {
            return values.ToArray();
        }
    }
}