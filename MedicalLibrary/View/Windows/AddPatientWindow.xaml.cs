using System.Windows;
using MedicalLibrary.ViewModel.PatientPageViewModel;

namespace MedicalLibrary.View.Windows
{
    /// <summary>
    /// Interaction logic for AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        public AddPatientWindow( ref AddPatientViewModel ViewModel)
        {
            InitializeComponent();
            this.DataContext = ViewModel;
        }
    }
}
