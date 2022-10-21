namespace NetBike.Xml.Contracts.Builders
{
    using System;
    using System.Collections.Generic;

    public sealed class XmlEnumItemCollection : IEnumerable<XmlEnumItem>
    {
        private readonly List<XmlEnumItem> items;

        public XmlEnumItemCollection()
        {
            this.items = new List<XmlEnumItem>();
        }

        public XmlEnumItemCollection(IEnumerable<XmlEnumItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = new List<XmlEnumItem>(items);
        }

        public int Count => this.items.Count;

        public void Add(long value, string name)
        {
            var item = new XmlEnumItem(value, name);
            this.Add(item);
        }

        public void Add(XmlEnumItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = this.IndexOf(item.Value);

            if (index != -1)
            {
                throw new ArgumentException($"Enum item \"{item.Value}\" already registered.", nameof(item));
            }

            this.items.Add(item);
        }

        public void Set(XmlEnumItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var index = this.IndexOf(item.Value);

            if (index == -1)
            {
                this.items.Add(item);
            }
            else
            {
                this.items[index] = item;
            }
        }

        public bool Contains(long value)
        {
            return this.IndexOf(value) != -1;
        }

        public bool Remove(long value)
        {
            var index = this.IndexOf(value);

            if (index != -1)
            {
                this.items.RemoveAt(index);
                return true;
            }

            return false;
        }

        public IEnumerator<XmlEnumItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        private int IndexOf(long value)
        {
            for (var i = 0; i < this.items.Count; i++)
            {
                if (this.items[i].Value == value)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}