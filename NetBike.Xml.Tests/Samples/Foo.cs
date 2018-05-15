namespace NetBike.Xml.Tests.Samples
{
    using System;

    public class Foo : IFoo
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public FooEnum? EnumValue { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Foo;

            if (other == null)
            {
                return false;
            }

            return other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}