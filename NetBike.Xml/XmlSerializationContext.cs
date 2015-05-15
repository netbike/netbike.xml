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
        
        internal bool ReadValueType(XmlReader reader, ref Type valueType)
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

        internal bool TryResolveValueType(object value, ref XmlMember member, out Type valueType)
        {
            if (member.IsOpenType)
            {
                var typeHandling = member.TypeHandling ?? this.settings.TypeHandling;

                if (typeHandling != XmlTypeHandling.None)
                {
                    valueType = value.GetType();
                    member = member.ResolveMember(valueType);
                    return typeHandling == XmlTypeHandling.Always || valueType != member.ValueType;
                }
            }

            valueType = member.ValueType;

            return false;
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

            Type valueType;
            XmlTypeContext context = null;

            if (member == null)
            {
                context = this.settings.GetTypeContext(memberType);
                member = context.Contract.Root;
            }

            var shouldWriteTypeName = this.TryResolveValueType(value, ref member, out valueType);

            if (member.DefaultValue != null)
            {
                var defaultValueHandling = member.DefaultValueHandling ?? this.settings.DefaultValueHandling;

                if (defaultValueHandling == XmlDefaultValueHandling.Ignore && value.Equals(member.DefaultValue))
                {
                    return;
                }
            }

            if (context == null || context.Contract.ValueType != member.ValueType)
            {
                context = this.settings.GetTypeContext(valueType);
            }

            switch (member.MappingType)
            {
                case XmlMappingType.Element:
                    writer.WriteStartElement(member.Name);

                    if (this.initialState)
                    {
                        this.initialState = false;
                        this.WriteNamespaces(writer);
                    }

                    if (shouldWriteTypeName)
                    {
                        this.WriteTypeName(writer, valueType);
                    }

                    this.WriteXml(writer, value, member, context);
                    writer.WriteEndElement();
                    break;

                case XmlMappingType.Attribute:
                    writer.WriteStartAttribute(member.Name);
                    this.WriteXml(writer, value, member, context);
                    writer.WriteEndAttribute();
                    break;

                case XmlMappingType.InnerText:
                    this.WriteXml(writer, value, member, context);
                    break;
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
                if (!this.ReadValueType(reader, ref valueType))
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