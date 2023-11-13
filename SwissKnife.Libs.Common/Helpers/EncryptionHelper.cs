namespace SwissKnife.Libs.Common;
using System.Security.Cryptography;

public class EncryptionHelper
{
    #region AES
    /// <summary>
    /// Encrypts plain text using AES
    /// </summary>
    /// <param name="plainText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static byte[] AesEncrypt(string plainText, byte[] key, byte[] iv)
    {
        byte[] cipherText;

        using (var encryptor = Aes.Create().CreateEncryptor(key, iv))
        {
            using MemoryStream cipherStream = new();
            using CryptoStream aesStream = new(cipherStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter writer = new(aesStream))
            {
                writer.Write(plainText);
            }
            cipherText = cipherStream.ToArray();
        }

        return cipherText;
    }

    /// <summary>
    /// Decrypts plain text using AES
    /// </summary>
    /// <param name="cipherText"></param>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    public static string AesDecrypt(byte[] cipherText, byte[] key, byte[] iv)
    {
        string plainText;

        using (ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key, iv))
        {
            using MemoryStream cipherStream = new(cipherText);
            using CryptoStream aesStream = new(cipherStream, decryptor, CryptoStreamMode.Read);
            using StreamReader reader = new(aesStream);
            plainText = reader.ReadToEnd();
        }

        return plainText;
    }
    #endregion

    #region SHA
    /// <summary>
    /// Creates Hash using SHA
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static byte[] ShaHash(byte[] plainText)
    {
        byte[] cipherText;
        cipherText = SHA512.HashData(plainText);

        return cipherText;
    }
    #endregion

}
