using System;

namespace NinjaCore.Extensions.Abstractions
{
    public abstract class EncryptionCredentials : IEncryptionCredentials
    {
        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public byte[] InitialVector { get; set; }

        public int PasswordIterations { get; set; }

        public int KeySize { get; set; }

        /// <summary>
        /// The <see cref="OnDisposing"/> method is useful for clearing or zeroing out sensitive data in custom fields
        /// or properties of <seealso cref="EncryptionCredentials"/> derived objects at the end of their lifecycle, where
        /// the custom fields or properties are not already described by the <see cref="IEncryptionCredentials"/> interface.
        /// <remarks>
        /// Objects deriving from <seealso cref="EncryptionCredentials"/> should be fetched only when authentication needs
        /// to happen, and then should be immediately cleared and discarded as to not save the object around for a whole
        /// entire user session and not for the lifetime of the entire application. Being conscious of how parameters are
        /// passed around By Value or By Reference and then trying to code in such a way that sensitive values do not get
        /// stuck in plain text form by having this array value getting copied plain text when the method getting called,
        /// receives a cloned copy of the array value, when getting passed across method call boundaries when passed By
        /// Value as parameters values that are copied and you end up with multiple copies of this plain text value at the
        /// same time, passed By Value creating multiple copies, and values getting stuck around, waiting onto long running
        /// operations to finish, where flow of control will not be able to manually call or handle a dispose until the long
        /// running process has finished. Credential type objects hold plain text values at times with critical data, and
        /// the goal is to not get the values large decrypted string values stuck in gen2 garbage collection as plain text
        /// and decrypted values have a chance of getting memory scrapped when swapping to and from the heap to the stack.
        /// The gen2 garbage collection is a full garbage collection sweep and takes the longest to perform, removing larger
        /// objects from memory and returning the freed up system memory back to the operating system. Sometimes gen2 can
        /// start getting frequents under intense system load and to combat credentials from being memory scraped. Zeroing
        /// out data by the <code>using(var ...)</code> statements is an easy way to reduce risks for this but aren't
        /// perfect. The <see cref="OnDisposing"/> method gets called directly from the base method
        /// <seealso cref="EncryptionCredentials.Dispose()"/>, which implements the <seealso cref="IDisposable"/>
        /// interface method <seealso cref="IDisposable.Dispose()"/>. When <seealso cref="EncryptionCredentials"/> derived
        /// objects are wrapped in <code>using</code> statements, the <seealso cref="EncryptionCredentials.Dispose()"/>
        /// method gets automatically called at the end of the <code>using(var ...)</code> code block. In turn the
        /// <seealso cref="EncryptionCredentials.Dispose()"/> method calls the <see cref="OnDisposing"/> method in the
        /// derived and active code base.
        /// </remarks>
        /// </summary>
        public abstract void OnDisposing();

        /// <summary>
        /// Implements <seealso cref="IDisposable.Dispose()"/> 
        /// </summary>
        public void Dispose()
        {
            
            // Clear data
            try
            {
                Password.TryClear(clearAfterUse: true);
                Salt.TryClear(clearAfterUse: true);
                InitialVector.TryClear(clearAfterUse: true);
                PasswordIterations = 0;
                KeySize = 0;
            }
            catch (Exception) 
            {
                // ignored
            }

            OnDisposing();
        }
    }
}
