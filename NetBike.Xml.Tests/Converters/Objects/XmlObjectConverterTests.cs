namespace NetBike.Xml.Tests.Converters.Objects
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Objects;
    using NetBike.Xml.Tests.Samples;
    using NetBike.XmlUnit;
    using NetBike.XmlUnit.NUnitAdapter;
    using NUnit.Framework;

    [TestFixture]
    public class XmlObjectConverterTests
    {
        [Test]
        public void WriteFooTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetConverter().ToXml(value);
            var expected = "<xml><id>1</id><name>test</name></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteFooWithDefaultsTest()
        {
            var contract = GetContractWithDefaults();

            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = "<xml><name>test</name></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteFooWithIgnoreNullHandlingTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = null
            };

            var contract = GetContractWithIgnoreNull();
            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = "<xml><id>1</id></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteFooWithIncludeNullHandlingTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = null
            };

            var contract = GetContractWithIncludeNull();
            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = @"<xml><id>1</id><name xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" /></xml>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnore(XmlComparisonType.NamespacePrefix));
        }

        [Test]
        public void WriteFooWithGlobalIncludeNullHandlingTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = null
            };

            var settings = new XmlSerializerSettings
            {
                OmitXmlDeclaration = true,
                NullValueHandling = XmlNullValueHandling.Include,
                ContractResolver = new XmlContractResolver(NamingConventions.CamelCase)
            };

            var actual = GetConverter().ToXml(value, settings: settings);
            var expected = @"<xml><id>1</id><name xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" /><enumValue xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" /></xml>";

            Assert.That(actual, IsXml.Equals(expected).WithIgnore(XmlComparisonType.NamespacePrefix));
        }

        [Test]
        public void WriteFooAttributeTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var contract = GetContractWithOneAttribute();
            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = "<xml id=\"1\"><name>test</name></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteFooAttributesTest()
        {
            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var contract = GetContractWithAttributes();
            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = "<xml id=\"1\" name=\"test\" />";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteFooInnerTextTest()
        {
            var contract = GetContractWithInnerText();

            var value = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetConverter().ToXml(value, contract: contract);
            var expected = @"<xml id=""1"">test</xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void WriteXmlWithCollectionPropertyTest()
        {
            var value = new ClassWithCollectionProperty
            {
                Name = "two",
                Numbers = new List<int> { 1, 2 }
            };

            var actual = GetConverter().ToXml(value);
            var expected = @"<xml><name>two</name><number>1</number><number>2</number></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadXmlWithCollectionPropertyTest()
        {
            var xml = @"<xml><name>two</name><number>1</number><number>2</number></xml>";

            var expected = new ClassWithCollectionProperty
            {
                Name = "two",
                Numbers = new List<int> { 1, 2 }
            };

            var actual = GetConverter().ParseXml<ClassWithCollectionProperty>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void WriteXmlWithKnownTypePropertyTest()
        {
            var value = new ClassWithKnownTypeProperty
            {
                Name = "test",
                Foo = new FooBar
                {
                    Id = 1,
                    Name = "foobar"
                }
            };

            var actual = GetConverter().ToXml(value);
            var expected = @"<xml><name>test</name><fooBar><id>1</id><name>foobar</name></fooBar></xml>";

            Assert.That(actual, IsXml.Equals(expected));
        }

        [Test]
        public void ReadXmlWithKnownTypePropertyTest()
        {
            var xml = @"<xml><name>test</name><fooBar><id>1</id><name>foobar</name></fooBar></xml>";

            var expected = new ClassWithKnownTypeProperty
            {
                Name = "test",
                Foo = new FooBar
                {
                    Id = 1,
                    Name = "foobar"
                }
            };

            var actual = GetConverter().ParseXml<ClassWithKnownTypeProperty>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooTest()
        {
            var value = "<xml><id>1</id><name>test</name></xml>";

            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            var actual = GetConverter().ParseXml<Foo>(value);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithEmptyElementTest()
        {
            var value = "<xml><id>1</id><name /></xml>";

            var expected = new Foo
            {
                Id = 1,
                Name = string.Empty
            };

            var actual = GetConverter().ParseXml<Foo>(value);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithMissedPropertyTest()
        {
            var expected = new Foo
            {
                Id = 1,
                Name = null
            };

            var xml = "<xml><id>1</id></xml>";
            var actual = GetConverter().ParseXml<Foo>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithMissedPropertyThatRequiredTest()
        {
            var contract = new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p.SetName("id").SetRequired())
                .SetProperty(x => x.Name, "name")
                .Build();

            var converter = GetConverter();
            var value = "<xml><name>test</name></xml>";

            Assert.Throws<XmlSerializationException>(() => converter.ParseXml<Foo>(value, contract: contract));
        }

        [Test]
        public void ReadFooWithDeclaredMissedPropertyTest()
        {
            var expected = new Foo
            {
                Id = 1,
                Name = null
            };

            var value = @"<xml><id>1</id><name xsi:nil=""true"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" /></xml>";

            var actual = GetConverter().ParseXml<Foo>(value);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooFromEmptyElementTest()
        {
            var expected = new Foo();

            var xml = "<xml />";
            var actual = GetConverter().ParseXml<Foo>(xml);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithOneAttributeTest()
        {
            var contract = GetContractWithOneAttribute();
            var value = "<xml id=\"1\"><name>test</name></xml>";
            var actual = GetConverter().ParseXml<Foo>(value, contract: contract);

            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithAttributesTest()
        {
            var contract = GetContractWithAttributes();
            var value = "<xml id=\"1\" name=\"test\" />";
            var actual = GetConverter().ParseXml<Foo>(value, contract: contract);

            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithInnerTextTest()
        {
            var contract = GetContractWithInnerText();

            var value = @"<xml id=""1"">test</xml>";
            var actual = GetConverter().ParseXml<Foo>(value, contract: contract);

            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReadFooWithDefaultPropertyTest()
        {
            var contract = GetContractWithDefaults();
            var value = "<xml><name>test</name></xml>";
            var actual = GetConverter().ParseXml<Foo>(value, contract: contract);

            var expected = new Foo
            {
                Id = 1,
                Name = "test"
            };

            Assert.AreEqual(expected, actual);
        }

        private static XmlContract GetContractWithDefaults()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p.SetName("id").SetDefaultValueHandling(XmlDefaultValueHandling.Ignore).SetDefaultValue(1))
                .SetProperty(x => x.Name, "name")
                .Build();
        }

        private static XmlContract GetContractWithIgnoreNull()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, "id")
                .SetProperty(x => x.Name, p => p.SetName("name").SetNullValueHandling(XmlNullValueHandling.Ignore))
                .Build();
        }

        private static XmlContract GetContractWithIncludeNull()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty("Id", new XmlName("id"))
                .SetProperty(x => x.Name, p => p.SetName("name").SetNullValueHandling(XmlNullValueHandling.Include))
                .Build();
        }

        private static XmlContract GetContractWithOneAttribute()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p.SetName("id").SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Name, "name")
                .Build();
        }

        private static XmlContract GetContractWithAttributes()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p.SetName("id").SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Name, p => p.SetName("name").SetMappingType(XmlMappingType.Attribute))
                .Build();
        }

        private static XmlContract GetContractWithInnerText()
        {
            return new XmlObjectContractBuilder<Foo>()
                .SetName("test")
                .SetProperty(x => x.Id, p => p.SetName("id").SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Name, p => p.SetMappingType(XmlMappingType.InnerText))
                .Build();
        }

        private IXmlConverter GetConverter()
        {
            return new XmlObjectConverter();
        }

        public class ClassWithKnownTypeProperty
        {
            [XmlElement("name")]
            public string Name { get; set; }

            [XmlElement("foo", Type = typeof(Foo))]
            [XmlElement("fooBar", Type = typeof(FooBar))]
            public IFoo Foo { get; set; }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as ClassWithKnownTypeProperty;
                return other != null &&
                    other.Name == Name &&
                    ((Foo == null && other.Foo == null) || Foo.Equals(other.Foo));
            }
        }

        public class ClassWithCollectionProperty
        {
            public string Name { get; set; }

            [XmlElement("number")]
            public List<int> Numbers { get; set; }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is ClassWithCollectionProperty other)
                {
                    if (Name == other.Name && Numbers.Count == other.Numbers.Count)
                    {
                        for (var i = 0; i < Numbers.Count; i++)
                        {
                            if (Numbers[i] != other.Numbers[i])
                            {
                                return false;
                            }
                        }

                        return true;
                    }
                }

                return false;
            }
        }
    }
}