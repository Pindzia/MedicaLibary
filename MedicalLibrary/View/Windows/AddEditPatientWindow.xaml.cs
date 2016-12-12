using System.Windows;
using MedicalLibrary.ViewModel.WindowsViewModel;

namespace MedicalLibrary.View.Windows
{
    /// <summary>
    /// Interaction logic for AddEditPatientWindow.xaml
    /// </summary>
    public partial class AddEditPatientWindow : Window
    {
        public AddEditPatientWindow( ref AddEditPatientViewModel ViewModel)
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
