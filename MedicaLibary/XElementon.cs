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

        public void Load()
        {
            CryptoClass crypt = CryptoClass.Instance;
            if (File.Exists("encrypted.xml"))
            {
                setDatabase(crypt.Decrypt(LoadEncrypted(crypt)));
            }
            else
            {
                LoadRaw();
                Save();

                //test
                Load();
            }
        }

        public void LoadRaw()
        {
            FileStream file = new FileStream("lib.xml", FileMode.Open, FileAccess.Read);
            int length = (int)file.Length;

            byte[] buffer = new byte[length];
            file.Read(buffer, 0, length);
            file.Close();

            string test = "";

            test = test + Encoding.UTF8.GetString(buffer);

            setDatabase(buffer);
        }

        public void Save()
        {
            CryptoClass crypt = CryptoClass.Instance;
            SaveEncrypted(crypt.Encrypt(this.getDatabase().ToString()));
        }

        public void setDatabase(byte[] xml)
        {
            XDocument document = XDocument.Load(new MemoryStream(xml));
            database = document.Root;
        }

        public void setDatabase(string xml)
        {
            XDocument document = XDocument.Parse(xml);
            database = document.Root;
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

        private byte [] LoadEncrypted (CryptoClass cryptor)
        {
            FileStream file = new FileStream("encrypted.xml", FileMode.Open, FileAccess.Read);
            int length = (int)file.Length;

            byte[] buffer = new byte[length];
            file.Read(buffer, 0, length);
            file.Close();

            return buffer;
        }

        private void SaveEncrypted(byte [] cipher)
        {
            FileStream writeStream = new FileStream("encrypted.xml", FileMode.Create);
            writeStream.Write(cipher, 0, cipher.Length);
            writeStream.Close();
        }

        private XElementon() { }

        private XElement database = null;

    }
}
