namespace NetBike.Xml.Contracts.Builders
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public static class XmlContractBuilderExtensions
    {
        public static TBuilder SetTypeHandling<TBuilder>(this TBuilder builder, XmlTypeHandling? typeHandling)
            where TBuilder : XmlObjectContractBuilder
        {
            builder.TypeHandling = typeHandling;
            return builder;
        }

        public static TBuilder SetProperty<TBuilder>(
            this TBuilder builder, 
            string propertyName, 
            XmlName name = null, 
            XmlMappingType mappingType = XmlMappingType.Element)
            where TBuilder : XmlObjectContractBuilder
        {
            return SetProperty(builder, propertyName, x => x.SetName(name).SetMappingType(mappingType));
        }

        public static TBuilder SetProperty<TBuilder>(
            this TBuilder builder, 
            string propertyName, 
            Action<XmlPropertyBuilder> build)
            where TBuilder : XmlObjectContractBuilder
        {
            if (build == null)
            {
                throw new ArgumentNullException(nameof(build));
            }

            var property = XmlPropertyBuilder.Create(builder.ValueType, propertyName);

            build(property);

            if (builder.Properties == null)
            {
                builder.Properties = new XmlPropertyBuilderCollection();
            }

            builder.Properties.Set(property);

            return builder;
        }

        public static TBuilder RemoveProperty<TBuilder>(this TBuilder builder, string propertyName)
            where TBuilder : XmlObjectContractBuilder
        {
            if (builder.Properties != null)
            {
                var propertyInfo = XmlPropertyBuilder.GetPropertyInfo(builder.ValueType, propertyName);
                builder.Properties.Remove(propertyInfo);
            }

            return builder;
        }

        public static XmlObjectContractBuilder<T> SetProperty<T, TProperty>(
            this XmlObjectContractBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            XmlName name = null,
            XmlMappingType mappingType = XmlMappingType.Element)
        {
            return SetProperty(builder, expression, x => x.SetName(name).SetMappingType(mappingType));
        }

        public static XmlObjectContractBuilder<T> SetProperty<T, TProperty>(
            this XmlObjectContractBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            Action<XmlPropertyBuilder> build)
        {
            if (build == null)
            {
                throw new ArgumentNullException(nameof(build));
            }

            var property = XmlPropertyBuilder.Create<T, TProperty>(expression);

            build(property);

            if (builder.Properties == null)
            {
                builder.Properties = new XmlPropertyBuilderCollection();
            }

            builder.Properties.Set(property);

            return builder;
        }

        public static XmlObjectContractBuilder<T> RemoveProperty<T, TProperty>(
            this XmlObjectContractBuilder<T> builder,
            Expression<Func<T, TProperty>> expression)
        {
            if (builder.Properties != null)
            {
                var propertyInfo = XmlPropertyBuilder.GetPropertyInfo(expression);
                builder.Properties.Remove(propertyInfo);
            }

            return builder;
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, long value, string name)
            where TBuilder : XmlEnumContractBuilder
        {
            return SetItem(builder, new XmlEnumItem(value, name));
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, XmlEnumItem item)
            where TBuilder : XmlEnumContractBuilder
        {
            if (builder.Items == null)
            {
                builder.Items = new XmlEnumItemCollection();
            }

            builder.Items.Set(item);
            return builder;
        }

        public static TBuilder RemoveItem<TBuilder>(this TBuilder builder, long value)
            where TBuilder : XmlEnumContractBuilder
        {
            if (builder.Items != null)
            {
                builder.Items.Remove(value);
            }

            return builder;
        }

        public static XmlEnumContractBuilder<T> SetItem<T>(this XmlEnumContractBuilder<T> builder, T value, string name)
            where T : struct, IConvertible
        {
            var item = new XmlEnumItem(value.ToInt64(CultureInfo.InvariantCulture), name);
            return SetItem(builder, item);
        }

        public static XmlEnumContractBuilder<T> RemoveItem<T>(this XmlEnumContractBuilder<T> builder, T value)
            where T : struct, IConvertible
        {
            var longValue = value.ToInt64(CultureInfo.InvariantCulture);
            return RemoveItem(builder, longValue);
        }
    }
}