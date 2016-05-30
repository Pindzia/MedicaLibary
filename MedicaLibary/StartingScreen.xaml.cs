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
    /// Interaction logic for StartingScreen.xaml
    /// </summary>
    public partial class StartingScreen : Page
    {
        public StartingScreen()
        {
            InitializeComponent();
            XElement root = XElement.Load(Environment.CurrentDirectory + "\\visits.xml");
            var orderedVisits = root.Elements("visit")
                                    .OrderBy(x => (DateTime)x.Element("data")) //rrrr-mm-dd gg-mm-ss
                                    .ToArray();
            
            DataGrid.ItemsSource = orderedVisits;
            DataGrid.AutoGenerateColumns = false;
        }
    }
}
