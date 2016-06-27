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
    /// Interaction logic for GetPatientVisits.xaml
    /// </summary>
    public partial class GetPatientVisits : Page
    {
        XElement database = XElementon.Instance.getDatabase();
        //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");

        public GetPatientVisits(/*string a*/)
        {

            InitializeComponent();
            
            DataGrid.DataContext = database;
            DataGrid.AutoGenerateColumns = false;

            //database.Elements().ToList();

            //var result = from c in database.Descendants("visit")
            //             select c;


            ////Dane do Wyników
            //DataGrid.ItemsSource = result;
            ////DataGrid.DataContext = database;
            //DataGrid.AutoGenerateColumns = false;
        }

        private void DeleteVisit(object sender, RoutedEventArgs e)
        {
            for (int i = DataGrid.SelectedItems.Count - 1; i >= 0; i--)
            {
                var a = (XElement)DataGrid.SelectedItems[i]; //Tutaj musi być [i], a w GetListPage musi być [0]? !!! Tutaj nie odświerza nam się datagrid na bieżąco pomimo +/- identycznego kodu? !!!
                a.Remove();
            }
            //DataGrid.Items.Refresh(); //dziwno - i nie działa. Lepsze bindingi?

            //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            list.Visibility = Visibility.Hidden; //nie wiem jak dobrze refreshować liste więc ją ukrywam xd
        }

        private void EditVisit(object sender, RoutedEventArgs e)
        {
            list.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Visible;

            ID.Text = ((XElement)DataGrid.SelectedItem).Parent.Element("id").Value;
            Imię.Text = ((XElement)DataGrid.SelectedItem).Parent.Element("imie").Value;
            Nazwisko.Text = ((XElement)DataGrid.SelectedItem).Parent.Element("nazwisko").Value;
            Pesel.Text = ((XElement)DataGrid.SelectedItem).Parent.Element("pesel").Value;
            IDWizyty.Text = ((XElement)DataGrid.SelectedItem).Element("idv").Value;
            DataWizyty.Text = ((XElement)DataGrid.SelectedItem).Element("visit_addition_date").Value;
            Komentarz.Text = ((XElement)DataGrid.SelectedItem).Element("comment").Value;
        }

        private void EditVisitConfirm(object sender, RoutedEventArgs e)
        {
            edit.Visibility = Visibility.Hidden;

            ((XElement)DataGrid.SelectedItem).Element("comment").Value = Komentarz.Text;
            //database.Save(Environment.CurrentDirectory + "\\lib.xml");
            MessageBox.Show("Pomyślnie Edytowano Wizytę");
        }

    }
}
