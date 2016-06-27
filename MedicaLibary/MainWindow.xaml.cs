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
            if (XElementon.Instance.getAccess() == false)
            {
                Registry_and_Login window = new Registry_and_Login();
                window.ShowDialog();
            }
            if(XElementon.Instance.getAccess() == true)
            {
                InitializeComponent();
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                XElementon.Instance.Load();
                RemoteFrame.Source = new Uri("LastChanges.xaml", UriKind.Relative);
            }
            else
            {
                Close();
            }
            
            
        }

        private void addPatient(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("addPatientPage.xaml", UriKind.Relative);
        }

        private void startMe(object sender, RoutedEventArgs e) //placeholder
        {
            RemoteFrame.Source = new Uri("LastChanges.xaml", UriKind.Relative);
        }

        private void getListPatient(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("GetListPage.xaml", UriKind.Relative);
        }

        private void getPatientVisits(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("GetPatientVisits.xaml", UriKind.Relative);
        }

        private void searchEngine(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("SearchEngine.xaml", UriKind.Relative);
        }

        private void seekAndDelete(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("SeekAndDelete.xaml", UriKind.Relative);
        }

        private void editPatient(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("EditPatientPage.xaml", UriKind.Relative);
        }

        private void addVisit(object sender, RoutedEventArgs e)
        {
            RemoteFrame.Source = new Uri("AddVisitPage.xaml", UriKind.Relative);
        }

        private void killApp(object sender, RoutedEventArgs e)
        {
            XElementon.Instance.Save();
            Close();
        }

        
    }
}
