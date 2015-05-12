namespace NetBike.Xml.TypeResolvers
{
    using System;

    public interface IXmlTypeResolver
    {
        string GetTypeName(Type valueType);

        Type ResolveTypeName(Type rootType, string typeName);
    }
}