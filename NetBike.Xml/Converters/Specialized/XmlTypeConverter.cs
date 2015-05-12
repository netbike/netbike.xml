namespace NetBike.Xml.Converters.Specialized
{
    using System;

    public sealed class XmlTypeConverter : XmlBasicConverter<Type>
    {
        protected override Type Parse(string value, XmlSerializationContext context)
        {
            return Type.GetType(value, true, false);
        }

        protected override string ToString(Type value, XmlSerializationContext context)
        {
            return value.ToString();
        }
    }
}