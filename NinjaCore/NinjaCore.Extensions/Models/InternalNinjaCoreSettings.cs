using System.Text;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions.Models
{
    /// <summary>
    /// The <seealso cref="NinjaCore"/> framework internal developer settings preference object.
    /// This object is a completely non null facade to use for obtaining a current view model of
    /// active <seealso cref="NinjaCore"/> framework settings.
    /// </summary>
    internal class InternalNinjaCoreSettings
    {
        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> internal instance specific developer preference setting for <see cref="BoundsMode"/>.
        /// </summary>
        internal BoundsMode BoundsMode { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> internal instance specific developer preference setting for <see cref="ClearAfterUse"/>.
        /// </summary>
        internal bool ClearAfterUse { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> internal instance specific developer preference setting for <see cref="Encoding"/>.
        /// </summary>
        internal Encoding Encoding { get; set; }
    }
}
