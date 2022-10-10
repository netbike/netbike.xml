namespace NetBike.Xml.Converters.Basics
{
    using System;
    using NetBike.Xml.Utilities;

    public sealed class XmlDateTimeConverter : XmlBasicRawConverter<DateTime>
    {
        protected override DateTime Parse(string value, XmlSerializationContext context)
        {
            if (context.Member.DataType == "date")
            {
                return RfcDateTime.ParseDate(value);
            }

            return RfcDateTime.ParseDateTime(value);
        }

        protected override string ToString(DateTime value, XmlSerializationContext context)
        {
            if (context.Member.DataType == "date")
            {
                return RfcDateTime.ToDateString(value);
            }

            return RfcDateTime.ToDateTimeString(value);
        }
    }
}