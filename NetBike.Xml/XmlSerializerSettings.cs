namespace NetBike.Xml
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using NetBike.Xml.Contracts;
    using NetBike.Xml.Converters;
    using NetBike.Xml.Converters.Basics;
    using NetBike.Xml.Converters.Collections;
    using NetBike.Xml.Converters.Objects;
    using NetBike.Xml.Converters.Specialized;
    using NetBike.Xml.TypeResolvers;

    public sealed class XmlSerializerSettings
    {
        private static readonly XmlConverterCollection DefaultConverters;

        private readonly ConcurrentDictionary<Type, XmlTypeContext> typeContextCache;
        private readonly XmlConverterCollection converters;
        private readonly List<XmlNamespace> namespaces;
        private bool omitXmlDeclaration;
        private bool indent;
        private string indentChars;
        private Encoding encoding;
        private IXmlTypeResolver typeResolver;
        private IXmlContractResolver contractResolver;
        private CultureInfo cultureInfo;
        private XmlName typeAttributeName;
        private XmlName nullAttributeName;
        private XmlReaderSettings readerSettings;
        private XmlWriterSettings writerSettings;

        static XmlSerializerSettings()
        {
            DefaultConverters = new XmlConverterCollection
            {
                new XmlStringConverter(),
                new XmlBooleanConverter(),
                new XmlCharConverter(),
                new XmlByteConverter(),
                new XmlSByteConverter(),
                new XmlInt16Converter(),
                new XmlUInt16Converter(),
                new XmlInt32Converter(),
                new XmlUInt32Converter(),
                new XmlInt64Converter(),
                new XmlUInt64Converter(),
                new XmlSingleConverter(),
                new XmlDoubleConverter(),
                new XmlDecimalConverter(),
                new XmlEnumConverter(),
                new XmlGuidConverter(),
                new XmlDateTimeConverter(),
                new XmlTimeSpanConverter(),
                new XmlDateTimeOffsetConverter(),
                new XmlArrayConverter(),
                new XmlListConverter(),
                new XmlDictionaryConverter(),
                new XmlKeyValuePairConverter(),
                new XmlNullableConverter(),
                new XmlEnumerableConverter(),
                new XmlObjectConverter()
            };
        }

        public XmlSerializerSettings()
        {
            this.converters = new XmlConverterCollection();
            this.converters.CollectionChanged += (sender, ea) => this.typeContextCache.Clear();
            this.typeContextCache = new ConcurrentDictionary<Type, XmlTypeContext>();
            this.typeResolver = new XmlTypeResolver();
            this.contractResolver = new XmlContractResolver();
            this.cultureInfo = CultureInfo.InvariantCulture;
            this.typeAttributeName = new XmlName("type", XmlNamespace.Xsi);
            this.nullAttributeName = new XmlName("nil", XmlNamespace.Xsi);
            this.encoding = Encoding.UTF8;
            this.TypeHandling = XmlTypeHandling.Auto;
            this.NullValueHandling = XmlNullValueHandling.Ignore;
            this.DefaultValueHandling = XmlDefaultValueHandling.Include;
            this.omitXmlDeclaration = false;
            this.indentChars = "  ";
            this.indent = false;
            this.namespaces = new List<XmlNamespace>
            {
                new XmlNamespace("xsi", XmlNamespace.Xsi)
            };
        }

        public XmlTypeHandling TypeHandling { get; set; }

        public XmlNullValueHandling NullValueHandling { get; set; }

        public XmlDefaultValueHandling DefaultValueHandling { get; set; }

        public bool OmitXmlDeclaration
        {
            get
            {
                return this.omitXmlDeclaration;
            }

            set
            {
                this.omitXmlDeclaration = value;
                this.readerSettings = null;
            }
        }

        public bool Indent
        {
            get
            {
                return this.indent;
            }

            set
            {
                this.indent = value;
                this.readerSettings = null;
            }
        }

        public string IndentChars
        {
            get
            {
                return this.indentChars;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.indentChars = value;
                this.readerSettings = null;
            }
        }

        public XmlName TypeAttribute
        {
            get
            {
                return this.typeAttributeName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.typeAttributeName = value;
            }
        }

        public XmlName NullAttribute
        {
            get
            {
                return this.nullAttributeName;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.nullAttributeName = value;
            }
        }

        public CultureInfo Culture
        {
            get
            {
                return this.cultureInfo;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.cultureInfo = value;
            }
        }

        public IXmlTypeResolver TypeResolver
        {
            get
            {
                return this.typeResolver;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.typeResolver = value;
                this.typeContextCache.Clear();
            }
        }

        public IXmlContractResolver ContractResolver
        {
            get
            {
                return this.contractResolver;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.contractResolver = value;
                this.typeContextCache.Clear();
            }
        }

        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.encoding = value;
                this.readerSettings = null;
            }
        }

        public ICollection<XmlNamespace> Namespaces
        {
            get { return this.namespaces; }
        }

        public ICollection<IXmlConverter> Converters
        {
            get { return this.converters; }
        }

        internal XmlWriterSettings GetWriterSettings()
        {
            var settings = this.writerSettings;

            if (settings == null)
            {
                settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = this.OmitXmlDeclaration,
                    Indent = this.Indent,
                    Encoding = this.Encoding,
                    IndentChars = this.IndentChars,
                    CloseOutput = false
                };

                this.writerSettings = settings;
            }

            return settings;
        }

        internal XmlReaderSettings GetReaderSettings()
        {
            var settings = this.readerSettings;

            if (settings == null)
            {
                settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                    CloseInput = false
                };

                this.readerSettings = settings;
            }

            return settings;
        }

        internal XmlTypeContext GetTypeContext(Type valueType)
        {
            XmlTypeContext context;

            if (!this.typeContextCache.TryGetValue(valueType, out context))
            {
                context = this.CreateTypeContext(valueType, context);
            }

            return context;
        }

        private static IXmlConverter GetConverter(XmlContract contract, IXmlConverter converter)
        {
            if (converter == null)
            {
                return null;
            }

            var factory = converter as IXmlConverterFactory;

            if (factory != null)
            {
                converter = factory.CreateConverter(contract);
            }

            return converter;
        }

        private XmlTypeContext CreateTypeContext(Type valueType, XmlTypeContext context)
        {
            IXmlConverter readConverter = null;
            IXmlConverter writeConverter = null;

            foreach (var converter in this.converters.Concat(DefaultConverters))
            {
                if (readConverter == null && converter.CanRead(valueType))
                {
                    readConverter = converter;

                    if (writeConverter != null)
                    {
                        break;
                    }
                }

                if (writeConverter == null && converter.CanWrite(valueType))
                {
                    writeConverter = converter;

                    if (readConverter != null)
                    {
                        break;
                    }
                }
            }

            var contract = this.contractResolver.ResolveContract(valueType);

            readConverter = GetConverter(contract, readConverter);
            writeConverter = GetConverter(contract, writeConverter);

            context = new XmlTypeContext(contract, readConverter, writeConverter);
            this.typeContextCache.TryAdd(valueType, context);
            return context;
        }
    }
}