namespace NetBike.Xml.Converters.Collections
{
    public interface ICollectionProxy
    {
        void Add(object value);

        object GetResult();
    }
}