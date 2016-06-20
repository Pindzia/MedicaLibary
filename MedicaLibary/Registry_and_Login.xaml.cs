using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for Registry_and_Login.xaml
    /// </summary>
    public partial class Registry_and_Login : Window
    {
        public Registry_and_Login()
        {
            InitializeComponent();
            this.CenterWindowOnScreen();
            if (File.Exists("User"))
            {
                RegistryFrame.Source = new Uri("Login.xaml", UriKind.Relative);
            }
            else
            {
                RegistryFrame.Source = new Uri("Registry.xaml", UriKind.Relative);
            }
        }

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
