namespace NetBike.Xml.Tests.Converters.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Tests.Samples;
    using NetBike.XmlUnit;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;

    public abstract class XmlCollectionConverterBaseTests
    {
        [Test]
        public void CanReadTypeTest()
        {
            var converter = CreateConverter();
            Assert.IsTrue(converter.CanRead(GetCollectionType<int>()));
            Assert.IsTrue(converter.CanRead(GetCollectionType<IFoo>()));
            Assert.IsFalse(converter.CanRead(typeof(int)));
            Assert.IsFalse(converter.CanRead(typeof(IFoo)));
        }

        [Test]
        public void CanWriteTypeTest()
        {
            var converter = CreateConverter();
            Assert.IsTrue(converter.CanWrite(GetCollectionType<int>()));
            Assert.IsTrue(converter.CanWrite(GetCollectionType<IFoo>()));
            Assert.IsFalse(converter.CanRead(typeof(int)));
            Assert.IsFalse(converter.CanRead(typeof(IFoo)));
        }
        
        [Test]
        public void WriteInt32CollectionTest()
        {
            var value = GetCollection(new List<int> { 1, 2, 3 });
            var actual = CreateConverter().ToXml(value.GetType(), value);
            var expected = "<xml><int>1</int><int>2</int><int>3</int></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadInt32CollectionTest()
        {
            var xml = "<xml><int>1</int><int>2</int><int>3</int></xml>";

            var type = GetCollectionType<int>();
            var actual = CreateConverter().ParseXml(type, xml);
            var expected = new List<int> { 1, 2, 3 };

            Assert.IsInstanceOf(type, actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WriteFooCollectionTest()
        {
            var value = GetCollection(new List<Foo>
            {
                new Foo { Id = 1, Name = "foo-1" },
                new Foo { Id = 2, Name = "foo-2" }
            });

            var actual = CreateConverter().ToXml(value.GetType(), value);
            var expected = "<xml><foo><id>1</id><name>foo-1</name></foo><foo><id>2</id><name>foo-2</name></foo></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadEmptyCollectionTest()
        {
            var xml = "<xml></xml>";
            var type = GetCollectionType<Foo>();

            var actual = CreateConverter().ParseXml(type, xml);
            
            Assert.IsInstanceOf(type, actual);
            Assert.IsEmpty((IEnumerable<Foo>)actual);
        }

        [Test]
        public void ReadEmptyElementTest()
        {
            var xml = "<xml />";
            var type = GetCollectionType<Foo>();

            var actual = CreateConverter().ParseXml(type, xml);
            
            Assert.IsInstanceOf(type, actual);
            Assert.IsEmpty((IEnumerable<Foo>)actual);
        }

        [Test]
        public void ReadFooCollectionTest()
        {
            var xml = "<xml><foo><id>1</id><name>foo-1</name></foo><foo><id>2</id><name>foo-2</name></foo></xml>";
            var type = GetCollectionType<Foo>();

            var actual = CreateConverter().ParseXml(type, xml);

            var expected = new List<Foo>
            {
                new Foo { Id = 1, Name = "foo-1" },
                new Foo { Id = 2, Name = "foo-2" }
            };

            Assert.IsInstanceOf(type, actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ReadCollectionWithUnknownXmlElementsTest()
        {
            var xml = "<xml><someItem>123</someItem><foo><id>1</id><name>foo-1</name></foo><unknown /></xml>";
            var type = GetCollectionType<Foo>();

            var actual = CreateConverter().ParseXml(type, xml);

            var expected = new List<Foo>
            {
                new Foo { Id = 1, Name = "foo-1" },
            };

            Assert.IsInstanceOf(type, actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WriteCollectionWithKnownItemTypeTest()
        {
            var value = GetCollection(new List<Foo>
            {
                new Foo { Id = 1, Name = "foo" },
                new FooBar { Id = 2, Name = "foo-bar" }
            });

            var collectionType = value.GetType();
            var itemType = typeof(Foo);

            var contract = new XmlObjectContractBuilder(collectionType)
                .SetItem(itemType, x => x.SetName("foo").SetKnownType<FooBar>("fooBar"))
                .Build();

            var actual = CreateConverter().ToXml(collectionType, value, contract: contract);
            var expected = @"<xml><foo><id>1</id><name>foo</name></foo><fooBar><id>2</id><name>foo-bar</name></fooBar></xml>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadCollectionWithKnownItemTypeTest()
        {
            var collectionType = GetCollectionType<Foo>();
            var xml = @"<xml><foo><id>1</id><name>foo</name></foo><fooBar><id>2</id><name>foo-bar</name></fooBar></xml>";

            var contract = new XmlObjectContractBuilder(collectionType)
                .SetItem(typeof(Foo), x => x.SetName("foo").SetKnownType<FooBar>("fooBar"))
                .Build();

            var actual = CreateConverter().ParseXml(collectionType, xml, contract: contract);

            var expected = new List<Foo>
            {
                new Foo { Id = 1, Name = "foo" },
                new FooBar { Id = 2, Name = "foo-bar" }
            };

            Assert.IsInstanceOf(collectionType, actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void WriteCollectionWithDifferentItemTypeTest()
        {
            var value = GetCollection(new List<Foo>
            {
                new Foo { Id = 1, Name = "foo" },
                new FooBar { Id = 2, Name = "foo-bar" }
            });

            var actual = CreateConverter().ToXml(value.GetType(), value);
            var expected = $@"<xml>
  <foo>
    <id>1</id>
    <name>foo</name>
  </foo>
  <foo xsi:type=""{typeof(FooBar)}"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <id>2</id>
    <name>foo-bar</name>
  </foo>
</xml>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnore(XmlComparisonType.NamespacePrefix));
        }

        [Test]
        public void ReadCollectionWithDifferentItemTypeTest()
        {
            var xml = $@"<xml xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <foo>
    <id>1</id>
    <name>foo</name>
  </foo>
  <foo xsi:type=""{typeof(FooBar)}"">
    <description>Africa unite!</description>
    <id>2</id>
    <name>foo-bar</name>
  </foo>
</xml>";

            var type = GetCollectionType<Foo>();

            var actual = CreateConverter().ParseXml(type, xml);

            var expected = new List<Foo>
            {
                new Foo { Id = 1, Name = "foo" },
                new FooBar { Id = 2, Name = "foo-bar", Description = "Africa unite!" }
            };

            Assert.IsInstanceOf(type, actual);
            Assert.That(actual, Is.EqualTo(expected));
        }

        protected abstract IXmlConverter CreateConverter();

        protected abstract Type GetCollectionType<TItem>();

        protected abstract object GetCollection<T>(IEnumerable<T> values);
    }
}
