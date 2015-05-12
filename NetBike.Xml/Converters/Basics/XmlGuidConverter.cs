namespace NetBike.Xml.Converters.Basics
{
    using System;
    using System.Xml;

    public sealed class XmlGuidConverter : XmlBasicRawConverter<Guid>
    {
        private readonly string format;

        public XmlGuidConverter(string format = null)
        {
            this.format = format;
        }

        public string Format
        {
            get { return this.format; }
        }

        protected override Guid Parse(string value, XmlSerializationContext context)
        {
            return Guid.Parse(value);
        }

        protected override string ToString(Guid value, XmlSerializationContext context)
        {
            return value.ToString(this.format);
        }
    }
}