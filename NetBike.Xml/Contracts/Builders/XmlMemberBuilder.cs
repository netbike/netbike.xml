namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public class XmlMemberBuilder : IXmlBuilder
    {
        public XmlMemberBuilder(Type valueType)
        {
            this.ValueType = valueType ?? throw new ArgumentNullException(nameof(valueType));
        }

        public Type ValueType { get; }

        public XmlName Name { get; set; }

        public XmlTypeHandling? TypeHandling { get; set; }
        
        public string DataType { get; set; }
    }
}