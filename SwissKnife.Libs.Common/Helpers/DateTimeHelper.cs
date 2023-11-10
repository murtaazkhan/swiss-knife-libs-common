
namespace SwissKnife.Libs.Common.Helpers
{
    /// <summary>
    /// This class provides commonly date time helper functions
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// To convert provided date time with offset value
        /// </summary>
        /// <param name="dateTime">Date time to convert</param>
        /// <param name="timezoneOffset">offset value for conversion eg. 4:00, -5:00 </param>
        public static DateTime ConvertToTimeZoneOffset(DateTime dateTime, string timezoneOffset)
        {
            var result = dateTime;
            if (!string.IsNullOrEmpty(timezoneOffset))
            {
                string offset;
                bool isNegative = false;
                if (timezoneOffset.Contains('-'))
                {
                    offset = timezoneOffset.Replace("-", "");
                    isNegative = true;
                }
                else
                {
                    offset = timezoneOffset.Replace("+", "");
                }

                TimeSpan utcOffset = TimeSpan.Parse(offset);
                result = isNegative ? dateTime.Add(-utcOffset) : dateTime.Add(utcOffset);
            }

            return result;
        }
    }
}
//TODO: timezone format using string