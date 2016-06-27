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
        XElement database = XElementon.Instance.getDatabase();
        //XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");
        IEnumerable<XElement> result;
        string time;

        public AddVisit()
        {
            InitializeComponent();
        }

        private void onInput(object sender, RoutedEventArgs e)
        {
            time = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

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
                DataWizyty.Text = time;
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

            var max= database.Descendants("max_idv").First();
            var idv = max.Value;
            max.Value = (Convert.ToInt16(max.Value) + 1).ToString();
            //database.Save(Environment.CurrentDirectory + "\\lib.xml");

            if (Imię.Text!="") //Imie autouzupełnia gdy pacjent o podanym ID istnieje, i zamienia na puste gdy nie istnieje
            {
                XElement nowy = new XElement(
                new XElement("visit",
                new XElement("idv", idv),
                new XElement("visit_addition_date", time),
                new XElement("comment", Komentarz.Text)
                ));

                result.FirstOrDefault().Add(nowy);
                //database.Save(Environment.CurrentDirectory + "\\lib.xml");
                MessageBox.Show("Pomyślnie dodano");
                addgrid.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane");
            }
        }
    }
}



