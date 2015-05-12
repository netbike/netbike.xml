namespace NetBike.Xml.Contracts
{
    using System;
    using System.Reflection;

    internal static class XmlContractExtensions
    {
        public static XmlObjectContract ToObjectContract(this XmlContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            var objectContract = contract as XmlObjectContract;

            if (objectContract == null)
            {
                throw new XmlSerializationException(string.Format("Contract \"{0}\" must be object contract.", contract.ValueType));
            }

            return objectContract;
        }

        public static XmlEnumContract ToEnumContract(this XmlContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException("contract");
            }

            var enumContract = contract as XmlEnumContract;

            if (enumContract == null)
            {
                throw new XmlSerializationException(string.Format("Contract \"{0}\" must be enum contract.", contract.ValueType));
            }

            return enumContract;
        }
    }
}