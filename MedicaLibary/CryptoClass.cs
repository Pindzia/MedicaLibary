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
        public CryptoClass()
        {
            aesM = new AesManaged();    
            this.setKey(aesM);
            this.setIV(aesM);
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

        public static byte[] Encrypt(string text, byte[] Key, byte[] IV)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using (AesManaged aesM = new AesManaged())
            {
                aesM.Key = Key;
                aesM.IV = IV;

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
            }

            return encrypted;
        }

        public static string Decrypt(byte[] cipherT, byte[] Key, byte[] IV)
        {
            if (cipherT == null || cipherT.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string decrypted;

            using (AesManaged aesM = new AesManaged())
            {
                aesM.Key = Key;
                aesM.IV = IV;

                ICryptoTransform decryptor = aesM.CreateDecryptor(aesM.Key, aesM.IV);

                using (MemoryStream msDec = new MemoryStream(cipherT))
                {
                    using (CryptoStream csDec = new CryptoStream(msDec, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swDec = new StreamReader(csDec))
                        {
                            decrypted = swDec.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }


        private void setKey(AesManaged aes)
        {
            FileStream file = new FileStream("key.dll", FileMode.Open);
            StreamReader liczbaKl = new StreamReader("liczbaKl.dll");
            int length = Convert.ToInt32(liczbaKl.ReadToEnd());
            liczbaKl.Close();
            file.Read(aes.Key, 0, length);
            file.Close();
        }

        private void setIV(AesManaged aes)
        {
            FileStream file = new FileStream("IV.dll", FileMode.Open);
            StreamReader liczbaIV = new StreamReader("liczbaIV.dll");
            int length = Convert.ToInt32(liczbaIV.ReadToEnd());
            liczbaIV.Close();
            file.Read(aes.IV, 0, length);
            file.Close();
        }

        private AesManaged aesM;

    }
}
