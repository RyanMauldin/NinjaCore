using System;

namespace NinjaCore.Extensions.Models
{
    public class AesEncryptionCredentials : IDisposable
    {
        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        public byte[] InitialVector { get; set; }

        public int PasswordIterations { get; set; }

        public int KeySize { get; set; }

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            Password = Password.TryClear(clearAfterUse: true);
            Salt = Salt.TryClear(clearAfterUse: true);
            InitialVector = InitialVector.TryClear(clearAfterUse: true);
            PasswordIterations = 0;
            KeySize = 0;
        }
    }
}
