namespace NetBike.Xml.Contracts
{
    using System;
    using NetBike.Xml.Utilities;

    public class XmlContract
    {
        private readonly Type valueType;
        private readonly XmlName name;
        private XmlMember root;
        private Func<object> creator;

        public XmlContract(Type valueType, XmlName name)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            this.valueType = valueType;
            this.name = name;
        }

        public Type ValueType
        {
            get { return this.valueType; }
        }

        public XmlName Name
        {
            get { return this.name; }
        }

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
                this.creator = DynamicWrapperFactory.CreateConstructor(this.valueType);
            }

            return this.creator();
        }

        protected virtual XmlMember GetDefaultMember()
        {
            return new XmlMember(
                this.valueType,
                this.name,
                XmlMappingType.Element,
                XmlTypeNameHandling.None,
                XmlNullValueHandling.Include,
                XmlDefaultValueHandling.Include);
        }
    }
}