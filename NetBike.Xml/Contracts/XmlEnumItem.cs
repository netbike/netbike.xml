namespace NetBike.Xml.Contracts
{
    using System;

    public sealed class XmlEnumItem
    {
        private readonly long value;
        private readonly string name;

        public XmlEnumItem(long value, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            for (var i = 0; i < name.Length; i++)
            {
                var symbol = name[i];

                if (!char.IsLetterOrDigit(symbol) && symbol != '-' && symbol != '_' && symbol != ':')
                {
                    throw new ArgumentException("Invalid xml name of enum item.");
                }
            }

            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get { return this.name; }
        }

        public long Value
        {
            get { return this.value; }
        }
    }
}