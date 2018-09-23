using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GUISimpleTCPClient
{
    class Symmetric_Cryptography
    {
        public byte[] sessionkey;
        public byte[] sessioniv;


        public string encryptFile(byte[] file)
        {
            //SymmetricAlgorithm symmetricAlgorithm =    SymmetricAlgorithm.Create();
            //byte[] sessionKey = GenerateKey(symmetricAlgorithm.ToString());
            //symmetricAlgorithm.GenerateKey();
            //symmetricAlgorithm.GenerateIV();


            sessionkey = GenerateKey("Rijndael");

            sessioniv = GenerateIV("Rijndael");
            string encryptedFile = Encrypt(file, sessionkey, sessioniv, "Rijndael");
            return encryptedFile;
        }



        public static string Encrypt(byte[] plainText, string Key, string IV, string algorithmName)
        {
            SymmetricAlgorithm symmetricAlgorithm =

               SymmetricAlgorithm.Create(algorithmName);

            symmetricAlgorithm.Key = Encoding.ASCII.GetBytes(Key);
            symmetricAlgorithm.IV = Encoding.ASCII.GetBytes(IV);
            ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor();

            MemoryStream t = new MemoryStream();

            CryptoStream cryptoStream =

                new CryptoStream(t, transform, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Convert.ToBase64String(t.ToArray());
        }

        public static string Encrypt(byte[] plainText, byte[] Key, byte[] IV, string algorithmName)
        {
            SymmetricAlgorithm symmetricAlgorithm =

               SymmetricAlgorithm.Create(algorithmName);

            symmetricAlgorithm.Key = Key;
            symmetricAlgorithm.IV = IV;
            ICryptoTransform transform = symmetricAlgorithm.CreateEncryptor();

            MemoryStream t = new MemoryStream();

            CryptoStream cryptoStream =

                new CryptoStream(t, transform, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            return Convert.ToBase64String(t.ToArray());
        }

        public static byte[] GenerateKey(string symmetricAlgorithmName)
        {
            SymmetricAlgorithm s = SymmetricAlgorithm.Create(symmetricAlgorithmName);
            s.GenerateKey();
            return s.Key;
        }
        public static byte[] GenerateIV(string symmetricAlgorithmName)
        {
            SymmetricAlgorithm s = SymmetricAlgorithm.Create(symmetricAlgorithmName);
            s.GenerateIV();
            return s.IV;
        }
    }
}
