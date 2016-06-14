using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for Registry.xaml
    /// </summary>
    public partial class Registry : Page
    {
        public Registry()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Length <= 6)
            {
                MessageBox.Show("Hasło musi mieć conajmniej 6 znaków");
            }
            else
            {
                string hash;
                using (MD5 md5Hash = MD5.Create())
                { 
                    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Password.Password));
                    StringBuilder sBuilder = new StringBuilder();

                    for (int i = 0; i < data.Length; i++)
                    {
                        sBuilder.Append(data[i].ToString("x2"));
                    }
                    hash = sBuilder.ToString();
                }

                
                byte[] cipher = CryptoClass.Instance.Encrypt(hash);
                FileStream writeStream = new FileStream("User", FileMode.Create);
                writeStream.Write(cipher, 0, cipher.Length);
                writeStream.Close();


                foreach (var item in Application.Current.Windows)
                {
                    if (item.GetType() == typeof(Registry_and_Login))
                    {
                        Uri newer = new Uri("Login.xaml", UriKind.Relative);
                        (item as Registry_and_Login).RegistryFrame.Source = newer;
                    }
                }
            }
        }
    }
}
