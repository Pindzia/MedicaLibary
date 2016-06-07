using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using System.Xml;
using System.Security.Cryptography;
using System.IO;

namespace MedicaLibary
{
    class CryptoClass
    {
        public static CryptoClass Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CryptoClass();
                }
                return instance;
            }
        }

        private static CryptoClass instance;

        private CryptoClass()
        {
            aesM = new AesManaged();
            loadKey();
            loadIV();
        }

        public byte[] getKey()
        {
            return aesM.Key;
        }

        public byte[] getIV()
        {
            return aesM.IV;
        }

        public AesManaged getCrypt()
        {
            return aesM;
        }

        public byte[] Encrypt(string text)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (aesM.Key == null || aesM.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesM.IV == null || aesM.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            ICryptoTransform encryptor = aesM.CreateEncryptor(aesM.Key, aesM.IV);

            using (MemoryStream msEnc = new MemoryStream())
            {
                using (CryptoStream csEnc = new CryptoStream(msEnc, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEnc = new StreamWriter(csEnc))
                    {
                        swEnc.Write(text);
                    }
                    encrypted = msEnc.ToArray();
                }
            }

            return encrypted;
        }

        public string Decrypt(byte[] cipherT)
        {
            if (cipherT == null || cipherT.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (aesM.Key == null || aesM.Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (aesM.IV == null || aesM.IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string decrypted;
            aesM.Padding = PaddingMode.PKCS7;

            //byte[] decryptedBytes;

            ICryptoTransform decryptor = aesM.CreateDecryptor(aesM.Key, aesM.IV);

            using (MemoryStream msDec = new MemoryStream(cipherT))
            {
                using (CryptoStream csDec = new CryptoStream(msDec, decryptor, CryptoStreamMode.Read))
                {
                    //int length = (int)csDec.Length;
                    //decryptedBytes = new byte[length];
                    //
                    //
                    //csDec.Read(decryptedBytes, 0, length);

                    using (StreamReader swDec = new StreamReader(csDec))
                    {
                        decrypted = swDec.ReadToEnd();
                    }
                }
            }

            return decrypted;
        }


        private void loadKey()
        {
            if (File.Exists("key"))
            {
                FileStream file = new FileStream("key", FileMode.Open, FileAccess.Read);
                int length = (int)file.Length;

                aesM.Key = new byte[length];
                file.Read(aesM.Key, 0, length);
                file.Close();
            }
            else
            {
                FileStream file = new FileStream("key", FileMode.CreateNew);
                file.Write(aesM.Key, 0, aesM.Key.Length);
                file.Close();
            }
        }

        private void loadIV()
        {
            if (File.Exists("IV"))
            {
                FileStream file = new FileStream("IV", FileMode.Open, FileAccess.Read);
                int length = (int)file.Length;

                aesM.IV = new byte[length];
                file.Read(aesM.IV, 0, length);
                file.Close();
            }
            else
            {
                FileStream file = new FileStream("IV", FileMode.CreateNew);
                file.Write(aesM.IV, 0, aesM.IV.Length);
                file.Close();
            }
        }

        private AesManaged aesM;

    }
}
