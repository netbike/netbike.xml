namespace NetBike.Xml.Converters.Basics
{
    using System.Xml;

    public sealed class XmlSingleConverter : XmlBasicRawConverter<float>
    {
        protected override float Parse(string value, XmlSerializationContext context)
        {
            return XmlConvert.ToSingle(value);
        }

        protected override string ToString(float value, XmlSerializationContext context)
        {
            return XmlConvert.ToString(value);
        }
    }
}
