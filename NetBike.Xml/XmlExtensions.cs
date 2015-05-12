namespace NetBike.Xml
{
    using System.Xml;
    using NetBike.Xml.Contracts;

    internal static class XmlExtensions
    {
        public static void WriteStartElement(this XmlWriter writer, XmlName name)
        {
            writer.WriteStartElement(null, name.LocalName, name.NamespaceUri);
        }

        public static void WriteStartAttribute(this XmlWriter writer, XmlName name)
        {
            writer.WriteStartAttribute(null, name.LocalName, name.NamespaceUri);
        }

        public static void WriteAttributeString(this XmlWriter writer, XmlName name, string value)
        {
            writer.WriteStartAttribute(null, name.LocalName, name.NamespaceUri);
            writer.WriteString(value);
            writer.WriteEndAttribute();
        }

        public static void WriteNamespace(this XmlWriter writer, XmlNamespace ns)
        {
            writer.WriteAttributeString("xmlns", ns.Prefix, null, ns.NamespaceUri);
        }

        public static string ReadAttributeOrElementContent(this XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Attribute)
            {
                return reader.Value;
            }

            return reader.ReadElementContentAsString();
        }
    }
}