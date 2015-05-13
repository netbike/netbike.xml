namespace NetBike.Xml.Tests.Contract
{
    using NetBike.Xml.Contracts;
    using NUnit.Framework;

    [TestFixture]
    public class NamingConventionsTests
    {
        [Test]
        public void StringToCamelCaseTest()
        {
            var actual = NamingConventions.CamelCase("HelloWorld");
            var expected = "helloWorld";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CapsStringToCamelCaseTest()
        {
            var actual = NamingConventions.CamelCase("ID");
            var expected = "id";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StringToDashedTest()
        {
            var actual = NamingConventions.Dashed("HelloWorld");
            var expected = "hello-world";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StringWithNumbersToDashedTest()
        {
            var actual = NamingConventions.Dashed("1Test123F5");
            var expected = "1-test123-f5";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void UpperStringToDashedTest()
        {
            var actual = NamingConventions.Dashed("TEST");
            var expected = "test";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void VariantStringToDashedTest()
        {
            var actual = NamingConventions.Dashed("abcID");
            var expected = "abc-id";
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void StringToUnderscoreTest()
        {
            var actual = NamingConventions.Underscore("HelloWorld");
            var expected = "hello_world";
            Assert.AreEqual(expected, actual);
        }
    }
}