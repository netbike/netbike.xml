namespace NetBike.Xml.Converters.Objects
{
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters.Collections;

    internal struct XmlPropertyInfo
    {
        public XmlNameRef NameRef;
        public XmlProperty Property;
        public XmlNameRef[] KnownNameRefs;
        public ICollectionProxy CollectionProxy;
        public XmlMember Item;
        public bool Assigned;

        public static XmlPropertyInfo[] GetInfo(XmlObjectContract contract, XmlNameTable nameTable, XmlSerializationContext context)
        {
            var propertyInfos = new XmlPropertyInfo[contract.Properties.Count];

            for (var i = 0; i < propertyInfos.Length; i++)
            {
                var property = contract.Properties[i];
                propertyInfos[i].Property = property;

                if (!property.IsCollection)
                {
                    propertyInfos[i].NameRef.Reset(property.Name, nameTable);
                    propertyInfos[i].KnownNameRefs = XmlItemInfo.GetKnownNameRefs(property, nameTable);
                }
                else
                {
                    var typeContext = context.Settings.GetTypeContext(property.ValueType);

                    if (!(typeContext.ReadConverter is XmlCollectionConverter collectionConverter))
                    {
                        throw new XmlSerializationException(
                            $"Readable collection converter for the type \"{property.ValueType}\" is not found.");
                    }

                    var item = property.Item ?? typeContext.Contract.Root;

                    propertyInfos[i].CollectionProxy = collectionConverter.CreateProxy(property.ValueType);
                    propertyInfos[i].Item = item;
                    propertyInfos[i].NameRef.Reset(item.Name, nameTable);
                    propertyInfos[i].KnownNameRefs = XmlItemInfo.GetKnownNameRefs(item, nameTable);
                }
            }

            return propertyInfos;
        }

        public XmlMember Match(XmlMappingType mappingType, XmlReader reader)
        {
            if (mappingType == this.Property.MappingType)
            {
                if (this.NameRef.Match(reader))
                {
                    return this.Property;
                }

                if (this.KnownNameRefs != null)
                {
                    for (var i = 0; i < this.KnownNameRefs.Length; i++)
                    {
                        if (this.KnownNameRefs[i].Match(reader))
                        {
                            return (this.Item ?? this.Property).KnownTypes[i];
                        }
                    }
                }
            }

            return null;
        }
    }
}