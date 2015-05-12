namespace NetBike.Xml
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Xml;
    using NetBike.Xml.Contracts;

    internal static class Types
    {
        public static readonly Type Void = typeof(void);

        public static readonly Type Object = typeof(object);

        public static readonly Type String = typeof(string);

        public static readonly Type Byte = typeof(byte);

        public static readonly Type Int16 = typeof(short);

        public static readonly Type Int32 = typeof(int);

        public static readonly Type Int64 = typeof(long);

        public static readonly Type Char = typeof(char);

        public static readonly Type Float = typeof(float);

        public static readonly Type Double = typeof(double);

        public static readonly Type Bool = typeof(bool);

        public static readonly Type Decimal = typeof(decimal);

        public static readonly Type Enumerable = typeof(IEnumerable);

        public static readonly Type EnumerableDefinition = typeof(IEnumerable<>);

        public static readonly Type NullableDefinition = typeof(Nullable<>);

        public static readonly Type XmlReader = typeof(XmlReader);

        public static readonly Type XmlWriter = typeof(XmlWriter);

        public static readonly Type XmlName = typeof(XmlName);

        public static readonly Type XmlSerializationContext = typeof(XmlSerializationContext);

        public static readonly Type FlagsAttribute = typeof(FlagsAttribute);
    }
}