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
    public sealed class XElementon
    {

        public void Initialize()
        {
            CryptoClass crypt = new CryptoClass();
            this.setDatabase(CryptoClass.Decrypt(this.loadEncrypted(crypt), crypt.getKey(), crypt.getIV()));
        }

        public void Disable()
        {
            CryptoClass crypt = new CryptoClass();
            using (AesManaged aesM = new AesManaged())
            {
                this.exportEncrypted(CryptoClass.Encrypt(this.getDatabase().ToString(), crypt.getKey(), crypt.getIV()));
            }
            
        }



        public void setDatabase (string xml)
        {
            this.database = XElement.Parse(xml);
        }

        public XElement getDatabase()
        {
            return database;
        }

        public static XElementon Instance
        {
            get
            {
                if (instance==null)
                {
                    instance = new XElementon();
                }
                return instance;
            }

        }

        private static XElementon instance;

        private byte [] loadEncrypted (CryptoClass cryptor)
        {
            FileStream file = new FileStream("encrypted.xml", FileMode.Open, FileAccess.Read);

            StreamReader liczbaKrypt = new StreamReader("liczbaKrypt.dll");
            int length = Convert.ToInt32(liczbaKrypt.ReadToEnd());
            liczbaKrypt.Close();

            byte[] buffer = new byte[length];
            file.Read(buffer, 0, length);
            file.Close();

            return buffer;
        }

        private void exportEncrypted(byte [] cipher)
        {
            FileStream writeStream = new FileStream("encrypted.xml", FileMode.Create);
            writeStream.Write(cipher, 0, cipher.Length);
            writeStream.Close();

            StreamWriter liczbaKrypt = new StreamWriter("liczbaKrypt.dll",false);
            liczbaKrypt.Write(cipher.Length);
            liczbaKrypt.Close();
        }


        private XElementon() { }

        private XElement database = null;

    }
}
