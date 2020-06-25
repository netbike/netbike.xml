namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;
    using System.Xml.Serialization;

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
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var attributes = new XmlPropertyAttributes();

            foreach (var attribute in Attribute.GetCustomAttributes(propertyInfo))
            {
                switch (attribute)
                {
                    case XmlElementAttribute elementAttribute:
                    {
                        if (attributes.Elements == null)
                        {
                            attributes.Elements = new List<XmlElementAttribute>();
                        }

                        attributes.Elements.Add(elementAttribute);
                        break;
                    }
                    case XmlAttributeAttribute attributeAttribute:
                    {
                        if (attributes.Attributes == null)
                        {
                            attributes.Attributes = new List<XmlAttributeAttribute>();
                        }

                        attributes.Attributes.Add(attributeAttribute);
                        break;
                    }
                    case XmlArrayItemAttribute itemAttribute:
                    {
                        if (attributes.ArrayItems == null)
                        {
                            attributes.ArrayItems = new List<XmlArrayItemAttribute>();
                        }

                        attributes.ArrayItems.Add(itemAttribute);
                        break;
                    }
                    case XmlArrayAttribute arrayAttribute:
                        attributes.Array = arrayAttribute;
                        break;
                    case XmlTextAttribute textAttribute:
                        attributes.Text = textAttribute;
                        break;
                    case DefaultValueAttribute valueAttribute:
                        attributes.Default = valueAttribute;
                        break;
                    case XmlIgnoreAttribute ignoreAttribute:
                        attributes.Ignore = ignoreAttribute;
                        break;
                }
            }

            return attributes;
        }
    }
}