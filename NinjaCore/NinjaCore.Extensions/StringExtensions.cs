using System;
using System.Text;

namespace NinjaCore.Extensions
{
    public static class StringExtensions
    {
        public static string ToBase64String(this string value)
        {
            // TODO: pull GetBytes out and add option to clear byte array afterwards.
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static string ToBase64String(this string value, Encoding encoding)
        {
            // TODO: pull GetBytes out and add option to clear byte array afterwards.
            return Convert.ToBase64String(encoding.GetBytes(value));
        }

        public static string FromBase64String(this string value)
        {
            // TODO: pull GetBytes out and add option to clear byte array afterwards.
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }

        public static string FromBase64String(this string value, Encoding encoding)
        {
            // TODO: pull GetBytes out and add option to clear byte array afterwards.
            return encoding.GetString(Convert.FromBase64String(value));
        }

        public static bool IsValidDate(this string value)
        {
            return DateTime.TryParse(value, out _);
        }

        public static bool IsValidNinjaDate(this string value, bool? clearAfterUse = null, NinjaCoreSettings settings = null)
        {
            var exceptionThrown = false;
            var internalSettings = settings.GetInternalSettings(null, null, clearAfterUse);
            clearAfterUse = internalSettings.ClearAfterUse;
            DateTime dateTime;

            try
            {
                return DateTime.TryParse(value, out dateTime);
            }
            catch (Exception)
            {
                exceptionThrown = true;
                // Clear critical values.
                if (!clearAfterUse.Value) throw;

                dateTime = default;
                value = default;

                throw;
            }
            finally
            {
                // Clear critical values.
                if (!exceptionThrown && clearAfterUse.Value)
                {
                    dateTime = default;
                    value = default;
                }
            }
        }
    }
}
