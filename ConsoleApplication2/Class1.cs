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


namespace ConsoleApplication2
{
    public class Class1
    {
        public Class1()
        {
            XElement nowy = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
            using (AesManaged aes = new AesManaged())
            {
                Console.WriteLine(Encoding.Default.GetString(Encrypt(nowy.ToString(), aes.Key, aes.IV)));
                byte[] cipher = Encrypt(nowy.ToString(), aes.Key, aes.IV);
                Console.WriteLine(Decrypt(cipher, aes.Key, aes.IV));
                FileStream writeStream = new FileStream("encrypted.xml",FileMode.Create);
                writeStream.Write(cipher, 0, cipher.Length);
                writeStream.Close();


                FileStream readStream = new FileStream("encrypted.xml", FileMode.Open);
                byte [] toDecrypt = new byte [cipher.Length];
                readStream.Read(toDecrypt,0,cipher.Length);
                string decripted = Decrypt(toDecrypt,aes.Key,aes.IV);

                StreamWriter decrypt = new StreamWriter("decrypted.xml",false);
                decrypt.Write(decripted);
                decrypt.Close();

            }

            
        }
        public byte[] StringToByte(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


        static byte[] Encrypt(string text, byte [] Key, byte [] IV)
        {
            if (text == null || text.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            using(AesManaged aesM = new AesManaged())
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
                        encrypted=msEnc.ToArray();
                    }
                }
            }

            return encrypted;
        }


        static string Decrypt(byte [] cipherT, byte [] Key, byte [] IV)
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
    }

}
