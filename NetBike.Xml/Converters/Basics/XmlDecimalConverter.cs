namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlDecimalConverter : XmlBasicRawConverter<decimal>
    {
        protected override decimal Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToDecimal(value);
        }

        protected override string ToString(decimal value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}