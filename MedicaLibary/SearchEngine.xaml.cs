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
    /// <summary>
    /// Potrzebna nowa klasa żeby serializować to co wyszukamy: http://xmltocsharp.azurewebsites.net/
    /// XmlSerializer
    /// jako token ta nowa klasa, przykład z josnem: https://github.com/ProgramerPanda/In-Prot
    /// </summary>
    public partial class SearchEngine : Page
    {
        public SearchEngine()
        {
            InitializeComponent();
            }

        

        private void searchInXML(object sender, RoutedEventArgs e)
        {
            var Id = ID.Text;
            var imie = Imię.Text;
            var nazwisko = Nazwisko.Text;
            var pesel = Pesel.Text;

            XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");

            database.Elements().ToList();
            
            //var found = from c in database.Descendants("lib") select a;

                    /*
                    Patient token = new Patient();
                    XmlSerializer serek = new XmlSerializer(typeof(DataSet));
                    token = serek.Deserialize<Patient>(found);
                    */

            var result = from c in database.Descendants("patient")
                                 select c;
            if (Id != "")
            {
                result = result
                    .Where(b => b.Elements("id")
                        .Any(f => (string)f == Id));
            }

            if (imie != "")
            {
                result = result
                    .Where(b => b.Elements("imie")
                        .Any(f => (string)f == imie));
            }

            if (nazwisko != "")
            {
                result = result
                    .Where(b => b.Elements("nazwisko")
                        .Any(f => (string)f == nazwisko));
            }

            if (pesel != "")
            {
                result = result
                    .Where(b => b.Elements("pesel")
                        .Any(f => (string)f == pesel));
            }
                    

                    //Dane do Wyników
                    DataGrid.ItemsSource = result;
                    //DataGrid.DataContext = database;
                    DataGrid.AutoGenerateColumns = false;
                    //Zamiana Grida
                    results.Visibility = Visibility.Visible;
                    parameters.Visibility = Visibility.Hidden;
        }

        private void ShowPatient(object sender, RoutedEventArgs e)
        {

        }
    }
}