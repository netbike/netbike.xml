namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public static class XmlBuilderExtensions
    {
        public static TBuilder SetName<TBuilder>(this TBuilder builder, XmlName name) where TBuilder : IXmlBuilder
        {
            builder.Name = name;
            return builder;
        }

        public static TBuilder SetTypeHandling<TBuilder>(this TBuilder builder, XmlTypeHandling? typeHandling)
            where TBuilder : XmlMemberBuilder
        {
            builder.TypeHandling = typeHandling;
            return builder;
        }
        
        public static TBuilder SetDataType<TBuilder>(this TBuilder builder, string dataType)
            where TBuilder : XmlMemberBuilder
        {
            builder.DataType = dataType;
            return builder;
        }

        public static TBuilder SetKnownType<TBuilder>(this TBuilder builder, Type valueType, XmlName name = null)
            where TBuilder : XmlItemBuilder
        {
            return SetKnownType(builder, valueType, x => x.SetName(name));
        }

        public static TBuilder SetKnownType<TBuilder>(this TBuilder builder, Type valueType, Action<XmlKnownTypeBuilder> build)
            where TBuilder : XmlItemBuilder
        {
            if (builder.KnownTypes == null)
            {
                builder.KnownTypes = new XmlKnownTypeBuilderCollection();
            }

            var knownType = new XmlKnownTypeBuilder(valueType);
            build(knownType);

            builder.KnownTypes.Set(knownType);

            return builder;
        }

        public static XmlItemBuilder SetKnownType<T>(this XmlItemBuilder builder, XmlName name = null)
        {
            return SetKnownType(builder, typeof(T), name);
        }

        public static XmlItemBuilder SetKnownType<T>(this XmlItemBuilder builder, Action<XmlKnownTypeBuilder> build)
        {
            return SetKnownType(builder, typeof(T), build);
        }

        public static XmlPropertyBuilder SetKnownType<T>(this XmlPropertyBuilder builder, XmlName name = null)
        {
            return SetKnownType(builder, typeof(T), name);
        }

        public static XmlPropertyBuilder SetKnownType<T>(this XmlPropertyBuilder builder, Action<XmlKnownTypeBuilder> build)
        {
            return SetKnownType(builder, typeof(T), build);
        }

        public static TBuilder RemoveKnownType<TBuilder>(this TBuilder builder, Type valueType)
            where TBuilder : XmlItemBuilder
        {
            if (builder.KnownTypes != null)
            {
                builder.KnownTypes.Remove(valueType);
            }

            return builder;
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, XmlName name)
            where TBuilder : IXmlCollectionBuilder
        {
            var valueType = builder.ValueType.GetEnumerableItemType();
            return SetItem(builder, valueType, x => x.SetName(name));
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, Type valueType, XmlName name)
            where TBuilder : IXmlCollectionBuilder
        {
            return SetItem(builder, valueType, x => x.SetName(name));
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, Action<XmlItemBuilder> build)
            where TBuilder : IXmlCollectionBuilder
        {
            var valueType = builder.ValueType.GetEnumerableItemType();
            return SetItem(builder, valueType, build);
        }

        public static TBuilder SetItem<TBuilder>(this TBuilder builder, Type valueType, Action<XmlItemBuilder> build)
            where TBuilder : IXmlCollectionBuilder
        {
            var item = new XmlItemBuilder(valueType);
            build(item);
            builder.Item = item;
            return builder;
        }

        public static T SetNullable<T>(this T builder, bool isNullable)
            where T : IXmlObjectBuilder
        {
            if (isNullable)
            {
                builder.NullValueHandling = XmlNullValueHandling.Include;
            }
            else
            {
                builder.NullValueHandling = null;
            }

            return builder;
        }

        public static T SetNullValueHandling<T>(this T builder, XmlNullValueHandling? nullValueHandling)
            where T : IXmlObjectBuilder
        {
            builder.NullValueHandling = nullValueHandling;
            return builder;
        }

        public static TBuilder SetDefaultValue<TBuilder>(this TBuilder builder, object defaultValue)
            where TBuilder : IXmlObjectBuilder
        {
            builder.DefaultValue = defaultValue;
            return builder;
        }

        public static TBuilder SetDefaultValueHandling<TBuilder>(this TBuilder builder, XmlDefaultValueHandling? defaultValueHandling)
            where TBuilder : IXmlObjectBuilder
        {
            builder.DefaultValueHandling = defaultValueHandling;
            return builder;
        }

        public static XmlPropertyBuilder SetMappingType(this XmlPropertyBuilder builder, XmlMappingType mappingType)
        {
            builder.MappingType = mappingType;
            return builder;
        }

        public static XmlPropertyBuilder SetOrder(this XmlPropertyBuilder builder, int order)
        {
            builder.Order = order;
            return builder;
        }

        public static XmlPropertyBuilder SetRequired(this XmlPropertyBuilder builder, bool isRequired = true)
        {
            builder.IsRequired = isRequired;
            return builder;
        }

        public static XmlPropertyBuilder SetCollection(this XmlPropertyBuilder builder, bool isCollection = true)
        {
            builder.IsCollection = isCollection;
            return builder;
        }
    }
}