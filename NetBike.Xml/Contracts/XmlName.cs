namespace NetBike.Xml.Contracts
{
    using System;
    using System.Xml;

    public sealed class XmlName : IEquatable<XmlName>
    {
        public XmlName(string localName)
            : this(localName, null)
        {
        }

        public XmlName(string localName, string namespaceUri)
        {
            if (localName == null)
            {
                throw new ArgumentNullException(nameof(localName));
            }

            XmlConvert.VerifyNCName(localName);

            if (namespaceUri != null)
            {
                XmlNamespace.VerifyNamespaceUri(namespaceUri);
            }

            this.LocalName = localName;
            this.NamespaceUri = namespaceUri;
        }

        public string LocalName { get; }

        public string NamespaceUri { get; }

        public static implicit operator XmlName(string name)
        {
            return new XmlName(name);
        }

        public override string ToString()
        {
            if (this.NamespaceUri != null)
            {
                return "{" + this.NamespaceUri + "}" + this.LocalName;
            }

            return this.LocalName;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as XmlName);
        }

        public override int GetHashCode()
        {
            var hashCode = 17;

            unchecked
            {
                hashCode = 31 * hashCode + this.LocalName.GetHashCode();

                if (this.NamespaceUri != null)
                {
                    hashCode = 31 * hashCode + this.NamespaceUri.GetHashCode();
                }
            }

            return hashCode;
        }

        public bool Equals(XmlName other)
        {
            if (other == null)
            {
                return false;
            }

            return this.LocalName == other.LocalName && this.NamespaceUri == other.NamespaceUri;
        }

        internal XmlName Create(string localName, string namespaceUri)
        {
            if (string.IsNullOrEmpty(localName))
            {
                localName = this.LocalName;
            }

            if (string.IsNullOrEmpty(namespaceUri))
            {
                namespaceUri = this.NamespaceUri;
            }

            return new XmlName(localName, namespaceUri);
        }
    }
}