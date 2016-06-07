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
        XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
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
                a.Remove();
            }
            database.Save(Environment.CurrentDirectory + "\\lib.xml");
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

            SelItem.Element("id").Value = ID.Text;
            SelItem.Element("imie").Value = Imię.Text;
            SelItem.Element("nazwisko").Value = Nazwisko.Text;
            SelItem.Element("pesel").Value = Pesel.Text;
            database.Save(Environment.CurrentDirectory + "\\lib.xml");
            MessageBox.Show("Pomyślnie Edytowano Pacjenta");
        }
    } 
}
