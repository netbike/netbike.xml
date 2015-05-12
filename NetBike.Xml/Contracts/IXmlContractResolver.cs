namespace NetBike.Xml.Contracts
{
    using System;

    public interface IXmlContractResolver
    {
        XmlContract ResolveContract(Type valueType);
    }
}