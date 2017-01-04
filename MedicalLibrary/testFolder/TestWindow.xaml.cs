using MedicalLibrary.View.CustomControls;
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
using System.Windows.Shapes;

namespace MedicalLibrary.testFolder
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            listOfControls = new ListBox();
        }

        public ListBox listOfControls { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            List<UserControl> list = new List<UserControl>();
            CheckControl CheckControl1 = new CheckControl();
            CheckControl CheckControl2 = new CheckControl();
            CheckControl CheckControl3 = new CheckControl();
            list.Add(CheckControl1);
            list.Add(CheckControl2);
            list.Add(CheckControl3);
            listOfControls.ItemsSource = list;


            Viewer.Content = listOfControls;
        }
    }
}
