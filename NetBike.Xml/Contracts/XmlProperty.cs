namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using NetBike.Xml.Utilities;

    public class XmlProperty : XmlMember
    {
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
            int order = -1,
            string dataType = null)
            : base(propertyInfo.PropertyType, name, mappingType, typeHandling, nullValueHandling, defaultValueHandling, defaultValue, item, knownTypes, dataType)
        {
            if (isCollection)
            {
                if (!propertyInfo.PropertyType.IsEnumerable())
                {
                    throw new ArgumentException("Collection flag is available only for the IEnumerable type.");
                }

                this.IsCollection = true;
            }

            this.PropertyInfo = propertyInfo;
            this.IsRequired = isRequired;
            this.Order = order;
            this.HasGetterAndSetter = propertyInfo.CanRead && propertyInfo.CanWrite;
        }

        public PropertyInfo PropertyInfo { get; }

        public string PropertyName => this.PropertyInfo.Name;

        public bool IsRequired { get; }

        public bool IsCollection { get; }

        public int Order { get; }

        internal bool HasGetterAndSetter { get; }

        internal object GetValue(object target)
        {
            if (this.getter == null)
            {
                this.getter = DynamicWrapperFactory.CreateGetter(this.PropertyInfo);
            }

            return this.getter(target);
        }

        internal void SetValue(object target, object value)
        {
            if (this.setter == null)
            {
                this.setter = DynamicWrapperFactory.CreateSetter(this.PropertyInfo);
            }

            this.setter(target, value);
        }
    }
}