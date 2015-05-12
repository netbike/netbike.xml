namespace NetBike.Xml.Converters.Basics
{
    using System;
    using NetBike.Xml.Utilities;

    public sealed class XmlDateTimeConverter : XmlBasicRawConverter<DateTime>
    {
        protected override DateTime Parse(string value, XmlSerializationContext context)
        {
            return RfcDateTime.ParseDateTime(value);
        }

        protected override string ToString(DateTime value, XmlSerializationContext context)
        {
            return RfcDateTime.ToString(value);
        }
    }
}