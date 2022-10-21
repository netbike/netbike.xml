namespace NetBike.Xml.Contracts.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class XmlKnownTypeBuilderCollection : IEnumerable<XmlKnownTypeBuilder>
    {
        private readonly List<XmlKnownTypeBuilder> items;

        public XmlKnownTypeBuilderCollection()
        {
            this.items = new List<XmlKnownTypeBuilder>();
        }

        public XmlKnownTypeBuilderCollection(IEnumerable<XmlKnownTypeBuilder> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = new List<XmlKnownTypeBuilder>(items);
        }

        public int Count => this.items.Count;

        public void Add(Type valueType, XmlName name)
        {
            var builder = new XmlKnownTypeBuilder(valueType).SetName(name);
            this.Add(builder);
        }

        public void Add(XmlKnownTypeBuilder item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = this.IndexOf(item.ValueType);

            if (index != -1)
            {
                throw new ArgumentException($"Known type \"{item.ValueType}\" already registered.");
            }

            this.items.Add(item);
        }

        public void Set(XmlKnownTypeBuilder item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = this.IndexOf(item.ValueType);

            if (index == -1)
            {
                this.items.Add(item);
            }
            else
            {
                this.items[index] = item;
            }
        }

        public bool Contains(Type valueType)
        {
            return this.IndexOf(valueType) != -1;
        }

        public bool Remove(Type valueType)
        {
            var index = this.IndexOf(valueType);

            if (index != -1)
            {
                this.items.RemoveAt(index);
                return true;
            }

            return false;
        }

        public IEnumerable<XmlKnownType> Build()
        {
            return this.items.Select(x => x.Build());
        }

        public IEnumerator<XmlKnownTypeBuilder> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        internal static XmlKnownTypeBuilderCollection Create(IEnumerable<XmlKnownType> knownTypes)
        {
            if (knownTypes == null)
            {
                throw new ArgumentNullException(nameof(knownTypes));
            }

            var items = knownTypes.Select(x => XmlKnownTypeBuilder.Create(x));
            return new XmlKnownTypeBuilderCollection(items);
        }

        private int IndexOf(Type valueType)
        {
            for (var i = 0; i < this.items.Count; i++)
            {
                if (this.items[i].ValueType == valueType)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}