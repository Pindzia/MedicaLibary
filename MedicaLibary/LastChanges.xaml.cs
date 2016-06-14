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
    public partial class LastChanges : Page
    {
        //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        //IEnumerable<XElement> result;
        
        public LastChanges()
        {
            InitializeComponent();
            //PatientsFrame.Source = new Uri("GetListPage.xaml", UriKind.Relative);
        }

        private void edit(object sender, RoutedEventArgs e)
        { /*
            foreach (var item in Application.Current.Windows)
            {
                if (item.GetType() == typeof(MainWindow))
                {
                    Uri newer = new Uri("EditSinglePatient.xaml", UriKind.Relative);
                    (item as MainWindow).RemoteFrame.Source = newer;
                }
            }*/
        }
    }
}
