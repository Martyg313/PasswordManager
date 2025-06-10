using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WPFPasswordManager
{
    internal static class KeyFileManager
    {
        private static string fileName = "EncryptedData";                           // File name
        private static string fileFormat = ".txt";                                  // File format
        private static string fileNameFormat = fileName + fileFormat;               // File name format

        private static string mainPath = Directory.GetCurrentDirectory();           // Current directoryof application
        private static string filePath = Path.Combine(mainPath, fileNameFormat);    // Path of txt vault file

        /**
         * Checks for the existence of this file
         */
        public static bool CheckFile()
        {
            Debug.WriteLine($"Path: {filePath}");

            if (!File.Exists(filePath))
            {
                Debug.WriteLine($"Created new EncryptedData file\n");
                FileStream fs = File.Create(filePath); // Creates a filestream on creation which is immediately closed
                fs.Close();
                return false;
            }
            else
            {
                Debug.WriteLine($"EncryptedData file already exists\n");
                return true;
            }
        }

        /**
         * Writes encrypted data to valut file
         */
        public static void WriteToFile() 
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("SALT-" + Convert.ToBase64String(Encryption.passwordSalt));
                sw.WriteLine("IV-" + Convert.ToBase64String(Encryption.passwordIV));

                foreach (byte[] b in Encryption.multiEncryptedData)
                {
                    sw.WriteLine(Convert.ToBase64String(b));
                }
            }
        }

        /**
         * Reads data from vault file for encryption/decryption
         */
        public static void ReadFromFile() 
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = sr.ReadLine();
                Encryption.passwordSalt = Convert.FromBase64String(line.Substring(line.IndexOf('-') + 1));
                line = sr.ReadLine();
                Encryption.passwordIV = Convert.FromBase64String(line.Substring(line.IndexOf('-') + 1));

                Encryption.multiEncryptedData.Clear();

                while (sr.Peek() != -1) 
                {
                    line = sr.ReadLine();
                    Encryption.multiEncryptedData.Add(Convert.FromBase64String(line));  // Title
                    line = sr.ReadLine();
                    Encryption.multiEncryptedData.Add(Convert.FromBase64String(line));  // Info
                }
            }
        }
    }
}
