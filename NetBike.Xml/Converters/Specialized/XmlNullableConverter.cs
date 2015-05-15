namespace NetBike.Xml.Converters.Specialized
{
    using System;
    using System.Xml;
    using NetBike.Xml.Contracts;

    public sealed class XmlNullableConverter : IXmlConverter
    {
        public bool CanRead(Type valueType)
        {
            return valueType.IsNullable();
        }

        public bool CanWrite(Type valueType)
        {
            return valueType.IsNullable();
        }

        public void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var underlyingType = context.ValueType.GetUnderlyingNullableType();
            context.SerializeBody(writer, value, underlyingType);
        }

        public object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            var member = context.Member;
            var underlyingType = member.ValueType.GetUnderlyingNullableType();

            if (member.MappingType == XmlMappingType.Element)
            {
                if (!context.ReadValueType(reader, ref underlyingType))
                {
                    reader.Skip();
                    return null;
                }
            }

            return context.Deserialize(reader, underlyingType);
        }
    }
}