using System;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Unix Timestamp extensions to help with deriving or computing timestamp values.
    /// </summary>
    /// <remarks>
    /// This class resolves issues as seen on the following urls:
    /// https://stackoverflow.com/questions/17632584/how-to-get-the-unix-timestamp-in-c-sharp.
    /// </remarks>
    public static class UnixTimestampExtensions
    {
        /// <summary>
        /// Returns the Start Date of the Epoch as a UTC DateTime. This is the stored value 1/1/1970 12:00:00 AM, which
        /// converts over to 0 for a Unix Timestamp.
        /// </summary>
        public static DateTime StartOfEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Returns the Start Date of the Epoch as a Unix Timestamp with the value 0, which converts over to the
        /// UTC DateTime 1/1/1970 12:00:00 AM.
        /// </summary>
        public static int StartOfEpochUnixTimestamp => StartOfEpoch.ToUnixTimestamp();

        /// <summary>
        /// Returns the End Date of the Epoch as a UTC DateTime. This is the stored value 1/19/2038 3:14:07 AM, which
        /// converts over to 2147483647 for a Unix Timestamp.
        /// </summary>
        public static DateTime EndOfEpoch => new DateTime(2038, 1, 19, 3, 14, 7, DateTimeKind.Utc);

        /// <summary>
        /// Returns the End Date of the Epoch as a Unix Timestamp with the value 2147483647, which converts over to the
        /// UTC DateTime 1/19/2038 3:14:07 AM.
        /// </summary>
        public static int EndOfEpochUnixTimestamp => EndOfEpoch.ToUnixTimestamp();

        /// <summary>
        /// Returns the End Date of the Epoch as a UTC DateTime through computation of the Maximum possible UTC DateTime
        /// value that could be converted to a 32-bit Epoch integer. This is generally the value 1/19/2038 3:14:07 AM,
        /// which converts over to 2147483647 for a Unix Timestamp.
        /// </summary>
        /// <remarks>
        /// This would not realistically be used unless someone were debugging it going into 64-bit / 128-bit Epoch realm
        /// and this person needed to derive a real computation of the new constants. This is more of an exploratory
        /// method than it is anything else, and it could be computationally slow.
        /// Start Date of Epoch: 1/1/1970 12:00:00 AM
        /// Start Unix Timestamp: 0
        /// End Date of Epoch: 1/19/2038 3:14:07 AM
        /// End Unix Timestamp: 2147483647
        /// Utc Now: 8/13/2019 10:39:31 AM
        /// Utc Now Unix Timestamp: 1565692771
        /// </remarks>
        /// <returns>
        /// A DateTime object holding the maximum DateTime value in UTC that can be held by a 32-bit Epoch integer.
        /// This is generally the value 1/19/2038 3:14:07 AM, which converts over to 2147483647 for a Unix Timestamp.
        /// </returns>
        public static DateTime ComputedEndOfEpoch
        {
            get
            {
                // Get .Net Max DateTime Value.
                var endDateOfEpoch = StartOfEpoch.AddYears(68);

                // Clear out Milliseconds as Epoch values are only accurate to the second.
                endDateOfEpoch = endDateOfEpoch.AddMilliseconds(-endDateOfEpoch.Millisecond);

                // While the ToUnixTimestamp method returns an overflow negative integer, subtract years.
                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddYears(1);
                endDateOfEpoch = endDateOfEpoch.AddYears(-1);

                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddMonths(1);
                endDateOfEpoch = endDateOfEpoch.AddMonths(-1);

                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddDays(1);
                endDateOfEpoch = endDateOfEpoch.AddDays(-1);

                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddHours(1);
                endDateOfEpoch = endDateOfEpoch.AddHours(-1);

                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddMinutes(1);
                endDateOfEpoch = endDateOfEpoch.AddMinutes(-1);

                while (endDateOfEpoch.ToUnixTimestamp() > 1)
                    endDateOfEpoch = endDateOfEpoch.AddSeconds(1);
                endDateOfEpoch = endDateOfEpoch.AddSeconds(-1);

                return endDateOfEpoch;
            }
        }

        /// <summary>
        /// Returns the End Date of the Epoch as a 32-bit integer representing a Unix Timestamp
        /// through computation of the Maximum possible UTC DateTime value that could be converted
        /// to a 32-bit Integer. This is generally the value 2147483647, which converts over to
        /// the UTC DateTime value 1/19/2038 3:14:07 AM.
        /// </summary>
        /// <remarks>
        /// Start Date of Epoch: 1/1/1970 12:00:00 AM
        /// Start Unix Timestamp: 0
        /// End Date of Epoch: 1/19/2038 3:14:07 AM
        /// End Unix Timestamp: 2147483647
        /// Utc Now: 8/13/2019 10:39:31 AM
        /// Utc Now Unix Timestamp: 1565692771
        /// </remarks>
        /// <returns>
        /// An integer value holding the maximum DateTime value in UTC that can be held
        /// by a 32-bit Epoch integer. This is generally the value 1/19/2038 3:14:07 AM,
        /// which converts over to 2147483647 for a Unix Timestamp.
        /// </returns>
        public static int ComputedEndOfEpochUnixTimestamp => ComputedEndOfEpoch.ToUnixTimestamp();


        /// <summary>
        /// Converts a given DateTime into a Unix timestamp
        /// </summary>
        /// <param name="value">Any DateTime</param>
        /// <returns>The given DateTime in Unix timestamp format</returns>
        public static int ToUnixTimestamp(this DateTime value)
        {
            return (int)Math.Truncate(value.ToUniversalTime().Subtract(StartOfEpoch).TotalSeconds);
        }

        /// <summary>
        /// Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp</returns>
        public static int UnixTimestamp(this DateTime ignored)
        {
            return (int)Math.Truncate(DateTime.UtcNow.Subtract(StartOfEpoch).TotalSeconds);
        }
    }
}
