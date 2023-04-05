namespace NetBike.Xml.Contracts
{
    using System;

    public sealed class XmlEnumItem
    {
        public XmlEnumItem(long value, string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Value = value;
        }

        public string Name { get; }

        public long Value { get; }
    }
}