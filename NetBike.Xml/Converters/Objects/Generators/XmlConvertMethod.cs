namespace NetBike.Xml.Converters.Objects.Generators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    internal sealed class XmlConvertMethod
    {
        private readonly Type valueType;
        private readonly LambdaExpression parseExpression;
        private readonly LambdaExpression toStringExpression;

        public XmlConvertMethod(Type valueType, LambdaExpression parseExpression, LambdaExpression toStringExpression)
        {
            this.valueType = valueType;
            this.parseExpression = parseExpression;
            this.toStringExpression = toStringExpression;
        }

        public static IEnumerable<XmlConvertMethod> GetMethods(XmlSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            throw new NotImplementedException();
        }
    }
}