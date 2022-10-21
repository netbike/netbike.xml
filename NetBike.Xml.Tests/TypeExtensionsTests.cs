namespace NetBike.Xml.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Xml;
    using NUnit.Framework;

    [TestFixture]
    public class TypeExtensionsTests
    {
        [Test]
        public void ArrayIsEnumerableTest()
        {
            var value = new int[] { };
            Assert.IsTrue(value.GetType().IsEnumerable());
        }

        [Test]
        public void ArrayListIsEnumerableTest()
        {
            var value = new ArrayList();
            Assert.IsTrue(value.GetType().IsEnumerable());
        }

        [Test]
        public void ListIsEnumerableTest()
        {
            var value = new List<int>();
            Assert.IsTrue(value.GetType().IsEnumerable());
        }

        [Test]
        public void ObjectIsNotEnumerableTest()
        {
            var value = new object();
            Assert.IsFalse(value.GetType().IsEnumerable());
        }

        [Test]
        public void TestClass1IsActivableTest()
        {
            Assert.IsTrue(typeof(TestClass1).IsActivable());
        }

        [Test]
        public void TestClass2IsNotActivableTest()
        {
            Assert.IsFalse(typeof(TestClass2).IsActivable());
        }

        [Test]
        public void GetListItemTypeTest()
        {
            var listType = typeof(List<int>);
            Assert.AreEqual(typeof(int), listType.GetEnumerableItemType());
        }

        [Test]
        public void GetArrayItemTypeTest()
        {
            var arrayType = typeof(string[]);
            Assert.AreEqual(typeof(string), arrayType.GetEnumerableItemType());
        }

        [Test]
        public void GetDictionaryElementTypeTest()
        {
            var dicType = typeof(Dictionary<string, object>);
            Assert.AreEqual(typeof(KeyValuePair<string, object>), dicType.GetEnumerableItemType());
        }

        [Test]
        public void GetCollectionElementTypeForNotEnumerableObjectTest()
        {
            Assert.IsNull(typeof(DateTime).GetEnumerableItemType());
            Assert.IsNull(typeof(object).GetEnumerableItemType());
        }

        [Test]
        public void GetStaticMethodTest()
        {
            var ownerType = typeof(XmlConvert);

            var expected = ownerType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(byte) }, null);
            var actual = ownerType.GetStaticMethod("ToString", typeof(byte));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetInstanceMethodTest()
        {
            var ownerType = typeof(XmlWriter);

            var expected = ownerType.GetMethod("WriteStartElement", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(string) }, null);
            var actual = ownerType.GetInstanceMethod("WriteStartElement", typeof(string), typeof(string));

            Assert.AreEqual(expected, actual);
        }

        public class TestClass1
        {
            public TestClass1(int i)
            {
            }

            public TestClass1()
            {
            }
        }

        public class TestClass2
        {
            public TestClass2(string s)
            {
            }
        }
    }
}
