using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using System.IO;

namespace RSAExample
{
    class AsymmetricEncryptionUtility
    {
        public byte[] publicKey(string key, string targetFile)
        {
            AsymmetricEncryptionUtility rsa = new AsymmetricEncryptionUtility();
            String publicKey = rsa.GenerateKey(targetFile);
            byte[] cipher = rsa.EncryptData(key, publicKey);
            // Return the public key
            return cipher;

        }

        public string GenerateKey(string targetFile)
        {
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            // Save the private key
            string CompleteKey = Algorithm.ToXmlString(true);
            byte[] KeyBytes = Encoding.UTF8.GetBytes(CompleteKey);
            /* KeyBytes = ProtectedData.Protect(KeyBytes,
             null, DataProtectionScope.LocalMachine);*/

            using (FileStream fs = new FileStream(targetFile, FileMode.Open))
            {
                string pk = Algorithm.ToXmlString(false);
                byte[] d = Encoding.UTF8.GetBytes(pk);
                fs.Write(d, 0, d.Length);
                fs.Close();
            }
            // Return the public key
            return Algorithm.ToXmlString(false);
        }

        public byte[] EncryptData(string data, string publicKey)
        {
            // Create the algorithm based on the public key
            RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
            Algorithm.FromXmlString(publicKey);
            //ReadKey(Algorithm, publicKey);
            // Now encrypt the data
            return Algorithm.Encrypt(
            Encoding.UTF8.GetBytes(data), true);
        }

        private void ReadKey(RSACryptoServiceProvider algorithm, string keyFile)
        {
            byte[] KeyBytes;
            FileStream fs = new FileStream(keyFile, FileMode.Open);

            KeyBytes = new byte[fs.Length];
            fs.Read(KeyBytes, 0, (int)fs.Length);

            /*  KeyBytes = ProtectedData.Unprotect(KeyBytes,
                null, DataProtectionScope.LocalMachine);*/
            fs.Close();
            algorithm.FromXmlString(Encoding.UTF8.GetString(KeyBytes));
        }
    }
}
