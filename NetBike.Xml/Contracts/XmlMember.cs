namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class XmlMember
    {
        private static readonly List<XmlKnownType> EmptyKnownTypes = new List<XmlKnownType>();

        private readonly XmlName name;
        private readonly Type valueType;
        private readonly XmlItem item;
        private readonly XmlMappingType mappingType;
        private readonly XmlTypeNameHandling? typeNameHandling;
        private readonly XmlDefaultValueHandling? defaultValueHandling;
        private readonly XmlNullValueHandling? nullValueHandling;
        private readonly object defaultValue;
        private readonly bool isFinalType;
        private readonly List<XmlKnownType> knownTypes;

        internal XmlMember(
            Type valueType,
            XmlName name,
            XmlMappingType mappingType = XmlMappingType.Element,
            XmlTypeNameHandling? typeNameHandling = null,
            XmlNullValueHandling? nullValueHandling = null,
            XmlDefaultValueHandling? defaultValueHandling = null,
            object defaultValue = null,
            XmlItem item = null,
            IEnumerable<XmlKnownType> knownTypes = null)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.valueType = valueType;
            this.name = name;
            this.typeNameHandling = typeNameHandling;
            this.defaultValueHandling = defaultValueHandling;
            this.defaultValue = defaultValue;
            this.isFinalType = valueType.IsFinalType();
            this.item = item;
            this.nullValueHandling = nullValueHandling;
            this.mappingType = mappingType;

            if (knownTypes != null)
            {
                var count = knownTypes.Count();

                if (count > 0)
                {
                    if (mappingType != XmlMappingType.Element)
                    {
                        throw new ArgumentException("Known types may be set only for XML Element.", "knownTypes");
                    }

                    if (this.isFinalType)
                    {
                        throw new ArgumentException("Known types cannot be set for final value type.", "knownTypes");
                    }

                    this.knownTypes = new List<XmlKnownType>(count);

                    foreach (var knownType in knownTypes)
                    {
                        if (valueType == knownType.valueType)
                        {
                            throw new ArgumentException(string.Format("Known type \"{0}\" cannot be equal to the value type.", valueType), "knownTypes");
                        }

                        if (!valueType.IsAssignableFrom(knownType.ValueType))
                        {
                            throw new ArgumentException(string.Format("Known type \"{0}\" must be inherits from \"{1}\".", knownType.ValueType, valueType), "knownTypes");
                        }

                        this.knownTypes.Add(knownType);
                    }
                }
            }
        }

        public Type ValueType
        {
            get { return this.valueType; }
        }

        public XmlName Name
        {
            get { return this.name; }
        }

        public XmlMappingType MappingType
        {
            get { return this.mappingType; }
        }

        public XmlTypeNameHandling? TypeNameHandling
        {
            get { return this.typeNameHandling; }
        }

        public XmlNullValueHandling? NullValueHandling
        {
            get { return this.nullValueHandling; }
        }

        public XmlDefaultValueHandling? DefaultValueHandling
        {
            get { return this.defaultValueHandling; }
        }

        public object DefaultValue
        {
            get { return this.defaultValue; }
        }

        public XmlItem Item
        {
            get { return this.item; }
        }

        public bool IsFinalType
        {
            get { return this.isFinalType; }
        }

        public IReadOnlyList<XmlKnownType> KnownTypes
        {
            get { return this.knownTypes ?? EmptyKnownTypes; }
        }

        internal XmlMember ResolveMember(Type valueType)
        {
            if (this.knownTypes != null)
            {
                foreach (var item in this.knownTypes)
                {
                    if (item.valueType == valueType)
                    {
                        return item;
                    }
                }
            }

            return this;
        }
    }
}