namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlEnumContract : XmlContract
    {
        private readonly List<XmlEnumItem> items;

        public XmlEnumContract(Type valueType, XmlName name, IEnumerable<XmlEnumItem> items)
            : base(valueType, name)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (!valueType.IsEnum)
            {
                throw new ArgumentException("Expected enum type.", nameof(valueType));
            }

            this.items = new List<XmlEnumItem>(items);
            this.IsFlag = valueType.IsDefined(Types.FlagsAttribute, false);
            this.UnderlyingType = Enum.GetUnderlyingType(valueType);
        }

        public Type UnderlyingType { get; }

        public IEnumerable<XmlEnumItem> Items => this.items;

        public bool IsFlag { get; }
    }
}