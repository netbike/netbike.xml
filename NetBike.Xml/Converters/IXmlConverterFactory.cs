namespace NetBike.Xml.Converters
{
    using NetBike.Xml.Contracts;

    public interface IXmlConverterFactory
    {
        IXmlConverter CreateConverter(XmlContract contract);
    }
}