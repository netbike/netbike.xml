namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlInt64Converter : XmlBasicRawConverter<long>
    {
        protected override long Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToInt64(value);
        }

        protected override string ToString(long value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}