using System;

namespace NinjaCore.Extensions.Abstractions
{
    public interface IEncryptionCredentials : IDisposable
    {
        byte[] Password { get; set; }

        byte[] Salt { get; set; }

        byte[] InitialVector { get; set; }

        int PasswordIterations { get; set; }

        int KeySize { get; set; }
        
        void OnDisposing();
    }
}
