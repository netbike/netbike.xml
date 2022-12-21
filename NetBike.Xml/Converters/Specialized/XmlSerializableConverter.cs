namespace NetBike.Xml.Converters.Specialized
{
    using System;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public sealed class XmlSerializableConverter : IXmlConverter
    {
        public bool CanRead(Type valueType)
        {
            return IsSerializable(valueType);
        }

        public bool CanWrite(Type valueType)
        {
            return IsSerializable(valueType);
        }

        public void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var serializable = (IXmlSerializable)value;
            serializable.WriteXml(writer);
        }

        public object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            var value = context.Contract.CreateDefault();
            ((IXmlSerializable)value).ReadXml(reader);
            return value;
        }

        private static bool IsSerializable(Type valueType)
        {
            return valueType.GetInterfaces().Any(x => x == typeof(IXmlSerializable));
        }
    }
}