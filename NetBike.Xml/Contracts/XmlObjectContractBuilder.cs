namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    public sealed class XmlObjectContractBuilder<T>
    {
        private readonly Type valueType;
        private readonly List<XmlProperty> properties;
        private XmlName xmlName;
        private XmlCollectionItem item;
        private XmlTypeNameHandling? typeNameHandling;

        internal XmlObjectContractBuilder(XmlName xmlName = null)
        {
            this.xmlName = xmlName;
            this.properties = new List<XmlProperty>();
            this.valueType = typeof(T);
        }

        public XmlObjectContractBuilder<T> Name(XmlName xmlName)
        {
            this.xmlName = xmlName;
            return this;
        }

        public XmlObjectContractBuilder<T> Item(XmlCollectionItem item)
        {
            this.item = item;
            return this;
        }

        public XmlObjectContractBuilder<T> Item(XmlName xmlName, XmlTypeNameHandling? typeNameHandling = null)
        {
            var enumerableType = valueType.GetEnumerableItemType();

            if (enumerableType == null)
            {
                throw new InvalidOperationException("Object contract is not enumerable.");
            }

            this.item = new XmlCollectionItem(enumerableType, xmlName, typeNameHandling);
            return this;
        }

        public XmlObjectContractBuilder<T> TypeHandling(XmlTypeNameHandling? typeNameHandling)
        {
            this.typeNameHandling = typeNameHandling;
            return this;
        }

        public XmlObjectContractBuilder<T> Property(XmlProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (valueType != property.PropertyInfo.ReflectedType)
            {
                throw new ArgumentException("Invalid property owner.");
            }

            this.properties.Add(property);
            return this;
        }

        public XmlObjectContractBuilder<T> Property(
            string propertyName,
            XmlName name = null)
        {
            var builder = XmlProperty.Builder<T>(propertyName);
            return this.Property(builder.SetName(name ?? builder.PropertyInfo.Name).Build());
        }

        public XmlObjectContractBuilder<T> Property<TProperty>(
            string propertyName,
            Action<XmlPropertyBuilder> construct)
        {
            var builder = XmlProperty.Builder<T>(propertyName);
            construct(builder);
            return this.Property(builder.Build());
        }

        public XmlObjectContractBuilder<T> Property<TProperty>(
            Expression<Func<T, TProperty>> expression,
            XmlName name = null)
        {
            var builder = XmlProperty.Builder(expression);
            return this.Property(builder.SetName(name ?? builder.PropertyInfo.Name).Build());
        }

        public XmlObjectContractBuilder<T> Property<TProperty>(
            Expression<Func<T, TProperty>> expression,
            Action<XmlPropertyBuilder> construct)
        {
            var builder = XmlPropertyBuilder.Create(expression);
            construct(builder);
            return this.Property(builder.Build());
        }

        public XmlObjectContract Build()
        {
            return new XmlObjectContract(
                this.valueType,
                this.xmlName ?? valueType.GetShortName(),
                this.properties,
                this.typeNameHandling,
                this.item);
        }
    }
}