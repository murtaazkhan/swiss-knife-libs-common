using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
namespace SwissKnife.Libs.Common.Helpers;

public static class PasswordHelper
{
    private static readonly char[] Punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();
    private static readonly char[] StartingChars = new char[] { '<', '&' };

    /// <summary>Generates a random password of the specified length.</summary>
    /// <returns>A random password of the specified length.</returns>
    /// <param name="length">The number of characters in the generated password. The length must be between 1 and 128 characters. </param>
    /// <param name="numberOfNonAlphanumericCharacters">The minimum number of non-alphanumeric characters (such as @, #, !, %, &amp;, and so on) in the generated password.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="length" /> is less than 1 or greater than 128 -or-<paramref name="numberOfNonAlphanumericCharacters" /> is less than 0 or greater than <paramref name="length" />. </exception>
    public static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
    {
        if (length < 1 || length > 128)
            throw new ArgumentException("password_length_incorrect", nameof(length));
        if (numberOfNonAlphanumericCharacters > length || numberOfNonAlphanumericCharacters < 0)
            throw new ArgumentException("min_required_non_alphanumeric_characters_incorrect", nameof(numberOfNonAlphanumericCharacters));
        string s;
        do
        {
            var data = new byte[length];
            var chArray = new char[length];
            var num1 = 0;
            RandomNumberGenerator.Create().GetBytes(data);
            for (var index = 0; index < length; ++index)
            {
                var num2 = (int)data[index] % 87;
                if (num2 < 10)
                    chArray[index] = (char)(48 + num2);
                else if (num2 < 36)
                    chArray[index] = (char)(65 + num2 - 10);
                else if (num2 < 62)
                {
                    chArray[index] = (char)(97 + num2 - 36);
                }
                else
                {
                    chArray[index] = Punctuations[num2 - 62];
                    ++num1;
                }
            }
            if (num1 < numberOfNonAlphanumericCharacters)
            {
                var random = new Random();
                for (var index1 = 0; index1 < numberOfNonAlphanumericCharacters - num1; ++index1)
                {
                    int index2;
                    do
                    {
                        index2 = random.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(chArray[index2]));
                    chArray[index2] = Punctuations[random.Next(0, Punctuations.Length)];
                }
            }
            s = new string(chArray);
        }
        while (IsDangerousString(s, out int matchIndex));
        return s;
    }

    /// <summary>
    /// Generated a hash of specified arguments
    /// </summary>
    /// <param name="password"></param>
    /// <param name="prf"></param>
    /// <param name="iterationCount"></param>
    /// <param name="saltSize"></param>
    /// <returns></returns>
    public static string GenerateHash(string password, KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256, int iterationCount = 10000, int saltSize = 16)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[saltSize];
        rng.GetBytes(salt);

