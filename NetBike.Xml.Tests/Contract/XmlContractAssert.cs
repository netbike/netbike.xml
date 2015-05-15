namespace NetBike.Xml.Tests.Contract
{
    using System.Collections.Generic;
    using System.Linq;
    using NetBike.Xml.Contracts;
    using NUnit.Framework;

    internal static class XmlContractAssert
    {
        public static void AreEqual(XmlContract expected, XmlContract actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(expected.GetType(), actual);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ValueType, actual.ValueType);
            AreEqual(expected.Root, actual.Root);

            if (expected is XmlObjectContract)
            {
                var eo = (XmlObjectContract)expected;
                var ao = (XmlObjectContract)actual;

                AreEqual(eo.Item, ao.Item);
                Assert.AreEqual(eo.TypeHandling.HasValue, ao.TypeHandling.HasValue);
                Assert.AreEqual(eo.TypeHandling, ao.TypeHandling);

                Assert.AreEqual(eo.Properties.Count, ao.Properties.Count);

                for (var i = 0; i < eo.Properties.Count; i++)
                {
                    AreEqual(eo.Properties[i], ao.Properties[i]);
                }
            }
            else if (expected is XmlEnumContract)
            {
                var ee = (XmlEnumContract)expected;
                var ae = (XmlEnumContract)actual;

                var eitems = new List<XmlEnumItem>(ee.Items);
                var aitems = new List<XmlEnumItem>(ae.Items);

                for (var i = 0; i < eitems.Count; i++)
                {
                    Assert.AreEqual(eitems[i].Name, aitems[i].Name);
                    Assert.AreEqual(eitems[i].Value, aitems[i].Value);
                }
            }
        }

        public static void AreEqual(XmlMember expected, XmlMember actual)
        {
            if (expected == null)
            {
                Assert.IsNull(actual);
                return;
            }

            Assert.IsNotNull(actual);
            Assert.IsInstanceOf(expected.GetType(), actual);
            Assert.AreEqual(expected.ValueType, actual.ValueType);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.MappingType, actual.MappingType);
            Assert.AreEqual(expected.DefaultValueHandling.HasValue, actual.DefaultValueHandling.HasValue);
            Assert.AreEqual(expected.DefaultValueHandling, actual.DefaultValueHandling);
            Assert.AreEqual(expected.DefaultValue, actual.DefaultValue);
            Assert.AreEqual(expected.TypeHandling.HasValue, actual.TypeHandling.HasValue);
            Assert.AreEqual(expected.TypeHandling, actual.TypeHandling);
            Assert.AreEqual(expected.NullValueHandling.HasValue, actual.NullValueHandling.HasValue);
            Assert.AreEqual(expected.NullValueHandling, actual.NullValueHandling);

            Assert.AreEqual(expected.KnownTypes.Count, actual.KnownTypes.Count);

            foreach (var expectedKnownType in expected.KnownTypes)
            {
                var actualKnownType = actual.KnownTypes.FirstOrDefault(x => x.ValueType == expectedKnownType.ValueType);
                AreEqual(expectedKnownType, actualKnownType);
            }

            AreEqual(expected.Item, actual.Item);

            if (expected is XmlProperty)
            {
                var ep = (XmlProperty)expected;
                var ap = (XmlProperty)actual;

                Assert.AreEqual(ep.PropertyInfo, ap.PropertyInfo);
                Assert.AreEqual(ep.PropertyName, ap.PropertyName);
                Assert.AreEqual(ep.IsRequired, ap.IsRequired);
                Assert.AreEqual(ep.Order, ap.Order);
                Assert.AreEqual(ep.IsCollection, ap.IsCollection);
            }
        }
    }
}