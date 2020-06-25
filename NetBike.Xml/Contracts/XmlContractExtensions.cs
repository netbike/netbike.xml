namespace NetBike.Xml.Contracts
{
    using System;

    internal static class XmlContractExtensions
    {
        public static XmlObjectContract ToObjectContract(this XmlContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var objectContract = contract as XmlObjectContract;

            if (objectContract == null)
            {
                throw new XmlSerializationException($"Contract \"{contract.ValueType}\" must be object contract.");
            }

            return objectContract;
        }

        public static XmlEnumContract ToEnumContract(this XmlContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var enumContract = contract as XmlEnumContract;

            if (enumContract == null)
            {
                throw new XmlSerializationException($"Contract \"{contract.ValueType}\" must be enum contract.");
            }

            return enumContract;
        }
    }
}