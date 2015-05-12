namespace NetBike.Xml.Converters
{
    using System;
    using System.Xml;

    public interface IXmlConverter
    {
        bool CanRead(Type valueType);

        bool CanWrite(Type valueType);

        void WriteXml(XmlWriter writer, object value, XmlSerializationContext context);

        object ReadXml(XmlReader reader, XmlSerializationContext context);
    }
}