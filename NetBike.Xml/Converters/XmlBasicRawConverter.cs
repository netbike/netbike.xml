namespace NetBike.Xml.Converters
{
    using System.Xml;

    public abstract class XmlBasicRawConverter<T> : XmlBasicConverter<T>
    {
        public override void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var valueString = ToString((T)value, context);

            if (valueString != null)
            {
                writer.WriteRaw(valueString);
            }
        }
    }
}