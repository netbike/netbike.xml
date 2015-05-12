namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public sealed class XmlEnumContractBuilder<T> : XmlEnumContractBuilder
        where T : struct, IConvertible
    {
        public XmlEnumContractBuilder()
            : base(typeof(T))
        {
        }
    }
}