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

            if (!(contract is XmlObjectContract objectContract))
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

            if (!(contract is XmlEnumContract enumContract))
            {
                throw new XmlSerializationException($"Contract \"{contract.ValueType}\" must be enum contract.");
            }

            return enumContract;
        }
    }
}