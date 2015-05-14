namespace NetBike.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;

    public sealed partial class XmlSerializationContext
    {
        private readonly XmlSerializationSettings settings;
        private XmlContract currentContract;
        private XmlMember currentMember;
        private bool initialState;
        private Dictionary<string, object> properties;
        private XmlNameRef typeNameRef;
        private XmlNameRef nullNameRef;
        private XmlReader lastUsedReader;

        public XmlSerializationContext(XmlSerializationSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
            this.initialState = true;
        }

        internal XmlSerializationContext(XmlSerializationSettings settings, XmlMember member, XmlContract contract)
            : this(settings)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            this.currentContract = contract;
            this.currentMember = member;
            this.initialState = false;
        }

        public Type ValueType
        {
            get { return this.currentContract.ValueType; }
        }

        public XmlContract Contract
        {
            get { return this.currentContract; }
        }

        public XmlMember Member
        {
            get { return this.currentMember; }
        }

        public IDictionary<string, object> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new Dictionary<string, object>();
                }

                return this.properties;
            }
        }

        public XmlSerializationSettings Settings
        {
            get { return this.settings; }
        }

        public XmlContract GetTypeContract(Type valueType)
        {
            return this.settings.GetTypeContext(valueType).Contract;
        }

        public void Serialize(XmlWriter writer, object value, Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            this.Serialize(writer, value, valueType, null);
        }

        public void Serialize(XmlWriter writer, object value, XmlMember member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            this.Serialize(writer, value, member.ValueType, member);
        }

        public object Deserialize(XmlReader reader, Type valueType)
        {
            return this.Deserialize(reader, valueType, null);
        }

        public object Deserialize(XmlReader reader, XmlMember member)
        {
            return this.Deserialize(reader, member.ValueType, member);
        }

        public void SerializeBody(XmlWriter writer, object value, Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            this.SerializeBody(writer, value, valueType, null);
        }

        public void SerializeBody(XmlWriter writer, object value, XmlMember member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            this.SerializeBody(writer, value, member.ValueType, member);
        }

        internal bool ShouldWriteValue(object value, XmlMember member)
        {
            if (member.DefaultValue != null)
            {
                var defaultValueHandling = member.DefaultValueHandling ?? this.settings.DefaultValueHandling;

                if (defaultValueHandling == XmlDefaultValueHandling.Ignore && value.Equals(member.DefaultValue))
                {
                    return false;
                }
            }

            return true;
        }

        internal bool ShouldWriteTypeName(Type valueType, Type memberType, XmlMember member)
        {
            if (!member.IsClosedType)
            {
                var typeNameHandling = member.TypeNameHandling ?? this.settings.TypeNameHandling;

                if (typeNameHandling != XmlTypeNameHandling.None &&
                   (typeNameHandling == XmlTypeNameHandling.Always || valueType != memberType))
                {
                    return true;
                }
            }

            return false;
        }

        internal void WriteTypeName(XmlWriter writer, Type valueType)
        {
            var typeName = this.settings.TypeResolver.GetTypeName(valueType);
            writer.WriteAttributeString(this.settings.TypeAttribute, typeName);
        }

        internal void WriteNull(XmlWriter writer, Type valueType, XmlMember member)
        {
            var nullValueHandling = this.settings.NullValueHandling;

            if (member != null)
            {
                if (member.MappingType == XmlMappingType.Attribute)
                {
                    return;
                }

                nullValueHandling = member.NullValueHandling ?? nullValueHandling;
            }

            if (nullValueHandling != XmlNullValueHandling.Ignore)
            {
                if (member == null)
                {
                    member = this.settings.GetTypeContext(valueType).Contract.Root;
                }

                writer.WriteStartElement(member.Name);

                if (this.initialState)
                {
                    this.initialState = false;
                    this.WriteNamespaces(writer);
                }

                writer.WriteAttributeString(this.settings.NullAttribute, "true");
                writer.WriteEndElement();
            }
        }
        
        internal bool ReadSerializationType(XmlReader reader, ref Type valueType)
        {
            if (reader.AttributeCount > 0)
            {
                if (!object.ReferenceEquals(this.lastUsedReader, reader))
                {
                    this.typeNameRef.Reset(this.settings.TypeAttribute, reader.NameTable);
                    this.nullNameRef.Reset(this.settings.NullAttribute, reader.NameTable);
                    this.lastUsedReader = reader;
                }

                if (reader.MoveToFirstAttribute())
                {
                    do
                    {
                        if (this.nullNameRef.Match(reader))
                        {
                            return false;
                        }
                        else if (this.typeNameRef.Match(reader))
                        {
                            valueType = this.settings.TypeResolver.ResolveTypeName(valueType, reader.Value);
                        }
                    }
                    while (reader.MoveToNextAttribute());

                    reader.MoveToElement();
                }
            }

            return true;
        }

        internal void WriteXml(XmlWriter writer, object value, XmlMember member, XmlTypeContext typeContext)
        {
            var lastMember = this.currentMember;
            var lastContract = this.currentContract;

            this.currentMember = member;
            this.currentContract = typeContext.Contract;

            typeContext.WriteXml(writer, value, this);

            this.currentMember = lastMember;
            this.currentContract = lastContract;
        }

        internal object ReadXml(XmlReader reader, XmlMember member, XmlTypeContext typeContext)
        {
            var lastMember = this.currentMember;
            var lastContract = this.currentContract;

            this.currentMember = member;
            this.currentContract = typeContext.Contract;

            var value = typeContext.ReadXml(reader, this);

            this.currentMember = lastMember;
            this.currentContract = lastContract;

            return value;
        }

        private void SerializeBody(XmlWriter writer, object value, Type memberType, XmlMember member)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (value == null)
            {
                this.WriteNull(writer, memberType, member);
            }
            else
            {
                var typeContext = this.settings.GetTypeContext(memberType);
                this.WriteXml(writer, value, member ?? typeContext.Contract.Root, typeContext);
            }
        }

        private void Serialize(XmlWriter writer, object value, Type memberType, XmlMember member)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (value == null)
            {
                this.WriteNull(writer, memberType, member);
                return;
            }

            var valueType = value.GetType();
            var typeContext = this.settings.GetTypeContext(valueType);

            if (member == null)
            {
                member = typeContext.Contract.Root;
            }
            else
            {
                member = member.ResolveMember(valueType);
                memberType = valueType;
            }

            if (!this.ShouldWriteValue(value, member))
            {
                return;
            }

            var mappingType = member.MappingType;

            if (mappingType == XmlMappingType.Element)
            {
                writer.WriteStartElement(member.Name);

                if (this.initialState)
                {
                    this.initialState = false;
                    this.WriteNamespaces(writer);
                }

                if (this.ShouldWriteTypeName(valueType, memberType, member))
                {
                    this.WriteTypeName(writer, valueType);
                }

                this.WriteXml(writer, value, member, typeContext);

                writer.WriteEndElement();
            }
            else if (mappingType == XmlMappingType.Attribute)
            {
                writer.WriteStartAttribute(member.Name);
                this.WriteXml(writer, value, member, typeContext);
                writer.WriteEndAttribute();
            }
            else
            {
                this.WriteXml(writer, value, member, typeContext);
            }
        }

        private object Deserialize(XmlReader reader, Type valueType, XmlMember member)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            if (this.initialState && reader.NodeType == XmlNodeType.None)
            {
                this.initialState = false;

                while (reader.NodeType != XmlNodeType.Element)
                {
                    if (!reader.Read())
                    {
                        return null;
                    }
                }
            }

            if (reader.NodeType == XmlNodeType.Element)
            {
                if (!this.ReadSerializationType(reader, ref valueType))
                {
                    reader.Skip();
                    return null;
                }
            }

            var typeInfo = this.settings.GetTypeContext(valueType);

            if (member == null)
            {
                member = typeInfo.Contract.Root;
            }

            return this.ReadXml(reader, member, typeInfo);
        }

        private void WriteNamespaces(XmlWriter writer)
        {
            foreach (var item in this.settings.Namespaces)
            {
                writer.WriteNamespace(item);
            }
        }
    }
}