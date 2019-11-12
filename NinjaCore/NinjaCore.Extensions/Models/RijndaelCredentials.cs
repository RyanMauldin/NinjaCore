using System;
using NinjaCore.Extensions.Abstractions;

namespace NinjaCore.Extensions.Models
{
    public class RijndaelCredentials : EncryptionCredentials
    {
        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public byte[] InitialVector { get; set; }

        public int PasswordIterations { get; set; }

        public int KeySize { get; set; }

        /// <summary>
        /// Code list
        /// </summary>
        /// <inheritdoc />
        public override void OnDisposing()
        {
            // Add code here to execute during dispose from IDisposable
        }
    }
}