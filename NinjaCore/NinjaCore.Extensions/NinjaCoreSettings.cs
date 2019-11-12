using System.Text;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Ninja Core User Settings Object.
    /// </summary>
    public class NinjaCoreSettings
    {
        /// <summary>
        /// The global default setting for <seealso cref="BoundsMode" />.
        /// </summary>
        public static BoundsMode? DefaultBoundsMode { get; set; }

        /// <summary>
        /// The global default setting for <seealso cref="ClearAfterUse" />.
        /// </summary>
        public static bool? DefaultClearAfterUse { get; set; }

        /// <summary>
        /// The global default setting for <seealso cref="Encoding" />.
        /// </summary>
        public static Encoding DefaultEncoding { get; set; }
        
        /// <summary>
        /// The instance specific <see cref="BoundsMode"/> override of <see cref="DefaultBoundsMode"/>.
        /// </summary>
        public BoundsMode? BoundsMode { get; set; }

        /// <summary>
        /// The instance specific <see cref="ClearAfterUse"/> override of <see cref="ClearAfterUse"/>.
        /// </summary>
        public bool? ClearAfterUse { get; set; }

        /// <summary>
        /// The instance specific <see cref="Encoding"/> override of <see cref="DefaultEncoding"/>.
        /// </summary>
        public Encoding Encoding { get; set; }

        public NinjaCoreSettings()
        {
            BoundsMode = null;
            ClearAfterUse = null;
            Encoding = null;
        }
    }
}
