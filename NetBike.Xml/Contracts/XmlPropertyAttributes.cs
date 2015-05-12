namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts.Builders;

    internal sealed class XmlPropertyAttributes
    {
        public List<XmlElementAttribute> Elements { get; private set; }

        public List<XmlAttributeAttribute> Attributes { get; private set; }

        public List<XmlArrayItemAttribute> ArrayItems { get; private set; }

        public XmlArrayAttribute Array { get; set; }

        public XmlTextAttribute Text { get; set; }

        public DefaultValueAttribute Default { get; set; }

        public XmlIgnoreAttribute Ignore { get; set; }

        public static XmlPropertyAttributes GetAttributes(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            var attributes = new XmlPropertyAttributes();

            foreach (var attribute in Attribute.GetCustomAttributes(propertyInfo))
            {
                if (attribute is XmlElementAttribute)
                {
                    if (attributes.Elements == null)
                    {
                        attributes.Elements = new List<XmlElementAttribute>();
                    }

                    attributes.Elements.Add((XmlElementAttribute)attribute);
                }
                else if (attribute is XmlAttributeAttribute)
                {
                    if (attributes.Attributes == null)
                    {
                        attributes.Attributes = new List<XmlAttributeAttribute>();
                    }

                    attributes.Attributes.Add((XmlAttributeAttribute)attribute);
                }
                else if (attribute is XmlArrayItemAttribute)
                {
                    if (attributes.ArrayItems == null)
                    {
                        attributes.ArrayItems = new List<XmlArrayItemAttribute>();
                    }

                    attributes.ArrayItems.Add((XmlArrayItemAttribute)attribute);
                }
                else if (attribute is XmlArrayAttribute)
                {
                    attributes.Array = (XmlArrayAttribute)attribute;
                }
                else if (attribute is XmlTextAttribute)
                {
                    attributes.Text = (XmlTextAttribute)attribute;
                }
                else if (attribute is DefaultValueAttribute)
                {
                    attributes.Default = (DefaultValueAttribute)attribute;
                }
                else if (attribute is XmlIgnoreAttribute)
                {
                    attributes.Ignore = (XmlIgnoreAttribute)attribute;
                }
            }

            return attributes;
        }
    }
}