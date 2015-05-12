namespace NetBike.Xml
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;

    public sealed class XmlSerializer
    {
        private readonly XmlSerializationSettings settings;

        public XmlSerializer()
            : this(new XmlSerializationSettings())
        {
        }

        public XmlSerializer(XmlSerializationSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
        }

        public XmlSerializationSettings Settings
        {
            get { return this.settings; }
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
                throw new ArgumentNullException("stream");
            }

            var writer = XmlWriter.Create(stream, this.settings.GetWriterSettings());
            this.Serialize(writer, valueType, value);
        }

        public void Serialize(TextWriter output, Type valueType, object value)
        {
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var writer = XmlWriter.Create(output, this.settings.GetWriterSettings());
            this.Serialize(writer, valueType, value);
        }

        public void Serialize(XmlWriter writer, Type valueType, object value)
        {
            var context = new XmlSerializationContext(this.settings);
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
                throw new ArgumentNullException("stream");
            }

            var reader = XmlReader.Create(stream, this.settings.GetReaderSettings());
            return this.Deserialize(reader, valueType);
        }

        public object Deserialize(TextReader input, Type valueType)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }

            var reader = XmlReader.Create(input, this.settings.GetReaderSettings());
            return this.Deserialize(reader, valueType);
        }

        public object Deserialize(XmlReader reader, Type valueType)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            var context = new XmlSerializationContext(this.settings);
            return context.Deserialize(reader, valueType);
        }
    }
}