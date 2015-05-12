namespace NetBike.Xml.Contracts.Builders
{
    public class XmlObjectContractBuilder<T> : XmlObjectContractBuilder
    {
        public XmlObjectContractBuilder()
            : base(typeof(T))
        {
        }
    }
}