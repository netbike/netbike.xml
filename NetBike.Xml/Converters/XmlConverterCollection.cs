namespace NetBike.Xml.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    internal sealed class XmlConverterCollection : Collection<IXmlConverter>
    {
        public XmlConverterCollection()
        {
        }

        public XmlConverterCollection(IEnumerable<IXmlConverter> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public event EventHandler CollectionChanged;

        protected override void InsertItem(int index, IXmlConverter item)
        {
            base.InsertItem(index, item);
            this.OnCollectionChanged();
        }

        protected override void ClearItems()
        {
            base.ClearItems();
            this.OnCollectionChanged();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            this.OnCollectionChanged();
        }

        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}