using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NinjaCore.Extensions.Models;

namespace NinjaCore.Extensions
{
    /// <summary>
    /// Extension methods to take advantage of AES encryption.
    /// </summary>
    public static class AesEncryptionExtensions
    {
        private static void CredentialCleanup(AesEncryptionCredentials credentials)
        {
            credentials?.Clear();
        }

        public static byte[] ToAesEncryptedBytes(this byte[] value, AesEncryptionCredentials credentials)
        {
            return Encrypt(value, credentials);
        }

        public static string ToAesEncryptedString(this string value, AesEncryptionCredentials credentials)
        {
            return Encrypt(value, credentials);
        }

        public static async Task<string> ToAesEncryptedStringAsync(this string value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await EncryptAsync(value, credentials, cancellationToken);
        }

        public static async Task<byte[]> ToAesEncryptedBytesAsync(this byte[] value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await EncryptAsync(value, credentials, cancellationToken);
        }

        public static string ToAesDecryptedString(this string value, AesEncryptionCredentials credentials)
        {
            return Decrypt(value, credentials);
        }

        public static byte[] ToAesDecryptedBytes(this byte[] value, AesEncryptionCredentials credentials)
        {
            return Decrypt(value, credentials);
        }

        public static async Task<string> ToAesDecryptedStringAsync(this string value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await DecryptAsync(value, credentials, cancellationToken);
        }

        public static async Task<byte[]> ToAesDecryptedBytesAsync(this byte[] value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await DecryptAsync(value, credentials, cancellationToken);
        }

        public static string Encrypt(string value, AesEncryptionCredentials credentials)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var encryptor = rijndaelManaged.CreateEncryptor(derivedBytes, initialVectorBytes);
                    using var memoryStream = new MemoryStream();
                    using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    valueBytes = Encoding.UTF8.GetBytes(value);
                    cryptoStream.Write(valueBytes, 0, valueBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    buffer = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Convert.ToBase64String(buffer);
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static byte[] Encrypt(byte[] value, AesEncryptionCredentials credentials)
        {
            try
            {
                if (value == null || value.Length == 0)
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var encryptor = rijndaelManaged.CreateEncryptor(derivedBytes, initialVectorBytes);
                    using var memoryStream = new MemoryStream();
                    using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    cryptoStream.Write(value, 0, value.Length);
                    cryptoStream.FlushFinalBlock();
                    buffer = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Encoding.ASCII.GetBytes(Convert.ToBase64String(buffer));
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (value.Any())
                        Array.Clear(value, 0, value.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static async Task<string> EncryptAsync(string value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var encryptor = rijndaelManaged.CreateEncryptor(derivedBytes, initialVectorBytes);
                    await using var memoryStream = new MemoryStream();
                    await using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    valueBytes = Encoding.UTF8.GetBytes(value);
                    await cryptoStream.WriteAsync(valueBytes, 0, valueBytes.Length, cancellationToken);
                    cryptoStream.FlushFinalBlock();
                    buffer = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Convert.ToBase64String(buffer);
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static async Task<byte[]> EncryptAsync(byte[] value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                if (value == null || value.Length == 0)
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var encryptor = rijndaelManaged.CreateEncryptor(derivedBytes, initialVectorBytes);
                    await using var memoryStream = new MemoryStream();
                    await using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                    await cryptoStream.WriteAsync(value, 0, value.Length, cancellationToken);
                    cryptoStream.FlushFinalBlock();
                    buffer = memoryStream.ToArray();
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Encoding.ASCII.GetBytes(Convert.ToBase64String(buffer));
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (value.Any())
                        Array.Clear(value, 0, value.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static string Decrypt(string value, AesEncryptionCredentials credentials)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    valueBytes = Convert.FromBase64String(value);
                    buffer = new byte[valueBytes.Length];
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var decryptor = rijndaelManaged.CreateDecryptor(derivedBytes, initialVectorBytes);
                    using var memoryStream = new MemoryStream(valueBytes);
                    using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                    var count = cryptoStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Encoding.UTF8.GetString(buffer, 0, count);
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static byte[] Decrypt(byte[] value, AesEncryptionCredentials credentials)
        {
            try
            {
                if (value == null || value.Length == 0)
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    valueBytes = Convert.FromBase64String(Encoding.ASCII.GetString(value));
                    buffer = new byte[valueBytes.Length];
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var decryptor = rijndaelManaged.CreateDecryptor(derivedBytes, initialVectorBytes);
                    using var memoryStream = new MemoryStream(valueBytes);
                    using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                    var count = cryptoStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    Array.Resize(ref buffer, count);
                    return buffer;
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static async Task<string> DecryptAsync(string value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                if (string.IsNullOrEmpty(value))
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    valueBytes = Convert.FromBase64String(value);
                    buffer = new byte[valueBytes.Length];
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var decryptor = rijndaelManaged.CreateDecryptor(derivedBytes, initialVectorBytes);
                    await using var memoryStream = new MemoryStream(valueBytes);
                    await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                    var count = await cryptoStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    return Encoding.UTF8.GetString(buffer, 0, count);
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }

        public static async Task<byte[]> DecryptAsync(byte[] value, AesEncryptionCredentials credentials,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                if (value == null || value.Length == 0)
                    return value;

                byte[] passwordBytes = null;
                byte[] saltBytes = null;
                byte[] initialVectorBytes = null;
                byte[] derivedBytes = null;
                byte[] valueBytes = null;
                byte[] buffer = null;

                try
                {
                    passwordBytes = credentials.Password.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    saltBytes = credentials.Salt.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    initialVectorBytes = credentials.InitialVector.ToByteArray(Encoding.ASCII, clearOnException: true, clearAfterUse: true);
                    var passwordIterations = credentials.PasswordIterations;
                    var keySize = credentials.KeySize;
                    valueBytes = Convert.FromBase64String(Encoding.ASCII.GetString(value));
                    buffer = new byte[valueBytes.Length];
                    using var rfcDeriveBytes = new Rfc2898DeriveBytes(passwordBytes, saltBytes, passwordIterations);
                    using var rijndaelManaged = new RijndaelManaged { Mode = CipherMode.CBC };
                    derivedBytes = rfcDeriveBytes.GetBytes(keySize / 8);
                    using var decryptor = rijndaelManaged.CreateDecryptor(derivedBytes, initialVectorBytes);
                    await using var memoryStream = new MemoryStream(valueBytes);
                    await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                    var count = await cryptoStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    memoryStream.Close();
                    cryptoStream.Close();
                    rijndaelManaged.Clear();
                    Array.Resize(ref buffer, count);
                    return buffer;
                }
                finally
                {
                    if (buffer != null && buffer.Any())
                        Array.Clear(buffer, 0, buffer.Length);
                    if (passwordBytes != null && passwordBytes.Any())
                        Array.Clear(passwordBytes, 0, passwordBytes.Length);
                    if (valueBytes != null && valueBytes.Any())
                        Array.Clear(valueBytes, 0, valueBytes.Length);
                    if (saltBytes != null && saltBytes.Any())
                        Array.Clear(saltBytes, 0, saltBytes.Length);
                    if (initialVectorBytes != null && initialVectorBytes.Any())
                        Array.Clear(initialVectorBytes, 0, initialVectorBytes.Length);
                    if (derivedBytes != null && derivedBytes.Any())
                        Array.Clear(derivedBytes, 0, derivedBytes.Length);
                }
            }
            finally
            {
                CredentialCleanup(credentials);
            }
        }
    }
}