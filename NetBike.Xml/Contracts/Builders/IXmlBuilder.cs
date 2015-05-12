namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public interface IXmlBuilder
    {
        Type ValueType { get; }

        XmlName Name { get; set; }
    }
}