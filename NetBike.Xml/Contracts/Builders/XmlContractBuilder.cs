namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public abstract class XmlContractBuilder : IXmlBuilder
    {
        public XmlContractBuilder(Type valueType)
        {
            this.ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
        }

        public Type ValueType { get; private set; }

        public XmlName Name { get; set; }

        public abstract XmlContract BuildContract();
    }
}