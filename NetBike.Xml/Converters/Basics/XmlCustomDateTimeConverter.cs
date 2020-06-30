namespace NetBike.Xml.Converters.Basics
{
    using System;

    public sealed class XmlCustomDateTimeConverter : XmlBasicConverter<DateTime>
    {
        public XmlCustomDateTimeConverter(string format = null)
        {
            this.Format = format;
        }

        public string Format { get; }

        protected override DateTime Parse(string value, XmlSerializationContext context)
        {
            return DateTime.ParseExact(value, this.Format, context.Settings.Culture);
        }

        protected override string ToString(DateTime value, XmlSerializationContext context)
        {
            return value.ToString(this.Format, context.Settings.Culture);
        }
    }
}