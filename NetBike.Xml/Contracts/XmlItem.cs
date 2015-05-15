namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlItem : XmlMember
    {
        public XmlItem(Type valueType, XmlName name, XmlTypeHandling? typeHandling = null, IEnumerable<XmlKnownType> knownTypes = null)
            : base(valueType, name, typeHandling: typeHandling, knownTypes: knownTypes)
        {
        }
    }
}