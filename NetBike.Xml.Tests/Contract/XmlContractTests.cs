namespace NetBike.Xml.Tests.Contract
{
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Contracts.Builders;
    using NUnit.Framework;

    [TestFixture]
    public class XmlContractTests
    {
        [Test]
        public void PropertiesOrderTest()
        {
            var contract = new XmlObjectContractBuilder<Test>()
                .SetName("test")
                .SetProperty(x => x.Prop1, p => p.SetOrder(10))
                .SetProperty(x => x.Prop2, p => p.SetOrder(3))
                .SetProperty(x => x.Prop3)
                .Build();

            var properties = contract.Properties;

            Assert.AreEqual("Prop3", properties[0].PropertyName);
            Assert.AreEqual("Prop2", properties[1].PropertyName);
            Assert.AreEqual("Prop1", properties[2].PropertyName);
        }

        [Test]
        public void PropertiesOrderWithAttributeMappingTest()
        {
            var contract = new XmlObjectContractBuilder<Test>()
                .SetProperty(x => x.Prop1, p => p.SetOrder(10))
                .SetProperty(x => x.Prop2)
                .SetProperty(x => x.Prop3, p => p.SetMappingType(XmlMappingType.Attribute).SetOrder(1))
                .SetProperty(x => x.Prop4, p => p.SetMappingType(XmlMappingType.Attribute))
                .Build();

            var properties = contract.Properties;

            Assert.AreEqual("Prop4", properties[0].PropertyName);
            Assert.AreEqual("Prop3", properties[1].PropertyName);
            Assert.AreEqual("Prop2", properties[2].PropertyName);
            Assert.AreEqual("Prop1", properties[3].PropertyName);
        }

        [Test]
        public void PropertiesOrderWithInnerTestMappingTest()
        {
            var contract = new XmlObjectContractBuilder<Test>()
                .SetProperty(x => x.Prop1, p => p.SetMappingType(XmlMappingType.Attribute))
                .SetProperty(x => x.Prop2, p => p.SetMappingType(XmlMappingType.InnerText))
                .SetProperty(x => x.Prop3, p => p.SetMappingType(XmlMappingType.Attribute))
                .Build();

            var properties = contract.Properties;

            Assert.AreEqual("Prop1", properties[0].PropertyName);
            Assert.AreEqual("Prop3", properties[1].PropertyName);
            Assert.AreEqual("Prop2", properties[2].PropertyName);
        }

        public class Test
        {
            public int Prop1 { get; set; }

            public int Prop2 { get; set; }

            public int Prop3 { get; set; }

            public int Prop4 { get; set; }
        }
    }
}