        var pbkdf2Hash = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, 32);
        return Convert.ToBase64String(ComposeIdentityV3Hash(salt, (uint)iterationCount, pbkdf2Hash));
    }

    /// <summary>
    /// Verifies Hash
    /// </summary>
    /// <param name="password"></param>
    /// <param name="passwordHash"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static bool VerifyHash(string password, string passwordHash)
    {
        if (passwordHash != null)
        {
            var identityV3HashArray = Convert.FromBase64String(passwordHash);
            if (identityV3HashArray[0] != 1) throw new InvalidOperationException("passwordHash is not Identity V3");

            var prfAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 1, prfAsArray, 0, 4);
            var prf = (KeyDerivationPrf)ConvertFromNetworOrder(prfAsArray);

            var iterationCountAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 5, iterationCountAsArray, 0, 4);
            var iterationCount = (int)ConvertFromNetworOrder(iterationCountAsArray);

            var saltSizeAsArray = new byte[4];
            Buffer.BlockCopy(identityV3HashArray, 9, saltSizeAsArray, 0, 4);
            var saltSize = (int)ConvertFromNetworOrder(saltSizeAsArray);

            var salt = new byte[saltSize];
            Buffer.BlockCopy(identityV3HashArray, 13, salt, 0, saltSize);

            var savedHashedPassword = new byte[identityV3HashArray.Length - 1 - 4 - 4 - 4 - saltSize];
            Buffer.BlockCopy(identityV3HashArray, 13 + saltSize, savedHashedPassword, 0, savedHashedPassword.Length);

            var hashFromInputPassword = KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, 32);

            return AreByteArraysEqual(hashFromInputPassword, savedHashedPassword);
        }
        else
        {
            throw new InvalidOperationException("passwordHash is not Identity V3");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string GetSha256Hash(string input)
    {
        var byteValue = Encoding.UTF8.GetBytes(input);
        var byteHash = SHA256.HashData(byteValue);
        return Convert.ToBase64String(byteHash);
    }

    #region Private Methods
    private static bool IsDangerousString(string s, out int matchIndex)
    {
        //bool inComment = false;
        matchIndex = 0;

        for (var i = 0; ;)
        {

            // Look for the start of one of our patterns 
            var n = s.IndexOfAny(StartingChars, i);

            // If not found, the string is safe
            if (n < 0) return false;

            // If it's the last char, it's safe 
            if (n == s.Length - 1) return false;

            matchIndex = n;

            switch (s[n])
            {
                case '<':
                    // If the < is followed by a letter or '!', it's unsafe (looks like a tag or HTML comment)
                    if (IsAtoZ(s[n + 1]) || s[n + 1] == '!' || s[n + 1] == '/' || s[n + 1] == '?') return true;
                    break;
                case '&':
                    // If the & is followed by a #, it's unsafe (e.g. &#83;) 
                    if (s[n + 1] == '#') return true;
                    break;
            }

            // Continue searching
            i = n + 1;
        }
    }

    private static bool IsAtoZ(char c)
    {
        if ((int)c >= 97 && (int)c <= 122)
            return true;
        if ((int)c >= 65)
            return (int)c <= 90;
        return false;
    }
    
    private static byte[] ComposeIdentityV3Hash(byte[] salt, uint iterationCount, byte[] passwordHash)
    {
        var hash = new byte[1 + 4/*KeyDerivationPrf value*/ + 4/*Iteration count*/ + 4/*salt size*/ + salt.Length /*salt*/ + 32 /*password hash size*/];
        hash[0] = 1; //Identity V3 marker

        Buffer.BlockCopy(ConvertToNetworkOrder((uint)KeyDerivationPrf.HMACSHA256), 0, hash, 1, sizeof(uint));
        Buffer.BlockCopy(ConvertToNetworkOrder((uint)iterationCount), 0, hash, 1 + sizeof(uint), sizeof(uint));
        Buffer.BlockCopy(ConvertToNetworkOrder((uint)salt.Length), 0, hash, 1 + 2 * sizeof(uint), sizeof(uint));
        Buffer.BlockCopy(salt, 0, hash, 1 + 3 * sizeof(uint), salt.Length);
        Buffer.BlockCopy(passwordHash, 0, hash, 1 + 3 * sizeof(uint) + salt.Length, passwordHash.Length);

        return hash;
    }
	private static bool AreByteArraysEqual(byte[] array1, byte[] array2)
    {
        if (array1.Length != array2.Length) return false;

        var areEqual = true;
        for (var i = 0; i < array1.Length; i++)
        {
            areEqual &= (array1[i] == array2[i]);
        }
        //If you stop as soon as the arrays don't match you'll be disclosing information about how different they are by the time it takes to compare them
        //this way no information is disclosed
        return areEqual;
    }

    private static byte[] ConvertToNetworkOrder(uint number)
    {
        return BitConverter.GetBytes(number).Reverse().ToArray();
    }

    private static uint ConvertFromNetworOrder(byte[] reversedUint)
    {
        return BitConverter.ToUInt32(reversedUint.Reverse().ToArray(), 0);
    }

    #endregion
}
