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
    public partial class SeekAndDelete : Page
    {
        public SeekAndDelete()
        {
            InitializeComponent();
        }

        TaskCompletionSource<bool> _tcs;
        private bool yesClicked = false;
        private bool noClicked = false;
        
        private async void seekAndDestroy(object sender, RoutedEventArgs e)
        {
            var Id = ID.Text;
            var imie = Imię.Text;
            var nazwisko = Nazwisko.Text;
            var pesel = Pesel.Text;

            XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");

            database.Elements().ToList();

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
            
            DataGrid.ItemsSource = result;
            DataGrid.AutoGenerateColumns = false;
            results.Visibility = Visibility.Visible;
            parameters.Visibility = Visibility.Hidden;

            _tcs = new TaskCompletionSource<bool>();
            await _tcs.Task;

            if (yesClicked)
            {

                foreach (var c in result.Elements("id"))
                {
                    XElement nowy = new XElement("open", c.Value);
                    database.Add(nowy);
                }

                result.Remove();

                database.Save(Environment.CurrentDirectory + "\\lib.xml");
                MessageBox.Show("Usunięto");
                results.Visibility = Visibility.Hidden;
            }
            else if (noClicked)
            {
                MessageBox.Show("Nic nie usunięto, powracanie do ekranu startowego");
                // Brzydko
                results.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Niestety program natrafił na błąd, przepraszamy :(");
                return;
            }
        }

        private void Yes(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult(false);
            yesClicked = true;
        }

        private void No(object sender, RoutedEventArgs e)
        {
            _tcs.SetResult(false);
            noClicked = true;
        }
    }
}
