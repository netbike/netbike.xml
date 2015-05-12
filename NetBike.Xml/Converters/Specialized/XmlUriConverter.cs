namespace NetBike.Xml.Converters.Specialized
{
    using System;

    public sealed class XmlUriConverter : XmlBasicConverter<Uri>
    {
        protected override Uri Parse(string value, XmlSerializationContext context)
        {
            return new Uri(value);
        }

        protected override string ToString(Uri value, XmlSerializationContext context)
        {
            return value.ToString();
        }
    }
}