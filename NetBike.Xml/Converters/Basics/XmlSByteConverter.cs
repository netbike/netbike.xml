namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlSByteConverter : XmlBasicRawConverter<sbyte>
    {
        protected override sbyte Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToSByte(value);
        }

        protected override string ToString(sbyte value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}