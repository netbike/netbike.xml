namespace NetBike.Xml.Converters.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using NetBike.Xml.Contracts;

    public abstract class XmlCollectionConverter : IXmlConverter
    {
        public abstract bool CanRead(Type valueType);

        public virtual bool CanWrite(Type valueType)
        {
            return valueType.IsEnumerable();
        }

        public abstract ICollectionProxy CreateProxy(Type valueType);

        public void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            if (context.Member.MappingType != XmlMappingType.Element)
            {
                throw new XmlSerializationException(string.Format("XML mapping of \"{0}\" must be Element.", context.ValueType));
            }

            var collectionItem = GetCollectionItem(context);

            if (collectionItem == null)
            {
                throw new XmlSerializationException(string.Format("XML contract of \"{0}\" must contains collection item", context.ValueType));
            }

            Type lastItemType = null;
            XmlTypeContext typeContext = null;

            foreach (var item in (IEnumerable)value)
            {
                if (item == null)
                {
                    context.WriteNull(writer, collectionItem.ValueType, collectionItem);
                }
                else
                {
                    Type itemType;
                    var member = (XmlMember)collectionItem;
                    var shouldWriteTypeName = context.TryResolveValueType(item, ref member, out itemType);

                    if (itemType != lastItemType)
                    {
                        typeContext = context.Settings.GetTypeContext(itemType);
                        lastItemType = itemType;
                    }

                    writer.WriteStartElement(member.Name);

                    if (shouldWriteTypeName)
                    {
                        context.WriteTypeName(writer, lastItemType);
                    }

                    context.WriteXml(writer, item, member, typeContext);

                    writer.WriteEndElement();
                }
            }
        }

        public object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            if (context.Member.MappingType != XmlMappingType.Element)
            {
                throw new XmlSerializationException(string.Format("XML mapping of \"{0}\" must be Element.", context.ValueType));
            }

            var item = GetCollectionItem(context);

            if (item == null)
            {
                throw new XmlSerializationException(string.Format("XML contract of \"{0}\" must contains item info", context.ValueType));
            }

            var collectionProxy = this.CreateProxy(context.ValueType);

            if (!reader.IsEmptyElement)
            {
                Type lastItemType = null;
                XmlTypeContext typeContext = null;
                var itemInfo = new XmlItemInfo(item, reader.NameTable);

                reader.ReadStartElement();

                var nodeType = reader.NodeType;

                while (nodeType != XmlNodeType.EndElement)
                {
                    if (nodeType == XmlNodeType.Element)
                    {
                        var member = itemInfo.Match(reader);

                        if (member != null)
                        {
                            var valueType = member.ValueType;

                            if (context.ReadValueType(reader, ref valueType))
                            {
                                if (lastItemType != valueType)
                                {
                                    typeContext = context.Settings.GetTypeContext(valueType);
                                    lastItemType = valueType;
                                }

                                var value = context.ReadXml(reader, member, typeContext);
                                collectionProxy.Add(value);
                            }
                            else
                            {
                                reader.Skip();
                            }
                        }
                        else
                        {
                            reader.Skip();
                        }
                    }
                    else
                    {
                        reader.Read();
                    }

                    nodeType = reader.NodeType;
                }
            }

            reader.Read();

            return collectionProxy.GetResult();
        }

        private static XmlItem GetCollectionItem(XmlSerializationContext context)
        {
            return context.Member.Item ?? context.Contract.ToObjectContract().Item;
        }
    }
}