namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public interface IXmlCollectionBuilder : IXmlBuilder
    {
        XmlItemBuilder Item { get; set; }
    }
}