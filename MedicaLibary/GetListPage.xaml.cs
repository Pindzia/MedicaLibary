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

namespace MedicaLibary
{
    /// <summary>
    /// Interaction logic for GetListPage.xaml
    /// </summary>
    public partial class GetListPage : Page
    {
        //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        XElement database = XElementon.Instance.getDatabase();
        XElement SelItem;

        public GetListPage()
        {
            InitializeComponent();
            DataGrid.DataContext = database;
            DataGrid.AutoGenerateColumns = false;
        }



        private void DeletePatient(object sender, RoutedEventArgs e)
        {
            for(int i = DataGrid.SelectedItems.Count - 1; i >= 0; i--)
            {
                var a = (XElement)DataGrid.SelectedItems[0];

               XElement data = new XElement(a);
               data.Name = "data";

                XElement modification = new XElement("modification",
                new XElement("operation", "D"),
                new XElement("node_type", "patient"),
                new XElement("id", a.Element("id").Value),
                new XElement(data)
                );
                database.Descendants("modifications").First().Add(modification);
                while (database.Element("meta").Element("modifications").Elements("modifications").Count() > 5)
                    database.Element("meta").Element("modifications").Elements("modifications").First().Remove();

                a.Remove();

            }
            //database.Save(Environment.CurrentDirectory + "\\lib.xml");



        }

        private void EditPatient(object sender, RoutedEventArgs e)
        {
            list.Visibility = Visibility.Hidden;
            parameters.Visibility = Visibility.Visible;

            SelItem = (XElement)DataGrid.SelectedItem;

            ID.Text = SelItem.Element("id").Value;
            Imię.Text = SelItem.Element("imie").Value;
            Nazwisko.Text = SelItem.Element("nazwisko").Value;
            Pesel.Text= SelItem.Element("pesel").Value;
        }


        private void EditPatientConfirm(object sender, RoutedEventArgs e)
        {
            parameters.Visibility = Visibility.Hidden;


            XElement data = new XElement(SelItem);
            data.Descendants("visit").Remove(); //usuwam węzły <visit> od dodawanego pacjenta, wizyty podczas edycji są zapamiętywane osobno
            data.Name = "data";

            XElement modification = new XElement("modification",
            new XElement("operation", "E"),
            new XElement("node_type", "patient"),
            new XElement("id", SelItem.Element("id").Value),
            new XElement(data)
            );
            database.Descendants("modifications").First().Add(modification);
            while (database.Element("meta").Element("modifications").Elements("modifications").Count() > 5)
                database.Element("meta").Element("modifications").Elements("modifications").First().Remove();

            SelItem.Element("id").Value = ID.Text;
            SelItem.Element("imie").Value = Imię.Text;
            SelItem.Element("nazwisko").Value = Nazwisko.Text;
            SelItem.Element("pesel").Value = Pesel.Text;
            //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            MessageBox.Show("Pomyślnie Edytowano Pacjenta");


            XElement patient_change = new XElement("id", SelItem.Element("id").Value);
            database.Descendants("patient_changes").First().Add(patient_change);
            while (database.Element("meta").Element("patient_changes").Elements("id").Count() > 5)
                database.Element("meta").Element("patient_changes").Elements("id").First().Remove();
            MessageBox.Show("Pomyślnie Edytowano Pacjenta");
        }
    } 
}
