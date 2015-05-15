namespace NetBike.Xml.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Tests.Samples;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;
    using XmlSerializer = NetBike.Xml.XmlSerializer;

    [TestFixture]
    public class XmlSerializerTests
    {
        [Test]
        public void SerializeFooTest()
        {
            var actual = GetSerializer().ToXml(new Foo
            {
                Id = 1,
                Name = "test"
            });

            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <id>1</id>
  <name>test</name>
</foo>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void DeserializeFooTest()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <id>1</id>
  <name>test</name>
</foo>";
            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetSerializer().ParseXml<Foo>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SerializeFooWithoutNamespacesTest()
        {
            var serializer = GetSerializer();
            serializer.Settings.Namespaces.Clear();

            var actual = serializer.ToXml(new Foo
            {
                Id = 1,
                Name = "test"
            });

            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo>
  <id>1</id>
  <name>test</name>
</foo>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void SerializeNullTest()
        {
            var serializer = GetSerializer();
            serializer.Settings.NullValueHandling = XmlNullValueHandling.Include;

            var actual = serializer.ToXml<Foo>(null);

            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void DeserializeNullTest()
        {
            var serializer = new XmlSerializer();
            serializer.Settings.NullValueHandling = XmlNullValueHandling.Include;

            var xml = @"<foo xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" />";
            var actual = serializer.ParseXml<Foo>(xml);

            Assert.IsNull(actual);
        }

        [Test]
        public void SerializeFooAsInterfaceTest()
        {
            var serializer = GetSerializer();
            serializer.Settings.OmitXmlDeclaration = true;

            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var expected = @"
<ifoo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
     xsi:type=""NetBike.Xml.Tests.Samples.Foo"">
  <id>1</id>
  <name>test</name>
</ifoo>";
            var actual = serializer.ToXml<IFoo>(value);

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void DeserializeFooAsInterfaceTest()
        {
            var xml = @"
<foo xsi:type=""NetBike.Xml.Tests.Samples.Foo"" 
     xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <id>1</id>
  <name>foo</name>
</foo>";

            var expected = new Foo
            {
                Id = 1,
                Name = "foo"
            };

            var actual = GetSerializer().ParseXml<IFoo>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DeserializeWithUninknownPropertiesTest()
        {
            var xml = @"<foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <id>1</id>
  <unknown id=""2""><name>test</name></unknown>
  <name>test</name>
  <test />
</foo>";
            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetSerializer().ParseXml<Foo>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SerializeFooInNamespaceTests()
        {
            var serializer = new XmlSerializer();

            var actual = serializer.ToXml(new SampleClass
            {
                Id = 1,
                Name = "test"
            });

            var expected = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" p2:id=""1"" xmlns:p2=""http://example.org/id"" xmlns=""http://example.org"">
    <name>test</name>
</foo>";
            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void DeserializeFooInNamespaceTest()
        {
            var xml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<foo xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" p2:id=""1"" xmlns:p2=""http://example.org/id"" xmlns=""http://example.org"">
    <name>test</name>
</foo>";
            var expected = new SampleClass
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetSerializer().ParseXml<SampleClass>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SerializeEnumerableTest()
        {
            var value = new List<Foo>
            {
                new Foo { Id = 1 },
                new Foo { Id = 2 }
            };

            var actual = GetSerializer().ToXml<IEnumerable<Foo>>(value);
            var expected = string.Format(@"
<ienumerable xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:type=""{0}"">
  <foo>
    <id>1</id>
  </foo>
  <foo>
    <id>2</id>
</foo>
</ienumerable>", value.GetType().FullName);

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void SerializeEnumerableWithoutTypeHandlingTest()
        {
            var value = new List<Foo>
            {
                new Foo { Id = 1 },
                new Foo { Id = 2 }
            };

            var serializer = GetSerializer();
            serializer.Settings.TypeNameHandling = XmlTypeNameHandling.None;

            var actual = serializer.ToXml<IEnumerable<Foo>>(value);
            var expected = "<ienumerable><foo><id>1</id></foo><foo><id>2</id></foo></ienumerable>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void SerializeFooCollectionWithFooBarTypeTest()
        {
            var value = new List<Foo>
            {
                new Foo { Id = 1},
                new FooBar { Id = 2, Description = "test" }
            };

            var actual = GetSerializer().ToXml(value);
            var expected = @"<list xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><foo><id>1</id></foo><foo xsi:type=""NetBike.Xml.Tests.Samples.FooBar""><description>test</description><id>2</id></foo></list>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void SerializeFooCollectionWithFooBarTypeAndWithoutTypeHandlingTest()
        {
            var value = new List<Foo>
            {
                new Foo { Id = 1},
                new FooBar { Id = 2, Description = "test" }
            };

            var serializer = GetSerializer();
            serializer.Settings.TypeNameHandling = XmlTypeNameHandling.None;
            var actual = serializer.ToXml(value);
            var expected = @"<list xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><foo><id>1</id></foo><foo><id>2</id></foo></list>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void SerializeFooContainerTest()
        {
            var value = new FooContainer
            {
                Foo = new FooBar
                {
                    Id = 1,
                    Description = "test"
                }
            };

            var serializer = GetSerializer();
            var actual = serializer.ToXml(value);
            var expected = @"<fooContainer xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><foo xsi:type=""NetBike.Xml.Tests.Samples.FooBar""><description>test</description><id>1</id></foo></fooContainer>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        [Test]
        public void SerializeFooContainerWithoutTypeHandlingTest()
        {
            var value = new FooContainer
            {
                Foo = new FooBar
                {
                    Id = 1,
                    Description = "test"
                }
            };

            var serializer = GetSerializer();
            serializer.Settings.TypeNameHandling = XmlTypeNameHandling.None;
            var actual = serializer.ToXml(value);
            var expected = @"<fooContainer xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><foo><id>1</id></foo></fooContainer>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnoreDeclaration());
        }

        private XmlSerializer GetSerializer()
        {
            var settings = new XmlSerializationSettings
            {
                ContractResolver = new XmlContractResolver(NamingConventions.CamelCase)
            };

            return new XmlSerializer(settings);
        }

        [XmlRoot(ElementName = "foo", Namespace = "http://example.org")]
        public class SampleClass
        {
            [XmlAttribute(AttributeName = "id", Namespace = "http://example.org/id")]
            public int Id { get; set; }

            [XmlElement("name")]
            public string Name { get; set; }

            public override bool Equals(object obj)
            {
                var other = obj as SampleClass;

                if (other == null)
                {
                    return false;
                }

                return other.Id == Id && other.Name == Name;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}