using System;
using System.Security.Cryptography;
using Backend.Core.Features.UserManagement.Security.Abstraction;

namespace Backend.Core.Features.UserManagement.Security
{
    public class RandomPasswordGenerator : IPasswordGenerator
    {
        private const int PasswordLength = 10;

        private const int NumberOfNonAlphanumericCharacters = 2;

        private static readonly char[] _punctuations = "!@#$%^&*()_-+=[{]};:>|./?".ToCharArray();

        public string Generate() => GeneratePassword(PasswordLength, NumberOfNonAlphanumericCharacters);

        private static string GeneratePassword(int length, int numberOfNonAlphanumericCharacters)
        {
            // https://referencesource.microsoft.com/#system.web/Security/Membership.cs,fe744ec40cace139,references

            string password;
            byte[] buf;
            char[] cBuf;
            int count;

            buf = new byte[length];
            cBuf = new char[length];
            count = 0;

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buf);
            }

            for (int iter = 0; iter < length; iter++)
            {
                int i = buf[iter] % 87;
                if (i < 10)
                {
                    cBuf[iter] = (char)('0' + i);
                }
                else if (i < 36)
                {
                    cBuf[iter] = (char)('A' + i - 10);
                }
                else if (i < 62)
                {
                    cBuf[iter] = (char)('a' + i - 36);
                }
                else
                {
                    cBuf[iter] = _punctuations[i - 62];
                    count++;
                }
            }

            if (count < numberOfNonAlphanumericCharacters)
            {
                int j, k;
                var rand = new Random();

                for (j = 0; j < numberOfNonAlphanumericCharacters - count; j++)
                {
                    do
                    {
                        k = rand.Next(0, length);
                    }
                    while (!char.IsLetterOrDigit(cBuf[k]));

                    cBuf[k] = _punctuations[rand.Next(0, _punctuations.Length)];
                }
            }

            password = new string(cBuf);

            return password;
        }
    }
}
