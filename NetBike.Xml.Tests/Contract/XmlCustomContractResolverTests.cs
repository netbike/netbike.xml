namespace NetBike.Xml.Tests.Contract
{
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Tests.Samples;
    using NUnit.Framework;

    [TestFixture]
    public class XmlCustomContractResolverTests
    {
        [Test]
        public void ResolveKnownContractTest()
        {
            var fooContract = new XmlObjectContract(typeof(Foo), new XmlName("foo"));
            var fooBarContract = new XmlObjectContract(typeof(FooBar), new XmlName("fooBar"));

            var contracts = new XmlContract[] { fooBarContract, fooContract };

            var resolver = new XmlCustomContractResolver(contracts, null);

            Assert.AreSame(fooContract, resolver.ResolveContract(typeof(Foo)));
            Assert.AreSame(fooBarContract, resolver.ResolveContract(typeof(FooBar)));
        }

        [Test]
        public void ResolveUnknownContactTest()
        {
            var contracts = new XmlContract[] { };
            var resolver = new XmlCustomContractResolver(contracts, null);

            Assert.Throws<XmlSerializationException>(() => resolver.ResolveContract(typeof(Foo)));
        }

        [Test]
        public void ResolveUnknownContractByFallbackTest()
        {
            var fooContract = new XmlObjectContract(typeof(Foo), new XmlName("foo"));
            var contracts = new XmlContract[] { fooContract };
            var resolver = new XmlCustomContractResolver(contracts, new XmlContractResolver());

            var actual = resolver.ResolveContract(typeof(FooBar));
            Assert.AreSame(typeof(FooBar), actual.ValueType);
        }
    }
}