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
    /// Interaction logic for AddVisit.xaml
    /// </summary>
    public partial class AddVisit : Page
    {
        XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        IEnumerable<XElement> result;

        public AddVisit()
        {
            InitializeComponent();
        }

        private void onInput(object sender, RoutedEventArgs e)
        {
            var Id = ID.Text;
            result = from c in database.Descendants("patient")
                     select c;

            result = result
                .Where(b => b.Elements("id")
                    .Any(f => (string)f == Id));

            var res = result.FirstOrDefault();
            if (res != null)
            {
                Imię.Text = res.Element("imie").Value;
                Nazwisko.Text = res.Element("nazwisko").Value;
                Pesel.Text = res.Element("pesel").Value;
            }
            else
            {
                Imię.Text = "";
                Nazwisko.Text = "";
                Pesel.Text = "";
                DataWizyty.Text = "";
            }
        }

        private void saveToXML(object sender, RoutedEventArgs e)
        {
            //var Id = ID.Text;
            //string Id;
            //open - 'dziury' po wycięciu czegoś innego, 'wolne miejsca', max - maxid+1

            if (true)
            {
                XElement nowy = new XElement(
                new XElement("wizyta",
                new XElement("data_wizyty", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")),
                new XElement("komentarz", Komentarz.Text)
                ));

                result.FirstOrDefault().Add(nowy);
                database.Save(Environment.CurrentDirectory + "\\lib.xml");
                MessageBox.Show("Pomyślnie dodano");
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane");
            }
        }
    }
}



