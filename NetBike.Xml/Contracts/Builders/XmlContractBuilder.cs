namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public abstract class XmlContractBuilder : IXmlBuilder
    {
        public XmlContractBuilder(Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            this.ValueType = valueType;
        }

        public Type ValueType { get; private set; }

        public XmlName Name { get; set; }

        public abstract XmlContract BuildContract();
    }
}