namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlItem : XmlMember
    {
        public XmlItem(Type valueType, XmlName name, XmlTypeNameHandling? typeNameHandling = null, IEnumerable<XmlKnownType> knownTypes = null)
            : base(valueType, name, typeNameHandling: typeNameHandling, knownTypes: knownTypes)
        {
        }
    }
}