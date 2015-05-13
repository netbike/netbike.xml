namespace NetBike.Xml.Converters
{
    using System;
    using System.Xml;

    public abstract class XmlConverter<T> : IXmlConverter
    {
        public bool CanRead(Type valueType)
        {
            return valueType == typeof(T);
        }

        public bool CanWrite(Type valueType)
        {
            return valueType == typeof(T);
        }

        public abstract void WriteXml(XmlWriter writer, object value, XmlSerializationContext context);

        public abstract object ReadXml(XmlReader reader, XmlSerializationContext context);
    }
}