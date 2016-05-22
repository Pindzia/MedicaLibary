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
    /// Interaction logic for AddPatient.xaml
    /// </summary>
    public partial class AddPatient : Page
    {
        public AddPatient()
        {
            InitializeComponent();
        }

        private void saveToXML(object sender, RoutedEventArgs e)
        {
            //var Id = ID.Text;
            string Id;
            var imie = Imię.Text;
            var nazwisko = Nazwisko.Text;
            var pesel = Pesel.Text;

            XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");

            //open - 'dziury' po wycięciu czegoś innego, 'wolne miejsca', max - maxid+1
            var open = database.Descendants("open").FirstOrDefault(); //OrDefault żeby nie rzucał wyjątkami lecz przypisywał nulla w przypadku braku
            if (open != null)
            {
                int nid = Convert.ToInt16(open.Value);
                Id = nid.ToString();
                open.Remove();
                database.Save(Environment.CurrentDirectory + "\\lib.xml");
            }
            else
            {
                var max = database.Descendants("max").First();
                Id = max.Value;
                max.Value = (Convert.ToInt16(Id) + 1).ToString();
                database.Save(Environment.CurrentDirectory + "\\lib.xml");
            }



            if (Id != "" && imie != "" && nazwisko != "" && pesel != "")
            {
                XElement nowy = new XElement(
                new XElement("patient",
                new XElement("id", Id),
                new XElement("imie", imie),
                new XElement("nazwisko", nazwisko),
                new XElement("pesel", pesel)));

                database.Add(nowy);
                database.Save(Environment.CurrentDirectory + "\\lib.xml");
                MessageBox.Show("Pomyślnie Dodano");
            }
            else
            {
                MessageBox.Show("Wszystkie pola są wymagane");
            }
        }
    }
}



