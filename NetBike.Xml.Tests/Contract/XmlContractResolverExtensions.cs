namespace NetBike.Xml.Tests.Contract
{
    using NetBike.Xml.Contracts;

    public static class XmlContractResolverExtensions
    {
        public static XmlContract ResolveContract<TValue>(this IXmlContractResolver contractResolver)
        {
            return contractResolver.ResolveContract(typeof(TValue));
        }
    }
}