using System;

namespace NetBike.Xml.Tests.Samples
{
    public class FooDateTime : IEquatable<FooDateTime>
    {
        [System.Xml.Serialization.XmlElementAttribute("DateTimeValue", DataType="dateTime")]
        public DateTime DateTimeValue { get; set; }
        
        [System.Xml.Serialization.XmlElementAttribute("DateValue", DataType="date")]
        public DateTime DateValue { get; set; }

        public bool Equals(FooDateTime other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return DateTimeValue.Equals(other.DateTimeValue) && DateValue.Equals(other.DateValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FooDateTime) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DateTimeValue, DateValue);
        }
    }
}