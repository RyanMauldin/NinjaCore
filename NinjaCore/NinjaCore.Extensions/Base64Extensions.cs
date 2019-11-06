using System;
using System.Linq;
using System.Text;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Base64 extensions to make working with Base64 value strings easier.
    /// </summary>
    public static class Base64Extensions
    {
        /// <summary>
        /// The default character used for padding in society.
        /// </summary>
        public const char DefaultPadCharacter = '=';

        /// <summary>
        /// The legacy character used for padding in society.
        /// </summary>
        public const char LegacyPadCharacter = '-';

        /// <summary>
        /// The default character used for splitting.
        /// </summary>
        public const char DefaultSplitCharacter = '_';

        /// <summary>
        /// Converts from a UTF8 encoded string value to a Base64 string.
        /// </summary>
        /// <remarks>
        /// If <paramref name="preserveSplits"/> is set to false, and invalid characters are found, this method will
        /// throw an exception.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <param name="addPadding">Whether or not to add padding to the end of the value after encoding.</param>
        /// <param name="padCharacter">
        /// The target padding character to utilize if <paramref name="addPadding"/> is set to true.
        /// </param>
        /// <param name="preserveSplits">
        /// Whether or not to check for and split on key splitting tokens to encode segments separately.
        /// </param>
        /// <param name="splitCharacter">
        /// The key key token character to utilize if <paramref name="preserveSplits"/> is set to true.
        /// </param>
        /// <returns>A Base64 encoded value.</returns>
        public static string ToBase64String(this string value, bool addPadding = true, char padCharacter = DefaultPadCharacter,
            bool preserveSplits = true, char splitCharacter = DefaultSplitCharacter)
        {
            // Call the main function, but specify Encoding.UTF8.
            return string.IsNullOrEmpty(value)
                ? value
                : value.ToBase64String(Encoding.UTF8, addPadding, padCharacter, preserveSplits, splitCharacter);
        }

        /// <summary>
        /// Converts from a custom encoded string value of encoding type <paramref name="encoding"/> to a Base64 string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="addPadding">Whether or not to add padding to the end of the value after encoding.</param>
        /// <param name="padCharacter">
        /// The target padding character to utilize if <paramref name="addPadding"/> is set to true.
        /// </param>
        /// <param name="preserveSplits">
        /// Whether or not to check for and split on key splitting tokens to encode segments separately.
        /// </param>
        /// <param name="splitCharacter">
        /// The character used to split the <paramref name="value"/> string into segments where each segment is individually
        /// Base64 encoded and joined back together using the split character value, preserving the split boundaries.
        /// </param>
        /// <returns>A Base64 encoded value.</returns>
        public static string ToBase64String(this string value, Encoding encoding, bool addPadding = true, char padCharacter = DefaultPadCharacter,
            bool preserveSplits = true, char splitCharacter = DefaultSplitCharacter)
        {
            // Do basic validation check, and return early if there is no string value to process.
            if (string.IsNullOrEmpty(value)) return value;

            // Setup builders.
            var builder = new StringBuilder();
            var trimCharacters = new[] { DefaultPadCharacter, LegacyPadCharacter, padCharacter };

            // Get the padded Base64 encoded value without padding.
            if (preserveSplits && value.Contains(splitCharacter, StringComparison.Ordinal))
            {
                // Process segments and preserve the split character values.
                var segments = value.Split(new[] { splitCharacter }, StringSplitOptions.RemoveEmptyEntries).ToList();
                // If there are any segments go ahead and Base64 encode each segment value and join these values back
                // together with the split character.
                if (segments.Any())
                    builder = builder.AppendJoin(splitCharacter, segments.Select(
                        segment => Convert.ToBase64String(encoding.GetBytes(segment), Base64FormattingOptions.None).TrimEnd(trimCharacters)));
            }
            else
            {
                // Base64 encode the value.
                builder.Append(Convert.ToBase64String(encoding.GetBytes(value), Base64FormattingOptions.None).TrimEnd(trimCharacters));
            }

            // Setup unpadded return value.
            var unpaddedValue = builder.ToString();
            // Return padded value if addPadding setting is enabled.
            return addPadding ? unpaddedValue.ToByteWidthPaddedString(padCharacter) : unpaddedValue;
        }

        /// <summary>
        /// Creates a padded string.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="padCharacter">The target padding character.</param>
        /// <returns>A padded string value.</returns>
        public static string ToByteWidthPaddedString(this string value, char padCharacter = DefaultPadCharacter)
        {
            // Do basic validation check and return early if there is no string value to process.
            if (string.IsNullOrEmpty(value)) return value;
            // Grab the content length of the value string.
            var valueLength = value.Length;
            // Do modular division by 4 against the length of the value string.
            var remainder = valueLength % 4;
            // If remainder of the modulo division is 3, then pad with 1 character, otherwise use the existing remainder.
            if (remainder == 3) remainder = 1;
            // Add remainder to the length of the value string to determine how many characters to pad with.
            var paddedLength = valueLength + remainder;
            // If the padded length is identical to the existing length of the value string than do not add padding.
            // Otherwise pass in the padded length and character to pad with to the PadRight method.
            var paddedValue = valueLength == paddedLength ? value : value.PadRight(paddedLength, padCharacter);
            // Return the padded string value.
            return paddedValue;
        }

        /// <summary>
        /// Converts from a Base64 string to a UTF8 encoded string value.
        /// </summary>
        /// <remarks>
        /// Passing in the <paramref name="value"/> parameter comprised of split characters that are not Base64 safe characters,
        /// in conjunction with setting <paramref name="preserveSplits"/> to a false value, will cause the internally called
        /// <see cref="Convert.FromBase64String(string)"/> method to throw an exception.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <param name="trimPadding">Whether or not to trim the end padding prior to doing Base64 conversions.</param>
        /// <param name="padCharacter">
        /// The target padding characters to union with <seealso cref="DefaultPadCharacter"/> for removing
        /// characters as padding, prior to doing Base64 conversions.
        /// </param>
        /// <param name="preserveSplits">Whether or not to split on the <paramref name="splitCharacter"/> value.</param>
        /// <param name="splitCharacter">
        /// The character to split the <paramref name="value"/> string into individual segments that each get Base64 decoded
        /// and tied back together again by preserving the split character values when joining back the segments.
        /// </param>
        /// <returns>A base 64 decoded string.</returns>
        public static string FromBase64String(this string value, bool trimPadding = true, char padCharacter = DefaultPadCharacter,
            bool preserveSplits = true, char splitCharacter = DefaultSplitCharacter)
        {
            // Call the main function, but specify Encoding.UTF8.
            return value.FromBase64String(Encoding.UTF8, trimPadding, padCharacter, preserveSplits, splitCharacter);
        }

        /// <summary>
        /// Converts from a Base64 string to a custom encoded string value of encoding type <paramref name="encoding"/>.
        /// </summary>
        /// <remarks>
        /// Passing in the <paramref name="value"/> parameter comprised of split characters that are not Base64 safe characters,
        /// in conjunction with setting <paramref name="preserveSplits"/> to a false value, will cause the internally called
        /// <see cref="Convert.FromBase64String(string)"/> method to throw an exception.
        /// </remarks>
        /// <param name="value">The value to convert.</param>
        /// <param name="encoding">The custom encoding to convert to.</param>
        /// <param name="trimPadding">Whether or not to trim the end padding prior to doing Base64 conversions.</param>
        /// <param name="padCharacter">
        /// The target padding character to union with <seealso cref="DefaultPadCharacter"/> for removing characters as
        /// padding, prior to doing Base64 conversions.
        /// </param>
        /// <param name="preserveSplits">Whether or not to split on the <paramref name="splitCharacter"/> value.</param>
        /// <param name="splitCharacter">
        /// The character to split the <paramref name="value"/> string into individual segments that each get Base64 decoded
        /// and tied back together again by preserving the split character values when joining back the segments.
        /// </param>
        /// <returns>A base 64 decoded string.</returns>
        public static string FromBase64String(this string value, Encoding encoding, bool trimPadding = true, char padCharacter = DefaultPadCharacter,
            bool preserveSplits = true, char splitCharacter = DefaultSplitCharacter)
        {
            // Do basic validation check, and return early if there is no string value to process.
            if (string.IsNullOrEmpty(value)) return value;

            // Setup builders.
            var builder = new StringBuilder();
            var trimCharacters = new[] { DefaultPadCharacter, LegacyPadCharacter, padCharacter };

            // Perform early padding trim of value.
            var unpaddedValue = trimPadding ? value.TrimEnd(trimCharacters) : value;
            // Check and see if we still have a valid value.
            if (string.IsNullOrEmpty(unpaddedValue)) return unpaddedValue;

            // If the user wants to preserve any split separators that may exist in the Base64 encoded value, then we
            // will need to do Base64 decoding on all sub-string parts individually after splitting the string values up.
            // For the last sub-string part, we may need to cleanup for the end padding, by trimming the end of the value
            // string for the specified padding characters. Once values are decoded and cleaned, we will want to join all
            // of the string parts back together, using the split character value between each segment.
            if (preserveSplits && unpaddedValue.Contains(splitCharacter, StringComparison.Ordinal))
            {
                // Process segments and preserve the split character values.
                var segments = unpaddedValue.Split(new[] { splitCharacter }, StringSplitOptions.RemoveEmptyEntries).ToList();
                // If there are any segments go ahead and Base64 decode each segment value and join these values back
                // together with the split character.
                if (segments.Any())
                    builder = builder.AppendJoin(splitCharacter, segments.Select(
                        segment => encoding.GetString(Convert.FromBase64String(segment.ToByteWidthPaddedString()))));
            }
            else
            {
                // Base64 decode the unpadded value.
                builder.Append(encoding.GetString(Convert.FromBase64String(unpaddedValue.ToByteWidthPaddedString())));
            }

            return builder.ToString();
        }
    }
}
