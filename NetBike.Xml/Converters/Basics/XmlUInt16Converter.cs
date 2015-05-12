namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlUInt16Converter : XmlBasicRawConverter<ushort>
    {
        protected override ushort Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToUInt16(value);
        }

        protected override string ToString(ushort value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}