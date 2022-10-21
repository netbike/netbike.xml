namespace NetBike.Xml.Utilities
{
    using System;
    using System.Text;

    /// <summary>
    /// Implementation of RFC3339
    /// http://tools.ietf.org/html/rfc3339
    /// </summary>
    internal static class RfcDateTime
    {
        public static string ToDateString(DateTime dateTime)
        {
            var builder = new StringBuilder(9);

            builder
                .Append4(dateTime.Year)
                .Append('-')
                .Append2(dateTime.Month)
                .Append('-')
                .Append2(dateTime.Day);

            return builder.ToString();
        }

        public static string ToDateTimeString(DateTime dateTime)
        {
            var builder = new StringBuilder(35);

            builder
                .Append4(dateTime.Year)
                .Append('-')
                .Append2(dateTime.Month)
                .Append('-')
                .Append2(dateTime.Day)
                .Append('T')
                .Append2(dateTime.Hour)
                .Append(':')
                .Append2(dateTime.Minute)
                .Append(':')
                .Append2(dateTime.Second);

            var ticks = dateTime.Ticks % TimeSpan.TicksPerSecond;

            if (ticks != 0)
            {
                builder.Append('.');
                builder.Append(ticks.ToString().TrimEnd('0'));
            }

            if (dateTime.Kind == DateTimeKind.Utc)
            {
                builder.Append('Z');
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                var utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);

                if (utcOffset.Ticks > 0)
                {
                    builder.Append('+');
                }
                else
                {
                    builder.Append('-');
                }

                builder
                    .Append2(utcOffset.Hours)
                    .Append(':')
                    .Append2(utcOffset.Minutes);
            }

            return builder.ToString();
        }

        public static DateTime ParseDateTime(string value)
        {
            if (!TryParseDateTime(value, out var result))
            {
                throw new FormatException("Invalid ISO-8601 DateTime.");
            }

            return result;
        }

        public static bool TryParseDateTime(string value, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrEmpty(value) || value.Length < 16)
            {
                return false;
            }

            if (value[4] != '-' || value[7] != '-' || value[10] != 'T' || value[13] != ':')
            {
                return false;
            }

            if (!TryParseYear(value, 0, 4, out var year) ||
                !TryParseMonth(value, 5, 2, out var month) ||
                !TryParseDay(value, 8, 2, out var day) ||
                !TryParseHours(value, 11, 2, out var hour) ||
                !TryParseMinute(value, 14, 2, out var minute))
            {
                return false;
            }

            if (value.Length == 16)
            {
                result = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Unspecified);
                return true;
            }

            int tzd;

            if (value[16] != ':')
            {
                if (!TryParseTimeZone(value, 16, value.Length - 16, out tzd))
                {
                    return false;
                }

                result = new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc).AddMinutes(-1 * tzd);
                return true;
            }

            if (value.Length < 19)
            {
                return false;
            }

            if (!TryParseSecond(value, 17, 2, out var second))
            {
                return false;
            }

            if (value.Length == 19)
            {
                result = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
            }
            else if (value[19] != '.')
            {
                if (!TryParseTimeZone(value, 19, value.Length - 19, out tzd))
                {
                    return false;
                }

                result = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc).AddMinutes(-1 * tzd);
            }
            else
            {
                var index = 20;

                while (index < value.Length && char.IsDigit(value[index]))
                {
                    index++;
                }

                if (!TryParseTicks(value, 20, index - 20, out var ticks))
                {
                    return false;
                }

                if (index == value.Length)
                {
                    result = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified).AddTicks(ticks);
                    return true;
                }

                if (!TryParseTimeZone(value, index, value.Length - index, out tzd))
                {
                    return false;
                }

                result = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc).AddMinutes(-1 * tzd).AddTicks(ticks);
            }

            return true;
        }
        
        public static DateTime ParseDate(string value)
        {
            if (!TryParseDate(value, out var result))
            {
                throw new FormatException("Invalid Date.");
            }

            return result;
        }

        public static bool TryParseDate(string value, out DateTime result)
        {
            result = DateTime.MinValue;

            if (string.IsNullOrEmpty(value) || value.Length < 10)
            {
                return false;
            }

            if (value[4] != '-' || value[7] != '-')
            {
                return false;
            }

            if (!TryParseYear(value, 0, 4, out var year) ||
                !TryParseMonth(value, 5, 2, out var month) ||
                !TryParseDay(value, 8, 2, out var day))
            {
                return false;
            }
            
            result = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Unspecified);
            return true;
        }

        private static bool TryParseYear(string value, int offset, int count, out int year)
        {
            return TryParseInt32(value, offset, count, out year);
        }

        private static bool TryParseMonth(string value, int offset, int count, out int month)
        {
            return TryParseInt32(value, offset, count, out month) && month >= 1 && month <= 12;
        }

        private static bool TryParseDay(string value, int offset, int count, out int day)
        {
            return TryParseInt32(value, offset, count, out day) && day >= 1 && day <= 31;
        }

        private static bool TryParseHours(string value, int offset, int count, out int hour)
        {
            return TryParseInt32(value, offset, count, out hour) && hour >= 0 && hour <= 23;
        }

        private static bool TryParseMinute(string value, int offset, int count, out int minute)
        {
            return TryParseInt32(value, offset, count, out minute) && minute >= 0 && minute <= 59;
        }

        private static bool TryParseSecond(string value, int offset, int count, out int second)
        {
            return TryParseInt32(value, offset, count, out second) && second >= 0 && second <= 59;
        }

        private static bool TryParseTimeZone(string value, int offset, int count, out int tzd)
        {
            if (count == 1 && value[offset] == 'Z')
            {
                tzd = 0;
                return true;
            }

            if (count != 6 ||
                value[offset + 3] != ':' ||
                !TryParseHours(value, offset + 1, 2, out var hour) ||
                !TryParseMinute(value, offset + 4, 2, out var minutes))
            {
                tzd = -1;
                return false;
            }

            tzd = hour * 60 + minutes;

            if (value[offset] == '-')
            {
                tzd *= -1;
            }

            return true;
        }

        private static bool TryParseTicks(string value, int offset, int count, out int ticks)
        {
            ticks = 0;
            var length = offset + Math.Min(count, 7);
            var factor = 1000000;

            for (var i = offset; i < length; i++)
            {
                var digit = value[i] - '0';

                if (digit < 0 || digit > 9)
                {
                    ticks = -1;
                    return false;
                }

                ticks = ticks + digit * factor;
                factor /= 10;
            }

            return true;
        }

        private static bool TryParseInt32(string value, int offset, int count, out int result)
        {
            result = 0;

            var length = offset + count;

            for (var i = offset; i < length; i++)
            {
                var digit = value[i] - '0';

                if (digit < 0 || digit > 9)
                {
                    result = -1;
                    return false;
                }

                result = result * 10 + digit;
            }

            return true;
        }

        private static StringBuilder Append2(this StringBuilder builder, int number)
        {
            if (number < 10)
            {
                builder.Append('0');
            }

            builder.Append(number);

            return builder;
        }

        private static StringBuilder Append4(this StringBuilder builder, int number)
        {
            if (number < 1000)
            {
                builder.Append('0');

                if (number < 100)
                {
                    builder.Append('0');
                }

                if (number < 10)
                {
                    builder.Append2('0');
                }
            }

            builder.Append(number);
            return builder;
        }
    }
}