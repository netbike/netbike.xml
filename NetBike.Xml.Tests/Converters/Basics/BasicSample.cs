namespace NetBike.Xml.Tests.Converters.Basics
{
    public class BasicSample
    {
        public BasicSample(string stringValue, object value)
        {
            this.Value = value;
            this.StringValue = stringValue;
        }

        public object Value { get; }

        public string StringValue { get; }

        public override string ToString()
        {
            return $"[{(this.Value != null ? this.Value.ToString() : "null")}, {this.StringValue ?? "null"}]";
        }
    }
}