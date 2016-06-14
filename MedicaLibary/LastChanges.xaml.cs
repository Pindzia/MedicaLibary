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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Data;

namespace MedicaLibary
{
    public partial class LastChanges : Page
    {
        XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        
        
        public LastChanges()
        {
            //PatientsFrame.Source = new Uri("GetListPage.xaml", UriKind.Relative);
            InitializeComponent();
            var resultPatients = from c in database.Descendants("patient")
                                    select c;
            var resultVisits = from d in database.Descendants("visit")
                               select d;

            string[] patients = new string [5];// {"1","2","3","4","5"};
            string[] visits = new string[5];//{ "1", "2", "3" };

            var result1 = from c in database.Descendants("patient_changes").Elements() select c;
            var result2 = from c in database.Descendants("visit_changes").Elements() select c;

            //foreach jeśli będziemy mieli zmienną listę zmian jednak.
            for (int i = 0; i < 5; i++)
            {
                patients[i] = result1.ToArray()[i].Value;
            }
            for (int i = 0; i < 3; i++)
            {
                visits[i] = result2.ToArray()[i].Value;
            }

            resultPatients = resultPatients.Where(b => b.Elements("id").Any(f => patients.Contains((string)f)));
            resultVisits = resultVisits.Where(b => b.Elements("idv").Any(f => visits.Contains((string)f)));
            
            DataGridPatients.DataContext = resultPatients;
            DataGridPatients.AutoGenerateColumns = false;
            DataGridVisits.DataContext = resultVisits;
            DataGridVisits.AutoGenerateColumns = false;
        }

        private void editPatient(object sender, RoutedEventArgs e)
        { /*
            foreach (var item in Application.Current.Windows)
            {
                if (item.GetType() == typeof(MainWindow))
                {
                    Uri newer = new Uri("EditSinglePatient.xaml", UriKind.Relative);
                    (item as MainWindow).RemoteFrame.Source = newer;
                }
            }
            for (int i = DataGridPatients.SelectedItems.Count - 1; i >= 0; i--)
            {
                var a = (XElement)DataGridPatients.SelectedItems[0];
                a.Remove();
                MessageBox.Show("Usuwanie?");
            }*/
        }

        private void editVisit(object sender, RoutedEventArgs e)
        {

        }
    }
}
