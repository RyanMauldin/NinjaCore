using System;
using System.Linq;
using System.Text;
using NinjaCore.Extensions.Models;

namespace NinjaCore.Extensions
{
    internal static class BoundsValidationResultExtensions
    {
        internal static readonly string DefaultErrorMessage = "An error occured while checking bounds";
        
        internal static Exception ToException(this BoundsValidationResult result, string argumentName)
        {
            var builder = new StringBuilder(string.IsNullOrWhiteSpace(argumentName)
                ? $"{DefaultErrorMessage}." : $"{DefaultErrorMessage} for {argumentName}.");

            if (result?.InvalidBounds != null && result.InvalidBounds.Count >= 0)
                builder = builder.Append(" ").AppendJoin(" ", result.InvalidBounds.Select(
                    invalidBounds => $"{invalidBounds.ArgumentName}: {invalidBounds.ErrorMessage}"));
            
            return string.IsNullOrWhiteSpace(argumentName)
                ? new ArgumentException(builder.ToString())
                : new ArgumentException(builder.ToString(), argumentName);
        }
    }
}
