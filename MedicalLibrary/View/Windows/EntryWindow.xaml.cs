using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedicalLibrary.View.Windows
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public EntryWindow()
        {
            InitializeComponent();
        }

        public static EntryWindow window;

        public static void Complete()
        {
            window.DialogResult = true;
            window.Close();
        }

        public static void NavigateTo( string whereTo)
        {
            if(whereTo == "Register")
            {
                window.ModernTab.SelectedSource = window.ModernTab.Links[0].Source;
                window.MaxHeight = 550;
                window.MaxWidth = 500;
                window.Height = 550;
                window.Width = 500;
            }
            
            if(whereTo == "Login")
            {
                window.ModernTab.SelectedSource = window.ModernTab.Links[1].Source;
                window.MaxHeight = 350;
                window.MaxWidth = 340;
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window = this;
        }
    }
}
