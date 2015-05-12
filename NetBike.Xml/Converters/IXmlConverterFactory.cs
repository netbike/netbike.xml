namespace NetBike.Xml.Converters
{
    using System;
    using NetBike.Xml.Contracts;

    public interface IXmlConverterFactory
    {
        IXmlConverter CreateConverter(XmlContract contract);
    }
}