namespace NetBike.Xml.Tests.Converters.Basics
{
    public class BasicSample
    {
        public BasicSample(string stringValue, object value)
        {
            Value = value;
            StringValue = stringValue;
        }

        public object Value { get; private set; }

        public string StringValue { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", Value != null ? Value.ToString() : "null", StringValue ?? "null");
        }
    }
}