namespace NetBike.Xml.Formatting
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using NetBike.Xml;

    public sealed class NetBikeXmlMediaTypeFormatter : BufferedMediaTypeFormatter
    {
        private readonly XmlSerializer serializer;

        public NetBikeXmlMediaTypeFormatter()
            : this(CreateDefaultSettings())
        {
        }

        public NetBikeXmlMediaTypeFormatter(XmlSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.serializer = new XmlSerializer(settings);

            this.SupportedEncodings.Add(new UTF8Encoding(false, true));
            this.SupportedEncodings.Add(new UnicodeEncoding(false, true, true));

            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));
        }

        public XmlSerializerSettings Settings => this.serializer.Settings;

        public override bool CanReadType(Type type)
        {
            return this.serializer.CanDeserialize(type);
        }

        public override bool CanWriteType(Type type)
        {
            return this.serializer.CanSerialize(type);
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            return this.serializer.Deserialize(readStream, type);
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            this.serializer.Serialize(writeStream, type, value);
        }

        private static XmlSerializerSettings CreateDefaultSettings()
        {
            return new XmlSerializerSettings();
        }
    }
}