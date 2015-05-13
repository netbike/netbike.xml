namespace NetBike.Xml.Contracts
{
    using System.Globalization;
    using System.Text;

    public static class NamingConventions
    {
        public static string Ignore(string value)
        {
            return value;
        }

        public static string CamelCase(string value)
        {
            if (string.IsNullOrEmpty(value) || !char.IsUpper(value[0]))
            {
                return value;
            }

            var index = 0;
            var source = value.ToCharArray();

            do
            {
                source[index] = char.ToLower(source[index], CultureInfo.InvariantCulture);
                index++;
            }
            while (index != source.Length && char.IsUpper(source[index]));

            return new string(source);
        }

        public static string Dashed(string value)
        {
            return SeparateWords(value, '-');
        }

        public static string Underscore(string value)
        {
            return SeparateWords(value, '_');
        }

        private static string SeparateWords(string value, char separator)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var separate = true;
            var builder = new StringBuilder(value.Length + 5);

            for (var i = 0; i < value.Length; i++)
            {
                var ch = value[i];

                if (char.IsUpper(ch))
                {
                    ch = char.ToLower(ch);

                    if (!separate)
                    {
                        builder.Append(separator);
                    }

                    separate = true;
                }
                else
                {
                    separate = false;
                }

                builder.Append(ch);
            }

            return builder.ToString();
        }
    }
}