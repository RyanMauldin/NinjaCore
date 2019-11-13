using System.Text;
using NinjaCore.Extensions.Models;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// The <seealso cref="NinjaCore"/> developer settings extension object. 
    /// </summary>
    public static class NinjaCoreSettingExtensions
    {
        /// <summary>
        /// Clones a <seealso cref="NinjaCoreSettings"/> object.
        /// </summary>
        /// <param name="settings">The settings to clone.</param>
        /// <returns>Null if null, otherwise a new settings object.</returns>
        public static NinjaCoreSettings Clone(this NinjaCoreSettings settings)
        {
            if (settings == null)
                return null;

            return new NinjaCoreSettings
            {
                BoundsMode = settings.BoundsMode,
                ClearAfterUse = settings.ClearAfterUse,
                Encoding = settings.Encoding
            };
        }

        /// <summary>
        /// Produces a <seealso cref="NinjaCoreSettings"/> object mapped to applied settings values
        /// as if we were passed the given parameters as arguments to analyze.
        /// </summary>
        /// <param name="settings">The settings object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="boundsMode">The bonus mode value.</param>
        /// <param name="clearAfterUse">
        /// Whether or not to attempt to clear critical data values by setting them to default values,
        /// after the method invocation finishes and in the event an exception occurs.
        /// </param>
        /// <returns></returns>
        internal static NinjaCoreSettings GetSettings(this NinjaCoreSettings settings, Encoding encoding = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null)
        {
            if (settings == null)
                return null;

            return new NinjaCoreSettings
            {
                BoundsMode = boundsMode ?? settings.BoundsMode ?? NinjaCoreSettings.DefaultBoundsMode
                    ?? NinjaCoreSettings.InternalBoundsMode,
                ClearAfterUse = clearAfterUse ?? settings.ClearAfterUse ?? NinjaCoreSettings.DefaultClearAfterUse
                    ?? NinjaCoreSettings.InternalClearAfterUse,
                Encoding = encoding ?? settings.Encoding ?? NinjaCoreSettings.DefaultEncoding
                    ?? NinjaCoreSettings.InternalEncoding
            };
        }

        /// <summary>
        /// Produces a <seealso cref="NinjaCoreSettings"/> object mapped to applied settings values
        /// as if we were passed the given parameters as arguments to analyze.
        /// </summary>
        /// <param name="settings">The settings object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="boundsMode">The bonus mode value.</param>
        /// <param name="clearAfterUse">
        /// Whether or not to attempt to clear critical data values by setting them to default values,
        /// after the method invocation finishes and in the event an exception occurs.
        /// </param>
        /// <returns></returns>
        internal static InternalNinjaCoreSettings GetInternalSettings(this NinjaCoreSettings settings, Encoding encoding = null,
            BoundsMode? boundsMode = null, bool? clearAfterUse = null)
        {
            return new InternalNinjaCoreSettings
            {
                BoundsMode = boundsMode ?? settings?.BoundsMode ?? NinjaCoreSettings.DefaultBoundsMode
                    ?? NinjaCoreSettings.InternalBoundsMode,
                ClearAfterUse = clearAfterUse ?? settings?.ClearAfterUse ?? NinjaCoreSettings.DefaultClearAfterUse
                    ?? NinjaCoreSettings.InternalClearAfterUse,
                Encoding = encoding ?? settings?.Encoding ?? NinjaCoreSettings.DefaultEncoding
                    ?? NinjaCoreSettings.InternalEncoding
            };
        }
    }
}
