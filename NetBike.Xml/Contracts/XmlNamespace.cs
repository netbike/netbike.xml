namespace NetBike.Xml.Contracts
{
    using System;
    using System.Xml;

    public class XmlNamespace
    {
        internal const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
        internal const string Xsd = "http://www.w3.org/2001/XMLSchema";

        public XmlNamespace(string prefix, string namespaceUri)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (namespaceUri == null)
            {
                throw new ArgumentNullException(nameof(namespaceUri));
            }

            XmlConvert.VerifyNCName(prefix);

            VerifyNamespaceUri(namespaceUri);

            this.Prefix = prefix;
            this.NamespaceUri = namespaceUri;
        }

        public string Prefix { get; }

        public string NamespaceUri { get; }

        internal static void VerifyNamespaceUri(string namespaceUri)
        {
            if (!Uri.TryCreate(namespaceUri, UriKind.RelativeOrAbsolute, out _))
            {
                throw new ArgumentException("Invalid XML namespace", "ns");
            }
        }
    }
}