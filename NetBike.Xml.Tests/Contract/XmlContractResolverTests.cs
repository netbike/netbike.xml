namespace NetBike.Xml.Tests.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NUnit.Framework;

    [TestFixture]
    public class XmlContractResolverTests
    {
        [XmlRoot("Enum")]
        private enum TestEnum
        {
            [XmlEnum("1")]
            True,
            [XmlEnum("0")]
            False,
            [XmlEnum("-")]
            SomethingElse,
            [XmlEnum(".")]
            Dot,
        }

        [Test]
        public void ResolveObjectContractWithoutSystemAttributesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase, ignoreSystemAttributes: true);
            var valueType = typeof(TestClass);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<TestClass>()
                .SetName("testClass")
                .SetProperty(x => x.Id, "id")
                .SetProperty(x => x.Value, "value")
                .SetProperty(x => x.Comment, "comment")
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveExtendedObjectContractWithoutSystemAttributesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase, ignoreSystemAttributes: true);
            var valueType = typeof(ExtendedTestClass);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<ExtendedTestClass>()
                .SetName("extendedTestClass")
                .SetProperty(x => x.DateTime, "dateTime")
                .SetProperty(x => x.Id, "id")
                .SetProperty(x => x.Value, "value")
                .SetProperty(x => x.Comment, "comment")
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveCollectionContractWithoutSystemAttributesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase, ignoreSystemAttributes: true);
            var valueType = typeof(TestClassWithCollections);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<TestClassWithCollections>()
                .SetName("testClassWithCollections")
                .SetProperty(x => x.Array, p => p.SetName("array"))
                .SetProperty(x => x.GenericList, p => p.SetName("genericList"))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveGenericCollectionContractTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase, ignoreSystemAttributes: true);
            var valueType = typeof(List<TestClass>);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<List<TestClass>>()
                .SetName("list")
                .SetProperty(x => x.Capacity, "capacity")
                .SetProperty(x => x.Count, "count")
                .SetItem("testClass")
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveEnumContractWithoutSystemAttributesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase, ignoreSystemAttributes: true);
            var valueType = typeof(TestEnum);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlEnumContractBuilder<TestEnum>()
                .SetName("testEnum")
                .SetItem(TestEnum.True, "true")
                .SetItem(TestEnum.False, "false")
                .SetItem(TestEnum.SomethingElse, "somethingElse")
                .SetItem(TestEnum.Dot, "dot")
                .BuildContract();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase);
            var valueType = typeof(TestClass);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<TestClass>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p
                    .SetName(new XmlName("ID", "http://example.org"))
                    .SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Value, p => p
                    .SetName("VALUE")
                    .SetNullValueHandling(XmlNullValueHandling.Include).SetOrder(10))
                .SetProperty(x => x.Comment, "comment")
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithInnerTextTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.Ignore);

            var valueType = typeof(TestClassWithInnerText);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlObjectContractBuilder<TestClassWithInnerText>()
                .SetName("TestClassWithInnerText")
                .SetProperty(x => x.Comment, p => p
                    .SetMappingType(XmlMappingType.InnerText))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithElementKnownTypesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase);

            var actual = resolver.ResolveContract<TestClassWithElementKnownTypes>();

            var expected = new XmlObjectContractBuilder<TestClassWithElementKnownTypes>()
                .SetName("testClassWithElementKnownTypes")
                .SetProperty(x => x.Value, p => p
                    .SetName("object")
                    .SetKnownType<int>("number")
                    .SetKnownType<string>("string"))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithCollectionItemKnownTypesTest()
        {
            var actual = new XmlContractResolver(NamingConventions.CamelCase)
                .ResolveContract<TestClassWithCollectionItemKnownTypes>();

            var expected = new XmlObjectContractBuilder<TestClassWithCollectionItemKnownTypes>()
                .SetName("testClassWithCollectionItemKnownTypes")
                .SetProperty(x => x.Items, p => p
                    .SetName("items")
                    .SetItem(i => i
                        .SetName("object")
                        .SetKnownType<int>("number")
                        .SetKnownType<string>("string")))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithCollectionsTest()
        {
            var actual = new XmlContractResolver(NamingConventions.CamelCase)
                .ResolveContract<TestClassWithCollections>();

            var expected = new XmlObjectContractBuilder<TestClassWithCollections>()
                .SetName("testClassWithCollections")
                .SetProperty(x => x.Array, p => p.SetName("array").SetOrder(1))
                .SetProperty(x => x.GenericList, p => p
                    .SetName("Array2")
                    .SetOrder(2)
                    .SetItem(new XmlName("Item", "http://example.org")))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithFlattenCollectionTest()
        {
            var actual = new XmlContractResolver(NamingConventions.CamelCase)
                .ResolveContract<TestClassWithFlattenCollection>();

            var expected = new XmlObjectContractBuilder<TestClassWithFlattenCollection>()
                .SetName("testClassWithFlattenCollection")
                .SetProperty(x => x.Items, p => p
                    .SetName("number")
                    .SetCollection()
                    .SetItem("number"))
                .Build();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveObjectContractWithFlattenCollectionKnownTypesTest()
        {
            var resolver = new XmlContractResolver(NamingConventions.CamelCase);

            var actual = resolver.ResolveContract<TestClassWithFlattenCollectionKnownTypes>();

            var expected = new XmlObjectContractBuilder<TestClassWithFlattenCollectionKnownTypes>()
                .SetName("testClassWithFlattenCollectionKnownTypes")
                .SetProperty(x => x.Items, p => p
                    .SetName("items")
                    .SetCollection()
                    .SetItem(i => i
                        .SetName("object")
                        .SetKnownType<int>("number")
                        .SetKnownType<string>("string")))
                .BuildContract();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ResolveEnumContractTest()
        {
            var resolver = new XmlContractResolver();
            var valueType = typeof(TestEnum);

            var actual = resolver.ResolveContract(valueType);

            var expected = new XmlEnumContractBuilder<TestEnum>()
                .SetName("Enum")
                .SetItem(TestEnum.True, "1")
                .SetItem(TestEnum.False, "0")
                .SetItem(TestEnum.SomethingElse, "-")
                .SetItem(TestEnum.Dot, ".")
                .BuildContract();

            XmlContractAssert.AreEqual(expected, actual);
        }

        [XmlRoot("test")]
        public class TestClass
        {
            [XmlAttribute(AttributeName = "ID", Namespace = "http://example.org")]
            public string Id { get; set; }

            [XmlElement(ElementName = "VALUE", IsNullable = true, Order = 10)]
            public string Value { get; set; }

            public string Comment { get; set; }
        }

        public class ExtendedTestClass : TestClass
        {
            public DateTime DateTime { get; set; }
        }

        public class TestClassWithInnerText
        {
            [XmlText]
            public string Comment { get; set; }
        }

        public class TestClassWithCollections
        {
            [XmlArray(Order = 1)]
            public string[] Array { get; set; }

            [XmlArray(ElementName = "Array2", Order = 2)]
            [XmlArrayItem(ElementName = "Item", Namespace = "http://example.org")]
            public List<int> GenericList { get; set; }
        }

        public class TestClassWithElementKnownTypes
        {
            [XmlElement("object", typeof(object))]
            [XmlElement("string", typeof(string))]
            [XmlElement("number", typeof(int))]
            public object Value { get; set; }
        }

        public class TestClassWithCollectionItemKnownTypes
        {
            [XmlArrayItem("object", typeof(object))]
            [XmlArrayItem("string", typeof(string))]
            [XmlArrayItem("number", typeof(int))]
            public List<object> Items { get; set; }
        }

        public class TestClassWithFlattenCollection
        {
            [XmlElement("number")]
            public List<int> Items { get; set; }
        }

        public class TestClassWithFlattenCollectionKnownTypes
        {
            [XmlElement("object", typeof(object))]
            [XmlElement("string", typeof(string))]
            [XmlElement("number", typeof(int))]
            public List<object> Items { get; set; }
        }
    }
}