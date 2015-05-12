namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlCharConverter : XmlBasicRawConverter<char>
    {
        protected override char Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToChar(value);
        }

        protected override string ToString(char value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}