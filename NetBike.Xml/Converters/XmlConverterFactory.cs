namespace NetBike.Xml.Converters
{
    using System;
    using System.Xml;
    using NetBike.Xml.Contracts;

    public abstract class XmlConverterFactory : IXmlConverterFactory, IXmlConverter
    {
        public virtual IXmlConverter CreateConverter(XmlContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var valueType = contract.ValueType;

            if (!this.AcceptType(valueType))
            {
                throw new ArgumentException($"Type \"{valueType}\" is not acceptable.", nameof(contract));
            }

            var converterType = this.GetConverterType(valueType);
            return (IXmlConverter)Activator.CreateInstance(converterType);
        }

        public virtual bool CanRead(Type valueType)
        {
            return this.AcceptType(valueType);
        }

        public virtual bool CanWrite(Type valueType)
        {
            return this.AcceptType(valueType);
        }

        public void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var converter = this.CreateConverter(context.Contract);
            converter.WriteXml(writer, value, context);
        }

        public object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            var converter = this.CreateConverter(context.Contract);
            return converter.ReadXml(reader, context);
        }

        protected abstract bool AcceptType(Type valueType);

        protected abstract Type GetConverterType(Type valueType);
    }
}