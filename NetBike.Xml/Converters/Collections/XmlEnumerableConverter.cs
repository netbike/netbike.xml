namespace NetBike.Xml.Converters.Collections
{
    using System;

    internal sealed class XmlEnumerableConverter : XmlCollectionConverter
    {
        public override bool CanRead(Type valueType)
        {
            return false;
        }

        public override ICollectionProxy CreateProxy(Type valueType)
        {
            throw new XmlSerializationException("Can't deserialize anonymous enumerable type.");
        }
    }
}