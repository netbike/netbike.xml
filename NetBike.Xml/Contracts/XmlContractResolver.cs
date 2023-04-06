namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using NetBike.Xml.Contracts.Builders;

    public class XmlContractResolver : IXmlContractResolver
    {
        private readonly Func<string, string> nameResolver;
        private readonly bool ignoreSystemAttributes;

        public XmlContractResolver(bool ignoreSystemAttributes = false)
            : this(NamingConventions.Ignore, ignoreSystemAttributes)
        {
        }

        public XmlContractResolver(Func<string, string> nameResolver, bool ignoreSystemAttributes = false)
        {
            this.ignoreSystemAttributes = ignoreSystemAttributes;
            this.nameResolver = nameResolver ?? throw new ArgumentNullException(nameof(nameResolver));
        }

        public virtual XmlContract ResolveContract(Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            var name = this.ResolveContractName(valueType);

            if (IsBasicContract(valueType))
            {
                return new XmlContract(valueType, name);
            }
            else if (valueType.IsEnum)
            {
                return new XmlEnumContract(valueType, name, this.ResolveEnumItems(valueType));
            }

            var properties = this.GetProperties(valueType)
                .Select(x => this.ResolveProperty(x))
                .Where(x => x != null);

            return new XmlObjectContract(valueType, name, properties, null, this.ResolveItem(valueType));
        }

        protected virtual XmlName ResolveName(Type valueType)
        {
            return this.ResolveName(valueType, valueType.GetShortName());
        }

        protected virtual XmlName ResolveName(Type valueType, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Length == 0)
            {
                throw new ArgumentException("Name is empty.", nameof(name));
            }

            return new XmlName(this.GetLocalName(name));
        }

        protected virtual XmlName ResolveContractName(Type valueType)
        {
            if (!this.ignoreSystemAttributes)
            {
                var rootAttribute = valueType
                    .GetCustomAttributes(typeof(XmlRootAttribute), false)
                    .Cast<XmlRootAttribute>().FirstOrDefault();

                if (rootAttribute != null)
                {
                    return new XmlName(rootAttribute.ElementName, rootAttribute.Namespace);
                }
            }

            return this.ResolveName(valueType);
        }

        protected virtual string GetLocalName(string name)
        {
            return this.nameResolver(name);
        }

        protected virtual string GetEnumItemName(Type valueType, string name)
        {
            return this.nameResolver(name);
        }

        protected virtual IEnumerable<PropertyInfo> GetProperties(Type valueType)
        {
            return valueType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetIndexParameters().Length == 0);
        }

        protected virtual XmlItem ResolveItem(Type valueType)
        {
            if (valueType == Types.String)
            {
                return null;
            }

            var itemType = valueType.GetEnumerableItemType();

            if (itemType == null)
            {
                return null;
            }

            return new XmlItem(itemType, this.ResolveName(itemType));
        }

        protected virtual IEnumerable<XmlEnumItem> ResolveEnumItems(Type valueType)
        {
            var fields = valueType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var count = fields.Length;
            var items = new XmlEnumItem[count];

            for (int i = 0; i < count; i++)
            {
                var field = fields[i];
                var name = this.GetEnumItemName(valueType, field.Name);
                var value = Convert.ToInt64(field.GetRawConstantValue());

                if (!this.ignoreSystemAttributes)
                {
                    var xmlEnum = field
                        .GetCustomAttributes(typeof(XmlEnumAttribute), false)
                        .Cast<XmlEnumAttribute>()
                        .FirstOrDefault();

                    if (xmlEnum != null)
                    {
                        name = xmlEnum.Name;
                    }
                }

                items[i] = new XmlEnumItem(value, name);
            }

            return items;
        }

        protected virtual XmlProperty ResolveProperty(PropertyInfo propertyInfo)
        {
            var propertyBuilder = new XmlPropertyBuilder(propertyInfo)
            {
                Name = this.ResolveName(propertyInfo.PropertyType, propertyInfo.Name)
            };

            if (!this.SetPropertyAttributes(propertyBuilder))
            {
                return null;
            }

            return propertyBuilder.Build();
        }

        private static bool IsBasicContract(Type valueType)
        {
            return valueType.IsPrimitive || valueType == Types.String;
        }

        private bool SetPropertyAttributes(XmlPropertyBuilder propertyBuilder)
        {
            if (this.ignoreSystemAttributes)
            {
                return true;
            }

            var attributes = XmlPropertyAttributes.GetAttributes(propertyBuilder.PropertyInfo);

            if (attributes.Ignore != null)
            {
                return false;
            }

            var propertyName = propertyBuilder.Name;
            var propertyType = propertyBuilder.PropertyInfo.PropertyType;
            var item = this.ResolveItem(propertyType);

            if (attributes.Elements != null)
            {
                foreach (var attribute in attributes.Elements)
                {
                    var name = propertyName.Create(attribute.ElementName, attribute.Namespace);

                    if (attribute.Type == null || attribute.Type == propertyType)
                    {
                        propertyBuilder.SetName(name)
                            .SetMappingType(XmlMappingType.Element)
                            .SetNullable(attribute.IsNullable)
                            .SetOrder(attribute.Order)
                            .SetDataType(attribute.DataType);
                    }
                    else if (propertyType.IsAssignableFrom(attribute.Type))
                    {
                        propertyBuilder.SetKnownType(
                            attribute.Type,
                            x => x.SetName(name).SetNullable(attribute.IsNullable));
                    }

                    if (item != null)
                    {
                        name = item.Name.Create(attribute.ElementName, attribute.Namespace);

                        if (propertyBuilder.Item == null)
                        {
                            propertyBuilder.SetItem(item.ValueType, item.Name);
                        }

                        if (attribute.Type == null || attribute.Type == item.ValueType)
                        {
                            propertyBuilder.Item.SetName(name);
                        }
                        else if (item.ValueType.IsAssignableFrom(attribute.Type))
                        {
                            propertyBuilder.Item.SetKnownType(attribute.Type, name);
                        }

                        propertyBuilder.SetCollection();
                    }
                }
            }
            else if (attributes.Attributes != null)
            {
                foreach (var attribute in attributes.Attributes)
                {
                    if (attribute.Type == null || attribute.Type == propertyType)
                    {
                        var name = propertyName.Create(attribute.AttributeName, attribute.Namespace);
                        propertyBuilder.SetName(name)
                            .SetMappingType(XmlMappingType.Attribute)
                            .SetDataType(attribute.DataType);
                        break;
                    }
                }
            }
            else if (attributes.Text != null)
            {
                propertyBuilder.SetMappingType(XmlMappingType.InnerText)
                    .SetDataType(attributes.Text.DataType);
            }
            else if (attributes.Array != null)
            {
                var name = propertyName.Create(attributes.Array.ElementName, attributes.Array.Namespace);
                propertyBuilder.SetName(name).SetOrder(attributes.Array.Order).SetNullable(attributes.Array.IsNullable).SetCollection(false);
            }

            if (attributes.Default != null)
            {
                propertyBuilder.SetDefaultValue(attributes.Default.Value);
            }

            if (attributes.ArrayItems != null && item != null && !propertyBuilder.IsCollection)
            {
                foreach (var attribute in attributes.ArrayItems)
                {
                    var name = item.Name.Create(attribute.ElementName, attribute.Namespace);

                    if (propertyBuilder.Item == null)
                    {
                        propertyBuilder.SetItem(item.ValueType, item.Name);
                    }

                    if (attribute.Type == null || attribute.Type == item.ValueType)
                    {
                        propertyBuilder.Item.SetName(name);
                    }
                    else if (item.ValueType.IsAssignableFrom(attribute.Type))
                    {
                        propertyBuilder.Item.SetKnownType(attribute.Type, name);
                    }

                    propertyBuilder.Item.SetDataType(attribute.DataType);
                }
            }

            return true;
        }
    }
}