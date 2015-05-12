namespace NetBike.Xml.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class XmlPropertyComparer : IComparer<XmlProperty>
    {
        internal static readonly IComparer<XmlProperty> Instance = new XmlPropertyComparer();

        public int Compare(XmlProperty x, XmlProperty y)
        {
            if (x.MappingType == XmlMappingType.Attribute)
            {
                if (y.MappingType != XmlMappingType.Attribute)
                {
                    return -1;
                }
                else
                {
                    return x.Order.CompareTo(y.Order);
                }
            }
            else
            {
                if (y.MappingType == XmlMappingType.Attribute)
                {
                    return 1;
                }
                else
                {
                    return x.Order.CompareTo(y.Order);
                }
            }
        }
    }
}