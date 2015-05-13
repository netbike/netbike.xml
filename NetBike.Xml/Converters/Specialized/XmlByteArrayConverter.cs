namespace NetBike.Xml.Converters.Specialized
{
    using System;
    using System.IO;
    using System.Xml;

    public sealed class XmlByteArrayConverter : XmlConverter<byte[]>
    {
        public const int ChunkSize = 512;

        public override void WriteXml(XmlWriter writer, object value, XmlSerializationContext context)
        {
            var bytes = (byte[])value;
            writer.WriteBase64(bytes, 0, bytes.Length);
        }

        public override object ReadXml(XmlReader reader, XmlSerializationContext context)
        {
            if (reader.NodeType == XmlNodeType.Attribute)
            {
                return Convert.FromBase64String(reader.Value);
            }

            int bytesRead;
            byte[] buffer = new byte[ChunkSize];
            MemoryStream stream = null;

            if (!reader.IsEmptyElement)
            {
                reader.ReadStartElement();

                while ((bytesRead = reader.ReadContentAsBase64(buffer, 0, buffer.Length)) > 0)
                {
                    if (stream == null)
                    {
                        stream = new MemoryStream(ChunkSize * 2);
                    }

                    stream.Write(buffer, 0, bytesRead);
                }

                reader.ReadEndElement();
            }
            else
            {
                reader.Read();
            }

            if (stream == null)
            {
                return new byte[0];
            }

            return stream.ToArray();
        }
    }
}