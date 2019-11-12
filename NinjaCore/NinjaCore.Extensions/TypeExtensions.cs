using System;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Type extension methods to help validate type conversions and identify compatibility.
    /// </summary>
    /// <remarks>
    /// This class resolves issues as seen on the following urls:
    /// https://stackoverflow.com/questions/1399273/test-if-convert-changetype-will-work-between-two-types/4102028
    /// </remarks>
    public static class TypeExtensions
    {
        public static bool IsConvertibleType<T>(this T value, Type conversionType)
        {
            if (value == null) return false;
            if (conversionType == null) return false;
            return value is IConvertible;
        }
    }
}
