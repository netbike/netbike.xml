namespace NetBike.Xml
{
    using System;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;

    internal class XmlTypeContext
    {
        private readonly XmlContract contract;
        private readonly IXmlConverter readConverter;
        private readonly IXmlConverter writeConverter;
        private readonly Func<XmlReader, XmlSerializationContext, object> reader;
        private readonly Action<XmlWriter, object, XmlSerializationContext> writer;

        public XmlTypeContext(XmlContract contract, IXmlConverter readConverter, IXmlConverter writeConverter)
        {
            this.contract = contract;
            this.readConverter = readConverter;
            this.writeConverter = writeConverter;
            this.reader = readConverter != null ? readConverter.ReadXml : NotReadable(contract.ValueType);
            this.writer = writeConverter != null ? writeConverter.WriteXml : NotWritable(contract.ValueType);
        }

        public XmlContract Contract
        {
            get { return this.contract; }
        }

        public IXmlConverter ReadConverter
        {
            get { return this.readConverter; }
        }

        public IXmlConverter WriteConverter
        {
            get { return this.writeConverter; }
        }

        public Func<XmlReader, XmlSerializationContext, object> ReadXml
        {
            get { return this.reader; }
        }

        public Action<XmlWriter, object, XmlSerializationContext> WriteXml
        {
            get { return this.writer; }
        }

        private static Func<XmlReader, XmlSerializationContext, object> NotReadable(Type valueType)
        {
            return (r, c) =>
            {
                throw new XmlSerializationException(string.Format("Readable converter for the type \"{0}\" is not found.", valueType));
            };
        }

        private static Action<XmlWriter, object, XmlSerializationContext> NotWritable(Type valueType)
        {
            return (w, v, c) =>
            {
                throw new XmlSerializationException(string.Format("Writable converter for the type \"{0}\" is not found.", valueType));
            };
        }
    }
}