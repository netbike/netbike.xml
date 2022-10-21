namespace NetBike.Xml
{
    using System;
    using System.IO;
    using System.Xml;

    public sealed class XmlSerializer
    {
        public XmlSerializer()
            : this(new XmlSerializerSettings())
        {
        }

        public XmlSerializer(XmlSerializerSettings settings)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public XmlSerializerSettings Settings { get; }

        public bool CanSerialize(Type valueType)
        {
            return this.Settings.GetTypeContext(valueType).WriteConverter != null;
        }

        public bool CanDeserialize(Type valueType)
        {
            return this.Settings.GetTypeContext(valueType).ReadConverter != null;
        }

        public void Serialize<T>(Stream stream, T value)
        {
            this.Serialize(stream, typeof(T), value);
        }

        public void Serialize<T>(TextWriter output, T value)
        {
            this.Serialize(output, typeof(T), value);
        }

        public void Serialize<T>(XmlWriter writer, T value)
        {
            this.Serialize(writer, typeof(T), value);
        }

        public void Serialize(Stream stream, Type valueType, object value)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var writer = XmlWriter.Create(stream, this.Settings.GetWriterSettings());
            this.Serialize(writer, valueType, value);
        }

        public void Serialize(TextWriter output, Type valueType, object value)
        {
            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            var writer = XmlWriter.Create(output, this.Settings.GetWriterSettings());
            this.Serialize(writer, valueType, value);
        }

        public void Serialize(XmlWriter writer, Type valueType, object value)
        {
            var context = new XmlSerializationContext(this.Settings);
            context.Serialize(writer, value, valueType);
            writer.Flush();
        }

        public T Deserialize<T>(Stream stream)
        {
            return (T)this.Deserialize(stream, typeof(T));
        }

        public T Deserialize<T>(TextReader input)
        {
            return (T)this.Deserialize(input, typeof(T));
        }

        public T Deserialize<T>(XmlReader reader)
        {
            return (T)this.Deserialize(reader, typeof(T));
        }

        public object Deserialize(Stream stream, Type valueType)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var reader = XmlReader.Create(stream, this.Settings.GetReaderSettings());
            return this.Deserialize(reader, valueType);
        }

        public object Deserialize(TextReader input, Type valueType)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var reader = XmlReader.Create(input, this.Settings.GetReaderSettings());
            return this.Deserialize(reader, valueType);
        }

        public object Deserialize(XmlReader reader, Type valueType)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (valueType == null)
            {
                throw new ArgumentNullException(nameof(valueType));
            }

            var context = new XmlSerializationContext(this.Settings);
            return context.Deserialize(reader, valueType);
        }
    }
}