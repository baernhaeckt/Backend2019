using System;
using System.Security.Cryptography;
using Backend.Core.Features.UserManagement.Security.Abstraction;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Backend.Core.Features.UserManagement.Security
{
    public class HmacSha512PasswordStorage : IPasswordStorage
    {
        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;
        public const KeyDerivationPrf Key_Derivation_Prf = KeyDerivationPrf.HMACSHA512;

        // These constants define the encoding and may not be changed.
        public const int HASH_SECTIONS = 5;
        public const int HASH_ALGORITHM_INDEX = 0;
        public const int ITERATION_INDEX = 1;
        public const int HASH_SIZE_INDEX = 2;
        public const int SALT_INDEX = 3;
        public const int PBKDF2_INDEX = 4;

        public string Create(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[SALT_BYTES];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Generate(password, salt, PBKDF2_ITERATIONS, Key_Derivation_Prf, HASH_BYTES);

            // format: algorithm:iterations:hashSize:salt:hash
            string parts =
                Key_Derivation_Prf.ToString("F") +
                ":" +
                PBKDF2_ITERATIONS +
                ":" +
                hash.Length +
                ":" +
                Convert.ToBase64String(salt) +
                ":" +
                Convert.ToBase64String(hash);
            return parts;
        }

        public bool Match(string password, string goodHash)
        {
            char[] delimiter = { ':' };
            string[] split = goodHash.Split(delimiter);

            if (split.Length != HASH_SECTIONS)
            {
                throw new InvalidHashException(
                    "Fields are missing from the password hash."
                );
            }

            var keyDerivationPrf = (KeyDerivationPrf)Enum.Parse(typeof(KeyDerivationPrf), split[HASH_ALGORITHM_INDEX]);

            int iterations;
            try
            {
                iterations = int.Parse(split[ITERATION_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Int32.Parse",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Could not parse the iteration count as an integer.",
                    ex
                );
            }
            catch (OverflowException ex)
            {
                throw new InvalidHashException(
                    "The iteration count is too large to be represented.",
                    ex
                );
            }

            if (iterations < 1)
            {
                throw new InvalidHashException(
                    "Invalid number of iterations. Must be >= 1."
                );
            }

            byte[] salt;
            try
            {
                salt = Convert.FromBase64String(split[SALT_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of salt failed.",
                    ex
                );
            }

            byte[] hash;
            try
            {
                hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Convert.FromBase64String",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Base64 decoding of pbkdf2 output failed.",
                    ex
                );
            }

            int storedHashSize;
            try
            {
                storedHashSize = int.Parse(split[HASH_SIZE_INDEX]);
            }
            catch (ArgumentNullException ex)
            {
                throw new CannotPerformOperationException(
                    "Invalid argument given to Int32.Parse",
                    ex
                );
            }
            catch (FormatException ex)
            {
                throw new InvalidHashException(
                    "Could not parse the hash size as an integer.",
                    ex
                );
            }
            catch (OverflowException ex)
            {
                throw new InvalidHashException(
                    "The hash size is too large to be represented.",
                    ex
                );
            }

            if (storedHashSize != hash.Length)
            {
                throw new InvalidHashException(
                    "Hash length doesn't match stored hash length."
                );
            }

            byte[] testHash = Generate(password, salt, iterations, keyDerivationPrf, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] Generate(string password, byte[] salt, int iterations, KeyDerivationPrf keyDerivationPrf, int outputBytes)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: keyDerivationPrf,
                iterationCount: iterations,
                numBytesRequested: outputBytes);
        }
    }
}
