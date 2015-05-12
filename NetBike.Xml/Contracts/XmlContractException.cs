namespace NetBike.Xml.Contracts
{
    using System;

    public class XmlContractException : XmlSerializationException
    {
        public XmlContractException(string message)
            : base(message)
        {
        }

        public XmlContractException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}