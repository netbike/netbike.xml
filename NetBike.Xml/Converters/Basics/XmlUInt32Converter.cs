namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlUInt32Converter : XmlBasicRawConverter<uint>
    {
        protected override uint Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToUInt32(value);
        }

        protected override string ToString(uint value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}