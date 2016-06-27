using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
            Password.Focus();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (!pwdCorr)
            {
                MessageBox.Show("Nie spełnia podanych wymagań");
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

                CryptoClass.Instance.set_KeyIV(hash);

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

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Regex firstReg = new Regex(@"([a-zA-Z0-9]+){7,16}");
            Match firstMatch = firstReg.Match(Password.Password);

            bool one = false ,two = false, three = false;
            if (firstMatch.Success)
            {
                first.Source = new BitmapImage(new Uri("ok.jpg", UriKind.Relative));
                one = true;
            }
            else
            {
                first.Source = new BitmapImage(new Uri("notok.jpg", UriKind.Relative));
            }

            Regex secondLowerReg = new Regex(@".*[a-z].*");
            Match secondLowerMatch = secondLowerReg.Match(Password.Password);
            Regex secondUpperReg = new Regex(@".*[A-Z].*");
            Match secondUpperMatch = secondUpperReg.Match(Password.Password);
            if (secondLowerMatch.Success && secondUpperMatch.Success)
            {
                second.Source = new BitmapImage(new Uri("ok.jpg", UriKind.Relative));
                two = true;
            }
            else
            {
                second.Source = new BitmapImage(new Uri("notok.jpg", UriKind.Relative));
            }

            Regex thirdReg = new Regex(@".*[0-9].*");
            Match thirdMatch = thirdReg.Match(Password.Password);
            if (thirdMatch.Success)
            {
                third.Source = new BitmapImage(new Uri("ok.jpg", UriKind.Relative));
                three = true;
            }
            else
            {
                third.Source = new BitmapImage(new Uri("notok.jpg", UriKind.Relative));
            }

            if(one && two && three)
            {
                pwdCorr = true;
            }
            else
            {
                pwdCorr = false;
            }
        }

        bool pwdCorr = false;
    }
}
