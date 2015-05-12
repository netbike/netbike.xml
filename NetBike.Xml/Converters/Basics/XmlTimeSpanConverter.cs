namespace NetBike.Xml.Converters.Basics
{
    using System;
    using System.Xml;

    public sealed class XmlTimeSpanConverter : XmlBasicRawConverter<TimeSpan>
    {
        protected override TimeSpan Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToTimeSpan(value);
        }

        protected override string ToString(TimeSpan value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}