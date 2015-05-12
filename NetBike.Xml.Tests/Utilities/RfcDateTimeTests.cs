namespace NetBike.Xml.Tests.Utilities
{
    using System;
    using NetBike.Xml.Utilities;
    using NUnit.Framework;

    [TestFixture]
    public class RfcDateTimeTests
    {
        [Test]
        public void ToStringUtcDateTest()
        {
            var value = new DateTime(2013, 09, 12, 12, 58, 48, DateTimeKind.Utc).AddTicks(2449990);
            var expected = "2013-09-12T12:58:48.244999Z";
            var actual = RfcDateTime.ToString(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringUnspecifiedDateTest()
        {
            var value = new DateTime(2013, 09, 12, 12, 58, 48);
            var expected = "2013-09-12T12:58:48";
            var actual = RfcDateTime.ToString(value);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringLocalDateTest()
        {
            var expected = new DateTime(2016, 11, 12, 12, 58, 48, DateTimeKind.Local);
            var value = RfcDateTime.ToString(expected);
            var actual = RfcDateTime.ParseDateTime(value);

            Assert.AreEqual(expected, actual.ToLocalTime());
        }

        [TestCase("2013-09-12T12:58:48.2449990Z")]
        [TestCase("2013-09-12T12:58:48.244999Z")]
        [TestCase("2013-09-12T12:58:48.49Z")]
        [TestCase("2013-09-12T12:58:48Z")]
        [TestCase("2013-09-12T12:58Z")]
        [TestCase("2013-09-12T12:58:48.2449990+05:15")]
        [TestCase("2013-09-12T12:58:48.244999-01:01")]
        [TestCase("2013-09-12T12:58:48.49+05:15")]
        [TestCase("2013-09-12T12:58:48-01:01")]
        [TestCase("2013-09-12T12:58+01:01")]
        public void ParseDateTimeWithTZDTest(string validDateTime)
        {
            var expected = DateTime.Parse(validDateTime).ToUniversalTime();
            var actual = RfcDateTime.ParseDateTime(validDateTime);
            Assert.AreEqual(expected, actual);
        }

        [TestCase("2013-09-12T12:58:48.2449990")]
        [TestCase("2013-09-12T12:58:48.244999")]
        [TestCase("2013-09-12T12:58:48.49")]
        [TestCase("2013-09-12T12:58:48")]
        [TestCase("2013-09-12T12:58")]
        [TestCase("2013-09-12T12:58:48.2449990")]
        [TestCase("2013-09-12T12:58:48.244999")]
        [TestCase("2013-09-12T12:58:48.49")]
        [TestCase("2013-09-12T12:58:48")]
        [TestCase("2013-09-12T12:58")]
        public void ParseDateTimeWithoutTZDTest(string value)
        {
            var expected = DateTime.Parse(value);
            var actual = RfcDateTime.ParseDateTime(value);
            Assert.AreEqual(expected, actual);
        }
    }
}