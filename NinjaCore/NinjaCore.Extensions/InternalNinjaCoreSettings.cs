using System.Text;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Ninja Core Internal User Settings Object.
    /// </summary>
    internal static class InternalNinjaCoreSettings
    {
        private static readonly BoundsMode DefaultBoundsMode;
        private static readonly bool DefaultClearAfterUse;
        private static readonly Encoding DefaultEncoding;
        
        static InternalNinjaCoreSettings()
        {
            DefaultBoundsMode = BoundsMode.Ninja;
            DefaultClearAfterUse = false;
            DefaultEncoding = Encoding.UTF8;
        }

        internal static BoundsMode GetBoundsMode(NinjaCoreSettings ninjaCoreSettings = null, BoundsMode? boundsMode = null)
        {
            return boundsMode ?? ninjaCoreSettings?.BoundsMode
                   ?? NinjaCoreSettings.DefaultBoundsMode ?? DefaultBoundsMode;
        }

        internal static bool GetClearAfterUse(NinjaCoreSettings ninjaCoreSettings = null, bool? clearAfterUse = null)
        {
            return clearAfterUse ?? ninjaCoreSettings?.ClearAfterUse
                   ?? NinjaCoreSettings.DefaultClearAfterUse?? DefaultClearAfterUse;
        }
        
        internal static Encoding GetEncoding(NinjaCoreSettings ninjaCoreSettings = null, Encoding encoding = null)
        {
            return encoding ?? ninjaCoreSettings?.Encoding
                   ?? NinjaCoreSettings.DefaultEncoding ?? DefaultEncoding;
        }
    }
}