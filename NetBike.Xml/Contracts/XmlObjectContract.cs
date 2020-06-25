namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;

    public sealed partial class XmlObjectContract : XmlContract
    {
        private static readonly List<XmlProperty> EmptyProperties = new List<XmlProperty>();

        private readonly List<XmlProperty> properties;

        public XmlObjectContract(
            Type valueType,
            XmlName name,
            IEnumerable<XmlProperty> properties = null,
            XmlTypeHandling? typeHandling = null,
            XmlItem item = null)
            : base(valueType, name)
        {
            var elementCount = 0;

            if (properties == null)
            {
                properties = EmptyProperties;
            }

            this.Item = item;
            this.TypeHandling = typeHandling;
            this.properties = new List<XmlProperty>(properties);

            foreach (var property in this.properties)
            {
                if (property == null)
                {
                    throw new ArgumentNullException("properties.property");
                }

                if (valueType != property.PropertyInfo.ReflectedType)
                {
                    throw new ArgumentException("Property must be declared in contract type.", "properties.property");
                }

                if (property.IsRequired || property.DefaultValue != null)
                {
                    this.HasRequiredOrDefaultsProperties = true;
                }

                if (property.MappingType == XmlMappingType.Element)
                {
                    elementCount++;
                }
                else if (property.MappingType == XmlMappingType.InnerText)
                {
                    if (this.InnerTextProperty != null)
                    {
                        throw new XmlSerializationException("Contract must have only one innerText property.");
                    }

                    this.InnerTextProperty = property;
                }
            }

            if (this.InnerTextProperty != null && elementCount > 0)
            {
                throw new XmlSerializationException("Contract must not contain elements, if it contains innerText property.");
            }

            this.properties.Sort(XmlPropertyComparer.Instance);
        }

        public IReadOnlyList<XmlProperty> Properties => this.properties;

        public XmlItem Item { get; }

        public XmlTypeHandling? TypeHandling { get; }

        internal bool HasRequiredOrDefaultsProperties { get; }

        internal XmlProperty InnerTextProperty { get; }

        protected override XmlMember GetDefaultMember()
        {
            return new XmlMember(
                this.ValueType,
                this.Name,
                XmlMappingType.Element,
                this.TypeHandling,
                null,
                null,
                null,
                this.Item);
        }
    }
}