using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WPFPasswordManager
{
    internal static class Encryption
    {
        private const int saltSize = 16;                                         // Salt size in bytes   
        private const int keySize = 32;                                          // Key size in bytes for hashing
        private const int iterations = 100000;                                   // Iterations for key hashing

        static private byte[] hKey = null!;                                      // Key

        // All Updated from KeyFileManager.ReadFromFile(); if encrypted file exists
        static public byte[] passwordSalt = null!;
        static public byte[] passwordIV = null!;    
        static public byte[] EncryptedTest = null!;                              // Test for decryption
        static public List<byte[]> multiEncryptedData = new List<byte[]>();      // Encrypted data
        static public List<int> chunkLengths = new List<int>();                  // Store original lengths of data in byte format

        /**
         * Hashes key from the given password
         */
        public static void HashPassword(string Password)
        {
            byte[] bytePassword = Encoding.UTF8.GetBytes(Password);
            hKey = Rfc2898DeriveBytes.Pbkdf2(bytePassword, passwordSalt, iterations, HashAlgorithmName.SHA512, keySize);
        }

        /**
         * Creates a newly hashed key from the given password
         */
        public static void HashNewPassword(string newPassword)
        {
            byte[] byteNewPassword = Encoding.UTF8.GetBytes(newPassword);
            passwordSalt = new byte[saltSize];

            using (RandomNumberGenerator rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(passwordSalt); // Generates the random salt bytes
            }

            // Hashed PBKDF2 key
            hKey = Rfc2898DeriveBytes.Pbkdf2(byteNewPassword, passwordSalt, iterations, HashAlgorithmName.SHA512, keySize);
        }

        /**
         * Encrypt Test for AES-CBC key verification
         */
        public static byte[] EncryptTest()
        {
            using (Aes aes = Aes.Create())
            {
                byte[] testData = Encoding.UTF8.GetBytes("PASSWORD_VALIDATION");

                aes.Key = hKey;
                aes.IV = passwordIV;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(testData, 0, testData.Length);
                            cs.FlushFinalBlock();
                        }
                    }
                    EncryptedTest = ms.ToArray();
                    return ms.ToArray();
                }
            }
        }
      
        /**
         * Encrypts the user's entry to file
         */
        public static void EncryptFile(List<string> priorData ,string title, string info, bool update)
        {
            byte[] titleToEncrypt = Encoding.UTF8.GetBytes(title);
            byte[] infoToEncrypt = Encoding.UTF8.GetBytes(info);

            // Clear previous data
            multiEncryptedData.Clear();
            chunkLengths.Clear();

            using (Aes aes = Aes.Create())
            {
                aes.Key = hKey;
                passwordIV = aes.IV;

                // Encrypt each piece separately to maintain boundaries
                foreach (string data in priorData)
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    chunkLengths.Add(dataBytes.Length);
                    multiEncryptedData.Add(EncryptBytes(dataBytes, aes));
                }

                // Add title and info
                if (!update)
                {
                    chunkLengths.Add(titleToEncrypt.Length);
                    multiEncryptedData.Add(EncryptBytes(titleToEncrypt, aes));

                    chunkLengths.Add(infoToEncrypt.Length);
                    multiEncryptedData.Add(EncryptBytes(infoToEncrypt, aes));
                }     
            }
            KeyFileManager.WriteToFile();
        }

        /**
         * Helper function for encrypting user's entry
         */
        private static byte[] EncryptBytes(byte[] data, Aes aes)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                        cs.FlushFinalBlock();
                    }
                }
                return ms.ToArray();
            }
        }

        /**
         * Decrypts and updates user entries from file
         */
        public static List<string> DecryptFile() 
        {
            List<string> data = new List<string>();

            using (Aes aes = Aes.Create())
            {
                aes.Key = hKey;
                aes.IV = passwordIV;

                DecryptTest(aes);     // Key check

                for (int i = 0; i < multiEncryptedData.Count; i++)
                {
                    data.Add(DecryptBytes(aes, multiEncryptedData[i]));
                }
            }
            return data;
        }

        /**
         * Helper method for decrypting user entries
         */
        private static string DecryptBytes(Aes aes, byte[] bytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    using (CryptoStream decryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(decryptStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }

            }
        }

        /**
         * Helper method for decrypting encrypted test for password validation
         */
        private static void DecryptTest(Aes aes)
        {
            using (MemoryStream memoryStream = new MemoryStream(EncryptedTest))
            {
                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    using (CryptoStream decryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(decryptStream))
                        {
                            streamReader.ReadToEnd();
                        }
                    }
                }

            }
        }

        /**
         * Removes the given encrypted entry
         */
        public static List<string> RemoveEntryDecryptFile(int index) 
        {
            List<string> modifiedDecryptedData = DecryptFile();
            modifiedDecryptedData.RemoveAt(index * 2 + 1);      // Removes info
            modifiedDecryptedData.RemoveAt(index * 2);          // Removes title
            return modifiedDecryptedData;
        }
    }
}
