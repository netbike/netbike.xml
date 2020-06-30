namespace NetBike.Xml
{
    using System;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;

    internal class XmlTypeContext
    {
        public XmlTypeContext(XmlContract contract, IXmlConverter readConverter, IXmlConverter writeConverter)
        {
            this.Contract = contract;
            this.ReadConverter = readConverter;
            this.WriteConverter = writeConverter;
            this.ReadXml = readConverter != null ? readConverter.ReadXml : NotReadable(contract.ValueType);
            this.WriteXml = writeConverter != null ? writeConverter.WriteXml : NotWritable(contract.ValueType);
        }

        public XmlContract Contract { get; }

        public IXmlConverter ReadConverter { get; }

        public IXmlConverter WriteConverter { get; }

        public Func<XmlReader, XmlSerializationContext, object> ReadXml { get; }

        public Action<XmlWriter, object, XmlSerializationContext> WriteXml { get; }

        private static Func<XmlReader, XmlSerializationContext, object> NotReadable(Type valueType)
        {
            return (r, c) => throw new XmlSerializationException($"Readable converter for the type \"{valueType}\" is not found.");
        }

        private static Action<XmlWriter, object, XmlSerializationContext> NotWritable(Type valueType)
        {
            return (w, v, c) => throw new XmlSerializationException(
                $"Writable converter for the type \"{valueType}\" is not found.");
        }
    }
}