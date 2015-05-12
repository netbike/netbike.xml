namespace NetBike.Xml
{
    using System.Xml;
    using NetBike.Xml.Contracts;

    internal struct XmlNameRef
    {
        public object LocalName;
        public object NamespaceUri;

        public XmlNameRef(XmlName name, XmlNameTable nameTable)
        {
            this.LocalName = nameTable.Add(name.LocalName);
            this.NamespaceUri = name.NamespaceUri != null ? nameTable.Add(name.NamespaceUri) : null;
        }

        public void Reset(XmlName name, XmlNameTable nameTable)
        {
            this.LocalName = nameTable.Add(name.LocalName);
            this.NamespaceUri = name.NamespaceUri != null ? nameTable.Add(name.NamespaceUri) : null;
        }

        public bool Match(XmlReader reader)
        {
            if (object.ReferenceEquals(this.LocalName, reader.LocalName))
            {
                return this.NamespaceUri == null || object.ReferenceEquals(this.NamespaceUri, reader.NamespaceURI);
            }

            return false;
        }
    }
}