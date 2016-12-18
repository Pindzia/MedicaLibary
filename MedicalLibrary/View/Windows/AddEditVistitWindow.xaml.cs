using MedicalLibrary.ViewModel.WindowsViewModel;
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

namespace MedicalLibrary.View.Windows
{
    /// <summary>
    /// Interaction logic for AddEditVistitWindow.xaml
    /// </summary>
    public partial class AddEditVistitWindow : Window
    {
        public AddEditVistitWindow()
        {
            InitializeComponent();
            DataContext = new AddEditVisitViewModel();
        }

        public AddEditVistitWindow( ref AddEditVisitViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
