using System;
using System.Linq;
using System.Text;
using NinjaCore.Extensions.Models;

namespace NinjaCore.Extensions
{
    internal static class BoundsExtensions
    {
        internal static readonly string DefaultErrorMessage = "An error occured while checking bounds";
        
        internal static Exception ToException(this Bounds bounds)
        {
            var builder = new StringBuilder(string.IsNullOrWhiteSpace(bounds?.ArgumentName)
                ? $"{DefaultErrorMessage}." : $"{DefaultErrorMessage} for {bounds.ArgumentName}.");

            if (bounds?.InvalidBounds != null && bounds.InvalidBounds.Count >= 0)
                builder = builder.Append(" ").AppendJoin(" ", bounds.InvalidBounds.Select(
                    invalidBounds => $"{invalidBounds.ArgumentName}: {invalidBounds.ErrorMessage}"));
            
            return string.IsNullOrWhiteSpace(bounds?.ArgumentName)
                ? new ArgumentException(builder.ToString())
                : new ArgumentException(builder.ToString(), bounds.ArgumentName);
        }
    }
}
