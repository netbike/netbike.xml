namespace NetBike.Xml.Contracts
{
    using System;
    using System.Xml;

    public class XmlNamespace
    {
        internal const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";
        internal const string Xsd = "http://www.w3.org/2001/XMLSchema";

        private readonly string prefix;
        private readonly string namespaceUri;

        public XmlNamespace(string prefix, string namespaceUri)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException("prefix");
            }

            if (namespaceUri == null)
            {
                throw new ArgumentNullException("namespaceUri");
            }

            XmlConvert.VerifyNCName(prefix);

            VerifyNamespaceUri(namespaceUri);

            this.prefix = prefix;
            this.namespaceUri = namespaceUri;
        }

        public string Prefix
        {
            get { return this.prefix; }
        }

        public string NamespaceUri
        {
            get { return this.namespaceUri; }
        }

        internal static void VerifyNamespaceUri(string namespaceUri)
        {
            Uri uri;

            if (!Uri.TryCreate(namespaceUri, UriKind.RelativeOrAbsolute, out uri))
            {
                throw new ArgumentException("Invalid XML namespace", "ns");
            }
        }
    }
}