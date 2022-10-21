namespace NetBike.Xml.Tests.Converters
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;
    using NUnit.Framework;

    public static class XmlConverterExtensions
    {
        public static TValue ParseXml<TValue>(this IXmlConverter converter, string xmlString, XmlMember member = null, XmlContract contract = null, XmlSerializerSettings settings = null)
        {
            return (TValue)ParseXml(converter, typeof(TValue), xmlString, member, contract, settings);
        }

        public static string ToXml<TValue>(this IXmlConverter converter, TValue value, XmlMember member = null, XmlContract contract = null, XmlSerializerSettings settings = null)
        {
            return ToXml(converter, typeof(TValue), value, member, contract, settings);
        }

        public static object ParseXml(this IXmlConverter converter, Type valueType, string xmlString, XmlMember member = null, XmlContract contract = null, XmlSerializerSettings settings = null)
        {
            var context = CreateContext(valueType, member, contract, settings);

            using var input = new StringReader(xmlString);
            using var reader = XmlReader.Create(input, context.Settings.GetReaderSettings());
            while (reader.NodeType != XmlNodeType.Element && reader.Read())
            {
            }

            if (reader.Name != "xml")
            {
                Assert.Fail("Expected start element \"xml\".");
            }

            var isAttribute = context.Member.MappingType == XmlMappingType.Attribute;

            if (isAttribute)
            {
                reader.MoveToAttribute(context.Member.Name.LocalName, context.Member.Name.NamespaceUri);
            }

            var value = converter.ReadXml(reader, context);

            if (isAttribute)
            {
                reader.MoveToElement();

                if (reader.NodeType != XmlNodeType.Element || reader.Name != "xml")
                {
                    Assert.Fail("Expected element \"xml\".");
                }
            }
            else if (reader.NodeType != XmlNodeType.None)
            {
                Assert.Fail("Expected end of xml.");
            }

            return value;
        }

        public static string ToXml(this IXmlConverter converter, Type valueType, object value, XmlMember member = null, XmlContract contract = null, XmlSerializerSettings settings = null)
        {
            var builder = new StringBuilder();
            var context = CreateContext(valueType, member, contract, settings);

            using (var output = new StringWriter(builder))
            using (var writer = XmlWriter.Create(output, context.Settings.GetWriterSettings()))
            {
                writer.WriteStartElement("xml");

                var isAttribute = context.Member.MappingType == XmlMappingType.Attribute;

                if (isAttribute)
                {
                    writer.WriteStartAttribute(context.Member.Name);
                }

                converter.WriteXml(writer, value, context);

                if (isAttribute)
                {
                    writer.WriteEndAttribute();
                }

                writer.WriteEndElement();
            }

            return builder.ToString();
        }

        private static XmlSerializationContext CreateContext(Type valueType, XmlMember member, XmlContract contract, XmlSerializerSettings settings)
        {
            settings ??= new XmlSerializerSettings
            {
                OmitXmlDeclaration = true,
                ContractResolver = new XmlContractResolver(NamingConventions.CamelCase)
            };

            contract ??= settings.ContractResolver.ResolveContract(valueType);

            member ??= contract.Root;

            return new XmlSerializationContext(settings, member, contract);
        }
    }
}