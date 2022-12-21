namespace NetBike.Xml.Converters.Collections
{
    using System.Xml;
    using NetBike.Xml.Contracts;

    internal struct XmlItemInfo
    {
        public XmlNameRef NameRef;
        public XmlItem Item;
        public XmlNameRef[] KnownNameRefs;

        public XmlItemInfo(XmlItem item, XmlNameTable nameTable)
        {
            this.Item = item;
            this.NameRef = new XmlNameRef(item.Name, nameTable);
            this.KnownNameRefs = GetKnownNameRefs(item, nameTable);
        }

        public XmlMember Match(XmlReader reader)
        {
            if (this.NameRef.Match(reader))
            {
                return this.Item;
            }

            if (this.KnownNameRefs != null)
            {
                for (var i = 0; i < this.KnownNameRefs.Length; i++)
                {
                    if (this.KnownNameRefs[i].Match(reader))
                    {
                        return this.Item.KnownTypes[i];
                    }
                }
            }

            return null;
        }

        internal static XmlNameRef[] GetKnownNameRefs(XmlMember item, XmlNameTable nameTable)
        {
            if (item.KnownTypes.Count == 0)
            {
                return null;
            }

            var nameRefs = new XmlNameRef[item.KnownTypes.Count];

            for (var i = 0; i < nameRefs.Length; i++)
            {
                nameRefs[i].Reset(item.KnownTypes[i].Name, nameTable);
            }

            return nameRefs;
        }
    }
}