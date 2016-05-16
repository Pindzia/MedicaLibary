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

            IEnumerable<XElement> found = null;

            //var found = database;


            /*
            XDocument found = new XDocument(
                new XElement("patient",
                new XElement("id", 0),
                new XElement("imie", "Imie"),
                new XElement("nazwisko", "Nazwisko"),
                new XElement("pesel","00000000000")));

            //Jesli któreś jest wypełnione */
            
            if (Id != "" || imie != "" || nazwisko != "" || pesel != "")
            {
                
                if (((imie != "" && nazwisko == "") || (imie == "" && nazwisko != "")) && Id == "" && pesel == "")
                {
                    MessageBox.Show("Jeśli chcesz szukać po imieniu i nazwisku musisz podać oba");
                } else if (pesel !="")
                {
                    //Nie działa
                    found =
                        (from elem in database.Elements()
                        where elem.Attribute("pesel").Value == pesel
                        select elem);
                } else if (Id !="")
                {
                    found =
                        (from elem in database.Element("patient").Elements("patient")
                         where elem.Attribute("id").Value == Id
                         select elem).ToList();

                    

                    if (found != null)
                    {
                        MessageBox.Show("Znalazłem id\n" + found);
                    }
                } else if (imie !="" && nazwisko !="")
                {
                    found =
                        from elem in database.Elements()
                        where (string)elem.Attribute("imie") == imie
                        && (string)elem.Attribute("nazwisko") == nazwisko
                        select elem;
                }
                //*/
                
                if (found == null)
                {
                    MessageBox.Show("Brak wyników wyszukiwania");
                }
                else
                {
                    /*
                    Patient token = new Patient();
                    XmlSerializer serek = new XmlSerializer(typeof(DataSet));
                    token = serek.Deserialize<Patient>(found);
                    */
                    

                    MessageBox.Show(found.ToString());
                    MessageBox.Show(database.ToString());
                    //Dane do Wyników
                    DataGrid.ItemsSource = found; //Nie działa ani na XElement ani na XElement.Elements()
                    //DataGrid.DataContext = database;
                    DataGrid.AutoGenerateColumns = false;
                    //Zamiana Grida
                    results.Visibility = Visibility.Visible;
                    parameters.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("Uzupełnij przynamniej jedno pole");
            }
        }

        private void ShowPatient(object sender, RoutedEventArgs e)
        {

        }
    }
}