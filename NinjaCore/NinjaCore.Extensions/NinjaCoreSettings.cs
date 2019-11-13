using System.Text;
using NinjaCore.Extensions.Types;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// The <seealso cref="NinjaCore"/> framework developer settings preference object. The <see cref="NinjaCoreSettings"/>
    /// object is inherently used with many <seealso cref="NinjaCore"/> framework extension method calls. The
    /// <see cref="NinjaCoreSettings"/> object provides <seealso cref="NinjaCore"/> framework developers with overridable
    /// global default settings values for easy runtime configuration. The <see cref="NinjaCoreSettings"/> object can
    /// additionally be created as an instance object, and then be passed in as an optional parameter, to all
    /// <seealso cref="NinjaCore"/> framework extension methods signatures.
    /// </summary>
    /// <remarks>
    /// Having the <see cref="NinjaCoreSettings"/> object available as an optional parameter to every <seealso cref="NinjaCore"/>
    /// framework extension method signature is a critical and necessary facet for developer friendliness. One such reason is for
    /// developer friendliness of library naming and design conventions. When <seealso cref="NinjaCore"/> framework packages
    /// get included in other software development projects, there is always potential for method signature collisions.
    /// Having the <seealso cref="NinjaCore"/> framework method signatures to all adopt the optional <see cref="NinjaCoreSettings"/>
    /// object as the final parameter, will make all <seealso cref="NinjaCore"/> framework method signatures standout
    /// and become instantly identifiable within intellisense amongst identically named method signatures. Another
    /// monumental reason for including the optional <see cref="NinjaCoreSettings"/> parameter in all method signatures,
    /// is that this facilitates developers with the ability to constrain differing <seealso cref="NinjaCore"/> framework
    /// modes and behaviors in isolation of each other on a per extension method call basis, all while being able to
    /// leave the global default settings values alone. The <seealso cref="NinjaCore"/> framework settings area applied
    /// in a deterministic order of precedence to achieve this separation of concerns. Nullable parameters, nullable
    /// developer instance object settings, nullable developer global default settings, and non null, internal global
    /// default property settings values are applied in a dependable order, where settings with null values, fallback to
    /// settings with lower precedence. When <seealso cref="NinjaCore"/> framework setting values are null, this indicates
    /// that the developer has not provided a settings value, and that it should be ignored and yielded to the next setting
    /// in order of precedence. The first and highest settings precedence value is held by <seealso cref="NinjaCore"/> framework
    /// extension method parameters, where the parameter names match the <see cref="NinjaCoreSettings"/> instance object
    /// property names. The second highest settings precedence is given to <see cref="NinjaCoreSettings"/> instance object
    /// properties, which undoubtedly makes sense, as the instance object itself is passed around as a parameter in
    /// <seealso cref="NinjaCore"/> framework extension method signatures. The third highest order of precedence is given to
    /// <see cref="NinjaCoreSettings"/> static global default object properties, prefixed with the word <value>'Default'</value>,
    /// which match the see <see cref="NinjaCoreSettings"/> instance object property names when used without the prefix.
    /// The final <seealso cref="NinjaCore"/> framework settings precedence is given to the non null internal framework global
    /// default property values found in the <seealso cref="NinjaCoreSettings"/> object. All <seealso cref="NinjaCore"/> framework
    /// settings found on the <see cref="NinjaCoreSettings"/> object, can be modified on the fly, without effecting the execution
    /// modes and behaviors in concurrent running extension method contexts. At the beginning of every
    /// <seealso cref="NinjaCore"/> framework extension method invocation, <see cref="NinjaCoreSettings"/> values are
    /// evaluated by order of precedence, and the resulting effective settings values are assigned to local variables
    /// within the scope of the invoked extension method's current execution context. This small bit of up front
    /// settings validation, safeguards <seealso cref="NinjaCore"/> framework extension methods from unintended, out of
    /// process, logic behavior changes, due to shared settings object modifications.
    /// </remarks>
    public class NinjaCoreSettings
    {
        /// <summary>
        /// The see cref="NinjaCoreSettings"/> internal global default <seealso cref="BoundsMode"/> framework preference setting for
        /// <see cref="InternalBoundsMode" />. The internal <seealso cref="NinjaCore"/> framework global default setting for
        /// <see cref="InternalBoundsMode" /> is the value <value>Encoding.UTF8</value>. This property value is only globally
        /// active when both <seealso cref="DefaultBoundsMode"/> and <seealso cref="BoundsMode"/> properties are null and no matching
        /// extension parameter name was found matching the name <seealso cref="BoundsMode"/>.
        /// </summary>
        public static BoundsMode InternalBoundsMode { get; }

        /// <summary>
        /// The see cref="NinjaCoreSettings"/> internal global default <seealso cref="NinjaCore"/> framework preference setting for
        /// <see cref="InternalClearAfterUse" />. The internal <seealso cref="NinjaCore"/> framework global default setting for
        /// <see cref="InternalClearAfterUse" /> is the value <value>Encoding.UTF8</value>. This property value is only globally
        /// active when both <seealso cref="DefaultClearAfterUse"/> and <seealso cref="ClearAfterUse"/> properties are null and no matching
        /// extension parameter name was found matching the name <seealso cref="ClearAfterUse"/>.
        /// </summary>
        public static bool InternalClearAfterUse { get; }

        /// <summary>
        /// The see cref="NinjaCoreSettings"/> internal global default <seealso cref="NinjaCore"/> framework preference setting for
        /// <see cref="InternalEncoding" />. The internal <seealso cref="NinjaCore"/> framework global default setting for
        /// <see cref="InternalEncoding" /> is the value <value>Encoding.UTF8</value>. This property value is only globally
        /// active when both <seealso cref="DefaultEncoding"/> and <seealso cref="Encoding"/> properties are null and no matching
        /// extension parameter name was found matching the name <seealso cref="Encoding"/>.
        /// </summary>
        public static Encoding InternalEncoding { get; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> global default developer preference setting for <see cref="DefaultBoundsMode"/>.
        /// This property value takes precedence over the <see cref="InternalBoundsMode"/> property value. The <see cref="BoundsMode"/>
        /// setting take precedence over this <see cref="DefaultBoundsMode"/> setting when <see cref="BoundsMode"/> holds a non null value.
        /// </summary>
        public static BoundsMode? DefaultBoundsMode { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> global default developer preference setting for <see cref="DefaultClearAfterUse"/>.
        /// This property value takes precedence over the <see cref="InternalClearAfterUse"/> property value. The <see cref="ClearAfterUse"/>
        /// setting take precedence over this <see cref="DefaultClearAfterUse"/> setting when <see cref="ClearAfterUse"/> holds a non null value.
        /// </summary>
        public static bool? DefaultClearAfterUse { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> global default developer preference setting for <see cref="DefaultEncoding"/>.
        /// This property value takes precedence over the <see cref="InternalEncoding"/> property value. The <see cref="Encoding"/>
        /// setting take precedence over this <see cref="DefaultEncoding"/> setting when <see cref="Encoding"/> holds a non null value.
        /// </summary>
        public static Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> instance specific developer preference setting for <see cref="BoundsMode"/>.
        /// This property value takes precedence over the <see cref="DefaultBoundsMode"/> and <see cref="InternalBoundsMode" />
        /// property values respectively. When the <see cref="NinjaCoreSettings"/> instance object is passed into <seealso cref="NinjaCore"/>
        /// framework extension method parameter, the only value that can take precedence over this <see cref="BoundsMode"/> setting
        /// value is a non null extension method parameter value named to match this setting.
        /// </summary>
        public BoundsMode? BoundsMode { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> instance specific developer preference setting for <see cref="ClearAfterUse"/>.
        /// This property value takes precedence over the <see cref="DefaultClearAfterUse"/> and <see cref="InternalClearAfterUse" />
        /// property values respectively. When the <see cref="NinjaCoreSettings"/> instance object is passed into <seealso cref="NinjaCore"/>
        /// framework extension method parameter, the only value that can take precedence over this <see cref="ClearAfterUse"/> setting
        /// value is a non null extension method parameter value named to match this setting.
        /// </summary>
        public bool? ClearAfterUse { get; set; }

        /// <summary>
        /// The <see cref="NinjaCoreSettings"/> instance specific developer preference setting for <see cref="Encoding"/>.
        /// This property value takes precedence over the <see cref="DefaultEncoding"/> and <see cref="InternalEncoding" />
        /// property values respectively. When the <see cref="NinjaCoreSettings"/> instance object is passed into <seealso cref="NinjaCore"/>
        /// framework extension method parameter, the only value that can take precedence over this <see cref="Encoding"/> setting
        /// value is a non null extension method parameter value named to match this setting.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The static constructor.
        /// </summary>
        static NinjaCoreSettings()
        {
            InternalBoundsMode = Types.BoundsMode.Ninja;
            InternalClearAfterUse = false;
            InternalEncoding = Encoding.UTF8;
            DefaultBoundsMode = null;
            DefaultClearAfterUse = null;
            DefaultEncoding = null;
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public NinjaCoreSettings()
        {
            BoundsMode = null;
            ClearAfterUse = null;
            Encoding = null;
        }
    }
}