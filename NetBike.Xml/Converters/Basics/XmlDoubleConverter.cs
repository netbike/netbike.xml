namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlDoubleConverter : XmlBasicRawConverter<double>
    {
        protected override double Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToDouble(value);
        }

        protected override string ToString(double value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}