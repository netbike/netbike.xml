namespace NetBike.Xml.Contracts
{
    using System;
    using NetBike.Xml.Utilities;

    public class XmlContract
    {
        private XmlMember root;
        private Func<object> creator;

        public XmlContract(Type valueType, XmlName name)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.ValueType = valueType;
            this.Name = name;
        }

        public Type ValueType { get; }

        public XmlName Name { get; }

        internal XmlMember Root
        {
            get
            {
                if (this.root == null)
                {
                    this.root = this.GetDefaultMember();
                }

                return this.root;
            }
        }

        internal object CreateDefault()
        {
            if (this.creator == null)
            {
                this.creator = DynamicWrapperFactory.CreateConstructor(this.ValueType);
            }

            return this.creator();
        }

        protected virtual XmlMember GetDefaultMember()
        {
            return new XmlMember(
                this.ValueType,
                this.Name,
                XmlMappingType.Element,
                XmlTypeHandling.None,
                XmlNullValueHandling.Include,
                XmlDefaultValueHandling.Include);
        }
    }
}