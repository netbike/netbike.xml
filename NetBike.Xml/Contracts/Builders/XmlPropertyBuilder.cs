namespace NetBike.Xml.Contracts.Builders
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed class XmlPropertyBuilder : XmlItemBuilder, IXmlObjectBuilder, IXmlCollectionBuilder
    {
        public XmlPropertyBuilder(PropertyInfo propertyInfo)
            : base(propertyInfo.PropertyType)
        {
            this.PropertyInfo = propertyInfo;
            this.MappingType = XmlMappingType.Element;
            this.Order = -1;
        }

        public PropertyInfo PropertyInfo { get; }

        public XmlMappingType MappingType { get; set; }

        public XmlNullValueHandling? NullValueHandling { get; set; }

        public XmlDefaultValueHandling? DefaultValueHandling { get; set; }

        public object DefaultValue { get; set; }

        public int Order { get; set; }

        public XmlItemBuilder Item { get; set; }

        public bool IsCollection { get; set; }

        public bool IsRequired { get; set; }

        public static XmlPropertyBuilder Create(Type ownerType, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(ownerType, propertyName);
            return new XmlPropertyBuilder(propertyInfo);
        }

        public static XmlPropertyBuilder Create<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> expression)
        {
            var propertyInfo = GetPropertyInfo(expression);
            return new XmlPropertyBuilder(propertyInfo);
        }

        public static XmlPropertyBuilder Create(XmlProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            return new XmlPropertyBuilder(property.PropertyInfo)
            {
                Name = property.Name,
                MappingType = property.MappingType,
                NullValueHandling = property.NullValueHandling,
                TypeHandling = property.TypeHandling,
                DefaultValueHandling = property.DefaultValueHandling,
                DefaultValue = property.DefaultValue,
                IsRequired = property.IsRequired,
                IsCollection = property.IsCollection,
                Order = property.Order,
                DataType = property.DataType,
                Item = property.Item != null ? XmlItemBuilder.Create(property.Item) : null,
                KnownTypes = property.KnownTypes != null ? XmlKnownTypeBuilderCollection.Create(property.KnownTypes) : null
            };
        }

        public new XmlProperty Build()
        {
            return new XmlProperty(
                this.PropertyInfo,
                this.Name ?? this.PropertyInfo.Name,
                this.MappingType,
                this.IsRequired,
                this.TypeHandling,
                this.NullValueHandling,
                this.DefaultValueHandling,
                this.DefaultValue,
                this.Item?.Build(),
                this.KnownTypes?.Build(),
                this.IsCollection,
                this.Order,
                this.DataType);
        }

        internal static PropertyInfo GetPropertyInfo(Type ownerType, string propertyName)
        {
            if (ownerType == null)
            {
                throw new ArgumentNullException(nameof(ownerType));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var propertyInfo = ownerType.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"Property \"{propertyName}\" is not declared in the type \"{ownerType}\".", nameof(propertyName));
            }

            return propertyInfo;
        }

        internal static PropertyInfo GetPropertyInfo<TOwner, TProperty>(Expression<Func<TOwner, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException("Expected property expression.");
            }

            var propertyInfo = memberExpression.Member as PropertyInfo;

            if (propertyInfo == null)
            {
                throw new ArgumentException("Expected property expression.");
            }

            var ownerType = typeof(TOwner);

            if (propertyInfo.DeclaringType == ownerType)
            {
                return propertyInfo;
            }

            return ownerType.GetProperty(propertyInfo.Name);
        }
    }
}