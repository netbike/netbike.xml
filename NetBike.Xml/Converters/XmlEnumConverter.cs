namespace NetBike.Xml.Converters
{
    using System;
    using System.Text;
    using System.Xml;
    using NetBike.Xml.Contracts;

    public sealed class XmlEnumConverter : IXmlConverter
    {
        private readonly char separator;

        public XmlEnumConverter()
            : this(' ')
        {
        }

        public XmlEnumConverter(char separator)
        {
            this.separator = separator;
        }

        public bool CanRead(Type valueType)
        {
            return valueType.IsEnum;
        }

        public bool CanWrite(Type valueType)
        {
            return valueType.IsEnum;
        }

        public void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var contract = context.Contract.ToEnumContract();
            var enumValue = Convert.ToInt64(value);
            string enumName = null;

            foreach (var item in contract.Items)
            {
                if (item.Value == enumValue)
                {
                    enumName = item.Name;
                    break;
                }
            }

            if (enumName == null && contract.IsFlag)
            {
                var builder = new StringBuilder();

                foreach (var item in contract.Items)
                {
                    if ((item.Value & enumValue) != 0)
                    {
                        if (builder.Length != 0)
                        {
                            builder.Append(this.separator);
                        }

                        builder.Append(item.Name);

                        enumValue &= ~item.Value;
                    }
                }

                if (enumValue == 0 && builder.Length > 0)
                {
                    enumName = builder.ToString();
                }
            }

            if (enumName == null)
            {
                throw new FormatException(
                    $"Enumerable value \"{enumValue}\" of type \"{contract.ValueType}\" is invalid.");
            }

            writer.WriteString(enumName);
        }

        public object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            var contract = context.Contract.ToEnumContract();
            var valueString = reader.ReadAttributeOrElementContent();
            var value = 0L;

            if (!contract.IsFlag)
            {
                value = GetEnumValue(contract, valueString);
            }
            else
            {
                var names = valueString.Split(new[] { this.separator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var name in names)
                {
                    value |= GetEnumValue(contract, name);
                }
            }

            return Convert.ChangeType(value, contract.UnderlyingType);
        }

        private static long GetEnumValue(XmlEnumContract contract, string name)
        {
            name = name.Trim();

            foreach (var item in contract.Items)
            {
                if (name == item.Name)
                {
                    return item.Value;
                }
            }

            throw new FormatException($"Enumerable name \"{name}\" of type \"{contract.ValueType}\" is invalid.");
        }
    }
}