namespace NetBike.Xml.Converters.Specialized
{
    using System;
    using System.Collections.Generic;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters.Objects;

    public sealed class XmlKeyValuePairConverter : XmlConverterFactory
    {
        protected override bool AcceptType(Type valueType)
        {
            return valueType.IsGenericTypeOf(typeof(KeyValuePair<,>));
        }

        protected override Type GetConverterType(Type valueType)
        {
            var arguments = valueType.GetGenericArguments();
            return typeof(XmlTypedKeyValuePairConverter<,>).MakeGenericType(arguments);
        }

        private sealed class XmlTypedKeyValuePairConverter<TKey, TValue> : XmlObjectConverter
        {
            public override bool CanRead(Type valueType)
            {
                return valueType == typeof(KeyValuePair<TKey, TValue>);
            }

            public override bool CanWrite(Type valueType)
            {
                return valueType == typeof(KeyValuePair<TKey, TValue>);
            }

            protected override bool CanWriteProperty(XmlProperty property)
            {
                return true;
            }

            protected override object CreateTarget(XmlContract contract)
            {
                return new ValueProxy();
            }

            protected override object GetResult(object target)
            {
                var valueProxy = (ValueProxy)target;
                return new KeyValuePair<TKey, TValue>(valueProxy.Key, valueProxy.Value);
            }

            protected override object GetValue(object target, XmlProperty property)
            {
                var kvp = (KeyValuePair<TKey, TValue>)target;
                return property.PropertyName.Length == 3 ? (object)kvp.Key : kvp.Value;
            }

            protected override void SetValue(object target, XmlProperty property, object propertyValue)
            {
                var valueProxy = (ValueProxy)target;

                if (property.PropertyName.Length == 3)
                {
                    valueProxy.Key = (TKey)propertyValue;
                }
                else
                {
                    valueProxy.Value = (TValue)propertyValue;
                }
            }

            private class ValueProxy
            {
                public TKey Key { get; set; }

                public TValue Value { get; set; }
            }
        }
    }
}