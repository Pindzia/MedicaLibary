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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addPatient(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("AddPatientPage.xaml", UriKind.Relative);
        }

        private void getListPatient(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("GetListPage.xaml", UriKind.Relative);
        }

        private void searchEngine(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("SearchEngine.xaml", UriKind.Relative);
        }

        private void killApp(object sender, RoutedEventArgs e)
        {
            Close();
        }

        
    }
}
