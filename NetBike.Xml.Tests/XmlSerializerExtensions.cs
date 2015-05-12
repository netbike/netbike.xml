namespace NetBike.Xml.Tests
{
    using System;
    using System.IO;

    public static class XmlSerializerExtensions
    {
        public static string ToXml<T>(this XmlSerializer serializer, T value)
        {
            return ToXml(serializer, typeof(T), value);
        }

        public static string ToXml(this XmlSerializer serializer, Type valueType, object value)
        {
            var output = new StringWriter();
            serializer.Serialize(output, valueType, value);
            return output.ToString();
        }

        public static T ParseXml<T>(this XmlSerializer serializer, string xmlString)
        {
            return (T)ParseXml(serializer, typeof(T), xmlString);
        }

        public static object ParseXml(this XmlSerializer serializer, Type valueType, string xmlString)
        {
            return serializer.Deserialize(new StringReader(xmlString), valueType);
        }
    }
}