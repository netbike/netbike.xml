namespace NetBike.Xml
{
    using System;

    public class XmlSerializationException : Exception
    {
        public XmlSerializationException(string message)
            : base(message)
        {
        }

        public XmlSerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}