namespace NetBike.Xml.Converters.Basics
{
    public sealed class XmlStringConverter : XmlBasicConverter<string>
    {
        protected override string Parse(string value, XmlSerializationContext context)
        {
            return value;
        }

        protected override string ToString(string value, XmlSerializationContext context)
        {
            return value;
        }
    }
}