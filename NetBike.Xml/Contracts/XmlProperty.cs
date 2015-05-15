namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    using NetBike.Xml.Contracts.Builders;
    using NetBike.Xml.Utilities;

    public class XmlProperty : XmlMember
    {
        private readonly bool isRequired;
        private readonly int order;
        private readonly PropertyInfo propertyInfo;
        private readonly bool hasGetterAndSetter;
        private readonly bool isCollection;
        private Action<object, object> setter;
        private Func<object, object> getter;

        public XmlProperty(
            PropertyInfo propertyInfo,
            XmlName name,
            XmlMappingType mappingType = XmlMappingType.Element,
            bool isRequired = false,
            XmlTypeHandling? typeHandling = null,
            XmlNullValueHandling? nullValueHandling = null,
            XmlDefaultValueHandling? defaultValueHandling = null,
            object defaultValue = null,
            XmlItem item = null,
            IEnumerable<XmlKnownType> knownTypes = null,
            bool isCollection = false,
            int order = -1)
            : base(propertyInfo.PropertyType, name, mappingType, typeHandling, nullValueHandling, defaultValueHandling, defaultValue, item, knownTypes)
        {
            if (isCollection)
            {
                if (!propertyInfo.PropertyType.IsEnumerable())
                {
                    throw new ArgumentException("Collection flag is available only for the IEnumerable type.");
                }

                this.isCollection = true;
            }

            this.propertyInfo = propertyInfo;
            this.isRequired = isRequired;
            this.order = order;
            this.hasGetterAndSetter = propertyInfo.CanRead && propertyInfo.CanWrite;
        }

        public PropertyInfo PropertyInfo
        {
            get { return this.propertyInfo; }
        }

        public string PropertyName
        {
            get { return this.propertyInfo.Name; }
        }

        public bool IsRequired
        {
            get { return this.isRequired; }
        }

        public bool IsCollection
        {
            get { return this.isCollection; }
        }

        public int Order
        {
            get { return this.order; }
        }

        internal bool HasGetterAndSetter
        {
            get { return this.hasGetterAndSetter; }
        }
        
        internal object GetValue(object target)
        {
            if (this.getter == null)
            {
                this.getter = DynamicWrapperFactory.CreateGetter(this.propertyInfo);
            }

            return this.getter(target);
        }

        internal void SetValue(object target, object value)
        {
            if (this.setter == null)
            {
                this.setter = DynamicWrapperFactory.CreateSetter(this.propertyInfo);
            }

            this.setter(target, value);
        }
    }
}