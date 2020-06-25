namespace NetBike.Xml.Contracts
{
    using System;

    public sealed class XmlEnumItem
    {
        public XmlEnumItem(long value, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            for (var i = 0; i < name.Length; i++)
            {
                var symbol = name[i];

                if (!char.IsLetterOrDigit(symbol) && symbol != '-' && symbol != '_' && symbol != ':')
                {
                    throw new ArgumentException("Invalid xml name of enum item.");
                }
            }

            this.Name = name;
            this.Value = value;
        }

        public string Name { get; }

        public long Value { get; }
    }
}