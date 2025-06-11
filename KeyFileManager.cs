using System.IO;

namespace WPFPasswordManager
{
    internal static class KeyFileManager
    {
        private static string fileName = "EncryptedData";                           // File name
        private static string fileFormat = ".txt";                                  // File format
        private static string fileNameFormat = fileName + fileFormat;               // File name format

        private static string mainPath = Directory.GetCurrentDirectory();           // Current directory of application
        private static string filePath = Path.Combine(mainPath, fileNameFormat);    // Path of txt vault file

        /**
         * Checks for the existence of this file
         */
        public static bool CheckFile()
        {
            if (!File.Exists(filePath))
            {
                
                return false;
            }
            else
            {
                return true;
            }
        }

        /**
         * Creates vault file
         */
        public static void MakeFile()
        {
            FileStream fs = File.Create(filePath);
            fs.Close();
        }

        /**
         * Writes encrypted data to vault file
         */
        public static void WriteToFile() 
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine("VALIDATION-" + Convert.ToBase64String(Encryption.EncryptTest()));
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
                Encryption.EncryptedTest = Convert.FromBase64String(line.Substring(line.IndexOf('-') + 1));
                line = sr.ReadLine();
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
