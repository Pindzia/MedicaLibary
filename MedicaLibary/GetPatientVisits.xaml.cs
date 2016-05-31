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
        public GetPatientVisits(/*string a*/)
        {

            string a = "1";
            InitializeComponent();
            XElement TrackList = XElement.Load(Environment.CurrentDirectory +"\\lib.xml");
            DataGrid.DataContext = TrackList;
            DataGrid.AutoGenerateColumns = false;

            XElement database = XElement.Load(Environment.CurrentDirectory + "\\lib.xml");

            //database.Elements().ToList();

            var result = from c in database.Descendants("patient")
                         select c;
            if (a != "")
            {
                result = result
                    .Where(b => b.Elements("id")
                        .Any(f => (string)f == a));
            }


            //Dane do Wyników
            DataGrid.ItemsSource = result;
            //DataGrid.DataContext = database;
            DataGrid.AutoGenerateColumns = false;
        }
    }
}
