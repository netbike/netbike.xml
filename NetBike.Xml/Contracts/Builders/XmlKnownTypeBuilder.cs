namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public class XmlKnownTypeBuilder : XmlMemberBuilder, IXmlObjectBuilder
    {
        public XmlKnownTypeBuilder(Type valueType)
            : base(valueType)
        {
        }

        public XmlNullValueHandling? NullValueHandling { get; set; }

        public XmlDefaultValueHandling? DefaultValueHandling { get; set; }

        public object DefaultValue { get; set; }

        public static XmlKnownTypeBuilder Create(XmlKnownType knownType)
        {
            if (knownType == null)
            {
                throw new ArgumentNullException(nameof(knownType));
            }

            return new XmlKnownTypeBuilder(knownType.ValueType)
            {
                Name = knownType.Name,
                TypeHandling = knownType.TypeHandling,
                NullValueHandling = knownType.NullValueHandling,
                DefaultValueHandling = knownType.DefaultValueHandling,
                DefaultValue = knownType.DefaultValue
            };
        }

        public XmlKnownType Build()
        {
            return new XmlKnownType(
                this.ValueType,
                this.Name ?? this.ValueType.GetShortName(),
                this.TypeHandling,
                this.NullValueHandling,
                this.DefaultValueHandling,
                this.DefaultValue);
        }
    }
}