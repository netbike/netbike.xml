namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public class XmlItemBuilder : XmlMemberBuilder
    {
        public XmlItemBuilder(Type valueType)
            : base(valueType)
        {
        }

        public XmlKnownTypeBuilderCollection KnownTypes { get; set; }

        public static XmlItemBuilder Create(XmlItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return new XmlItemBuilder(item.ValueType)
            {
                Name = item.Name,
                TypeHandling = item.TypeHandling,
                KnownTypes = item.KnownTypes != null ? XmlKnownTypeBuilderCollection.Create(item.KnownTypes) : null
            };
        }

        public XmlItem Build()
        {
            return new XmlItem(
                this.ValueType,
                this.Name ?? this.ValueType.GetShortName(),
                this.TypeHandling,
                this.KnownTypes?.Build());
        }
    }
}