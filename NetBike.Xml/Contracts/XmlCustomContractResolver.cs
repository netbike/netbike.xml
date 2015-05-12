namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlCustomContractResolver : IXmlContractResolver
    {
        private readonly IXmlContractResolver fallbackResolver;
        private readonly Dictionary<Type, XmlContract> contracts;

        public XmlCustomContractResolver(IEnumerable<XmlContract> contracts)
            : this(contracts, new XmlContractResolver())
        {
        }

        public XmlCustomContractResolver(IEnumerable<XmlContract> contracts, IXmlContractResolver fallbackResolver)
        {
            if (contracts == null)
            {
                throw new ArgumentNullException("contracts");
            }

            this.fallbackResolver = fallbackResolver;
            this.contracts = new Dictionary<Type, XmlContract>();

            foreach (var contract in contracts)
            {
                this.contracts.Add(contract.ValueType, contract);
            }
        }

        public IEnumerable<XmlContract> Contracts
        {
            get { return this.contracts.Values; }
        }

        public XmlContract ResolveContract(Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            XmlContract contract;

            if (!this.contracts.TryGetValue(valueType, out contract))
            {
                if (this.fallbackResolver == null)
                {
                    throw new XmlSerializationException(string.Format("Can't resolve contract for \"{0}\".", valueType));
                }

                contract = this.fallbackResolver.ResolveContract(valueType);
            }

            return contract;
        }
    }
}