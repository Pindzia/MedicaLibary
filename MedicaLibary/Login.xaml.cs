using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Data.SqlClient;
using System.Windows.Shapes;
using System.IO;
using System.Security.Cryptography;

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            string current;
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Password.Password));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                current = sBuilder.ToString();
            }

            string compare;


            FileStream file = new FileStream("User", FileMode.Open, FileAccess.Read);
            int length = (int)file.Length;

            byte[] byteCompare = new byte[length];
            file.Read(byteCompare, 0, length);
            file.Close();
            compare = CryptoClass.Instance.Decrypt(byteCompare);


            if (current == compare)
            {
                MessageBox.Show("Pomyślnie zalogowano");
                XElementon.Instance.setAccess();
                foreach (var item in Application.Current.Windows)
                {
                    if (item.GetType() == typeof(Registry_and_Login))
                    {
                        (item as Registry_and_Login).Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Hasło jest nieprawidłowe");
            }

        }
    }
}
