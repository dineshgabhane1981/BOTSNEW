using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;


namespace DLC.App_Start
{
    public class Hash
    {
        public string HashPassword(string password)
        {
            const int iterCount = 10000;
            const int saltSize = 16; // 128 bits
            const int keySize = 32;  // 256 bits

            byte[] salt = new byte[saltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Use the constructor with 3 arguments: password, salt, and iteration count
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterCount))
            {
                byte[] subkey = deriveBytes.GetBytes(keySize); // Get the derived bytes (key)

                byte[] hash = new byte[saltSize + keySize]; // Combine salt and key
                Buffer.BlockCopy(salt, 0, hash, 0, saltSize);
                Buffer.BlockCopy(subkey, 0, hash, saltSize, keySize);

                return Convert.ToBase64String(hash); // Return base64-encoded string
            }
        }

        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            byte[] decodedHash = Convert.FromBase64String(hashedPassword);

            int saltSize = 16; // 128 bits
            int keySize = 32;  // 256 bits

            byte[] salt = new byte[saltSize];
            Buffer.BlockCopy(decodedHash, 0, salt, 0, saltSize);

            byte[] expectedSubkey = new byte[keySize];
            Buffer.BlockCopy(decodedHash, saltSize, expectedSubkey, 0, keySize);

            using (var deriveBytes = new Rfc2898DeriveBytes(providedPassword, salt, 10000))
            {
                byte[] actualSubkey = deriveBytes.GetBytes(keySize); // Derive the key from provided password

                return actualSubkey.SequenceEqual(expectedSubkey); // Compare keys
            }
        }
    }
}