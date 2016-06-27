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

using System.Globalization; //CultureInfo
using System.Windows.Markup; //MarkupExtensions

namespace converters
{ 
    //[ValueConversion(typeof(String), typeof(String))]
public class AEDConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string)value == "A")
            return "Dodanie";
        if ((string)value == "E")
            return "Edycja";
        if ((string)value == "D")
            return "Usunięcie";
        return "Error Konwersji AED";
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if ((string)value == "Dodanie")
            return "A";
        if ((string)value == "Edycja")
            return "E";
        if ((string)value == "Usunięcie")
            return "D";
        return "Error Konwersji Wstecznej AED";
    }
}

    public class PVConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "patient")
                return "Pacjenta";
            if ((string)value == "visit")
                return "Wizyty";
            return "Error Konwersji PV";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "Pacjenta")
                return "patient";
            if ((string)value == "Wizyty")
                return "visit";
            return "Error Konwersji Wstecznej PV";
        }
    }
}






namespace MedicaLibary
{
    public partial class LastChanges : Page
    {
        //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        XElement database = XElementon.Instance.getDatabase();
        
        public LastChanges()
        {
            InitializeComponent();

            DataGridChangelog.DataContext = database;
            DataGridChangelog.AutoGenerateColumns = false;
            //PatientsFrame.Source = new Uri("GetListPage.xaml", UriKind.Relative);
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

        private void Revert(object sender, RoutedEventArgs e)
        {
            

            //'this' z DataGrida
            XElement selected = (XElement)DataGridChangelog.SelectedItem;

            var operation = selected.Element("operation").Value;
            var node_type = selected.Element("node_type").Value;
            var Id = selected.Element("id").Value;

            if (operation=="A" && node_type == "patient")
            {
               
                var patient = from c in database.Descendants("patient")
                             select c;
                if (Id != "")
                {
                    patient = patient
                        .Where(b => b.Elements("id")
                            .Any(f => (string)f == Id));
                }

                patient.Remove();

                selected.Remove();
                //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            }

            if (operation == "E" && node_type == "patient")
            {
                var patient = from c in database.Descendants("patient")
                              select c;
                if (Id != "")
                {
                    patient = patient
                        .Where(b => b.Elements("id")
                            .Any(f => (string)f == Id));
                }

                foreach(var b in selected.Element("data").Elements()){
                    patient.First().Element(b.Name).Value = b.Value;
                }

                selected.Remove();
                //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            }

            if (operation == "D" && node_type == "patient")
            {

                //nowy = this.data;
                XElement nowy = selected.Element("data");
                nowy.Name = "patient";
                

                XElement patient_change = new XElement("id", (string)nowy.Element("id").Value);
                database.Descendants("patient_changes").First().Add(patient_change);
                if (database.Element("meta").Element("patient_changes").Elements("id").Count() > 5)
                    database.Element("meta").Element("patient_changes").Elements("id").First().Remove();


                selected.Remove();
                nowy.Name = "patient"; //z struktury nazwanej data do struktury nazwanej patient
                database.Add(nowy);
                //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            }
        }

    }
}
