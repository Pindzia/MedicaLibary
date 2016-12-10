using System.Windows.Controls;
using MedicalLibrary.ViewModel.WindowsViewModel;

namespace MedicalLibrary
{
    /// <summary>
    /// Interaction logic for PatientPage.xaml
    /// </summary>
    public partial class PatientPage : Page
    {
        public PatientPage()
        {
            InitializeComponent();
            this.DataContext = new PatientPageViewModel();
        }
    }
}
