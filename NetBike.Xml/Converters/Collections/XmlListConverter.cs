namespace NetBike.Xml.Converters.Collections
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlListConverter : XmlConverterFactory
    {
        protected override bool AcceptType(Type valueType)
        {
            return valueType.IsGenericTypeOf(typeof(List<>), typeof(IList<>));
        }

        protected override Type GetConverterType(Type valueType)
        {
            return typeof(XmlTypedListConverter<>).MakeGenericType(valueType.GetGenericArguments());
        }

        private sealed class XmlTypedListConverter<TItem> : XmlCollectionConverter
        {
            public override bool CanRead(Type valueType)
            {
                return valueType == typeof(List<TItem>) || valueType == typeof(IList<TItem>);
            }

            public override ICollectionProxy CreateProxy(Type valueType)
            {
                return new ListProxy();
            }

            private sealed class ListProxy : ICollectionProxy
            {
                private readonly List<TItem> items = new List<TItem>();

                public void Add(object value)
                {
                    this.items.Add((TItem)value);
                }

                public object GetResult()
                {
                    return this.items;
                }
            }
        }
    }
}