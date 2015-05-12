namespace NetBike.Xml.Tests.Samples
{
    using System;

    public class FooBar : Foo, IBar
    {
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as FooBar;
            return other != null && base.Equals(obj) && other.Description == Description;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}