namespace NetBike.Xml.Contracts.Builders
{
    using System;

    public class XmlEnumContractBuilder : XmlContractBuilder
    {
        private static readonly XmlEnumItemCollection EmptyItems = new XmlEnumItemCollection();

        public XmlEnumContractBuilder(Type valueType)
            : base(valueType)
        {
            if (!valueType.IsEnum)
            {
                throw new ArgumentException("Contract type must be Enum.", nameof(valueType));
            }
        }

        public XmlEnumItemCollection Items { get; set; }

        public static XmlEnumContractBuilder Create(XmlEnumContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            return new XmlEnumContractBuilder(contract.ValueType)
            {
                Name = contract.Name,
                Items = new XmlEnumItemCollection(contract.Items)
            };
        }

        public XmlEnumContract Build()
        {
            return new XmlEnumContract(
                this.ValueType,
                this.Name,
                this.Items ?? EmptyItems);
        }

        public override XmlContract BuildContract()
        {
            return this.Build();
        }
    }
}